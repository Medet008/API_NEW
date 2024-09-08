using API_NEW.Data;
using API_NEW.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace API_NEW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;
        public OrderController(ApplicationDbContext db) 
        {
            _db = db;
            _response = new ApiResponse();  
        }


        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetOrders(string? userId)
        {
            try
            {
                var orderHeaders = _db.OrderHeaders.Include(u => u.OrderDetails)
                    .ThenInclude(u => u.MenuItem)
                    .OrderByDescending(u => u.OrderHeaderId); 
                if(!string.IsNullOrEmpty(userId))
                {
                    _response.Result = orderHeaders.Where(u => u.ApplicationUserId == userId); 

                }
                else
                {
                    _response.Result = orderHeaders; 
                }

                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response); 

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages 
                    = new List<string> { ex.ToString() };
            }
            return _response; 
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse>> GetOrder(int id)
        {
            try
            {
                if(id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response); 
                }

                var orderHeaders = _db.OrderHeaders.Include(_u => _u.OrderDetails)
                    .ThenInclude(u => u.MenuItem)
                    .Where(u => u.OrderHeaderId == id); 

                if(orderHeaders == null)
                {
                    _response.StatusCode = HttpStatusCode.OK;
                    return NotFound(_response); 
                }

                _response.Result = orderHeaders; 
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false; 
                _response.ErrorMessages
                     = new List<string>(){ ex.ToString() }; 
            }
            return _response;
        }
    }
}
