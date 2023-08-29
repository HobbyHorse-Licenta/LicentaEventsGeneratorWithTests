using EventsGenerator.Entities;
using EventsGenerator.EventProcessorsInterfaces;
using EventsGenerator.ExtraNeededClasses;
using EventsGenerator.ExtraNeededClasses;
using EventsGenerator.Utils;
using EventsGenerator.UtilsInterfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsGenerator.EventProcessors
{
    public class CasualAndSpeedSkatingEventGenerator : ICasualAndSpeedSkatingEventGenerator
    {
        public readonly IFetch _fetch;
        public readonly IProcessingUtils _processingUtils;
        public CasualAndSpeedSkatingEventGenerator(IFetch fetch, IProcessingUtils processingUtils)
        {
            _fetch = fetch;
            _processingUtils = processingUtils;
        }
        public  bool eventInList(Event eventToBeChecked, List<Event> events)
        {
            foreach (Event listEvent in events)
            {
                if (eventToBeChecked.Equals(listEvent))
                {
                    return true;
                }
            }
            return false;
        }
        public  async void createEvents(List<Pairing> pairings)
        {
            try
            {
                List<Event> existingEvents = await _fetch.getAllEvents();
                List<ParkTrail> allParkTrails = await _fetch.getAllParkTrails();

                foreach (Pairing pairing in pairings)
                {
                    Console.WriteLine($"Trying to create an event from pairing");
                    Event createdEvent = createEventFromPairing(pairing, allParkTrails);
                    if (createdEvent != null)
                    {
                        if (!eventInList(createdEvent, existingEvents))
                        {
                            //if it doesn't exist already
                            try
                            {
                                Console.WriteLine("Posting created event:\n");
                                _fetch.PostEvent(createdEvent);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else Console.WriteLine("Event already exists");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public  string getGenderForPairing(List<Schedule> schedules)
        {
            int male = 0;
            int female = 0;
            schedules.ForEach((schedule) =>
            {
                if (schedule.SkateProfile != null && schedule.SkateProfile.User != null &&
                schedule.SkateProfile.User.Gender != null)
                {
                    if (schedule.SkateProfile.User.Gender == "Male")
                        male += 1;
                    else female += 1;
                }
            });

            string gender = "Mixed";
            if (male == 0)
                gender = "Female";
            if (female == 0)
                gender = "Male";

            return gender;
        }

        public  int getMinimumAgeFromPairing(List<Schedule> schedules)
        {
            int minimumAge = int.MaxValue;
            foreach (Schedule schedule in schedules)
            {
                if (schedule.SkateProfile.User.Age < minimumAge)
                    minimumAge = schedule.SkateProfile.User.Age;
            }

            return minimumAge;
        }

        public  int getMaximumAgeFromPairing(List<Schedule> schedules)
        {
            int maximumAge = 0;
            foreach (Schedule schedule in schedules)
            {
                if (schedule.SkateProfile.User.Age > maximumAge)
                    maximumAge = schedule.SkateProfile.User.Age;
            }

            return maximumAge;
        }

        public  double getStartHourFromPairing(List<Schedule> schedules)
        {
            double startHour = 0;
            foreach (Schedule schedule in schedules)
            {
                if (_processingUtils.compareTimeStamps(schedule.StartTime, startHour) > 0 )
                    startHour = schedule.StartTime;
            }

            return startHour;
        }
        public  double getEndHourFromPairing(List<Schedule> schedules)
        {
            double endHour = schedules[0].EndTime;
            foreach (Schedule schedule in schedules)
            {
                if (_processingUtils.compareTimeStamps(schedule.EndTime, endHour) < 0)
                    endHour = schedule.EndTime;
            }
            return endHour;
        }

        public  Event createEventFromPairing(Pairing pairing, List<ParkTrail> parkTrails)
        {
            if (pairing.Schedules != null && pairing.Schedules.Count > 0)
            {

                List<ParkTrail> commonParkTrails = _processingUtils.getCommonParkTrailsFromSchedules(pairing.Schedules, parkTrails);
                if (commonParkTrails.Count > 0)
                {
                    Random random = new Random();
                    int randomTrailIndex = random.Next(0, commonParkTrails.Count);

                    string EventId = Guid.NewGuid().ToString();

                    List<ScheduleRefrence> scheduleRefrences = new List<ScheduleRefrence>();
                    foreach(Schedule schedule in pairing.Schedules)
                    {
                        ScheduleRefrence refrence = new ScheduleRefrence()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ScheduleId = schedule.Id,
                            SkateProfileId = schedule.SkateProfile.Id,
                            EventOwner = false,
                            YesVote = false,
                        };

                        scheduleRefrences.Add(refrence);
                    }

                    //create event
                    Outing outing = new Outing
                    {
                        Id = Guid.NewGuid().ToString(),
                        EventId = EventId,
                        SkatePracticeStyle = pairing.Schedules[0].SkateProfile.SkatePracticeStyle,
                        StartTime = getStartHourFromPairing(pairing.Schedules),
                        EndTime = getEndHourFromPairing(pairing.Schedules),
                        Booked = false,
                        Days = _processingUtils.getCommonDaysFromSchedules(pairing.Schedules),
                        Trail = parkTrails[randomTrailIndex]
                    };

                    Event createdEvent = new Event
                    {
                        Id = EventId,
                        Gender = getGenderForPairing(pairing.Schedules),
                        MinimumAge = getMinimumAgeFromPairing(pairing.Schedules),
                        MaximumAge = getMaximumAgeFromPairing(pairing.Schedules),
                        ScheduleRefrences = scheduleRefrences,
                        MaxParticipants = pairing.Schedules.Count,
                        Name = "Event",
                        Note = "There is no note yet",
                        RecommendedSkateProfiles = _processingUtils.gettingSkateProfilesFromSchedules(pairing.Schedules),
                        SkateExperience = "Advanced Begginer", //TODO: left to find out
                        Outing = outing
                    };

                    return createdEvent;
                }
                else return null;
            }
            else return null;
        }

    }
}
