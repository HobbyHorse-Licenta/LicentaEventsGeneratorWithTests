using EventsGenerator.JsonConverters;
using EventsGenerator.Utils;
using EventsGenerator.UtilsInterfaces;
using System.Reflection;
using System.Text.Json.Serialization;

namespace EventsGenerator.Entities
{
    [JsonConverter(typeof(OutingConverter))]
    public class Outing
    {
        public readonly IProcessingUtils _processingUtils;
        public Outing()
        {

        }
        public Outing(IProcessingUtils processingUtils)
        {
            _processingUtils = processingUtils;
        }
        public string Id { get; set; }
        public string EventId { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public List<Day> Days { get; set; }

        //public Day? VotedDay { get; set; }
        public string? VotedDayId { get; set; }
        public double VotedStartTime { get; set; }


        public string SkatePracticeStyle { get; set; }
        public Trail Trail { get; set; }
        public bool Booked { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Outing comparingOuting = (Outing)obj;

            if (StartTime != comparingOuting.StartTime)
                return false;

            if (EndTime != comparingOuting.EndTime)
                return false;

            if (!SkatePracticeStyle.Equals(comparingOuting.SkatePracticeStyle))
                return false;

            //compare trail by Id since the trail is unique
            if (!Trail.Id.Equals(comparingOuting.Trail.Id))
                return false;

            //compare days
            if(!daysArraysAreEqual(Days, comparingOuting.Days))
                return false;

            return true;
        }

        private bool daysArraysAreEqual(List<Day> dayArray1, List<Day> dayArray2)
        {
            List<int> allDaysInWeek = _processingUtils.getDaysForEntireWeek();
            List<Day> filteredDayArray1 = dayArray1.FindAll(day => allDaysInWeek.Contains(day.DayOfMonth));
            List<Day> filteredDayArray2 = dayArray2.FindAll(day => allDaysInWeek.Contains(day.DayOfMonth));

            dayArray1 = filteredDayArray1;
            dayArray2 = filteredDayArray2;

             if (dayArray1.Count != dayArray2.Count)
                return false;

            List<Day> sortedDayArray1 = dayArray1.OrderBy(day => day.DayOfMonth).ToList();
            List<Day> sortedDayArray2 = dayArray2.OrderBy(day => day.DayOfMonth).ToList();

            for(int i = 0; i < sortedDayArray1.Count; i++)
            {
                if (sortedDayArray1[i].DayOfMonth != sortedDayArray2[i].DayOfMonth)
                    return false;
            }

            return true;
        }

    }
}
