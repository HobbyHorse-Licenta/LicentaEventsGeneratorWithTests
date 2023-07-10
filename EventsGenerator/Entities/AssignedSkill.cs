namespace EventsGenerator.Entities
{
    public class AssignedSkill
    {
        public string Id { get; set; }

        public string SkateProfileId { get; set; }
        public SkateProfile? SkateProfile { get; set; }
        public Skill? Skill { get; set; }
        public string SkillId { get; set; }
        public string MasteringLevel { get; set; }

    }
}
