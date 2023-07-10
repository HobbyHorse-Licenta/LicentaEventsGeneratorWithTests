
namespace EventsGenerator.Entities
{
    public class ScheduleRefrence
    {
        public string Id { get; set; }
        public string? ScheduleId { get; set; }
        public string SkateProfileId { get; set; }
        public bool EventOwner { get; set; } = false;
        public bool YesVote { get; set; } = false;
    }
}
