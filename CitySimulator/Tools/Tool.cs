using System.Drawing;

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
        public void MouseDown(Game game, Camera camera, Point screenPos)
        {
            if (_mousePressed)
                return;

            _mousePressed = true;

            OnMouseDown(game, camera, screenPos);
            OnMouseDrag(game, camera, screenPos);
        }
        protected abstract void OnMouseDown(Game game, Camera camera, Point screenPos);

        /// <summary>
        /// Called when the mouse is released while the tool is active
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="view">The isometric view for converting screen positions in other positions.</param>
        /// <param name="screenPos">The screen position in pixels.</param>
        public void MouseUp(Game game, Camera camera, Point screenPos)
        {
            if (!_mousePressed)
                return;

            _mousePressed = false;

            OnMouseDrag(game, camera, screenPos);
            OnMouseUp(game, camera, screenPos);
        }
        protected abstract void OnMouseUp(Game game, Camera camera, Point screenPos);

        /// <summary>
        /// Called when the mouse is dragged while the tool is active
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="view">The isometric view for converting screen positions in other positions.</param>
        /// <param name="screenPos">The screen position in pixels.</param>
        public void MouseMoved(Game game, Camera camera, Point screenPos)
        {
            if (!_mousePressed)
                return;

            OnMouseDrag(game, camera, screenPos);
        }
        protected abstract void OnMouseDrag(Game game, Camera camera, Point screenPos);
    }
}
