using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace computational_geometry_algorithm
{
    /// <summary>
    /// Randomly generates content
    /// </summary>
    public static class ProceduralGeneration
    {
        private static Random random = new Random();

        public static List<Point2D> GenerateRandomPolygon(Int32 maxPointNumber, Int32 maxX, Int32 maxY, Int32 offsetX = 0, Int32 offsetY = 0)
        {
            //Create points
            List<Point2D> pointList = new List<Point2D>();
            for (int p = 0; p < maxPointNumber; p++)
            {
                Point2D newPoint = new Point2D(random.Next(maxX) + offsetX, random.Next(maxY) + offsetY);

                if (!PolygonManipulation.Contains(pointList, newPoint))
                    pointList.Add(newPoint);
            }
            pointList = pointList.Distinct().ToList();

            return pointList;
        }

        /// <summary>
        /// Returns a map which is a single obstacle test
        /// </summary>
        public static Map GenerateSingleObstacleMap(Int32 maxPointNumber, Int32 maxX, Int32 maxY)
        {
            //Generate the map 

            var polygon = new List<List<Point2D>>();
            polygon.Add(GenerateRandomPolygon(maxPointNumber, maxX, maxY, 2, 2));

            //Insert polygon into generated Map
            Int32 polygonWidth = polygon.First().Max(p => p.X);
            Int32 polygonHeight = polygon.First().Max(p => p.Y);
            Int32 mapWidth = polygonWidth + 2;
            Int32 mapHeight = polygonHeight + 2;

            Point2D start = new Point2D(random.Next(2), Convert.ToInt32((random.Next(mapHeight) - 0.5 * mapHeight) + (float)mapHeight / 2));
            Point2D end = new Point2D(mapWidth - random.Next(2), mapHeight - start.Y);
            return new Map(polygon, start, end);
        }

        /// <summary>
        /// Polygon count must be between 1 and 4
        /// </summary>
        public static Map GenerateMultipleObstacleMap(Int32 polygonNumber, Int32 maxPointNumber, Int32 maxX, Int32 maxY)
        {
            if (polygonNumber == 1)
                return GenerateSingleObstacleMap(maxPointNumber, maxX, maxY);

            if (polygonNumber > 4)
                polygonNumber = 4;

            List<List<Point2D>> polygons = new List<List<Point2D>>();
            Point2D start = null;
            Point2D mid = null;
            Point2D end = null;

            //Spread the polygons out based in the number
            if (polygonNumber == 2)
            {
                //Arrange side by side
                /* ##### #####
                 * #   # #   #
                 * ##### #####
                */
                polygons.Add(GenerateRandomPolygon(maxPointNumber, maxX, maxY, 2, 2));
                polygons.Add(GenerateRandomPolygon(maxPointNumber, maxX, maxY, maxX + 3, 2));
                Int32 maxHeight = GetMaximumHeight(polygons);
                start = new Point2D(0, random.Next(maxHeight));
                mid = new Point2D(maxX + 2 + random.Next(1), random.Next(maxHeight));
                end = new Point2D(2 * maxX + 3, random.Next(maxHeight));
            }
            else if (polygonNumber == 3)
            {
                //Arrange in tri-shape
                /*   #####
                 *   #   #
                 *   #####
                 *   
                 * ##### #####
                 * #   # #   #
                 * ##### #####
                */
                polygons.Add(GenerateRandomPolygon(maxPointNumber, maxX, maxY, maxX / 2 + 2, 2));
                polygons.Add(GenerateRandomPolygon(maxPointNumber, maxX, maxY, 2, maxY + 3));
                polygons.Add(GenerateRandomPolygon(maxPointNumber, maxX, maxY, maxX + 3, maxY + 3));
                Int32 maxHeight = GetMaximumHeight(polygons);
                start = new Point2D(0, random.Next(maxHeight));
                mid = new Point2D(maxX + 2 + random.Next(1), random.Next(maxHeight / 2) + maxHeight / 2 + 2);
                end = new Point2D(2 * maxX + 3, random.Next(maxHeight));
            }
            else if (polygonNumber == 4)
            {
                //Arrange in sqaure
                /* ##### #####
                 * #   # #   #
                 * ##### #####
                 *   
                 * ##### #####
                 * #   # #   #
                 * ##### #####
                */
                polygons.Add(GenerateRandomPolygon(maxPointNumber, maxX, maxY, 2, 2));
                polygons.Add(GenerateRandomPolygon(maxPointNumber, maxX, maxY, maxX + 3, 2));
                polygons.Add(GenerateRandomPolygon(maxPointNumber, maxX, maxY, 2, maxY + 3));
                polygons.Add(GenerateRandomPolygon(maxPointNumber, maxX, maxY, maxX + 3, maxY + 3));
                Int32 maxHeight = GetMaximumHeight(polygons);
                start = new Point2D(0, random.Next(maxHeight));
                mid = new Point2D(maxX + 2 + random.Next(1), random.Next(maxHeight));
                end = new Point2D(2 * maxX + 3, random.Next(maxHeight));
            }

            return new Map(polygons, start, end) { Mid = mid };
        }

        /// <summary>
        /// Gets the maximum Y coordinate of a point of any polygon in the list
        /// </summary>
        private static Int32 GetMaximumHeight(IEnumerable<List<Point2D>> polygons)
        {
            Int32 max = 0;
            foreach (var polygon in polygons)
            {
                Int32 newMax = polygon.Max(p => p.Y);
                if (newMax > max)
                    max = newMax;
            }

            return max;
        }
    }
}