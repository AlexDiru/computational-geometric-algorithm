using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace computational_geometry_algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Point2D> p = new List<Point2D>();
            p.Add(new Point2D(0, 3));
            p.Add(new Point2D(3, 0));
            p.Add(new Point2D(10, 3));
            p.Add(new Point2D(3, 5));
            p.Add(new Point2D(10, 2));
            p.Add(new Point2D(11, 2));
            p.Add(new Point2D(9, 6));
            p.Add(new Point2D(13, 4));
            p.Add(new Point2D(4, 5));
            p.Add(new Point2D(4, 11));
            p.Add(new Point2D(5, 4));
            p.Add(new Point2D(6, 5));
            p.Add(new Point2D(7, 6));


            p = ConvexHull.TopologicalSort(p);

            ConvexHull.Draw(p);
        }
    }
}
