using SFML.Graphics;
using SFML.Window;

namespace CitySimulator {
    class SfmlCityRenderer : SFML.Graphics.Drawable {
        private City _city;

        public SfmlCityRenderer(City city) {
            _city = city;
        }

        public void Draw(RenderTarget target, RenderStates states) {
            target.Clear();

            DrawTerrain(target, states);
            DrawBuildings(target,states);
        }

        private void DrawTerrain(RenderTarget target, RenderStates states) {
            SFML.Graphics.RectangleShape shape = new RectangleShape();
            shape.Size = new Vector2f(32, 32);
            shape.FillColor = new Color(64,128,0);

            for (var x = 0; x < _city.Width; x++) {
                for (var y = 0; y < _city.Height; y++) {
                    shape.Position = new Vector2f(x, y) * 32;
                    

                    target.Draw(shape);
                }
            }
        }

        private void DrawBuildings(RenderTarget target, RenderStates states) {
            foreach (var building in _city.Buildings) {

                SFML.Graphics.RectangleShape shape = new RectangleShape();
                shape.Position = new Vector2f(building.Position.X, building.Position.Y) * 32;
                shape.Size = new Vector2f(64,64);
                shape.FillColor = new Color(255, 0, 0);

                target.Draw(shape);
            }
        }
    }
}
