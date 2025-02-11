using Models;

namespace Repository.Interface;

public interface IFeedbackRepository
{
    Task<Feedback> CreateFeedbackAsync(Feedback feedback);
    Task<FeedbackImage> AddFeedbackImageAsync(FeedbackImage image);
    Task<List<FeedbackImage>> AddFeedbackImagesAsync(List<FeedbackImage> images);
    Task<List<Feedback>> GetProductFeedbacksAsync(int productId);
    Task<Feedback> GetFeedbackByIdAsync(int feedbackId);
    Task<bool> UpdateFeedbackAsync(Feedback feedback);
    Task<bool> DeleteFeedbackAsync(int feedbackId, int userId);
    Task<bool> DeleteFeedbackImageAsync(int imageId);
}