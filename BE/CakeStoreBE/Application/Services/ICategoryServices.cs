using CakeStoreBE.Application.DTOs.CategoryDTOs;
using CakeStoreBE.Domain;
using CakeStoreBE.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CakeStoreBE.Application.Services
{
    public interface ICategoryServices {
        public Task<IActionResult> HandleGetAllCategory();
        public Task<IActionResult> HandleGetCategoryById(string _categoryId);    
        public Task<IActionResult> HandleCreateCategory(CreateCategoryDTO _categoryDTO);
        public Task<IActionResult> HandleUpdateCategory(UpdateCategoryDTO _categoryDTO);
        public Task<IActionResult> HandleDeleteCategory(string _categoryId);
    }

    public class CategoryServices : ControllerBase, ICategoryServices
    {
        private readonly BakeStoreDbContext _context;
        public CategoryServices(BakeStoreDbContext context)
        {
            _context = context;
        } 

        public async Task<IActionResult> HandleGetAllCategory(){

            try{

                var categories = await _context.Categories
                    .ToListAsync();

                return Ok(categories);

            }catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { message = "Database error", error = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred", error = ex.Message });
            }
        }
    public async Task<IActionResult> HandleGetCategoryById(string _categoryId)
    {
            try{

                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.CategoryId == _categoryId);

                if (category is null)
                {
                    return NotFound(new { message = "Category not found" });
                }

                return Ok(category);

            }catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { message = "Database error", error = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred", error = ex.Message });
            }
        }

        public async Task<IActionResult> HandleCreateCategory(CreateCategoryDTO _categoryDTO)
        {
            try{

                var exsitingCategory = _context.Categories.Any(c => c.Name == _categoryDTO.Name);

                if (exsitingCategory){
                    throw new Exception("Category already exits");
                }    

                var lastCategoryId = _context.Categories
                    .Where(c => c.CategoryId.StartsWith("Cate_"))
                    .OrderByDescending(c => c.CategoryId)
                    .FirstOrDefault();

                int nextNumber = 1;
                if (lastCategoryId is not null){
                    string lastId = lastCategoryId.CategoryId.Replace("Cate_", "");
                    if(int.TryParse(lastId, out int lastNumber)){
                        nextNumber = lastNumber + 1;
                    }
                }

                var category = new Category{
                    CategoryId = $"Cate_{nextNumber}",
                    Name = _categoryDTO.Name,
                    Description = _categoryDTO.Description,              
                };

                category.CreatedAt = DateTime.UtcNow;
               
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                return Ok(new {
                    category.CategoryId,
                    category.Name,
                    category.Description,
                    CreatedAt = category.CreatedAt?.ToString("yyyy-MM-dd HH:mm:ss"),
                });

            }catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { message = "Database error", error = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred", error = ex.Message });
            }
        }

        public async Task<IActionResult> HandleUpdateCategory(UpdateCategoryDTO _categoryDTO)
        {
            try{

                var exsitingCategory = _context.Categories
                    .FirstOrDefault(c => c.CategoryId == _categoryDTO.CategoryId);

                if (exsitingCategory is null){
                    return NotFound(new { message = "Category not found" });
                }   

                exsitingCategory.Name = _categoryDTO.Name;
                exsitingCategory.Description = _categoryDTO.Description;
                exsitingCategory.UpdatedAt = DateTime.UtcNow;

                _context.Categories.Update(exsitingCategory);
                await _context.SaveChangesAsync();

                return Ok(exsitingCategory);

            }catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { message = "Database error", error = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred", error = ex.Message });
            }
            }

        public async Task<IActionResult> HandleDeleteCategory(string _categoryId)
        {
            try{

                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.CategoryId == _categoryId);

                if (category is null){
                    return NotFound(new { message = "Category not found" });
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return Ok("Delete Successfull");

            }catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { message = "Database error", error = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred", error = ex.Message });
            }
            }
        }
}
