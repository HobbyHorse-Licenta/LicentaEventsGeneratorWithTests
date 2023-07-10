
namespace EventsGenerator.Entities
{
    public class AggresiveEvent
    {
        public AggresiveEvent()
        {

        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int MaxParticipants { get; set; }
        public string SkateExperience { get; set; }
        public List<Day> Days { get; set; }
        public List<ScheduleRefrence> ScheduleRefrences { get; set; } = new List<ScheduleRefrence>();
        public AggresiveOuting Outing { get; set; }
        public List<SkateProfile>? SkateProfiles { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public string Gender { get; set; }
        public int MinimumAge { get; set; }
        public int MaximumAge { get; set; }

    }
}
