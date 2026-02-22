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

        var dtos = result.Models.Select(pc => new PcDto
        {
            PcId = pc.PcId,
            UserId = pc.UserId,
            Name = pc.Name,
            Cpu = pc.Cpu,
            Gpu = pc.Gpu,
            Note = pc.Note
        });
        return Ok(dtos);
    }
}