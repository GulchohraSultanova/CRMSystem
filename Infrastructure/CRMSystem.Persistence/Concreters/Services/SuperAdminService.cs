using AutoMapper;
using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Account;
using CRMSystem.Application.GlobalAppException;
using CRMSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

public class AdminService : IAdminService
{
    private readonly UserManager<Admin> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AdminService> _logger;

    public AdminService(
        UserManager<Admin> userManager,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper,
        ITokenService tokenService,
        ILogger<AdminService> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _tokenService = tokenService;
        _logger = logger;
    }

    // Yeni qeydiyyat: telefon nömrəsi əsaslı
    public async Task RegisterAdminAsync(RegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            throw new GlobalAppException("Telefon nömrəsi tələb olunur!");

        // email kimi telefon@… formalaşdırırıq
        var normalizedPhone = dto.PhoneNumber.Trim();
        var email = $"{normalizedPhone}@gmail.com";

        // artıq varsa xətanı at
        var existing = await _userManager.FindByEmailAsync(email);
        if (existing != null)
            throw new GlobalAppException("Bu nömrə ilə artıq admin mövcuddur!");

        // AutoMapper ilə köçür, sonra e-poçt/username/phone doldur
        var admin = _mapper.Map<Admin>(dto);
        admin.Email = email;
        admin.UserName = email;
        admin.PhoneNumber = normalizedPhone;

        var result = await _userManager.CreateAsync(admin, dto.Password);
        if (!result.Succeeded)
            throw new GlobalAppException(
                $"Admin yaradılmadı: {string.Join(", ", result.Errors.Select(e => e.Description))}"
            );

        // SuperAdmin rolunu yoxla, yoxdursa yarat və təyin et
        if (!await _roleManager.RoleExistsAsync("SuperAdmin"))
            await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));

        await _userManager.AddToRoleAsync(admin, "SuperAdmin");
    }
    public async Task<TokenResponseDto> LoginFighterOrCustomerAsync(LoginDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
                throw new GlobalAppException("Telefon nömrəsi tələb olunur!");

            // Telefon nömrəsindən email formalaşdırırıq
            var normalizedPhone = dto.PhoneNumber.Trim();
            var email = $"{normalizedPhone}@gmail.com";

            // İstifadəçini tapırıq
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new GlobalAppException("Daxil edilən nömrə ilə istifadəçi tapılmadı.");

            // Şifrəni yoxlayırıq
            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new GlobalAppException("Şifrə yalnışdır.");

            // Rolları götürürük
            var roles = await _userManager.GetRolesAsync(user);
            // Yalnız Fighter və ya Customer rolları icazəlidir
            var allowedRole = roles.FirstOrDefault(r => r == "Fighter" || r == "Customer");
            if (string.IsNullOrEmpty(allowedRole))
                throw new GlobalAppException("Giriş rədd edildi! Yalnız Təchizatçı və ya Sifarişçi daxil ola bilər.");

            // Token yaradırıq
            return _tokenService.CreateToken(user, allowedRole);
        }
        catch (GlobalAppException ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Daxil olarkən gözlənilməz xəta baş verdi.");
            throw new GlobalAppException("Daxil olarkən gözlənilməz xəta baş verdi.", ex);
        }
    }

    // Telefon nömrəsi + "@gmail.com" ilə giriş
    public async Task<TokenResponseDto> LoginAdminAsync(LoginDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
                throw new GlobalAppException("Telefon nömrəsi tələb olunur!");

            var email = $"{dto.PhoneNumber.Trim()}@gmail.com";
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new GlobalAppException("Daxil edilən nömrə ilə admin tapılmadı.");

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new GlobalAppException("Şifrə yanlışdır.");

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("SuperAdmin"))
                throw new GlobalAppException("Giriş rədd edildi! Yalnız Admin daxil ola bilər.");

            // Token yarat
            return _tokenService.CreateToken(user, "SuperAdmin");
        }
        catch (GlobalAppException ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Admin daxil edilərkən xəta baş verdi.");
            throw new GlobalAppException("Daxil olarkən gözlənilməz xəta baş verdi.", ex);
        }
    }

    public async Task DeleteAllAdminsAsync()
    {
        var admins = await _userManager.GetUsersInRoleAsync("SuperAdmin");
        if (!admins.Any())
            throw new GlobalAppException("Silmək üçün heç bir admin tapılmadı!");

        foreach (var admin in admins)
        {
            var res = await _userManager.DeleteAsync(admin);
            if (!res.Succeeded)
                throw new GlobalAppException(
                    $"Admin {admin.Email} silinərkən xəta: {string.Join(", ", res.Errors.Select(e => e.Description))}"
                );
        }
    }
}
