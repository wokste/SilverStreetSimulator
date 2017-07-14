using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SFML.Graphics;
using SFML.Window;

namespace CitySimulator {
    class ZoneType {
        internal string Tag { get; private set; }
        internal string Name { get; private set; }
        public int BuildCost { get; internal set; }

        private readonly List<BuildingType> _buildings = new List<BuildingType>();

        internal BuildingType GetRandom(Random rnd) {
            return _buildings[rnd.Next(_buildings.Count)];
        }

        internal void Load(XElement zoneXmlElem) {
            Tag = zoneXmlElem.GetString("name");
            Name = zoneXmlElem.GetString("name",true);
            BuildCost = zoneXmlElem.GetInt("build_cost");

            foreach (var buildingGroupElem in zoneXmlElem.Elements()) {
                var sizeArray = buildingGroupElem.Attribute("size").Value.Split('x').Select(int.Parse).ToArray();
                var size = new Vector2i(sizeArray[0], sizeArray[1]);

                var population = buildingGroupElem.GetInt("population",true);
                var jobs = buildingGroupElem.GetInt("jobs", true);

                foreach (var textureElem in buildingGroupElem.Elements()) {
                    var left = textureElem.GetInt("left");
                    var top = textureElem.GetInt("top");
                    var width = textureElem.GetInt("width");
                    var height = textureElem.GetInt("height");

                    var b = new BuildingType {
                        Size = size,
                        TextureRect = new IntRect(left,top,width,height),
                        Population = population,
                        Jobs = jobs
                    };

                    _buildings.Add(b);
                }
            }
        }
    }
}
