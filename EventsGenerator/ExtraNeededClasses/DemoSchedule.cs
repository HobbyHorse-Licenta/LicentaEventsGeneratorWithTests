using EventsGenerator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsGenerator.ExtraNeededClasses
{
    public class DemoSchedule
    {
        public string Id { get; set; }
        public List<Day> Days { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public int UserAge { get; set; }
        public string UserGender { get; set; }
        public int MinimumAge { get; set; }
        public int MaximumAge { get; set; }
        public string GenderAllowance { get; set; }
        public int MaxNumberOfPeople { get; set; }
    }
}
