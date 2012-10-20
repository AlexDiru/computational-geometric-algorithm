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
        // Start - represented by A on the map
        // Mid - represented by B on the map
        // End - represented by C on the map
        // Mid can be null, if mid is null the path is A->C, else the path is A->B->C
        Point2D Start, Mid, End;

        // Maps a character (sprite) to a polygon, each polygon has a unique sprite to
        // identify it as a separate polygon from the others
        public Dictionary<char,List<Point2D>> Polygons;

        /// <summary>
        /// Base constructor - initialises the polygon map
        /// </summary>
        public Map()
        {
            Polygons = new Dictionary<char, List<Point2D>>();
        }

        /// <summary>
        /// Constructor used when a string is passed
        /// Parses the string and generates the map
        /// </summary>
        public Map(String map) : this()
        {
            ImportFromString(map);
        }

        /// <summary>
        /// Constructor used when a list of polygons and the start and end coordinates are
        /// passed
        /// </summary>
        public Map(List<List<Point2D>> polygons, Point2D start, Point2D end) : this()
        {
            //The sprite of the current polygon
            char currentKey = 'a';

            foreach (var polygon in polygons)
            {
                //Add the polygon to the map with its unique sprite
                Polygons.Add(currentKey++, polygon);
            }

            //Set the start and end coordinates
            Start = start;
            End = end;
        }

        /// <summary>
        /// Inputs the map from a string
        /// ' ' = Empty space
        /// A = Start
        /// B = Midpoint (optional)
        /// C = End
        /// other = Polygon
        /// </summary>
        public void ImportFromString(String data)
        {
            //Split data by lines
            String[] row = data.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            //Iterate through the rows of the string
            for (int r = 0; r < row.Count(); r++)
            {
                //Parse each character in the row
                for (int c = 0; c < row[r].Length; c++)
                {
                    //The x coordinate of the character
                    int x = c;

                    //Inverting the y value preserves the y axis
                    int y = row.Count() - 1 - r;

                    switch (row[r][c])
                    {
                        //Empty space
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
        /// Finds the quickest path avoiding the polygon given a start and end point
        /// Excludes start and end from the path
        /// </summary>
        private List<Point2D> FindQuickestPathSinglePolygon(Point2D start, Point2D end)
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

        /// <summary>
        /// Finds the quickest path avoiding the polygons given a start and end point
        /// Excludes start and end from the path
        /// Need to check for collisions with other polygons
        /// </summary>
        private List<Point2D> FindQuickestPathMultiplePolygons(Point2D start, Point2D end)
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

        /// <summary>
        /// Finds the path through the map
        /// </summary>
        public void SolveMap()
        {
            List<Point2D> path = new List<Point2D>();
            path.Add(Start);

            var start = Start;

            //If the mid point exists we must go to it first
            if (Mid != null)
            {
                if (Polygons.Count == 1)
                    path.AddRange(FindQuickestPathSinglePolygon(Start, Mid));
                else
                    path.AddRange(FindQuickestPathMultiplePolygons(start, Mid));
                
                path.Add(Mid);
                start = Mid;
            }

            //Go to the end
            if (Polygons.Count == 1)
                path.AddRange(FindQuickestPathSinglePolygon(Start, End));
            else
                path.AddRange(FindQuickestPathMultiplePolygons(start, End));
            path.Add(End);

            UserInterface.DrawPath(path, Polygons.Values.ToList());

        }
    }
}
