using EventsGenerator.Entities;
using EventsGenerator.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventsGenerator
{
    public interface IFetch
    {
        Task<string> makeGetRequest(string url);

        Task<string> makePostRequest(string url, Object obj);
        Task<string> makePutRequest(string url, Object obj);
        Task makeDeleteRequest(string url);

        Task<List<Schedule>> getAllSchedules();

        Task<User> getUserWithBasicInfo(string userId);

        Task<List<Event>> getAllEvents();

        Task<List<ParkTrail>> getAllParkTrails();

        Task deleteSchedule(string scheduleId);

        Task deleteEvent(string eventId);

        Task<List<Event>> getAllEventsUserParticipatesTo(string userId);

        Task<List<SkateProfile>> getAllSkateProfiles();

        Task<SkateProfile> getSkateProfile(string skateProfileId);

        Task<Event> PostEvent(Event evnt);

        Task<Event> PutEvent(Event evnt);
    }
}
