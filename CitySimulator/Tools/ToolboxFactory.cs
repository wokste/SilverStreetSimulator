using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitySimulator.Tools {
    class ToolboxFactory {
        public static List<Tool> getTools(CityMap city, ZoneManager zones) {
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
