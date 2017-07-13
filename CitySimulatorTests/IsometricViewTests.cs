using System;
using CitySimulator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFML.Graphics;
using SFML.Window;

namespace CitySimulatorTests {
    [TestClass()]
    public class IsometricViewTests {

        /// <summary>
        /// Tests whether coordinates convert back accurately
        /// </summary>
        [TestMethod()]
        public void CoordinatesToTileTest() {
            var view = new IsometricView();

            var tile2D = new Vector2f(1, 3);
            var tileIso = view.PxToWens(tile2D);
            var tile2DCopy = view.WensToPx(tileIso);

            Assert.AreEqual(tile2D, tile2DCopy);
        }

        /// <summary>
        /// Test small building sizes
        /// </summary>
        [TestMethod()]
        public void SmallSizeTileTest() {
            var view = new IsometricView();

            var tile2D = new Vector2f(1, 3);
            var tileIsoBasic = view.WensToPx(tile2D);
            var rect = new IntRect(234, 12, IsometricView.TileWidth, IsometricView.TileHeight);
            var tileIsoAdvanced = view.WensToPx(tile2D, rect);

            Assert.AreEqual(tileIsoBasic, tileIsoAdvanced);
        }

        /// <summary>
        /// Test larger building sizes
        /// </summary>
        [TestMethod()]
        public void LargeSizeTileTest() {
            var view = new IsometricView();
            var tile2D = new Vector2f(1, 3);
            
            const int size = 3;

            var tile2DCorners = new[] {
                tile2D,
                tile2D + new Vector2f(0, size - 1),
                tile2D + new Vector2f(size - 1, 0),
                tile2D + new Vector2f(size - 1, size - 1),
            };

            var tileIsoMin = new Vector2f(float.PositiveInfinity, float.PositiveInfinity);
            
            foreach(var corner in tile2DCorners) {
                var tileIso = view.WensToPx(corner);
                tileIsoMin.X = Math.Min(tileIso.X, tileIsoMin.X);
                tileIsoMin.Y = Math.Min(tileIso.Y, tileIsoMin.Y);
            }

            var rect = new IntRect(0, 0, IsometricView.TileWidth * size, IsometricView.TileHeight * size);
            var tileIsoCalc = view.WensToPx(tile2D, rect);

            Assert.AreEqual(tileIsoMin, tileIsoCalc);
        }
    }
}