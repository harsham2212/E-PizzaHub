using epizzahub.Entitites.Entitites;
using epizzahub.Models;
using epizzahub.Repositories.Interfaces;
using epizzahub.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace epizzahub.Services.Implementation
{
    public class AuthService : Service<User>, IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        public AuthService(IUserRepository userRepository,IConfiguration config):base(userRepository)
        {
            _userRepository = userRepository;
            _config = config;
        }

        private string GenerateToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                             new Claim(JwtRegisteredClaimNames.Sub, userInfo.Name),
                             new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                             };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                                            _config["Jwt:Audience"],
                                            claims,
                                            expires: DateTime.UtcNow.AddMinutes(60), //token expiry minutes
                                            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool CreateUser(User user, string role)
        {
            return _userRepository.CreateUser(user, role);
        }

        public UserModel ValidateUser(string Email, string Password)
        {
            UserModel model = _userRepository.ValidateUser(Email, Password);
            if (model != null)
            {
                model.Token = GenerateToken(model);
            }
            return model;
        }
    }
}
