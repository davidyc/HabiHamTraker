using HabiHamTracker.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HabiHamTracker.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class WeightController(IWeightService svc) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] DateTime? from, [FromQuery] DateTime? to, CancellationToken ct)
            => Ok(await svc.GetAllAsync(from, to, ct));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
            => (await svc.GetByIdAsync(id, ct)) is { } e ? Ok(e) : NotFound();

        [HttpGet("latest")]
        public async Task<IActionResult> Latest(CancellationToken ct)
            => (await svc.GetLatestAsync(ct)) is { } e ? Ok(e) : NotFound();

        [HttpGet("summary")]
        public async Task<IActionResult> Summary([FromQuery] int days = 30, CancellationToken ct = default)
            => Ok(await svc.GetSummaryAsync(days, ct));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WeightCreateDto dto, CancellationToken ct)
            => CreatedAtAction(nameof(GetById), new { id = (await svc.CreateAsync(dto, ct)).Id }, dto);

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] WeightUpdateDto dto, CancellationToken ct)
            => (await svc.UpdateAsync(id, dto, ct)) is { } e ? Ok(e) : NotFound();

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
            => await svc.DeleteAsync(id, ct) ? NoContent() : NotFound();
    }
}
