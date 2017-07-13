using SFML.Graphics;
using SFML.Window;

namespace CitySimulator {
    class BuildingType {
        internal int Jobs;
        internal int Population;
        internal Vector2i Size = new Vector2i(2,2);

        internal IntRect TextureRect;

        internal float Height => (TextureRect.Height - TextureRect.Width / 2);
    }
}
