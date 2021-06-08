using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = System.Drawing.Point;

namespace WpfAppComputerGraphics2
{
    public class Edge
    {
        public int ymax;
        public double xmin;
        public double k; // 1/m
        
        public Edge(int ym, double xm, double K)
        {
            ymax = ym;
            xmin = xm;
            k = K;
        }
    }
}
