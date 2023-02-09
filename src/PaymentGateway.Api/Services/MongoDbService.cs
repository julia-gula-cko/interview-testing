using Microsoft.Extensions.Options;

using MongoDB.Driver;

using PaymentGateway.Api.Models;
using PaymentGateway.Api.Settings;

namespace PaymentGateway.Api.Services;

public class MongoDbService : IMongoDbService
{
    private readonly IMongoCollection<Payment> _paymentCollection;

    public MongoDbService(IOptions<MongoDBSettings> mongoDbSettings)
    {
        MongoClient client = new(mongoDbSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _paymentCollection = database.GetCollection<Payment>("Payment");
    }


    public async Task<string> Add(Payment payment)
    {
        await _paymentCollection.InsertOneAsync(payment);
        return payment.Id;
    }

    public Task<Payment> Get(string id)
    {
        throw new NotImplementedException();
    }
}

public interface IMongoDbService
{
    public Task<string> Add(Payment payment);
    public Task<Payment> Get(string id);
}