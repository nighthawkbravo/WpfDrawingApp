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
    public class Line : IShape
    {
        public Color myColor { get; set; }
        public Point P1 { set; get; }
        public Point P2 { set; get; }
        public int Thickness { set; get; }

        public Line(Point p1, Point p2, int t=1) 
        {
            P1 = p1;
            P2 = p2;
            Thickness = t;
            myColor = Color.Black;
        }
        public Point GetCenter()
        {
            return new Point((P1.X+P2.X) / 2, (P1.Y + P2.Y) / 2);
        }
        public string Save()
        {
            return $"Line({Thickness}): ({P1.X},{P1.Y})-({P2.X},{P2.Y})";
        }
        public string GetNameAndCenter()
        {
            return $"Line ({GetCenter().X},{GetCenter().Y})";
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

        public Bitmap Render(Bitmap bm)
        {
            bm.SetPixel(P1.X, P1.Y, myColor);            
            // Vertical line
            if (P1.X == P2.X)
            {
                for(int i=Lower(P1.Y, P2.Y); i<Greater(P1.Y, P2.Y); ++i)
                {
                    bm.SetPixel(P1.X, i, myColor);
                }
            }
            // Horizontal line
            else if(P1.Y == P2.Y)
            {
                for (int i = Lower(P1.X, P2.X); i < Greater(P1.X, P2.X); ++i)
                {
                    bm.SetPixel(i, P1.Y, myColor);
                }
            }
            // non Horizontal/Vertical line
            else
            {
                MidPtL(P1,P2,bm);
            }


            bm.SetPixel(P2.X, P2.Y, myColor);
            return bm;            
        }
        
        private void MidPtL(Point p1, Point p2, Bitmap bm)
        {
            var slope = (double) (p2.Y - p1.Y) / (p2.X - p1.X);
            //MessageBox.Show($"SLOPE: {slope}, ({p1.X},{p1.Y}) - ({p2.X},{p2.Y})");


            // iterate over x's - slope=(0,1]
            if (slope > 0 && slope <= 1)
            {
                if(p1.X < p2.X && p1.Y < p2.Y) XmidPointBLTR(p1.X, p1.Y, p2.X, p2.Y, bm); 
                else XmidPointTRBL(p1.X, p1.Y, p2.X, p2.Y, bm);
            }
            // iterate over x's - slope=[-1,0)
            if (slope < 0 && slope >= -1)
            {
                if (p1.X > p2.X && p1.Y < p2.Y) XmidPointBRTL(p1.X, p1.Y, p2.X, p2.Y, bm);
                else XmidPointTLBR(p1.X, p1.Y, p2.X, p2.Y, bm);
            }

            // iterate over y's - slope=(1,inf)
            if (slope > 1 && slope <= double.PositiveInfinity)
            {
                if (p1.X < p2.X && p1.Y < p2.Y) YmidPointBLTR(p1.X, p1.Y, p2.X, p2.Y, bm);
                else YmidPointTRBL(p1.X, p1.Y, p2.X, p2.Y, bm);
            }
            // iterate over y's - slope=(-inf,-1)
            if (slope < -1 && slope >= double.NegativeInfinity)
            {
                if (p1.X > p2.X && p1.Y < p2.Y) YmidPointBRTL(p1.X, p1.Y, p2.X, p2.Y, bm);
                else YmidPointTLBR(p1.X, p1.Y, p2.X, p2.Y, bm);
            }
        }

        // --------X
        private void XmidPointBLTR(int x1, int y1, int x2, int y2, Bitmap bm)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;
            int d = dy - (dx / 2);
            int x = x1, y = y1;
            ColorPixels(x, y, bm);

            while (x < x2)
            {
                x++;
                if (d < 0) d = d + dy;
                else
                {
                    d += (dy - dx);
                    y++;
                }
                ColorPixels(x, y, bm);
            }
        }
        private void XmidPointTRBL(int x2, int y2, int x1, int y1, Bitmap bm)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;
            int d = dy - (dx / 2);
            int x = x1, y = y1;
            ColorPixels(x, y, bm);

            while (x < x2)
            {
                x++;
                if (d < 0) d = d + dy;
                else
                {
                    d += (dy - dx);
                    y++;
                }
                ColorPixels(x, y, bm);
            }
        }
        private void XmidPointBRTL(int x1, int y1, int x2, int y2, Bitmap bm)
        {
            int dx = x1 - x2;
            int dy = y2 - y1;
            int d = dy - (dx / 2);
            int x = x1, y = y1;
            ColorPixels(x, y, bm);

            while (x > x2)
            {
                x--;
                if (d < 0) d = d + dy;
                else
                {
                    d += (dy - dx);
                    y++;
                }
                ColorPixels(x, y, bm);
            }
        }
        private void XmidPointTLBR(int x2, int y2, int x1, int y1, Bitmap bm)
        {
            int dx = x1 - x2;
            int dy = y2 - y1;
            int d = dy - (dx / 2);
            int x = x1, y = y1;
            ColorPixels(x, y, bm);

            while (x > x2)
            {
                x--;       
                if (d < 0) d = d + dy;
                else
                {
                    d += (dy - dx);
                    y++;
                }
                ColorPixels(x, y, bm);
            }
        }

        // --------Y

        private void YmidPointBLTR(int x1, int y1, int x2, int y2, Bitmap bm)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;

            int d = dx - (dy / 2);
            int x = x1, y = y1;
            ColorPixels(x, y, bm);

            while (y < y2)
            {
                y++;
                if (d < 0) d = d + dx;
                else
                {
                    d += (dx - dy);
                    x++;
                }
                ColorPixels(x, y, bm);
            }
        }
        private void YmidPointTRBL(int x2, int y2, int x1, int y1, Bitmap bm)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;

            int d = dx - (dy / 2);
            int x = x1, y = y1;
            ColorPixels(x, y, bm);

            while (y < y2)
            {
                y++;
                if (d < 0) d = d + dx;
                else
                {
                    d += (dx - dy);
                    x++;
                }
                ColorPixels(x, y, bm);
            }
        }
        private void YmidPointBRTL(int x1, int y1, int x2, int y2, Bitmap bm)
        {
            int dx = x1 - x2; // x1-x2
            int dy = y2 - y1; //

            int d = dx - (dy / 2); // 
            int x = x1, y = y1;
            ColorPixels(x, y, bm);

            while (y < y2)
            {
                y++;
                if (d < 0) d = d + dx;
                else
                {
                    d += (dx - dy);
                    x--;            // x--
                }
                ColorPixels(x, y, bm);
            }
        }
        private void YmidPointTLBR(int x2, int y2, int x1, int y1, Bitmap bm)
        {
            int dx = x1 - x2; // x1-x2
            int dy = y2 - y1; //

            int d = dx - (dy / 2); // 
            int x = x1, y = y1;
            ColorPixels(x, y, bm);

            while (y < y2)
            {
                y++;
                if (d < 0) d = d + dx;
                else
                {
                    d += (dx - dy);
                    x--;            // x--
                }
                ColorPixels(x, y, bm);
            }
        }

        private void ColorPixels(int x, int y, Bitmap bm)
        {
            bm.SetPixel(x, y, myColor);
        }
    }
}
