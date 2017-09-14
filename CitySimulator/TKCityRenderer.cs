using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CitySimulator {
    class TkCityRenderer {
        private readonly CityMap _cityMap;

        private readonly Texture _tileSetSprite;
        private readonly Texture _buildingSprite;

        public TkCityRenderer(CityMap cityMap) {
            _cityMap = cityMap;

            _tileSetSprite = new Texture("Tileset.png");
            _buildingSprite = new Texture("buildings1x1.png");
        }

        public void Draw() {
            DrawTerrain();
            //DrawBuildingsIso();
        }

        private void DrawTerrain() {
            var area = GetRenderArea();

            _tileSetSprite.Bind();
            
            GL.Begin(PrimitiveType.Triangles);

            void Vertex3(float x, float y, float z)
            {
                GL.Vertex3(x, y, z);
            }

            void TexCoord2(int tileId, float dx, float dy) {
                var tex = new Vector2((tileId % 2 + dx) * 0.5f, (1 - tileId / 2 + dy) * 0.5f);
                GL.TexCoord2(tex);
            }

            for (var x = area.Left; x < area.Left + area.Width; x++) {
                for (var y = area.Top; y < area.Top + area.Height; y++)
                {
                    var tileId = _cityMap.Terrain[x, y].Terrain;
                    TexCoord2(tileId, 0, 0);
                    Vertex3(x, y, 0);
                    TexCoord2(tileId, 1, 0);
                    Vertex3(x + 1, y, 0);
                    TexCoord2(tileId, 0, 1);
                    Vertex3(x, y + 1, 0);
                    TexCoord2(tileId, 1, 0);
                    Vertex3(x + 1, y, 0);
                    TexCoord2(tileId, 0, 1);
                    Vertex3(x, y + 1, 0);
                    TexCoord2(tileId, 1, 1);
                    Vertex3(x + 1, y + 1, 0);
                }
            }

            GL.End();
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
            return new Rectangle(0,0, _cityMap.Width, _cityMap.Height);
            /*
            var screenX = 1024;// ScreenSize.X;
            var screenY = 768;// ScreenSize.Y;

            var corner00 = View.ScreenPxToWens(new Vector2(0, 0));
            var corner01 = View.ScreenPxToWens(new Vector2(0, screenY));
            var corner10 = View.ScreenPxToWens(new Vector2(screenX, 0));
            var corner11 = View.ScreenPxToWens(new Vector2(screenX, screenY));

            var x0 = Math.Max(corner00.X, 0);
            var x1 = Math.Min(corner11.X + 1, _cityMap.Width);
            var y0 = Math.Max(corner10.Y, 0);
            var y1 = Math.Min(corner01.Y + 1, _cityMap.Height);

            return new Rectangle(x0, y0, x1 - x0, y1 - y0);
            */
        }
    }
}
