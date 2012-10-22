﻿using System;
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
            map.SolveMap();
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
    }
}
