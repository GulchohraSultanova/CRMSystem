using CRMSystem.Application.Dtos.Account;
using CRMSystem.Application.GlobalAppException;
using CRMSystem.Domain.Entities;
using System.Threading.Tasks;

namespace CRMSystem.Application.Absrtacts.Services
{
    public interface IFighterService
    {
        Task RegisterFighterAsync(RegisterDto dto);
        Task ChangePasswordAsync(string fighterId, ChangePasswordDto dto);
        Task DeleteAllFightersAsync();
        Task<RegisterDto> UpdateFighterAsync(UpdateFighterDto updateDto);
        Task DeleteFighterByEmailAsync(string email);
        Task<AdminInfoDto> GetFighterByIdAsync(string id);
        Task<List<AdminInfoDto>> GetAllFightersAsync();
    }
}