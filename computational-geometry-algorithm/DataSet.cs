using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace computational_geometry_algorithm
{
    /// <summary>
    /// Algorithms to modify the DataSet (List of Point2D)
    /// </summary>
    public static class DataSet
    {
        /// <summary>
        /// Converts a list of Point2D to a list of Point
        /// </summary>
        public static List<Point> ConvertPolygon(List<Point2D> polygon, Int32 sizeMultiplier = 1)
        {
            List<Point> points = new List<Point>();

            foreach (var point in polygon)
            {
                points.Add(point.Convert(0,0,sizeMultiplier));
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
    }
}
