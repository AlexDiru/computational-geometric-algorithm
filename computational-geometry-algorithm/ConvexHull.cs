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

        public static void Solve(List<Point2D> points)
        {
            //Sort points by x
            var sortedPoints = points.OrderBy(p => p.X);

            var upperHull = UpperHull(sortedPoints);
            var lowerHull = LowerHull(sortedPoints);
        }

        private static List<Point2D> UpperHull(List<Point2D> points)
        {
            List<Point2D> upperHull = new List<Point2D>();
            upperHull.Add(points[0]);
            upperHull.Add(points[1]);

            for (int i = 2; i < points.Count; i++)
            {
                upperHull.Add(points[i]);
                while (upperHull.Count > 2 && NoRightTurn(upperHull[0], upperHull[1], upperHull[2]))
                {
                    upperHull.RemoveAt(1);
                }
            }

            return upperHull;
        }

        private static List<Point2D> LowerHull(List<Point2D> points)
        {
            List<Point2D> lowerHull = new List<Point2D>();
            lowerHull.Add(points.Last());
            lowerHull.Add(points[points.Count - 2]);
        }

        /// <summary>
        /// Whether three points make a right turn
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private Boolean NoRightTurn(Point2D a, Point2D b, Point2D c)
        {

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
