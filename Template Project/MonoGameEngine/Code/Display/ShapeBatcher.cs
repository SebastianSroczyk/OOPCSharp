// Based on the work by Two-Bit Coding on YouTube: https://www.youtube.com/watch?v=X03Ht69HRec
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameEngine
{
    internal sealed class ShapeBatcher : IDisposable
    {
        const int SHAPE_VERTEX_COUNT = 4;
        const int SHAPE_INDEX_COUNT = 6;

        const int MAX_VERTEX_COUNT = 2048;
        const int MAX_INDEX_COUNT = MAX_VERTEX_COUNT * 3;

        private bool _isDisposed;
        private readonly Core _core;
        private readonly BasicEffect _effect;

        private VertexPositionColor[] _vertices;
        private int[] _indices;

        private int _shapeCount = 0;
        private int _vertexCount = 0;
        private int _indexCount = 0;

        private readonly Matrix INVERT_SCALE;

        private bool _isStarted = false;

        internal ShapeBatcher(Core core)
        {
            _isDisposed = false;
            _core = core ?? throw new ArgumentNullException("ShapeBatcher");

            INVERT_SCALE = Matrix.CreateScale(1.0f, -1.0f, 1.0f);

            _effect = new BasicEffect(core.GraphicsDevice)
            {
                TextureEnabled = false,
                FogEnabled = false,
                LightingEnabled = false,
                VertexColorEnabled = true,
                World = Matrix.Identity,
                View = Matrix.Identity,
                Projection = Matrix.Identity,
                Alpha = 0.5f
            };

            _vertices = new VertexPositionColor[MAX_VERTEX_COUNT];
            _indices = new int[MAX_INDEX_COUNT];
        }

        internal void Begin()
        {
            if(_isStarted)
            {
                throw new Exception("Batching has already started.");
            }

            Viewport vp = _core.GraphicsDevice.Viewport;
            _effect.Projection = Matrix.CreateOrthographicOffCenter(0f, vp.Width, 0f, vp.Height, 0f, 1f);
            _effect.Projection *= INVERT_SCALE;
            _isStarted = true;
        }

        internal void End()
        {
            Flush();
            _isStarted = false;
        }

        private void Flush()
        {
            if (_shapeCount == 0)
                return;

            EnsureStarted();

            foreach(EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _core.GraphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.TriangleList, 
                    _vertices, 
                    0, 
                    _vertexCount, 
                    _indices, 
                    0, 
                    _indexCount / 3);
            }

            _shapeCount = 0;
            _indexCount = 0;
            _vertexCount = 0;

            _vertices = new VertexPositionColor[MAX_VERTEX_COUNT];
            _indices = new int[MAX_INDEX_COUNT];
        }

        private void EnsureStarted()
        {
            if (!_isStarted)
            {
                throw new Exception("Batching was never started.");
            }
        }

        private void EnsureSpace(int shapeVertexCount, int shapeIndexCount)
        {
            if (shapeVertexCount > _vertices.Length)
                throw new Exception("Maximium shape vertex count is: " + _vertices.Length);

            if (shapeIndexCount > _indices.Length)
                throw new Exception("Maximium shape index count is: " + _indices.Length);

            if (_vertexCount + shapeVertexCount > _vertices.Length ||
                _indexCount + shapeIndexCount > _indices.Length)
                Flush();
        }

        internal void DrawRectangle(float x, float y, float width, float height, Color? colour = null)
        {
            EnsureStarted();
            EnsureSpace(SHAPE_VERTEX_COUNT, SHAPE_INDEX_COUNT);

            float left = x;
            float right = x + width;
            float bottom = y;
            float top = y + height;

            var viewMatrix = Camera.Instance.GetViewMatrix(Vector2.One);

            Vector2 a = Vector2.Transform(new Vector2(left, top), viewMatrix);
            Vector2 b = Vector2.Transform(new Vector2(right, top), viewMatrix);
            Vector2 c = Vector2.Transform(new Vector2(right, bottom), viewMatrix);
            Vector2 d = Vector2.Transform(new Vector2(left, bottom), viewMatrix);

            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 1 + _vertexCount;
            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 3 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 0 + _vertexCount;

            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(a, 0f), colour ?? Color.White);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(b, 0f), colour ?? Color.White);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(c, 0f), colour ?? Color.White);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(d, 0f), colour ?? Color.White);

            _shapeCount++;
        }

        internal void DrawLine(float ax, float ay, float bx, float by, float thickness, Color? colour)
        {
            EnsureStarted();
            EnsureSpace(SHAPE_VERTEX_COUNT, SHAPE_INDEX_COUNT);

            // Ensure the thickness is a usable value
            thickness = Math.Clamp(thickness, 1f, 10f);
            float halfThickness = thickness * 0.5f;

            // Define the first edge's points
            var edge1x = bx - ax;
            var edge1y = by - ay;
            Normalize(ref edge1x, ref edge1y);
            edge1x *= halfThickness;
            edge1y *= halfThickness;

            // The second edge is just the inverse of the first
            var edge2x = -edge1x;
            var edge2y = -edge1y;

            // Define the first normal's points
            var normal1x = -edge1y;
            var normal1y = edge1x;

            // The second normal is just the inverse of the first
            var normal2x = -normal1x;
            var normal2y = -normal1y;

            // Define the 4 quadrilateral points for the line segment
            var quadPoint1x = ax + normal1x + edge2x;
            var quadPoint1y = ay + normal1y + edge2y;

            var quadPoint2x = bx + normal1x + edge1x;
            var quadPoint2y = by + normal1y + edge1y;

            var quadPoint3x = bx + normal2x + edge1x;
            var quadPoint3y = by + normal2y + edge1y;

            var quadPoint4x = ax + normal2x + edge2x;
            var quadPoint4y = ay + normal2y + edge2y;

            // Add the indices for the points in the correct order to the index list
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 1 + _vertexCount;
            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 3 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 0 + _vertexCount;

            // Grab the camera's view matrix to transform the line into world space.
            var viewMatrix = Camera.Instance.GetViewMatrix(Vector2.Zero);

            // Add the vertices to the vertex list
            _vertices[_vertexCount++] = new VertexPositionColor(Vector3.Transform(new Vector3(quadPoint1x, quadPoint1y, 0f), viewMatrix), colour ?? Color.White);
            _vertices[_vertexCount++] = new VertexPositionColor(Vector3.Transform(new Vector3(quadPoint2x, quadPoint2y, 0f), viewMatrix), colour ?? Color.White);
            _vertices[_vertexCount++] = new VertexPositionColor(Vector3.Transform(new Vector3(quadPoint3x, quadPoint3y, 0f), viewMatrix), colour ?? Color.White);
            _vertices[_vertexCount++] = new VertexPositionColor(Vector3.Transform(new Vector3(quadPoint4x, quadPoint4y, 0f), viewMatrix), colour ?? Color.White);

            // Increment the number of shapes that have been added to the batcher
            _shapeCount++;
        }

        internal void DrawLine(Vector2 a, Vector2 b, float thickness = 1.0f, Color? colour = null)
        {
            DrawLine(a.X, a.Y, b.X, b.Y, thickness, colour);
        }

        internal void DrawCircle(float x, float y, float radius, int points, float thickness, Color? colour)
        {
            float deltaAngle = MathHelper.TwoPi / points;
            float currentAngle = 0;

            for(int i = 0; i < points; i++)
            {
                float currentX = MathF.Sin(currentAngle) * radius + x;
                float currentY = MathF.Cos(currentAngle) * radius + y;

                currentAngle += deltaAngle;

                float nextX = MathF.Sin(currentAngle) * radius + x;
                float nextY = MathF.Cos(currentAngle) * radius + y;

                DrawLine(currentX, currentY, nextX, nextY, thickness, colour);
            }
        }

        private void Normalize(ref float x, ref float y)
        {
            float inverseLength = 1f / (float)Math.Sqrt((x * x) + (y * y));
            x *= inverseLength;
            y *= inverseLength;
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _effect?.Dispose();
            _isDisposed = true;
        }
    }
}
