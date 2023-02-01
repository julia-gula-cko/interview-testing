using FluentValidation;

namespace PaymentGateway.Api.Models;

public class Payment
{
    public string CardNumber { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public int Currency { get; set; }
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
        RuleFor(x => x.Currency).NotEmpty();
    }
}