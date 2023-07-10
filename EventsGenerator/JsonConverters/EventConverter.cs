using EventsGenerator.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EventsGenerator.JsonConverters
{
    public class EventConverter : System.Text.Json.Serialization.JsonConverter<Event>
    {
        public override Event Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Event evnt = new Event();

            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            JsonElement rootElement = jsonDocument.RootElement;

            evnt.Id = rootElement.GetProperty("id").GetString();
            evnt.Name = rootElement.GetProperty("name").GetString();
            evnt.Note = rootElement.GetProperty("note").GetString();
            evnt.MaxParticipants = rootElement.GetProperty("maxParticipants").GetInt32();
            evnt.SkateExperience = rootElement.GetProperty("skateExperience").GetString();
            evnt.Gender = rootElement.GetProperty("gender").GetString();
            evnt.MinimumAge = rootElement.GetProperty("minimumAge").GetInt32();
            evnt.MaximumAge = rootElement.GetProperty("maximumAge").GetInt32();
            
            var scheduleRefrences = rootElement.GetProperty("scheduleRefrences").GetRawText();
            evnt.ScheduleRefrences = JsonConvert.DeserializeObject<List<ScheduleRefrence>>(scheduleRefrences);

            var recommendedSkateProfile = rootElement.GetProperty("recommendedSkateProfiles").GetRawText();
            evnt.RecommendedSkateProfiles = JsonConvert.DeserializeObject<List<SkateProfile>>(recommendedSkateProfile);

            var skateProfiles = rootElement.GetProperty("skateProfiles").GetRawText();
            evnt.SkateProfiles = JsonConvert.DeserializeObject<List<SkateProfile>>(skateProfiles);


            var outingJson = rootElement.GetProperty("outing").GetRawText();
            evnt.Outing = JsonSerializer.Deserialize<Outing>(outingJson, options);

            // evnt.RecommendedSkateProfiles = JsonSerializer.Deserialize<List<SkateProfile>>(recommendedSkateProfile);

            return evnt;
        }

        public override void Write(Utf8JsonWriter writer, Event value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            Console.WriteLine("AJUNG IN SERIALIZATORUL CUSTOM DE EVENT");
            writer.WriteString("id", value.Id);

            writer.WriteEndObject();
        }

    }
}
