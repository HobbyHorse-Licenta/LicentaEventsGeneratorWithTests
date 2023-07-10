namespace EventsGenerator.Entities
{
    public class ParkTrail : Trail
    {
        public string PracticeStyle { get; set; }
        public string? PracticeStyle2 { get; set; }
        public int? Capacity { get; set; }
        public Location Location { get; set; }

        public int OpeningHour { get; set; }
        public int ClosingHour { get; set; }

    }
}
