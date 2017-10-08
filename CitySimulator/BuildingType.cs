using System;
using CitySimulator.MeshGeneration;
using CitySimulator.Render;
using OpenTK;

namespace CitySimulator {
    class BuildingType {
        internal int Jobs;
        internal int Population;
        internal Vector2 Size = new Vector2(2,2);
        internal float Height;
    }
}
