using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfAppComputerGraphics2.Separate;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using System.Drawing;

namespace WpfAppComputerGraphics2.Shapes
{
    public class Polygon : IShape
    {
        public Color myColor { get; set; }
        private List<Point> Points;
        private readonly int numOfPoints;


        public Polygon(List<Point> ps)
        {
            Points = ps;
            numOfPoints = ps.Count;
            myColor = Color.Black;
        }

        public Polygon(int n, List<Point> ps)
        {
            Points = ps;
            numOfPoints = n;
            myColor = Color.Black;
        }

        public Point GetCenter()
        {
            int sumX = 0;
            int sumY = 0;

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
                res += $"({p.X},{p.Y})";
            }

            return res;
        }

        public string GetNameAndCenter()
        {
            return $"Polygon ({GetCenter().X},{GetCenter().Y})";
        }

        public BitmapImage Render(BitmapImage bmi) { return null; }
    }
}
