using System;
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

        public void MouseDown(CityMap city, Vector2i mouseTile) {
            _mouseDownPos = mouseTile;
            _mouseDown = true;
        }

        public void MouseUp(CityMap city, Vector2i mouseTile) {
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

            FillZone(area, city);
        }

        private void FillZone(IntRect area, CityMap city) {
            Random rnd = new Random();
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
