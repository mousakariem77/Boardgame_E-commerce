namespace Xanh_Dau.DTO;

public class FeedbackDTO
{
    public int ProductID { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }
    public IFormFile[] Images { get; set; }
}