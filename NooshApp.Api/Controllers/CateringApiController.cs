using Microsoft.AspNetCore.Mvc;
using NooshApp.Api.Dtos;
using NooshApp.Api.Services.Interfaces;

namespace NooshApp.Api.Controllers
{
    [ApiController]
    [Route("api/catering")]
    public class CateringApiController : ControllerBase
    {
        // Reference to the catering service containing the core business logic
        private readonly ICateringService _cateringService;
        public CateringApiController(ICateringService cateringService) 
        {
             _cateringService = cateringService; 
        }
        //HTTP POST endpoint to submit a new catering request: POST /api/catering
        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] CateringRequestCreateDto request)
        {
            var result = await _cateringService.SubmitRequestAsync(request);
            // Return an HTTP 200 OK response containing the created catering request DTO
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            //Query the service layer for the requested catering record
            var result = await _cateringService.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }


        
    }
}