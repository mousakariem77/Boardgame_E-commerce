namespace Xanh_Dau.DTO;

public class CreateFeedbackDTO
{
    public int ProductId { get; set; }
    public int? Rating { get; set; }
    public string Comment { get; set; }
    public IFormFile[] Images { get; set; }
}