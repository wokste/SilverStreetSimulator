using System;
using System.Drawing;
using SFML.Graphics;
using SFML.Window;
using Color = SFML.Graphics.Color;
using Image = SFML.Graphics.Image;

namespace CitySimulator {
    class SfmlCityRenderer : Drawable {
        private readonly CityMap _cityMap;

        private int _tileSize = 32;

        private Texture tileSet;
        private Sprite tileSetSprite;
        private Texture buildings;
        private Sprite buildingSprite;

        public SfmlCityRenderer(CityMap cityMap) {
            _cityMap = cityMap;
            Image image = new Image(@"D:\AppData\Local\CitySimulator\Assets\TilesetIso.png");
            tileSet = new Texture(image);
            tileSetSprite = new Sprite {
                Texture = tileSet
            };

            image = new Image(@"D:\AppData\Local\CitySimulator\Assets\Buildings1x1.png");
            buildings = new Texture(image);
            buildingSprite = new Sprite {
                Texture = buildings
            };
        }

        public void Draw(RenderTarget target, RenderStates states) {
            target.Clear();

            DrawTerrainIso(target);
            DrawBuildingsIso(target);
        }

        private void DrawTerrainIso(RenderTarget target) {
            var x0 = 0;
            var x1 = _cityMap.Width;

            var y0 = 0;
            var y1 = _cityMap.Height;

            for (var x = x0; x < x1; x++) {
                for (var y = y0; y < y1; y++) {
                    var vec2D = new Vector2f(x, y);
                    var vecIso = ToIso(vec2D);
                    vecIso.X *= 32;
                    vecIso.Y *= 16;

                    tileSetSprite.Position = vecIso;

                    var tileId = _cityMap.Terrain[x, y];
                    tileSetSprite.TextureRect = new IntRect((tileId % 2) * 64, (tileId / 2) * 32, 64, 32);

                    target.Draw(tileSetSprite);
                }
            }
        }

        Vector2f To2D(Vector2f vecIso) {
            var vec2D = new Vector2f {
                X = (vecIso.Y + vecIso.X) / 2,
                Y = (vecIso.Y - vecIso.X) / 2
            };
            return vec2D;
        }

        Vector2f ToIso(Vector2f vec2D) {
            var vecIso = new Vector2f { 
                X = vec2D.X - vec2D.Y,
                Y = vec2D.X + vec2D.Y
            };
            return vecIso;
        }

        private void DrawBuildingsIso(RenderTarget target) {
            foreach (var building in _cityMap.Buildings) {
                var vec2D = building.Position.ToVector2F();
                var vecIso = ToIso(vec2D);
                vecIso.X *= 32;
                vecIso.Y *= 16;

                vecIso.Y -= building.Type.Height; // Adjust for building height

                buildingSprite.Position = vecIso;

                buildingSprite.TextureRect = building.Type.TextureRect;


                target.Draw(buildingSprite);
            }
        }
    }
}
