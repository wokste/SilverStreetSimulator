using System;
using System.Diagnostics;
using SFML.Graphics;
using SFML.Window;

namespace CitySimulator.Tools {
    class BuildZoneTool {
        private readonly ZoneType _zone;

        private Vector2i _mouseDownPos;
        private bool _mouseDown = false;

        public BuildZoneTool(ZoneType zone) {
            _zone = zone;
        }

        public void MouseDown(Game game, Vector2i mouseTile) {
            _mouseDownPos = mouseTile;
            _mouseDown = true;
        }

        public void MouseUp(Game game, Vector2i mouseTile) {
            if (!_mouseDown) {
                return;
            }
            _mouseDown = false;

            var area = new IntRect {
                Left = Math.Min(_mouseDownPos.X, mouseTile.X),
                Width = Math.Abs(_mouseDownPos.X - mouseTile.X) + 1,

                Top = Math.Min(_mouseDownPos.Y, mouseTile.Y),
                Height = Math.Abs(_mouseDownPos.Y - mouseTile.Y) + 1,
            };

            double cost = CalculateCost(area, game.City);
            if (cost > game.Money) {
                Debug.WriteLine($"insufficient money. Need: {cost} Has {game.Money}");
                return;
            }

            game.Money -= cost;

            FillZone(area, game.City);
        }

        private int CalculateCost(IntRect area, CityMap city) {
            var cells = 0;
            for (var x = area.Left; x < area.Left + area.Width; x++) {
                for (var y = area.Top; y < area.Top + area.Height; y++) {
                    if (city.IsFreeArea(new Vector2i(x, y), new Vector2i(1, 1))) {
                        cells++;
                    }
                }
            }
            return cells * _zone.BuildCost;
        }

        private void FillZone(IntRect area, CityMap city) {
            var rnd = new Random();
            for (var x = area.Left; x < area.Left + area.Width; x++) {
                for (var y = area.Top; y < area.Top + area.Height; y++) {
                    city.PlaceBuilding(new Vector2i(x, y), _zone.GetRandom(rnd));
                }
            }
        }

        public void MouseCancel() {
            _mouseDown = false;
        }
    }
}
