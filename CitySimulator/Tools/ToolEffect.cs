using SFML.Window;
using System;

namespace CitySimulator.Tools {
    interface ToolEffect {
        int Cost { get; }

        TileFilterResult Filter(int x, int y);
        void Apply(int x, int y);
    }

    class PlaceZoneEffect : ToolEffect {
        ZoneType _zone;
        CityMap _city;

        public PlaceZoneEffect(CityMap city, ZoneType zone) {
            _city = city;
            _zone = zone;
        }
        
        int ToolEffect.Cost => _zone.BuildCost;

        public TileFilterResult Filter(int x, int y) {
            // Only build inside map.
            if (!_city.IsFreeArea(new Vector2i(x, y)))
                return TileFilterResult.TERF_NoBuild;

            // Don't build where there is already the same zone. Would increase cost.
            if (_city.Terrain[x, y].ZoneId == _zone.Id)
                return TileFilterResult.TERF_AlreadyExists;

            // Not placed on roads
            if (_city.Terrain[x, y].IsRoad())
                return TileFilterResult.TERF_NoBuild;

            return TileFilterResult.TERF_CanBuild;
        }

        public void Apply(int x, int y) {
            _city.Terrain[x, y].ZoneId = _zone.Id;
            _city.Terrain[x, y].Building = null;
        }
    }

    class PlaceRoadEffect : ToolEffect {
        ZoneType _zone;
        CityMap _city;
        Random _rnd = new Random(0);

        public PlaceRoadEffect(CityMap city, ZoneType zone) {
            _city = city;
            _zone = zone;
        }

        int ToolEffect.Cost => _zone.BuildCost;

        public TileFilterResult Filter(int x, int y) {
            // Don't build outside map
            if (!_city.IsFreeArea(new Vector2i(x, y)))
                return TileFilterResult.TERF_NoBuild;
            
            // Don't build where there is already a road.
            // TODO: If multiple types of roads are added, road value needs to be taken into account.
            if (_city.Terrain[x, y].ZoneId == _zone.Id)
                return TileFilterResult.TERF_AlreadyExists;
            
            return TileFilterResult.TERF_CanBuild;
        }

        public void Apply(int x, int y) {
            // This needs to be changed as I don't want buildings to be used for roads.
            _city.Terrain[x, y].Building = new Building {
                Type = _zone.GetRandom(_rnd)
            };
            _city.Terrain[x, y].ZoneId = _zone.Id;
        }
    }

    
    class DestroyEffect : ToolEffect {
        private CityMap _city;

        int ToolEffect.Cost => 5;

        public DestroyEffect(CityMap city) {
            _city = city;
        }

        public TileFilterResult Filter(int x, int y) {
            if (!_city.IsFreeArea(new Vector2i(x, y)))
                return TileFilterResult.TERF_NoBuild;

            // Don't destroy empty tiles
            if (_city.Terrain[x, y].ZoneId < 0 && _city.Terrain[x, y].Building == null)
                return TileFilterResult.TERF_AlreadyExists;

            return TileFilterResult.TERF_CanBuild;
        }

        public void Apply(int x, int y) {
            _city.Terrain[x, y].ZoneId = -1;
            _city.Terrain[x, y].Building = null;
        }
    }
}
