using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using proeventos.api.Extensions;
using proeventos.api.helpers;
using proeventos.Application.Contratos;
using proeventos.Application.Dtos;

namespace proeventos.api.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly string _destino = "Perfil";
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        private readonly IUtil _util;
        public AccountController(IAccountService accountService,
                                 ITokenService tokenService,
                                 IUtil util)
        {
            _tokenService = tokenService;
            _accountService = accountService;
            _util = util;

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
                if (userUpdateDto.UserName != User.GetUserName())
                    return Unauthorized("Usuário inválido!");

                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
                if (user == null) return Unauthorized("Usuário não encontrado!");

                var userReturn = await _accountService.UpdateAccount(userUpdateDto);

                if (userReturn == null)
                {
                    return NoContent();
                }

                return Ok(
                    new {
                        UserName = userReturn.UserName,
                        PrimeiroNome = userReturn.PrimeiroNome,
                        Token = _tokenService.CreateToken(userReturn).Result
                    });

            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao atualizar usuário. Erro: {ex.Message}");
            }    
        }


        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage()
        {
            try
            {
                var account = await _accountService.GetUserByUserNameAsync(User.GetUserName());

                if (account == null) return NoContent();

                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    _util.DeleteImage(account.ImagemURL, _destino);
                    account.ImagemURL = await _util.SaveImage(file, _destino);
                }
                var AccountRetorno = await _accountService.UpdateAccount(account);

                ;
                return Ok(AccountRetorno);
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao inserir imagem do usuário. Erro: {ex.Message}");
            }     
        }
    }
}