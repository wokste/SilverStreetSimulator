using OpenTK;

namespace CitySimulator.Tools{
    abstract class Tool
    {
        private bool _mousePressed;

        /// <summary>
        /// Called when the mouse is clicked while the tool is active
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="view">The isometric view for converting screen positions in other positions.</param>
        /// <param name="screenPos">The screen position in pixels.</param>
        public void MouseDown(Game game, Vector3 screenPos)
        {
            if (_mousePressed)
                return;

            _mousePressed = true;

            OnMouseDown(game, screenPos);
        }
        protected abstract void OnMouseDown(Game game, Vector3 screenPos);

        /// <summary>
        /// Called when the mouse is released while the tool is active
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="view">The isometric view for converting screen positions in other positions.</param>
        /// <param name="screenPos">The screen position in pixels.</param>
        public void MouseUp(Game game, Vector3 screenPos)
        {
            if (!_mousePressed)
                return;

            _mousePressed = false;

            OnMouseUp(game, screenPos);
        }
        protected abstract void OnMouseUp(Game game, Vector3 screenPos);

        /// <summary>
        /// Called when the mouse is dragged while the tool is active
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="view">The isometric view for converting screen positions in other positions.</param>
        /// <param name="screenPos">The screen position in pixels.</param>
        public void MouseMoved(Game game, Vector3 screenPos)
        {
            if (!_mousePressed)
                return;

            OnMouseDrag(game, screenPos);
        }
        protected abstract void OnMouseDrag(Game game, Vector3 screenPos);
    }
}
