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

    // GET entire PC table
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _supabase.From<Pc>()
        .Order(pc => pc.PcId, Supabase.Postgrest.Constants.Ordering.Ascending)
        .Get();

        var dto = result.Models.Select(pc => new PcDto
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
}