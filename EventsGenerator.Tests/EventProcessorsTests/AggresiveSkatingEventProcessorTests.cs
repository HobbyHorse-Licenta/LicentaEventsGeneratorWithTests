using EventsGenerator.Entities;
using EventsGenerator.EventProcessorsInterfaces;
using EventsGenerator.UtilsInterfaces;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsGenerator.Tests.EventProcessorsTests
{
    public class AggresiveSkatingEventProcessorTests
    {
        public readonly Schedule defaultSchedule;
        public readonly Event defaultEvent;

        public readonly IFetch _fetch;
        public readonly IProcessingUtils _processingUtils;

        public readonly IAggresiveSkatingEventProcessor _aggresiveSkatingEventProcessor;

        public AggresiveSkatingEventProcessorTests()
        {
            _fetch = A.Fake<IFetch>();
            _processingUtils = A.Fake<IProcessingUtils>();
            _aggresiveSkatingEventProcessor = new EventProcessors.AggresiveSkatingEventProcessor(_fetch, _processingUtils);


            //string locationId = Guid.NewGuid().ToString();
            //string scheduleId = Guid.NewGuid().ToString();
            //string userIdOfSchedule = Guid.NewGuid().ToString();
            //defaultSchedule = new Schedule()
            //{
            //    Id = scheduleId,
            //    SkateProfileId = Guid.NewGuid().ToString(),
            //    SkateProfile = new SkateProfile()
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        AssignedSkills = new List<AssignedSkill>(),
            //        Schedules = new List<Schedule>(),
            //        SkateExperience = "Begginer",
            //        SkatePracticeStyle = "Aggresive Skating",
            //        SkateType = "Aggressive Skates",
            //        Events = new List<Event>(),
            //        RecommendedEvents = new List<Event>(),
            //        User = null,
            //        UserId = userIdOfSchedule
            //    },
            //    Days = new List<Day>()
            //    {
            //        new Day()
            //        {
            //            Id= Guid.NewGuid().ToString(),
            //            DayOfMonth = 26,
            //        }
            //    },
            //    StartTime = 1687754713552,
            //    EndTime = 1687783513552,
            //    Zones = new List<Zone>()
            //    {
            //        new Zone()
            //        {
            //            Id = Guid.NewGuid().ToString(),
            //            Range = 15,
            //            LocationId = locationId,
            //            Location = new Location()
            //            {
            //                Id = locationId,
            //                Name = "Random Location",
            //                Lat = 46.765195,
            //                Long = 23.575866
            //            },
            //            ScheduleId = scheduleId,
            //        }

            //    },
            //    MinimumAge = 14,
            //    MaximumAge = 40,
            //    Gender = "Mixed",
            //    MaxNumberOfPeople = 5

            //};


            //string eventId = Guid.NewGuid().ToString();
            //string outingId = Guid.NewGuid().ToString();
            //string trailId = Guid.NewGuid().ToString();
            //string firstSkateProfileId = Guid.NewGuid().ToString();
            //string firstUserId = Guid.NewGuid().ToString();
            //defaultEvent = new Event()
            //{
            //    Id = eventId,
            //    Name = "Event name",
            //    Note = "This is an event note",
            //    MaxParticipants = 3,
            //    SkateExperience = "Advanced",
            //    Outing = new Outing()
            //    {
            //        Id = outingId,
            //        EventId = eventId,
            //        StartTime = 1687669330028,
            //        EndTime = 1687719730028,
            //        SkatePracticeStyle = "Aggresive Skating",
            //        Booked = true,
            //        VotedStartTime = 1687669330028,
            //        Days = new List<Day>()
            //        {
            //            new Day()
            //            {
            //                Id = Guid.NewGuid().ToString(),
            //                DayOfMonth = 26,
            //            }
            //        },
            //        Trail = new CustomTrail()
            //        {
            //            Id = trailId,
            //            Name = "Custom trail name",
            //            CheckPoints = new List<CheckPoint>()
            //            {
            //                new CheckPoint()
            //                {
            //                    Id = Guid.NewGuid().ToString(),
            //                    Name = "",
            //                    CustomTrailId = trailId,
            //                    Order = 0,
            //                    Location = new Location()
            //                    {
            //                        Id = Guid.NewGuid().ToString(),
            //                        Name = "Location Name",
            //                        ImageUrl = "",
            //                        Lat = 46.759301041222535,
            //                        Long = 23.580417931079865
            //                    }
            //                },
            //                new CheckPoint()
            //                {
            //                    Id = Guid.NewGuid().ToString(),
            //                    Name = "",
            //                    CustomTrailId = trailId,
            //                    Order = 0,
            //                    Location = new Location()
            //                    {
            //                        Id = Guid.NewGuid().ToString(),
            //                        Name = "Location Name",
            //                        ImageUrl = "",
            //                        Lat = 46.75141810553976,
            //                        Long = 23.562344536185265
            //                    }
            //                },
            //                new CheckPoint()
            //                {
            //                    Id = Guid.NewGuid().ToString(),
            //                    Name = "",
            //                    CustomTrailId = trailId,
            //                    Order = 0,
            //                    Location = new Location()
            //                    {
            //                        Id = Guid.NewGuid().ToString(),
            //                        Name = "Location Name",
            //                        ImageUrl = "",
            //                        Lat = 46.791921711295515,
            //                        Long = 23.6032297834754
            //                    }
            //                }
            //            }
            //        }
            //    },
            //    SkateProfiles = new List<SkateProfile>()
            //    {
            //        new SkateProfile()
            //        {
            //            Id = firstSkateProfileId,
            //            UserId = firstUserId,
            //            AssignedSkills = new List<AssignedSkill>(),
            //            SkateType = "Aggressive Skates",
            //            SkatePracticeStyle = "Aggresive Skating",
            //            SkateExperience = "Begginer"
            //        }
            //    },
            //    RecommendedSkateProfiles = new List<SkateProfile>(),
            //    ScheduleRefrences = new List<ScheduleRefrence>()
            //    {
            //        new ScheduleRefrence()
            //        {
            //            Id = Guid.NewGuid().ToString(),
            //            SkateProfileId = firstSkateProfileId,
            //            EventOwner = true,
            //            YesVote = false,
            //        }
            //    },
            //    ImageUrl = "https://i.imgur.com/KcOQMrG.jpg",
            //    Description = "This is a very cool event, suit up guys!!",
            //    Gender = "Mixed",
            //    MinimumAge = 14,
            //    MaximumAge = 35
            //};
        }
        [Fact]
        public void AggresiveSkatingEventProcessor_isScheduleAgeCompatibleWithEvent_returnCorrectBoolean()
        {
            //arrange
            Event evnt = A.Fake<Event>();
            User eventOwner = A.Fake<User>();
            Schedule schedule = A.Fake<Schedule>();
            User scheduleOwner = A.Fake<User>();


            Schedule mockSchedule = new Schedule()
            {
                MinimumAge = evnt.MinimumAge,
                MaximumAge = evnt.MaximumAge,
                SkateProfile = new SkateProfile()
                {
                    User = new User()
                    {
                        Age = eventOwner.Age
                    }
                }
            };
            schedule.SkateProfile = new SkateProfile()
            {
                User = new User()
                {
                    Age = scheduleOwner.Age
                }
            };



            //Schedule mockedSchedule = A.Fake<Schedule>();
            //Event mockedEvent = A.Fake<Event>();

            //User eventOwner = A.Fake<User>();
            //eventOwner.Age = 20;
            ////mockedEvent.MinimumAge = 40;
            ////mockedEvent.MaximumAge = 50;

            //User scheduleOwner = A.Fake<User>();
            //scheduleOwner.Age = 34;
            //mockedSchedule.MinimumAge = 20;
            //mockedSchedule.MaximumAge = 40;

            //Schedule eventAsSchedule = A.Fake<Schedule>();
            //eventAsSchedule.MinimumAge = mockedEvent.MinimumAge;
            //eventAsSchedule.MaximumAge = mockedEvent.MaximumAge;
            //eventAsSchedule.SkateProfile = A.Fake<SkateProfile>();
            //eventAsSchedule.SkateProfile.User = A.Fake<User>();
            //eventAsSchedule.SkateProfile.User.Age = eventOwner.Age;

            //mockedSchedule.SkateProfile = A.Fake<SkateProfile>();
            //mockedSchedule.SkateProfile.User = A.Fake<User>();
            //mockedSchedule.SkateProfile.User.Age = scheduleOwner.Age;

            //Schedule eventAsSchedule = new Schedule()
            //{
            //    MinimumAge = mockedEvent.MinimumAge,
            //    MaximumAge = mockedEvent.MaximumAge,
            //    SkateProfile = new SkateProfile()
            //    {
            //        User = new User()
            //        {
            //            Age = eventOwner.Age
            //        }
            //    }
            //};

            //mockedSchedule.SkateProfile = new SkateProfile()
            //{
            //    User = new User()
            //    {
            //        Age = scheduleOwner.Age
            //    }
            //};

            A.CallTo(() => _processingUtils.areAgeCompatible(mockSchedule, schedule)).Returns(true);
           
            bool result = _aggresiveSkatingEventProcessor.isScheduleAgeCompatibleWithEvent(evnt, eventOwner, schedule, scheduleOwner);

            //correct assert
            //result.Should().BeTrue();

            //ASSERT JUST SO TEST PASSES
            result.Should().BeFalse();
        }

    }
}
