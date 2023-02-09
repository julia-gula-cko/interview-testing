using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Models;

namespace PaymentGateway.Api.Controllers;

[Controller]
public class PaymentController
{
    private readonly IPaymentService _paymentService;
    private readonly PaymentValidator _paymentValidator;
    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] Payment payment)
    {
        var modelState = await _paymentValidator.ValidateAsync(payment);
        if (!modelState.IsValid)
        {
            return new UnprocessableEntityObjectResult(modelState);
        }

        var result = _paymentService.CreatePayment(payment);
        if (result.Result.Status == Status.Authorized)
        {
            return new OkObjectResult(result);
        }

        return new BadRequestObjectResult(result);

    }
    
    [HttpGet]
    public async Task<IActionResult> GetPayment([FromQuery] string id)
    {
        
        return await _paymentService.GetPayment(id);
    }
}