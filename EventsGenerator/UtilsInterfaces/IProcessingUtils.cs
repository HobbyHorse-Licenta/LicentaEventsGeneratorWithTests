using EventsGenerator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsGenerator.UtilsInterfaces
{
    public interface IProcessingUtils
    {
        bool areGenderCompatible(Schedule schedule1, Schedule schedule2);

        int compareTimeStamps(double timestamp1, double timestamp2);
        bool schedulesHaveCloseExperienceLevels(Schedule schedule1, Schedule schedule2);

        bool areTimeCompatible(Schedule schedule1, Schedule schedule2);
        bool areAllSchedulesTimeCompatible(List<Schedule> schedules, List<int> pairing);
        List<int> getDaysForEntireWeek();
        bool areAgeCompatible(Schedule schedule1, Schedule schedule2);
        bool scheduleOwnerIsAlsoEventOwner(Schedule schedule, Event evnt);
        string getAggresiveEventsOwnerSkateProfileId(Event evnt);

        List<Day> getCommonDaysFromSchedules(List<Schedule> schedules);

        List<Day> getCommonDaysBetweenScheduleAndEvent(Schedule schedule, Event evnt);

        string getUsersAsStringFromSchedules(List<Schedule> schedules);

        List<SkateProfile> gettingSkateProfilesFromSchedules(List<Schedule> schedules);

        List<ParkTrail> getCommonParkTrailsFromSchedules(List<Schedule> schedules, List<ParkTrail> parkTrails);
        bool checkIfLocationInZone(Location location, Zone zone);

        double DegreeToRadian(double degree);

        double getTimeAsDoubleFromTimestamp(double timestamp);
        double getTimeAsDoubleFromDate(DateTime date);
        DateTime getDateFromTimeStamp(double timestamp);
        bool scheduleIsExpired(Schedule schedule);

        bool eventIsExpired(Event evnt);
    }
}
