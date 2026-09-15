using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtTokenBaseAuthentication.DTO;

namespace JwtTokenBaseAuthentication.Services.IServices
{
    public interface IUserService
    {
        Task<UserResponseDto> Register(UserRegisterDto userRegisterDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}