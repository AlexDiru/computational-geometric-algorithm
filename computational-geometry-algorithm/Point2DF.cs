using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace computational_geometry_algorithm
{
    /// <summary>
    /// Represents a 2D coordinate with floats
    /// </summary>
    public class Point2DF
    {
        public float X, Y;
        public Point2DF() { }

        public Point2DF(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Point2D To2DI()
        {
            return new Point2D((int)X, (int)Y);
        }
    }
}