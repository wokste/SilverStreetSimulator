﻿using System;
using System.Drawing;

namespace CitySimulator.Tools {
    abstract class AreaSelector {

        public Point Start { get; set; }
        public Point End { get; set; }

        public abstract void Iterate(Action<int, int> action, Func<int, int, TileFilterResult> filter);
    }

    class LineAreaSelector : AreaSelector {
        public override void Iterate(Action<int, int> action, Func<int, int, TileFilterResult> filter) {
            var pos = Start;

            var change = true;

            while (change) {
                if (filter(pos.X, pos.Y) == TileFilterResult.CanBuild) {
                    action(pos.X, pos.Y);
                }

                change = StepTo(ref pos, End);
            }
        }

        private bool StepTo(ref Point point, Point to) {
            var diff = point.Substract(to);

            if (Math.Abs(diff.X) > Math.Abs(diff.Y)) {
                return StepToX(ref point, to);
            } else {
                return StepToY(ref point, to);
            }
        }

        private bool StepToX(ref Point point, Point to, bool recurse = true) {
            switch (point.X - to.X) {
                case var n when n > 0:
                    point.X--;
                    return true;
                case var n when n < 0:
                    point.X++;
                    return true;
                default:
                    if (!recurse)
                        return false;

                    return StepToY(ref point, to, false);
            }
        }

        private bool StepToY(ref Point point, Point to, bool recurse = true) {
            switch (point.Y - to.Y) {
                case var n when n > 0:
                    point.Y--;
                    return true;
                case var n when n < 0:
                    point.Y++;
                    return true;
                default:
                    if (!recurse)
                        return false;

                    return StepToX(ref point, to, false);
            }
        }
    }

    class RectangeAreaSelector : AreaSelector {
        public override void Iterate(Action<int, int> action, Func<int, int, TileFilterResult> filter) {
            var x0 = Math.Min(Start.X, End.X);
            var x1 = Math.Max(Start.X, End.X);

            var y0 = Math.Min(Start.Y, End.Y);
            var y1 = Math.Max(Start.Y, End.Y);
            
            for (var x = x0; x <= x1; x++) {
                for (var y = y0; y <= y1; y++) {
                    if (filter(x, y) == TileFilterResult.CanBuild) {
                        action(x, y);
                    }
                }
            }
        }
    }
}
