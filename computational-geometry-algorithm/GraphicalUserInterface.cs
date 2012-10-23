using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace computational_geometry_algorithm
{
    public partial class GraphicalUserInterface : Form
    {
        //The graphics object
        public static Graphics GraphicsObject;

        //Brushes used to fill graphics
        private static SolidBrush StartBrush = new SolidBrush(System.Drawing.Color.Black);
        private static SolidBrush MidBrush = new SolidBrush(System.Drawing.Color.BlueViolet);
        private static SolidBrush EndBrush = new SolidBrush(System.Drawing.Color.DarkOrange);
        private static SolidBrush SolidColourBrush = new SolidBrush(System.Drawing.Color.LightGreen);
        private static SolidBrush VertexBrush = new SolidBrush(System.Drawing.Color.DarkMagenta);
        public static SolidBrush TextBrush = new SolidBrush(System.Drawing.Color.Black);

        //Pen used to draw the path
        private static Pen PathPen = new Pen(System.Drawing.Color.DarkSalmon) { Width = 5 };

        //Size of the path nodes (pixels)
        private static Int32 PathNodeSize = 8;

        //The color used to fill the background when the graphics are cleared
        private static Color BackgroundColour = DefaultBackColor;

        //The amount of pixels each Point2D coordinate represents
        public static Int32 SizeMultiplier = 20;

        //Parameters for testing, set to default and if possible grabbed from form controls using GetParameters()
        private static Int32 XSize;
        private static Int32 YSize;
        private static Int32 NumPoints;
        private static Int32 NumPolygons;

        //Number of pixel offset from the left hand side of the window
        private static Int32 GraphicalXOffset = 30;
        private static Int32 GraphicalYOffset = 30;

        //Text for debug data
        private static String DebugText = String.Empty;

        /// <summary>
        /// Initialises the form and the graphics
        /// </summary>
        public GraphicalUserInterface()
        {
            InitializeComponent();
            GraphicsObject = CreateGraphics();
        }

        /// <summary>
        /// Attempts the grab parameters from the form controls otherwise the parameters are set to default values
        /// </summary>
        private void GetParameters()
        {
            //Set default parameters
            XSize = 10;
            YSize = 10;
            NumPoints = 20;
            NumPolygons = 2;

            //Check for any manual parameters
            try { XSize = Convert.ToInt32(xSizeTextBox.Text); }
            catch { }
            try { YSize = Convert.ToInt32(ySizeTextBox.Text); }
            catch { }
            try { NumPoints = Convert.ToInt32(numberOfPointsTextBox.Text); }
            catch { }
            try { NumPolygons = Convert.ToInt32(numberOfPolygonsTextBox.Text); }
            catch { }
        }

        /// <summary>
        /// Draws a polygon to the screen
        /// Multiples all the points by a factor of SizeMultiplier to space them out, thus this
        /// must be taken into account for the offset
        /// </summary>
        public static void DrawPolygon(List<Point2D> polygon, bool fillPolygon, Int32 offsetX = 0, Int32 offsetY = 0, SolidBrush brush = null)
        {
            //Organise the polygon in clockwise order so the points are next to each other
            var newPolygon = PolygonManipulation.ConvertPolygon(polygon, SizeMultiplier).ToArray();

            //Offset the polygon
            for (int i = 0; i < newPolygon.Count(); i++)
            {
                newPolygon[i].X += offsetX;
                newPolygon[i].Y += offsetY;
            }

            //Fill the polygon or draw it by points
            if (fillPolygon)
                if (brush != null)
                    GraphicsObject.FillPolygon(brush, newPolygon, FillMode.Alternate);
                else
                    GraphicsObject.FillPolygon(SolidColourBrush, newPolygon, FillMode.Alternate);
            else
                DrawPolygonByPoints(newPolygon);
        }

        /// <summary>
        /// Given a list of points, draws them as small rectangles
        /// </summary>
        public static void DrawPolygonByPoints(Point[] polygon, Int32 offsetX = 0, Int32 offsetY = 0)
        {
            //Each point of the polygon is represented by a rectangle
            List<Rectangle> points = new List<Rectangle>();

            //Convert each point of the polygon to a rectangle
            foreach (var point in polygon)
            {
                points.Add(new Rectangle(point.X - 2 + offsetX, point.Y - 2 + offsetY, 4, 4));
            }

            //Fill each rectangle to make them solid
            GraphicsObject.FillRectangles(VertexBrush, points.ToArray());
        }

        /// <summary>
        /// Clears any graphics which have already been drawn on the window and any debug text
        /// </summary>
        private void Clear()
        {
            GraphicsObject.Clear(BackgroundColour);
            DebugText = String.Empty;
        }

        /// <summary>
        /// Writes the debug text to the textbox
        /// </summary>
        private void DrawDebugText()
        {
            debugTextBox.Text = DebugText;
        }

        /// <summary>
        /// Called when the testConvexHullButton is clicked
        /// </summary>
        private void TestConvexHullButtonClick(object sender, EventArgs e)
        {
            Clear();
            GetParameters();

            var polygon = ProceduralGeneration.GenerateRandomPolygon(NumPoints, XSize, YSize);
            DebugText += "Polygon generated: " + PolygonManipulation.Output(polygon) + "\r\n";

            //Draw the polygon at the top
            DrawPolygon(polygon, false, GraphicalXOffset, GraphicalYOffset);    

            //Draw the convex hull below
            Int32 xOffset = Convert.ToInt32(XSize * SizeMultiplier * 1.5) + GraphicalXOffset;
            List<Point2D> convexHull = ConvexHull.Solve(polygon, true, GraphicalYOffset + (Convert.ToInt32(YSize * SizeMultiplier * 1.5)), GraphicalXOffset, xOffset);
            DrawPolygon(convexHull, true, xOffset, GraphicalYOffset) ;

            DebugText += "Convex Hull: " + PolygonManipulation.Output(convexHull) + "\r\n";

            DrawDebugText();
        }

        private void TestSingleObstacleAvoidanceClick(object sender, EventArgs e)
        {
            Clear();
            GetParameters();

            var map = ProceduralGeneration.GenerateSingleObstacleMap(NumPoints, XSize, YSize);


            //Draw Polygon
            DrawPolygon(ConvexHull.Solve(map.Polygons.First()),true, GraphicalXOffset, GraphicalYOffset);
            DrawPolygonByPoints(PolygonManipulation.ConvertPolygon(map.Polygons.First(), SizeMultiplier).ToArray(), GraphicalXOffset, GraphicalYOffset);

            var path = PolygonManipulation.ConvertPolygon(map.SolveMap(), SizeMultiplier, GraphicalXOffset, GraphicalYOffset);

            //Draw a line for the path
            for (int i = 0; i < path.Count - 1; i++)
            {
                GraphicsObject.DrawLine(PathPen, path[i], path[i + 1]);
            }

            //Draw start and end points
            GraphicsObject.FillRectangle(StartBrush, new Rectangle(map.Start.X * SizeMultiplier - PathNodeSize / 2 + GraphicalXOffset,
                                                                 map.Start.Y * SizeMultiplier - PathNodeSize / 2 + GraphicalYOffset,
                                                                 PathNodeSize,
                                                                 PathNodeSize));
            GraphicsObject.FillRectangle(EndBrush, new Rectangle(map.End.X * SizeMultiplier - PathNodeSize / 2 + GraphicalXOffset,
                                                               map.End.Y * SizeMultiplier - PathNodeSize / 2 + GraphicalYOffset,
                                                               PathNodeSize,
                                                               PathNodeSize));

            DebugText += map.GetDebugText();
            DebugText += "Path: " + PolygonManipulation.Output(path) + "\n";
            DrawDebugText();
        }

        /// <summary>
        /// Called when the testMultipleObstacleAvoidance button is pressed
        /// </summary>
        private void TestMultipleObstacleAvoidanceClick(object sender, EventArgs e)
        {
            Clear();
            GetParameters();

            var map = ProceduralGeneration.GenerateMultipleObstacleMap(NumPolygons, NumPoints, XSize, YSize);

            //Draw Polygons
            foreach (var polygon in map.Polygons)
            {
                var convexPolygon = ConvexHull.Solve(polygon);
                DrawPolygon(convexPolygon, true, GraphicalXOffset, GraphicalYOffset);
                DrawPolygonByPoints(PolygonManipulation.ConvertPolygon(polygon, SizeMultiplier, GraphicalXOffset, GraphicalYOffset).ToArray());
            }

            var path = PolygonManipulation.ConvertPolygon(map.SolveMap(), SizeMultiplier, GraphicalXOffset, GraphicalYOffset);


            //Draw a line for the path
            for (int i = 0; i < path.Count - 1; i++)
                GraphicsObject.DrawLine(PathPen, path[i], path[i + 1]);

            //Draw start middle and end points
            GraphicsObject.FillRectangle(StartBrush, new Rectangle(map.Start.X * SizeMultiplier - PathNodeSize / 2 + GraphicalXOffset,
                                                                 map.Start.Y * SizeMultiplier - PathNodeSize / 2 + GraphicalYOffset,
                                                                 PathNodeSize,
                                                                 PathNodeSize));
            GraphicsObject.FillRectangle(MidBrush, new Rectangle(map.Mid.X * SizeMultiplier - PathNodeSize / 2 + GraphicalXOffset,
                                                               map.Mid.Y * SizeMultiplier - PathNodeSize / 2 + GraphicalYOffset,
                                                               PathNodeSize,
                                                               PathNodeSize));
            GraphicsObject.FillRectangle(EndBrush, new Rectangle(map.End.X * SizeMultiplier - PathNodeSize / 2 + GraphicalXOffset,
                                                               map.End.Y * SizeMultiplier - PathNodeSize / 2 + GraphicalYOffset,
                                                               PathNodeSize,
                                                               PathNodeSize));

            DebugText += map.GetDebugText();
            DebugText += "Path: " + PolygonManipulation.Output(path) + "\n";
            DrawDebugText();
        }

        private void testDCHull_Click(object sender, EventArgs e)
        {
            Clear();
            GetParameters();

            var polygon = ProceduralGeneration.GenerateRandomPolygon(NumPoints, XSize, YSize);
            DebugText += "Polygon generated: " + PolygonManipulation.Output(polygon) + "\r\n";

            //Draw the polygon at the top
            DrawPolygon(polygon, false, GraphicalXOffset, GraphicalYOffset);

            //Draw the convex hull below
            Int32 xOffset = Convert.ToInt32(XSize * SizeMultiplier * 1.5) + GraphicalXOffset;
            DCHull.XOffset = xOffset;
            DCHull.YOffset =  GraphicalYOffset;
            List<Point2D> convexHull = DCHull.Solve(polygon);
            //DrawPolygon(convexHull, true, xOffset, GraphicalYOffset);
            DrawPolygonByPoints(PolygonManipulation.ConvertPolygon(convexHull,SizeMultiplier).ToArray(), xOffset, GraphicalYOffset);

            DebugText += "Convex Hull: " + PolygonManipulation.Output(convexHull) + "\r\n";

            DrawDebugText();
        }
    }
}