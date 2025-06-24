using AutoMapper;
using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Account;
using CRMSystem.Application.GlobalAppException;
using CRMSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

public class FighterService : IFighterService
{
    private readonly UserManager<Admin> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly ILogger<FighterService> _logger;

    public FighterService(
        UserManager<Admin> userManager,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper,
        ITokenService tokenService,
        ILogger<FighterService> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task RegisterFighterAsync(RegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            throw new GlobalAppException("Telefon nömrəsi tələb olunur!");

        // +99450xxxxxxx və s. formatında olmalıdır, yoxsa yoz.
        // Validation qaydalarını RegisterDtoValidation-a əlavə edin.

        var phone = dto.PhoneNumber.Trim();
        var email = $"{phone}@gmail.com";

        if (await _userManager.FindByEmailAsync(email) != null)
            throw new GlobalAppException("Bu nömrə ilə artıq fighter mövcuddur!");

        var fighter = _mapper.Map<Admin>(dto);
        fighter.Email = email;
        fighter.UserName = email;
        fighter.PhoneNumber = phone;

        var result = await _userManager.CreateAsync(fighter, dto.Password);
        if (!result.Succeeded)
            throw new GlobalAppException(
                $"Fighter yaradılmadı: {string.Join(", ", result.Errors.Select(e => e.Description))}"
            );

        if (!await _roleManager.RoleExistsAsync("Fighter"))
            await _roleManager.CreateAsync(new IdentityRole("Fighter"));

        await _userManager.AddToRoleAsync(fighter, "Fighter");
    }



    public async Task<List<AdminInfoDto>> GetAllFightersAsync()
    {
        var fighters = await _userManager.GetUsersInRoleAsync("Fighter");
        return _mapper.Map<List<AdminInfoDto>>(fighters);
    }

    public async Task<AdminInfoDto> GetFighterByIdAsync(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new GlobalAppException("ID formatı düzgün deyil!");

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            throw new GlobalAppException("Fighter tapılmadı!");

        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Contains("Fighter"))
            throw new GlobalAppException("Bu istifadəçi Fighter rolunda deyil.");

        return _mapper.Map<AdminInfoDto>(user);
    }

    public async Task<RegisterDto> UpdateFighterAsync(UpdateFighterDto dto)
    {
        // Yeniləmə telefon nömrəsinə görə yox, Email (=nömrə@gmail.com)-ə görə
;
        var fighter = await _userManager.FindByIdAsync(dto.Id);
        if (fighter == null)
            throw new GlobalAppException("Fighter tapılmadı.");
        
        fighter.Name = dto.Name?? fighter.Name;
        fighter.Surname = dto.Surname ?? fighter.Surname;
        fighter.FinCode = dto.FinCode ?? fighter.FinCode;


        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(fighter);
            var passResult = await _userManager.ResetPasswordAsync(fighter, token, dto.Password);
            if (!passResult.Succeeded)
                throw new GlobalAppException(
                    $"Şifrə güncəllənərkən xəta: {string.Join(", ", passResult.Errors.Select(e => e.Description))}"
                );
        }

        var res = await _userManager.UpdateAsync(fighter);
        if (!res.Succeeded)
            throw new GlobalAppException(
                $"Fighter güncəllənərkən xəta: {string.Join(", ", res.Errors.Select(e => e.Description))}"
            );

        return _mapper.Map<RegisterDto>(fighter);
    }

    public async Task DeleteAllFightersAsync()
    {
        var fighters = await _userManager.GetUsersInRoleAsync("Fighter");
        if (!fighters.Any())
            throw new GlobalAppException("Silmək üçün fighter tapılmadı.");

        foreach (var f in fighters)
        {
            var r = await _userManager.DeleteAsync(f);
            if (!r.Succeeded)
                throw new GlobalAppException(
                    $"Fighter {f.Email} silinərkən xəta: {string.Join(", ", r.Errors.Select(e => e.Description))}"
                );
        }
    }
    public async Task ChangePasswordAsync(string fighterId, ChangePasswordDto dto)
    {
        if (dto.NewPassword != dto.ConfirmPassword)
            throw new GlobalAppException("Yeni şifrə və təsdiq eyni deyil!");

        var user = await _userManager.FindByIdAsync(fighterId);
        if (user == null)
            throw new GlobalAppException("Fighter tapılmadı!");

        var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new GlobalAppException($"Şifrə dəyişdirilə bilmədi: {errors}");
        }
    }

    public async Task DeleteFighterByEmailAsync(string phoneNumber)
    {
        var email = $"{phoneNumber.Trim()}@gmail.com";
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            throw new GlobalAppException("Fighter tapılmadı.");

        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Contains("Fighter"))
            throw new GlobalAppException("Bu istifadəçi Fighter rolunda deyil.");

        var res = await _userManager.DeleteAsync(user);
        if (!res.Succeeded)
            throw new GlobalAppException(
                $"Fighter silinərkən xəta: {string.Join(", ", res.Errors.Select(e => e.Description))}"
            );
    }
}
