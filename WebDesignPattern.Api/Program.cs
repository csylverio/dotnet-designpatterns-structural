using LibraryExternal.SAP;
using Microsoft.OpenApi.Models;
using WebDesignPattern.Domain.CustomerRelationshipManagement;
using WebDesignPattern.Domain.InventoryManagement;
using WebDesignPattern.Domain.PurchaseTransaction;
using WebDesignPattern.Domain.PurchaseTransaction.Discount;
using WebDesignPattern.Domain.PurchaseTransaction.Financial;
using WebDesignPattern.Infra.Data;
using WebDesignPattern.Infra.Financial;
using WebDesignPattern.Infra.Financial.PagSeguro;
using WebDesignPattern.Infra.Financial.PayPal;
using WebDesignPattern.Infra.Sap;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "WebDesignPattern API",
        Version = "v1",
        Description = "API exemplos de utilização de Design Patterns",
        Contact = new OpenApiContact { Name = "Carlos Sylverio", Email = "contato@example.com" }
    });
});
builder.Services.AddControllers();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderBuilder, OrderBuilder>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<PayPalApi>();
builder.Services.AddScoped<PagSeguroService>();
builder.Services.AddScoped<IPaymentGatewayFactory, PaymentGatewayFactory>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.Services.AddSingleton<IDiscountConfiguration, DiscountConfiguration>();

builder.Services.AddScoped<IAccountingService, SapAccountingFacade>();
builder.Services.AddScoped<ISapBapiService, SapBapiService>();
builder.Services.AddScoped<ISapIdocService, SapIdocService>();
builder.Services.AddScoped<ISapRfcService, SapRfcService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();