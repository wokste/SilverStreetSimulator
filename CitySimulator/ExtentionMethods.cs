using SFML.Window;

namespace CitySimulator {
    static class ExtentionMethods {
        internal static Vector2f ToVector2F(this Vector2i i) {
            return new Vector2f(i.X, i.Y);
        }

        internal static Vector2f ToVector2F(this Vector2u u) {
            return new Vector2f(u.X, u.Y);
        }
    }
}
