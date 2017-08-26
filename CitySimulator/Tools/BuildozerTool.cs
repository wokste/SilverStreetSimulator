using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Audio;

namespace CitySimulator.Tools
{
    class BuildozerTool : Tool
    {
        private Sound Sound;

        public BuildozerTool(SoundManager soundManager, ZoneType zone)
        {
            Sound = soundManager.GetSound(zone.BuildSoundName);
        }

        protected override void OnMouseDown(Game game, IsometricView view, Vector2f screenPos)
        {
            var tile = view.ScreenPxToWens(screenPos);
            game.City.Terrain[tile.X, tile.Y].Building = null;
        }

        protected override void OnMouseDrag(Game game, IsometricView view, Vector2f screenPos)
        {
            var tile = view.ScreenPxToWens(screenPos);
            game.City.Terrain[tile.X, tile.Y].Building = null;
        }

        protected override void OnMouseUp(Game game, IsometricView view, Vector2f screenPos)
        {

        }
    }
}
