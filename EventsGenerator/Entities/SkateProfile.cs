namespace EventsGenerator.Entities
{
    public class SkateProfile
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }

        public List<Event>? Events { get; set; }
        public List<Event>? RecommendedEvents { get; set; }
        public List<AssignedSkill>? AssignedSkills { get; set; }
        public List<Schedule>? Schedules { get; set; }
        public string SkateType { get; set; }
        public string SkatePracticeStyle { get; set; }
        public string SkateExperience { get; set; }
    }
}
