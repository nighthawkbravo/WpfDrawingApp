using System;
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
        public int Thickness { get; set; }
        public Color myColor { get; set; }
        private List<Point> Points;
        private List<Line> Edges;
        private readonly int numOfPoints;

        public Polygon(List<Point> ps, int t = 1)
        {
            Points = new List<Point>();
            Points = ps;
            numOfPoints = ps.Count;
            myColor = Color.Black;
            Thickness = t;
            Edges = new List<Line>();

            int i = 0;
            for (; i < Points.Count - 1; ++i)
            {
                Edges.Add(new Line(Points[i], Points[i + 1], Color.Black, t));
            }
            Edges.Add(new Line(Points[i], Points[0], Color.Black, t));
        }
        public Polygon(List<Point> ps, Color b, int t = 1)
        {
            Points = new List<Point>();
            Points = ps;
            numOfPoints = ps.Count;
            myColor = b;
            Thickness = t;
            Edges = new List<Line>();

            int i = 0;
            for (; i < Points.Count - 1; ++i)
            {
                Edges.Add(new Line(Points[i], Points[i + 1], b, t));
            }
            Edges.Add(new Line(Points[i], Points[0], b, t));
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
        public List<Point> GetPoints()
        {
            return Points;
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
        private double CalcEucliDist(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.Y - p1.Y, 2) + Math.Pow(p2.X - p1.X, 2));
        }
        public Point FindClosestPoint(Point a)
        {
            Point res = a;
            double sum = double.PositiveInfinity;

            foreach (var p in Points)
            {
                var v = CalcEucliDist(p, a);
                if (v < sum)
                {
                    sum = v;
                    res = p;
                }
            }

            return res;
        }
        public Line FindClosestEdge(Point a)
        {
            Line res = null;
            double sum = double.PositiveInfinity;

            foreach (var e in Edges)
            {
                var v = CalcEucliDist(e.GetCenter(), a);
                if (v < sum)
                {
                    sum = v;
                    res = e;
                }
            }

            return res;
        }
        public void ColorPixel(int x, int y, Bitmap bm)
        {
            if (!IsInBound(x, y, bm)) return;
            bm.SetPixel(x, y, myColor);


            if (Thickness > 1 && Thickness % 2 != 0)
            {
                int r = Thickness - 3;
                int N = Thickness;

                for (int i = x - r; i < x + r; ++i)
                {
                    for (int j = y - r; j < y + r; ++j)
                    {
                        int X = i - x;
                        int Y = j - y;

                        if (X * X + Y * Y < r * r)
                        {
                            if (!IsInBound(x, y, bm)) continue;
                            bm.SetPixel(i, j, myColor);
                        }
                    }
                }
            }
        }
        public Bitmap Render(Bitmap bm) 
        {
            foreach(var line in Edges)
            {
                bm = line.Render(bm);
            }
            return bm;
        }
    }
}
