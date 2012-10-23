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
    }
}