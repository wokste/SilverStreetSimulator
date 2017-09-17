using System.Collections.Generic;

namespace CitySimulator
{
    class GrowthSimulator
    {
        readonly CityMap _map;

        readonly List<Desire.CityActor> _actors = new List<Desire.CityActor>();

        internal GrowthSimulator(CityMap map, ZoneManager zoneManager) {
            _map = map;

            _actors.Add(new Desire.CityActor(zoneManager[0]));
            _actors.Add(new Desire.CityActor(zoneManager[1]));
            _actors.Add(new Desire.CityActor(zoneManager[2]));
            _actors.Add(new Desire.CityActor(zoneManager[3]));
        }

        internal void Update(long timeMs)
        {
            foreach (var actor in _actors){
                actor.Update(_map, timeMs);
            }
        }
    }
}
