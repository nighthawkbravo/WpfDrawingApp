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

        public Point GetCenter()
        {
            double sumX = 0;
            double sumY = 0;

            foreach (var p in Points)
            {
                sumX += p.X;
                sumY += p.Y;
            }

            return new Point(sumX / numOfPoints, sumY / numOfPoints);
        }

        public string Save()
        {
            string res = $"Polygon({numOfPoints}): ";

            foreach(var p in Points)
            {
                res += $"({p})";
            }

            return res;
        }

        public void Render() { }
    }
}
