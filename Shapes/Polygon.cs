﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using Bitmap = System.Drawing.Bitmap;

namespace WpfAppComputerGraphics2.Shapes
{
    public class Polygon : IShape
    {
        public Color myColor { get; set; }
        private List<Point> Points;
        private readonly int numOfPoints;


        public Polygon(List<Point> ps)
        {
            Points = ps;
            numOfPoints = ps.Count;
            myColor = Color.Black;
        }
        public Polygon(int n, List<Point> ps)
        {
            Points = ps;
            numOfPoints = n;
            myColor = Color.Black;
        }
        public Point GetCenter()
        {
            int sumX = 0;
            int sumY = 0;

            foreach (var p in Points)
            {
                sumX += p.X;
                sumY += p.Y;
            }

            return new Point(sumX / numOfPoints, sumY / numOfPoints);
        }
        public string Save()
        {
            string res = $"Polygon({numOfPoints}): ";

            foreach(var p in Points)
            {
                res += $"({p.X},{p.Y})";
            }

            return res;
        }
        public string GetNameAndCenter()
        {
            return $"Polygon ({GetCenter().X},{GetCenter().Y})";
        }
        public int Lower(int a, int b)
        {
            if (a < b) return a;
            else return b;
        }
        public int Greater(int a, int b)
        {
            if (a > b) return a;
            else return b;
        }
        public bool IsInBound(int x, int y, Bitmap bm)
        {
            if (x > 0 && x < bm.Width && y > 0 && y < bm.Height)
            {
                return true;
            }
            return false;
        }
        public Bitmap Render(Bitmap bm) { return bm; }
    }
}
