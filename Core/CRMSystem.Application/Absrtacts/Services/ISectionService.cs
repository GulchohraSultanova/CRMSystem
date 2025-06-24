using CRMSystem.Application.Dtos.Section;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRMSystem.Application.Absrtacts.Services
{
    public interface ISectionService
    {
        /// <summary>
        /// Yeni bölmə yaradır.
        /// </summary>
        Task<SectionDto> CreateSectionAsync(CreateSectionDto dto);

        /// <summary>
        /// Bütün bölmələri qaytarır (silinməyənləri).
        /// </summary>
        Task<List<SectionDto>> GetAllSectionsAsync();

        /// <summary>
        /// ID-yə əsasən bölməni qaytarır.
        /// </summary>
        Task<SectionDto> GetSectionByIdAsync(string id);

        /// <summary>
        /// Verilən department ID-sinə əsasən bölmələri qaytarır.
        /// </summary>
        Task<List<SectionDto>> GetSectionsByDepartmentIdAsync(string departmentId);

        /// <summary>
        /// Mövcud bölməni güncəlləyir.
        /// </summary>
        Task<SectionDto> UpdateSectionAsync(UpdateSectionDto dto);

        /// <summary>
        /// Bölməni silir (soft delete).
        /// </summary>
        Task DeleteSectionAsync(string id);
    }
}
