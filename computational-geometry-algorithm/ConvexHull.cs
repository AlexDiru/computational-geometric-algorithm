using System;
using System.Collections.Generic;
using System.Linq;

namespace computational_geometry_algorithm
{
    public static class ConvexHull
    {
        /// <summary>
        /// Performs the ConvexHull algorithm on a set of points
        /// Algorithm adapted from pseudocode from Dr Muniyappa Manjunathaiah
        /// </summary>
        public static List<Point2D> Solve(List<Point2D> points)
        {
            //Sort points by x coordinates
            var xSortedPoints = points.OrderBy(p => p.X).ToList();

            //In case of x coordinates equal, sort by y
            var groupedPoints = xSortedPoints.GroupBy(sp => sp.X);
            var sortedPoints = new List<Point2D>();
            foreach (var group in groupedPoints)
            {
                sortedPoints.AddRange(group.OrderBy(g => g.Y));
            }

            //Calculate the upper and lower hull of the points
            var upperHull = UpperHull(sortedPoints);
            var lowerHull = LowerHull(sortedPoints);

            //Remove the first and last point in the lower hull
            upperHull.RemoveAt(upperHull.Count - 1);
            lowerHull.RemoveAt(lowerHull.Count - 1);

            //Union both the hulls to get the convex hull
            return UnionHulls(lowerHull, upperHull);
        }

        /// <summary>
        /// Given two lists of points, merges them and removes any duplicate values
        /// </summary>
        public static List<Point2D> UnionHulls(List<Point2D> lowerHull, List<Point2D> upperHull)
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

            for (int i = 0; i < points.Count; i++)
            {
                //While upper hull has more than two points and the last three points do not make a right turn
                while (upperHull.Count >= 2 && NoRightTurn(upperHull[upperHull.Count - 2], upperHull[upperHull.Count - 1], points[i]))
                {
                    //Delete the middle of the 3 above points
                    upperHull.RemoveAt(upperHull.Count - 1);
                }
                upperHull.Add(points[i]);
            }

            return upperHull;
        }

        /// <summary>
        /// Calculates the lower hull
        /// </summary>
        private static List<Point2D> LowerHull(List<Point2D> points)
        {
            List<Point2D> lowerHull = new List<Point2D>();

            for (int i = points.Count - 1; i >= 0; i--)
            {
                while (lowerHull.Count >= 2 && NoRightTurn(lowerHull[lowerHull.Count - 2], lowerHull[lowerHull.Count - 1], points[i]))
                {
                    lowerHull.RemoveAt(lowerHull.Count - 1);
                }
                lowerHull.Add(points[i]);
            }

            return lowerHull;
        }

        /// <summary>
        /// Whether three points make a right turn
        /// </summary>
        private static Boolean NoRightTurn(Point2D a, Point2D b, Point2D c)
        {
            return CrossProduct(a, b, c) <= 0;
        }

        /// <summary>
        /// Calculates the cross product of 3 2d points
        /// </summary>
        private static Int32 CrossProduct(Point2D a, Point2D b, Point2D c)
        {
            return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y)*(c.X - a.X);
        }

        /// <summary>
        /// Gets the center of a polygon
        /// </summary>
        public static Point2D GetPolygonCentre(List<Point2D> polygon)
        {
            Int32 xTotal = 0;
            Int32 yTotal = 0;

            foreach (Point2D point in polygon)
            {
                xTotal += point.X;
                yTotal += point.Y;
            }

            //Return the average of the points
            return new Point2D(xTotal / polygon.Count, yTotal / polygon.Count);
        }

        /// <summary>
        /// Organises a list so it can be traversed clockwise or counterclockwise
        /// Uses crossproduct - quite inefficient algorithm O(n^2)
        /// If time, speed this up
        /// Algorithm from http://gamedev.stackexchange.com/questions/13229/sorting-array-of-points-in-clockwise-order
        /// Only works on convex hulls!!
        /// </summary>
        /*public static List<Point2D> OrganiseClockwise(List<Point2D> points)
        {
            //Calculate centre of points
            Point2D centre = GetPolygonCentre(points);

            //Assigns the points a float priority which is calculated later
            //Must map to a list on the off chance two (or more) points have the same key
            var priorityPoints = new Dictionary<float, List<Point2D>>();

            foreach (var point in points)
            {
                float priorityValue = (float)Math.Atan2(Convert.ToDouble(point.Y - centre.Y), Convert.ToDouble(point.X - centre.X));

                if (!priorityPoints.ContainsKey(priorityValue))
                {
                    priorityPoints.Add(priorityValue, new List<Point2D>());
                }
                priorityPoints[priorityValue].Add(point);
            }

            //Sort by priority value
            priorityPoints.OrderBy(pp => pp.Key);

            //Convert dictionary back a single list
            var sortedList = new List<Point2D>();
            foreach (var list in priorityPoints.Values)
            {
                sortedList.AddRange(list);
            }

            return sortedList;
        }*/

        /// <summary>
        /// Gets the Manhattan distance between two points
        /// </summary>
        public static float GetDistance(Point2D a, Point2D b)
        {
            return (float)Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        /// <summary>
        /// Given a polygon chain, calculates the total distance if it is followed
        /// </summary>
        public static float GetPolygonChainDistance(List<Point2D> polygonChain)
        {
            float totalDistance = 0;
            for (int i = 0; i < polygonChain.Count - 1; i++)
            {
                totalDistance += GetDistance(polygonChain[i], polygonChain[i + 1]);
            }
            return totalDistance;
        }

        /// <summary>
        /// Given a convex hull and a start point and end point which are points on the convex hull
        /// Traverses the convex hull both clockwise and counterwise and returns the list of points with the minimum length
        /// </summary>
        public static List<Point2D> GetMinimumPolygonChain(List<Point2D> convexHull, Point2D start, Point2D end)
        {
            var circularHull = convexHull;// OrganiseClockwise(convexHull);

            var clockwiseChain = new List<Point2D>();
            var counterClockwiseChain = new List<Point2D>();

            //Clockwise chain
            //Start at the start index
            Int32 currentIndex = circularHull.IndexOf(start);

            //While the end isn't reached
            while (!PolygonManipulation.Equals(circularHull[currentIndex], end) || circularHull.Count == clockwiseChain.Count)
            {
                //Add the point to the polygon chain
                clockwiseChain.Add(circularHull[currentIndex]);

                //Go to next point
                if (currentIndex < circularHull.Count - 1)
                    currentIndex++;
                else
                    currentIndex = 0;
            }

            //Add the end
            clockwiseChain.Add(end);

            //Counterclockwise chain
            //Start at the start index
            currentIndex = circularHull.IndexOf(start);

            //While the end isn't reached
            while (!PolygonManipulation.Equals(circularHull[currentIndex], end) || circularHull.Count == counterClockwiseChain.Count)
            {
                //Add the point to the polygon chain
                counterClockwiseChain.Add(circularHull[currentIndex]);

                //Go to prev point
                if (currentIndex > 0)
                    currentIndex--;
                else
                    currentIndex = circularHull.Count - 1;
            }

            //Add the end
            counterClockwiseChain.Add(end);

            //Return the shortest chain
            return GetPolygonChainDistance(counterClockwiseChain) > GetPolygonChainDistance(clockwiseChain) ? clockwiseChain : counterClockwiseChain;
        }
    }
}