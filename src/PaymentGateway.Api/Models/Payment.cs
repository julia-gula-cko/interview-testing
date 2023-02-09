using FluentValidation;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaymentGateway.Api.Models;

public class Payment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string CardNumber { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }
    public int CVV { get; set; }
}

public class PaymentValidator : AbstractValidator<Payment>
{
    public PaymentValidator()
    {
        RuleFor(x => x.CardNumber).CreditCard().WithMessage("Invalid credit card").NotEmpty();
        RuleFor(p => p.ExpiryYear)
            .NotEmpty()
            .WithMessage("Credit card expiry year is required")
            .Must(x => Convert.ToInt32(x) >= DateTime.Now.Year)
            .WithMessage("The credit card expiry year is invalid");
        RuleFor(p => p.ExpiryMonth)
            .NotEmpty()
            .WithMessage("Credit card expiry month is required")
            .Must(x => Convert.ToInt32(x) >= DateTime.Now.Month)
            .WithMessage("The credit card expiry month is invalid")
            .When(f => Convert.ToInt32(f.ExpiryYear) == DateTime.Now.Year);
        RuleFor(x => x.Currency).IsEnumName(typeof(Currency), caseSensitive: false);
    }
}