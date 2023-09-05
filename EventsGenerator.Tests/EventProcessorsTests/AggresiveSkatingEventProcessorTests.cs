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


            A.CallTo(() => _processingUtils.areAgeCompatible(mockSchedule, schedule)).Returns(false);
           
            bool result = _aggresiveSkatingEventProcessor.isScheduleAgeCompatibleWithEvent(evnt, eventOwner, schedule, scheduleOwner);

            //correct assert
            result.Should().BeFalse();
        }

    }
}
