using EventsGenerator.Entities;
using EventsGenerator.EventProcessorsInterfaces;
using EventsGenerator.ExtraNeededClasses;
using EventsGenerator.Utils;
using EventsGenerator.UtilsInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventsGenerator.EventProcessors
{
    public class AggresiveSkatingEventProcessor : IAggresiveSkatingEventProcessor
    {

        private readonly IFetch _fetch;
        private readonly IProcessingUtils _processingUtils;

        public AggresiveSkatingEventProcessor(IFetch fetch, IProcessingUtils processingUtils)
        {
            _fetch = fetch;
            _processingUtils = processingUtils;
        }

        



        ///"FILTERS" APPLIED ON SCHEDULE & EVENT
        public bool isScheduleAgeCompatibleWithEvent(Event evnt, User eventOwner, Schedule schedule, User scheduleOwner)
        {
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
            schedule.SkateProfile.User = new User()
            {
                Age = scheduleOwner.Age
            };

            if (_processingUtils.areAgeCompatible(mockSchedule, schedule) == true)
            {
                return true;
            }
            else return false;
        }
        public bool isScheduleExperienceCompatibleWithEvent(Event evnt, User eventOwner, Schedule schedule, User scheduleOwner)
        {
            Schedule mockSchedule = new Schedule()
            {
                SkateProfile = new SkateProfile()
                {
                    SkateExperience = evnt.SkateExperience
                }
            };

            if (_processingUtils.schedulesHaveCloseExperienceLevels(mockSchedule, schedule) == true)
            {
                return true;
            }
            else return false;
        }
        public bool isScheduleGenderCompatibleWithEvent(Event evnt, User eventOwner, Schedule schedule, User scheduleOwner)
        {
            //put event prefrences and event owner data as a schedule mockup
            Schedule mockSchedule = new Schedule()
            {
                Gender = evnt.Gender,
                SkateProfile = new SkateProfile()
                {
                    User = new User()
                    {
                        Gender = eventOwner.Gender
                    }
                }
            };
            schedule.SkateProfile.User = new User()
            {
                Gender = scheduleOwner.Gender
            };
            if (_processingUtils.areGenderCompatible(mockSchedule, schedule) == true)
            {
                return true;
            }
            else return false;
        }
        public bool isScheduleTimeCompatibleWithEvent(Event evnt, User eventOwner, Schedule schedule, User scheduleOwner)
        {
           
            //put event prefrences and event owner data as a schedule mockup
            Schedule mockSchedule = new Schedule()
            {
                StartTime = evnt.Outing.StartTime,
                EndTime = evnt.Outing.EndTime,
            };
            if (_processingUtils.areTimeCompatible(mockSchedule, schedule) == true)
            {
                return true;
            }
            else return false;
        }
        public bool isScheduleDaysCompatibleWithEvent(Event evnt, User eventOwner, Schedule schedule, User scheduleOwner)
        {

            Schedule mockSchedule = new Schedule()
            {
                Days = evnt.Outing.Days
            };

            //put event prefrences and event owner data as a schedule mockup
            List<Schedule> schedulesToCheck = new List<Schedule>()
            {
                mockSchedule,
                schedule
            };
            List<Day> commonDays = _processingUtils.getCommonDaysFromSchedules(schedulesToCheck);
            if (commonDays != null && commonDays.Count > 0)
            {
                return true;
            }
            else return false;
        }

        ///FILTERS APPLIED ON SCHEDULES & AGGRESIVE EVENT
        public List<Schedule> getSchedulesAgeCompatibleWithOwnerBothWays(AggresiveEvent aggresiveEvent, User eventOwner, List<Schedule> schedules)
        {
            List<Schedule> filteredSchedules = new List<Schedule>();

            foreach (Schedule schedule in schedules)
            {
                Schedule mockSchedule = new Schedule()
                {
                    MinimumAge = aggresiveEvent.MinimumAge,
                    MaximumAge = aggresiveEvent.MaximumAge,
                    SkateProfile = new SkateProfile()
                    {
                        User = new User()
                        {
                            Age = eventOwner.Age
                        }
                    }
                };
                if (_processingUtils.areAgeCompatible(mockSchedule, schedule) == true)
                {
                    filteredSchedules.Add(schedule);
                }
            }

            return filteredSchedules;
        }
        public async Task<List<Schedule>> getSchedulesExperienceCompatibleWithOwnerBothWays(AggresiveEvent aggresiveEvent, User eventOwner, List<Schedule> schedules)
        {
            List<SkateProfile> skateProfiles = await _fetch.getAllSkateProfiles();
            List<Schedule> filteredSchedules = new List<Schedule>();

            foreach (Schedule schedule in schedules)
            {
                ////put event prefrences and event owner data as a schedule mockup
                Schedule mockSchedule = new Schedule()
                {
                    SkateProfile = new SkateProfile()
                    {
                        SkateExperience = aggresiveEvent.SkateExperience
                    }
                };

                if (_processingUtils.schedulesHaveCloseExperienceLevels(mockSchedule, schedule) == true)
                {
                    filteredSchedules.Add(schedule);
                }
            }

            return filteredSchedules;
        }
        public List<Schedule> getSchedulesGenderCompatibleWithOwnerBothWays(AggresiveEvent aggresiveEvent, User eventOwner, List<Schedule> schedules)
        {
            List<Schedule> filteredSchedules = new List<Schedule>();

            foreach (Schedule schedule in schedules)
            {
                //put event prefrences and event owner data as a schedule mockup
                Schedule mockSchedule = new Schedule()
                {
                    Gender = aggresiveEvent.Gender,
                    SkateProfile = new SkateProfile()
                    {
                        User = new User()
                        {
                            Gender = eventOwner.Gender
                        }
                    }
                };

                if (_processingUtils.areGenderCompatible(mockSchedule, schedule) == true)
                {
                    filteredSchedules.Add(schedule);
                }
            }

            return filteredSchedules;
        }
        public List<Schedule> getSchedulesTimeCompatibleWithOwnerBothWays(AggresiveEvent aggresiveEvent, User eventOwner, List<Schedule> schedules)
        {
            List<Schedule> filteredSchedules = new List<Schedule>();

            foreach (Schedule schedule in schedules)
            {
                //put event prefrences and event owner data as a schedule mockup
                Schedule mockSchedule = new Schedule()
                {
                    StartTime = aggresiveEvent.Outing.StartTime,
                    EndTime = aggresiveEvent.Outing.EndTime,
                };
                if (_processingUtils.areTimeCompatible(mockSchedule, schedule) == true)
                {
                    filteredSchedules.Add(schedule);
                }
            }
            return filteredSchedules;
        }
        public List<Schedule> getSchedulesDaysCompatibleWithOwnerBothWays(AggresiveEvent aggresiveEvent, User eventOwner, List<Schedule> schedules)
        {
            List<Schedule> filteredSchedules = new List<Schedule>();

            Schedule mockSchedule = new Schedule()
            {
                Days = aggresiveEvent.Days
            };

            foreach (Schedule schedule in schedules)
            {
                //put event prefrences and event owner data as a schedule mockup
                List<Schedule> schedulesToCheck = new List<Schedule>()
                {
                    mockSchedule,
                    schedule
                };
                List<Day> commonDays = _processingUtils.getCommonDaysFromSchedules(schedulesToCheck);
                if (commonDays != null && commonDays.Count > 0)
                {
                    filteredSchedules.Add(schedule);
                }
            }

            return filteredSchedules;
        }

        
        
        ///FILTERS APPLIED ON SCHEDULES
        public List<Schedule> getSchedulesFilteredBySkatingStyle(string skatingStyle, List<Schedule> schedules)
        {
            List<Schedule> filteredSchedules = new List<Schedule>();

            foreach (Schedule schedule in schedules)
            {
                if (schedule.SkateProfile.SkatePracticeStyle == skatingStyle)
                    filteredSchedules.Add(schedule);
            }

            return filteredSchedules;
        }
        public List<Schedule> eliminateUserSchedules(User user, List<Schedule> schedules)
        {
            List<Schedule> filteredSchedules = new List<Schedule>();

            foreach (Schedule schedule in schedules)
            {
                if (schedule.SkateProfile.UserId.Equals(user.Id) == false)
                {
                    filteredSchedules.Add(schedule);
                }
            }
            return filteredSchedules;
        }
        public List<Schedule> getSchedulesInRangeForCustomTrail(CustomTrail customTrail, List<Schedule> schedules)
        {
            List<Schedule> filteredSchedules = new List<Schedule>();
            HashSet<string> filteredSchedulesIds = new HashSet<string>();


            foreach (CheckPoint checkpoint in customTrail.CheckPoints)
            {
                //for each checkpoint we look if schedules are in their proximity

                foreach (Schedule schedule in schedules)
                {
                    if (_processingUtils.checkIfLocationInZone(checkpoint.Location, schedule.Zones[0]) == true)
                    {
                        //add schedule because it is in proximity of one checkpoint from trail
                        if (!filteredSchedulesIds.Contains(schedule.Id))
                        {
                            filteredSchedules.Add(schedule);
                            filteredSchedulesIds.Add(schedule.Id);
                        }
                    }
                }
            }

            return filteredSchedules;
        }
        public bool isScheduleInRangeForCustomTrail(CustomTrail customTrail, Schedule schedule)
        {
            foreach (CheckPoint checkpoint in customTrail.CheckPoints)
            {
                //for each checkpoint we look if schedules are in their proximity

                if (_processingUtils.checkIfLocationInZone(checkpoint.Location, schedule.Zones[0]) == true)
                {
                    //schedule is in range for customTrail because it is in proximity of one checkpoint from trail
                    return true;
                }
                
            }
            return false;
        }
    }
}
