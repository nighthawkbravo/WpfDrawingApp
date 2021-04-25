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
        public Point GetCenter()
        {
            return Center;
        }
        public string Save()
        {
            return $"Circle: ({Center.X},{Center.Y})-({EdgePoint.X},{EdgePoint.Y})";
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
        public Bitmap Render(Bitmap bm)
        {
            int r = (int)CalcEucliDist(Center, EdgePoint);

            bm = midPointCircleDraw(Center.X, Center.Y, r, bm);

            return bm;
        }

        private Bitmap midPointCircleDraw(int x_centre, int y_centre, int r, Bitmap bm)
        {
            int x = r, y = 0;

            
            // When radius is zero only a single
            // point will be printed
            if (r > 0)
            {
                //Console.Write("(" + (x + x_centre) + ", " + (-y + y_centre) + ")");
                ColorPixel(x + x_centre, -y + y_centre, bm);
                //Console.Write("(" + (y + x_centre) + ", " + (x + y_centre) + ")");
                ColorPixel(y + x_centre, x + y_centre, bm);
                //Console.WriteLine("(" + (-y + x_centre) + ", " + (x + y_centre) + ")");
                ColorPixel(-y + x_centre, x + y_centre, bm);
            }

            // Initialising the value of P
            int P = 1 - r;
            while (x > y)
            {
                y++;

                // Mid-point is inside or on the perimeter
                if (P <= 0) P = P + 2 * y + 1;

                // Mid-point is outside the perimeter
                else
                {
                    x--;
                    P = P + 2 * y - 2 * x + 1;
                }

                // All the perimeter points have already 
                // been printed
                if (x < y) break;

                // Printing the generated point and its 
                // reflection in the other octants after
                // translation

                //Console.Write("(" + (x + x_centre) + ", " + (y + y_centre) + ")");
                ColorPixel(x + x_centre, y + y_centre, bm);
                //Console.Write("(" + (-x + x_centre) + ", " + (y + y_centre) + ")");
                ColorPixel(-x + x_centre, y + y_centre, bm);
                //Console.Write("(" + (x + x_centre) + ", " + (-y + y_centre) + ")");
                ColorPixel(x + x_centre, -y + y_centre, bm);
                //Console.WriteLine("(" + (-x + x_centre) + ", " + (-y + y_centre) + ")");
                ColorPixel(-x + x_centre, -y + y_centre, bm);

                // If the generated point is on the 
                // line x = y then the perimeter points
                // have already been printed
                if (x != y)
                {
                    //Console.Write("(" + (y + x_centre) + ", " + (x + y_centre) + ")");
                    ColorPixel(y + x_centre, x + y_centre, bm);
                    //Console.Write("(" + (-y + x_centre) + ", " + (x + y_centre) + ")");
                    ColorPixel(-y + x_centre, x + y_centre, bm);
                    //Console.Write("(" + (y + x_centre) + ", " + (-x + y_centre) + ")");
                    ColorPixel(y + x_centre, -x + y_centre, bm);
                    //Console.WriteLine("(" + (-y + x_centre) + ", " + (-x + y_centre) + ")");
                    ColorPixel(-y + x_centre, -x + y_centre, bm);
                }
            }
            return bm;
        }
        public void ColorPixel(int x, int y, Bitmap bm)
        {
            if (!IsInBound(x, y, bm)) return;
            bm.SetPixel(x, y, myColor);
        }
    }
}
