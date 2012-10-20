using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace computational_geometry_algorithm
{
    /// <summary>
    /// Provides methods to display data in the console
    /// </summary>
    public static class UserInterface
    {
        /// <summary>
        /// Draws a list of points to the console in a 2d grid format
        /// </summary>
        public static void Draw(List<Point2D> points)
        {
            //Assign priority characters
            //a = top of list, z = end of list
            Dictionary<Point2D, char> priority = new Dictionary<Point2D, char>();
            char current = 'a';
            foreach (var point in points)
            {
                priority.Add(point, current++);
            }

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
                    Console.Write(priority[point]);
                    currentPositionInRow = point.X + 1;
                }
                Console.Write("\n");
            }

        }
    }
}
