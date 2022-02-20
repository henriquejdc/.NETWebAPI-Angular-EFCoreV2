using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using proeventos.Application.Contratos;
using proeventos.Application.Dtos;
using proeventos.Domain.Identity;
using proeventos.Persistence.Contratos;

namespace proeventos.Application
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserPersistence _userPersistence;

        public AccountService(UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              IMapper mapper,
                              IUserPersistence userPersistence)
        {
            _userPersistence = userPersistence;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password)
        {
            try
            {

                var user = await _userManager.Users
                                             .SingleOrDefaultAsync(user => user.UserName == userUpdateDto.UserName.ToLower());
                
                return await _signInManager.CheckPasswordSignInAsync(user, password, false);
            
            }
            catch (System.Exception ex)
            {
                
                throw new System.Exception($"Erro ao tentar verificar password. Erro {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> CreateAccountAsync(UserDto userDto)
        {
            try
            {
                
                var user = _mapper.Map<User>(userDto);

                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (result.Succeeded)
                {
                    var userToReturn = _mapper.Map<UserUpdateDto>(user);
                    return userToReturn;
                }

                return null;

            }
            catch (System.Exception ex)
            {
                
                throw new System.Exception($"Erro ao tentar criar a conta. Erro {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> GetUserByUserNameAsync(string userName)
        {
            try
            {
                var user = await _userPersistence.GetUserByUserNameAsync(userName);

                if(user == null) return null;

                var userUpdateDto = _mapper.Map<UserUpdateDto>(user);
                return userUpdateDto;

            }
            catch (System.Exception ex)
            {
                
                throw new System.Exception($"Erro ao tentar pegar o usuário. Erro {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userPersistence.GetUserByUserNameAsync(userUpdateDto.UserName);
                if (user == null) return null;

                userUpdateDto.Id = user.Id;

                _mapper.Map(userUpdateDto, user);

                if (userUpdateDto.Password != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password);
                }

                _userPersistence.Update<User>(user);

                if (await _userPersistence.SaveChangesAsync())
                {
                    var userRetorno = await _userPersistence.GetUserByUserNameAsync(user.UserName);

                    return _mapper.Map<UserUpdateDto>(userRetorno);
                }

                return null;
            }
            catch (System.Exception ex)
            {
                
                throw new System.Exception($"Erro ao tentar atualizar a conta. Erro {ex.Message}");
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            try
            {
                return await _userManager.Users.AnyAsync(user => user.UserName.ToLower() == userName.ToLower());
            }
            catch (System.Exception ex)
            {
                
                throw new System.Exception($"Erro ao tentar verificar o usuário. Erro {ex.Message}");
            }
        }
    }
}