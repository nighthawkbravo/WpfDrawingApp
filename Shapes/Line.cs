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

        public Line() { }

        public void Render() { }

        public string Save()
        {
            return $"Line: ({p1}) ({p2})";
        }
    }
}
