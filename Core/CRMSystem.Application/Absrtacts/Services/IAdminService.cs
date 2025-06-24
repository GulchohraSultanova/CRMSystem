using CRMSystem.Application.Dtos.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Application.Absrtacts.Services
{
    public interface IAdminService
    {
        Task RegisterAdminAsync(RegisterDto registerDto);
        Task<TokenResponseDto> LoginAdminAsync(LoginDto loginDto);

        Task<TokenResponseDto> LoginFighterOrCustomerAsync(LoginDto dto);

        Task DeleteAllAdminsAsync();
    
    }
}
