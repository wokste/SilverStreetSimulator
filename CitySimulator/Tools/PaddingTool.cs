using System.Drawing;
using OpenTK;
using OpenTK.Input;

namespace CitySimulator.Tools {
    class PaddingTool : Tool
    {
        private Point _lastScreenPos;
        private Vector3 _lastScreenPos3D;

        protected override void OnMouseDown(Game game, Camera camera, Point screenPos)
        {
            _lastScreenPos = screenPos;
            _lastScreenPos3D = camera.ScreenSpaceToWorldSpace(screenPos, null, false);
        }

        protected override void OnMouseUp(Game game, Camera camera, Point screenPos)
        {
        }

        protected override void OnMouseDrag(Game game, Camera camera, Point screenPos)
        {
            var mousePos3D = camera.ScreenSpaceToWorldSpace(screenPos, null, false);

            var keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Key.ShiftLeft) || keyboard.IsKeyDown(Key.ShiftRight)) {
                var mouseDrag2D = new Vector2(_lastScreenPos.X - screenPos.X, _lastScreenPos.Y - screenPos.Y);
                mouseDrag2D /= 50f;
                camera.MoveRotate(mouseDrag2D);
            } else {
                var mouseDrag3D = _lastScreenPos3D - mousePos3D;
                camera.MoveFocus(mouseDrag3D);
            }

            
            
            _lastScreenPos = screenPos;
            _lastScreenPos3D = mousePos3D;
        }
    }
}
