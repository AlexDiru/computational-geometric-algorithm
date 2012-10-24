using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace computational_geometry_algorithm.dc_hull
{
    public class Vector2D
    {
        public Point2D Start;
        public Point2D Target;

        public Vector2D() { }

        public Vector2D(Point2D a, Point2D b)
        {
            Start = a;
            Target = b;
        }

        /// <summary>
        /// If the vector contains a point
        /// </summary>
        public Boolean Contains(Point2D a)
        {
            return PolygonManipulation.Equals(Target,a) || PolygonManipulation.Equals(Start, a);
        }
    }
}
