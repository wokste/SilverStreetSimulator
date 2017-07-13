using System;
using System.Xml.Linq;
using SFML.Window;

namespace CitySimulator {
    static class ExtentionMethods {
        internal static Vector2f ToVector2F(this Vector2i i) {
            return new Vector2f(i.X, i.Y);
        }

        internal static Vector2f ToVector2F(this Vector2u u) {
            return new Vector2f(u.X, u.Y);
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
    }
}
