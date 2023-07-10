using EventsGenerator.Entities;
using EventsGenerator.ExtraNeededClasses;
using EventsGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsGenerator.EventProcessorsInterfaces
{
    public interface ICasualAndSpeedSkatingPairingsFinder
    {
        bool sameUser(Schedule schedule1, Schedule schedule2);
        List<Schedule> getSchedulePairingFromIndexPairing(List<Schedule> schedules, List<int> pairing);
        bool isAValidPairing(List<Schedule> schedules, List<int> pairing, List<ParkTrail> parkTrails);

        Pairing pairingIndexesToPairingObject(List<Schedule> schedules, List<int> pairing);
        void backtracking(List<Schedule> schedules, List<Pairing> pairings, List<int> currentPairing, List<ParkTrail> parkTrails);

        List<Pairing> findAllPairings(List<Schedule> schedules, List<ParkTrail> parkTrails);
        void displayPairings(List<Pairing> pairings);
        List<DemoSchedule> getHardcodedListOfSchedules();
    }
}
