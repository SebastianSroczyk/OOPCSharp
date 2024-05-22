using Microsoft.Xna.Framework;

using System;

namespace MonoGameEngine
{
    /// <summary>A class which represents a raycast from one point (in screen space) to another.</summary>
    public sealed class RayCast
    {
        private readonly Vector2 _startPos;
        private readonly Vector2 _endPos;
        private readonly Vector2 _direction;

        /// <summary>
        /// A constructor which will generate the vector direction based on the given coordinates.
        /// </summary>
        /// <param name="startPos">A Vector2 object representing the starting point of this RayCast.</param>
        /// <param name="endPos">A Vector2 object representing the end point of this RayCast.</param>
        public RayCast(Vector2 startPos, Vector2 endPos)
        {
            _startPos = startPos;
            _endPos = endPos;
            _direction = GetDirection();
        }

        /// <summary>
        /// A constructor which will generate the end point of this RayCast using the direction and length provided.
        /// </summary>
        /// <param name="startPos">A Vector2 object representing the starting point of this RayCast.</param>
        /// <param name="direction">A Vector2 object representing the direction this RayCast should travel.</param>
        /// <param name="length">A floating-point value representing the length of the desired RayCast (in pixels).</param>
        public RayCast(Vector2 startPos, Vector2 direction, float length)
        {
            _startPos = startPos;
            _direction = direction;
            _endPos = _startPos + (direction * length);
        }

        /// <summary>
        /// Checks to see if a single, given point can be found on this Raycast's line.
        /// </summary>
        /// <param name="point">The point that should be checked against this Raycast's line.</param>
        /// <returns>Returns 'true' if the point exists along this Raycast's line. Otherwise, returns 'false'.</returns>
        public bool IsPointOnVector(Vector2 point)
        {
            // Catch the edge cases where the direction in the X or Y axis was zero
            if(_direction.X == 0)
                return point.X == _startPos.X;
            if(_direction.Y == 0)
                return point.Y == _startPos.Y;

            return (_startPos.X - point.X) / _direction.X == (_startPos.Y - point.Y) / _direction.Y;
        }

        /// <summary>
        /// Method used to check whether or not this RayCast will intersect a given Rectangle.
        /// </summary>
        /// <param name="bounds">The Rectangle we want to check against.</param>
        /// <returns>Returns true if the Rectangle is intersected by this RayCast. Otherwise, returns false.</returns>
        public bool CheckBoundHit(Rectangle bounds)
        {
            // Check each side of the given Rectangle as its own line.
            bool left = LineIntersection(new Vector2(bounds.X, bounds.Y), new Vector2(bounds.X, bounds.Y + bounds.Height));
            bool right = LineIntersection(new Vector2(bounds.X + bounds.Width, bounds.Y), new Vector2(bounds.X + bounds.Width, bounds.Y + bounds.Height));
            bool top = LineIntersection(new Vector2(bounds.X, bounds.Y), new Vector2(bounds.X + bounds.Width, bounds.Y));
            bool bottom = LineIntersection(new Vector2(bounds.X, bounds.Y + bounds.Height), new Vector2(bounds.X + bounds.Width, bounds.Y + bounds.Height));

            // Check which of the sides detected an intersection
            if (left || right || top || bottom)
                return true;

            return false;
        }

        /// <summary>
        /// Method that can check if another Vector (from the start and end points provided) intersects this Raycast.
        /// </summary>
        /// <param name="lineStart">The start point of the vector to check.</param>
        /// <param name="lineEnd">The end point of the vector to check.</param>
        /// <returns>Returns 'true' if an intersection is detected. Otherwise, returns 'false'.</returns>
        private bool LineIntersection(Vector2 lineStart, Vector2 lineEnd)
        {
            // Set up the co-ordinate points
            double x1 = _startPos.X;
            double x2 = _endPos.X;
            double x3 = lineStart.X;
            double x4 = lineEnd.X;

            double y1 = _startPos.Y;
            double y2 = _endPos.Y;
            double y3 = lineStart.Y;
            double y4 = lineEnd.Y;

            // Calculate the distance to the point of intersection
            double uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            double uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

            // [UNUSED] Find the specific intersection point
            //double intersectionX = x1 + (uA * (x2 - x1));
            //double intersectionY = y1 + (uA * (y2 - y1));

            // When a collision has taken place, uA and uB should both be in the range of 0-1
            if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
                return true;

            return false;
        }

        /// <summary>
        /// A simple getter method to calculate the direction of this Raycast.
        /// </summary>
        /// <returns>A Vector2 containing the direction values of this Raycast.</returns>
        public Vector2 GetDirection()
        {
            return new Vector2(_endPos.X - _startPos.X, _endPos.Y - _startPos.Y);
        }

        /// <summary>
        /// A getter method which calculates the length (magnitude) of this Raycast.
        /// </summary>
        /// <returns>Returns a float value of the calculated length of this line.</returns>
        public float GetLength()
        {
            return (float)Math.Sqrt(Math.Pow(_endPos.X - _startPos.X, 2) + Math.Pow(_endPos.Y - _startPos.Y, 2));
        }
    }
}
