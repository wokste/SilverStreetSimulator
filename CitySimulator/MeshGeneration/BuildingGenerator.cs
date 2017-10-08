using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitySimulator.Render;
using OpenTK;

namespace CitySimulator.MeshGeneration
{
    class BuildingGenerator
    {
        private readonly BuildingPlanGenerator _planGenerator;
        private readonly WallGenerator _wallGenerator;
        private readonly RoofGenerator _roofGenerator;

        public BuildingGenerator(Random rnd = null)
        {
            var rnd1 = rnd ?? new Random();
            _planGenerator = new BuildingPlanGenerator(rnd1);
            _wallGenerator = new WallGenerator(rnd1);
            _roofGenerator = new RoofGenerator(rnd1);
        }

        public Mesh GenerateMesh(Building building, CityMap map)
        {
            var factory = new Mesh.Factory();

            var plan = _planGenerator.Generate(building.Pos + new Vector2(0.5f,0.5f), building.Type.Size);

            UpdateHeights(plan, map.HeightMap);

            _wallGenerator.CreateWalls(factory, plan);
            _roofGenerator.CreateRoof(factory, plan);

            return factory.ToMesh();
        }

        private void UpdateHeights(BuildingPlan plan, HeightMap heightMap)
        {
            var levels = plan.Polygon.Corners.Select(v => heightMap.GetHeight(v));

            plan.GroundLevel = levels.Max();
            plan.LowGroundLevel = levels.Min();
        }
    }
}
