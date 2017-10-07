using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OpenTK;

namespace CitySimulator {
    class ZoneType {
        internal string Tag { get; private set; }
        internal string Name { get; private set; }
        public int BuildCost { get; private set; }
        public string BuildSoundName { get; private set; }
        public int Id { get; }

        private readonly List<BuildingType> _buildings = new List<BuildingType>();

        internal ZoneType(int id) {
            Id = id;
        }

        internal BuildingType GetRandom(Random rnd) {
            return _buildings[rnd.Next(_buildings.Count)];
        }

        internal void Load(XElement zoneXmlElem) {
            Tag = zoneXmlElem.GetString("name");
            Name = zoneXmlElem.GetString("name",true);
            BuildCost = zoneXmlElem.GetInt("build_cost");
            BuildSoundName = zoneXmlElem.GetString("sound", true);

            foreach (var buildingGroupElem in zoneXmlElem.Elements()) {
                var sizeArray = buildingGroupElem.Attribute("size").Value.Split('x').Select(float.Parse).ToArray();
                var size = new Vector2(sizeArray[0], sizeArray[1]);

                var population = buildingGroupElem.GetInt("population",true);
                var jobs = buildingGroupElem.GetInt("jobs", true);
                var height = buildingGroupElem.GetFloat("height");

                var b = new BuildingType
                {
                    Size = size,
                    Population = population,
                    Jobs = jobs,
                    Height = height
                };

                _buildings.Add(b);
            }
        }
    }
}
