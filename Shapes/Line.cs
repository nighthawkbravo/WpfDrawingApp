using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

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
            return $"Line({Thickness}): ({P1})({P2})";
        }

        public BitmapImage Render(BitmapImage bmi) { return null; }
    }
}
