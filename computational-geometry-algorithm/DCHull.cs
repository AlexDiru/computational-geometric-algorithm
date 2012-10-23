using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;

namespace computational_geometry_algorithm
{
    public static class DCHull
    {
        private static Int32 MinimumSize = 3;
        public static Int32 XOffset = 0;
        public static Int32 YOffset = 0;

        public static List<Point2D> Solve(List<Point2D> points)
        {
            if (points.Count() <= MinimumSize)
                return ConvexHull.Solve(points.ToList());

            var sortedPoints = PolygonManipulation.SortTopogically(points);

            //Split points into two sets
            var a = new List<Point2D>();
            var b = new List<Point2D>();

            a.AddRange(sortedPoints.Take(sortedPoints.Count()/2));
            b.AddRange(sortedPoints.Skip(sortedPoints.Count()/2).Take(sortedPoints.Count() - sortedPoints.Count()/2));

            //Compute the convex hulls of a and b recursively
            a = ConvexHull.Solve(a);
            b = ConvexHull.Solve(b);

            return Merge(a, b);
        }

        private static Point2D GetRightmostPoint(IEnumerable<Point2D> polygon)
        {
            //Get all max x points
            var xMaxPoints = polygon.Where(p => p.X == polygon.Max(q => q.X));
            return xMaxPoints.Where(p => p.Y == xMaxPoints.Min(q => q.Y)).First();
        }

        private static Point2D GetLeftmostPoint(IEnumerable<Point2D> polygon)
        {
            //Get all max x points
            var yMinPoints = polygon.Where(p => p.X == polygon.Min(q => q.X));
            return yMinPoints.Where(p => p.Y == yMinPoints.Min(q => q.Y)).First();
        }

        private static List<Point2D> Merge(List<Point2D> polygonA, List<Point2D> polygonB)
        {
            var merged = new List<Point2D>();

            //polygonA = PolygonManipulation.SortTopogically(polygonA);
            //polygonB = PolygonManipulation.SortTopogically(polygonB);

            if (polygonA.Count == 1 && polygonB.Count == 1)
            {
                merged.AddRange(polygonA);
                merged.AddRange(polygonB);
                return merged;
            }

            var leftmostPoint = GetLeftmostPoint(polygonB);
            var rightmostPoint = GetRightmostPoint(polygonA);
            var mergedPolygons = new List<Point2D>();
            mergedPolygons.AddRange(polygonA);
            mergedPolygons.AddRange(polygonB);

            var pointsToRemove = new List<Point2D>();

            //While T = ab is not lower tangent to both A and B
            while (!(IsLowerTangent(polygonA, rightmostPoint, leftmostPoint) && IsLowerTangent(polygonB, leftmostPoint,rightmostPoint)))
            {
                //While T is no a lower tangent to A
                while (!IsLowerTangent(polygonA, rightmostPoint,leftmostPoint))
                {
                    pointsToRemove.Add(rightmostPoint);
                    //Move counter clockwise
                    rightmostPoint = PolygonManipulation.GetNextPoint(polygonA, rightmostPoint);
                }

                //While T is not a lower tangent to B
                while (!IsLowerTangent(polygonB, leftmostPoint,rightmostPoint))
                {
                    pointsToRemove.Add(leftmostPoint);
                    //Move clockwise
                    leftmostPoint = PolygonManipulation.GetPreviousPoint(polygonB, leftmostPoint);
                }
            }

            Vector2D lowerTangent = new Vector2D(rightmostPoint, leftmostPoint);

            leftmostPoint = GetLeftmostPoint(polygonB);
            rightmostPoint = GetRightmostPoint(polygonA);
            Int32 yMaxA = polygonA.Max(p => p.Y);
            Int32 yMaxB = polygonB.Max(p => p.Y);

            //While T = ab is not lower tangent to both A and B
            while (!IsUpperTangent(polygonA, rightmostPoint, leftmostPoint) || !(IsUpperTangent(polygonB, leftmostPoint, rightmostPoint)))
            {
                //While T is no a lower tangent to A
                while (!IsUpperTangent(polygonA, rightmostPoint, leftmostPoint))
                {
                    pointsToRemove.Add(rightmostPoint);
                    //Move counter clockwise
                    rightmostPoint = PolygonManipulation.GetPreviousPoint(polygonA, rightmostPoint);
                }

                //While T is not a lower tangent to B
                while (!IsUpperTangent(polygonB, leftmostPoint, rightmostPoint))
                {
                    pointsToRemove.Add(leftmostPoint);
                    //Move clockwise
                    leftmostPoint = PolygonManipulation.GetNextPoint(polygonB, leftmostPoint);
                }
            }

            Vector2D upperTangent = new Vector2D(rightmostPoint, leftmostPoint);

            //Draw tangents
            GraphicalUserInterface.DrawPolygon(polygonA, true, XOffset, YOffset, new System.Drawing.SolidBrush(System.Drawing.Color.Coral));
            GraphicalUserInterface.DrawPolygon(polygonB, true, XOffset, YOffset);
            GraphicalUserInterface.GraphicsObject.DrawLine(new System.Drawing.Pen(System.Drawing.Color.White) { Width = 5 }, lowerTangent.Start.Convert(XOffset, YOffset, GraphicalUserInterface.SizeMultiplier), lowerTangent.Target.Convert(XOffset, YOffset, GraphicalUserInterface.SizeMultiplier));
            GraphicalUserInterface.GraphicsObject.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black) { Width = 5 }, upperTangent.Start.Convert(XOffset, YOffset, GraphicalUserInterface.SizeMultiplier), upperTangent.Target.Convert(XOffset, YOffset, GraphicalUserInterface.SizeMultiplier));


