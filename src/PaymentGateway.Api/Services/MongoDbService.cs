using Microsoft.Extensions.Options;
using MongoDB.Driver;

using PaymentGateway.Api.Models;
using PaymentGateway.Api.Settings;

namespace PaymentGateway.Api.Services;

public class MongoDbService
{
    private MongoClient client;
    private IMongoDatabase database;
    public MongoDbService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
    }

    public async Task<Currency> GetCurrencies(string code) => await database.GetCollection<Currency>("Currencies").FindAsync(x => x.CC == code).Result;
}