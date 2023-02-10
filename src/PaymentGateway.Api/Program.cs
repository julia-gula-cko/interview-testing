using PaymentGateway.Api;
using PaymentGateway.Api.Services;
using PaymentGateway.Api.Settings;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.Configure<BankSimulatorSettings>(builder.Configuration.GetSection("BankSimulator"));
builder.Services.AddScoped<IMongoDbService, MongoDbService>();
builder.Services.AddScoped<IBankService, BankService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddHttpClient();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();