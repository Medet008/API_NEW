using API_NEW.Data;
using API_NEW.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API_NEW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;
        public MenuItemController(ApplicationDbContext db) 
        {
            _db = db;
            _response = new ApiResponse();
           
        }

        [HttpGet]
        public async Task<IActionResult> GetMenuItems()
        {
            var menuItems = await _db.MenuItems.ToListAsync();

            _response.Result = menuItems;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMenuItem(int id)
        {
            if(id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;   
                return BadRequest(_response);
            }
            MenuItem menuItem = await _db.MenuItems.FirstOrDefaultAsync(u => u.Id == id);  
            if (menuItem == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound; 
                return NotFound(_response);
            }
            _response.Result = menuItem;
            _response.StatusCode = HttpStatusCode.OK; 
            return Ok(_response) ;
           
        }

    }
}
