using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAppComputerGraphics2.Shapes
{
    public class Line : IShape
    {
        public Point P1 { set; get; }
        public Point P2 { set; get; }
        public int Thickness { set; get; }

        public Line(int x1, int y1, int x2, int y2, int t=1)
        {
            P1 = new Point(x1, y1);
            P2 = new Point(x2, y2);
            Thickness = t;
        }

        public Line(Point p1, Point p2, int t=1) 
        {
            P1 = p1;
            P2 = p2;
            Thickness = t;
        }
        
        public Point GetCenter()
        {
            return new Point((P1.X+P2.X) / 2, (P1.Y + P2.Y) / 2);
        }

        public string Save()
        {
            return $"Line({Thickness}): ({P1})({P2})";
        }

        public void Render() { }
    }
}
