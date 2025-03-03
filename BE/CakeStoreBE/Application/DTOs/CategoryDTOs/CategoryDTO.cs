namespace CakeStoreBE.Application.DTOs.CategoryDTOs;

public class CategoryDTO
{
    public string CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateCategoryDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    // public DateTime CreatedAt { get; set; }   
}

public class UpdateCategoryDTO
{
    public string CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime UpdatedAt { get; set; }
}