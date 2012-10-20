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
        public static List<Point2D> GenerateRandomPolygon(Int32 maxPointNumber, Int32 maxX, Int32 maxY)
        {
            //Create points
            List<Point2D> pointList = new List<Point2D>();
            for (int p = 0; p < maxPointNumber; p++)
            {
                Point2D newPoint = new Point2D(random.Next(maxX), random.Next(maxY));

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

                Console.WriteLine(String.Format("Test #{0} | Point Number = {1}",i+1, pointList.Count));
                UserInterface.Draw(pointList);

                pointList = ConvexHull.Solve(pointList);
                Console.WriteLine("Convex Hull | Point Number = " + pointList.Count);
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
            //Must have an empty top and bottom and left and right
            String generatedMap = "";

            Random random = new Random();

            var polygon = GenerateRandomPolygon(maxPointNumber, maxX, maxY);
            
            //Insert polygon into generated Map
            Int32 polygonWidth = polygon.Max(p => p.X);
            Int32 polygonHeight = polygon.Max(p => p.Y);
            Int32 mapWidth = polygonWidth + 2 + random.Next(10);
            Int32 mapHeight = polygonHeight + 2 + random.Next(10);
            Int32 leftOffset = mapWidth / 2;
            Int32 topOffset = mapHeight / 2;

            for (;;)
        }   
    }
}
