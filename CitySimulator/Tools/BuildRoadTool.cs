using System;
using SFML.Window;
using SFML.Audio;
using SFML.Graphics;

namespace CitySimulator.Tools
{
    class BuildRoadTool : Tool
    {
        private readonly ZoneType _zone;

        private Sound Sound;
        private Vector2i _mouseDownTile;

        public BuildRoadTool(SoundManager soundManager, ZoneType zone){
            Sound = soundManager.GetSound(zone.BuildSoundName);
            _zone = zone;
        }

        protected override void OnMouseDown(Game game, IsometricView view, Vector2f screenPos){
            _mouseDownTile = view.ScreenPxToWens(screenPos);
        }

        protected override void OnMouseDrag(Game game, IsometricView view, Vector2f screenPos)
        {
        }

        protected override void OnMouseUp(Game game, IsometricView view, Vector2f screenPos){
            var mouseTile = view.ScreenPxToWens(screenPos);

            var diff = mouseTile - _mouseDownTile;

            if (Math.Abs(diff.X) > Math.Abs(diff.Y)) {
                var mid = new Vector2i(_mouseDownTile.X, mouseTile.Y);
                PlaceRoadYAxis(game.City, _mouseDownTile, mid);
                PlaceRoadXAxis(game.City, mid, mouseTile);
            } else {

                var mid = new Vector2i(mouseTile.X, _mouseDownTile.Y);
                //PlaceRoadYAxis(game.City, _mouseDownTile, mouseTile);
                PlaceRoadXAxis(game.City, _mouseDownTile, mid);
                PlaceRoadYAxis(game.City, mid, mouseTile);
            }
        }

        private void PlaceRoadXAxis(CityMap city, Vector2i p1, Vector2i p2){
            var minX = Math.Min(p1.X, p2.X);
            var maxX = Math.Max(p1.X, p2.X);

            for (int x = minX; x <= maxX; x++){
                PlaceRoad(city, x, p1.Y);
            }
        }

        private void PlaceRoadYAxis(CityMap city, Vector2i p1, Vector2i p2) {
            var minY = Math.Min(p1.Y, p2.Y);
            var maxY = Math.Max(p1.Y, p2.Y);

            for (int y = minY; y <= maxY; y++) {
                PlaceRoad(city, p1.X, y);
            }
        }

        private void PlaceRoad(CityMap city, int x, int y) {
            city.PlaceBuilding(new Vector2i(x, y), _zone.GetRandom(new Random()));
        }
    }
}
