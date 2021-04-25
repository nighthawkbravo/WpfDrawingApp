using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

namespace WpfAppComputerGraphics2
{
    public interface IShape
    {
        Color myColor { get; set; }
        Point GetCenter();
        BitmapImage Render(BitmapImage bmi);
        string Save();
        string GetNameAndCenter();
    }
}
