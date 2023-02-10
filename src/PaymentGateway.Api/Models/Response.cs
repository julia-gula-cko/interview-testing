namespace PaymentGateway.Api.Models;

public class Response
{
    public Status Status { get; set; }
}

public class InvalidResponse : Response
{
    public List<string> Errors { get; set; }
}

public class PaymentResponse : Response
{
    public string Id { get; set; }
    public string Card { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }
}