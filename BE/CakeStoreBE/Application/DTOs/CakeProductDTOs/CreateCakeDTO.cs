namespace CakeStoreBE.Application.DTOs.CakeProductDTOs;

public class CreateCakeDTO{

    // public string CakeId { get; set; }
    public string? CategoryId {get; set;}
    public string? Name {get; set;}
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? StockQuantity { get; set; }
    public DateTime? CreatedAt { get; set; }
}