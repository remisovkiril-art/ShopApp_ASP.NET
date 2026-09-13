using ShopApplication.DTOs.ProductFeedbackDTOs;

namespace ShopApplication.Interfaces.Services;

public interface IProductFeedbackService
{
    Task AddAsync(
        ProductFeedbackCreateDTO dto,
        CancellationToken cancellationToken);
}