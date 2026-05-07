using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PcController : ControllerBase
{
    private readonly Supabase.Client _supabase;

    public PcController(Supabase.Client supabase)
    {
        _supabase = supabase;
    }

    // GET PC table
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        var result = await _supabase.From<Pc>()
            .Order(pc => pc.PcId, Supabase.Postgrest.Constants.Ordering.Ascending)
            .Get();

        var keywords = string.IsNullOrEmpty(search)
            ? Array.Empty<string>()
            : search.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var dto = result.Models
            .Where(pc => keywords.Length == 0 || keywords.All(keyword =>
                (pc.Name != null && pc.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                (pc.Cpu != null && pc.Cpu.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                (pc.Gpu != null && pc.Gpu.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                (pc.Note != null && pc.Note.Contains(keyword, StringComparison.OrdinalIgnoreCase))))
            .Select(pc => new PcDto
            {
                PcId = pc.PcId,
                UserId = pc.UserId,
                Name = pc.Name,
                Cpu = pc.Cpu,
                Gpu = pc.Gpu,
                Note = pc.Note
            });

        return Ok(dto);
    }

    // GET a single PC by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _supabase.From<Pc>()
            .Where(pc => pc.PcId == id)
            .Single();

        if (result == null) return NotFound();

        var dto = new PcDto
        {
            PcId = result.PcId,
            UserId = result.UserId,
            Name = result.Name,
            Cpu = result.Cpu,
            Gpu = result.Gpu,
            Note = result.Note
        };
        return Ok(dto);
    }

    // PUT /api/pc/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] PcDto dto)
    {
        var existing = await _supabase.From<Pc>()
            .Where(pc => pc.PcId == id)
            .Single();

        if (existing == null) return NotFound();

        existing.Name = dto.Name;
        existing.Cpu = dto.Cpu;
        existing.Gpu = dto.Gpu;
        existing.Note = dto.Note;

        await _supabase.From<Pc>()
            .Where(pc => pc.PcId == id)
            .Update(existing);

        return Ok(dto);
    }
}