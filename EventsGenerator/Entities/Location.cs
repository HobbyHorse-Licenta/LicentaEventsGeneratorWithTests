namespace EventsGenerator.Entities
{
    public class Location
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }

        public List<Zone>? Zones { get; set; }
    }
}
