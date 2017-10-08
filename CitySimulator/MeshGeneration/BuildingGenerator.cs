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
        private readonly Random _rnd;
        private readonly BuildingPlanGenerator _planGenerator;
        private readonly WallGenerator _wallGenerator;
        private readonly RoofGenerator _roofGenerator;

        public BuildingGenerator(Random rnd = null)
        {
            _rnd = rnd ?? new Random();
            _planGenerator = new BuildingPlanGenerator(_rnd);
            _wallGenerator = new WallGenerator(_rnd);
            _roofGenerator = new RoofGenerator(_rnd);
        }

        public Mesh GenerateMesh(Building building, CityMap map)
        {
            var factory = new Mesh.Factory();

            var plan = _planGenerator.Generate(building.Pos + new Vector2(0.5f,0.5f), building.Type.Size);

            plan.NumFloors = _rnd.Next(1, 3);

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
