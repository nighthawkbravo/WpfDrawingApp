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
    public class Circle : IShape
    {
        public Color myColor { get; set; }
        public Point Center { set; get; }
        public Point EdgePoint { set; get; }

        public Circle(Point c, Point e)
        {
            Center = c;
            EdgePoint = e;
            myColor = Color.Black;
        }
        public Circle(Point c, Point e, Color b)
        {
            Center = c;
            EdgePoint = e;
            myColor = b;
        }
        public Point GetCenter()
        {
            return Center;
        }
        public string Save()
        {
            return $"Circle{myColor.R},{myColor.G},{myColor.B},:{Center.X},{Center.Y},{EdgePoint.X},{EdgePoint.Y},;";
        }
        public string GetNameAndCenter()
        {
            return $"Circle ({GetCenter().X},{GetCenter().Y})";
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
        public Bitmap Render(Bitmap bm, bool aliasFlag)
        {
            int r = (int)CalcEucliDist(Center, EdgePoint);

            if(aliasFlag)
            {
                WuCircle(Center.X, Center.Y, r, bm);
            }
            else
            {
                bm = midPointCircleDraw(Center.X, Center.Y, r, bm);
            }

            return bm;
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
                ColorPixel(x + x_centre, y + y_centre, bm); // Top right
                ColorPixel(-x + x_centre, y + y_centre, bm); // Top left
                ColorPixel(x + x_centre, -y + y_centre, bm); // Bottom right
                ColorPixel(-x + x_centre, -y + y_centre, bm); // Bottom left

                if (x != y)
                {
                    ColorPixel(y + x_centre, x + y_centre, bm); // Top right
                    ColorPixel(-y + x_centre, x + y_centre, bm); // Top left
                    ColorPixel(y + x_centre, -x + y_centre, bm); // Bottom right
                    ColorPixel(-y + x_centre, -x + y_centre, bm); // Bottom left
                }
            }
            return bm;
        }
        public void ColorPixel(int x, int y, Bitmap bm)
        {
            if (!IsInBound(x, y, bm)) return;
            bm.SetPixel(x, y, myColor);
        }

        public void ColorPixel(int x, int y, Color c, Bitmap bm)
        {
            if (!IsInBound(x, y, bm)) return;
            bm.SetPixel(x, y, c);
        }

        private double D(int r, int y)
        {
            return Math.Ceiling(Math.Sqrt(r * r - y * y)) - Math.Sqrt(r * r - y * y);
        }

        private void WuCircle(int x_centre, int y_centre, int r, Bitmap bm) 
        { 
            Color L = myColor; /*Line color*/
            Color B = Color.FromArgb(255,0,0,0); /*Background Color*/
            int x = r; 
            int y = 0;
            ColorPixel(x, y, L, bm); 
            while (x > y) 
            { 
                ++y; 
                x = (int) Math.Ceiling(Math.Sqrt(r * r - y * y)); 
                
                double T = D(r, y);

                int r1 = (int)(L.R * (1 - T) + B.R * T);
                int g1 = (int)(L.G * (1 - T) + B.G * T);
                int b1 = (int)(L.B * (1 - T) + B.B * T);
                Color c2 = Color.FromArgb(255, r1, g1, b1);

                int r2 = (int)(L.R * T + B.R * (1 - T));
                int g2 = (int)(L.G * T + B.G * (1 - T));
                int b2 = (int)(L.B * T + B.B * (1 - T));
                Color c1 = Color.FromArgb(255, r2, g2, b2);

                ColorPixel(x + x_centre, y + y_centre, c2, bm);
                ColorPixel(x-1 + x_centre, y + y_centre, c1, bm);
                ColorPixel(-x + x_centre, y + y_centre, c2, bm);
                ColorPixel(-(x-1) + x_centre, y + y_centre, c1, bm);
                ColorPixel(x + x_centre, -y + y_centre, c2, bm);
                ColorPixel((x-1) + x_centre, -y + y_centre, c1, bm);
                ColorPixel(-x + x_centre, -y + y_centre, c2, bm);
                ColorPixel(-(x-1) + x_centre, -y + y_centre, c1, bm);

                if (x != y)
                {
                    ColorPixel(y + x_centre, x + y_centre, c2, bm);
                    ColorPixel(y-1 + x_centre, x + y_centre, c1, bm);
                    ColorPixel(-y + x_centre, x + y_centre, c2, bm);
                    ColorPixel(-(y-1) + x_centre, x + y_centre, c1, bm);
                    ColorPixel(y + x_centre, -x + y_centre, c2, bm);
                    ColorPixel((y-1) + x_centre, -x + y_centre, c1, bm);
                    ColorPixel(-y + x_centre, -x + y_centre, c2, bm);
                    ColorPixel(-(y-1) + x_centre, -x + y_centre, c1, bm);
                }
            } 
        }
    }
}
