using System.Text;

using MongoDB.Bson;

using Newtonsoft.Json;

using PaymentGateway.Api.Models;
using PaymentGateway.Api.Settings;

namespace PaymentGateway.Api.Services;

public class BankService : IBankService
{
    private readonly HttpClient _client;
    private readonly BankSimulatorSettings _bankSimulatorSettings;
    private readonly Uri _uri;

    public BankService(HttpClient client)
    {
        _client = new HttpClient();
        _uri = new Uri(_bankSimulatorSettings.URL);
    }

    public async Task<BankResponse> AuthorizePayment(BankRequest payment)
    {
        string? json = payment.ToJson();
        StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _client.PostAsync(_uri, data);
        return JsonConvert.DeserializeObject<BankResponse>(response.Content.ReadAsStringAsync().Result);
    }
}

public interface IBankService
{
    public Task<BankResponse> AuthorizePayment(BankRequest payment);
}