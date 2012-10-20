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
        public static void RandomisedTests(Int32 testNumber, Int32 maxPointNumber, Int32 maxX, Int32 maxY)
        {
            Random random = new Random();

            //Iterate tests
            for (int i = 0; i < testNumber; i++)
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
    }
}
