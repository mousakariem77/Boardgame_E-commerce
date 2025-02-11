namespace Models;

public class Address
{
    public int AddressId { get; set; }

    public int CustomerId { get; set; }

    public string Receiver { get; set; } = null!;

    public string ShipAddress { get; set; } = null!;

    public string ShipPhone { get; set; } = null!;

    public bool? IsDefault { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool? IsDeleted { get; set; }
}