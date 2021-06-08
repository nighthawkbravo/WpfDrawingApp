using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WpfAppComputerGraphics2._3DStuff
{
    public class Point3d
    {
        private int x;
        private int y;
        private int z;
        private int w;

        public Point3d(int X, int Y, int Z)
        {
            x = X;
            y = Y;
            z = Z;
            w = 1;
        }

        public void SetValues(int X, int Y, int Z)
        {
            x = X;
            y = Y;
            z = Z;
        }
    }
}
