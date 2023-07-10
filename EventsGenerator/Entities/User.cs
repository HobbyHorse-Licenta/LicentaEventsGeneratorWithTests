namespace EventsGenerator.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string? PushNotificationToken { get; set; }
        public List<SkateProfile> SkateProfiles { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string ShortDescription { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
