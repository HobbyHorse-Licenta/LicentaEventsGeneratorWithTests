namespace EventsGenerator.Entities
{
    public class CheckPoint
    {
        public string Id { get; set; }

        public string? Name { get; set; }
        public int Order { get; set; }
        public string CustomTrailId { get; set; }
        public Location Location { get; set; }
    }
}
