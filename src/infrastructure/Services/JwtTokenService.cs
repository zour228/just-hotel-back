using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Abstraction;
using Domain.User;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private JwtSecurityTokenHandler TokenHandler { get; } = new JwtSecurityTokenHandler();
        private Func<UserEntity, IEnumerable<Claim>>? ClaimsGenerator { get; set; }

        public string CreateToken(UserEntity userEntity)
        {
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TOKEN_SECRET_KEY") ?? "");

            var claims = null != ClaimsGenerator ? ClaimsGenerator.Invoke(userEntity) : CreateDefaultClaims(userEntity);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = null,
                Audience = null,
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(
                    Convert.ToDouble(Environment.GetEnvironmentVariable("TOKEN_TTL") ?? "0")
                ),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };


            return TokenHandler.WriteToken(TokenHandler.CreateJwtSecurityToken(descriptor));
        }

        public void ConfigureClaims(Func<UserEntity, IEnumerable<Claim>> claimsGenerator)
        {
            ClaimsGenerator = claimsGenerator;
        }

        private IEnumerable<Claim> CreateDefaultClaims(UserEntity user)
        {
            return new List<Claim>
            {
                new Claim("UserId", user.Id.ToString()), 
                new Claim("Login", user.Login),
                new Claim("Roles", user.Role.ToString())
            };
        }
    }
}