using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace CitySimulator {
    internal class ZoneManager {
        private readonly List<ZoneType> _zoneTypes = new List<ZoneType>();
        
        internal ZoneType this[int key] => _zoneTypes[key];
        internal ZoneType this[string key] => _zoneTypes[GetId(key)];

        internal int GetId(string tag) {
            var i = _zoneTypes.FindIndex(z => z.Tag == tag);

            if (i == -1) {
                throw new IndexOutOfRangeException();
            }

            return i;
        }

        internal void Load(string fileName) {
            var stream = new FileStream(fileName, FileMode.Open);
            var doc = XDocument.Load(stream);
            Load(doc);
        }

        private void Load(XDocument doc) {
            var root = doc.Root;

            if (root == null) {
                throw new Exception("XML not well formatted. Root element missing.");
            }

            foreach (var zoneXmlElem in root.Elements()) {
                var zone = new ZoneType();

                zone.Load(zoneXmlElem);
                _zoneTypes.Add(zone);
            }
        }
    }
}
