using EventsGenerator.Entities;
using EventsGenerator.UtilsInterfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventsGenerator.Utils
{
    public class ProcessingUtils : IProcessingUtils
    {
        public bool areGenderCompatible(Schedule schedule1, Schedule schedule2)
        {
            if (schedule1.Gender != "Mixed")
            {
                if (schedule1.Gender != schedule2.SkateProfile.User.Gender)
                    return false;
            }

            if (schedule2.Gender != "Mixed")
            {
                if (schedule2.Gender != schedule1.SkateProfile.User.Gender)
                    return false;
            }

            return true;
        }

        /*
         * Return < 0 if timestamp 1 is smaller
         * Return < 0 if timestamp 1 is bigger
         * Return = 0 if timestamps are equal
         */
        public int compareTimeStamps(double timestamp1, double timestamp2)
        {
            TimeSpan timeOfDay1 = new DateTime((long)timestamp1 * 10000).TimeOfDay;
            TimeSpan timeOfDay2 = new DateTime((long)timestamp2 * 10000).TimeOfDay;

            return TimeSpan.Compare(timeOfDay1, timeOfDay2);
        }
        public bool schedulesHaveCloseExperienceLevels(Schedule schedule1, Schedule schedule2)
        {
            Dictionary<string, int> experienceLevelDictionary = new Dictionary<string, int>()
            {
                {"Begginer",1 },
                {"Advanced Begginer", 2 },
                {"Intermediate",3 },
                {"Advanced",4 }
            };
            if (Math.Abs(experienceLevelDictionary[schedule1.SkateProfile.SkateExperience] - experienceLevelDictionary[schedule2.SkateProfile.SkateExperience]) > 1)
            {
                return false;
            }

            return true;
        }


        public bool areTimeCompatible(Schedule schedule1, Schedule schedule2)
        {
            //if ( schedule1.EndTime < schedule2.StartTime || schedule2.EndTime < schedule1.StartTime)
            if (compareTimeStamps(schedule1.EndTime, schedule2.StartTime) < 0 || compareTimeStamps(schedule2.EndTime, schedule1.StartTime) < 0)
                return false;
            else return true;
        }
        public bool areAllSchedulesTimeCompatible(List<Schedule> schedules, List<int> pairing)
        {
            double maxStartTime = 0;
            double minEndTime = double.MaxValue;

            foreach (int scheduleIndex in pairing)
            {
                Schedule schedule = schedules[scheduleIndex];
                double startTime = getTimeAsDoubleFromTimestamp(schedule.StartTime);
                double endTime = getTimeAsDoubleFromTimestamp(schedule.EndTime);

                if (startTime > maxStartTime)
                    maxStartTime = startTime;

                if (endTime < minEndTime)
                    minEndTime = endTime;
            }

            if (maxStartTime > minEndTime)
                return false;

            return true;
        }
        public List<int> getDaysForEntireWeek()
        {
            List<int> weekDays = new List<int>();
            DateTime today = DateTime.Now;
            DateTime aWeekFromNow = today.AddDays(7);
            for (DateTime date = today; date < aWeekFromNow; date = date.AddDays(1))
            {
                weekDays.Add(date.Day);
            }
            return weekDays;
        }
        public bool areAgeCompatible(Schedule schedule1, Schedule schedule2)
        {
            int schedule2UserAge = schedule2.SkateProfile.User.Age;
            int schedule1UserAge = schedule1.SkateProfile.User.Age;

            if (schedule1UserAge < schedule2.MinimumAge || schedule1UserAge > schedule2.MaximumAge)
                return false;

            if (schedule2UserAge < schedule1.MinimumAge || schedule2UserAge > schedule1.MaximumAge)
                return false;

            return true;
        }

        public bool scheduleOwnerIsAlsoEventOwner(Schedule schedule, Event evnt)
        {
            string skateProfileIdOfOwner = getAggresiveEventsOwnerSkateProfileId(evnt);
            if(skateProfileIdOfOwner == null)
            {
                string errorMessage = "Coudn't get owner of event";
                Console.WriteLine(errorMessage);
                throw new Exception(errorMessage);
            }
            else
            {
                if (schedule.SkateProfile.Id.Equals(skateProfileIdOfOwner) == true)
                    return true;
                else return false;
            }
        }

        public string getAggresiveEventsOwnerSkateProfileId(Event evnt)
        {
            if(evnt != null)
            {
                if(evnt.Outing.SkatePracticeStyle == "Aggresive Skating")
                {
                    if (evnt.ScheduleRefrences != null && evnt.ScheduleRefrences.Count > 0)
                    {
                        foreach (ScheduleRefrence scheduleRefrence in evnt.ScheduleRefrences)
                        {
                            if (scheduleRefrence.EventOwner == true)
                            {
                                return scheduleRefrence.SkateProfileId;
                            }
                        }
                    }
                    else Console.WriteLine("Event was not created properly; Missing scheduleRefrences");
                }
                else Console.WriteLine("'getAggresiveEventsOwnerId' only works for aggresive skating events");
            }
            else Console.WriteLine("A proper event was not passed in 'getEventsOwnerId'");

            return null;
        }


        public List<Day> getCommonDaysFromSchedules(List<Schedule> schedules)
        {
            List<Day> commonDays = new List<Day>();

            List<int> weekDays = getDaysForEntireWeek();

            foreach (int dayToCheck in weekDays)
            {
                bool isCommonDay = true;
                foreach (Schedule schedule in schedules)
                {
                    bool hasThisDay = false;
                    foreach (Day day in schedule.Days)
                    {
                        if (day.DayOfMonth == dayToCheck)
                            hasThisDay = true;
                    }
                    if (hasThisDay == false)
                        isCommonDay = false;
                }
                if (isCommonDay == true)
                {
                    commonDays.Add(new Day
                    {
                        Id = Guid.NewGuid().ToString(),
                        DayOfMonth = dayToCheck
                    });
                }
            }
            return commonDays;
        }

        public List<Day> getCommonDaysBetweenScheduleAndEvent(Schedule schedule, Event evnt)
        {
            List<Day> commonDays = new List<Day>();

            List<int> weekDays = getDaysForEntireWeek();

            foreach (int dayToCheck in weekDays)
            {
                bool scheduleHasThisDay = false;
                bool eventHasThisDay = false;

                foreach (Day day in schedule.Days)
                {
                    if (day.DayOfMonth == dayToCheck)
                        scheduleHasThisDay = true;
                }

                foreach (Day day in evnt.Outing.Days)
                {
                    if (day.DayOfMonth == dayToCheck)
                        eventHasThisDay = true;
                }

                if(eventHasThisDay == true && scheduleHasThisDay == true)
                {
                    //this day is common
                    commonDays.Add(new Day
                    {
                        Id = Guid.NewGuid().ToString(),
                        DayOfMonth = dayToCheck
                    });
                }
            }
            return commonDays;
        }

        public string getUsersAsStringFromSchedules(List<Schedule> schedules)
        {
            string res = "";
            bool first = true;
            foreach (Schedule schedule in schedules)
            {
                if (first)
                {
                    first = false;
                }
                else res += ", ";
                res += schedule.SkateProfile.User.Name;
            }

            return res;
        }

        public List<SkateProfile> gettingSkateProfilesFromSchedules(List<Schedule> schedules)
        {
            List<SkateProfile> skateProfiles = new List<SkateProfile>();

            foreach (Schedule schedule in schedules)
            {
                if (schedule.SkateProfile != null)
                    skateProfiles.Add(schedule.SkateProfile);
            }

            return skateProfiles;
        }

        public List<ParkTrail> getCommonParkTrailsFromSchedules(List<Schedule> schedules, List<ParkTrail> parkTrails)
        {
            List<ParkTrail> commonParkTrails = new List<ParkTrail>();
            foreach (ParkTrail parkTrail in parkTrails)
            {
                bool parkTrailCommon = true;
                foreach (Schedule schedule in schedules)
                {
                    if (checkIfLocationInZone(parkTrail.Location, schedule.Zones[0]) == false)
                    {
                        parkTrailCommon = false;
                    }
                }
                if (parkTrailCommon == true)
                {
                    commonParkTrails.Add(parkTrail);
                }
            }
            return commonParkTrails;
        }
        public bool checkIfLocationInZone(Location location, Zone zone)
        {
            //Distance on a sphere
            //Haversine Formula

            double earthRadiusKm = 6371; // Approximate radius of the Earth in kilometers

            double dLat = DegreeToRadian(location.Lat - zone.Location.Lat);
            double dLon = DegreeToRadian(location.Long - zone.Location.Long);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreeToRadian(zone.Location.Lat)) * Math.Cos(DegreeToRadian(location.Lat)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distanceInKm = earthRadiusKm * c;

            if (distanceInKm <= zone.Range)
                return true;
            else return false;
        }

        public double DegreeToRadian(double degree)
        {
            return degree * Math.PI / 180;
        }

        public double getTimeAsDoubleFromTimestamp(double timestamp)
        {
            DateTime date = getDateFromTimeStamp(timestamp);
            return getTimeAsDoubleFromDate(date);
        }
        public double getTimeAsDoubleFromDate(DateTime date)
        {
            return date.TimeOfDay.TotalMinutes;
        }
        public DateTime getDateFromTimeStamp(double timestamp)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds((long)timestamp);
            return dateTimeOffset.LocalDateTime;
        }
        public bool scheduleIsExpired(Schedule schedule)
        {

            List<int> weekDays = getDaysForEntireWeek();
            int firstDayInWeek = weekDays[0];

            //check if schedule contains days in current week
            bool hasDaysInCurrentWeek = false;
            int lastDayInSchedule = -1;
            foreach (int weekDay in weekDays)
            {
                Day foundDay = schedule.Days.Find(day => day.DayOfMonth == weekDay);
                if (foundDay != null)
                {
                    lastDayInSchedule = weekDay;
                    hasDaysInCurrentWeek = true;
                }
            }


            if (hasDaysInCurrentWeek == false)
            {
                //schedule has no days in the current week
                return true;
            }

            if (lastDayInSchedule == firstDayInWeek)
            {
                //last day of schedule is the current day
                //check if schedule is expired bu time
                double currentTime = getTimeAsDoubleFromDate(DateTime.Now);
                double endTimeInSchedule = getTimeAsDoubleFromTimestamp(schedule.EndTime);

                if (currentTime > endTimeInSchedule)
                    return true;
            }

            return false;
        }

        public bool eventIsExpired(Event evnt)
        {

            List<int> weekDays = getDaysForEntireWeek();
            int firstDayInWeek = weekDays[0];

            //check if schedule contains days in current week
            bool hasDaysInCurrentWeek = false;
            int lastDayInEvent = -1;
            foreach (int weekDay in weekDays)
            {
                Day foundDay = evnt.Outing.Days.Find(day => day.DayOfMonth == weekDay);
                if (foundDay != null)
                {
                    lastDayInEvent = weekDay;
                    hasDaysInCurrentWeek = true;
                }
            }


            if (hasDaysInCurrentWeek == false)
            {
                //event has no days in the current week
                return true;
            }

            if (lastDayInEvent == firstDayInWeek)
            {
                //last day of event is the current day
                //check if schedule is expired bu time
                double currentTime = getTimeAsDoubleFromDate(DateTime.Now);
                double endTimeInEvent = getTimeAsDoubleFromTimestamp(evnt.Outing.EndTime);

                if (currentTime > endTimeInEvent)
                    return true;
            }

            return false;
        }
    }
}
