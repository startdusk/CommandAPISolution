using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using CommandAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System;

namespace CommandAPI.Controller
{
    [Route("auth")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticateController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous] // 该属性表示所有人都能访问该endpoint，写不写都无所谓，写上去，代码好懂写
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            // 1.验证用户名和密码

            // 2.生成JWT 
            // 需要安装 
            // Microsoft.AspNetCore.Authentication.JwtBearer
            // Microsoft.IdentityModel.Tokens
            // header
            var signingAlgorithm = SecurityAlgorithms.HmacSha256;
            // payload
            var claims = new[]
            {
                // sub
                new Claim(JwtRegisteredClaimNames.Sub, "fake_user_id")
            };
            // signiture
            var secretByte = Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]);
            var signingKey = new SymmetricSecurityKey(secretByte);
            var signingCredentials = new SigningCredentials(signingKey, signingAlgorithm);

            double expiredTimeMinutes;
            if (!double.TryParse(_configuration["Authentication:ExpiredTimeMinutes"], out expiredTimeMinutes))
            {
                throw new SystemException($@"configuration exception field Authentication:ExpiredTimeMinutes {_configuration["ExpiredTimeMinutes"]}");
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow, // 发布时间
                expires: DateTime.UtcNow.AddMinutes(expiredTimeMinutes), // 过期时间(有效期)
                signingCredentials: signingCredentials
            );
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            // 3.return 200 ok + JWT
            return Ok(tokenStr);
        }
    }
}