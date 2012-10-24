using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;

namespace computational_geometry_algorithm
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

        public String Output()
        {
            String data = "";
            var currentVertex = Root;

            while (currentVertex != null)
            {
                if (currentVertex.Point != null)
                    data += "(" + currentVertex.Point.X + "," + currentVertex.Point.Y + ")";
                currentVertex = currentVertex.Next;
            }

            return data;
        }

        public String OutputRv()
        {
            String data = "";
            var currentVertex = Tail;

            while (currentVertex != null)
            {
                if (currentVertex.Point != null)
                    data += "(" + currentVertex.Point.X + "," + currentVertex.Point.Y + ")";
                currentVertex = currentVertex.Prev;
            }

            return data;
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

    public class Vertex
    {
        public Point2D Point;
        public Vertex Next = null;
        public Vertex Prev = null;
    }

    public static class DCHull
    {
        private static Int32 MinimumSize = 4;
        public static Int32 XOffset = 0;
        public static Int32 YOffset = 0;

        public static Polygon Solve(Polygon points)
        {
            if (points.Count() <= MinimumSize)
            {
                return Polygon.Get(ConvexHull.Solve(points.Convert()));
            }

            var sortedPoints = PolygonManipulation.SortTopogically(points.Convert());

            //Split points into two sets
            var a = new List<Point2D>();
            var b = new List<Point2D>();

            a.AddRange(sortedPoints.Take(sortedPoints.Count()/2));
            b.AddRange(sortedPoints.Skip(sortedPoints.Count()/2).Take(sortedPoints.Count() - sortedPoints.Count()/2));

            //Compute the convex hulls of a and b recursively
            var polyA = Solve(Polygon.Get(a));
            var polyB = Solve(Polygon.Get(b));

            return Merge(polyA, polyB);
        }

        private static Point2D GetRightmostPoint(IEnumerable<Point2D> polygon)
        {
            //Get all max x points
            var xMaxPoints = polygon.Where(p => p.X == polygon.Max(q => q.X));
            return xMaxPoints.Where(p => p.Y == xMaxPoints.Min(q => q.Y)).First();
        }

        private static Point2D GetLeftmostPoint(IEnumerable<Point2D> polygon)
        {
            //Get all max x points
            var yMinPoints = polygon.Where(p => p.X == polygon.Min(q => q.X));
            return yMinPoints.Where(p => p.Y == yMinPoints.Min(q => q.Y)).First();
        }

        private static Polygon Merge(Polygon polygonA, Polygon polygonB)
        {

            var newPolygonA = PolygonManipulation.SortTopogically(polygonA.Convert());
            var newPolygonB = PolygonManipulation.SortTopogically(polygonB.Convert());


            var leftmostPoint = GetLeftmostPoint(newPolygonB);
            var rightmostPoint = GetRightmostPoint(newPolygonA);

            //While T = ab is not lower tangent to both A and B
            while (!(IsLowerTangent(newPolygonA, rightmostPoint, leftmostPoint) && IsLowerTangent(newPolygonB, leftmostPoint,rightmostPoint)))
            {
                //While T is no a lower tangent to A
                while (!IsLowerTangent(newPolygonA, rightmostPoint,leftmostPoint))
                {
                    //Move counter clockwise
                    rightmostPoint = PolygonManipulation.GetNextPoint(newPolygonA, rightmostPoint);
                }

                //While T is not a lower tangent to B
                while (!IsLowerTangent(newPolygonB, leftmostPoint,rightmostPoint))
                {
                    //Move clockwise
                    leftmostPoint = PolygonManipulation.GetPreviousPoint(newPolygonB, leftmostPoint);
                }
            }

            Vector2D lowerTangent = new Vector2D(rightmostPoint, leftmostPoint);

            leftmostPoint = GetLeftmostPoint(newPolygonB);
            rightmostPoint = GetRightmostPoint(newPolygonA);
            Int32 yMaxA = newPolygonA.Max(p => p.Y);
            Int32 yMaxB = newPolygonB.Max(p => p.Y);

            //While T = ab is not lower tangent to both A and B
            while (!IsUpperTangent(newPolygonA, rightmostPoint, leftmostPoint) || !(IsUpperTangent(newPolygonB, leftmostPoint, rightmostPoint)))
            {
                //While T is no a lower tangent to A
                while (!IsUpperTangent(newPolygonA, rightmostPoint, leftmostPoint))
                {
                    //Move counter clockwise
                    rightmostPoint = PolygonManipulation.GetPreviousPoint(newPolygonA, rightmostPoint);
                }

                //While T is not a lower tangent to B
                while (!IsUpperTangent(newPolygonB, leftmostPoint, rightmostPoint))
                {
                    //Move clockwise
                    leftmostPoint = PolygonManipulation.GetNextPoint(newPolygonB, leftmostPoint);
                }
            }

            Vector2D upperTangent = new Vector2D(rightmostPoint, leftmostPoint);

            //Draw tangents
            /*GraphicalUserInterface.DrawPolygon(polygonA, !(polygonA.Count == 2), XOffset, YOffset, new System.Drawing.SolidBrush(System.Drawing.Color.Coral));
            GraphicalUserInterface.DrawPolygon(polygonB, polygonB.Count != 2, XOffset, YOffset);
            GraphicalUserInterface.GraphicsObject.DrawLine(new System.Drawing.Pen(System.Drawing.Color.White) { Width = 5 }, lowerTangent.Start.Convert(XOffset, YOffset, GraphicalUserInterface.SizeMultiplier), lowerTangent.Target.Convert(XOffset, YOffset, GraphicalUserInterface.SizeMultiplier));
            GraphicalUserInterface.GraphicsObject.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black) { Width = 5 }, upperTangent.Start.Convert(XOffset, YOffset, GraphicalUserInterface.SizeMultiplier), upperTangent.Target.Convert(XOffset, YOffset, GraphicalUserInterface.SizeMultiplier));

            Console.ReadKey();

            GraphicalUserInterface.Clear();*/

            //Wire together the polygons by tangent
            return Wire(polygonA, polygonB, lowerTangent, upperTangent);
        }

        //Doesnt seem to be cw
        public static Polygon Wire(Polygon polygonA, Polygon polygonB, Vector2D lowerTangent, Vector2D upperTangent)
        {
            var wiredPolygon = new List<Point2D>();

            wiredPolygon.Add(upperTangent.Start);
            var currentVertex = polygonB.GetVertex(upperTangent.Target);

            Boolean leftPolygon = false;

            do
            {
                wiredPolygon.Add(currentVertex.Point);


                if (PolygonManipulation.Equals(currentVertex.Point, lowerTangent.Target))
                {
                    leftPolygon = true;
                    currentVertex = polygonA.GetVertex(lowerTangent.Start);
                    wiredPolygon.Add(currentVertex.Point);
                }


                do
                {
                    if (leftPolygon)
                    {
                        if (currentVertex == null || currentVertex.Point == null)
                            currentVertex = polygonA.Tail.Prev;
                        else
                            currentVertex = currentVertex.Prev;
                    }
                    else
                    {
                        if (currentVertex == null || currentVertex.Point == null)
                            currentVertex = polygonB.Tail.Prev;
                        else
                            currentVertex = currentVertex.Prev;
                    }
                } while (currentVertex == null || currentVertex.Point == null);

                //While we aren't back at start
            } while (!PolygonManipulation.Equals(currentVertex.Point, upperTangent.Start));

            wiredPolygon = wiredPolygon.Distinct().ToList();
            wiredPolygon.Reverse();
            return Polygon.Get(wiredPolygon);
        }

        public static Boolean IsUpperTangent(List<Point2D> polygon, Point2D p, Point2D q)
        {
            double m;
            Int32 sum = p.X - q.X;
            if (sum == 0)
                m = 9999999;
            else
                m = ((double)p.Y - (double)q.Y) / (double)sum;
            double b = -1 * m * (double)p.X + (double)p.Y;

            foreach (var point in polygon)
            {
                if (!PolygonManipulation.Equals(point, p) && !PolygonManipulation.Equals(point, q))
                    if ((m * (double)point.X + b - (double)point.Y) < 0)
                        return false;
            }
            return true;
        }

        public static Boolean IsLowerTangent(List<Point2D> polygon, Point2D p, Point2D q)
        {
            double m;
            Int32 sum = p.X - q.X;
            if (sum == 0)
                m = 9999999;
            else
                m = ((double)p.Y - (double)q.Y) / (double)sum;
            double b = -1 * m * (double)p.X + (double)p.Y;

            foreach (var point in polygon)
            {
                if (!PolygonManipulation.Equals(point, p) && !PolygonManipulation.Equals(point, q))
                    if ((m * (double)point.X + b - (double)point.Y) > 0)
                        return false;
            }
            return true;
        }
    }
}
