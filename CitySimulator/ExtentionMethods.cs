using System;
using System.Drawing;
using System.Globalization;
using System.Xml.Linq;
using OpenTK;

namespace CitySimulator {
    static class ExtentionMethods {
        internal static Point Floor(this Vector2 v) {
            return new Point((int)Math.Floor(v.X), (int)Math.Floor(v.Y));
        }

        internal static Point Round(this Vector2 v) {
            return new Point((int)Math.Round(v.X), (int)Math.Round(v.Y));
        }

        internal static Point Substract(this Point left, Point right)
        {
            return new Point(left.X - right.X, left.Y - right.Y);
        }

        internal static string GetString(this XElement elem, string name, bool useDefault = false) {
            var xAttribute = elem.Attribute(name);

            if (xAttribute == null) {
                if (useDefault) {
                    return "";
                }
                throw new Exception($"missing XML attribute {name}");
            }

            return xAttribute.Value;
        }

        internal static int GetInt(this XElement elem, string name, bool useDefault = false) {
            var valueStr = elem.GetString(name, useDefault);

            if (valueStr == "") {
                if (useDefault) {
                    return 0;
                }
                throw new Exception($"missing XML attribute {name}");
            }

            return int.Parse(valueStr);
        }
        
        internal static float GetFloat(this XElement elem, string name, bool useDefault = false)
        {
            var valueStr = elem.GetString(name, useDefault);

            if (valueStr == "")
            {
                if (useDefault)
                {
                    return 0;
                }
                throw new Exception($"missing XML attribute {name}");
            }

            return float.Parse(valueStr, NumberStyles.Any, CultureInfo.InvariantCulture);
        }
    }
}
