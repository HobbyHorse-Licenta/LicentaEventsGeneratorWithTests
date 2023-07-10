using EventsGenerator.EventProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using EventsGenerator.Entities;
using FluentAssertions;
using EventsGenerator.Utils;
using Moq;
using EventsGenerator.EventProcessorsInterfaces;
using EventsGenerator.UtilsInterfaces;

namespace EventsGenerator.Tests.EventProcessorsTests
{
    public class AggresiveSkatingTests
    {
        public readonly Schedule defaultSchedule;
        public readonly Event defaultEvent;

        public readonly IAggresiveSkatingEventProcessor _aggresiveSkatingEventProcessor;
        public readonly IFetch _fetch;
        public readonly IProcessingUtils _processingUtils;

        public readonly IAggresiveSkating _aggresiveSkating;

        public AggresiveSkatingTests()
        {
            _fetch = A.Fake<IFetch>();
            _processingUtils = A.Fake<IProcessingUtils>();
            _aggresiveSkatingEventProcessor = A.Fake<IAggresiveSkatingEventProcessor>();
            _aggresiveSkating = new EventProcessors.AggresiveSkating(_fetch, _processingUtils, _aggresiveSkatingEventProcessor);
            

            string locationId = Guid.NewGuid().ToString();
            string scheduleId = Guid.NewGuid().ToString();
            string userIdOfSchedule = Guid.NewGuid().ToString();
            defaultSchedule = new Schedule()
            {
                Id = scheduleId,
                SkateProfileId = Guid.NewGuid().ToString(),
                SkateProfile = new SkateProfile()
                {
                   Id = Guid.NewGuid().ToString(),
                   AssignedSkills = new List<AssignedSkill>(),
                   Schedules = new List<Schedule>(),
                   SkateExperience = "Begginer",
                   SkatePracticeStyle = "Aggresive Skating",
                   SkateType = "Aggressive Skates",
                   Events = new List<Event>(),
                   RecommendedEvents = new List<Event>(),
                   User = null,
                   UserId = userIdOfSchedule
                },
                Days = new List<Day>()
                {
                    new Day()
                    {
                        Id= Guid.NewGuid().ToString(),
                        DayOfMonth = 26,
                    }
                },
                StartTime = 1687754713552,
                EndTime = 1687783513552,
                Zones = new List<Zone>()
                {
                    new Zone()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Range = 15,
                        LocationId = locationId,
                        Location = new Location()
                        {
                            Id = locationId,
                            Name = "Random Location",
                            Lat = 46.765195,
                            Long = 23.575866
                        },
                        ScheduleId = scheduleId,
                    }                       
                    
                },
                MinimumAge = 14,
                MaximumAge = 40,
                Gender = "Mixed",
                MaxNumberOfPeople = 5

            };


