using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;

namespace computational_geometry_algorithm
{
    public class Polygon
    {
        public Vertex Root;
        public Vertex Tail;

        public Int32 Count()
        {
            var count = 0;

            var currentVertex = Root;

            while (currentVertex != null)
            {
                count++;
                currentVertex = currentVertex.Next;
            }

            return count;
        }

        public String Output()
        {
            String data = "";
            var currentVertex = Root;

            while (currentVertex != null)
            {
                if (currentVertex.Point != null)
                    data += "(" + currentVertex.Point.X + "," + currentVertex.Point.Y + ")";
                currentVertex = currentVertex.Next;
            }

            return data;
        }

        public String OutputRv()
        {
            String data = "";
            var currentVertex = Tail;

            while (currentVertex != null)
            {
                if (currentVertex.Point != null)
                    data += "(" + currentVertex.Point.X + "," + currentVertex.Point.Y + ")";
                currentVertex = currentVertex.Prev;
            }

            return data;
        }

        public Vertex GetVertex(Point2D coord)
        {
            var currentVertex = Root;

            while (currentVertex != null)
            {
                if (currentVertex.Point != null)
                if (PolygonManipulation.Equals(currentVertex.Point, coord))
                    return currentVertex;
                currentVertex = currentVertex.Next;
            }

            return null;
        }

        public List<Point2D> Convert()
        {
            var list = new List<Point2D>();

            var currentVertex = Root;

            while (currentVertex != null)
            {
                if (currentVertex.Point != null)
                list.Add(currentVertex.Point);
                currentVertex = currentVertex.Next;
            }

            return list;
        }

        public void Import(List<Point2D> data)
        {
            Root = new Vertex();
            var currentVertex = Root;

            foreach (var datum in data)
            {
                currentVertex.Point = new Point2D(datum.X, datum.Y);
                currentVertex.Next = new Vertex();
                var oldVertex = currentVertex;
                currentVertex = currentVertex.Next;
                currentVertex.Prev = oldVertex;
            }

            Tail = currentVertex;
        }

        public static Polygon Get(List<Point2D> data)
        {
            Polygon p = new Polygon();
            p.Import(data);
            return p;
        }

    }

    public class Vertex
    {
        public Point2D Point;
        public Vertex Next = null;
        public Vertex Prev = null;
    }

    public static class DCHull
    {
        private static Int32 MinimumSize = 4;
        public static Int32 XOffset = 0;
        public static Int32 YOffset = 0;

