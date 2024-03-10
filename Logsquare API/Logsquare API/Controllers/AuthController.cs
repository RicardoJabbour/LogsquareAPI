using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Model.Data.DTOs;
using Model.Data.Models;
using Model.Data.Repositories.Interfaces;
using Services;
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
        private readonly AuthService _authService;

        public AuthController(IUserRepository userRepository, IMapper mapper, AuthService authService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _authService = authService;
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

                var token = _authService.GenerateJwtToken(userDTO);
                return Ok(new { token });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