            string eventId = Guid.NewGuid().ToString();
            string outingId = Guid.NewGuid().ToString();
            string trailId = Guid.NewGuid().ToString();
            string firstSkateProfileId = Guid.NewGuid().ToString();
            string firstUserId = Guid.NewGuid().ToString();
            defaultEvent = new Event()
            {
                Id = eventId,
                Name = "Event name",
                Note = "This is an event note",
                MaxParticipants = 3,
                SkateExperience = "Advanced",
                Outing = new Outing()
                {
                    Id = outingId,
                    EventId = eventId,
                    StartTime = 1687669330028,
                    EndTime = 1687719730028,
                    SkatePracticeStyle = "Aggresive Skating",
                    Booked = true,
                    VotedStartTime = 1687669330028,
                    Days = new List<Day>()
                    {
                        new Day()
                        {
                            Id = Guid.NewGuid().ToString(),
                            DayOfMonth = 26,
                        }
                    },
                    Trail = new CustomTrail()
                    {
                        Id = trailId,
                        Name = "Custom trail name",
                        CheckPoints = new List<CheckPoint>()
                        {
                            new CheckPoint()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "",
                                CustomTrailId = trailId,
                                Order = 0,
                                Location = new Location()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Name = "Location Name",
                                    ImageUrl = "",
                                    Lat = 46.759301041222535,
                                    Long = 23.580417931079865
                                }
                            },
                            new CheckPoint()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "",
                                CustomTrailId = trailId,
                                Order = 0,
                                Location = new Location()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Name = "Location Name",
                                    ImageUrl = "",
                                    Lat = 46.75141810553976,
                                    Long = 23.562344536185265
                                }
                            },
                            new CheckPoint()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "",
                                CustomTrailId = trailId,
                                Order = 0,
                                Location = new Location()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Name = "Location Name",
                                    ImageUrl = "",
                                    Lat = 46.791921711295515,
                                    Long = 23.6032297834754
                                }
                            }
                        }
                    }
                },
                SkateProfiles = new List<SkateProfile>()
                {
                    new SkateProfile()
                    {
                        Id = firstSkateProfileId,
                        UserId = firstUserId,
                        AssignedSkills = new List<AssignedSkill>(),
                        SkateType = "Aggressive Skates",
                        SkatePracticeStyle = "Aggresive Skating",
                        SkateExperience = "Begginer"
                    }
                },
                RecommendedSkateProfiles = new List<SkateProfile>(),
                ScheduleRefrences = new List<ScheduleRefrence>()
                {
                    new ScheduleRefrence()
                    {
                        Id = Guid.NewGuid().ToString(),
                        SkateProfileId = firstSkateProfileId,
                        EventOwner = true,
                        YesVote = false,
                    }
                },
                ImageUrl = "https://i.imgur.com/KcOQMrG.jpg",
                Description = "This is a very cool event, suit up guys!!",
                Gender = "Mixed",
                MinimumAge = 14,
                MaximumAge = 35
            };


        }

        [Fact]
        public async Task AggresiveSkatingEventProcessor_isScheduleForAggresiveEvent_returnTrueIfAllCheckAreCorrectAsync()
        {
            //arrange
            Schedule inputSchedule = A.Fake<Schedule>();
            Event inputEvent = A.Fake<Event>();

            SkateProfile scheduleSkateProfile = A.Fake<SkateProfile>();
            scheduleSkateProfile.UserId = "scheduleOwnerId";
            inputEvent.Gender = "Male";
            inputSchedule.SkateProfile = scheduleSkateProfile; 

            User eventOwner = A.Fake<User>();
            User scheduleOwner = A.Fake<User>();

            
            string eventOwnerSkateProfileId = "eventOwnerId";

            Outing outing = A.Fake<Outing>();
            CustomTrail customTrail = A.Fake<CustomTrail>();
            outing.Trail = customTrail;
            inputEvent.Outing = outing;


            A.CallTo(() => _processingUtils.getAggresiveEventsOwnerSkateProfileId(inputEvent)).Returns(eventOwnerSkateProfileId);
            
            A.CallTo(() => _processingUtils.scheduleOwnerIsAlsoEventOwner(inputSchedule, inputEvent)).Returns(false);
            A.CallTo(() => _fetch.getUserWithBasicInfo(eventOwnerSkateProfileId)).Returns(eventOwner);
            A.CallTo(() => _fetch.getUserWithBasicInfo(scheduleSkateProfile.UserId)).Returns(scheduleOwner);

            A.CallTo(() => _aggresiveSkatingEventProcessor.isScheduleGenderCompatibleWithEvent(
                inputEvent, eventOwner, inputSchedule, scheduleOwner)).Returns(true);
            A.CallTo(() => _aggresiveSkatingEventProcessor.isScheduleAgeCompatibleWithEvent(
                inputEvent, eventOwner, inputSchedule, scheduleOwner)).Returns(true);
            A.CallTo(() => _aggresiveSkatingEventProcessor.isScheduleTimeCompatibleWithEvent(
                inputEvent, eventOwner, inputSchedule, scheduleOwner)).Returns(true);
            A.CallTo(() => _aggresiveSkatingEventProcessor.isScheduleDaysCompatibleWithEvent(
                inputEvent, eventOwner, inputSchedule, scheduleOwner)).Returns(true);
            A.CallTo(() => _aggresiveSkatingEventProcessor.isScheduleInRangeForCustomTrail(
                (CustomTrail)inputEvent.Outing.Trail, inputSchedule)).Returns(true);
            A.CallTo(() => _aggresiveSkatingEventProcessor.isScheduleExperienceCompatibleWithEvent(
                inputEvent, eventOwner, inputSchedule, scheduleOwner)).Returns(true);


            //act
            bool result = await _aggresiveSkating.isScheduleForAggresiveEvent(inputEvent, inputSchedule);

            //correct assert
            result.Should().BeTrue();

            //ASSERT JUST SO TEST PASSES
            //result.Should().BeFalse();


        }

        [Fact]
        public async Task AggresiveSkatingEventProcessor_isScheduleForAggresiveEvent_returnFlaseIfOneCheckIsWrongAsync()
        {
            //arrange
            Schedule inputSchedule = A.Fake<Schedule>();
            Event inputEvent = A.Fake<Event>();

            SkateProfile scheduleSkateProfile = A.Fake<SkateProfile>();
            scheduleSkateProfile.UserId = "scheduleOwnerId";
            inputEvent.Gender = "Male";
            inputSchedule.SkateProfile = scheduleSkateProfile;

            User eventOwner = A.Fake<User>();
            User scheduleOwner = A.Fake<User>();


            string eventOwnerSkateProfileId = "eventOwnerId";

            Outing outing = A.Fake<Outing>();
            CustomTrail customTrail = A.Fake<CustomTrail>();
            outing.Trail = customTrail;
            inputEvent.Outing = outing;


            A.CallTo(() => _processingUtils.getAggresiveEventsOwnerSkateProfileId(inputEvent)).Returns(eventOwnerSkateProfileId);

            A.CallTo(() => _processingUtils.scheduleOwnerIsAlsoEventOwner(inputSchedule, inputEvent)).Returns(false);
            A.CallTo(() => _fetch.getUserWithBasicInfo(eventOwnerSkateProfileId)).Returns(eventOwner);
            A.CallTo(() => _fetch.getUserWithBasicInfo(scheduleSkateProfile.UserId)).Returns(scheduleOwner);

            A.CallTo(() => _aggresiveSkatingEventProcessor.isScheduleGenderCompatibleWithEvent(
                inputEvent, eventOwner, inputSchedule, scheduleOwner)).Returns(true);
            A.CallTo(() => _aggresiveSkatingEventProcessor.isScheduleAgeCompatibleWithEvent(
                inputEvent, eventOwner, inputSchedule, scheduleOwner)).Returns(true);
            A.CallTo(() => _aggresiveSkatingEventProcessor.isScheduleTimeCompatibleWithEvent(
                inputEvent, eventOwner, inputSchedule, scheduleOwner)).Returns(false);
            A.CallTo(() => _aggresiveSkatingEventProcessor.isScheduleDaysCompatibleWithEvent(
                inputEvent, eventOwner, inputSchedule, scheduleOwner)).Returns(true);
            A.CallTo(() => _aggresiveSkatingEventProcessor.isScheduleInRangeForCustomTrail(
                (CustomTrail)inputEvent.Outing.Trail, inputSchedule)).Returns(true);
            A.CallTo(() => _aggresiveSkatingEventProcessor.isScheduleExperienceCompatibleWithEvent(
                inputEvent, eventOwner, inputSchedule, scheduleOwner)).Returns(true);


            //act
            bool result = await _aggresiveSkating.isScheduleForAggresiveEvent(inputEvent, inputSchedule);

            //assert
            result.Should().BeFalse();

        }

        
    }
}
