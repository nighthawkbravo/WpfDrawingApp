using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAppComputerGraphics2.Shapes
{
    public class Circle : IShape
    {
        public Point Center { set; get; }
        public Point EdgePoint { set; get; }

        public Circle(Point c, Point e)
        {
            Center = c;
            EdgePoint = e;
        }

        public void Render() { }

        public string Save()
        {
            return $"Circle: ({Center})({EdgePoint})";
        }
    }
}
