using System;
using SFML.Window;
using Image = SFML.Graphics.Image;
using SFML.Graphics;

namespace CitySimulator {
    class TkCityRenderer {
        private readonly CityMap _cityMap;
        public IsometricView View = new IsometricView();

        //private readonly Sprite _tileSetSprite;
        //private readonly Sprite _buildingSprite;

        public TkCityRenderer(CityMap cityMap) {
            _cityMap = cityMap;

            //_tileSetSprite = MakeSprite("TilesetIso.png");
            //_buildingSprite = MakeSprite("buildings1x1.png");
        }

        private Sprite MakeSprite(string texName) {
            var image = new Image($"{Program.AssetsFolder}{texName}");
            var texture = new Texture(image);
            return new Sprite {
                Texture = texture
            };
        }

        public void Draw() {
            DrawTerrainIso();
            DrawBuildingsIso();
        }

        private void DrawTerrainIso() {
            var area = GetRenderArea();

            //_tileSetSprite.Scale = new Vector2f(1 / View.Zoom, 1 / View.Zoom);

            for (var x = area.Left; x < area.Left + area.Width; x++) {
                for (var y = area.Top; y < area.Top + area.Height; y++) {
                    var vec2D = new Vector2i(x, y);
                    var vecIso = View.WensToScreenPx(vec2D);

                    //_tileSetSprite.Position = vecIso;

                    var tileId = _cityMap.Terrain[x, y].Terrain;
                    //_tileSetSprite.TextureRect = new IntRect((tileId % 2) * 64, (tileId / 2) * 32, 64, 32);

                    //target.Draw(_tileSetSprite);
                }
            }
        }

        private void DrawBuildingsIso() {
            var area = GetRenderArea();

            //_buildingSprite.Scale = new Vector2f(1 / View.Zoom, 1 / View.Zoom);

            for (var x = area.Left; x < area.Left + area.Width; x++) {
                for (var y = area.Top; y < area.Top + area.Height; y++) {
                    var building = _cityMap.Terrain[x, y].Building;

                    if (building == null)
                        continue;

                    var vecWens = new Vector2i(x, y);
                    var vecWorld = View.WensToWorldPx(vecWens, building.Type.TextureRect);
                    var vecScreen = View.WorldPxToScreenPx(vecWorld);

                    //_buildingSprite.Position = vecScreen;

                    //_buildingSprite.TextureRect = building.Type.TextureRect;

                    //target.Draw(_buildingSprite);
                }
            }
        }

        private IntRect GetRenderArea() {
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
