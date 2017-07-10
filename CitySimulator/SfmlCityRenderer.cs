using SFML.Graphics;
using SFML.Window;

namespace CitySimulator {
    class SfmlCityRenderer : Drawable {
        private readonly City _city;

        public SfmlCityRenderer(City city) {
            _city = city;
        }

        public void Draw(RenderTarget target, RenderStates states) {
            target.Clear();

            DrawTerrain(target);
            DrawBuildings(target);
        }

        private void DrawTerrain(RenderTarget target) {
            var shape = new RectangleShape {
                Size = new Vector2f(1, 1),
                FillColor = new Color(64, 128, 0)
            };

            for (var x = 0; x < _city.Width; x++) {
                for (var y = 0; y < _city.Height; y++) {
                    shape.Position = new Vector2f(x, y);
                    
                    target.Draw(shape);
                }
            }
        }

        private void DrawBuildings(RenderTarget target) {
            foreach (var building in _city.Buildings) {

                var shape = new RectangleShape {
                    Position = building.Position.ToVector2F(),
                    Size = building.Type.Size.ToVector2F(),
                    FillColor = new Color(255, 0, 0)
                };

                target.Draw(shape);
            }
        }
    }
}
