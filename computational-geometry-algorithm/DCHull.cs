using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;

namespace computational_geometry_algorithm
{
    public static class DCHull
    {
        private static Int32 MinimumSize = 3;

        public static List<Point2D> Solve(List<Point2D> points)
        {
            if (points.Count() <= MinimumSize)
                return ConvexHull.Solve(points.ToList());

            var sortedPoints = PolygonManipulation.SortTopogically(points);

            //Split points into two sets
            var a = new List<Point2D>();
            var b = new List<Point2D>();

            a.AddRange(sortedPoints.Take(sortedPoints.Count()/2));
            b.AddRange(sortedPoints.Skip(sortedPoints.Count()/2).Take(sortedPoints.Count() - sortedPoints.Count()/2));

            //Compute the convex hulls of a and b recursively
            a = ConvexHull.Solve(a);
            b = ConvexHull.Solve(b);

            return Merge(a, b);
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

        private static List<Point2D> Merge(List<Point2D> polygonA, List<Point2D> polygonB)
        {
            var merged = new List<Point2D>();

            polygonA = PolygonManipulation.SortTopogically(polygonA);
            polygonB = PolygonManipulation.SortTopogically(polygonB);

            if (polygonA.Count == 1 && polygonB.Count == 1)
            {
                merged.AddRange(polygonA);
                merged.AddRange(polygonB);
                return merged;
            }

            var leftmostPoint = GetLeftmostPoint(polygonB);
            var rightmostPoint = GetRightmostPoint(polygonA);
            var mergedPolygons = new List<Point2D>();
            mergedPolygons.AddRange(polygonA);
            mergedPolygons.AddRange(polygonB);

            var pointsToRemove = new List<Point2D>();

            //While T = ab is not lower tangent to both A and B
            while (!IsLowerTangent(polygonA, rightmostPoint) || !(IsLowerTangent(polygonB, leftmostPoint)))
            {
                //While T is no a lower tangent to A
                while (!IsLowerTangent(polygonA, rightmostPoint))
                {
                    pointsToRemove.Add(rightmostPoint);
                    //Move counter clockwise
                    rightmostPoint = PolygonManipulation.GetNextPoint(polygonA, rightmostPoint);
                }

                //While T is not a lower tangent to B
                while (!IsLowerTangent(polygonB, leftmostPoint))
                {
                    pointsToRemove.Add(leftmostPoint);
                    //Move clockwise
                    leftmostPoint = PolygonManipulation.GetPreviousPoint(polygonB, leftmostPoint);
                }
            }

            Vector2D lowerTangent = new Vector2D(rightmostPoint, leftmostPoint);

            leftmostPoint = GetLeftmostPoint(polygonB);
            rightmostPoint = GetRightmostPoint(polygonA);

            //While T = ab is not lower tangent to both A and B
            while (!IsUpperTangent(polygonA, rightmostPoint) || !(IsUpperTangent(polygonB, leftmostPoint)))
            {
                //While T is no a lower tangent to A
                while (!IsUpperTangent(polygonA, rightmostPoint))
                {
                    pointsToRemove.Add(rightmostPoint);
                    //Move counter clockwise
                    rightmostPoint = PolygonManipulation.GetPreviousPoint(polygonA, rightmostPoint);
                }

                //While T is not a lower tangent to B
                while (!IsUpperTangent(polygonB, leftmostPoint))
                {
                    pointsToRemove.Add(leftmostPoint);
                    //Move clockwise
                    leftmostPoint = PolygonManipulation.GetNextPoint(polygonB, leftmostPoint);
                }
            }

            Vector2D upperTangent = new Vector2D(rightmostPoint, leftmostPoint);

            //Wire together the polygons by tangent
            var wiredPolygon = new List<Point2D>();

            return mergedPolygons;
        }

        /*private static List<Point2D> Merge(List<Point2D> polygonA, List<Point2D> polygonB)
        {
            //Lower tangent
            Point2D currentPointA = GetRightmostPoint(polygonA);
            Point2D currentPointB = GetLeftmostPoint(polygonB);

            Int32 indexA = polygonA.ToList().IndexOf(currentPointA);
            Int32 indexB = polygonB.ToList().IndexOf(currentPointB);

            var pointsToRemoveA = new List<Point2D>();
            var pointsToRemoveB = new List<Point2D>();

            //Get A's lowest point
            while (currentPointA.X > polygonA.Min(p => p.X))
            {
                //Remove this point
                pointsToRemoveA.Add(polygonA[indexA]);

                //Adjust the index
                indexA--;
                if (indexA == -1)
                    indexA = polygonA.Count - 1;

                //Get new point
                currentPointA = polygonA[indexA];
            }

            //Get B's lowest point
            while (currentPointB.X > polygonB.Min(p => p.X))
            {
                //Remove this point
                pointsToRemoveB.Add(polygonB[indexB]);

                //Adjust the index
                indexB--;
                if (indexB == -1)
                    indexB = polygonB.Count - 1;

                //Get new point
                currentPointB = polygonB[indexB];
            }

            //Upper tangent
            currentPointA = GetRightmostPoint(polygonA);
            currentPointB = GetLeftmostPoint(polygonB);

            indexA = polygonA.ToList().IndexOf(currentPointA);
            indexB = polygonB.ToList().IndexOf(currentPointB);

            //Get A's lowest point
            while (currentPointA.X < polygonA.Max(p => p.X))
            {
                //Remove this point
                pointsToRemoveA.Add(polygonA[indexA]);

                //Adjust the index
                indexA++;
                if (indexA == polygonA.Count)
                    indexA = 0;

                //Get new point
                currentPointA = polygonA[indexA];
            }

            //Get B's lowest point
            while (currentPointB.X < polygonB.Max(p => p.X))
            {
                //Remove this point
                pointsToRemoveB.Add(polygonB[indexB]);

                //Adjust the index
                indexB++;
                if (indexB == polygonB.Count)
                    indexB = 0;

                //Get new point
                currentPointB = polygonB[indexB];
            }

            
            //Delete any points between the tangents
            for (int i = 0; i < polygonA.Count; i++)
            {
                if (pointsToRemoveA.Contains(polygonA[i]))
                    polygonA.RemoveAt(i--);
            } 
            
            for (int i = 0; i < polygonB.Count; i++)
            {
                if (pointsToRemoveB.Contains(polygonB[i]))
                    polygonB.RemoveAt(i--);
            }

            //Merge A and B
            var mergedPolygons = new List<Point2D>();
            mergedPolygons.AddRange(polygonA);
            mergedPolygons.AddRange(polygonB);

            return mergedPolygons;
        }*/

        public static Boolean IsUpperTangent(List<Point2D> polygon, Point2D ownedByPolygon)
        {

            Int32 higherY = ownedByPolygon.Y;

            if (PolygonManipulation.GetPreviousPoint(polygon, ownedByPolygon).Y <= higherY &&
                PolygonManipulation.GetNextPoint(polygon, ownedByPolygon).Y <= higherY)
                return true;
            return false;
        }

        public static Boolean IsLowerTangent(List<Point2D> polygon, Point2D ownedByPolygon)
        {
            Int32 lowerY = ownedByPolygon.Y;

            if (PolygonManipulation.GetPreviousPoint(polygon, ownedByPolygon).Y >= lowerY &&
                PolygonManipulation.GetNextPoint(polygon, ownedByPolygon).Y >= lowerY)
                return true;
            return false;
        }

        public static double test(Point2D p, Point2D q, Point2D r)
        {
            if ((p.X - q.X) == 0)
                return -double.MaxValue;
            double m = (p.Y - q.Y)/(p.X-q.X);
            double b = -m * p.X + p.Y; 
            return m * r.X + b - r.Y;
        }
    }
}
