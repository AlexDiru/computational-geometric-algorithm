using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace computational_geometry_algorithm.dc_hull
{
    /// <summary>
    /// This is the doubly linked list polygon used in the DC Hull algorithm
    /// </summary>
    public class Polygon
    {
        //Root of the linked list
        public Vertex Root;

        //Tail of the linked list
        public Vertex Tail;

        /// <summary>
        /// Counts the number of vertices the polygon has
        /// </summary>
        public Int32 Count()
        {
            var count = 0;
            var currentVertex = Root;

            //Traverse until end
            while (currentVertex != null)
            {
                count++;
                currentVertex = currentVertex.Next;
            }

            return count;
        }

        /// <summary>
        /// Given a point, iterates the vertices, until a vertex with the coordinates is found
        /// On failure, returns null
        /// </summary>
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

        /// <summary>
        /// Converts the polygon into a list of points
        /// </summary>
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

        /// <summary>
        /// Uses the data in the input list to create the vertices of this polygon
        /// </summary>
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

        /// <summary>
        /// Static function to convert a list of points into a polygon without having a local object instance
        /// </summary>
        public static Polygon Get(List<Point2D> data)
        {
            Polygon p = new Polygon();
            p.Import(data);
            return p;
        }
    }
}
