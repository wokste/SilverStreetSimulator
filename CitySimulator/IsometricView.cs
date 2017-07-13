
using System;
using SFML.Graphics;
using SFML.Window;

namespace CitySimulator {
    class IsometricView {

        private const int TileWidth = 64;
        private const int TileHeight = 32;

        private const int HalfWidth = TileWidth / 2;
        private const int HalfHeight = TileHeight / 2;


        internal Vector2f CoordinatesToTile(Vector2f vecIso) {
            vecIso.X /= TileWidth;
            vecIso.Y /= TileHeight;

            var vec2D = new Vector2f {
                X = (vecIso.Y + vecIso.X),
                Y = (vecIso.Y - vecIso.X)
            };
            return vec2D;
        }

        internal Vector2f TileToCoordinates(Vector2f vec2D) {
            var vecIso = new Vector2f {
                X = (vec2D.X - vec2D.Y) * HalfWidth,
                Y = (vec2D.X + vec2D.Y) * HalfHeight
            };
            return vecIso;
        }
        
        internal Vector2f TileToCoordinates(Vector2f vecIso, IntRect textureRect) {
            var vec2D = TileToCoordinates(vecIso);
            vec2D.X -= (textureRect.Width - TileWidth) / 2;
            vec2D.Y -= (textureRect.Height - textureRect.Width / 2);

            return vec2D;
        }
    }
}
