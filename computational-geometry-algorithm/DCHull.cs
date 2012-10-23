using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace computational_geometry_algorithm
{
    public static class DCHull
    {
        private static Int32 MinimumSize = 4;

        public static List<Point2D> Solve(IEnumerable<Point2D> points)
        {
            if (points.Count() <= MinimumSize)
                return ConvexHull.Solve(points.ToList());

            var sortedPoints = new List<Point2D>();

            //Sort the points by x coords
            var xSortedPoints = points.OrderBy(p => p.X);

            //Sort the points by y coords
            var groupedPoints = xSortedPoints.GroupBy(p => p.X);

            foreach (var group in groupedPoints)
            {
                sortedPoints.AddRange(group.OrderBy(g => g.Y));
            }

            //Split points into two sets
            var a = new List<Point2D>();
            var b = new List<Point2D>();

            a.AddRange(sortedPoints.Take(sortedPoints.Count()/2));
            b.AddRange(sortedPoints.Skip(sortedPoints.Count()/2).Take(sortedPoints.Count() - sortedPoints.Count()/2));

            //Compute the convex hulls of a and b recursively
            a = Solve(a);
            b = Solve(b);

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

        private static Boolean IsLowerTangent(IEnumerable<Point2D> polygonA, IEnumerable<Point2D> polygonB, Point2D a, Point2D b)
        {
            return true;
        }

        private static List<Point2D> Merge(IEnumerable<Point2D> polygonA, IEnumerable<Point2D> polygonB)
        {
            Point2D rightmostPointA = GetRightmostPoint(polygonA);
            Point2D leftmostPointB = GetLeftmostPoint(polygonB);

            Int32 indexA = polygonA.ToList().IndexOf(rightmostPointA);
            Int32 indexB = polygonB.ToList().IndexOf(leftmostPointB);

            while (!IsLowerTangent(polygonA, polygonB, rightmostPointA, leftmostPointB))
            {

            }

            return null;
        }
    }
}
