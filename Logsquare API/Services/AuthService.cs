using Microsoft.IdentityModel.Tokens;
using Model.Data.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class AuthService
    {
        public AuthService() { }

        public string GenerateJwtToken(UserDTO user)
        {
            try
            {
                var secret = "1234567899874563210..0...00..0.0.0.0.0.0.0212";

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, user.Email)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
                };

                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(securityToken);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
