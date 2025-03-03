using CakeStoreBE.Application.Services;
using Microsoft.AspNetCore.Mvc;
using CakeStoreBE.Application.DTOs.CategoryDTOs;

namespace CakeStoreBE.Presentation.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase{
        private readonly ICategoryServices _categoryServices;
        public CategoriesController(ICategoryServices categoryServices){
            _categoryServices = categoryServices;
        }
        [HttpGet("Gett All Categories")]
        public async Task<IActionResult> GetAllCategories(){
            return await _categoryServices.HandleGetAllCategory();
        }

        [HttpGet("Get Category By Id")]
        public async Task<IActionResult> GetCategoryById(string categoryId){
            return await _categoryServices.HandleGetCategoryById(categoryId);
        }

        [HttpPost("Create Category")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDTO categoryDTO){
            return await _categoryServices.HandleCreateCategory(categoryDTO);
        }

        [HttpPut("Update Category")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryDTO categoryDTO){
            return await _categoryServices.HandleUpdateCategory(categoryDTO);
        }

        [HttpDelete("Delete Category")]
        public async Task<IActionResult> DeleteCategory(string categoryId){
            return await _categoryServices.HandleDeleteCategory(categoryId);
        }
    }
}