﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppComputerGraphics2
{
    public interface IShape
    {
        void Render();
        string Save();
    }
}