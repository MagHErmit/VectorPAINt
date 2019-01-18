using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Paint2.Paint
{
    class HandTool : Tool
    {

        public override void MouseDown(Point point)
        {
            Globals.Figures.Add(new HandLine(point));
            Globals.HandScrollX = point.X;
            Globals.HandScrollY = point.Y;
        }

        public override void MouseMove(Point point)
        {
            Globals.HandScrollX += Globals.Figures[Globals.Figures.Count - 1].Coordinates[0].X - Globals.Figures[Globals.Figures.Count - 1].Coordinates[1].X;
            Globals.HandScrollY += Globals.Figures[Globals.Figures.Count - 1].Coordinates[0].Y - Globals.Figures[Globals.Figures.Count - 1].Coordinates[1].Y;
            Globals.Figures[Globals.Figures.Count - 1].AddCord(point);
        }

        public override void MouseUp(Point point)
        {
            Globals.Figures.Remove(Globals.Figures[Globals.Figures.Count - 1]);
        }
    }
}
