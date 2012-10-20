using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace computational_geometry_algorithm
{
    /// <summary>
    /// Contains a start point A, mid point B, end point C
    /// Multiple polygons (lowercase letters)
    /// For now the polygons cannot border or intersect - there must be at least a one tile gap between them
    /// </summary>
    public class Map
    {
        Point2D Start, Mid, End;
        public Dictionary<char,List<Point2D>> Polygons; //Maps a character to the polygon

        public Map(String map)
        {
            Polygons = new Dictionary<char, List<Point2D>>();
            ImportFromString(map);
        }

        /// <summary>
        /// Inputs the map from a string
        /// </summary>
        public void ImportFromString(String data)
        {
            //Split data by lines
            String[] row = data.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            for (int r = 0; r < row.Count(); r++)
            {
                //Parse each character in the row
                for (int c = 0; c < row[r].Length; c++)
                {
                    //Inverting the y value preserves the y axis
                    int x = c;
                    int y = row.Count() - 1 - r;

                    switch (row[r][c])
                    {
                        case ' ':
                            break;

                        //Robot Start
                        case 'A':
                            Start = new Point2D(x, y);
                            break;

                        //Robot Mid
                        case 'B':
                            Mid = new Point2D(x, y);
                            break;

                        //Robot End
                        case 'C':
                            End = new Point2D(x, y);
                            break;

                        //Polygon Edge
                        default:
                            if (!Polygons.ContainsKey(row[r][c]))
                                Polygons.Add(row[r][c], new List<Point2D>());
                            Polygons[row[r][c]].Add(new Point2D(x, y));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Finds the quickest path avoiding the polygons given a start and end point
        /// Excludes start and end from the path
        /// Need to check for collisions with other polygons
        /// </summary>
        private List<Point2D> FindQuickestPath(Point2D start, Point2D end)
        {
            List<Point2D> shortestPath = new List<Point2D>();
            float shortestDistance = float.MaxValue;
            Boolean pathFound = false;

            foreach (var polygon in Polygons.Values)
            {
                //Calcualte convex hull with start and end point and polygon
                var newPolygon = new List<Point2D>();
                newPolygon.Add(start);
                newPolygon.AddRange(polygon);
                newPolygon.Add(end);

                newPolygon = ConvexHull.GetMinimumPolygonChain(ConvexHull.Solve(newPolygon), start, end);
                float distance = ConvexHull.GetPolygonChainDistance(newPolygon);

                //If we haven't found a path yet or this path is the shortest
                if (!pathFound || distance < shortestDistance)
                {
                    newPolygon.Remove(start);
                    newPolygon.Remove(end);
                    shortestDistance = distance;
                    shortestPath = newPolygon;
                    pathFound = true;
                }
            }

            return shortestPath;
        }

        public void SolveMap()
        {
            List<Point2D> path = new List<Point2D>();
            path.Add(Start);

            var start = Start;

            //If the mid point exists we must go to it first
            if (Mid != null)
            {
                path.AddRange(FindQuickestPath(Start, Mid));
                path.Add(Mid);
                start = Mid;
            }

            //Go to the end
            path.AddRange(FindQuickestPath(start, End));
            path.Add(End);

            UserInterface.DrawPath(path, Polygons.Values.ToList());

        }
    }
}
