using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace computational_geometry_algorithm
{
    public static class ConvexHull
    {
        /*
        public static List<Point2D> TopologicalSort(List<Point2D> Points)
        {
            //Group by x values
            var groupedPoints = Points.GroupBy(p => p.X);

            //Order groups by x values
            groupedPoints = groupedPoints.OrderBy(gp => gp.First().X);

            List<Point2D> newPoints = new List<Point2D>();

            foreach (var group in groupedPoints)
            {
                //Group each group by y values
                newPoints.AddRange(group.OrderBy(g => g.Y));
            }

            return newPoints;

        }*/

        /// <summary>
        /// Performs the ConvexHull algorithm on a set of points
        /// Algorithm adapted from pseudocode from Dr Muniyappa Manjunathaiah
        /// </summary>
        public static List<Point2D> Solve(List<Point2D> points)
        {
            //Sort points by x coordinates
            var sortedPoints = points.OrderBy(p => p.X).ToList();

            //Calculate the upper and lower hull of the points
            var upperHull = UpperHull(sortedPoints);
            var lowerHull = LowerHull(sortedPoints);

            //Remove the first and last point in the lower hull
            lowerHull.RemoveAt(0);
            lowerHull.RemoveAt(lowerHull.Count - 1);

            //Union both the hulls to get the convex hull
            return UnionHulls(lowerHull, upperHull);
        }

        /// <summary>
        /// Given two lists of points, merges them and removes any duplicate values
        /// </summary>
        private static List<Point2D> UnionHulls(List<Point2D> lowerHull, List<Point2D> upperHull)
        {
            lowerHull.AddRange(upperHull);
            return lowerHull.Distinct().ToList();
        }

        /// <summary>
        /// Calculates the upper hull
        /// </summary>
        private static List<Point2D> UpperHull(List<Point2D> points)
        {
            List<Point2D> upperHull = new List<Point2D>();
            upperHull.Add(points[0]);
            upperHull.Add(points[1]);

            for (int i = 2; i < points.Count; i++)
            {
                upperHull.Add(points[i]);

                //While upper hull has more than two points and the last three points do not make a right turn
                while (upperHull.Count > 2 && NoRightTurn(upperHull[upperHull.Count - 1], upperHull[upperHull.Count - 2], upperHull[upperHull.Count - 3]))
                {
                    //Delete the middle of the 3 above points
                    upperHull.RemoveAt(upperHull.Count - 2);
                }
            }

            return upperHull;
        }

        /// <summary>
        /// Calculates the lower hull
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private static List<Point2D> LowerHull(List<Point2D> points)
        {
            List<Point2D> lowerHull = new List<Point2D>();
            lowerHull.Add(points.Last());
            lowerHull.Add(points[points.Count - 2]);

            for (int i = points.Count - 3; i >= 0; i--)
            {
                lowerHull.Add(points[i]);
                while (lowerHull.Count > 2 && NoRightTurn(lowerHull[lowerHull.Count - 1], lowerHull[lowerHull.Count - 2], lowerHull[lowerHull.Count - 3]))
                {
                    lowerHull.RemoveAt(lowerHull.Count - 2);
                }
            }

            return lowerHull;
        }

        /// <summary>
        /// Whether three points make a right turn
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private static Boolean NoRightTurn(Point2D a, Point2D b, Point2D c)
        {
            return CrossProduct(a, b, c) < 0;
        }

        private static Int32 CrossProduct(Point2D a, Point2D b, Point2D c)
        {
            return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y)*(c.X - a.X);
        }

        public static Boolean Contains(List<Point2D> points, Point2D point)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X == point.X && points[i].Y == point.Y)
                    return true;
            }
            return false;
        }
    }
}
