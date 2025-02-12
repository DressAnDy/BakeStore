using CakeStoreBE.Application.DTOs.CakeProductDTOs;
using CakeStoreBE.Domain;
using CakeStoreBE.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CakeStoreBE.Application.Services
{
    public interface ICakeProductServices
    {
        public Task<IActionResult> HandleCreateCake(CreateCakeDTO _createCake);
        public Task<IActionResult> HandleUpdateCake(UpdateCakeProductDTO _updateCake);
        public Task<IActionResult> HandleGetCakeById(string _cakeId);
        public Task<IActionResult> HandleGetAllCake();
        public Task<IActionResult> HandleDeleteCake(string _cakeId);
    }

    public class CakeProductServices : ControllerBase, ICakeProductServices
    {
        public readonly BakeStoreDbContext _context;

        public CakeProductServices(BakeStoreDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> HandleCreateCake(CreateCakeDTO _createCake)
        {
            try
            {
                var exsitingCake = _context.CakeProduct.Any(c => c.Name == _createCake.Name);

                if (exsitingCake)
                {
                    throw new Exception("Cake already exits");
                }


                var cake = new CakeProduct
                {
                    CakeId = Guid.NewGuid().ToString(),
                    Name = _createCake.Name,
                    Description = _createCake.Description,
                    Price = _createCake.Price,
                    StockQuantity = _createCake.StockQuantity,
                    CreatedAt = DateTime.UtcNow,
                };

                await _context.CakeProduct.AddAsync(cake);
                await _context.SaveChangesAsync();

                return Ok(cake);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { message = "Database error", error = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred", error = ex.Message });
            }
        }

        public async Task<IActionResult> HandleUpdateCake(UpdateCakeProductDTO _updateCake)
        {
            try
            {
                var exsitingCake = _context.CakeProduct
                    .FirstOrDefault(c => c.CakeId == _updateCake.CakeId);

                if (exsitingCake == null){
                    return BadRequest("Cake not found");
                }

                exsitingCake.CakeId = _updateCake.CakeId;
                exsitingCake.Name = _updateCake.Name;
                exsitingCake.Description = _updateCake.Description;
                exsitingCake.Price = _updateCake.Price;
                exsitingCake.StockQuantity = _updateCake.StockQuantity;
                exsitingCake.UpdatedAt = DateTime.UtcNow;

                _context.CakeProduct.Update(exsitingCake);
                await _context.SaveChangesAsync();

                return Ok(exsitingCake);    
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { message = "Database error", error = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred", error = ex.Message });
            }
        }

        public async Task<IActionResult> HandleGetCakeById(string _cakeId)
        {
            try
            {
                var cake = await _context.CakeProduct
                    .FirstOrDefaultAsync(c => c.CakeId == _cakeId);

                if (cake == null)
                {
                    return BadRequest("Cake not found");
                }

                return Ok(cake);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { message = "Database error", error = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred", error = ex.Message });
            }
        }

        public async Task<IActionResult> HandleGetAllCake()
        {
            try
            {
                var cakes = await _context.CakeProduct
                    .ToListAsync();

                return Ok(cakes);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { message = "Database error", error = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred", error = ex.Message });
            }
        }

        public async Task<IActionResult> HandleDeleteCake(string _cakeId)
        {
            try
            {
                var cake = await _context.CakeProduct
                    .FirstOrDefaultAsync(c => c.CakeId == _cakeId);

                if (cake == null)
                {
                    return BadRequest("Cake not found");
                }

                _context.CakeProduct.Remove(cake);
                await _context.SaveChangesAsync();

                return Ok("Delete Successfull");
            }
            catch (DbUpdateException dbEx)
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


