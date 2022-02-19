using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using proeventos.api.Extensions;
using proeventos.Application.Contratos;
using proeventos.Application.Dtos;

namespace proeventos.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        public AccountController(IAccountService accountService,
                                 ITokenService tokenService)
        {
            _tokenService = tokenService;
            _accountService = accountService;

        }

        [HttpGet("GetUser")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userName = User.GetUserName();
                var user = await _accountService.GetUserByUserNameAsync(userName);
                return Ok(user);
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao recuperar usuário. Erro: {ex.Message}");
            }    
        }

        [HttpPost("RegisterUser")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser(UserDto userDto)
        {
            try
            {
                if (await _accountService.UserExists(userDto.Username))
                {
                    return BadRequest("Username já existe");
                }
                
                var user = await _accountService.CreateAccountAsync(userDto);

                if (user != null)
                {
                    return Ok(
                    new {
                        UserName = user.UserName,
                        PrimeiroNome = user.PrimeiroNome,
                        Token = _tokenService.CreateToken(user).Result
                    });
                }

                return BadRequest("Usuário não criado, tente mais tarde novamente!");

            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao registrar usuário. Erro: {ex.Message}");
            }    
        }

        [HttpPost("LoginUser")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser(UserLoginDto userLoginDto)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(userLoginDto.Username);
                if (user == null) return Unauthorized("Usuário não encontrado!");

                var result = await _accountService.CheckUserPasswordAsync(user, userLoginDto.Password);
                if (!result.Succeeded) return Unauthorized("Senha inválida!");

                return Ok(
                    new {
                        UserName = user.UserName,
                        PrimeiroNome = user.PrimeiroNome,
                        Token = _tokenService.CreateToken(user).Result
                    }
                );
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao logar. Erro: {ex.Message}");
            }    
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
                if (user == null) return Unauthorized("Usuário não encontrado!");

                var userReturn = await _accountService.UpdateAccount(userUpdateDto);

                if (userReturn != null)
                {
                    return NoContent();
                }

                return Ok(userReturn);

            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao atualizar usuário. Erro: {ex.Message}");
            }    
        }
    }
}