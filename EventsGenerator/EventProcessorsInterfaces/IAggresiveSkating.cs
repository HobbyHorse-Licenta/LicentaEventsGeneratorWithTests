using EventsGenerator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsGenerator.EventProcessorsInterfaces
{
    public interface IAggresiveSkating
    {
        ////MAIN FUNCTIONS
        Task<Event> completeAggresiveEvent(AggresiveEvent aggresiveEvent);
        Task<bool> JoinScheduleToAggresiveEvent(Schedule schedule, Event evnt);
        Task<Event> UpdateAggresiveEventWithScheduleIfSuitable(Schedule schedule, Event evnt);

        ////MEDIUM FUNCTIONS
        Task<List<Schedule>> findAppropriateSchedulesForPartialAggresiveEvent(AggresiveEvent aggresiveEvent, User eventOwner, List<Schedule> allExistingSchedules);
        Task<bool> isScheduleForAggresiveEvent(Event evnt, Schedule schedule);
    }
}
