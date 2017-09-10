using System.Collections.Generic;

namespace CitySimulator.Tools {
    class ToolboxFactory {
        public static List<Tool> GetTools(CityMap city, ZoneManager zones) {
            var list = new List<Tool>();

            list.Add(new TileTool(new RectangeAreaSelector(), new PlaceZoneEffect(city, zones[0])));
            list.Add(new TileTool(new RectangeAreaSelector(), new PlaceZoneEffect(city, zones[1])));
            list.Add(new TileTool(new RectangeAreaSelector(), new PlaceZoneEffect(city, zones[2])));
            list.Add(new TileTool(new RectangeAreaSelector(), new PlaceZoneEffect(city, zones[3])));

            list.Add(new TileTool(new LineAreaSelector(), new PlaceRoadEffect(city, zones[4])));
            list.Add(new TileTool(new RectangeAreaSelector(), new DestroyEffect(city)));

            return list;
        }
    }
}
