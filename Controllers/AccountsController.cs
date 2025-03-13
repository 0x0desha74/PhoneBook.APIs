using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.APIs.DTOs;
using PhoneBook.APIs.Errors;
using PhoneBook.Core.Entities.Identity;
using PhoneBook.Core.Services;

namespace PhoneBook.APIs.Controllers
{

    public class AccountsController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;

        public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null) return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)

            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new ApiResponse(400, "Email Already Exists"));

            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email.Split("@")[0]
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}


