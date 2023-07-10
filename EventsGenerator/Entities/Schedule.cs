namespace EventsGenerator.Entities
{
    public class Schedule
    {
        public string Id { get; set; }
        public List<Day> Days { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public string SkateProfileId { get; set; }
        public SkateProfile? SkateProfile { get; set; }
        public List<Zone> Zones { get; set; }

        public int? MinimumAge { get; set; }
        public int? MaximumAge { get; set; }
        public string Gender { get; set; }
        public int MaxNumberOfPeople { get; set; }
    }
}
