using EventsGenerator.Entities;
using EventsGenerator.ExtraNeededClasses;
using EventsGenerator.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsGenerator.EventProcessorsInterfaces
{
    public interface ICasualAndSpeedSkatingEventGenerator
    {
        bool eventInList(Event eventToBeChecked, List<Event> events);
        void createEvents(List<Pairing> pairings);

        string getGenderForPairing(List<Schedule> schedules);

        int getMinimumAgeFromPairing(List<Schedule> schedules);

        int getMaximumAgeFromPairing(List<Schedule> schedules);

        double getStartHourFromPairing(List<Schedule> schedules);
        double getEndHourFromPairing(List<Schedule> schedules);

        Event createEventFromPairing(Pairing pairing, List<ParkTrail> parkTrails);

    }
}
