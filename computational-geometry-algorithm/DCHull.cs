using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using computational_geometry_algorithm.dc_hull;

namespace computational_geometry_algorithm
{
    public static class DCHull
    {
        private static Int32 MinimumSize = 4;
        public static Int32 XOffset = 0;
        public static Int32 YOffset = 0;

        /// <summary>
        /// This method uses a different method to the Graham-scan convex hull
        /// Instead of storing the vertices of the polygon in lists
        /// The vertices are stored as doubly linked lists
        /// </summary>
        public static Polygon Solve(Polygon points, bool stepThrough = false)
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
            var polyA = Solve(Polygon.Get(a), stepThrough);
            var polyB = Solve(Polygon.Get(b), stepThrough);

            return Merge(polyA, polyB, stepThrough);
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

        private static Polygon Merge(Polygon polygonA, Polygon polygonB, Boolean stepThrough)
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
            if (stepThrough)
            {
                var listPolygonA = polygonA.Convert();
                var listPolygonB = polygonB.Convert();
                if (listPolygonA.Count != 2)
                {
                    GraphicalUserInterface.DrawPolygon(listPolygonA, true, XOffset, YOffset, new System.Drawing.SolidBrush(System.Drawing.Color.Coral));
                    GraphicalUserInterface.DrawPolygon(polygonB.Convert(), true, XOffset, YOffset);
                }
                GraphicalUserInterface.DrawPolygon(listPolygonA, false, XOffset, YOffset, new System.Drawing.SolidBrush(System.Drawing.Color.Coral));
                GraphicalUserInterface.DrawPolygon(polygonB.Convert(), false, XOffset, YOffset);
                GraphicalUserInterface.GraphicsObject.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Blue) { Width = 4 }, lowerTangent.Start.Convert(XOffset, YOffset, GraphicalUserInterface.SizeMultiplier), lowerTangent.Target.Convert(XOffset, YOffset, GraphicalUserInterface.SizeMultiplier));
                GraphicalUserInterface.GraphicsObject.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black) { Width = 4 }, upperTangent.Start.Convert(XOffset, YOffset, GraphicalUserInterface.SizeMultiplier), upperTangent.Target.Convert(XOffset, YOffset, GraphicalUserInterface.SizeMultiplier));

                Console.ReadKey();
            }

            //Wire together the polygons by tangent
            return Wire(polygonA, polygonB, lowerTangent, upperTangent);
        }

        //Doesnt seem to be cw
        public static Polygon Wire(Polygon polygonA, Polygon polygonB, Vector2D lowerTangent, Vector2D upperTangent)
        {
            var wiredPolygon = new List<Point2D>();

            //if ut.s == lt.s
            Boolean endEarly = PolygonManipulation.Equals(upperTangent.Start, lowerTangent.Start);

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

                    if (endEarly)
                        break;
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

        /// <summary>
        /// Determines is pq is the lower tangent of the polygon
        /// Pseudocode from https://facwiki.cs.byu.edu/cs312ringger/index.php/Project_2
        /// (Geometry for Common Tangents)
        /// </summary>
        public static Boolean IsLowerTangent(List<Point2D> polygon, Point2D p, Point2D q)
        {
            double m;
            Int32 sum = p.X - q.X;
            if (sum == 0)
                //Not double.Max because we don't want to overflow
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