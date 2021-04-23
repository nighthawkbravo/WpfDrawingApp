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
        public Point p1;
        public Point p2;

        public Line(int x1, int y1, int x2, int y2)
        {
            p1 = new Point(x1, y1);
            p2 = new Point(x2, y2);
        }

        public Line(Point P1, Point P2) 
        {
            p1 = P1;
            p2 = P2;
        }

        public void Render() { }

        public string Save()
        {
            return $"Line: ({p1}) ({p2})";
        }
    }
}
