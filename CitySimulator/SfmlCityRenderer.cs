using SFML.Graphics;
using SFML.Window;

namespace CitySimulator {
    class SfmlCityRenderer : Drawable {
        private readonly CityMap _cityMap;

        private int _tileSize = 32;

        public SfmlCityRenderer(CityMap cityMap) {
            _cityMap = cityMap;
        }

        public void Draw(RenderTarget target, RenderStates states) {
            target.Clear();

            DrawTerrain(target);
            DrawBuildings(target);
        }

        private void DrawTerrain(RenderTarget target) {
            var shape = new RectangleShape {
                Size = new Vector2f(1, 1) * _tileSize,
            };

            for (var x = 0; x < _cityMap.Width; x++) {
                for (var y = 0; y < _cityMap.Height; y++) {
                    shape.Position = new Vector2f(x, y) * _tileSize;

                    switch (_cityMap.Terrain[x, y]) {
                        case 1:
                            shape.FillColor = new Color(128, 255, 0);
                            break;
                        case 2:
                            shape.FillColor = new Color(255, 255, 128);
                            break;
                        case 3:
                            shape.FillColor = new Color(0, 128, 255);
                            break;
                        case 4:
                            shape.FillColor = new Color(64, 128, 0);
                            break;
                    }

                    

                    target.Draw(shape);
                }
            }
        }

        private void DrawBuildings(RenderTarget target) {
            foreach (var building in _cityMap.Buildings) {

                var shape = new RectangleShape {
                    Position = building.Position.ToVector2F() * _tileSize,
                    Size = building.Type.Size.ToVector2F() * _tileSize,
                    FillColor = new Color(255, 0, 0)
                };

                target.Draw(shape);
            }
        }
    }
}
