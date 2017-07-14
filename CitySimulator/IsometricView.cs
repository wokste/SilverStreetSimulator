
using System;
using SFML.Graphics;
using SFML.Window;

namespace CitySimulator {
    public class IsometricView {
        //public Vector2f ScreenSize;
        public float Zoom = 1;
        public Vector2f TopLeftPos;

        public const int TileWidth = 64;
        public const int TileHeight = 32;

        public const int HalfWidth = TileWidth / 2;
        public const int HalfHeight = TileHeight / 2;

        /// <summary>
        /// Converts wens coordinates to screen pixels
        /// </summary>
        /// <param name="vecWens">A vector in Wens coordinates. X represent distance in west-east direction. Y represents distance in north-south direction</param>
        /// <returns></returns>
        public Vector2f WensToScreenPx(Vector2i vecWens) {
            return WorldPxToScreenPx(WensToWorldPx(vecWens));
        }

        /// <summary>
        /// Converts world coordinates to screen coordinates
        /// </summary>
        /// <param name="worldPx">Coordinates in world positions</param>
        public Vector2f WorldPxToScreenPx(Vector2f worldPx) {
            var screenPx = (worldPx - TopLeftPos) / Zoom;
            return screenPx;
        }

        /// <summary>
        /// Converts screen coordinates to world coordinates
        /// </summary>
        /// <param name="screenPx">Coordinates in screen positions</param>
        /// <returns></returns>
        public Vector2f ScreenPxToWorldPx(Vector2f screenPx) {
            var worldPx = screenPx * Zoom + TopLeftPos;
            return worldPx;
        }

        /// <summary>
        /// Converts tile positions in pixels to world positions.
        /// </summary>
        /// <param name="vecWorldPx">A vector in pixel coordinates on the texture. Zooming and padding shouldn't change this</param>
        /// <returns>A vector in Wens coordinates. X represent distance in west-east direction. Y represents distance in north-south direction</returns>
        public Vector2i WorldPxToWens(Vector2f vecWorldPx) {
            vecWorldPx.X /= TileWidth;
            vecWorldPx.Y /= TileHeight;

            vecWorldPx.X -= 0.5f;

            var vecWens = new Vector2i {
                X = (int)Math.Floor(vecWorldPx.Y + vecWorldPx.X),
                Y = (int)Math.Floor(vecWorldPx.Y - vecWorldPx.X)
            };
            return vecWens;
        }

        /// <summary>
        /// Converts tile positions in world positions to pixels.
        /// </summary>
        /// <param name="vecWens">A vector in Wens coordinates. X represent distance in west-east direction. Y represents distance in north-south direction</param>
        /// <returns>A vector in pixel coordinates on the texture. Zooming and padding doesn't change this</returns>
        public Vector2f WensToWorldPx(Vector2i vecWens) {
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
        public Vector2f WensToWorldPx(Vector2i vecWens, IntRect textureRect) {
            var vecPx = WensToWorldPx(vecWens);
            vecPx.X -= (textureRect.Width - TileWidth) / 2.0f;
            vecPx.Y -= textureRect.Height - textureRect.Width / 2;

            return vecPx;
        }
    }
}
