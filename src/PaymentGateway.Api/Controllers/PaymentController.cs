using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Models;

namespace PaymentGateway.Api.Controllers;

[Controller]
public class PaymentController:Controller
{
    private readonly IPaymentService _paymentService;
    private readonly PaymentValidator _paymentValidator;
    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
        _paymentValidator = new PaymentValidator();
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest payment)
    {
        var modelState = await _paymentValidator.ValidateAsync(payment);
        if (!modelState.IsValid)
        {
            return new UnprocessableEntityObjectResult(modelState);
        }

        var result = await _paymentService.CreatePayment(payment);
        
        if (result.Status== Status.Authorized)
        {
            return new OkObjectResult(result);
        }

        return new BadRequestObjectResult(result);

    }
    
    [HttpGet(("{id}"))]
    public async Task<IActionResult> GetPayment(string id)
    {
        var result = await _paymentService.GetPayment(id);
        if (result==null)
        {
            return new BadRequestObjectResult("Payment not found");
        }

        return new OkObjectResult(result);
    }
}