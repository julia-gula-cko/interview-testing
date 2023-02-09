namespace PaymentGateway.Api.Models;

public class PaymentValidResponse : Response
{
    public string Id { get; set; }
    public Status Status { get; set; }
    public int Card { get; set; }
    public int ExpiryMonth { get; set; }
    public int Currency { get; set; }
    public string Amount { get; set; }
}

public class Response
{
    
}

public class InvalidResponse : Response
{
    private List<string> Errors { get; set; }
    private Status Status { get; set; }
}

internal class Error
{
    
}
