using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace computational_geometry_algorithm
{
    /// <summary>
    /// Provides methods to test the convex hull algorithm
    /// </summary>
    public static class ConvexHullTesting
    {
        public static List<Point2D> GenerateRandomPolygon(Int32 maxPointNumber, Int32 maxX, Int32 maxY, Int32 offsetX = 0, Int32 offsetY = 0)
        {
            Random random = new Random();

            //Create points
            List<Point2D> pointList = new List<Point2D>();
            for (int p = 0; p < maxPointNumber; p++)
            {
                Point2D newPoint = new Point2D(random.Next(maxX)+offsetX, random.Next(maxY)+offsetY);

                if (!DataSet.Contains(pointList, newPoint))
                    pointList.Add(newPoint);
            }
            pointList = pointList.Distinct().ToList();

            return pointList;
        }

        public static void RandomisedTests(Int32 testNumber, Int32 maxPointNumber, Int32 maxX, Int32 maxY)
        {
            Random random = new Random();

            //Iterate tests
            for (int i = 0; i < testNumber; i++)
            {
                var pointList = GenerateRandomPolygon(maxPointNumber, maxX, maxY);

                Console.WriteLine(String.Format("\nTest #{0} | Point Number = {1}\n",i+1, pointList.Count));
                UserInterface.Draw(pointList);

                pointList = ConvexHull.Solve(pointList);
                Console.WriteLine("\nConvex Hull | Point Number = " + pointList.Count + "\n");
                UserInterface.Draw(pointList);
            }
        }

        public static void TestData(List<Point2D> pointList)
        {
            Console.WriteLine("Original | Point Number = " + pointList.Count);
            UserInterface.Draw(pointList);
            pointList = ConvexHull.Solve(pointList);
            Console.WriteLine("Convex Hull | Point Number = " + pointList.Count);
            UserInterface.Draw(pointList);
        }

        public static void PathPlannerSingleObstacleRandomisedTest(Int32 testNumber, Int32 maxPointNumber, Int32 maxX, Int32 maxY)
        {
            //Generate the map 

            Random random = new Random();

            List<List<Point2D>> polygon = new List<List<Point2D>>();
            polygon.Add(GenerateRandomPolygon(maxPointNumber, maxX, maxY,5, 5));
            
            //Insert polygon into generated Map
            Int32 polygonWidth = polygon.First().Max(p => p.X);
            Int32 polygonHeight = polygon.First().Max(p => p.Y);
            Int32 mapWidth = polygonWidth + 2 + random.Next(10);
            Int32 mapHeight = polygonHeight + 2 + random.Next(10);
            Int32 widthOffset = mapWidth - polygonWidth;
            Int32 heightOffset = mapHeight - polygonHeight;

            Point2D start = new Point2D(random.Next(5), random.Next(polygonHeight));
            Point2D end = new Point2D(random.Next(5) + polygonWidth + 1, random.Next(polygonHeight));
            Map map = new Map(polygon, start, end);

            UserInterface.DrawMap(start, end, polygon);
            Console.WriteLine("-------------------");
            map.SolveMap(null);
        }

        /// <summary>
        /// Returns a map which is a single obstacle test
        /// </summary>
        public static Map GenerateSingleObstacleMap(Int32 maxPointNumber, Int32 maxX, Int32 maxY)
        {
            //Generate the map 
            Random random = new Random();

            List<List<Point2D>> polygon = new List<List<Point2D>>();
            polygon.Add(GenerateRandomPolygon(maxPointNumber, maxX, maxY, 2, 2));

            //Insert polygon into generated Map
            Int32 polygonWidth = polygon.First().Max(p => p.X);
            Int32 polygonHeight = polygon.First().Max(p => p.Y);
            Int32 mapWidth = polygonWidth + 2;
            Int32 mapHeight = polygonHeight + 2;
            Int32 widthOffset = mapWidth - polygonWidth;
            Int32 heightOffset = mapHeight - polygonHeight;

            Point2D start = new Point2D(random.Next(2),Convert.ToInt32( (random.Next(mapHeight)-0.5*mapHeight) + mapHeight/2));
            Point2D end = new Point2D(mapWidth - random.Next(2), mapHeight - start.Y);
            return new Map(polygon, start, end);
        }

        /// <summary>
        /// Gets the maximum Y coordinate of a point of any polygon in the list
        /// </summary>
        public static Int32 GetMaximumHeight(List<List<Point2D>> polygons)
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

        /// <summary>
        /// Polygon count must be between 1 and 4
        /// </summary>
        public static Map GenerateMultipleObstacleMap(Int32 polygonNumber, Int32 maxPointNumber, Int32 maxX, Int32 maxY, GraphicalUserInterface GUI)
        {
            if (polygonNumber == 1)
                return GenerateSingleObstacleMap(maxPointNumber, maxX, maxY);

            if (polygonNumber > 4)
                polygonNumber = 4;

            Random random = new Random();

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
                polygons.Add(GenerateRandomPolygon(maxPointNumber, maxX, maxY, maxX + 3,2));
                Int32 maxHeight = GetMaximumHeight(polygons);
                start = new Point2D(0, random.Next(maxHeight));
                mid = new Point2D(maxX + 2 + random.Next(1), random.Next(maxHeight));
                end = new Point2D(2*maxX + 3, random.Next(maxHeight));
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
                polygons.Add(GenerateRandomPolygon(maxPointNumber, maxX, maxY, maxX/2 + 2, 2));
                polygons.Add(GenerateRandomPolygon(maxPointNumber, maxX, maxY, 2, maxY + 3));
                polygons.Add(GenerateRandomPolygon(maxPointNumber, maxX, maxY, maxX + 3, maxY + 3));
                Int32 maxHeight = GetMaximumHeight(polygons);
                start = new Point2D(0, random.Next(maxHeight));
                mid = new Point2D(maxX + 2 + random.Next(1), random.Next(maxHeight/2) + maxHeight/2 + 2);
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

            return new Map(polygons, start, end) { Mid = mid, gui = GUI };
        }
    }
}
