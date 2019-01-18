using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Runtime.Serialization;

namespace Paint2.Paint
{
    [Serializable]

    class RoundRect : Figure
    {
        public RoundRect() { }

        public Brush BrushColor { get; set; }

        public RoundRect(Point point)
        {
            Coordinates = new List<Point> { point, point };
            Color = Globals.ColorNow;
            ColorString = Globals.ColorStringNow;
            BrushColor = Globals.BrushNow;
            BrushColorString = Globals.BrushStringNow;
            PenThikness = Globals.ThicnessNow;
            Dash = Globals.DashNow;
            DashString = Globals.DashStringhNow;
            Pen = new Pen(Color, PenThikness) { DashStyle = Dash };
            Select = false;
            SelectRect = null;
            RoundX = Globals.RoundXNow;
            RoundY = Globals.RoundYNow;
            Type = "RoundRect";

        }

        public override void Draw(DrawingContext drawingContext)
        {
            var diagonal = Point.Subtract(Coordinates[0], Coordinates[1]);
            drawingContext.DrawRoundedRectangle(BrushColor, Pen, new Rect(Coordinates[1], diagonal), RoundX, RoundY);
        }

        public override void AddCord(Point point)
        {
            Coordinates[1] = point;
        }

        public override void Selected()
        {
            if (Select == false)
            {
                Point pForRect3 = new Point
                {
                    X = Math.Min(Coordinates[0].X, Coordinates[1].X),
                    Y = Math.Min(Coordinates[0].Y, Coordinates[1].Y)
                };
                Point pForRect4 = new Point
                {
                    X = Math.Max(Coordinates[0].X, Coordinates[1].X),
                    Y = Math.Max(Coordinates[0].Y, Coordinates[1].Y)
                };
                SelectRect = new ZoomRect(new Point(pForRect3.X - 10, pForRect3.Y - 10), new Point(pForRect4.X + 10, pForRect4.Y + 10));
                var drawingVisual = new DrawingVisual();
                var drawingContext = drawingVisual.RenderOpen();
                SelectRect.Draw(drawingContext);
                drawingContext.Close();
                Paint.Globals.FigureHost.Children.Add(drawingVisual);
                Select = true;
            }
        }

        public override void UnSelected()
        {
            if (Select == true)
            {
                Select = false;
                SelectRect = null;
            }
        }

        public override void ChangePen(Brush color, string str)
        {
            Pen = new Pen(color, PenThikness) { DashStyle = Dash };
            Color = color;
            ColorString = str;
        }

        public override void ChangePen(Brush color, string str, bool check)
        {
            BrushColor = color;
            BrushColorString = str;
        }

        public override void ChangePen(DashStyle dash, string str)
        {
            Pen = new Pen(Color, PenThikness) { DashStyle = dash };
            Dash = dash;
            DashString = str;
        }

        public override void ChangePen(double thikness)
        {
            Pen = new Pen(Color, thikness) { DashStyle = Dash };
            PenThikness = thikness;
        }

        public override void ChangeRoundX(double newRoundX)
        {
            RoundX = newRoundX;
        }

        public override void ChangeRoundY(double newRoundY)
        {
            RoundY = newRoundY;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Coordinates", Coordinates);
            info.AddValue("PenThikness", PenThikness);
            info.AddValue("Color", ColorString);
            info.AddValue("BrushColor", BrushColorString);
            info.AddValue("Dash", DashString);
            info.AddValue("Type", Type);
            info.AddValue("RoundX", RoundX);
            info.AddValue("RoundY", RoundY);
        }

        public RoundRect(SerializationInfo info, StreamingContext context)
        {
            Coordinates = (List<Point>)info.GetValue("Coordinates", typeof(List<Point>));
            PenThikness = (double)info.GetValue("PenThikness", typeof(double));
            ColorString = (string)info.GetValue("Color", typeof(string));
            BrushColorString = (string)info.GetValue("BrushColor", typeof(string));
            DashString = (string)info.GetValue("Dash", typeof(string));
            Type = (string)info.GetValue("Type", typeof(string));
            RoundX = (double)info.GetValue("RoundX", typeof(double));
            RoundY = (double)info.GetValue("RoundY", typeof(double));
            Color = Globals.TransformColor[ColorString];
            BrushColor = Globals.TransformColor[BrushColorString];
            Dash = Globals.TransformDashProp[DashString];
            Pen = new Pen(Color, PenThikness) { DashStyle = Dash };
        }

        public override Figure Clone()
        {
            return new RoundRect
            {
                BrushColor = this.BrushColor,
                BrushColorString = this.BrushColorString,
                Color = this.Color,
                ColorString = this.ColorString,
                Coordinates = new List<Point>(Coordinates),
                Dash = this.Dash,
                DashString = this.DashString,
                Pen = this.Pen,
                PenThikness = this.PenThikness,
                RoundX = this.RoundX,
                RoundY = this.RoundY,
                Select = this.Select,
                SelectRect = this.SelectRect,
                Type = this.Type
            };
        }
    }
}
