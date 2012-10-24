using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace computational_geometry_algorithm
{
    /// <summary>
    /// Contains methods to manipulate polygons
    /// </summary>
    public static class PolygonManipulation
    {
        /// <summary>
        /// Converts a list of Point2D to a list of Point
        /// </summary>
        public static List<Point> ConvertPolygon(List<Point2D> polygon, Int32 sizeMultiplier = 1, Int32 offsetX = 0,Int32 offsetY = 0)
        {
            var points = new List<Point>();

            foreach (var point in polygon)
            {
                points.Add(point.Convert(offsetX, offsetY, sizeMultiplier));
            }

            return points;
        }

        /// <summary>
        /// Whether a list of points contains a certain point
        /// </summary>
        public static Boolean Contains(List<Point2D> points, Point2D point)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X == point.X && points[i].Y == point.Y)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks whether two given points are equal
        /// </summary>
        public static Boolean Equals(Point2D a, Point2D b)
        {
            return (a.X == b.X) && (a.Y == b.Y);
        }

        /// <summary>
        /// Gets the point on a polygon which is closest to a given point
        /// </summary>
        public static Point2D GetClosestPoint(List<Point2D> polygon, Point2D from)
        {
            float minDistance = float.MaxValue;
            Point2D closestPoint = null;

            foreach (var point in polygon)
            {
                var distance = ConvexHull.GetDistance(point, from);
                if (distance < minDistance && !PolygonManipulation.Equals(from, point))
                {
                    minDistance = distance;
                    closestPoint = point;
                }
            }

            return closestPoint;
        }

        /// <summary>
        /// Outputs the points of the polygon as a string
        /// </summary>
        public static String Output(List<Point2D> polygon)
        {
            StringBuilder output = new StringBuilder();
            foreach (var point in polygon)
            {
                output.Append(point.Output());
                output.Append(" ");
            }

            //Remove last space
            return output.ToString().Substring(0, output.Length - 1);
        }

        public static String Output(List<Point> polygon)
        {
            StringBuilder output = new StringBuilder();
            foreach (var point in polygon)
            {
                output.Append(String.Format("({0},{1}) ", point.X/GraphicalUserInterface.SizeMultiplier, point.Y/GraphicalUserInterface.SizeMultiplier));
            }

            //Remove last space
            return output.ToString().Substring(0, output.Length - 1);
        }

        public static Point2D GetPreviousPoint(List<Point2D> polygon, Point2D currentPoint)
        {
            var index = PolygonManipulation.IndexOf(polygon, currentPoint);
            if (index == 0)
                return polygon[polygon.Count - 1];
            else
                return polygon[index-1];
        }

        public static Point2D GetNextPoint(List<Point2D> polygon, Point2D currentPoint)
        {
            var index = PolygonManipulation.IndexOf(polygon, currentPoint);
            if (index == polygon.Count - 1)
                return polygon[0];
            else
                return polygon[index+1];
        }

        public static List<Point2D> SortTopogically(List<Point2D> points)
        {
            var sortedPoints = new List<Point2D>();

            //Sort the points by x coords
            var xSortedPoints = points.OrderBy(p => p.X);

            //Sort the points by y coords
            var groupedPoints = xSortedPoints.GroupBy(p => p.X);

            foreach (var group in groupedPoints)
            {
                sortedPoints.AddRange(group.OrderBy(g => g.Y));
            }

            return sortedPoints;
        }

        /// <summary>
        /// Value equivalent of .IndexOf
        /// </summary>
        public static Int32 IndexOf(List<Point2D> polygon, Point2D p)
        {
            for (int i = 0; i < polygon.Count; i++)
            {
                if (polygon[i].X == p.X && polygon[i].Y == p.Y)
                    return i;
            }
            return -1;
        }

    }
}