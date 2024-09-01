using API_NEW.Data;
using API_NEW.Models;
using API_NEW.Models.Dto;
using API_NEW.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API_NEW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string secretKey;

        public AuthController(ApplicationDbContext db, IConfiguration configuration,
             UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret"); 
            _response = new ApiResponse();  
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            ApplicationUser userFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(userFromDb, model.Password); 

            if (isValid == false) {
                _response.Result = new LoginRequestDTO(); 
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password is incorrect");
                return BadRequest(_response);
            }

            LoginResponseDTO loginResponse = new()
            {
                Email = userFromDb.Email,
                Token = "REPLACE WITH ACTUAL TOKEN ONCE WE GENERATE"
            }; 

            if (loginResponse.Email == null ||  string.IsNullOrEmpty(loginResponse.Token)) {
                _response.StatusCode =HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password is incorrect"); 
                return BadRequest(_response);
            
            }

            _response.StatusCode = HttpStatusCode.OK; 
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response); 
        }




        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            ApplicationUser userFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
            if (userFromDb != null) 
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username already exists"); 
                return BadRequest(_response);
            
            }

            ApplicationUser newUser = new()
            {
                UserName = model.UserName,
                Email = model.UserName,
                NormalizedEmail = model.UserName.ToUpper(),
                Name = model.Name
            }; 
            
            try
            {
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    if(!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
                    }
                    if(model.Role.ToLower() == SD.Role_Admin)
                    {
                        await _userManager.AddToRoleAsync(newUser, SD.Role_Admin);  
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(newUser, SD.Role_Admin); 
                    }
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSuccess = false;
                    return Ok(_response);
                }
            }
            catch (Exception)
            {

            }
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages.Add("Error while registering"); 
            return BadRequest(_response);
        }

    }
}
