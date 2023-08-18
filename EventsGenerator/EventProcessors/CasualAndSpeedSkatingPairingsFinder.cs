using EventsGenerator.Entities;
using EventsGenerator.EventProcessorsInterfaces;
using EventsGenerator.ExtraNeededClasses;
using EventsGenerator.Utils;
using EventsGenerator.UtilsInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace EventsGenerator.EventProcessors
{
    public class CasualAndSpeedSkatingPairingsFinder : ICasualAndSpeedSkatingPairingsFinder
    {
        public readonly IProcessingUtils _processingUtils;
        public CasualAndSpeedSkatingPairingsFinder(IProcessingUtils _processingUtils)
        {
            _processingUtils = _processingUtils;
        }
        public bool sameUser(Schedule schedule1, Schedule schedule2)
        {
            if (schedule1.SkateProfile.User.Id == schedule2.SkateProfile.User.Id)
                return true;

            return false;
        }
        public  List<Schedule> getSchedulePairingFromIndexPairing(List<Schedule> schedules, List<int> pairing)
        {
            List<Schedule> pairingSchedules = new List<Schedule>();
            foreach (int scheduleIndex in pairing)
            {
                pairingSchedules.Add(schedules.ElementAt(scheduleIndex));
            }
            return pairingSchedules;
        }
        public  bool isAValidPairing(List<Schedule> schedules, List<int> pairing, List<ParkTrail> parkTrails)
        {
            //filter by skills - no

            //filter by skateExperience - done
            //filter schedules by zone - done
            //filter by skatingStyle - done
            //filter by gender - done
            //filter by minimum age and maximum age - done
            //filter by days - done
            //filter by startTime filter by endTime - done
            //filter by maximumNrOFPartners - done

            int lastElementIndex = pairing.Count - 1;
            int usersInPairing = pairing.Count;
            Schedule lastScheduleInPairing = schedules.ElementAt(pairing.ElementAt(lastElementIndex));
            Schedule firstScheduleInPairing = schedules.ElementAt(pairing.ElementAt(0));


            if (usersInPairing < 2)
                return true;

            List<Schedule> pairingSchedules = getSchedulePairingFromIndexPairing(schedules, pairing);


            ////Schedules should have at least a common day between them/////
            List<Day> commonDays = _processingUtils.getCommonDaysFromSchedules(pairingSchedules);
            if (commonDays == null || commonDays.Count == 0)
            {
                Console.WriteLine($"Users {_processingUtils.getUsersAsStringFromSchedules(pairingSchedules)} have no common day");
                return false;
            }
            ////////////////////////////////////////////////////

            if (usersInPairing >= 2)
            {
                //////All schedules in pairing should have the same skating style///
                if (lastScheduleInPairing.SkateProfile.SkatePracticeStyle != firstScheduleInPairing.SkateProfile.SkatePracticeStyle)
                {
                    Console.WriteLine($"Not all users {_processingUtils.getUsersAsStringFromSchedules(pairingSchedules)} have the same skating style");
                    return false;
                }
                ////////////////////////////////////////////

                ////All schedules should be unique in pairing///
                if (pairing.ElementAt(lastElementIndex) <= pairing.ElementAt(lastElementIndex - 1))
                {
                    return false;
                }
                //////////////////////////////////////////////
            }

            for (int i = 0; i < lastElementIndex; i++)
            {
                Schedule scheduleI = schedules.ElementAt(pairing.ElementAt(i));
                Schedule scheduleLast = schedules.ElementAt(pairing.ElementAt(lastElementIndex));

                ////All schedules should be age compatible one by one////
                if (_processingUtils.areAgeCompatible(scheduleI, scheduleLast) == false)
                {
                    Console.WriteLine($"Users {_processingUtils.getUsersAsStringFromSchedules(pairingSchedules)} are not age compatible");
                    return false;
                }
                ///////////////////////////

                ////All schedules should be gender compatible one by one////
                if (_processingUtils.areGenderCompatible(scheduleI, scheduleLast) == false)
                {
                    Console.WriteLine($"Users {_processingUtils.getUsersAsStringFromSchedules(pairingSchedules)} are not gender compatible");
                    return false;
                }
                ///////////////////////////

                /////Each schedule in pairing should be owned by a different user
                if (sameUser(scheduleI, scheduleLast) == true)
                    return false;
                //////////////////////////////////////////////////////////////////////
            }

            /////All schedules should be time compatible
            if (_processingUtils.areAllSchedulesTimeCompatible(schedules, pairing) == false)
            {
                Console.WriteLine($"Users {_processingUtils.getUsersAsStringFromSchedules(pairingSchedules)} are not time compatible");
                return false;
            }
            //////////////////////////////////////////////////////////////////////

            for (int i = 0; i <= lastElementIndex; i++)
            {
                Schedule scheduleI = schedules.ElementAt(pairing.ElementAt(i));

                //All schedules should allow the current number of users in a pairing/////
                if (scheduleI.MaxNumberOfPeople + 1 < usersInPairing)
                {
                    Console.WriteLine($"Users {_processingUtils.getUsersAsStringFromSchedules(pairingSchedules)} in group are unsatisifed with group size");
                    return false;
                }
                //////////////////////////////////////////////////////////////////////////
            }

            //////////All schedules should have approximate skating experience///////////=
            if (_processingUtils.schedulesHaveCloseExperienceLevels(firstScheduleInPairing, lastScheduleInPairing) == false)
            {
                return false;
            }
            /////////////////////////////////////////////////////////////////////////////

            /////////////////////All schedules should have at least one trail in common ////
            List<ParkTrail> commonParkTrails = _processingUtils.getCommonParkTrailsFromSchedules(pairingSchedules, parkTrails);
            if (commonParkTrails.Count == 0)
            {
                Console.WriteLine($"Users {_processingUtils.getUsersAsStringFromSchedules(pairingSchedules)} have no common park trail");
                return false;
            }
            /////////

            return true;

        }

        public  Pairing pairingIndexesToPairingObject(List<Schedule> schedules, List<int> pairing)
        {
            List<Schedule> schedulesInPairing = new List<Schedule>();
            foreach (int scheduleIndex in pairing)
            {
                schedulesInPairing.Add(schedules.ElementAt(scheduleIndex));
            }

            return new Pairing()
            {
                Schedules = schedulesInPairing
            };
        }

        //public  bool existEventWithThisPairing(List<Schedule> schedules, List<int> pairing, List<Event> existingEvents)
        //{
        //    List<Schedule> pairingSchedules = getSchedulePairingFromIndexPairing(schedules, pairing);

        //    foreach(Event evnt in existingEvents)
        //    {
        //        //check if event has this pairing
        //        foreach()
        //    }

        //    return false;

        //}
        public  void backtracking(List<Schedule> schedules, List<Pairing> pairings, List<int> currentPairing, List<ParkTrail> parkTrails)
        {
            for (int scheduleIndex = 0; scheduleIndex < schedules.Count; scheduleIndex++)
            {

                currentPairing.Add(scheduleIndex);

                if (isAValidPairing(schedules, currentPairing, parkTrails) == true)
                {
                    //stop condition
                    int usersInPairing = currentPairing.Count;
                    if (usersInPairing >= 2)
                    {
                        pairings.Add(pairingIndexesToPairingObject(schedules, currentPairing));
                    }
                    backtracking(schedules, pairings, currentPairing, parkTrails);
                    currentPairing.RemoveAt(currentPairing.Count - 1);
                }
                else
                {
                    currentPairing.RemoveAt(currentPairing.Count - 1);
                }
            }
        }

        public  List<Pairing> findAllPairings(List<Schedule> schedules, List<ParkTrail> parkTrails)
        {
            Console.WriteLine("About to search for pairings");
            //List<DemoSchedule> testSchedules = getHardcodedListOfSchedules();
            List<Pairing> result = new List<Pairing>();

            List<int> currentPairing = new List<int>();
            backtracking(schedules, result, currentPairing, parkTrails);


            displayPairings(result);


            return result;
        }
        public  void displayPairings(List<Pairing> pairings)
        {
            if (pairings.Count == 0)
            {
                Console.WriteLine("NO PAIRINGS MADE");
            }
            foreach (Pairing pairing in pairings)
            {
                string display = "(";
                foreach (Schedule schedule in pairing.Schedules)
                {
                    display += schedule.SkateProfile.User.Name + ", ";
                }
                display += ")";
                Console.WriteLine(display);
            }
        }
        public  List<DemoSchedule> getHardcodedListOfSchedules()
        {
            return null;
            //var monday = new Day() { MinimumForm = "M", ShortForm = "Mon", LongForm = "Monday", Index = 1};
            //var tuesday = new Day() { MinimumForm = "T", ShortForm = "Tue", LongForm = "Tuesday", Index = 2};
            //var wednesday = new Day() { MinimumForm = "W", ShortForm = "Wed", LongForm = "Wednesday", Index = 3};
            //var thursday = new Day() { MinimumForm = "T", ShortForm = "Thu", LongForm = "Thursday", Index = 4};
            //var friday = new Day() { MinimumForm = "F", ShortForm = "Fri", LongForm = "Friday", Index = 5}; 
            //var saturday = new Day() { MinimumForm = "S", ShortForm = "Sat", LongForm = "Saturday", Index = 6}; 
            //var sunday = new Day() { MinimumForm = "S", ShortForm = "Sun", LongForm = "Sunday", Index = 7};



            //return new List<DemoSchedule>()
            //{
            //    new DemoSchedule()
            //    {
            //        Id = "User1",
            //        StartTime = 20,
            //        EndTime = 40,
            //        UserAge = 15,
            //        UserGender = "Male",
            //        Days = new List<Day> { monday, tuesday },
            //        GenderAllowance = "Male",
            //        MinimumAge = 10,
            //        MaximumAge = 30,
            //        MaxNumberOfPeople = 3,
            //    },
            //    new DemoSchedule()
            //    {
            //        Id = "User2",
            //        StartTime = 20,
            //        EndTime = 30,
            //        UserAge = 15,
            //        UserGender = "Male",
            //        Days = new List<Day> { monday, tuesday, wednesday },
            //        GenderAllowance = "Female",
            //        MinimumAge = 10,
            //        MaximumAge = 30,
            //        MaxNumberOfPeople = 2,
            //    },
            //    new DemoSchedule()
            //    {
            //        Id = "User3",
            //        StartTime = 28,
            //        EndTime = 40,
            //        UserAge = 15,
            //        UserGender = "Male",
            //        Days = new List<Day> { wednesday },
            //        GenderAllowance = "Male",
            //        MinimumAge = 10,
            //        MaximumAge = 30,
            //        MaxNumberOfPeople = 3,
            //    },
            //    new DemoSchedule()
            //    {
            //        Id = "User4",
            //        StartTime = 10,
            //        EndTime = 40,
            //        UserAge = 22,
            //        UserGender = "Male",
            //        Days = new List<Day> { wednesday, thursday, friday },
            //        GenderAllowance = "Male",
            //        MinimumAge = 10,
            //        MaximumAge = 30,
            //        MaxNumberOfPeople = 3,
            //    },
            //    new DemoSchedule()
            //    {
            //        Id = "User5",
            //        StartTime = 20,
            //        EndTime = 30,
            //        UserAge = 15,
            //        UserGender = "Male",
            //        Days = new List<Day> { monday, tuesday, wednesday },
            //        GenderAllowance = "Female",
            //        MinimumAge = 10,
            //        MaximumAge = 30,
            //        MaxNumberOfPeople = 2,
            //    }
            //};
        }
    }
}
