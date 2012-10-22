﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace computational_geometry_algorithm
{
    /// <summary>
    /// Represents a 2D coordinate
    /// </summary>
    public class Point2D
    {
        public Int32 X, Y;
        public Point2D() { }
        public Point2D(Int32 x, Int32 y)
        {
            X = x;
            Y = y;
        }

        //Converts a Point2D into System.Drawing.Point
        public Point Convert()
        {
            return new Point(X*10, Y*10);
        }
    }
}
