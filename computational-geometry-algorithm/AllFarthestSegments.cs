using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace computational_geometry_algorithm
{
    static class AllFarthestSegments
    {
        public static  void Solve(List<Point2D> points)
        {
            //Compute the convex hull
            var convexHull = ConvexHull.Solve(points);

            //For each point in the point set
            foreach (var point in points)
            {
                var segment = FindFarthestSegment(convexHull, point);
                GraphicalUserInterface.GraphicsObject.DrawLine(new System.Drawing.Pen(GraphicalUserInterface.TextBrush), point.Convert(0,0,GraphicalUserInterface.SizeMultiplier), segment.Value.To2DI().Convert(0,0,GraphicalUserInterface.SizeMultiplier));
            }
        }

        private static KeyValuePair<KeyValuePair<Point2D, Point2D>, Point2DF> FindFarthestSegment(List<Point2D> convexHull, Point2D currentPoint)
        {
            //Get all the segments which don't contain the current point
            var segments = IterateSegments(convexHull).Where(s => !PolygonManipulation.Equals(s.Value, currentPoint) && !PolygonManipulation.Equals(s.Key, currentPoint));
            float maximumDistance = float.MinValue;
            KeyValuePair<KeyValuePair<Point2D, Point2D>, Point2DF> farthestSegment = new KeyValuePair<KeyValuePair<Point2D, Point2D>, Point2DF>();

            foreach (var segment in segments)
            {
                var data = GetMinimumDistanceBetweenSegmentAndPoint(segment, currentPoint);
                float newDistance = data.Key;
                var furthestPoint = data.Value;

                if (newDistance > maximumDistance)
                {
                    maximumDistance = newDistance;
                    farthestSegment = new KeyValuePair<KeyValuePair<Point2D, Point2D>, Point2DF>(new KeyValuePair<Point2D, Point2D>(segment.Key, segment.Value), furthestPoint);
                }
            }

            return farthestSegment;
        }

        /// <summary>
        /// Calculates the minimum distance between a segment and point
        /// http://stackoverflow.com/questions/849211/shortest-distance-between-a-point-and-a-line-segment
        /// </summary>
        private static KeyValuePair<float, Point2DF> GetMinimumDistanceBetweenSegmentAndPoint(KeyValuePair<Point2D, Point2D> segment, Point2D point)
        {
            //Get the segment length
            var segmentLengthSquared = ConvexHull.GetDistanceSquared(segment.Key, segment.Value);

            if (segmentLengthSquared == 0)
                return new KeyValuePair<float, Point2DF>(ConvexHull.GetDistance(point, segment.Key), segment.Key.To2DF());

            //Find the project of point p on the line
            //It falls where t = ((p.
            var t = ((point.X - segment.Key.X) * (segment.Value.X - segment.Key.X) + (point.Y - segment.Key.Y) * (segment.Value.Y - segment.Key.Y)) / segmentLengthSquared;

            if (t < 0)
                return new KeyValuePair<float, Point2DF>(ConvexHull.GetDistance(point, segment.Key), segment.Key.To2DF());
            else if (t > 1)
                return new KeyValuePair<float, Point2DF>(ConvexHull.GetDistance(point, segment.Value), segment.Value.To2DF());

            var p = new Point2DF((float)segment.Key.X + (float)t * ((float)segment.Value.X - (float)segment.Key.X),
                                 (float)segment.Key.Y + (float)t * ((float)segment.Value.Y - (float)segment.Key.Y));
            
            return new KeyValuePair<float, Point2DF>(ConvexHull.GetDistance(point, p), p);
        }

        /// <summary>
        /// Gets all segments in the convex hull
        /// </summary>
        private static List<KeyValuePair<Point2D, Point2D>> IterateSegments(List<Point2D> convexHull)
        {
            var segments = new List<KeyValuePair<Point2D, Point2D>>();

            foreach (var startPoint in convexHull)
                foreach (var endPoint in convexHull)
                    //If the points are not equals
                    if (!PolygonManipulation.Equals(startPoint, endPoint))
                        //Make sure the segment isn't already in the list (points the other way around)
                        if (!segments.Contains(new KeyValuePair<Point2D, Point2D>(endPoint, startPoint)))
                            segments.Add(new KeyValuePair<Point2D, Point2D>(startPoint, endPoint));

            return segments;
        }
    }
}
