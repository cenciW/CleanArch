using CleanArchMvc.Domain.Account;
using CleanArchMvc.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchMvc.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAuthenticate _authentication;
        private readonly IConfiguration _configuration;
        public TokenController(IAuthenticate authentication, IConfiguration configuration)
        {
            _authentication = authentication ??
                throw new ArgumentNullException(nameof(authentication));
            _configuration = configuration;
        }

        
        [HttpPost("CreateUser")]
        //Para não aparecer no swagger
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> CreateUser([FromBody] LoginModel userInfo)
        {
            var result = await _authentication.RegisterUser(userInfo.Email, userInfo.Password);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Invalid  attemp");
                return BadRequest(ModelState);
            }
            else
            {

                return Ok($"User: {userInfo.Email} was created successfully");
            }

        }

        //autenticar user
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
                return GenerateToken(userInfo);

                //para teste:
                //return Ok($"User: {userInfo.Email} login successfully");
            }
        }

        private UserToken GenerateToken(LoginModel userInfo)
        {
            var claims = new[]
            {
                new Claim("email", userInfo.Email),
                new Claim("meuvalor", "oque voce quiser"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //Gerar chave privada para assinar token
            var privateKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            //Gerar assinatura digital
            var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);

            //Definir tempo de expiração do token
            var expiration = DateTime.UtcNow.ToLocalTime().AddMinutes(10);

            //gerar token
            JwtSecurityToken token = new JwtSecurityToken(
                //emissor 
                issuer: _configuration["Jwt:Issuer"],
                //Audiencia
                audience: _configuration["Jwt:Audience"],
                //claims
                claims: claims,
                //Data de expiração
                expires: expiration,
                //ASsinatura digital 
                signingCredentials: credentials
                );
            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration

            };
        }
    }
}
    