            //Wire together the polygons by tangent
            return Wire(polygonA, polygonB, lowerTangent, upperTangent);
        }

        private static List<Point2D> Wire(List<Point2D> polygonA, List<Point2D> polygonB, Vector2D lowerTangent, Vector2D upperTangent)
        {
            var wiredPolygon = new List<Point2D>();

            var currentPoint = polygonA.First();
            var currentPolygon = polygonA;
            var otherPolygon = polygonB;

            Boolean lowerTangentUsed = false;
            Boolean upperTangentUsed = false;
            Boolean started = false;

            //While we haven't reached the start again
            do
            {
                wiredPolygon.Add(currentPoint);

                //Check for tangent joins
                if (PolygonManipulation.Equals(currentPoint, lowerTangent.Start) && !lowerTangentUsed && started)
                {
                    //Swap polygons
                    var temp = otherPolygon;
                    otherPolygon = currentPolygon;
                    currentPolygon = temp;

                    wiredPolygon.Add(lowerTangent.Start);
                    currentPoint = currentPolygon[currentPolygon.IndexOf(lowerTangent.Target)];

                    lowerTangentUsed = true;
                }
                else if (PolygonManipulation.Equals(currentPoint, lowerTangent.Target) && !lowerTangentUsed && started)
                {
                    //Swap polygons
                    var temp = otherPolygon;
                    otherPolygon = currentPolygon;
                    currentPolygon = temp;

                    wiredPolygon.Add(lowerTangent.Target);
                    currentPoint = currentPolygon[currentPolygon.IndexOf(lowerTangent.Start)];

                    lowerTangentUsed = true;
                }
                else if (PolygonManipulation.Equals(currentPoint, upperTangent.Start) && !upperTangentUsed && started)
                {
                    //Swap polygons
                    var temp = otherPolygon;
                    otherPolygon = currentPolygon;
                    currentPolygon = temp;

                    wiredPolygon.Add(upperTangent.Start);
                    currentPoint = currentPolygon[currentPolygon.IndexOf(upperTangent.Target)];

                    upperTangentUsed = true;
                }
                else if (PolygonManipulation.Equals(currentPoint, upperTangent.Target) && !upperTangentUsed && started)
                {
                    //Swap polygons
                    var temp = otherPolygon;
                    otherPolygon = currentPolygon;
                    currentPolygon = temp;
                     
                    wiredPolygon.Add(upperTangent.Target);
                    currentPoint = currentPolygon[currentPolygon.IndexOf(upperTangent.Start)];

                    upperTangentUsed = true;
                }
                else
                {
                    currentPoint = PolygonManipulation.GetNextPoint(currentPolygon, currentPoint);
                }

                started = true;
            } while (!PolygonManipulation.Equals(currentPoint, wiredPolygon.First()));

            return wiredPolygon;
        }

        /*private static List<Point2D> Merge(List<Point2D> polygonA, List<Point2D> polygonB)
        {
            //Lower tangent
            Point2D currentPointA = GetRightmostPoint(polygonA);
            Point2D currentPointB = GetLeftmostPoint(polygonB);

            Int32 indexA = polygonA.ToList().IndexOf(currentPointA);
            Int32 indexB = polygonB.ToList().IndexOf(currentPointB);

            var pointsToRemoveA = new List<Point2D>();
            var pointsToRemoveB = new List<Point2D>();

            //Get A's lowest point
            while (currentPointA.X > polygonA.Min(p => p.X))
            {
                //Remove this point
                pointsToRemoveA.Add(polygonA[indexA]);

                //Adjust the index
                indexA--;
                if (indexA == -1)
                    indexA = polygonA.Count - 1;

                //Get new point
                currentPointA = polygonA[indexA];
            }

            //Get B's lowest point
            while (currentPointB.X > polygonB.Min(p => p.X))
            {
                //Remove this point
                pointsToRemoveB.Add(polygonB[indexB]);

                //Adjust the index
                indexB--;
                if (indexB == -1)
                    indexB = polygonB.Count - 1;

                //Get new point
                currentPointB = polygonB[indexB];
            }

            //Upper tangent
            currentPointA = GetRightmostPoint(polygonA);
            currentPointB = GetLeftmostPoint(polygonB);

            indexA = polygonA.ToList().IndexOf(currentPointA);
            indexB = polygonB.ToList().IndexOf(currentPointB);

            //Get A's lowest point
            while (currentPointA.X < polygonA.Max(p => p.X))
            {
                //Remove this point
                pointsToRemoveA.Add(polygonA[indexA]);

                //Adjust the index
                indexA++;
                if (indexA == polygonA.Count)
                    indexA = 0;

                //Get new point
                currentPointA = polygonA[indexA];
            }

            //Get B's lowest point
            while (currentPointB.X < polygonB.Max(p => p.X))
            {
                //Remove this point
                pointsToRemoveB.Add(polygonB[indexB]);

                //Adjust the index
                indexB++;
                if (indexB == polygonB.Count)
                    indexB = 0;

                //Get new point
                currentPointB = polygonB[indexB];
            }

            
            //Delete any points between the tangents
            for (int i = 0; i < polygonA.Count; i++)
            {
                if (pointsToRemoveA.Contains(polygonA[i]))
                    polygonA.RemoveAt(i--);
            } 
            
            for (int i = 0; i < polygonB.Count; i++)
            {
                if (pointsToRemoveB.Contains(polygonB[i]))
                    polygonB.RemoveAt(i--);
            }

            //Merge A and B
            var mergedPolygons = new List<Point2D>();
            mergedPolygons.AddRange(polygonA);
            mergedPolygons.AddRange(polygonB);

            return mergedPolygons;
        }*/

        public static Boolean IsUpperTangent(List<Point2D> polygon, Point2D p, Point2D q)
        {
            double m;
            Int32 sum = p.X - q.X;
            if (sum == 0)
                m = 9999999;
            else
                m = ((double)p.Y - (double)q.Y) / (double)sum;
            double b = -1 * m * (double)p.X + (double)p.Y;

            foreach (var point in polygon)
            {
                if (!PolygonManipulation.Equals(point, p) && !PolygonManipulation.Equals(point, q))
                    if ((m * (double)point.X + b - (double)point.Y) < 0)
                        return false;
            }
            return true;
        }

        public static Boolean IsLowerTangent(List<Point2D> polygon, Point2D p, Point2D q)
        {
            double m;
            Int32 sum = p.X - q.X;
            if (sum == 0)
                m = 9999999;
            else
                m = ((double)p.Y - (double)q.Y) / (double)sum;
            double b = -1 * m * (double)p.X + (double)p.Y;

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
