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
            card_number = paymentRequest.CardNumber,
            expiry_date = $"0{paymentRequest.ExpiryMonth.ToString()}/{paymentRequest.ExpiryYear.ToString()}",
            currency = paymentRequest.Currency,
            amount = decimal.ToInt32(paymentRequest.Amount),
            cvv = paymentRequest.CVV,
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
                Status = payment.Status.ToString(),
                Card = payment.CardNumber.Substring(payment.CardNumber.Length - 4),
                Month = payment.ExpiryMonth,
                Year = payment.ExpiryYear,
                Currency = payment.Currency,
                Amount = payment.Amount,
            };
        }

        InvalidResponse invalidResponse = new InvalidResponse {Status = Status.Declined.ToString(), Errors = null};
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
                Status = result.Status.ToString(),
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