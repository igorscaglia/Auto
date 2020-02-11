using System.Threading.Tasks;
using AutoMapper;
using Auto.VehicleCatalog.API.Dtos;
using Auto.VehicleCatalog.API.Helpers;
using Auto.VehicleCatalog.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Auto.VehicleCatalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(IConfiguration config,
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegister userForRegister)
        {
            // map to dto
            var userToCreate = _mapper.Map<User>(userForRegister);

            // Create the new user
            var result = await _userManager.CreateAsync(userToCreate, userForRegister.password);

            if (result.Succeeded)
            {
                // return created user
                return Ok(new
                {
                    user = userToCreate
                });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(UserForLogin userForLogin)
        {
            // UserManager is responsible for user database operations instead DataContext
            var user = await _userManager.FindByNameAsync(userForLogin.username);

            // SignInManager verify is password is ok
            var result = await _signInManager
                .CheckPasswordSignInAsync(user, userForLogin.password, false);

            if (result.Succeeded)
            {
                var authConfigurationSection = _config.GetSection("AuthConfiguration");
                var tokenSecurityKey = authConfigurationSection.GetValue<string>("TokenValidateSecurityKey");

                // return created token
                return Ok(new
                {
                    token = await user.GenerateJwtToken(_userManager, tokenSecurityKey, 24D)
                });
            }

            return Unauthorized();
        }
    }
}
