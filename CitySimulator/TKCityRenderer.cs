using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace CitySimulator {
    class TkCityRenderer {
        private readonly CityMap _cityMap;

        private readonly Texture _tileSetSprite;
        private readonly Texture _buildingSprite;
        private Mesh _heightMapMesh;
        private double _time = 0;

        private Dictionary<Building, Mesh> _buildingMeshes = new Dictionary<Building, Mesh>();

        public TkCityRenderer(CityMap cityMap) {
            _cityMap = cityMap;

            _tileSetSprite = new Texture("grass.png");
            _buildingSprite = new Texture("buildings1x1.png");

            _heightMapMesh = _cityMap.HeightMap.ToMesh();
        }

        public void Draw() {
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
            var area = GetRenderArea();
            
            for (var x = area.Left; x < area.Left + area.Width; x++) {
                for (var y = area.Top; y < area.Top + area.Height; y++) {
                    var building = _cityMap.Terrain[x, y].Building;

                    if (building == null)
                        continue;

                    var pos = new Vector3(x + 0.5f, y + 0.5f, 0);
                    

                    //_buildingSprite.Render2D(vecScreen, building.Type.TextureRect, new Vector2(1 / View.Zoom, 1 / View.Zoom));
                }
            }
        }

        private Rectangle GetRenderArea() {
            return new Rectangle(0,0, _cityMap.SizeX, _cityMap.SizeY);
        }
    }
}
