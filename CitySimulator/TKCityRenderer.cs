using System;
using SFML.Window;
using SFML.Graphics;

namespace CitySimulator {
    class TkCityRenderer {
        private readonly CityMap _cityMap;
        public IsometricView View = new IsometricView();

        private readonly Texture _tileSetSprite;
        private readonly Texture _buildingSprite;

        public TkCityRenderer(CityMap cityMap) {
            _cityMap = cityMap;

            _tileSetSprite = new Texture("TilesetIso.png");
            _buildingSprite = new Texture("buildings1x1.png");
        }

        public void Draw() {
            DrawTerrainIso();
            DrawBuildingsIso();
        }

        private void DrawTerrainIso() {
            var area = GetRenderArea();

            for (var x = area.Left; x < area.Left + area.Width; x++) {
                for (var y = area.Top; y < area.Top + area.Height; y++) {
                    var vec2D = new Vector2i(x, y);
                    var vecIso = View.WensToScreenPx(vec2D);

                    var tileId = _cityMap.Terrain[x, y].Terrain;
                    var textureRect = new IntRect((tileId % 2) * 64, (tileId / 2) * 32, 64, 32);

                    _tileSetSprite.Render2D(vecIso, textureRect, new Vector2f(1 / View.Zoom, 1 / View.Zoom));
                }
            }
        }

        private void DrawBuildingsIso() {
            var area = GetRenderArea();

            for (var x = area.Left; x < area.Left + area.Width; x++) {
                for (var y = area.Top; y < area.Top + area.Height; y++) {
                    var building = _cityMap.Terrain[x, y].Building;

                    if (building == null)
                        continue;

                    var vecWens = new Vector2i(x, y);
                    var vecWorld = View.WensToWorldPx(vecWens, building.Type.TextureRect);
                    var vecScreen = View.WorldPxToScreenPx(vecWorld);

                    _buildingSprite.Render2D(vecScreen, building.Type.TextureRect, new Vector2f(1 / View.Zoom, 1 / View.Zoom));
                }
            }
        }

        private IntRect GetRenderArea() {
            return new IntRect(0,0, _cityMap.Width, _cityMap.Height);

            var screenX = 1024;// ScreenSize.X;
            var screenY = 768;// ScreenSize.Y;

            var corner00 = View.ScreenPxToWens(new Vector2f(0, 0));
            var corner01 = View.ScreenPxToWens(new Vector2f(0, screenY));
            var corner10 = View.ScreenPxToWens(new Vector2f(screenX, 0));
            var corner11 = View.ScreenPxToWens(new Vector2f(screenX, screenY));

            var x0 = Math.Max(corner00.X, 0);
            var x1 = Math.Min(corner11.X + 1, _cityMap.Width);
            var y0 = Math.Max(corner10.Y, 0);
            var y1 = Math.Min(corner01.Y + 1, _cityMap.Height);

            return new IntRect(x0, y0, x1 - x0, y1 - y0);
        }
    }
}
