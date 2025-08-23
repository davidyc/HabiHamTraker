using HabiHamTracker.Models;

namespace HabiHamTracker.Services.Interfaces
{
    public record WeightCreateDto(DateTime Date, double WeightKg);
    public record WeightUpdateDto(DateTime Date, double WeightKg);
    public record WeightSummaryDto(int Days, int Count, double Min, double Max, double Avg);

    public interface IWeightService
    {
        Task<List<WeightEntry>> GetAllAsync(DateTime? from, DateTime? to, CancellationToken ct);
        Task<WeightEntry?> GetByIdAsync(int id, CancellationToken ct);
        Task<WeightEntry?> GetLatestAsync(CancellationToken ct);
        Task<WeightSummaryDto> GetSummaryAsync(int days, CancellationToken ct);
        Task<WeightEntry> CreateAsync(WeightCreateDto dto, CancellationToken ct);
        Task<WeightEntry?> UpdateAsync(int id, WeightUpdateDto dto, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
