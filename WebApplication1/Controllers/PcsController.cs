using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/pcs")]
public class PcsController : ControllerBase
{
    private readonly IPcService _service;

    public PcsController(IPcService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PcListDto>>> GetAll()
    {
        var pcs = await _service.GetAllAsync();
        return Ok(pcs);
    }

    [HttpGet("{id:int}/components")]
    public async Task<ActionResult<PcDetailsDto>> GetWithComponents(int id)
    {
        var pc = await _service.GetWithComponentsAsync(id);
        if (pc is null) return NotFound();
        return Ok(pc);
    }

    [HttpPost]
    public async Task<ActionResult<PcResponseDto>> Create([FromBody] PcCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetWithComponents), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] PcUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var ok = await _service.UpdateAsync(id, dto);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
