using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using computational_geometry_algorithm.dc_hull;

namespace computational_geometry_algorithm
{
    public static class DCHull
    {
        private static Int32 MinimumSize = 4;
        public static Int32 XOffset = 0;
        public static Int32 YOffset = 0;

        public static List<Point2D> Solve(List<Point2D> points, bool stepThrough = false)
        {
            points = RecursiveSolve(Polygon.Get(points), stepThrough).Convert();
            return CleanUp(points);
        }

        /// <summary>
        /// This method uses a different method to the Monotone Chain convex hull
        /// Instead of storing the vertices of the polygon in lists
        /// The vertices are stored as doubly linked lists
        /// </summary>
        private static Polygon RecursiveSolve(Polygon points, bool stepThrough = false)
        {
            //If there are few enough points to warrant the Graham Scan algorithm
            if (points.Count() <= MinimumSize)
            {
                return Polygon.Get(ConvexHull.Solve(points.Convert()));
            }

            //Sort the points by x value and then by y value
            var sortedPoints = PolygonManipulation.SortLexographically(points.Convert());

            //Split points into two sets
            var a = new List<Point2D>();
            var b = new List<Point2D>();

            a.AddRange(sortedPoints.Take(sortedPoints.Count()/2));
            b.AddRange(sortedPoints.Skip(sortedPoints.Count()/2).Take(sortedPoints.Count() - sortedPoints.Count()/2));

            //Compute the convex hulls of a and b recursively
            var polyA = RecursiveSolve(Polygon.Get(a), stepThrough);
            var polyB = RecursiveSolve(Polygon.Get(b), stepThrough);

            //Merge the convex hulls together
            return Merge(polyA, polyB, stepThrough);
        }

        /// <summary>
        /// Cleans up the polygon to remove points which are along the same gradient lines
        /// </summary>
        private static List<Point2D> CleanUp(List<Point2D> wiredPolygon)
        {
            //The last part is to check every point of the wired polygon and make sure that all points that are
            //next to each other have a change in gradient
            //If the gradient remains the same from A -> B -> C, the three points lie on the same line and so
            //B can be deleted

            //Ignore triangles
            if (wiredPolygon.Count > 3)
            {
                int pointsCounter = 0;
                int i = 2;

                //While not a triangle and not a whole loop of points has been iterated without removal
                while (wiredPolygon.Count() > 3 && !(pointsCounter == wiredPolygon.Count() - 2))
                {
                    //If the two gradients are equal
                    if (GradientEquals(wiredPolygon[i - 1], wiredPolygon[i - 2], wiredPolygon[i], wiredPolygon[i - 1]))
                    {
                        //Delete the middle point
                        wiredPolygon.RemoveAt(i - 1);
                        i--;
                        pointsCounter = 0;
                    }
                    else
                        pointsCounter++;

                    i++;

                    //Reset to start index
                    if (i >= wiredPolygon.Count() || i < 2)
                        i = 2;
                }

                //Check i = 1
                if (wiredPolygon.Count > 3)
                    if (GradientEquals(wiredPolygon[0], wiredPolygon[wiredPolygon.Count - 1], wiredPolygon[1], wiredPolygon[0]))
                        wiredPolygon.RemoveAt(0);

                //Check i = 0
                if (wiredPolygon.Count > 3)
                    if (GradientEquals(wiredPolygon[wiredPolygon.Count - 1], wiredPolygon[wiredPolygon.Count - 2], wiredPolygon[0], wiredPolygon[wiredPolygon.Count - 1]))
                        wiredPolygon.RemoveAt(wiredPolygon.Count - 1);
            }

            return wiredPolygon;
        }

        /// <summary>
        /// Given a polygon, finds the rightmost point of it
        /// </summary>
        private static Point2D GetRightmostPoint(IEnumerable<Point2D> polygon)
        {
            //Get all max x points
            var xMaxPoints = polygon.Where(p => p.X == polygon.Max(q => q.X));
            return xMaxPoints.Where(p => p.Y == xMaxPoints.Min(q => q.Y)).First();
        }

        /// <summary>
        /// Given a polygon, finds the leftmost point of it
        /// </summary>
        private static Point2D GetLeftmostPoint(IEnumerable<Point2D> polygon)
        {
            //Get all max x points
            var yMinPoints = polygon.Where(p => p.X == polygon.Min(q => q.X));
            return yMinPoints.Where(p => p.Y == yMinPoints.Min(q => q.Y)).First();
        }

