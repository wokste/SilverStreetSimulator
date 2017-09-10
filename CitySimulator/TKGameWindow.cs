using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using CitySimulator.Tools;
using System.Collections.Generic;
using SFML.Window;

namespace CitySimulator {
    class TkGameWindow : GameWindow {
        private readonly TkCityRenderer _renderer;
        private Vector2f _lastMousePos;

        private Tool _tool;
        private readonly List<Tool> toolbox;
        private readonly Game _game;

        public TkGameWindow() : base(800, 600, GraphicsMode.Default, "Silver Street Simulator") {
            VSync = VSyncMode.On;
            
            Random rnd = new Random();
            _game = new Game(rnd.Next());
            _renderer = new TkCityRenderer(_game.City);
            toolbox = ToolboxFactory.GetTools(_game.City, _game.ZoneManager);
            
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            
            GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
            //GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        protected override void OnKeyPress(KeyPressEventArgs e) {
            base.OnKeyPress(e);

            if (e.KeyChar >= '1' && e.KeyChar <= '9') {
                var id = e.KeyChar - '1';
                try {
                    _tool = toolbox[id];
                } catch (IndexOutOfRangeException) {
                    _tool = null;
                }
            }
        }

        protected override void OnMouseDown(OpenTK.Input.MouseButtonEventArgs e) {
            base.OnMouseDown(e);

            if (e.Button == MouseButton.Left) {
                _tool?.MouseDown(_game, _renderer.View, new Vector2f(e.X, e.Y));
            }
        }

        protected override void OnMouseUp(OpenTK.Input.MouseButtonEventArgs e) {
            base.OnMouseUp(e);

            if (e.Button == MouseButton.Left) {
                _tool?.MouseUp(_game, _renderer.View, new Vector2f(e.X, e.Y));
            }
        }

        protected override void OnMouseMove(OpenTK.Input.MouseMoveEventArgs e) {
            base.OnMouseMove(e);

            var mouseDrag = _lastMousePos - new Vector2f(e.X, e.Y);
            _lastMousePos = new Vector2f(e.X, e.Y);
            /*
            if (Mouse.IsButtonPressed(MouseButton.Right)) {
                _renderer.View.TopLeftPos += (mouseDrag * _renderer.View.Zoom);
            }
            if (Mouse.IsButtonPressed(MouseButton.Left)) {
                _tool.MouseDrag(_game, _renderer.View, new Vector2f(e.X, e.Y));
            }
            */
        }

        protected override void OnMouseWheel(OpenTK.Input.MouseWheelEventArgs e) {
            base.OnMouseWheel(e);

            if (e.Delta > 0) {
                Zoom(0.5f, new Vector2f(e.X, e.Y));
            } else {
                Zoom(2f, new Vector2f(e.X, e.Y));
            }
        }

        protected void Zoom(float f, Vector2f mousePos) {
            var mousePosWorld = _renderer.View.ScreenPxToWorldPx(mousePos);
            _renderer.View.Zoom *= f;
            var mousePosNew = _renderer.View.WorldPxToScreenPx(mousePosWorld);

            _renderer.View.TopLeftPos += (mousePosNew - mousePos);
        }

        protected override void OnUpdateFrame(FrameEventArgs e) {
            base.OnUpdateFrame(e);

            _game.Update(1000 / 30);
            this.Title = $"Silver Street Simulator - {_game.Money:C} +- {_game.Income:C}";
        }

        protected override void OnRenderFrame(FrameEventArgs e) {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);

            var matrix = Matrix4.CreateOrthographic(Width, Height, 1.0f, 100.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref matrix);

            _renderer.Draw();

            SwapBuffers();
        }
    }
}
