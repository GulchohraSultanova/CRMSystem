using AutoMapper;
using CRMSystem.Application.Absrtacts.Repositories.SectionCustomers;
using CRMSystem.Application.Absrtacts.Services;
using CRMSystem.Application.Dtos.Account;
using CRMSystem.Application.Dtos.Company;
using CRMSystem.Application.Dtos.Customer;
using CRMSystem.Application.Dtos.Department;
using CRMSystem.Application.Dtos.Section;
using CRMSystem.Application.GlobalAppException;
using CRMSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Persistence.Concreters.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly UserManager<Admin> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ISectionCustomerReadRepository _sectionCustomerRead;
        private readonly ISectionCustomerWriteRepository _sectionCustomerWrite;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public CustomerService(
            UserManager<Admin> userManager,
            RoleManager<IdentityRole> roleManager,
            ISectionCustomerReadRepository sectionCustomerRead,
            ISectionCustomerWriteRepository sectionCustomerWrite,
            ITokenService tokenService,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _sectionCustomerRead = sectionCustomerRead;
            _sectionCustomerWrite = sectionCustomerWrite;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto dto)
        {
            // Telefon nömrəsindən email formalaşdırırıq
            var phone = dto.PhoneNumber.Trim();
            var email = $"{phone}@gmail.com";

            if (await _userManager.FindByEmailAsync(email) != null)
                throw new GlobalAppException("Bu nömrə ilə artıq müştəri mövcuddur!");

            var user = new Admin
            {
                Name = dto.Name,
                Surname = dto.Surname,
                PhoneNumber = phone,
                Email = email,
                UserName = email,
                FinCode = dto.FinCode,
                JobId = string.IsNullOrWhiteSpace(dto.JobId)
                            ? null
                            : Guid.Parse(dto.JobId)
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new GlobalAppException($"Müştəri yaradılmadı: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            if (!await _roleManager.RoleExistsAsync("Customer"))
                await _roleManager.CreateAsync(new IdentityRole("Customer"));
            await _userManager.AddToRoleAsync(user, "Customer");

            // Əgər section‐lar seçilibsə, əlaqələri qururuq
            if (dto.SectionIds != null)
            {
                foreach (var sid in dto.SectionIds)
                {
                    if (Guid.TryParse(sid, out var secGuid))
                    {
                        await _sectionCustomerWrite.AddAsync(new SectionCustomer
                        {
                            AdminId = user.Id,
                            SectionId = secGuid,
                            CreatedDate = DateTime.UtcNow,
                            LastUpdatedDate = DateTime.UtcNow
                        });
                    }
                }
                await _sectionCustomerWrite.CommitAsync();
            }

            // DTO-ya çevirib qaytarırıq
            return await BuildCustomerDtoAsync(user);
        }
        public async Task RemoveSectionFromCustomerAsync(string customerId, string sectionId)
        {
            // sectionId düzgün GUID formatında olmalıdır
            if (!Guid.TryParse(sectionId, out var secGuid))
                throw new GlobalAppException("Section ID formatı düzgün deyil!");

            // əlaqəni tapırıq
            var link = await _sectionCustomerRead.GetAsync(
                sc => sc.AdminId == customerId && sc.SectionId == secGuid
            );
            if (link == null)
                throw new GlobalAppException("Belə bir bölmə-müştəri əlaqəsi tapılmadı!");

            // silinir
            await _sectionCustomerWrite.HardDeleteAsync(link);
            await _sectionCustomerWrite.CommitAsync();
        }

        public async Task<CustomerDto> UpdateCustomerAsync(UpdateCustomerDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id);
            if (user == null)
                throw new GlobalAppException("Müştəri tapılmadı!");

            // Telefon nömrəsini yeniləyirik
      

            user.Name = dto.Name ?? user.Name;
            user.Surname = dto.Surname ?? user.Surname;
            user.FinCode = dto.FinCode ?? user.FinCode;

            if (!string.IsNullOrWhiteSpace(dto.JobId)
                && Guid.TryParse(dto.JobId, out var jobGuid))
            {
                user.JobId = jobGuid;
            }

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var pwdRes = await _userManager.ResetPasswordAsync(user, token, dto.Password);
                if (!pwdRes.Succeeded)
                    throw new GlobalAppException($"Şifrə dəyişdirilə bilmədi: {string.Join(", ", pwdRes.Errors.Select(e => e.Description))}");
            }

            var updRes = await _userManager.UpdateAsync(user);
            if (!updRes.Succeeded)
                throw new GlobalAppException($"Müştəri güncəllənmədi: {string.Join(", ", updRes.Errors.Select(e => e.Description))}");

            // Section təyinatlarını da yeniləyə bilərik (əgər DTO-da gəlibsə)
           

            return await BuildCustomerDtoAsync(user);
        }

        public async Task<List<CustomerDto>> GetAllCustomersAsync()
        {
            var users = await _userManager.Users
                .Where(u => u.JobId != null)
                .ToListAsync();

            var list = new List<CustomerDto>();
            foreach (var u in users)
                list.Add(await BuildCustomerDtoAsync(u));

            return list;
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id)
                       ?? throw new GlobalAppException("Müştəri tapılmadı!");
            return await BuildCustomerDtoAsync(user);
        }

     

        public async Task DeleteCustomerAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id)
                       ?? throw new GlobalAppException("Müştəri tapılmadı!");
            var res = await _userManager.DeleteAsync(user);
            if (!res.Succeeded)
                throw new GlobalAppException("Müştəri silinmədi.");
        }

        public async Task AssignSectionsToCustomerAsync(string customerId, List<string> sectionIds)
        {
            var existing = await _sectionCustomerRead.GetAllAsync(sc => sc.AdminId == customerId);
            foreach (var sc in existing)
                await _sectionCustomerWrite.HardDeleteAsync(sc);
            await _sectionCustomerWrite.CommitAsync();

            foreach (var sid in sectionIds)
            {
                if (Guid.TryParse(sid, out var guid))
                {
                    await _sectionCustomerWrite.AddAsync(new SectionCustomer
                    {
                        AdminId = customerId,
                        SectionId = guid,
                        CreatedDate = DateTime.UtcNow,
                        LastUpdatedDate = DateTime.UtcNow
                    });
                }
            }
            await _sectionCustomerWrite.CommitAsync();
        }
        public async Task<List<CompanyDto>> GetCompaniesByCustomerAsync(string customerId)
        {
            // eyni qayda ilə əvvəlcə bütün linkləri gətir
            var links = await _sectionCustomerRead.GetAllAsync(
                sc => sc.AdminId == customerId,
                include: q => q
                   .Include(sc => sc.Section)
                   .ThenInclude(s => s.Department)
                   .ThenInclude(d => d.Company)
            );

            // şirkət obyektlərini distinct
            var comps = links
                .Select(sc => sc.Section.Department.Company)
                .DistinctBy(c => c.Id);

            return comps
                .Select(c => new CompanyDto
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Departments = new List<DepartmentDto>()
                })
                .ToList();
        }
        public async Task<List<SectionDto>> GetSectionsByCustomerAsync(string customerId, string departmentId)
        {
            if (!Guid.TryParse(departmentId, out var depGuid))
                throw new GlobalAppException("Department ID formatı düzgün deyil!");

            var links = await _sectionCustomerRead.GetAllAsync(
                sc => sc.AdminId == customerId
                      && sc.Section.DepartmentId == depGuid,
                include: q => q
                    .Include(sc => sc.Section)
                    .ThenInclude(s => s.Department)
                    .ThenInclude(d => d.Company)
            );

            return links
                .Select(sc => new SectionDto
                {
                    Id = sc.Section.Id.ToString(),
                    Name = sc.Section.Name,
                    DepartmentId = sc.Section.DepartmentId.ToString(),
                    DepartmentName = sc.Section.Department.Name,
                    CompanyName = sc.Section.Department.Company.Name,
                    Customers = null
                })
                .ToList();
        }

        public async Task<List<DepartmentDto>> GetDepartmentsByCustomerAsync(string customerId, string companyId)
        {
            if (!Guid.TryParse(companyId, out var compGuid))
                throw new GlobalAppException("Company ID formatı düzgün deyil!");

            // Müştəriyə bağlı bölmələrdən keçərək, onların şöbələrini (department) filtr edirik
            var links = await _sectionCustomerRead.GetAllAsync(
                sc => sc.AdminId == customerId
                      && sc.Section.Department.CompanyId == compGuid,
                include: q => q
                    .Include(sc => sc.Section)
                    .ThenInclude(s => s.Department)
                    .ThenInclude(d => d.Company)
            );

            var departments = links
                .Select(sc => sc.Section.Department)
                .GroupBy(d => d.Id)
                .Select(g => g.First());

            return departments
                .Select(d => new DepartmentDto
                {
                    Id = d.Id.ToString(),
                    Name = d.Name,
                    CompanyId = d.Company.Id.ToString(),
                    CompanyName = d.Company.Name,
                    Sections = null
                })
                .ToList();
        }

        public async Task ChangePasswordAsync(string customerId, ChangePasswordDto dto)
        {
            // 1) Yeni şifrə təsdiqini yoxla
            if (dto.NewPassword != dto.ConfirmPassword)
                throw new GlobalAppException("Yeni şifrə və təsdiq eyni deyil!");

            // 2) İstifadəçini tap
            var user = await _userManager.FindByIdAsync(customerId);
            if (user == null)
                throw new GlobalAppException("Müştəri tapılmadı!");

            // 3) Köhnə şifrəni yoxla və dəyiş
            var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new GlobalAppException($"Şifrə dəyişdirilə bilmədi: {errors}");
            }
        }
        // Köməkçi: Admin → CustomerDto çevirərkən bölmələri də yükləyir
        private async Task<CustomerDto> BuildCustomerDtoAsync(Admin user)
        {
            var secLinks = await _sectionCustomerRead.GetAllAsync(
                sc => sc.AdminId == user.Id,
                include: q => q
                    .Include(sc => sc.Section)
                    .ThenInclude(s => s.Department)
                    .ThenInclude(d => d.Company)
            );

            var sections = secLinks.Select(sc => new SectionDto
            {
                Id = sc.Section.Id.ToString(),
                Name = sc.Section.Name,
                DepartmentId = sc.Section.DepartmentId.ToString(),
                DepartmentName = sc.Section.Department.Name,
                CompanyName = sc.Section.Department.Company.Name,
                Customers = null
            }).ToList();

            var dto = _mapper.Map<CustomerDto>(user);
            dto.Sections = sections;
            return dto;
        }
    }
}
