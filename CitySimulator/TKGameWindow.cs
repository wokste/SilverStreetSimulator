using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using CitySimulator.Tools;
using System.Collections.Generic;
using System.Drawing;

namespace CitySimulator {
    class TkGameWindow : GameWindow {
        private readonly TkCityRenderer _renderer;
        private Vector3 _lastMousePos3D;

        private Tool _tool;
        private readonly List<Tool> _toolbox;
        private readonly Game _game;
        private readonly Camera _camera = new Camera();

        public TkGameWindow() : base(800, 600, GraphicsMode.Default, "Silver Street Simulator") {
            VSync = VSyncMode.On;
            
            var rnd = new Random();
            _game = new Game(rnd.Next());
            _renderer = new TkCityRenderer(_game.City);
            _toolbox = ToolboxFactory.GetTools(_game.City, _game.ZoneManager);
            
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


            _camera.ScreenSize = new Size(ClientRectangle.Width, ClientRectangle.Height);
            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
        }

        protected override void OnKeyPress(KeyPressEventArgs e) {
            base.OnKeyPress(e);

            if (e.KeyChar >= '1' && e.KeyChar <= '9') {
                var id = e.KeyChar - '1';
                try {
                    _tool = _toolbox[id];
                } catch (IndexOutOfRangeException) {
                    _tool = null;
                }
            }
        }

        protected override void OnMouseDown(OpenTK.Input.MouseButtonEventArgs e) {
            base.OnMouseDown(e);

            var mousePos3D = _camera.ScreenSpaceToWorldSpace(e.Position);

            if (e.Button == MouseButton.Left) {
                _tool?.MouseDown(_game, mousePos3D);
            }
        }

        protected override void OnMouseUp(OpenTK.Input.MouseButtonEventArgs e) {
            base.OnMouseUp(e);

            var mousePos3D = _camera.ScreenSpaceToWorldSpace(e.Position);

            if (e.Button == MouseButton.Left) {
                _tool?.MouseUp(_game, mousePos3D);
            }
        }

        protected override void OnMouseMove(OpenTK.Input.MouseMoveEventArgs e) {
            base.OnMouseMove(e);

            var mousePos3D = _camera.ScreenSpaceToWorldSpace(e.Position);

            var mouseDrag = _lastMousePos3D - mousePos3D;
            _lastMousePos3D = mousePos3D;
            
            var mouse = Mouse.GetState();
            if (mouse[MouseButton.Right]) {
                _camera.MoveFocus(mouseDrag);
            }
            if (mouse[MouseButton.Left]) {
                _tool?.MouseMoved(_game, mousePos3D);
            }
        }

        protected override void OnMouseWheel(OpenTK.Input.MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            var zoom = e.Delta > 0 ? 0.5f : 2f;
            _camera.Zoom *= zoom;
        }
        
        protected override void OnUpdateFrame(FrameEventArgs e) {
            base.OnUpdateFrame(e);

            _game.Update(1000 / 30);
            Title = $"Silver Street Simulator - {_game.Money:C} +- {_game.Income:C}";
        }

        protected override void OnRenderFrame(FrameEventArgs e) {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _camera.SetMatrices();

            _renderer.Draw();

            SwapBuffers();
        }
    }
}
