using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAppComputerGraphics2.Shapes
{
    public class Polygon : IShape
    {
        private List<Point> Points;
        private readonly int numOfPoints;


        public Polygon(List<Point> ps)
        {
            Points = ps;
            numOfPoints = ps.Count;
        }

        public Polygon(int n, List<Point> ps)
        {
            Points = ps;
            numOfPoints = n;
        }

        public void Render() { }
        public string Save()
        {
            string res = $"Polygon({numOfPoints}): ";

            foreach(var p in Points)
            {
                res += $"({p})";
            }

            return res;
        }
    }
}
