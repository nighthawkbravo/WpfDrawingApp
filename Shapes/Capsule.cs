using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using Bitmap = System.Drawing.Bitmap;
using System.Windows;

namespace WpfAppComputerGraphics2.Shapes
{
    public class Capsule : IShape
    {
        public Color myColor { get; set; }
        public bool Fill { get; set; }
        public Point Cp1 { set; get; }
        public Point Cp2 { set; get; }
        public Point EdgePoint { set; get; }

        private List<Line> sides;        

        public Capsule(Point p1, Point p2, Point ep)
        {
            Cp1 = p1;
            Cp2 = p2;
            EdgePoint = ep;
            myColor = Color.Black;
            sides = new List<Line>();
        }
        public Capsule(Point p1, Point p2, Point ep, Color b)
        {
            Cp1 = p1;
            Cp2 = p2;
            EdgePoint = ep;
            myColor = b;
            sides = new List<Line>();
        }
        public string GetNameAndCenter()
        {
            return $"Capsule ({GetCenter().X},{GetCenter().Y})";
        }
        public Point GetCenter()
        {
            return new Point((Cp1.X + Cp2.X) / 2, (Cp1.Y + Cp2.Y) / 2);
        }
        public string Save()
        {
            return $"Capsule{myColor.R},{myColor.G},{myColor.B},:{Cp1.X},{Cp1.Y},{Cp2.X},{Cp2.Y},{EdgePoint.X},{EdgePoint.Y},;";
        }
        public void ColorPixel(int x, int y, Bitmap bm)
        {
            if (!IsInBound(x, y, bm)) return;
            bm.SetPixel(x, y, myColor);
        }
        public bool IsInBound(int x, int y, Bitmap bm)
        {
            if (x > 0 && x < bm.Width && y > 0 && y < bm.Height)
            {
                return true;
            }
            return false;
        }
        public Bitmap Render(Bitmap bm, bool aliasFlag)
        {
            var p1 = FindClosestPoint(EdgePoint);
            int r = (int) CalcEucliDist(p1, EdgePoint);
            var p2 = GetOtherPoint(p1);

            Vector center2CenterVector = System.Windows.Point.Subtract(ToSystemPoint(p1), ToSystemPoint(p2));
            Vector tmp = Vector.Divide(center2CenterVector, CalcEucliDist(Cp1, Cp2));
            Vector V = Vector.Multiply(tmp, r);

            Vector v1 = new Vector(-V.Y, -V.X);
            Vector v2 = new Vector(V.Y, V.X);

            var p1_1 = System.Windows.Point.Subtract(ToSystemPoint(p1), v1);
            var p1_2 = System.Windows.Point.Subtract(ToSystemPoint(p2), v1);

            var p2_1 = System.Windows.Point.Subtract(ToSystemPoint(p1), v2);
            var p2_2 = System.Windows.Point.Subtract(ToSystemPoint(p2), v2);

            sides.Add(new Line(ToDrawingPoint(p1_1), ToDrawingPoint(p1_2), myColor));
            sides.Add(new Line(ToDrawingPoint(p2_1), ToDrawingPoint(p2_2), myColor));


            foreach(var s in sides)
            {
                s.Render(bm, aliasFlag);
            }

            bm = midPointCircleDraw(Cp1.X, Cp1.Y, r, bm);
            bm = midPointCircleDraw(Cp2.X, Cp2.Y, r, bm);
            return bm;
        }

        private System.Windows.Point ToSystemPoint(Point p)
        {
            return new System.Windows.Point(p.X, p.Y);
        }
        private Point ToDrawingPoint(System.Windows.Point p)
        {
            return new Point((int)p.X, (int) p.Y);
        }
        public Point GetOtherPoint(Point a)
        {
            if (a == Cp1) return Cp2;
            else return Cp1;
        }
        private double CalcEucliDist(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.Y - p1.Y, 2) + Math.Pow(p2.X - p1.X, 2));
        }
        public Point FindClosestPoint(Point a)
        {
            Point res = a;
            double sum = double.PositiveInfinity;

            List<Point> Points = new List<Point>();
            Points.Add(Cp1);
            Points.Add(Cp2);

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
        private Bitmap midPointCircleDraw(int x_centre, int y_centre, int r, Bitmap bm)
        {
            int x = r, y = 0;

            if (r > 0)
            {
                ColorPixel(x + x_centre, -y + y_centre, bm);
                ColorPixel(y + x_centre, x + y_centre, bm);
                ColorPixel(-y + x_centre, x + y_centre, bm);
            }

            int P = 1 - r;
            while (x > y)
            {
                y++;
                if (P <= 0) P = P + 2 * y + 1;
                else
                {
                    x--;
                    P = P + 2 * y - 2 * x + 1;
                }

                if (x < y) break; 
                ColorPixel(x + x_centre, y + y_centre, bm);
                ColorPixel(-x + x_centre, y + y_centre, bm);
                ColorPixel(x + x_centre, -y + y_centre, bm);
                ColorPixel(-x + x_centre, -y + y_centre, bm);

                if (x != y)
                {
                    ColorPixel(y + x_centre, x + y_centre, bm);
                    ColorPixel(-y + x_centre, x + y_centre, bm);
                    ColorPixel(y + x_centre, -x + y_centre, bm);
                    ColorPixel(-y + x_centre, -x + y_centre, bm);
                }
            }
            return bm;
        }
        private int check(Point c, Point ep, Point t)
        {
            var v = (ep.X - c.X) * (t.Y - c.Y) - (ep.Y - c.Y) * (t.X - c.X);

            if (v < 0) return -1;
            if (v == 0) return 0;
            else return 1;
        }
    }
}
