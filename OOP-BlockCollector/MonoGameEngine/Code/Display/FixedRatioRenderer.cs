using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace MonoGameEngine
{
    internal sealed class FixedRatioRenderer : IDisposable
    {
        private readonly Core _core;
        private bool _isDisposed = false;

        internal int Width { get { return RenderTexture.Width; } }

        internal int Height { get { return RenderTexture.Height; } }

        internal RenderTarget2D RenderTexture { get; private set; }

        internal Rectangle DestinationRect { get; private set; }

        internal FixedRatioRenderer(Core core)
        {
            _core = core;
            ResetRenderTarget();
            CalculateDestinationRectangle();
        }

        internal void BeginDraw()
        {
            _core.GraphicsDevice.SetRenderTarget(RenderTexture);
            CalculateDestinationRectangle();
        }

        internal void EndDraw()
        {
            _core.GraphicsDevice.SetRenderTarget(null);
        }

        internal Vector2 ScaleMouseToScreenCoordinates(Vector2 screenPosition)
        {
            // Based on the work from Slobodan Pavkov's blog on Resolution Independant Rendering: 
            // http://blog.roboblob.com/2013/07/27/solving-resolution-independent-rendering-and-2d-camera-using-monogame/comment-page-1/
            var realX = screenPosition.X - DestinationRect.X;
            var realY = screenPosition.Y - DestinationRect.Y;
            
            var ratioX = DestinationRect.Width / Settings.GameResolution.X;
            var ratioY = DestinationRect.Height / Settings.GameResolution.Y;

            var virtualMousePosition = Vector2.Zero;
            virtualMousePosition.X = realX / ratioX;
            virtualMousePosition.Y = realY / ratioY;

            return virtualMousePosition;
        }

        internal void ResetRenderTarget()
        {
            var renderWidth = (int)Math.Clamp(Settings.GameResolution.X, 64, 4096);
            var renderHeight = (int)Math.Clamp(Settings.GameResolution.Y, 64, 4096);
            RenderTexture = new RenderTarget2D(_core.GraphicsDevice, renderWidth, renderHeight);
        }

        private void CalculateDestinationRectangle()
        {
            Rectangle backBufferBounds = _core.GraphicsDevice.PresentationParameters.Bounds;
            float backBufferAspectRatio = (float)backBufferBounds.Width / (float)backBufferBounds.Height;
            float screenAspectRatio = (float)Width / (float)Height;

            Rectangle destination = new Rectangle
            {
                X = 0,
                Y = 0,
                Width = backBufferBounds.Width,
                Height = backBufferBounds.Height
            };

            if (backBufferAspectRatio > screenAspectRatio)
            {
                destination.Width = (int)(destination.Height * screenAspectRatio);
                destination.X = (backBufferBounds.Width - destination.Width) / 2;
            }
            else if(backBufferAspectRatio < screenAspectRatio)
            {
                destination.Height = (int)(destination.Width / screenAspectRatio);
                destination.Y = (backBufferBounds.Height - destination.Height) / 2;
            }

            
            DestinationRect = destination;
            destination.X = 0; //DEBUG
            _core.GraphicsDevice.Viewport = new Viewport(destination);
        }

        internal Matrix GetScaleMatrix()
        {
            var scaleX = (float)Settings.ScreenDimensions.X / Width;
            var scaleY = (float)Settings.ScreenDimensions.Y / Height;

            return Matrix.CreateScale(scaleX, scaleY, 1.0f);
        }

        internal Viewport GetRenderViewport() 
        {
            return _core.GraphicsDevice.Viewport;
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            RenderTexture?.Dispose();
            _isDisposed = true;
        }
    }
}
