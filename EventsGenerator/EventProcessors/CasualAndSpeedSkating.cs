using EventsGenerator.Entities;
using EventsGenerator.EventProcessorsInterfaces;
using EventsGenerator.ExtraNeededClasses;
using EventsGenerator.Utils;
using EventsGenerator.UtilsInterfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsGenerator.EventProcessors
{
    public class CasualAndSpeedSkating :  ICasualAndSpeedSkating
    {
        public readonly IFetch _fetch;
        public readonly ICasualAndSpeedSkatingPairingsFinder _casualAndSpeedSkatingPairingsFinder;
        public readonly ICasualAndSpeedSkatingEventGenerator _casualAndSpeedSkatingEventGenerator;
        public readonly IProcessingUtils _processingUtils;
        public CasualAndSpeedSkating(IFetch fetch, ICasualAndSpeedSkatingPairingsFinder casualAndSpeedSkatingPairingsFinder,
            ICasualAndSpeedSkatingEventGenerator casualAndSpeedSkatingEventGenerator, IProcessingUtils processingUtils)
        {
            _fetch = fetch;
            _casualAndSpeedSkatingPairingsFinder = casualAndSpeedSkatingPairingsFinder;
            _casualAndSpeedSkatingEventGenerator = casualAndSpeedSkatingEventGenerator;
            _processingUtils = processingUtils;
        }
        public async void GenerateEvents()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {

                    List<Schedule> allSchedules = await _fetch.getAllSchedules();
                    List<ParkTrail> allParkTrails = await _fetch.getAllParkTrails();

                    if (allSchedules != null && allParkTrails != null && allSchedules.Count > 0 && allParkTrails.Count > 0)
                    {
                        DisplayUtils.displaySchedulesAndUserData(allSchedules);
                        List<Pairing> pairings = _casualAndSpeedSkatingPairingsFinder.findAllPairings(allSchedules, allParkTrails);

                        _casualAndSpeedSkatingEventGenerator.createEvents(pairings);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }


        public async Task<bool> ScheduleCoversCasualAndSpeedSkatingEvent(Schedule schedule, Event evnt)
        {
            List<ParkTrail> allParkTrails = await _fetch.getAllParkTrails();
            List<Schedule> schedulesOfEvent = await _processingUtils.GetSchedulesOfEvent(evnt);

            //we add the schedule we want to test to the existing schedule of the event and then we use the
            //is a valid pairing function to see if the schedule is compatible with the event.
            schedulesOfEvent.Add(schedule);
            List<int> currentPairing = new List<int>();
            for(int i = 0; i < schedulesOfEvent.Count; i++)
            {
                currentPairing.Add(i);
            }

            bool result = await _casualAndSpeedSkatingPairingsFinder.isAValidPairing(schedulesOfEvent, currentPairing, allParkTrails);
            return result;
        }
        public async Task<bool> SkateProfileCanAttendCasualAndSpeedSkatingEvent(string skateProfileId, Event evnt)
        {
            try
            {
                SkateProfile skateprofile = await _fetch.getSkateProfile(skateProfileId);
                if (skateprofile != null)
                {
                    if (skateprofile.Schedules != null && skateprofile.Schedules.Count > 0)
                    {
                        foreach (Schedule schedule in skateprofile.Schedules)
                        {
                            bool scheduleCoversEvent = await ScheduleCoversCasualAndSpeedSkatingEvent(schedule, evnt);
                            if (scheduleCoversEvent == true)
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                    else return false;
                }
                else Console.WriteLine("[SkateProfileCanAttendCasualAndSpeedSkatingEvent]: Coudnt't get skateprofile");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
            return false;
            
        }

    }
}
