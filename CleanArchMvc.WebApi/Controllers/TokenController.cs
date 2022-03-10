using CleanArchMvc.Domain.Account;
using CleanArchMvc.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchMvc.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAuthenticate _authentication;
        public TokenController(IAuthenticate authentication)
        {
            _authentication = authentication ??
                throw new ArgumentNullException(nameof(authentication));
        }

        [HttpPost("LoginUser")]
        public async Task<ActionResult<UserToken>> Login([FromBody] LoginModel userInfo)
        {

            var result = await _authentication.Authenticate(userInfo.Email, userInfo.Password);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Invalid Login attemp");
                return BadRequest(ModelState);
            }
            else
            {
                //return GenerateToken(userInfo);
                
                //para teste:
                return Ok($"User: {userInfo.Email} login successfully");
            }
        }
    }
}
    