using SFML.Window;

namespace CitySimulator.Tools{
    abstract class Tool
    {
        private bool mousePressed;

        /// <summary>
        /// Called when the mouse is clicked while the tool is active
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="view">The isometric view for converting screen positions in other positions.</param>
        /// <param name="screenPos">The screen position in pixels.</param>
        public void MouseDown(Game game, IsometricView view, Vector2f screenPos)
        {
            if (mousePressed)
                return;

            mousePressed = true;

            OnMouseDown(game, view, screenPos);
        }
        protected abstract void OnMouseDown(Game game, IsometricView view, Vector2f screenPos);

        /// <summary>
        /// Called when the mouse is released while the tool is active
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="view">The isometric view for converting screen positions in other positions.</param>
        /// <param name="screenPos">The screen position in pixels.</param>
        public void MouseUp(Game game, IsometricView view, Vector2f screenPos)
        {
            if (!mousePressed)
                return;

            mousePressed = false;

            OnMouseUp(game, view, screenPos);
        }
        protected abstract void OnMouseUp(Game game, IsometricView view, Vector2f screenPos);

        /// <summary>
        /// Called when the mouse is dragged while the tool is active
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="view">The isometric view for converting screen positions in other positions.</param>
        /// <param name="screenPos">The screen position in pixels.</param>
        public void MouseDrag(Game game, IsometricView view, Vector2f screenPos)
        {
            if (!mousePressed)
                return;

            OnMouseDrag(game, view, screenPos);
        }
        protected abstract void OnMouseDrag(Game game, IsometricView view, Vector2f screenPos);
    }
}
