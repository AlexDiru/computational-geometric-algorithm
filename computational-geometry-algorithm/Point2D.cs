using System;
using System.Drawing;

namespace computational_geometry_algorithm
{
    /// <summary>
    /// Represents a 2D coordinate
    /// </summary>
    public class Point2D
    {
        public Int32 X, Y;
        public Point2D() { }
        public Point2D(Int32 x, Int32 y)
        {
            X = x;
            Y = y;
        }

        //Converts a Point2D into System.Drawing.Point
        public Point Convert(Int32 offsetX = 0, Int32 offsetY = 0, Int32 sizeMultiplier = 1)
        {
            return new Point(X* sizeMultiplier + offsetX, Y*sizeMultiplier + offsetY);
        }
    }
}
