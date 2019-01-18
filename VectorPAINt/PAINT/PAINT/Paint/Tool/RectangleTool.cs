﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Paint2.Paint
{
    class RectanglTool : Tool
    {
        public override void MouseDown(Point point)
        {
            Globals.Figures.Add(new Rectangle(point));
        }

        public override void MouseMove(Point point)
        {
            Globals.Figures[Globals.Figures.Count - 1].AddCord(point);
        }
    }
}
