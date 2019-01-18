using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Paint2.Paint;

namespace Paint2.Paint
{
    class ZoomTool : Tool
    {
        public override void MouseDown(Point point)
        {
            Globals.Figures.Add(new ZoomRect(point));
        }

        public override void MouseMove(Point point)
        {
            Globals.Figures[Globals.Figures.Count - 1].AddCord(point);
        }

        public override void MouseUp(Point point)
        {
            if (Point.Subtract(Globals.Figures[Globals.Figures.Count - 1].Coordinates[0], Globals.Figures[Globals.Figures.Count - 1].Coordinates[1]).Length > 50)
            {
                if (Globals.Figures[Globals.Figures.Count - 1].Coordinates[1].X > Globals.Figures[Globals.Figures.Count - 1].Coordinates[0].X)
                {
                    Globals.ScaleRateX = Globals.CanvasWidth / (Globals.Figures[Globals.Figures.Count - 1].Coordinates[1].X - Globals.Figures[Globals.Figures.Count - 1].Coordinates[0].X);
                }
                else
                {
                    Globals.ScaleRateX = Globals.CanvasWidth / (Globals.Figures[Globals.Figures.Count - 1].Coordinates[0].X - Globals.Figures[Globals.Figures.Count - 1].Coordinates[1].X);
                }

                if (Globals.Figures[Globals.Figures.Count - 1].Coordinates[1].Y > Globals.Figures[Globals.Figures.Count - 1].Coordinates[0].Y)
                {
                    Globals.ScaleRateY = Globals.CanvasHeigth / (Globals.Figures[Globals.Figures.Count - 1].Coordinates[1].Y - Globals.Figures[Globals.Figures.Count - 1].Coordinates[0].Y);
                }
                else
                {
                    Globals.ScaleRateY = Globals.CanvasHeigth / (Globals.Figures[Globals.Figures.Count - 1].Coordinates[0].Y - Globals.Figures[Globals.Figures.Count - 1].Coordinates[1].Y);
                }

                if (Globals.ScaleRateX > Globals.ScaleRateY)
                {
                    Globals.ScaleRateY = Globals.ScaleRateX;
                }
                else
                {
                    Globals.ScaleRateX = Globals.ScaleRateY;
                }

                if (Globals.Figures[Globals.Figures.Count - 1].Coordinates[1].X > Globals.Figures[Globals.Figures.Count - 1].Coordinates[0].X)
                {
                    Globals.DistanceToPointX = Globals.Figures[Globals.Figures.Count - 1].Coordinates[0].X;
                }
                else
                {
                    Globals.DistanceToPointX = Globals.Figures[Globals.Figures.Count - 1].Coordinates[1].X;
                }

                if (Globals.Figures[Globals.Figures.Count - 1].Coordinates[1].Y > Globals.Figures[Globals.Figures.Count - 1].Coordinates[0].Y)
                {
                    Globals.DistanceToPointY = Globals.Figures[Globals.Figures.Count - 1].Coordinates[0].Y;
                }
                else
                {
                    Globals.DistanceToPointY = Globals.Figures[Globals.Figures.Count - 1].Coordinates[1].Y;
                }
            }
            else
            {
                Globals.ScaleRateX = 1;
                Globals.ScaleRateY = 1;
                Globals.DistanceToPointX = 0;
                Globals.DistanceToPointY = 0;
            }
            Globals.Figures.Remove(Globals.Figures[Globals.Figures.Count - 1]);
        }
    }
}
