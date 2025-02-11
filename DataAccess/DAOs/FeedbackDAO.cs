using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.DAOs;

public class FeedbackDAO : SingletonBase<FeedbackDAO>
{
    public async Task<Feedback> CreateFeedbackAsync(Feedback feedback)
    {
        if (feedback == null) throw new ArgumentNullException(nameof(feedback));

        await _context.Feedbacks.AddAsync(feedback);
        await _context.SaveChangesAsync();
        return feedback;
    }

    public async Task<FeedbackImage> AddFeedbackImageAsync(FeedbackImage image)
    {
        if (image == null) throw new ArgumentNullException(nameof(image));

        await _context.FeedbackImages.AddAsync(image);
        await _context.SaveChangesAsync();
        return image;
    }

    public async Task<List<FeedbackImage>> AddFeedbackImagesAsync(List<FeedbackImage> images)
    {
        if (images == null || !images.Any()) return new List<FeedbackImage>();

        await _context.FeedbackImages.AddRangeAsync(images);
        await _context.SaveChangesAsync();
        return images;
    }

    public async Task<List<Feedback>> GetProductFeedbacksAsync(int productId)
    {
        return await _context.Feedbacks
            .Include(f => f.Customer)
            .Include(f => f.FeedbackImages)
            .Where(f => f.ProductId == productId && !f.IsDelete.Value)
            .OrderByDescending(f => f.CreateAt)
            .ToListAsync();
    }

    public async Task<Feedback> GetFeedbackByIdAsync(int feedbackId)
    {
        return await _context.Feedbacks
            .Include(f => f.FeedbackImages)
            .FirstOrDefaultAsync(f => f.FeedbackId == feedbackId && !f.IsDelete.Value);
    }

    public async Task<bool> UpdateFeedbackAsync(Feedback feedback)
    {
        try
        {
            _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteFeedbackAsync(int feedbackId, int userId)
    {
        try
        {
            var feedback = await GetFeedbackByIdAsync(feedbackId);
            if (feedback == null) return false;

            feedback.IsDelete = true;
            feedback.UpdateAt = DateTime.Now;
            // Soft delete related images
            foreach (var image in feedback.FeedbackImages) image.IsDelete = true;

            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteFeedbackImageAsync(int imageId)
    {
        try
        {
            var image = await _context.FeedbackImages.FindAsync(imageId);
            if (image == null) return false;

            image.IsDelete = true;
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    // public async Task<List<Feedback>> GetAllFeedbacksAsync()
    // {
    //     return await _context.Feedbacks.ToListAsync();
    // }
    //
    // public async Task<Feedback> GetFeedbackByIdAsync(long feedbackId)
    // {
    //     return await _context.Feedbacks.FindAsync(feedbackId);
    // }
    //
    // public async Task<Feedback> CreateFeedbackAsync(Feedback feedback)
    // {
    //     if (feedback == null) throw new ArgumentNullException(nameof(feedback));
    //     await _context.Feedbacks.AddAsync(feedback);
    //     await _context.SaveChangesAsync();
    //     return feedback;
    // }
    //
    // public async Task<Feedback> UpdateFeedbackAsync(Feedback feedback)
    // {
    //     try
    //     {
    //         // Tìm phản hồi hiện tại
    //         var existingFeedback = await _context.Feedbacks.FindAsync(feedback.FeedbackId);
    //         
    //         if (existingFeedback == null)
    //         {
    //             // Nếu không tìm thấy, trả về null
    //             Console.WriteLine($"Feedback with ID {feedback.FeedbackId} not found.");
    //             return null;
    //         }
    //
    //         // Cập nhật các trường cần thiết
    //         existingFeedback.ProductId = feedback.ProductId;                  // Mã sản phẩm
    //         existingFeedback.CustomerId = feedback.CustomerId;                // Mã khách hàng
    //         existingFeedback.FeedbackRate = feedback.FeedbackRate;            // Đánh giá của khách hàng
    //         existingFeedback.FeedbackComment = feedback.FeedbackComment;      // Bình luận của khách hàng
    //         existingFeedback.UpdateBy = feedback.UpdateBy;                    // Người cập nhật
    //         existingFeedback.UpdateAt = DateTime.Now;                          // Cập nhật thời gian
    //
    //         // Lưu thay đổi vào cơ sở dữ liệu
    //         await _context.SaveChangesAsync();
    //
    //         return existingFeedback;
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"Error updating feedback: {ex.Message}");
    //         return null;
    //     }
    // }
    //
    //
    // public async Task DeleteFeedbackAsync(long feedbackId)
    // {
    //     var feedback = await _context.Feedbacks.FindAsync(feedbackId);
    //     if (feedback != null)
    //     {
    //         _context.Feedbacks.Remove(feedback);
    //         await _context.SaveChangesAsync();
    //     }
    // }
}