using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace computational_geometry_algorithm.dc_hull
{
    public class Polygon
    {
        public Vertex Root;
        public Vertex Tail;

        public Int32 Count()
        {
            var count = 0;

            var currentVertex = Root;

            while (currentVertex != null)
            {
                count++;
                currentVertex = currentVertex.Next;
            }

            return count;
        }

        public Vertex GetVertex(Point2D coord)
        {
            var currentVertex = Root;

            while (currentVertex != null)
            {
                if (currentVertex.Point != null)
                    if (PolygonManipulation.Equals(currentVertex.Point, coord))
                        return currentVertex;
                currentVertex = currentVertex.Next;
            }

            return null;
        }

        public List<Point2D> Convert()
        {
            var list = new List<Point2D>();

            var currentVertex = Root;

            while (currentVertex != null)
            {
                if (currentVertex.Point != null)
                    list.Add(currentVertex.Point);
                currentVertex = currentVertex.Next;
            }

            return list;
        }

        public void Import(List<Point2D> data)
        {
            Root = new Vertex();
            var currentVertex = Root;

            foreach (var datum in data)
            {
                currentVertex.Point = new Point2D(datum.X, datum.Y);
                currentVertex.Next = new Vertex();
                var oldVertex = currentVertex;
                currentVertex = currentVertex.Next;
                currentVertex.Prev = oldVertex;
            }

            Tail = currentVertex;
        }

        public static Polygon Get(List<Point2D> data)
        {
            Polygon p = new Polygon();
            p.Import(data);
            return p;
        }
    }
}
