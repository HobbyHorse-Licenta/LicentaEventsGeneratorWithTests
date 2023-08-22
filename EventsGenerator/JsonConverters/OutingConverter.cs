using EventsGenerator.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EventsGenerator.JsonConverters
{
    public class OutingConverter : System.Text.Json.Serialization.JsonConverter<Outing>
    {
        public override Outing Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            JsonElement rootElement = jsonDocument.RootElement;

            JsonElement trailElement = rootElement.GetProperty("Trail");
            
            Trail trail = null;
            try
            {
                JsonElement openingHour;
                if(trailElement.TryGetProperty("openingHour", out openingHour) == true)
                {
                    //it is a ParkTrail
                    trail = new ParkTrail()
                    {
                        PracticeStyle = trailElement.GetProperty("practiceStyle").GetString(),
                        PracticeStyle2 = trailElement.GetProperty("practiceStyle2").GetString(),
                        Capacity = trailElement.GetProperty("capacity").GetInt32(),
                        //location is not deserialized here (as in the api's outing coverter)
                        OpeningHour = trailElement.GetProperty("openingHour").GetInt32(),
                        ClosingHour = trailElement.GetProperty("closingHour").GetInt32(),
                        Id = trailElement.GetProperty("id").GetString(),
                        Name = trailElement.GetProperty("name").GetString()
                    };
                }
                else
                {
                    JsonElement checkpointsElement = trailElement.GetProperty("checkPoints");

                    trail = new CustomTrail()
                    {
                        Id = trailElement.GetProperty("id").GetString(),
                        Name = trailElement.GetProperty("name").GetString(),
                        CheckPoints = JsonConvert.DeserializeObject<List<CheckPoint>>(checkpointsElement.GetRawText())
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Deserializing trail error: " + ex.Message);
            }

            JsonElement daysElement = rootElement.GetProperty("days");

            Outing outing = new Outing();
            outing.Trail = trail;
            outing.Id = rootElement.GetProperty("id").GetString();
            outing.EventId = rootElement.GetProperty("eventId").GetString();
            outing.StartTime = rootElement.GetProperty("startTime").GetDouble();
            outing.EndTime = rootElement.GetProperty("endTime").GetDouble();
            outing.SkatePracticeStyle = rootElement.GetProperty("skatePracticeStyle").GetString();
            outing.Booked = rootElement.GetProperty("booked").GetBoolean();
            outing.Days = JsonConvert.DeserializeObject<List<Day>>(daysElement.GetRawText());


            //////NEWLY ADDED////
            JsonElement votedDay;
            if (rootElement.TryGetProperty("votedDay", out votedDay) == true)
            {
                outing.VotedDay = JsonConvert.DeserializeObject<Day>(rootElement.GetProperty("votedDay").GetRawText());
            }
            else outing.VotedDay = null;

            JsonElement votedStartTime;
            if (rootElement.TryGetProperty("votedStartTime", out votedStartTime) == true)
            {
                outing.VotedStartTime = rootElement.GetProperty("votedStartTime").GetDouble();
            }
            else outing.VotedStartTime = outing.StartTime;
            ///////////////////
            
            return outing;
        }

        public override void Write(Utf8JsonWriter writer, Outing value, JsonSerializerOptions options)
        {
            Console.WriteLine("AJUNG IN SERIALIZATORUL CUSTOM DE OUTING");

            writer.WriteStartObject();

            writer.WriteString("id", value.Id);
            writer.WriteString("eventId", value.EventId);
            writer.WriteNumber("startTime", value.StartTime);
            writer.WriteNumber("endTime", value.EndTime);
            writer.WriteString("skatePracticeStyle", value.SkatePracticeStyle);
            writer.WriteBoolean("booked", value.Booked);
            
            //////NEWLY ADDED////
            writer.WriteNumber("votedStartTime", value.VotedStartTime);
   
            if(value.VotedDay != null)
            {
                writer.WriteStartObject("votedDay");
                writer.WriteString("id", value.VotedDay.Id);
                writer.WriteNumber("dayOfMonth", value.VotedDay.DayOfMonth);
                writer.WriteEndObject();
            }
           
            ///////////////////

            writer.WriteStartArray("days");
            value.Days.ForEach(day =>
            {
                writer.WriteStartObject();

                writer.WriteString("id", day.Id);
                writer.WriteNumber("dayOfMonth", day.DayOfMonth);

                writer.WriteEndObject();
            });
            writer.WriteEndArray();

            if(value.Trail.GetType() == typeof(ParkTrail))
            {
                ParkTrail parkTrail = (ParkTrail)value.Trail;
                writer.WriteStartObject("Trail");

                writer.WriteString("id", parkTrail.Id);
                writer.WriteString("name", parkTrail.Name);
                writer.WriteString("practiceStyle", parkTrail.PracticeStyle);
                writer.WriteString("practiceStyle2", parkTrail.PracticeStyle2);
                if(parkTrail.Capacity != null)
                {
                    int capacityNumber = (int)parkTrail.Capacity;
                    writer.WriteNumber("capacity", capacityNumber);
                }

                //ADDED NOW
                writer.WriteStartObject("location");
                    writer.WriteString("id", parkTrail.Location.Id);
                    writer.WriteString("name", parkTrail.Location.Name);
                    writer.WriteString("imageUrl", parkTrail.Location.ImageUrl);
                    writer.WriteNumber("lat", parkTrail.Location.Lat);
                    writer.WriteNumber("long", parkTrail.Location.Long);
                writer.WriteEndObject();
                ///

                writer.WriteNumber("openingHour", parkTrail.OpeningHour);
                writer.WriteNumber("closingHour", parkTrail.ClosingHour);

                //location object is not serialized here
                writer.WriteEndObject();
            }
            else
            {
                CustomTrail customTrail = (CustomTrail)value.Trail;
                writer.WriteStartObject("Trail");

                    writer.WriteString("id", customTrail.Id);
                    writer.WriteString("name", customTrail.Name);

                    writer.WriteStartArray("checkPoints");
                    customTrail.CheckPoints.ForEach(checkpoint =>
                    {
                        writer.WriteStartObject();

                        writer.WriteString("id", checkpoint.Id);
                        writer.WriteString("name", checkpoint.Name);
                        writer.WriteString("customTrailId", checkpoint.CustomTrailId);
                        writer.WriteNumber("order", checkpoint.Order);

                        writer.WriteStartObject("location");
                            writer.WriteString("id", checkpoint.Location.Id);
                            writer.WriteString("name", checkpoint.Location.Name);
                            writer.WriteString("imageUrl", checkpoint.Location.ImageUrl);
                            writer.WriteNumber("lat", checkpoint.Location.Lat);
                            writer.WriteNumber("long", checkpoint.Location.Long);
                            writer.WriteEndObject();

                        writer.WriteEndObject();
                    });
                    writer.WriteEndArray();

                writer.WriteEndObject();
            }

            writer.WriteEndObject();

            Console.WriteLine("IES DIN SERIALIZATORUL CUSTOM DE OUTING");
        }

    }
}