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
        public bool Fill { get; set; }
        private List<Point> Points;        
        private List<Line> Edges;
        private readonly int numOfPoints;

        public Polygon(List<Point> ps, bool f, int t = 1)
        {
            Points = new List<Point>();
            Points = ps;
            numOfPoints = ps.Count;
            myColor = Color.Black;
            Thickness = t;
            Edges = new List<Line>();
            Fill = f;

            int i = 0;
            for (; i < Points.Count - 1; ++i)
            {
                Edges.Add(new Line(Points[i], Points[i + 1], Color.Black, t));
            }
            Edges.Add(new Line(Points[i], Points[0], Color.Black, t));
        }
        public Polygon(List<Point> ps, Color b, bool f, int t = 1)
        {
            Points = new List<Point>();
            Points = ps;
            numOfPoints = ps.Count;
            myColor = b;
            Thickness = t;
            Edges = new List<Line>();
            Fill = f;

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
            string res = $"Polygon{Thickness},{myColor.R},{myColor.G},{myColor.B},{Bool2Num(Fill)},:";

            foreach(var p in Points)
            {
                res += $"{p.X},{p.Y},";
            }
            StringBuilder sb = new StringBuilder(res);
            res = sb.ToString();
            res = res + ";";
            

            return res;
        }

        private int Bool2Num(bool b)
        {
            if (b) return 1;
            return 0;
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
        public Bitmap Render(Bitmap bm, bool aliasFlag) 
        {
            foreach(var line in Edges)
            {
                bm = line.Render(bm, aliasFlag);
            }
            //MessageBox.Show($"{Fill}");
            if (Fill)
            {
                
                FillPolygon(bm);
            }
            return bm;
        }

        private void FillPolygon(Bitmap bm)
        {
            var tmpPoints = new List<Point>();
            foreach (var p in Points)
            {
                tmpPoints.Add(p);
            }

            tmpPoints = tmpPoints.OrderBy(o => o.Y).ToList();
            var indices = new List<int>();
            for(int q=0;q<tmpPoints.Count;++q)
            {
                indices.Add(Points.IndexOf(tmpPoints[q]));
            }

            int k = 0;
            var i = indices[0];
            var y = Points[indices[0]].Y;
            var ymin = y;
            var ymax = Points[indices[indices.Count - 1]].Y;
            var AET = new List<List<Edge>>();
            for(int j=0; j<ymax - ymin;++j)
            {
                AET.Add(new List<Edge>());
            }
            
            while(y<ymax-1)
            {
                while(Points[i].Y == y) 
                {
                    if (Points[IndexClamp(i-1)].Y > Points[i].Y)
                    {
                        AET[y - ymin].Add(AddEdge(Points[i], Points[IndexClamp(i - 1)]));
                    }
                    if (Points[IndexClamp(i + 1)].Y > Points[i].Y)
                    {
                        AET[y - ymin].Add(AddEdge(Points[i], Points[IndexClamp(i+1)]));
                    }

                    ++k;
                    i = indices[k];
                }
                AET[y - ymin] = AET[y - ymin].OrderBy(o => o.xmin).ToList();
                //AET[y - ymin].Reverse();
                for (int g = 0; g < AET[y - ymin].Count; g = g + 2)
                {
                    for (int h = (int) AET[y - ymin][g].xmin; h < AET[y - ymin][g + 1].xmin; ++h)
                    {
                        //bm.SetPixel(h, y, myColor);
                        ColorPixel(h, y, bm);
                    }
                }                
                ++y;

                foreach (var p in AET[y - ymin - 1])
                {
                    if (p.ymax != y)
                    {
                        AET[y - ymin].Add(new Edge(p.ymax, p.xmin, p.k));
                    }
                }

                foreach (var p in AET[y - ymin])
                {
                    p.xmin = (p.xmin + p.k);
                }
            }
        }

        private Edge AddEdge(Point p1, Point p2)
        {
            //double m = (p2.Y - p1.Y) / (p2.X - p1.X);
            //return new Edge(p2.Y, p1.X, 1/m);            
            return new Edge(p2.Y, p1.X, (double)(p2.X - p1.X) / (double) (p2.Y - p1.Y));
        }
        private int IndexClamp(int i)
        {
            if (i == -1)
            {
                return Points.Count-1;
            }
            if (i == Points.Count)
            {
                return 0;
            }
            return i;
        }

        public List<Point> FindNeighbors(Point a)
        {
            var res = new List<Point>();

            foreach(var e in Edges)
            {
                if(e.P1 == a)
                {
                    res.Add(e.P2);
                }
                if (e.P2 == a)
                {
                    res.Add(e.P1);
                }
            }

            return res;
        }
    }
}
