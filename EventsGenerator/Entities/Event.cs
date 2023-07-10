using EventsGenerator.JsonConverters;
using System.Collections;
using System.Text.Json.Serialization;

namespace EventsGenerator.Entities
{
    public class Event
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int MaxParticipants { get; set; }
        public string SkateExperience { get; set; }

        [JsonConverter(typeof(OutingConverter))]
        public Outing Outing { get; set; }
        public List<SkateProfile>? SkateProfiles { get; set; }
        public List<SkateProfile> RecommendedSkateProfiles { get; set; }
        public List<ScheduleRefrence> ScheduleRefrences { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public string Gender { get; set; }
        public int MinimumAge { get; set; }
        public int MaximumAge { get; set; }

        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Event comparingEvent = (Event)obj;

            if (MaxParticipants != comparingEvent.MaxParticipants)
                return false;

            if (!SkateExperience.Equals(comparingEvent.SkateExperience))
                return false;

            if (!Gender.Equals(comparingEvent.Gender))
                return false;

            if (MinimumAge != comparingEvent.MinimumAge)
                return false;

            if (MaximumAge != comparingEvent.MaximumAge)
                return false;

            //compare outing
            if (!Outing.Equals(comparingEvent.Outing))
                return false;

            //compare recommended skateProfiles by Id
            List<SkateProfile> skateProfiles1;
            if (SkateProfiles != null)
            {
                skateProfiles1 = RecommendedSkateProfiles.Concat(SkateProfiles).ToList();
            }
            else skateProfiles1 = RecommendedSkateProfiles;

            List<SkateProfile> skateProfiles2;
            if (comparingEvent.SkateProfiles != null)
            {
                skateProfiles2 = comparingEvent.RecommendedSkateProfiles.Concat(comparingEvent.SkateProfiles).ToList();
            }
            else skateProfiles2 = comparingEvent.RecommendedSkateProfiles;


            List<SkateProfile> sortedSkateProfiles1 = skateProfiles1.OrderBy(skateProfile => skateProfile.Id).ToList();
            List<SkateProfile> sortedSkateProfiles2 = skateProfiles2.OrderBy(skateProfile => skateProfile.Id).ToList();

            for (int i = 0; i < sortedSkateProfiles1.Count; i++)
            {
                if (sortedSkateProfiles1[i].Id != sortedSkateProfiles2[i].Id)
                    return false;
            }

            return true;
        }

    }
}
