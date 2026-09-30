using Microsoft.AspNetCore.Mvc;
using NooshApp.Api.Services.Interfaces;

namespace NooshApp.Api.Controllers
{
    [ApiController]
    //Sets the base route URL path for all endpoints in this controller to "/api/menu"
    [Route("api/menu")]
    public class MenuApiController : ControllerBase
    {
        //Reference to the menu service containing the business logic layer
        private readonly IMenuService _menuService;

        //Constructor injection to receive the menu service implementation
        public MenuApiController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        
    }
}