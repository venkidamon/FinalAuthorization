using Authorization.Data;
using Authorization.Dtos;
using Authorization.Entities;
using Authorization.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Authorization.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            this._context = context;
            this._tokenService = tokenService;
            this._mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await UserExists(registerDto.UserName))
            {
                return BadRequest("UserName Already Taken");
            }

            var user = _mapper.Map<AppUser>(registerDto);   
            using var hmac = new HMACSHA512();


            user.UserName = registerDto.UserName.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string userName)
        {
            var user = await _context.Users.AnyAsync(x => x.UserName == userName.ToLower());
            if (user == false) return false;
            else return true;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());
            if(user == null) return Unauthorized("Invalid Username");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for(int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");

            }
            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

        }
    }
}
