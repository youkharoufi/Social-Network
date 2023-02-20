using AutoMapper;
using Facebook.ClaimsUsers;
using Facebook.Models;
using Facebook.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signinManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<User> userManager,
            SignInManager<User> signinManager,
            ITokenService tokenService,
            IMapper mapper
            )
        {
            _mapper = mapper;
            _userManager = userManager;
            _signinManager = signinManager;
            _tokenService = tokenService;
        }

        //[HttpGet("{username}")]
        //public async Task<ActionResult<User>> GetUser(string username)
        //{
        //    return await _userManager.Users.Include(p => p.Friends).FirstOrDefaultAsync(o=>o.UserName == username);
        //}

        [HttpGet("{id}")]
        public async Task<User> GetUserById(string id)
        {
            return await _userManager.Users.FirstOrDefaultAsync(o => o.Id == id);
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterUser userData)
        {
            var user = _mapper.Map<User>(userData);

            var result = await _userManager.CreateAsync(user, userData.Password);

            //var roleResults = await _userManager.AddToRolesAsync(user, userData.Roles);


            var newUser = new User
            {
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,
                Token = ""
            };

            if (result.Succeeded)
            {

                return Ok(newUser);
              

            }
            else
            {

                return BadRequest("Registration error");


            }    
        }

        

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginUser loginData)
        {

            var userFromDb = await _userManager.Users.FirstOrDefaultAsync(o=>o.UserName == loginData.UserName);

            if (userFromDb == null) return NotFound();

            var result = await _signinManager.CheckPasswordSignInAsync(userFromDb, loginData.Password, false);

            if (!result.Succeeded) return BadRequest("Invalid Password");

            //var roles = await _userManager.GetRolesAsync(userFromDb);

            //if (roles.Count == 0) return BadRequest("ponible");

            //foreach(var role in roles)
            //{
            //    System.Diagnostics.Debug.WriteLine("Test de la montoj : " + role);
            //}

            userFromDb.Token =  _tokenService.GenerateToken(userFromDb);

            return Ok(userFromDb);
             

        }

        [HttpGet("user/{username}")]
        public async Task<ActionResult<User>> GetUserByUsername(string username)
        {
            var user = await _userManager.Users.Include(o => o.Photos).FirstOrDefaultAsync(o => o.UserName == username);

            return user;
        }

        [HttpPut]
        public async Task<User> ChangeUsername(UserChangeUsername userChange)
        {
            var user = await _userManager.FindByNameAsync(userChange.oldUsername);

            user.UserName = userChange.newUsername;

            await _userManager.UpdateAsync(user);

            return user;
        }

        [HttpPut("role")]
        public async Task<User> changeUserRole(UserChangeRole userChangeRole)
        {
            var user = await _userManager.FindByIdAsync(userChangeRole.UserId);

            user.Role = userChangeRole.newRole;

            await _userManager.UpdateAsync(user);

            return user;
        }

    }
}
