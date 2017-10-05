using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using CitySimulator.Tools;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace CitySimulator {
    class TkGameWindow : GameWindow {
        private readonly TkCityRenderer _renderer;

        private Tool _toolLeft;
        private readonly Tool _toolRight = new PaddingTool();
        private readonly List<Tool> _toolbox;
        private readonly Game _game;
        private readonly Camera _camera = new Camera();

        private Point MOUSE_POS_TEMP;

        public TkGameWindow() : base(800, 600, GraphicsMode.Default, "Silver Street Simulator") {
            VSync = VSyncMode.On;
            Context.ErrorChecking = true;
            
            var rnd = new Random();
            _game = new Game(rnd.Next());
            _renderer = new TkCityRenderer(_game.City);
            _toolbox = ToolboxFactory.GetTools(_game.City, _game.ZoneManager);
            
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            
            GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.CullFace);
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
                    _toolLeft = _toolbox[id];
                } catch (IndexOutOfRangeException) {
                    _toolLeft = null;
                }
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e) {
            base.OnMouseDown(e);

            if (e.Button == MouseButton.Left) {
                _toolLeft?.MouseDown(_game, _camera, e.Position);
            }
            if (e.Button == MouseButton.Right) {
                _toolRight?.MouseDown(_game, _camera, e.Position);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e) {
            base.OnMouseUp(e);

            if (e.Button == MouseButton.Left) {
                _toolLeft?.MouseUp(_game, _camera, e.Position);
            }
            if (e.Button == MouseButton.Right) {
                _toolRight?.MouseUp(_game, _camera, e.Position);
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs e) {
            base.OnMouseMove(e);

            MOUSE_POS_TEMP = e.Position;

            var mouse = Mouse.GetState();
            if (mouse[MouseButton.Left]) {
                _toolLeft?.MouseMoved(_game, _camera, e.Position);
            }
            if (mouse[MouseButton.Right]) {
                _toolRight?.MouseMoved(_game, _camera, e.Position);
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            
            var zoom = 1.03f;
            if (e.Delta > 0)
                _camera.Zoom *= zoom;
            else
                _camera.Zoom /= zoom;
        }
        
        protected override void OnUpdateFrame(FrameEventArgs e) {
            base.OnUpdateFrame(e);

            _game.Update(1000 / 30);
            //Title = $"Silver Street Simulator - {_game.Money:C} +- {_game.Income:C}";
        }

        protected override void OnRenderFrame(FrameEventArgs e) {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _camera.SetMatrices();

            _renderer.Draw();
            RenderMouse();

            SwapBuffers();
        }
        
        private void RenderMouse()
        {
            var mouse = Mouse.GetState();
            var mousePos = new Point(mouse.X, mouse.Y);
            Title = $"{mousePos} {MOUSE_POS_TEMP}";
            var pos3D = _camera.ViewportSpaceToWorldSpace(MOUSE_POS_TEMP, null, true);

            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Texture2D);
            
            GL.Begin(PrimitiveType.Triangles);

            for (var i = 0; i < 3; i++)
            {
                var vec = new Vector3((float)Math.Sin(i * 0.6667 * Math.PI) * 0.5f, 0.1f, (float)Math.Cos(i * 0.6667 * Math.PI) * 0.5f);
                GL.Vertex3(vec + pos3D);
            }

            GL.End();

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest);
        }
        
    }
}
