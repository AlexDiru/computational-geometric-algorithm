using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace computational_geometry_algorithm
{
    /// <summary>
    /// Algorithms to modify the DataSet (List of Point2D)
    /// </summary>
    public static class DataSet
    {
        /// <summary>
        /// Converts a string to 2d point data
        /// # = Polygon Edge
        /// A = Robot Start position
        /// B = Robot End position
        /// </summary>
        public static List<Point2D> GetDataFromString(String data)
        {
            //Split data by lines
            String[] row = data.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            List<Point2D> dataSet = new List<Point2D>();
            Point2D start = null;
            Point2D end = null;

            for (int r = 0; r < row.Count(); r++ )
            {
                //Parse each character in the row
                for (int c = 0; c < row[r].Length; c++)
                {
                    //Inverting the y value preserves the y axis
                    int x = c;
                    int y = row.Count() - 1 - r;

                    switch (row[r][c])
                    {
                        //Polygon Edge
                        case '#':
                            dataSet.Add(new Point2D(x,y));
                            break;

                        //Robot Start
                        case 'A':
                            start = new Point2D(x, y);
                            break;

                        //Robot End
                        case 'B':
                            end = new Point2D(x, y);
                            break;
                    }
                }
            }

            //Insert start and end
            if (start != null)
                dataSet.Insert(0, start);
            if (end != null)
                dataSet.Add(end);

            return dataSet;
        }

        /// <summary>
        /// Whether a list of points contains a certain point
        /// </summary>
        public static Boolean Contains(List<Point2D> points, Point2D point)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X == point.X && points[i].Y == point.Y)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks whether two given points are equal
        /// </summary>
        public static Boolean Equals(Point2D a, Point2D b)
        {
            return (a.X == b.X) && (a.Y == b.Y);
        }
    }
}
