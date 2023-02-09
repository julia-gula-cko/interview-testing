using PaymentGateway.Api.Models;
using PaymentGateway.Api.Services;

namespace PaymentGateway.Api;

public class PaymentService : IPaymentService
{
    private IMongoDbService _mongoDbService;
    private IBankService _bankService;

    public PaymentService(IMongoDbService mongoDbService, IBankService bankService)
    {
        _mongoDbService = mongoDbService;
        _bankService = bankService;
    }

    public Task<PaymentValidResponse> CreatePayment(Payment payment)
    {
        
    }

    private Task AmountConverter(int amount)
    {
    }
}

public interface IPaymentService
{
    public Task<PaymentValidResponse> CreatePayment(Payment payment);
    public Task<GetResponse> GetPayment(string id);
}