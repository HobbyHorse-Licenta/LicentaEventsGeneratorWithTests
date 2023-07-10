using EventsGenerator.Entities;
using EventsGenerator.EventProcessorsInterfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EventsGenerator.EventProcessors
{
    public class AggresiveSkatingController : IAggresiveSkatingController
    {
        public readonly IAggresiveSkating _aggresiveSkating;
        public readonly IFetch _fetch;
        public AggresiveSkatingController(IAggresiveSkating aggresiveSkating, IFetch fetch)
        {
            _aggresiveSkating = aggresiveSkating;
            _fetch = fetch;
        }
        public async Task createAggresiveEventFromJson(string aggresiveEvent)
        {
            AggresiveEvent skatingEvent = JsonConvert.DeserializeObject<AggresiveEvent>(aggresiveEvent);
            Event evnt = await _aggresiveSkating.completeAggresiveEvent(skatingEvent);
            if (evnt != null)
            {
                Console.WriteLine("Created and event from aggresive event");
                try
                {
                    Console.WriteLine("\n\n\n\nPOSTING:\n" + JsonSerializer.Serialize(evnt) + "\n\n\n\n");
                    _fetch.PostEvent(evnt);
                }
                catch (Exception)
                {
                    Console.WriteLine("Could not post just created aggresive skating event");
                }
            }
            else Console.WriteLine("Coudn't create event from aggresive event");

        }
        //public static void updateExistingEventsWithNewPossibleSkaters()
        //{
        //    List<Schedule> allSchedules = await Fetch.getAllSchedules();
        //    List<Event> existingEvents = await Fetch.getAllEvents();


        //}

        public async void addScheduleToExistingEvents(Schedule schedule)
        {
            List<Event> allEvents = await _fetch.getAllEvents();

            foreach (Event evnt in allEvents)
            {
                Console.WriteLine("Check if schedule is suitable then add it");
                Event updatedEvent = await _aggresiveSkating.UpdateAggresiveEventWithScheduleIfSuitable(schedule, evnt);
                if (updatedEvent == null)
                {
                    Console.WriteLine("We didn't update event, schedule was not appropriate for the event");
                }
                else
                {
                    try
                    {
                        _fetch.PutEvent(updatedEvent);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("PUT event failed");
                    }
                }
                
            }


        }
    }
}
