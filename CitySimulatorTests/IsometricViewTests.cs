using System;
using CitySimulator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFML.Graphics;
using SFML.Window;

namespace CitySimulatorTests {
    [TestClass()]
    public class IsometricViewTests {

        /// <summary>
        /// Tests whether world to local coordinates convert back accurately
        /// </summary>
        [TestMethod()]
        public void ScreenToWorldTest() {
            var view = new IsometricView {
                Zoom = 3.5f,
                TopLeftPos = new Vector2f(1879,289)
            };

            var vecScreen1 = new Vector2f(678,421);
            var vecWorld = view.ScreenPxToWorldPx(vecScreen1);
            var vecScreen2 = view.WorldPxToScreenPx(vecWorld);

            Assert.AreEqual(vecScreen1, vecScreen2);
        }

        /// <summary>
        /// Tests whether world to local coordinates convert back accurately
        /// </summary>
        [TestMethod()]
        public void ScreenToTileTest() {
            var view = new IsometricView {
                TopLeftPos = new Vector2f(1879, 289)
            };

            var vecWens = new Vector2i(1, 3);
            var vecScreenPx = view.WensToScreenPx(vecWens);
            var vecWensCopy = view.ScreenPxToWens(vecScreenPx + new Vector2f(IsometricView.HalfWidth, IsometricView.HalfHeight));

            Assert.AreEqual(vecWens, vecWensCopy);
        }

        /// <summary>
        /// Tests whether world to tile coordinates convert back accurately
        /// </summary>
        [TestMethod()]
        public void WorldToTileTest() {
            var view = new IsometricView();

            var vecWens = new Vector2i(1, 3);
            var vecWorldPx = view.WensToWorldPx(vecWens);
            var vecWensCopy = view.WorldPxToWens(vecWorldPx + new Vector2f(IsometricView.HalfWidth, IsometricView.HalfHeight));

            Assert.AreEqual(vecWens, vecWensCopy);
        }

        /// <summary>
        /// Test small building sizes
        /// </summary>
        [TestMethod()]
        public void SmallSizeTileTest() {
            var view = new IsometricView();

            var tile2D = new Vector2i(1, 3);
            var tileIsoBasic = view.WensToWorldPx(tile2D);
            var rect = new IntRect(234, 12, IsometricView.TileWidth, IsometricView.TileHeight);
            var tileIsoAdvanced = view.WensToWorldPx(tile2D, rect);

            Assert.AreEqual(tileIsoBasic, tileIsoAdvanced);
        }

        /// <summary>
        /// Test larger building sizes
        /// </summary>
        [TestMethod()]
        public void LargeSizeTileTest() {
            var view = new IsometricView();
            var tile2D = new Vector2i(1, 3);
            
            const int size = 3;

            var tile2DCorners = new[] {
                tile2D,
                tile2D + new Vector2i(0, size - 1),
                tile2D + new Vector2i(size - 1, 0),
                tile2D + new Vector2i(size - 1, size - 1),
            };

            var tileIsoMin = new Vector2f(float.PositiveInfinity, float.PositiveInfinity);
            
            foreach(var corner in tile2DCorners) {
                var tileIso = view.WensToWorldPx(corner);
                tileIsoMin.X = Math.Min(tileIso.X, tileIsoMin.X);
                tileIsoMin.Y = Math.Min(tileIso.Y, tileIsoMin.Y);
            }

            var rect = new IntRect(0, 0, IsometricView.TileWidth * size, IsometricView.TileHeight * size);
            var tileIsoCalc = view.WensToWorldPx(tile2D, rect);

            Assert.AreEqual(tileIsoMin, tileIsoCalc);
        }
    }
}