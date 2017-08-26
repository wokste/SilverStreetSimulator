﻿using System;
using System.Diagnostics;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;

namespace CitySimulator.Tools {
    class BuildZoneTool : Tool {
        private readonly ZoneType _zone;

        private Sound Sound;
        private Vector2i _mouseDownPos;

        public BuildZoneTool(SoundManager soundManager, ZoneType zone) {
            _zone = zone;
            Sound = soundManager.GetSound(zone.BuildSoundName);
        }

        protected override void OnMouseDown(Game game, IsometricView view, Vector2f screenPos)
        {
            _mouseDownPos = view.ScreenPxToWens(screenPos);
        }

        protected override void OnMouseUp(Game game, IsometricView view, Vector2f screenPos)
        {
            var mouseTile = view.ScreenPxToWens(screenPos);

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

            Sound.Play();

            game.Money -= cost;

            FillZone(area, game.City);
        }


        protected override void OnMouseDrag(Game game, IsometricView view, Vector2f screenPos) {}

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
            /*
            var rnd = new Random();
            for (var x = area.Left; x < area.Left + area.Width; x++) {
                for (var y = area.Top; y < area.Top + area.Height; y++) {
                    city.PlaceBuilding(new Vector2i(x, y), _zone.GetRandom(rnd));
                }
            }*/

            for (var x = area.Left; x < area.Left + area.Width; x++){
                for (var y = area.Top; y < area.Top + area.Height; y++){
                    city.Terrain[x, y].Zone = _zone.Id;
                }
            }
        }
    }
}
