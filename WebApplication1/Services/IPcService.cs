using WebApplication1.DTOs;

namespace WebApplication1.Services;

public interface IPcService
{
    Task<IEnumerable<PcListDto>> GetAllAsync();
    Task<PcDetailsDto?> GetWithComponentsAsync(int id);
    Task<PcResponseDto> CreateAsync(PcCreateDto dto);
    Task<bool> UpdateAsync(int id, PcUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
