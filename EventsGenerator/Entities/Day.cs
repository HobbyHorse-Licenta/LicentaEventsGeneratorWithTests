
using System.Text.Json.Serialization;

namespace EventsGenerator.Entities
{
    public class Day
    {
        public string Id { get; set; }
        public int DayOfMonth { get; set; }
        //public string MinimumForm { get; set; }
        //public string ShortForm { get; set; }
        //public string LongForm { get; set; }

        //[JsonIgnore]
        //public List<Schedule>? Schedules { get; set; } = null;
    }
}