        public static Polygon Solve(Polygon points)
        {
            if (points.Count() <= MinimumSize)
            {
                return Polygon.Get(ConvexHull.Solve(points.Convert()));
            }

            var sortedPoints = PolygonManipulation.SortTopogically(points.Convert());

            //Split points into two sets
            var a = new List<Point2D>();
            var b = new List<Point2D>();

            a.AddRange(sortedPoints.Take(sortedPoints.Count()/2));
            b.AddRange(sortedPoints.Skip(sortedPoints.Count()/2).Take(sortedPoints.Count() - sortedPoints.Count()/2));

            //Compute the convex hulls of a and b recursively
            var polyA = Solve(Polygon.Get(a));
            var polyB = Solve(Polygon.Get(b));

            return Merge(polyA, polyB);
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

        private static Polygon Merge(Polygon polygonA, Polygon polygonB)
        {

            var newPolygonA = PolygonManipulation.SortTopogically(polygonA.Convert());
            var newPolygonB = PolygonManipulation.SortTopogically(polygonB.Convert());


            var leftmostPoint = GetLeftmostPoint(newPolygonB);
            var rightmostPoint = GetRightmostPoint(newPolygonA);

            //While T = ab is not lower tangent to both A and B
            while (!(IsLowerTangent(newPolygonA, rightmostPoint, leftmostPoint) && IsLowerTangent(newPolygonB, leftmostPoint,rightmostPoint)))
            {
                //While T is no a lower tangent to A
                while (!IsLowerTangent(newPolygonA, rightmostPoint,leftmostPoint))
                {
                    //Move counter clockwise
                    rightmostPoint = PolygonManipulation.GetNextPoint(newPolygonA, rightmostPoint);
                }

                //While T is not a lower tangent to B
                while (!IsLowerTangent(newPolygonB, leftmostPoint,rightmostPoint))
                {
                    //Move clockwise
                    leftmostPoint = PolygonManipulation.GetPreviousPoint(newPolygonB, leftmostPoint);
                }
            }

            Vector2D lowerTangent = new Vector2D(rightmostPoint, leftmostPoint);

            leftmostPoint = GetLeftmostPoint(newPolygonB);
            rightmostPoint = GetRightmostPoint(newPolygonA);
            Int32 yMaxA = newPolygonA.Max(p => p.Y);
            Int32 yMaxB = newPolygonB.Max(p => p.Y);

            //While T = ab is not lower tangent to both A and B
            while (!IsUpperTangent(newPolygonA, rightmostPoint, leftmostPoint) || !(IsUpperTangent(newPolygonB, leftmostPoint, rightmostPoint)))
            {
                //While T is no a lower tangent to A
                while (!IsUpperTangent(newPolygonA, rightmostPoint, leftmostPoint))
                {
                    //Move counter clockwise
                    rightmostPoint = PolygonManipulation.GetPreviousPoint(newPolygonA, rightmostPoint);
                }

                //While T is not a lower tangent to B
                while (!IsUpperTangent(newPolygonB, leftmostPoint, rightmostPoint))
                {
                    //Move clockwise
                    leftmostPoint = PolygonManipulation.GetNextPoint(newPolygonB, leftmostPoint);
                }
            }

            Vector2D upperTangent = new Vector2D(rightmostPoint, leftmostPoint);

            //Draw tangents
            /*GraphicalUserInterface.DrawPolygon(polygonA, !(polygonA.Count == 2), XOffset, YOffset, new System.Drawing.SolidBrush(System.Drawing.Color.Coral));
            GraphicalUserInterface.DrawPolygon(polygonB, polygonB.Count != 2, XOffset, YOffset);
            GraphicalUserInterface.GraphicsObject.DrawLine(new System.Drawing.Pen(System.Drawing.Color.White) { Width = 5 }, lowerTangent.Start.Convert(XOffset, YOffset, GraphicalUserInterface.SizeMultiplier), lowerTangent.Target.Convert(XOffset, YOffset, GraphicalUserInterface.SizeMultiplier));
            GraphicalUserInterface.GraphicsObject.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black) { Width = 5 }, upperTangent.Start.Convert(XOffset, YOffset, GraphicalUserInterface.SizeMultiplier), upperTangent.Target.Convert(XOffset, YOffset, GraphicalUserInterface.SizeMultiplier));

            Console.ReadKey();

            GraphicalUserInterface.Clear();*/

            //Wire together the polygons by tangent
            return Wire(polygonA, polygonB, lowerTangent, upperTangent);
        }

        //Doesnt seem to be cw
        public static Polygon Wire(Polygon polygonA, Polygon polygonB, Vector2D lowerTangent, Vector2D upperTangent)
        {
            var wiredPolygon = new List<Point2D>();

            wiredPolygon.Add(upperTangent.Start);
            var currentVertex = polygonB.GetVertex(upperTangent.Target);

            Boolean leftPolygon = false;

            do
            {
                wiredPolygon.Add(currentVertex.Point);


                if (PolygonManipulation.Equals(currentVertex.Point, lowerTangent.Target))
                {
                    leftPolygon = true;
                    currentVertex = polygonA.GetVertex(lowerTangent.Start);
                    wiredPolygon.Add(currentVertex.Point);
                }

            retry:
                if (leftPolygon)
                {
                    if (currentVertex == null || currentVertex.Point == null)
                        currentVertex = polygonA.Tail.Prev;
                    else
                        currentVertex = currentVertex.Prev;
                }
                else
                {
                    if (currentVertex == null || currentVertex.Point == null)
                        currentVertex = polygonB.Tail.Prev;
                    else
                        currentVertex = currentVertex.Prev;
                }


                if (currentVertex == null || currentVertex.Point == null)
                    goto retry;

                //While we aren't back at start
            } while (!PolygonManipulation.Equals(currentVertex.Point, upperTangent.Start));

            wiredPolygon = wiredPolygon.Distinct().ToList();
            wiredPolygon.Reverse();
            return Polygon.Get(wiredPolygon);

            /*var vertexA = polygonA.GetVertex(upperTangent.Start);
            var vertexB = polygonB.GetVertex(upperTangent.Target);
            vertexA.Next = vertexB;

            var aList = vertexB;

            //Save polygon A
            var remainingPolygonA = new Polygon();
            remainingPolygonA.Root = vertexA.Next;

            var currentNode = vertexB;
            while (!PolygonManipulation.Equals(currentNode.Point, lowerTangent.Target))
            {
                aList.Next = currentNode;
                currentNode = currentNode.Prev;
                if (currentNode == null || currentNode.Point == null)
                    currentNode = polygonB.Tail.Prev;
            }
            //Get lower Tangent target
            aList.Next = currentNode;
            aList.Next.Next = new Vertex();
            aList.Next.Next.Point = lowerTangent.Start;


            //polygonA.Tail = new Vertex();
           // polygonA.Tail.Point = lowerTangent.Target;
            //vertexA.Prev.Next = null;

            var vertexC = polygonA.GetVertex(lowerTangent.Start);
            var vertexD = remainingPolygonA.GetVertex(lowerTangent.Start);
            vertexC.Next = vertexD.Next;
            int b = 2;

            //vertexB = polygonB.GetVertex(upperTangent.Target);
            //vertexB.Next = new Vertex();
  //          vertexB.Next.Point = new Point2D(upperTangent.Start.X, upperTangent.Start.Y);

            //GraphicalUserInterface.DrawPolygon(polygonA.Convert(), true, 100, 300);


            return polygonA;
            /*List<Point2D> wiredPolygon = new List<Point2D>();

            wiredPolygon.Add(lowerTangent.Start);

            //Lower Target -> Upper Start
            var currentPoint = polygonA[polygonA.IndexOf(lowerTangent.Start)];
            while (!PolygonManipulation.Equals(currentPoint, upperTangent.Start))
            {
                wiredPolygon.Add(currentPoint);
                //Anti cw
                currentPoint = PolygonManipulation.GetPreviousPoint(polygonA, currentPoint);
            }

            wiredPolygon.Add(upperTangent.Start);
            wiredPolygon.Add(upperTangent.Target);

            Int32 indexStore = wiredPolygon.Count - 1;

            //Upper Target -> Lower Start
            currentPoint = polygonB[polygonB.IndexOf(upperTangent.Target)];
            while (!PolygonManipulation.Equals(currentPoint, lowerTangent.Target))
            {
                wiredPolygon.Insert(indexStore,currentPoint);
                //Cw
                currentPoint = PolygonManipulation.GetNextPoint(polygonB, currentPoint);
            }

            wiredPolygon.Add(lowerTangent.Target);

            return wiredPolygon;*/




            //GraphicalUserInterface.DrawPolygonByPoints(PolygonManipulation.ConvertPolygon(polygonA, GraphicalUserInterface.SizeMultiplier).ToArray());
            //GraphicalUserInterface.DrawPolygonByPoints(PolygonManipulation.ConvertPolygon(polygonB, GraphicalUserInterface.SizeMultiplier).ToArray());
            /*
            var wiredPolygon = new List<Point2D>();

            //polygonB = PolygonManipulation.SortTopogically(polygonB);

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
                //travelClockwise = !travelClockwise;

                //Check for tangent joins
                if (PolygonManipulation.Equals(currentPoint, lowerTangent.Start) && !lowerTangentUsed && started)
                {
                    //Swap polygons
                    var temp = otherPolygon;
                    otherPolygon = currentPolygon;
                    currentPolygon = temp;

                    //wiredPolygon.Add(lowerTangent.Start);
                    currentPoint = currentPolygon[PolygonManipulation.IndexOf(currentPolygon, lowerTangent.Target)];

                    lowerTangentUsed = true;
                }
                else if (PolygonManipulation.Equals(currentPoint, lowerTangent.Target) && !lowerTangentUsed && started)
                {
                    //Swap polygons
                    var temp = otherPolygon;
                    otherPolygon = currentPolygon;
                    currentPolygon = temp;

                    //wiredPolygon.Add(lowerTangent.Target);
                    currentPoint = currentPolygon[PolygonManipulation.IndexOf(currentPolygon, lowerTangent.Start)];

                    lowerTangentUsed = true;
                }
                else if (PolygonManipulation.Equals(currentPoint, upperTangent.Start) && !upperTangentUsed && started)
                {
                    //Swap polygons
                    
                    var temp = otherPolygon;
                    otherPolygon = currentPolygon;
                    currentPolygon = temp;

                    //wiredPolygon.Add(upperTangent.Start);
                    currentPoint = currentPolygon[PolygonManipulation.IndexOf(currentPolygon, upperTangent.Target)];

                    upperTangentUsed = true;
                }
                else if (PolygonManipulation.Equals(currentPoint, upperTangent.Target) && !upperTangentUsed && started)
                {
                    //Swap polygons
                    var temp = otherPolygon;
                    otherPolygon = currentPolygon;
                    currentPolygon = temp;
                     
                    //wiredPolygon.Add(upperTangent.Target);
                    currentPoint = currentPolygon[PolygonManipulation.IndexOf(currentPolygon, upperTangent.Start)];

                    upperTangentUsed = true;
                }
                else
                {

                    //travelClockwise = !travelClockwise;

                    currentPoint = PolygonManipulation.GetPreviousPoint(currentPolygon, currentPoint);
                }

                started = true;
            } while (!PolygonManipulation.Equals(currentPoint, wiredPolygon.First()));

            return wiredPolygon;*/
             
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
