namespace EventsGenerator.Entities
{
    public class AggresiveOuting
    {
        public string Id { get; set; }
        public string EventId { get; set; }
        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public string SkatePracticeStyle { get; set; }
        public CustomTrail Trail { get; set; }
        public bool Booked { get; set; }

    }
}
