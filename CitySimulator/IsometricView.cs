
using System;
using SFML.Graphics;
using SFML.Window;

namespace CitySimulator {
    public class IsometricView {
        public const int TileWidth = 64;
        public const int TileHeight = 32;

        public const int HalfWidth = TileWidth / 2;
        public const int HalfHeight = TileHeight / 2;


        public Vector2f CoordinatesToTile(Vector2f vecIso) {
            vecIso.X /= TileWidth;
            vecIso.Y /= TileHeight;

            var vec2D = new Vector2f {
                X = (vecIso.Y + vecIso.X),
                Y = (vecIso.Y - vecIso.X)
            };
            return vec2D;
        }

        public Vector2f TileToCoordinates(Vector2f vec2D) {
            var vecIso = new Vector2f {
                X = (vec2D.X - vec2D.Y) * HalfWidth,
                Y = (vec2D.X + vec2D.Y) * HalfHeight
            };
            return vecIso;
        }

        public Vector2f TileToCoordinates(Vector2f vecIso, IntRect textureRect) {
            var vec2D = TileToCoordinates(vecIso);
            vec2D.X -= (textureRect.Width - TileWidth) / 2;
            vec2D.Y -= (textureRect.Height - textureRect.Width / 2);

            return vec2D;
        }
    }
}
