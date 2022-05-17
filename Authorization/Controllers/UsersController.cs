using Authorization.Data;
using Authorization.Dtos;
using Authorization.Entities;
using Authorization.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Controllers
{
    
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            return Ok(_userRepository.GetUserAsync());
        }
        [Authorize]
        [HttpGet("{username}")]
        public async Task<ActionResult<AppUser>> GetUserByUsername(string username)
        {
            return await _userRepository.GetUserByNameAsync(username);
           
        }


    }
}
