using SFML.Graphics;
using SFML.Window;
using Image = SFML.Graphics.Image;

namespace CitySimulator {
    class SfmlCityRenderer : Drawable {
        private readonly CityMap _cityMap;
        private IsometricView _isometricView = new IsometricView();

        private readonly Sprite _tileSetSprite;
        private readonly Sprite _buildingSprite;

        public SfmlCityRenderer(CityMap cityMap) {
            _cityMap = cityMap;
            
            _tileSetSprite = MakeSprite("TilesetIso.png");
            _buildingSprite = MakeSprite("buildings1x1.png");
        }

        private Sprite MakeSprite(string texName) {
            var image = new Image($"D:\\AppData\\Local\\CitySimulator\\Assets\\{texName}");
            var texture = new Texture(image);
            return new Sprite {
                Texture = texture
            };
        }

        public void Draw(RenderTarget target, RenderStates states) {
            target.Clear();

            DrawTerrainIso(target);
            DrawBuildingsIso(target);
        }

        private void DrawTerrainIso(RenderTarget target) {
            var area = GetRenderArea(target);

            for (var x = area.Left; x < area.Left + area.Width; x++) {
                for (var y = area.Top; y < area.Top + area.Height; y++) {
                    var vec2D = new Vector2i(x, y);
                    var vecIso = _isometricView.WensToPx(vec2D);

                    _tileSetSprite.Position = vecIso;

                    var tileId = _cityMap.Terrain[x, y].Terrain;
                    _tileSetSprite.TextureRect = new IntRect((tileId % 2) * 64, (tileId / 2) * 32, 64, 32);

                    target.Draw(_tileSetSprite);
                }
            }
        }


        private void DrawBuildingsIso(RenderTarget target) {
            var area = GetRenderArea(target);

            for (var x = area.Left; x < area.Left + area.Width; x++) {
                for (var y = area.Top; y < area.Top + area.Height; y++) {
                    var building = _cityMap.Terrain[x, y].Building;

                    if (building == null)
                        continue;

                    var vec2D = new Vector2i(x,y);
                    var vecIso = _isometricView.WensToPx(vec2D, building.Type.TextureRect);

                    _buildingSprite.Position = vecIso;

                    _buildingSprite.TextureRect = building.Type.TextureRect;
                    
                    target.Draw(_buildingSprite);
                }
            }
        }

        private IntRect GetRenderArea(RenderTarget target) {
            return new IntRect(0,0, _cityMap.Width, _cityMap.Height);
        }
    }
}
