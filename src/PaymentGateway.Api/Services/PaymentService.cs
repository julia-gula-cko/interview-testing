using PaymentGateway.Api.Models;
using PaymentGateway.Api.Services;

namespace PaymentGateway.Api;

public class PaymentService : IPaymentService
{
    private readonly IMongoDbService _mongoDbService;
    private readonly IBankService _bankService;

    public PaymentService(IMongoDbService mongoDbService, IBankService bankService)
    {
        _mongoDbService = mongoDbService;
        _bankService = bankService;
    }

    public async Task<Response> CreatePayment(PaymentRequest paymentRequest)
    {
        BankRequest bankRequest = new()
        {
            CardNumber = paymentRequest.CardNumber,
            ExpiryDate = $"{paymentRequest.ExpiryMonth.ToString()}/{paymentRequest.ExpiryYear.ToString()}",
            Currency = paymentRequest.Amount.ToString(),
            Amount = AmountConverter(paymentRequest.Amount).ToString(),
            CVV = paymentRequest.CVV,
        };

        BankResponse bankResult = await _bankService.AuthorizePayment(bankRequest);

        if (bankResult.Authorized)
        {
            Payment payment = new()
            {
                Id = null,
                CardNumber = paymentRequest.CardNumber,
                ExpiryMonth = paymentRequest.ExpiryMonth,
                ExpiryYear = paymentRequest.ExpiryYear,
                Status = Status.Authorized,
                Currency = paymentRequest.Currency,
                Amount = AmountConverter(paymentRequest.Amount),
                CVV = paymentRequest.CVV,
            };

            Task<string> id = _mongoDbService.Add(payment);
            return new PaymentResponse
            {
                Status = payment.Status,
                Card = payment.CardNumber.Substring(payment.CardNumber.Length - 4),
                Month = payment.ExpiryMonth,
                Year = payment.ExpiryYear,
                Currency = payment.Currency,
                Amount = payment.Amount,
            };
        }

        InvalidResponse invalidResponse = new InvalidResponse {Status = Status.Declined, Errors = null};
        return invalidResponse;
    }

    public async Task<Response> GetPayment(string id)
    {
        Payment? result = await _mongoDbService.Get(id);
        if (result != null)
        {
            PaymentResponse paymentResponse = new()
            {
                Id = result.Id,
                Status = result.Status,
                Card = result.CardNumber.Substring(result.CardNumber.Length - 4),
                Month = result.ExpiryMonth,
                Year = result.ExpiryYear,
                Currency = result.Currency,
                Amount = result.Amount,
            };
            return paymentResponse;
        }

        return null;
    }

    private decimal AmountConverter(decimal amount)
    {
        return amount;
    }
}

public interface IPaymentService
{
    public Task<Response> CreatePayment(PaymentRequest payment);
    public Task<Response> GetPayment(string id);
}