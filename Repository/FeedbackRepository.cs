using DataAccess.DAOs;
using Models;
using Repository.Interface;

namespace Repository;

public class FeedbackRepository : IFeedbackRepository
{
    private readonly FeedbackDAO _feedbackDAO;

    public FeedbackRepository(FeedbackDAO feedbackDAO)
    {
        _feedbackDAO = feedbackDAO;
    }

    public async Task<Feedback> CreateFeedbackAsync(Feedback feedback)
    {
        return await _feedbackDAO.CreateFeedbackAsync(feedback);
    }

    public async Task<FeedbackImage> AddFeedbackImageAsync(FeedbackImage image)
    {
        return await _feedbackDAO.AddFeedbackImageAsync(image);
    }

    public async Task<List<FeedbackImage>> AddFeedbackImagesAsync(List<FeedbackImage> images)
    {
        return await _feedbackDAO.AddFeedbackImagesAsync(images);
    }

    public async Task<List<Feedback>> GetProductFeedbacksAsync(int productId)
    {
        return await _feedbackDAO.GetProductFeedbacksAsync(productId);
    }

    public async Task<Feedback> GetFeedbackByIdAsync(int feedbackId)
    {
        return await _feedbackDAO.GetFeedbackByIdAsync(feedbackId);
    }

    public async Task<bool> UpdateFeedbackAsync(Feedback feedback)
    {
        return await _feedbackDAO.UpdateFeedbackAsync(feedback);
    }

    public async Task<bool> DeleteFeedbackAsync(int feedbackId, int userId)
    {
        return await _feedbackDAO.DeleteFeedbackAsync(feedbackId, userId);
    }

    public async Task<bool> DeleteFeedbackImageAsync(int imageId)
    {
        return await _feedbackDAO.DeleteFeedbackImageAsync(imageId);
    }
}