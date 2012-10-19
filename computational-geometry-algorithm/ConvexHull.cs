using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace computational_geometry_algorithm
{
    public static class ConvexHull
    {
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

        }

        public static List<Point2D> Solve(List<Point2D> points)
        {
            //Sort points by x
            var sortedPoints = points.OrderBy(p => p.X).ToList();

            var upperHull = UpperHull(sortedPoints);
            var lowerHull = LowerHull(sortedPoints);

            //Merge hulls
            lowerHull.RemoveAt(0);
            lowerHull.RemoveAt(lowerHull.Count - 1);

            return UnionHulls(lowerHull, upperHull);
        }

        private static List<Point2D> UnionHulls(List<Point2D> lowerHull, List<Point2D> upperHull)
        {
            lowerHull.AddRange(upperHull);
            return lowerHull.Distinct().ToList();
        }

        private static List<Point2D> UpperHull(List<Point2D> points)
        {
            List<Point2D> upperHull = new List<Point2D>();
            upperHull.Add(points[0]);
            upperHull.Add(points[1]);

            for (int i = 2; i < points.Count; i++)
            {
                upperHull.Add(points[i]);
                while (upperHull.Count > 2 && NoRightTurn(upperHull[upperHull.Count - 1], upperHull[upperHull.Count - 2], upperHull[upperHull.Count - 3]))
                {
                    upperHull.RemoveAt(upperHull.Count - 2);
                }
            }

            return upperHull;
        }

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

        public static void Draw(List<Point2D> points)
        {
            //Group by y values
            var groupedPoints = points.GroupBy(p => p.Y);

            //Order groups by y values
            groupedPoints = groupedPoints.OrderBy(gp => gp.First().Y);

            int currentRow = groupedPoints.Last().First().Y + 1;

            for (int i = groupedPoints.Count() - 1; i >= 0; i--)
            {

                var newGroup = groupedPoints.ToList()[i].OrderBy(g => g.X);

                
                //Row check
                while (currentRow != newGroup.First().Y + 1)
                {
                    Console.Write("\n");
                    currentRow--;
                }
                currentRow = newGroup.First().Y;

                int currentPositionInRow = 0;

                foreach (var point in newGroup)
                {
                    for (int r = currentPositionInRow; r < point.X; r++)
                    {
                        Console.Write(" ");
                    }
                    Console.Write("#");
                    currentPositionInRow = point.X + 1;
                }
                Console.Write("\n");
            }

        }
    }
}
