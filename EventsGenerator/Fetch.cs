using EventsGenerator.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;
using EventsGenerator.JsonConverters;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Net.Http.Headers;
using Microsoft.Extensions.Hosting.Internal;
using System.Diagnostics;

namespace EventsGenerator
{
    public class Fetch: IFetch
    {
        private readonly static string apiUrl = Environment.GetEnvironmentVariable("HOBBYHORSE_API_BASE_URL");

       // private readonly static bool apiInDevelopment = false;
        static Fetch()
        {
            //if (apiInDevelopment == true)
            //{
            //    apiUrl = "https://localhost:7085";
            //}
            //else
            //{
            //    apiUrl = "https://hobby-horse-api.herokuapp.com";
            //}
        }

        public async Task<string> makeGetRequest(string url)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    //encoded in base32
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "JBXWEYTZJBXXE43FI5SW4ZLSMF2G64Q=");
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        return responseContent;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }


        public async Task<string> makePostRequest(string url, Object obj)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    //JsonSerializerOptions options = new JsonSerializerOptions();
                    //options.Converters.Add(new EventConverter());
                    //options.Converters.Add(new OutingConverter());
                    var jsonData = JsonSerializer.Serialize(obj);
                    var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                    //encoded in base32
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "JBXWEYTZJBXXE43FI5SW4ZLSMF2G64Q=");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        return responseContent;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<string> makePutRequest(string url, Object obj)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var jsonData = JsonSerializer.Serialize(obj);
                    var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                    //encoded in base32
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "JBXWEYTZJBXXE43FI5SW4ZLSMF2G64Q=");
                    HttpResponseMessage response = await httpClient.PutAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        return responseContent;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
                
            }
        }
        public async Task makeDeleteRequest(string url)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    //encoded in base32
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "JBXWEYTZJBXXE43FI5SW4ZLSMF2G64Q=");
                    HttpResponseMessage response = await httpClient.DeleteAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("DELETE succeeded");
                    }
                    else
                    {
                        Console.WriteLine($"DELETE failed with status code {response.StatusCode} and message {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        public async Task<List<Schedule>> getAllSchedules()
        {
            string url = $"{apiUrl}/schedule/allSchedules";
            var responseContent = await makeGetRequest(url);
            if(responseContent != null)
            {
                List<Schedule> schedules = JsonConvert.DeserializeObject<List<Schedule>>(responseContent);
                return schedules;
            }
            else
            {
                throw new Exception("Couldn't get schedules");
            }
        }

        public async Task<User> getUserWithBasicInfo(string userId)
        {
            string url = $"{apiUrl}/user/getBasicInfo/{userId}";
            var responseContent = await makeGetRequest(url);
            if (responseContent != null)
            {
                User user = JsonConvert.DeserializeObject<User>(responseContent);
                return user;
            }
            else
            {
                throw new Exception("Couldn't get basic info about user");
            }
        }

        public async Task<List<Event>> getAllEvents()
        {
            string url = $"{apiUrl}/event/getAllEvents";
            var responseContent = await makeGetRequest(url);
            if (responseContent != null)
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.Converters.Add(new EventConverter());
                options.Converters.Add(new OutingConverter());
                List<Event> events = JsonSerializer.Deserialize<List<Event>>(responseContent, options);

                return events;
            }
            else
            {
                throw new Exception("Couldn't get events");
            }
        }

        public async Task<List<ParkTrail>> getAllParkTrails()
        {
            string url = $"{apiUrl}/trail/allParkTrails";
            var responseContent = await makeGetRequest(url);
            if (responseContent != null)
            {
                List<ParkTrail> parkTrails = JsonConvert.DeserializeObject<List<ParkTrail>>(responseContent);
                return parkTrails;
            }
            else
            {
                throw new Exception("Couldn't get park trails");
            }
        }

        public async Task deleteSchedule(string scheduleId)
        {
            string url = $"{apiUrl}/schedule/delete/schedule/{scheduleId}";
            try
            {
                await makeDeleteRequest(url);
            }
            catch(Exception ex)
            {
                throw new Exception("Couldn't delete schedules");
            }
        }

        public async Task deleteEvent(string eventId)
        {
            string url = $"{apiUrl}/event/delete/{eventId}";
            try
            {
                await makeDeleteRequest(url);
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't delete event");
            }
        }

        public async Task<List<SkateProfile>> getAllSkateProfiles()
        {
            string url = $"{apiUrl}/skateProfile/getAll";
            var responseContent = await makeGetRequest(url);
            if (responseContent != null)
            {
                List<SkateProfile> skateProfiles = JsonConvert.DeserializeObject<List<SkateProfile>>(responseContent);
                return skateProfiles;
            }
            else
            {
                throw new Exception("Couldn't get skateProfiles");
            }
        }

        public async Task<SkateProfile> getSkateProfile(string skateProfileId)
        {
            string url = $"{apiUrl}/skateProfile/get/{skateProfileId}";
            var responseContent = await makeGetRequest(url);
            if (responseContent != null)
            {
                SkateProfile skateProfile = JsonConvert.DeserializeObject<SkateProfile>(responseContent);
                return skateProfile;
            }
            else
            {
                throw new Exception("Couldn't get skateProfile with specified id");
            }
        }

        public async Task<Event> PostEvent(Event evnt)
        {
            string url = $"{apiUrl}/event/post";
            var responseContent = await makePostRequest(url, evnt);
            if (responseContent != null)
            {
                Event postedEvent = JsonConvert.DeserializeObject<Event>(responseContent);
                return postedEvent;
            }
            else
            {
                throw new Exception("Couldn't post event");
            }
        }

        public async Task<Event> PutEvent(Event evnt)
        {
            string url = $"{apiUrl}/event/put";
            var responseContent = await makePutRequest(url, evnt);
            if (responseContent != null)
            {
                Event updatedEvent = JsonConvert.DeserializeObject<Event>(responseContent);
                return updatedEvent;
            }
            else
            {
                throw new Exception("Couldn't update event");
            }
        }


    }
}
