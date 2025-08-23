using HabiHamTracker.Data;
using HabiHamTracker.Models;
using HabiHamTracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HabiHamTracker.Services
{
    public class WeightService(AppDbContext db) : IWeightService
    {
        public async Task<List<WeightEntry>> GetAllAsync(DateTime? from, DateTime? to, CancellationToken ct)
        {
            var q = db.WeightEntries.AsNoTracking().AsQueryable();
            if (from is not null) q = q.Where(x => x.Date >= from);
            if (to is not null) q = q.Where(x => x.Date <= to);
            return await q.OrderByDescending(x => x.Date).ThenByDescending(x => x.Id).ToListAsync(ct);
        }

        public async Task<WeightEntry?> GetByIdAsync(int id, CancellationToken ct)
            => await db.WeightEntries.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task<WeightEntry?> GetLatestAsync(CancellationToken ct)
            => await db.WeightEntries.AsNoTracking()
                   .OrderByDescending(x => x.Date).ThenByDescending(x => x.Id)
                   .FirstOrDefaultAsync(ct);

        public async Task<WeightSummaryDto> GetSummaryAsync(int days, CancellationToken ct)
        {
            days = Math.Max(1, days);
            var from = DateTime.UtcNow.AddDays(-days);
            var weights = await db.WeightEntries.AsNoTracking()
                             .Where(x => x.Date >= from)
                             .Select(x => x.WeightKg)
                             .ToListAsync(ct);
            if (weights.Count == 0) return new WeightSummaryDto(days, 0, 0, 0, 0);
            var min = weights.Min();
            var max = weights.Max();
            var avg = Math.Round(weights.Average(), 2);
            return new WeightSummaryDto(days, weights.Count, min, max, avg);
        }

        public async Task<WeightEntry> CreateAsync(WeightCreateDto dto, CancellationToken ct)
        {
            if (dto.WeightKg <= 0) throw new ArgumentOutOfRangeException(nameof(dto.WeightKg), "WeightKg должен быть > 0.");
            var entity = new WeightEntry
            {
                Date = dto.Date == default ? DateTime.UtcNow : dto.Date,
                WeightKg = dto.WeightKg,
                CreatedAt = DateTime.UtcNow
            };
            db.WeightEntries.Add(entity);
            await db.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<WeightEntry?> UpdateAsync(int id, WeightUpdateDto dto, CancellationToken ct)
        {
            if (dto.WeightKg <= 0) throw new ArgumentOutOfRangeException(nameof(dto.WeightKg), "WeightKg должен быть > 0.");
            var entity = await db.WeightEntries.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null) return null;

            entity.Date = dto.Date == default ? entity.Date : dto.Date;
            entity.WeightKg = dto.WeightKg;

            await db.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var entity = await db.WeightEntries.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null) return false;
            db.Remove(entity);
            await db.SaveChangesAsync(ct);
            return true;
        }
    }
}
