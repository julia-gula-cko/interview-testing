using System.Text.Json.Serialization;

namespace PaymentGateway.Api.Models;

public class BankRequest
{
    [JsonPropertyName("card_number")]
    public string card_number { get; set; }

    [JsonPropertyName("expiry_date")]
    public string expiry_date { get; set; }

    [JsonPropertyName("currency")]
    public string currency { get; set; }

    [JsonPropertyName("amount")]
    public int amount { get; set; }

    [JsonPropertyName("cvv")]
    public int cvv { get; set; }
}