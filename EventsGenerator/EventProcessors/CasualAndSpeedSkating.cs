using EventsGenerator.Entities;
using EventsGenerator.EventProcessorsInterfaces;
using EventsGenerator.ExtraNeededClasses;
using EventsGenerator.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public CasualAndSpeedSkating(IFetch fetch, ICasualAndSpeedSkatingPairingsFinder casualAndSpeedSkatingPairingsFinder,
            ICasualAndSpeedSkatingEventGenerator casualAndSpeedSkatingEventGenerator)
        {
            _fetch = fetch;
            _casualAndSpeedSkatingPairingsFinder = casualAndSpeedSkatingPairingsFinder;
            _casualAndSpeedSkatingEventGenerator = casualAndSpeedSkatingEventGenerator;
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

    }
}
