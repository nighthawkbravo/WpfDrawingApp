using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppComputerGraphics2.Shapes
{
    class Rectangle : IShape
    {
        public Color myColor { get; set; }
        public bool Fill { get; set; }
        public int Thickness { set; get; }
        public Point P1 { set; get; }
        public Point P2 { set; get; }

        private Polygon poly;

        public Rectangle(Point p1, Point p2, Color b, bool f, int t = 1)
        {
            P1 = p1;
            P2 = p2;

            List<Point> Points = new List<Point>();
            Points.Add(P1);

            int xdif = -1 * (P1.X - P2.X);
            int ydif = -1 * (P1.Y - P2.Y);

            var P3 = new Point(P1.X+xdif, P1.Y); //var P3 = new Point(P1.X+xdif, P1.Y + ydif);
            Points.Add(P3);
            Points.Add(P2);
            var P4 = new Point(P2.X - xdif, P2.Y); //var P4 = new Point(P2.X - xdif, P2.Y - ydif);
            Points.Add(P4);

            poly = new Polygon(Points, b, f, t);

            Thickness = t;
            myColor = b;
            Fill = f;
        }
        public Rectangle(List<Point> points, Color b, bool f, int t = 1)
        {
            P1 = points[0];
            P2 = points[2];

            List<Point> Points = points;            

            poly = new Polygon(Points, b, f, t);

            Thickness = t;
            myColor = b;
            Fill = f;
        }

        public void ColorPixel(int x, int y, Bitmap bm)
        {
            
        }
        public Point GetCenter()
        {
            return new Point((P1.X + P2.X) / 2, (P1.Y + P2.Y) / 2);
        }
        public string GetNameAndCenter()
        {
            return $"Rect ({GetCenter().X},{GetCenter().Y})";
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
            Bitmap b = poly.Render(bm, aliasFlag);

            var p1 = TopLeft();
            var p2 = BottomRight();

            if (Fill)
            {
                for (int i = p1.X; i < p2.X; ++i)
                {
                    for (int j = p2.Y; j < p1.Y; ++j)
                    {
                        b.SetPixel(i, j, myColor);
                    }
                }
            }




            return b;
        }

        private Point TopLeft()
        {
            var tl = new Point(int.MaxValue, 0);
            foreach(var p in GetPoints())
            {
                if(p.X<tl.X && p.Y > tl.Y)
                {
                    tl = p;
                }
            }
            return tl;
        }
        private Point BottomRight()
        {
            var br = new Point(0, int.MaxValue);
            foreach (var p in GetPoints())
            {
                if (p.X > br.X && p.Y < br.Y)
                {
                    br = p;
                }
            }
            return br;
        }
        public string Save()
        {
            //return $"Rect{Thickness},{myColor.R},{myColor.G},{myColor.B},:{P1.X},{P1.Y},{P2.X},{P2.Y},;";
            return $"Rect{Thickness},{myColor.R},{myColor.G},{myColor.B},{Bool2Num(Fill)},:{P1.X},{P1.Y},{P2.X},{P2.Y},;";
        }

        private int Bool2Num(bool b)
        {
            if (b) return 1;
            return 0;
        }

        public List<Point> GetPoints()
        {
            return poly.GetPoints();
        }
        public Point FindClosestPoint(Point a)
        {
            return poly.FindClosestPoint(a);
        }
        public Line FindClosestEdge(Point a)
        {
            return poly.FindClosestEdge(a);
        }
        public List<Point> FindNeighbors(Point a)
        {
            return poly.FindNeighbors(a);
        }
    }
}
