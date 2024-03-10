using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Model.Data.DTOs;
using Model.Data.Models;
using Model.Data.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Logsquare_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AuthController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(UserDTO userDTO)
        {
            try
            {
                userDTO.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
                var user = _mapper.Map<User>(userDTO);
                var userAdded = await _userRepository.SignIn(user);
                var res = _mapper.Map<UserDTO>(userAdded);

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values.SelectMany(v => v.Errors));
                }

                var user = await _userRepository.GetUser(login.Email);
                bool verified = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);

                if (user == null || !verified)
                {
                    return Unauthorized("Invalid email or password");
                }

                var userDTO = _mapper.Map<UserDTO>(user);

                var token = GenerateJwtToken(userDTO);
                return Ok(new { token });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GenerateJwtToken(UserDTO user)
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
