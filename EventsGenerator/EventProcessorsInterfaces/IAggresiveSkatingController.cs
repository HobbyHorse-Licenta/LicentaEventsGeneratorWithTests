using EventsGenerator.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsGenerator.EventProcessorsInterfaces
{
    public interface IAggresiveSkatingController
    {
        Task createAggresiveEventFromJson(string aggresiveEvent);
        void addScheduleToExistingEvents(Schedule schedule);
    }
}
