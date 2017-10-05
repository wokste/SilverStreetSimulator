﻿using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Linq;

namespace CitySimulator {
    class TkCityRenderer {
        private readonly CityMap _cityMap;

        private readonly Sun _sun = new Sun(LightName.Light0);
        private readonly Material _material = new Material();

        private readonly Mesh _heightMapMesh;
        private readonly Mesh _waterMesh;

        private double _time = 0;

        public TkCityRenderer(CityMap cityMap) {
            _cityMap = cityMap;

            _heightMapMesh = _cityMap.HeightMap.ToMesh();
            _heightMapMesh.Texture = new Texture("grass.png");

            var waterMeshFactory = new Mesh.Factory();
            waterMeshFactory.AddSurface(
                new Mesh.Vertex{Pos = Vector3.Zero, Normal = Vector3.UnitY, TexCoords = Vector2.Zero},
                new Mesh.Vertex { Pos = new Vector3(0, 0, 128), TexCoords = new Vector2(0, 32) },
                new Mesh.Vertex { Pos = new Vector3(128,0,0), TexCoords = new Vector2(32,0) });

            _waterMesh = waterMeshFactory.ToMesh();
            _waterMesh.Texture = new Texture("water.png");

        }

        public void Draw() {
            _sun.AddTime(1f);
            _sun.Update();
            _material.Update();

            DrawTerrain();
            DrawBuildings();
        }

        private void DrawTerrain()
        {
            _time++;

            _heightMapMesh.Render();

            // Water

            GL.PushMatrix();
            var waterLevel = -2 + Math.Sin(_time / 200.0) * 0.1;
            GL.Translate(0,waterLevel,0);
            _waterMesh.Render();
            GL.PopMatrix();
        }

        private void DrawBuildings() {
            for (var x = 0; x < _cityMap.SizeX; x++) {
                for (var y = 0; y < _cityMap.SizeY; y++) {
                    var building = _cityMap.Terrain[x, y].Building;

                    if (building == null)
                        continue;

                    //TODO: Find correct heightmap values
                    var height = new [] { _cityMap.HeightMap.Height[x, y], _cityMap.HeightMap.Height[x, y+1], _cityMap.HeightMap.Height[x+1, y], _cityMap.HeightMap.Height[x+1, y+1] }.Min();

                    var pos = new Vector3(x + 0.5f, height, y + 0.5f);

                    GL.PushMatrix();

                    GL.Translate(pos);

                    building.Mesh.Render();

                    GL.PopMatrix();
                }
            }
        }
    }
}
