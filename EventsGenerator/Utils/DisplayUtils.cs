using EventsGenerator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsGenerator.Utils
{
    public class DisplayUtils
    {
        public static string getStringDateFromTimeStamp(double timestamp)
        {
            //gets the date as a printable string from a timestamp variable as double.
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds((long)timestamp);
            DateTime localDateTime = dateTimeOffset.LocalDateTime;

            return localDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static void displayAggresiveEvent(AggresiveEvent evnt)
        {
            string info = "";
            info += $"StartTime: {evnt.Outing.StartTime} ";
            info += $"EndTime: {evnt.Outing.EndTime} ";
            info += $"gender: {evnt.Gender} ";
            info += $"maxParticipants: {evnt.MaxParticipants} ";
            info += $"age restriction: {evnt.MinimumAge}-{evnt.MaximumAge} ";
            info += $"days: ";
            foreach (Day day in evnt.Days)
            {
                info += $"{day.DayOfMonth.ToString()} ";
            }
            Console.WriteLine(info);
        }
        public static List<User> getUserListFromSkateProfiles(List<SkateProfile> skateProfiles)
        {
            List<User> users = new List<User>();
            HashSet<string> selectedUserIds = new HashSet<string>();

            foreach (SkateProfile skateProfile in skateProfiles)
            {
                string userId = skateProfile.UserId;
                if (!selectedUserIds.Contains(userId))
                {
                    users.Add(skateProfile.User);
                    selectedUserIds.Add(userId);
                }
            }

            return users;
        }
        public static void displayUsersWithTheirProfiles(List<SkateProfile> skateProfiles)
        {
            List<User> users = getUserListFromSkateProfiles(skateProfiles);

            List<SkateProfile> aggresiveSkateProfiles = skateProfiles.FindAll(skateProfile => skateProfile.SkatePracticeStyle == "Aggresive Skating");

            foreach (User user in users)
            {
                Console.WriteLine($"UserName: {user.Name}");
            }
        }

        public static void displaySchedulesAndUserData(List<Schedule> schedules)
        {
            string info;
            foreach (Schedule schedule in schedules)
            {
                info = "";
                if (schedule.SkateProfile != null)
                {
                    if (schedule.SkateProfile.User != null)
                    {
                        info += $"{schedule.SkateProfile.User.Name}: ";
                        info += $"{schedule.SkateProfile.User.Age}, ";
                        info += $"{schedule.SkateProfile.User.Gender}\n";
                    }
                    else info += $"schedule.SkateProfile.User: null ";
                    info += $"skatingStyle: {schedule.SkateProfile.SkatePracticeStyle}\n";
                }
                else info += $"skateProfile: null ";
                if (schedule.Days != null)
                {
                    info += $"days: ";
                    foreach (Day day in schedule.Days)
                    {
                        info += $"{day.DayOfMonth.ToString()} ";
                    }

                }
                if (schedule.EndTime != null && schedule.StartTime != null)
                {
                    info += "\ntimeRange " + getStringDateFromTimeStamp(schedule.StartTime) + " - " + getStringDateFromTimeStamp(schedule.EndTime) + "\n";
                }
                if (schedule.MinimumAge != null && schedule.MaximumAge != null)
                {
                    info += "ageRange " + schedule.MinimumAge + " - " + schedule.MaximumAge + "\n";
                }
                if (schedule.Gender != null)
                {
                    info += "allowed gender: " + schedule.Gender + "\n";
                }
                if (schedule.Zones != null && schedule.Zones.Count > 0)
                {
                    if (schedule.Zones[0].Location != null)
                    {

                        info += $"zone => location: {schedule.Zones[0].Location.Lat}, {schedule.Zones[0].Location.Long}";
                        info += $" range: {schedule.Zones[0].Range}\n";
                    }
                    else Console.WriteLine("Zone exist but there is no LOCATION");

                }
                else Console.WriteLine("NO ZONE SELECTED");
                info += $"\n\n\n";
                Console.WriteLine(info);
            }
        }
    }
}
