using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace computational_geometry_algorithm.dc_hull
{
    public class Vertex
    {
        public Point2D Point;
        public Vertex Next = null;
        public Vertex Prev = null;
    }
}
