using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using Bitmap = System.Drawing.Bitmap;

namespace WpfAppComputerGraphics2
{
    public interface IShape
    {
        Color myColor { get; set; }
        Point GetCenter();
        Bitmap Render(Bitmap wbm);
        string Save();
        string GetNameAndCenter();
    }
}
