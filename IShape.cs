using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAppComputerGraphics2
{
    public interface IShape
    {
        Point GetCenter();
        void Render();
        string Save();
    }
}