        /// <summary>
        /// Merges two convex hulls into one convex hull
        /// Finds upper and lower tangents and removes the points in between
        /// </summary>
        private static Polygon Merge(Polygon polygonA, Polygon polygonB, Boolean stepThrough)
        {
            //Since a lot of functions require polygons of type List<Point2D>
            //Convert them here to save time converting lots later on
            var newPolygonA = PolygonManipulation.SortLexographically(polygonA.Convert());
            var newPolygonB = PolygonManipulation.SortLexographically(polygonB.Convert());

            //Get the rightmost point of polygon A and the leftmost point of polygon B
            var tangentPointA = GetRightmostPoint(newPolygonA);
            var tangentPointB = GetLeftmostPoint(newPolygonB);

            //Find lower tangent
            //While Tangent = tangentPointA -> tangentPointB is not a lower tangent to both A and B
            while (!(IsLowerTangent(newPolygonA, tangentPointA, tangentPointB) && IsLowerTangent(newPolygonB, tangentPointB,tangentPointA)))
            {
                //While Tangent = tangentPointA -> tangentPointB is not a lower tangent to A
                while (!IsLowerTangent(newPolygonA, tangentPointA,tangentPointB))
                {
                    //Move counter clockwise
                    tangentPointA = PolygonManipulation.GetNextPoint(newPolygonA, tangentPointA);
                }

                //While Tangent = tangentPointA -> tangentPointB is not a lower tangent to B
                while (!IsLowerTangent(newPolygonB, tangentPointB,tangentPointA))
                {
                    //Move clockwise
                    tangentPointB = PolygonManipulation.GetPreviousPoint(newPolygonB, tangentPointB);
                }
            }

            //Get the lower tangent
            Vector2D lowerTangent = new Vector2D(tangentPointA, tangentPointB);

            //Reset the tangent points
            tangentPointA = GetRightmostPoint(newPolygonA);
            tangentPointB = GetLeftmostPoint(newPolygonB);

            //While Tangent = tangentPointA -> tangentPointB is not an upper tangent to both A and B
            while (!IsUpperTangent(newPolygonA, tangentPointA, tangentPointB) || !(IsUpperTangent(newPolygonB, tangentPointB, tangentPointA)))
            {
                //While Tangent = tangentPointA -> tangentPointB is not an upper tangent to A
                while (!IsUpperTangent(newPolygonA, tangentPointA, tangentPointB))
                {
                    //Move counter clockwise
                    tangentPointA = PolygonManipulation.GetPreviousPoint(newPolygonA, tangentPointA);
                }

                //While Tangent = tangentPointA -> tangentPointB is not an upper tangent to B
                while (!IsUpperTangent(newPolygonB, tangentPointB, tangentPointA))
                {
                    //Move clockwise
                    tangentPointB = PolygonManipulation.GetNextPoint(newPolygonB, tangentPointB);
                }
            }

            //Get the upper tangent
            Vector2D upperTangent = new Vector2D(tangentPointA, tangentPointB);

            //Draw tangents if in step through mode
            if (stepThrough)
            {
                //Convert the polygons into a format for drawing
                var listPolygonA = polygonA.Convert();
                var listPolygonB = polygonB.Convert();

                //If the polygons have more than two points, fill them in
                if (listPolygonA.Count > 2)
                {
                    GraphicalUserInterface.DrawPolygon(listPolygonA, true, XOffset, YOffset, new System.Drawing.SolidBrush(System.Drawing.Color.Coral));
                    GraphicalUserInterface.DrawPolygon(polygonB.Convert(), true, XOffset, YOffset);
                }

                GraphicalUserInterface.DrawPolygon(listPolygonA, false, XOffset, YOffset, new System.Drawing.SolidBrush(System.Drawing.Color.Coral));
                GraphicalUserInterface.DrawPolygon(polygonB.Convert(), false, XOffset, YOffset);
                GraphicalUserInterface.GraphicsObject.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Blue) { Width = 4 }, 
                                                               lowerTangent.Start.Convert(XOffset, YOffset, GraphicalUserInterface.SizeMultiplier), 
                                                               lowerTangent.Target.Convert(XOffset, YOffset, GraphicalUserInterface.SizeMultiplier));
                GraphicalUserInterface.GraphicsObject.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black) { Width = 4 }, 
                                                               upperTangent.Start.Convert(XOffset, YOffset, GraphicalUserInterface.SizeMultiplier), 
                                                               upperTangent.Target.Convert(XOffset, YOffset, GraphicalUserInterface.SizeMultiplier));

                //Wait for the user to enter a key into the console to continue
                Console.ReadKey();
            }

            //Wire together the polygons by tangent
            return Wire(polygonA, polygonB, lowerTangent, upperTangent);
        }

        /// <summary>
        /// Given two polygons, and two tangent which will join together
        /// This function will create a polygon which merge the two polygons together using the tangents to bridge between them
        /// </summary>
        public static Polygon Wire(Polygon polygonA, Polygon polygonB, Vector2D lowerTangent, Vector2D upperTangent)
        {
            var wiredPolygon = new List<Point2D>();

            //If UpperTangent.Start is equal to LowerTangent.Start, we need to end the loop early
            Boolean endEarly = PolygonManipulation.Equals(upperTangent.Start, lowerTangent.Start);

            //Add the start point to the wired polygon
            wiredPolygon.Add(upperTangent.Start);

            //Jump across the tangent at set the target at the current vertex
            var currentVertex = polygonB.GetVertex(upperTangent.Target);

            //Since we need to traverse each polygon in a different direction, this marks what polygon we are on
            Boolean leftPolygon = false;

            //While we aren't back at start
            do
            {
                //Add the current vertex to the wired polygon
                wiredPolygon.Add(currentVertex.Point);

                //If we have hit the lower tangent target
                if (PolygonManipulation.Equals(currentVertex.Point, lowerTangent.Target))
                {
                    //Jump across the lower tangent onto the other polygon
                    leftPolygon = true;
                    currentVertex = polygonA.GetVertex(lowerTangent.Start);
                    wiredPolygon.Add(currentVertex.Point);

                    //If we need to end early, break the loop
                    if (endEarly)
                        break;
                }

                //While we haven't find a suitable current vertex
                do
                {
                    if (leftPolygon)
                    {
                        //Traverse Clockwise
                        if (currentVertex == null || currentVertex.Point == null)
                            currentVertex = polygonA.Tail.Prev;
                        else
                            currentVertex = currentVertex.Prev;
                    }
                    else
                    {
                        //Traverse Counterclockwise
                        if (currentVertex == null || currentVertex.Point == null)
                            currentVertex = polygonB.Tail.Prev;
                        else
                            currentVertex = currentVertex.Prev;
                    }
                } while (currentVertex == null || currentVertex.Point == null);

            } while (!PolygonManipulation.Equals(currentVertex.Point, upperTangent.Start));

            wiredPolygon = wiredPolygon.Distinct().ToList();
    
            //Since the wired polygon is in clockwise order, we need to reverse it
            wiredPolygon.Reverse();
            return Polygon.Get(wiredPolygon);
        }

        public static Boolean GradientEquals(Point2D a, Point2D b, Point2D c, Point2D d)
        {
            float run1 = b.X - a.X;
            float run2 = d.X - c.X;
            float rise1 = b.Y - a.Y;
            float rise2 = d.Y - c.Y;

            if (run1 == 0)
                if (run2 == 0)
                    return true;
                else
                    return false;

            if (run2 == 0)
                return false;

            if (rise1 == 0)
                if (rise2 == 0)
                    return true;
                else
                    return false;

            if (rise2 == 0)
                return false;

            return rise1/run1 == rise2/run2;

        }

        /// <summary>
        /// Calculates the gradient between two points
        /// </summary>
        private static float CalculateGradient(Point2D a, Point2D b)
        {
            float rise = (float)b.Y - (float)a.Y;
            float run = (float)b.X - (float)a.X;
            if (run == 0)
                return 999999;
            else if (rise == 0)
                //Have to return an abnormal value here, else if m1.run = 0 and m2.rise = 0
                //Apparentally the gradients are equal
                return 888888;
            else
                return rise / run;
        }

        /// <summary>
        /// Determines is pq is the upper tangent of the polygon
        /// Pseudocode from https://facwiki.cs.byu.edu/cs312ringger/index.php/Project_2
        /// (Geometry for Common Tangents)
        /// </summary>
        public static Boolean IsUpperTangent(List<Point2D> polygon, Point2D p, Point2D q)
        {
            //Calculate the gradient taking precautions againist division by zero
            double m;
            Int32 sum = p.X - q.X;
            if (sum == 0)
                //Not double.Max because we don't want to overflow
                m = 9999999;
            else
                m = ((double)p.Y - (double)q.Y) / (double)sum;

            //Calculate the y intercept
            double b = -1 * m * (double)p.X + (double)p.Y;

            //Check if each point in the polygon is a upper tangent
            foreach (var point in polygon)
            {
                if (!PolygonManipulation.Equals(point, p) && !PolygonManipulation.Equals(point, q))
                    if ((m * (double)point.X + b - (double)point.Y) < 0)
                        return false;
            }
            return true;
        }

        /// <summary>
        /// Determines is pq is the lower tangent of the polygon
        /// Pseudocode from https://facwiki.cs.byu.edu/cs312ringger/index.php/Project_2
        /// (Geometry for Common Tangents)
        /// </summary>
        public static Boolean IsLowerTangent(List<Point2D> polygon, Point2D p, Point2D q)
        {
            //Calculate the gradient taking precautions againist division by zero
            double m;
            Int32 sum = p.X - q.X;
            if (sum == 0)
                //Not double.Max because we don't want to overflow
                m = 9999999;
            else
                m = ((double)p.Y - (double)q.Y) / (double)sum;

            //Calculate the y intercept
            double b = -1 * m * (double)p.X + (double)p.Y;

            //Check if each point in the polygon is a lower tangent
            foreach (var point in polygon)
            {
                if (!PolygonManipulation.Equals(point, p) && !PolygonManipulation.Equals(point, q))
                    if ((m * (double)point.X + b - (double)point.Y) > 0)
                        return false;
            }
            return true;
        }
    }
}