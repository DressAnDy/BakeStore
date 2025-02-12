using CakeStoreBE.Application.DTOs.CakeProductDTOs;
using CakeStoreBE.Application.Services;
using Microsoft.AspNetCore.Mvc;



namespace CakeStoreBE.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CakeController : ControllerBase
    {
        private readonly ICakeProductServices _cakeProductServices;
        public CakeController(ICakeProductServices cakeProductServices)
        {
            _cakeProductServices = cakeProductServices;
        }

        [HttpGet("Get Cake By Id")]
        public async Task<IActionResult> GetCakeById(string cakeId)
        {
            return await _cakeProductServices.HandleGetCakeById(cakeId);
        }

        [HttpGet("Get All Cake")]
        public async Task<IActionResult> GetAllCake()
        {
            return await _cakeProductServices.HandleGetAllCake();
        }

        [HttpDelete("Delete Cake By Id")]
        public async Task<IActionResult> DeleteCakeById(string cakeId)
        {
            return await _cakeProductServices.HandleDeleteCake(cakeId);
        }

        [HttpPost("Create Cake Product")]
        public async Task<IActionResult> CreateCakeProduct([FromBody] CreateCakeDTO createCakeDTO)
        {
            return await _cakeProductServices.HandleCreateCake(createCakeDTO);
        }

        [HttpPut("Update Cake Product")]
        public async Task<IActionResult> UpdateCakeProduct([FromBody] UpdateCakeProductDTO updateCakeProductDTO)
        {
            return await _cakeProductServices.HandleUpdateCake(updateCakeProductDTO);
        }
    }
}