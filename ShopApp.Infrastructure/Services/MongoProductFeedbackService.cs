using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ShopApplication.DTOs.ProductFeedbackDTOs;
using ShopApplication.Interfaces.Services;
using ShopInfrastructure.Configuration;
using ShopInfrastructure.Models;

namespace ShopInfrastructure.Services;

public class MongoProductFeedbackService : IProductFeedbackService
{
    private readonly IMongoCollection<ProductFeedback> _collection;

    public MongoProductFeedbackService(
        IOptions<MongoDbSettings> options)
    {
        var settings = options.Value;

        var client = new MongoClient(
            settings.ConnectionString);

        var database = client.GetDatabase(
            settings.DatabaseName);

        _collection = database.GetCollection<ProductFeedback>(
            "ProductFeedbacks");
    }

    public async Task AddAsync(
        ProductFeedbackCreateDTO dto,
        CancellationToken cancellationToken)
    {
        var feedback = new ProductFeedback
        {
            ProductId = dto.ProductId,
            Type = dto.Type,
            Message = dto.Message,
            CreatedAt = DateTime.UtcNow
        };

        await _collection.InsertOneAsync(
            feedback,
            cancellationToken: cancellationToken);
    }
}