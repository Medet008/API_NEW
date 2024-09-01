using API_NEW.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_NEW.Controllers
{
    [Route("api/AuthTest")]
    [ApiController]
    public class AuthTestController : ControllerBase
    {

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<string>> GetSomething()
        {
            return "You are authenticated"; 
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<ActionResult<string>> GetSomething(int SomeIntValue)
        {
            return "You are Authorized with Role of Admin"; 
        }
        
    }
}
