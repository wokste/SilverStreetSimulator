using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Linq;

namespace CitySimulator {
    class TkCityRenderer {
        private readonly CityMap _cityMap;

        private Light _sun = new Light(LightName.Light0);
        private Material _material = new Material();

        private readonly Texture _tileSetSprite;
        private readonly Texture _buildingSprite;
        private Mesh _heightMapMesh;
        private double _time = 0;

        public TkCityRenderer(CityMap cityMap) {
            _cityMap = cityMap;

            _tileSetSprite = new Texture("grass.png");
            _buildingSprite = new Texture("buildings1x1.png");

            _heightMapMesh = _cityMap.HeightMap.ToMesh();
        }

        public void Draw() {
            _sun.Update();
            _material.Update();

            DrawTerrain();
            DrawBuildings();
        }

        private void DrawTerrain()
        {
            _time++;
            _tileSetSprite.Bind();

            _heightMapMesh.Render();

            // Water

            GL.Disable(EnableCap.Texture2D);
            GL.Color4((byte)128, (byte)192, (byte)255, (byte)128);

            var waterLevel = -2f + (float)Math.Sin(_time / 200.0) * 0.1;

            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(0, waterLevel, 0);
            GL.Vertex3(0, waterLevel, 128);
            GL.Vertex3(128, waterLevel, 128);
            GL.Vertex3(128, waterLevel, 0);
            GL.End();

            GL.Color4((byte)255, (byte)255, (byte)255, (byte)255);
            GL.Enable(EnableCap.Texture2D);
        }

        private void DrawBuildings() {
            
            GL.Disable(EnableCap.Texture2D);

            for (var x = 0; x < _cityMap.SizeX; x++) {
                for (var y = 0; y < _cityMap.SizeY; y++) {
                    var building = _cityMap.Terrain[x, y].Building;

                    if (building == null)
                        continue;

                    GL.Color3((byte)((x * 33 + y * 127) % 128 + 127), (byte)((x * 49 + y * 33) % 128 + 127), (byte)((x * 17 + y * 42) % 128 + 127));

                    //TODO: Find correct heightmap values
                    var height = new [] { _cityMap.HeightMap.Height[x, y], _cityMap.HeightMap.Height[x, y+1], _cityMap.HeightMap.Height[x+1, y], _cityMap.HeightMap.Height[x+1, y+1] }.Min();

                    var pos = new Vector3(x + 0.5f, height, y + 0.5f);

                    GL.PushMatrix();

                    GL.Translate(pos);

                    building.Mesh.Render();

                    GL.PopMatrix();
                }
            }

            GL.Color3((byte)255, (byte)255, (byte)255);
            GL.Enable(EnableCap.Texture2D);
        }
    }
}
