using EventsGenerator.Entities;
using EventsGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsGenerator.EventProcessorsInterfaces
{
    public interface IAggresiveSkatingEventProcessor
    {
        ///"FILTERS" APPLIED ON SCHEDULE & EVENT
        bool isScheduleAgeCompatibleWithEvent(Event evnt, User eventOwner, Schedule schedule, User scheduleOwner);
        bool isScheduleExperienceCompatibleWithEvent(Event evnt, User eventOwner, Schedule schedule, User scheduleOwner);
        bool isScheduleGenderCompatibleWithEvent(Event evnt, User eventOwner, Schedule schedule, User scheduleOwner);
        Task<bool> isScheduleTimeCompatibleWithEvent(Event evnt, User eventOwner, Schedule schedule, User scheduleOwner);
        bool isScheduleDaysCompatibleWithEvent(Event evnt, User eventOwner, Schedule schedule, User scheduleOwner);

        ///FILTERS APPLIED ON SCHEDULES & AGGRESIVE EVENT
        List<Schedule> getSchedulesAgeCompatibleWithOwnerBothWays(AggresiveEvent aggresiveEvent, User eventOwner, List<Schedule> schedules);
        Task<List<Schedule>> getSchedulesExperienceCompatibleWithOwnerBothWays(AggresiveEvent aggresiveEvent, User eventOwner, List<Schedule> schedules);
        List<Schedule> getSchedulesGenderCompatibleWithOwnerBothWays(AggresiveEvent aggresiveEvent, User eventOwner, List<Schedule> schedules);
        Task<List<Schedule>> getSchedulesTimeCompatibleWithOwnerBothWays(AggresiveEvent aggresiveEvent, User eventOwner, List<Schedule> schedules);
        List<Schedule> getSchedulesDaysCompatibleWithOwnerBothWays(AggresiveEvent aggresiveEvent, User eventOwner, List<Schedule> schedules);



        ///FILTERS APPLIED ON SCHEDULES
        List<Schedule> getSchedulesFilteredBySkatingStyle(string skatingStyle, List<Schedule> schedules);
        List<Schedule> eliminateUserSchedules(User user, List<Schedule> schedules);
        List<Schedule> getSchedulesInRangeForCustomTrail(CustomTrail customTrail, List<Schedule> schedules);
        bool isScheduleInRangeForCustomTrail(CustomTrail customTrail, Schedule schedule);
    }
}
