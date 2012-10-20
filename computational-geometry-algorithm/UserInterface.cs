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

        /// <summary>
        /// Draws a list of points to the console in a 2d grid format
        /// </summary>
        public static void DrawPath(List<Point2D> path, List<List<Point2D>> polygons)
        {
            //Maps a point to a path
            List<KeyValuePair<Point2D, char>> points = new List<KeyValuePair<Point2D, char>>();

            //Add path to 'biglist'
            foreach (var point in path)
                points.Add(new KeyValuePair<Point2D, char>(point, '.'));

            //Add polygons to 'biglist'
            foreach (var polygon in polygons)
                foreach (var point in polygon)
                    points.Add(new KeyValuePair<Point2D, char>(point, '#'));

            //Since some path points will be the same as polygon points
            //We need to remove those polygon points
            for (int p = 0; p < points.Count - 1; p++)
            {
                for (int q = p + 1; q < points.Count; q++)
                {
                    if (DataSet.Equals(points[p].Key, points[q].Key))
                    {
                        if (points[p].Value == '#')
                            points.RemoveAt(p);
                        else
                            points.RemoveAt(q);
                    }
                }
            }

            //Group by y values
            var groupedPoints = points.GroupBy(p => p.Key.Y);

            //Order groups by y values
            groupedPoints = groupedPoints.OrderBy(gp => gp.First().Key.Y);

            int currentRow = groupedPoints.Last().First().Key.Y + 1;

            for (int i = groupedPoints.Count() - 1; i >= 0; i--)
            {

                var newGroup = groupedPoints.ToList()[i].OrderBy(g => g.Key.X);


                //Row check
                while (currentRow != newGroup.First().Key.Y + 1)
                {
                    Console.Write("\n");
                    currentRow--;
                }
                currentRow = newGroup.First().Key.Y;

                int currentPositionInRow = 0;

                foreach (var point in newGroup)
                {
                    for (int r = currentPositionInRow; r < point.Key.X; r++)
                    {
                        Console.Write(" ");
                    }
                    Console.Write(point.Value);
                    currentPositionInRow = point.Key.X + 1;
                }
                Console.Write("\n");
            }

        }
    }

}
