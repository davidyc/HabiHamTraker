namespace HabiHamTracker.Models
{
    public class WeightEntry
    {
        public int Id { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
        
        public double WeightKg { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
