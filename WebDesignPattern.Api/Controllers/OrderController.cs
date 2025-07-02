using Microsoft.AspNetCore.Mvc;
using WebDesignPattern.Api.DTOs;
using WebDesignPattern.Domain.PurchaseTransaction;
using WebDesignPattern.Domain.PurchaseTransaction.States;

namespace WebDesignPattern.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IOrderBuilder _orderBuilder;

    public OrderController(IOrderService orderService, IOrderBuilder orderBuilder)
    {
        _orderService = orderService;
        _orderBuilder = orderBuilder;
    }

    [HttpPost]
    public IActionResult Create([FromBody] OrderDTO orderDTO)
    {
        IOrderBuilder orderBuilder = _orderBuilder
            .SetCustomerId(orderDTO.CustomerId)
            .SetPaymentMethod(orderDTO.PaymentMethodId)
            .SetShippingMethod(orderDTO.ShippingMethodId)
            .SetDiscount(orderDTO.Discount);

        foreach (var item in orderDTO.Items)
        {
            orderBuilder.AddItem(item.ProductId, item.Quantity);
        }

        Order order = orderBuilder.Build();
        order = _orderService.Create(order);

        return Ok(new
        {
            Id = order.Id,
            TotalAmount = order.TotalAmount
        });
    }

    [HttpPost("finalize-order")]
    public IActionResult FinalizeOrder([FromBody] FinalizeOrderDTO finalizeOrderDTO)
    {
        var order = _orderService.GetById(finalizeOrderDTO.OrderId);
        if (order == null)
            return NotFound("Pedido não encontrado.");

        try
        {
            order = _orderService.FinalizeOrder(order, finalizeOrderDTO.CouponCode);
            return Ok(new
            {
                Id = order.Id,
                BaseDiscount = order.DiscountDetail.BaseDiscount,
                PromotionalDiscount = order.DiscountDetail.PromotionalDiscount,
                FinalDiscount = order.DiscountDetail.FinalDiscount
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPost("make-payment")]
    public IActionResult MakePayment([FromBody] PaymentDTO paymentDTO)
    {
        var order = _orderService.GetById(paymentDTO.OrderId);
        if (order == null)
            return NotFound("Pedido não encontrado.");

        try
        {
            //Força estado para teste (essa informaçaõ deveria vir do banco de dados e não ser manipulada diretamente)  
            order.SetState(new AwaitingPaymentState());


            var paymentResult = _orderService.MakePayment(order, paymentDTO.PaymentMethodId);
            if (paymentResult.Success)
                return Ok(paymentResult); // HTTP 200 com detalhes do pagamento
            else
                return BadRequest(paymentResult); // HTTP 400 com erro
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("ship-order")]
    public IActionResult ShipOrder([FromBody] ShipDTO shipDTO)
    {
        var order = _orderService.GetById(shipDTO.OrderId);
        if (order == null)
            return NotFound("Pedido não encontrado.");

        try
        {
            //Força estado para teste (essa informaçaõ deveria vir do banco de dados e não ser manipulada diretamente)  
            order.SetState(new PaymentApprovedState());

            order = _orderService.Ship(order, shipDTO.ShippingMethodId);
            return Ok(new
            {
                Id = order.Id,
                Carrier= order.ShippingResult.Carrier,
                TrackingNumber = order.ShippingResult.TrackingNumber,
                EstimatedDelivery = order.ShippingResult.EstimatedDelivery
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
