
using System;
using SFML.Graphics;
using SFML.Window;

namespace CitySimulator {
    public class IsometricView {
        public const int TileWidth = 64;
        public const int TileHeight = 32;

        public const int HalfWidth = TileWidth / 2;
        public const int HalfHeight = TileHeight / 2;

        /// <summary>
        /// Converts tile positions in pixels to world positions.
        /// </summary>
        /// <param name="vecPx">A vector in pixel coordinates on the texture. Zooming and padding shouldn't change this</param>
        /// <returns>A vector in Wens coordinates. X represent distance in west-east direction. Y represents distance in north-south direction</returns>
        public Vector2i PxToWens(Vector2f vecPx) {
            vecPx.X /= TileWidth;
            vecPx.Y /= TileHeight;

            vecPx.X -= 0.5f;

            var vecWens = new Vector2i {
                X = (int)Math.Floor(vecPx.Y + vecPx.X),
                Y = (int)Math.Floor(vecPx.Y - vecPx.X)
            };
            return vecWens;
        }

        /// <summary>
        /// Converts tile positions in world positions to pixels.
        /// </summary>
        /// <param name="vecWens">A vector in Wens coordinates. X represent distance in west-east direction. Y represents distance in north-south direction</param>
        /// <returns>A vector in pixel coordinates on the texture. Zooming and padding doesn't change this</returns>
        public Vector2f WensToPx(Vector2i vecWens) {
            var vecPx = new Vector2f {
                X = (vecWens.X - vecWens.Y) * HalfWidth,
                Y = (vecWens.X + vecWens.Y) * HalfHeight
            };
            return vecPx;
        }
        
        /// <summary>
        /// Converts tile positions in world positions to pixels.
        /// </summary>
        /// <param name="vecWens">A vector in Wens coordinates. X represent distance in west-east direction. Y represents distance in north-south direction</param>
        /// <returns>A vector in pixel coordinates on the texture. Zooming and padding doesn't change this</returns>
        /// <param name="textureRect"></param>
        /// <returns></returns>
        public Vector2f WensToPx(Vector2i vecWens, IntRect textureRect) {
            var vecPx = WensToPx(vecWens);
            vecPx.X -= (textureRect.Width - TileWidth) / 2;
            vecPx.Y -= textureRect.Height - textureRect.Width / 2;

            return vecPx;
        }
    }
}
