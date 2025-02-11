namespace Xanh_Dau.DTO;

public class OrderDTO
{
    public string SelectedItemIds { get; set; }
    public bool UseDefaultAddress { get; set; }
    public long SelectedAddressId { get; set; }
    public string? ShippingAddress { get; set; }
    public string? Receiver { get; set; }
    public string? ShipPhone { get; set; }
    public bool SaveAddress { get; set; }
    public string? VoucherCode { get; set; }
    public int? VoucherId { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
}