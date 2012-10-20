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

        public void SolveMap()
        {
            //If the mid point exists we must go to it first
            if (Mid != null)
            {
                
            }
        }
    }
}
