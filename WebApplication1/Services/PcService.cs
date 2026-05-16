using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Entities;

namespace WebApplication1.Services;

public class PcService : IPcService
{
    private readonly AppDbContext _db;

    public PcService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<PcListDto>> GetAllAsync()
    {
        return await _db.PCs
            .AsNoTracking()
            .Select(p => new PcListDto
            {
                Id = p.Id,
                Name = p.Name,
                Weight = p.Weight,
                Warranty = p.Warranty,
                CreatedAt = p.CreatedAt,
                Stock = p.Stock
            })
            .ToListAsync();
    }

    public async Task<PcDetailsDto?> GetWithComponentsAsync(int id)
    {
        var pc = await _db.PCs
            .AsNoTracking()
            .Include(p => p.PCComponents)
                .ThenInclude(pc => pc.Component)
                    .ThenInclude(c => c.Manufacturer)
            .Include(p => p.PCComponents)
                .ThenInclude(pc => pc.Component)
                    .ThenInclude(c => c.Type)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pc is null) return null;

        return new PcDetailsDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock,
            Components = pc.PCComponents.Select(pcc => new PcComponentDto
            {
                Amount = pcc.Amount,
                Component = new ComponentDto
                {
                    Code = pcc.Component.Code,
                    Name = pcc.Component.Name,
                    Description = pcc.Component.Description,
                    Manufacturer = new ManufacturerDto
                    {
                        Id = pcc.Component.Manufacturer.Id,
                        Abbreviation = pcc.Component.Manufacturer.Abbreviation,
                        FullName = pcc.Component.Manufacturer.FullName,
                        FoundationDate = pcc.Component.Manufacturer.FoundationDate
                    },
                    Type = new TypeDto
                    {
                        Id = pcc.Component.Type.Id,
                        Abbreviation = pcc.Component.Type.Abbreviation,
                        Name = pcc.Component.Type.Name
                    }
                }
            }).ToList()
        };
    }

    public async Task<PcResponseDto> CreateAsync(PcCreateDto dto)
    {
        var pc = new PC
        {
            Name = dto.Name,
            Weight = dto.Weight,
            Warranty = dto.Warranty,
            CreatedAt = dto.CreatedAt,
            Stock = dto.Stock
        };

        _db.PCs.Add(pc);
        await _db.SaveChangesAsync();

        return new PcResponseDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock
        };
    }

    public async Task<bool> UpdateAsync(int id, PcUpdateDto dto)
    {
        var pc = await _db.PCs.FirstOrDefaultAsync(p => p.Id == id);
        if (pc is null) return false;

        pc.Name = dto.Name;
        pc.Weight = dto.Weight;
        pc.Warranty = dto.Warranty;
        pc.CreatedAt = dto.CreatedAt;
        pc.Stock = dto.Stock;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var pc = await _db.PCs.FirstOrDefaultAsync(p => p.Id == id);
        if (pc is null) return false;

        _db.PCs.Remove(pc);
        await _db.SaveChangesAsync();
        return true;
    }
}
