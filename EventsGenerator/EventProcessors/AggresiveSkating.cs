using EventsGenerator.Entities;
using EventsGenerator.EventProcessorsInterfaces;
using EventsGenerator.UtilsInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsGenerator.EventProcessors
{
    public class AggresiveSkating : IAggresiveSkating
    {
        //the updating the event right after join (join is done, in api, on method return update on
        //generator with that event id should be called)
        //every time some joins the event, the others that are still just recommended should be removed
        //\\\\\or warned that some people who dont meet its criterias joined. - will not do.
        //so it should be a group (people that joined + owner), this group should always
        //be compatible one by one (this includes max nr of people, the event permits, should be kept
        //the initial max number of skaters from aggresive event).
        //filter by maximumNrOFPartners - event will dissapear from recomandation after all users joined

        private static readonly string AggresiveSkatingStyle = "Aggresive Skating";
        private static readonly string CasualSkatingStyle = "Casual Skating";
        private static readonly string SpeedSkatingStyle = "Speed Skating";

        public readonly IFetch _fetch;
        public readonly IAggresiveSkatingEventProcessor _eventProcessor;
        public readonly IProcessingUtils _processingUtils;
        public AggresiveSkating(IFetch fetch, IProcessingUtils processingUtils, IAggresiveSkatingEventProcessor eventProcessor)
        {
            _fetch = fetch;
            _processingUtils = processingUtils;
            _eventProcessor = eventProcessor;
        }

        ////MAIN FUNCTIONS
        public async Task<Event> completeAggresiveEvent(AggresiveEvent aggresiveEvent)
        {
            if (aggresiveEvent.SkateProfiles != null && aggresiveEvent.SkateProfiles[0] != null)
            {
                //owner of the event was added to the list of skateProfiles in aggresive evnt
                //get basic info about owner user (since it would have the first and only skateProfile)
                Console.WriteLine("Starting to create an event from the aggresive event received through queue");

                try
                {
                    User owner = await _fetch.getUserWithBasicInfo(aggresiveEvent.SkateProfiles[0].UserId);

                    List<Schedule> allSchedules = await _fetch.getAllSchedules();


                    List<Schedule> reccommendedSchedules = new List<Schedule>();
                    List<SkateProfile> recommendedSkateProfiles = new List<SkateProfile>();

                    List<ScheduleRefrence> scheduleRefrences = new List<ScheduleRefrence>();

                    scheduleRefrences.Add(new ScheduleRefrence()
                    {
                        Id = Guid.NewGuid().ToString(),
                        SkateProfileId = aggresiveEvent.SkateProfiles[0].Id,
                        EventOwner = true,
                        YesVote = false
                    });

                    if (allSchedules != null && allSchedules.Count > 0)
                    {
                        reccommendedSchedules = await findAppropriateSchedulesForPartialAggresiveEvent(aggresiveEvent, owner, allSchedules);

                        foreach (Schedule schedule in reccommendedSchedules)
                        {
                            ScheduleRefrence scheduleRefrence = new ScheduleRefrence
                            {
                                Id = Guid.NewGuid().ToString(),
                                ScheduleId = schedule.Id,
                                SkateProfileId = schedule.SkateProfile.Id,
                                EventOwner = false,
                                YesVote = false
                            };

                            scheduleRefrences.Add(scheduleRefrence);
                        }

                        recommendedSkateProfiles = _processingUtils.gettingSkateProfilesFromSchedules(reccommendedSchedules);

                        Console.WriteLine("Got the recommended skate profiles for the event that's being created");
                    }

                    Outing outing = new Outing()
                    {
                        Id = aggresiveEvent.Outing.Id, //not changing
                        EventId = aggresiveEvent.Outing.EventId, //not changing
                        StartTime = aggresiveEvent.Outing.StartTime, //intersection between this and joined schedules
                        EndTime = aggresiveEvent.Outing.EndTime, //intersection between this and joined schedules
                        Days = aggresiveEvent.Days, //intersection between this and joined schedules
                        VotedStartTime = aggresiveEvent.Outing.StartTime,
                        VotedDay = new Day
                        {
                            Id = Guid.NewGuid().ToString(),
                            DayOfMonth = aggresiveEvent.Days[0].DayOfMonth
                        },
                        SkatePracticeStyle = aggresiveEvent.Outing.SkatePracticeStyle, //not changing
                        Trail = aggresiveEvent.Outing.Trail, //not changing
                        Booked = aggresiveEvent.Outing.Booked //not changing
                    };

                    return new Event()
                    {
                        Id = aggresiveEvent.Id, //not changing
                        Name = aggresiveEvent.Name, //not changing
                        Note = aggresiveEvent.Note, //not changing
                        MinimumAge = aggresiveEvent.MinimumAge,//intersection between this and joined schedules
                        MaximumAge = aggresiveEvent.MaximumAge,//intersection between this and joined schedules
                        MaxParticipants = aggresiveEvent.MaxParticipants, //minimum of all
                        SkateExperience = aggresiveEvent.SkateExperience, //not changing
                        Outing = outing, //already settled
                        SkateProfiles = aggresiveEvent.SkateProfiles, //add here when one joins, remove when one leaves
                        RecommendedSkateProfiles = recommendedSkateProfiles, //add back here when one leaves,remove when one joins
                                                                             //remove all of them if skateProfiles count = maxParticipants
                        ScheduleRefrences = scheduleRefrences, //this changes when a schedule gets deleted
                        ImageUrl = aggresiveEvent.ImageUrl, //add here when one joins, remove when one leaves
                        Description = aggresiveEvent.Description, //not changing
                        Gender = aggresiveEvent.Gender //intersection between this and joined schedules
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
            return null;
        }

        public async Task<bool> JoinScheduleToAggresiveEvent(Schedule schedule, Event evnt)
        {

            //Outing outing = new Outing()
            //{
            //    StartTime = aggresiveEvent.Outing.StartTime, //intersection between this and joined schedules
            //    EndTime = aggresiveEvent.Outing.EndTime, //intersection between this and joined schedules
            //    Days = aggresiveEvent.Days, //intersection between this and joined schedules

            //};

            //return new Event()
            //{
            //    MinimumAge = aggresiveEvent.MinimumAge,//intersection between this and joined schedules
            //    MaximumAge = aggresiveEvent.MaximumAge,//intersection between this and joined schedules
            //    MaxParticipants = aggresiveEvent.MaxParticipants, //minimum of all
            //    Outing = outing, //already settled
            //    SkateProfiles = aggresiveEvent.SkateProfiles, //add here when one joins, remove when one leaves
            //    RecommendedSkateProfiles = recommendedSkateProfiles, //add back here when one leaves,remove when one joins
            //                                                         //remove all of them if skateProfiles count = maxParticipants
            //    ScheduleRefrences = scheduleRefrences, //this changes when a schedule gets deleted
            //    ImageUrl = aggresiveEvent.ImageUrl, //add here when one joins, remove when one leaves
            //    Gender = aggresiveEvent.Gender //intersection between this and joined schedules
            //};

            // List<Schedules>
            double startTime = 0;
            double endTime = 0;
            if (_processingUtils.getTimeAsDoubleFromTimestamp(schedule.StartTime) >= _processingUtils.getTimeAsDoubleFromTimestamp(evnt.Outing.EndTime)
                || _processingUtils.getTimeAsDoubleFromTimestamp(schedule.EndTime) <= _processingUtils.getTimeAsDoubleFromTimestamp(evnt.Outing.StartTime))
            {
                //these dont intersect time wise
                return false;
            }
            else
            {
                if (schedule.StartTime > evnt.Outing.StartTime)
                    startTime = schedule.StartTime;
                else startTime = evnt.Outing.StartTime;

                if (schedule.EndTime < evnt.Outing.EndTime)
                    endTime = schedule.EndTime;
                else endTime = evnt.Outing.EndTime;


                evnt.Outing.StartTime = startTime;
                evnt.Outing.EndTime = endTime;
            }

            List<Day> commonDays = _processingUtils.getCommonDaysBetweenScheduleAndEvent(schedule, evnt);
            if (commonDays == null || commonDays.Count < 1)
                return false;

            evnt.Outing.Days = commonDays;


            if (schedule.SkateProfile != null && schedule.SkateProfile.UserId != null && schedule.SkateProfile.UserId.Length > 0)
            {
                User scheduleOwnerInfo = await _fetch.getUserWithBasicInfo(schedule.SkateProfile.UserId);

                if (scheduleOwnerInfo.Age > evnt.MaximumAge || scheduleOwnerInfo.Age < evnt.MinimumAge)
                {
                    return false;
                }
                else
                {
                    if (schedule.MinimumAge != null && schedule.MaximumAge != null)
                    {
                        int newMaximumAgeForEvent;
                        int newMinimumAgeForEvent;
                        if (evnt.MinimumAge > schedule.MinimumAge)
                        {
                            newMinimumAgeForEvent = evnt.MinimumAge;
                        }
                        else newMinimumAgeForEvent = (int)schedule.MinimumAge;
                        if (evnt.MaximumAge < schedule.MaximumAge)
                        {
                            newMaximumAgeForEvent = evnt.MaximumAge;
                        }
                        else newMaximumAgeForEvent = (int)schedule.MaximumAge;

                        if (newMaximumAgeForEvent < newMinimumAgeForEvent)
                        {
                            return false;
                        }
                        else
                        {
                            evnt.MaximumAge = newMaximumAgeForEvent;
                            evnt.MinimumAge = newMinimumAgeForEvent;
                        }
                    }
                    else return false;

                }
            }
            else
            {
                Console.WriteLine("Could not get info about user whos schedule we want to integrate in events");
                return false;
            }


            if (schedule.MaxNumberOfPeople < evnt.MaxParticipants)
                evnt.MaxParticipants = schedule.MaxNumberOfPeople;
            //put evnt
            return true;
        }


        public async Task<Event> UpdateAggresiveEventWithScheduleIfSuitable(Schedule schedule, Event evnt)
        {
            bool scheduleSuitable = await isScheduleForAggresiveEvent(evnt, schedule);
            if (scheduleSuitable == true)
            {
                SkateProfile skateProfileToAdd = await _fetch.getSkateProfile(schedule.SkateProfileId);
                evnt.RecommendedSkateProfiles.Add(skateProfileToAdd);
                evnt.ScheduleRefrences.Add(new ScheduleRefrence()
                {
                    Id = Guid.NewGuid().ToString(),
                    ScheduleId = schedule.Id,
                    SkateProfileId = skateProfileToAdd.Id,
                    EventOwner = false,
                    YesVote = false
                });
                return evnt;
            }
            return null;
        }




        ////MEDIUM FUNCTIONS
        public async Task<List<Schedule>> findAppropriateSchedulesForPartialAggresiveEvent(AggresiveEvent aggresiveEvent, User eventOwner, List<Schedule> allExistingSchedules)
        {
            //filter by skills - not doing it

            //filter by skatingStyle - done
            //filter by gender - done
            //filter by minimum age and maximum age - done
            //filter by startTime filter by endTime - done
            //filter by days - done
            //filter schedules by zone - done
            //filter by skateExperience - done



            //dont search through the schedules of the owner of the event
            List<Schedule> allSchedules = _eventProcessor.eliminateUserSchedules(eventOwner, allExistingSchedules);

            //all schedules should have be for aggresive skating style//////
            List<Schedule> onlyAggresiveSkatingSchedules = _eventProcessor.getSchedulesFilteredBySkatingStyle(AggresiveSkatingStyle, allSchedules);
            //////////////////////////////////////////

            //owner compatible with another user and that user compatible with owner///
            //recommended schedules are not compatible between each other////
            List<Schedule> schedulesFilteredByGender = _eventProcessor.getSchedulesGenderCompatibleWithOwnerBothWays(aggresiveEvent, eventOwner, onlyAggresiveSkatingSchedules);

            List<Schedule> schedulesfilteredByAge = _eventProcessor.getSchedulesAgeCompatibleWithOwnerBothWays(aggresiveEvent, eventOwner, schedulesFilteredByGender);

            List<Schedule> schedulesfilteredByTime = _eventProcessor.getSchedulesTimeCompatibleWithOwnerBothWays(aggresiveEvent, eventOwner, schedulesfilteredByAge);

            List<Schedule> schedulesfilteredByDays = _eventProcessor.getSchedulesDaysCompatibleWithOwnerBothWays(aggresiveEvent, eventOwner, schedulesfilteredByTime);

            List<Schedule> schedulesInRange = _eventProcessor.getSchedulesInRangeForCustomTrail(aggresiveEvent.Outing.Trail, schedulesfilteredByDays);

            List<Schedule> schedulesfilteredBySkatingExperience = await _eventProcessor.getSchedulesExperienceCompatibleWithOwnerBothWays(aggresiveEvent, eventOwner, schedulesInRange);

            //////////////////////////////////////////

            return schedulesfilteredBySkatingExperience;
        }

        public async Task<bool> isScheduleForAggresiveEvent(Event evnt, Schedule schedule)
        {
            ////SIMILAR TO findAppropriateSchedulesForPartialAggresiveEvent


            //check by skills - not doing it

            //check by skatingStyle - done
            //check by gender - done
            //check by minimum age and maximum age - done
            //check by startTime filter by endTime - done
            //check by days - done
            //check schedules by zone - done
            //check by skateExperience - done

            if (_processingUtils.scheduleOwnerIsAlsoEventOwner(schedule, evnt) == true)
            {
                return false;
            }

            string eventOwnerSkateProfileId = _processingUtils.getAggresiveEventsOwnerSkateProfileId(evnt);
            if (eventOwnerSkateProfileId == null)
            {
                Console.WriteLine("Coudnt get info on eventOwner; Will not proceed checking if schedule is suitable with event");
                return false;
            }
            User eventOwner;
            User scheduleOwner;
            try
            {
                SkateProfile eventOwnerSkateProfile = await _fetch.getSkateProfile(eventOwnerSkateProfileId);
                eventOwner = eventOwnerSkateProfile.User;
                scheduleOwner = await _fetch.getUserWithBasicInfo(schedule.SkateProfile.UserId);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                return false;
            }
            

            if (_eventProcessor.isScheduleGenderCompatibleWithEvent(evnt, eventOwner, schedule, scheduleOwner) == false)
            {
                return false;
            }

            if (_eventProcessor.isScheduleAgeCompatibleWithEvent(evnt, eventOwner, schedule, scheduleOwner) == false)
            {
                return false;
            }
            if (_eventProcessor.isScheduleTimeCompatibleWithEvent(evnt, eventOwner, schedule, scheduleOwner) == false)
            {
                return false;
            }

            if (_eventProcessor.isScheduleDaysCompatibleWithEvent(evnt, eventOwner, schedule, scheduleOwner) == false)
            {
                return false;
            }

            if (_eventProcessor.isScheduleInRangeForCustomTrail((CustomTrail)evnt.Outing.Trail, schedule) == false)
            {
                return false;
            }

            if (_eventProcessor.isScheduleExperienceCompatibleWithEvent(evnt, eventOwner, schedule, scheduleOwner) == false)
            {
                return false;
            }

            return true;
        }
    }
}
