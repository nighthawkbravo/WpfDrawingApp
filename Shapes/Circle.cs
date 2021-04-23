using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

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
            return $"Circle: ({Center})({EdgePoint})";
        }

        public void Render() { }
    }
}
