using LibraryExternal.SAP;
using Microsoft.OpenApi.Models;
using WebDesignPattern.Domain.CustomerRelationshipManagement;
using WebDesignPattern.Domain.InventoryManagement;
using WebDesignPattern.Domain.Notification;
using WebDesignPattern.Domain.PurchaseTransaction;
using WebDesignPattern.Domain.PurchaseTransaction.Discount;
using WebDesignPattern.Domain.PurchaseTransaction.Financial;
using WebDesignPattern.Domain.PurchaseTransaction.Observers;
using WebDesignPattern.Domain.PurchaseTransaction.Shippings;
using WebDesignPattern.Infra.Data;
using WebDesignPattern.Infra.Financial;
using WebDesignPattern.Infra.Financial.PagSeguro;
using WebDesignPattern.Infra.Financial.PayPal;
using WebDesignPattern.Infra.Notification;
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

builder.Services.AddTransient<IShippingStrategy, StandardShippingStrategy>();
builder.Services.AddTransient<IShippingStrategy, ExpressShippingStrategy>();
builder.Services.AddTransient<IShippingStrategy, StorePickupStrategy>();
builder.Services.AddSingleton<ShippingServiceContext>();


builder.Services.AddSingleton<IEmailService>(provider => 
{
    var config = provider.GetRequiredService<IConfiguration>();
    var apiKey = config["SendGrid:ApiKey"] ?? throw new ArgumentNullException("SendGrid:ApiKey", "SendGrid API Key is not configured.");
    var fromEmail = config["SendGrid:FromEmail"] ?? throw new ArgumentNullException("SendGrid:FromEmail", "SendGrid FromEmail is not configured.");
    var fromName = config["SendGrid:FromName"] ?? throw new ArgumentNullException("SendGrid:FromName", "SendGrid FromName is not configured.");
    return new SendGridEmailService(
        apiKey,
        fromEmail,
        fromName
    );
});
builder.Services.AddScoped<INotificationManager, FirebaseNotificationManager>();
// Registrar os observers com injeção de dependência
builder.Services.AddScoped<IOrderObserver>(provider => new EmailNotificationService(provider.GetRequiredService<IEmailService>()));
builder.Services.AddScoped<IOrderObserver>(provider => new InventoryManagementService(provider.GetRequiredService<IProductRepository>()));
builder.Services.AddScoped<IOrderObserver>(provider => new MobilePushNotificationService(provider.GetRequiredService<INotificationManager>()));
// Registrar o OrderEventManager e injetar todos os observers
builder.Services.AddScoped<OrderEventManager>(provider =>
{
    var eventManager = new OrderEventManager();
    var observers = provider.GetServices<IOrderObserver>();
    foreach (var observer in observers)
    {
        eventManager.Subscribe(observer);
    }
    return eventManager;
});

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