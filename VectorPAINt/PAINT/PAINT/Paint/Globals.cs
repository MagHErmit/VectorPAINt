using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Paint2.Paint
{
    [Serializable]
    public class Globals
    {
        public static List<Figure> Figures = new List<Figure>();
        
        public static FigureHost FigureHost = new FigureHost();

        public static Tool ToolNow = new PenTool();

        public static List<List<Figure>> HistoryCanvas = new List<List<Figure>>();

        public static int StepNumber = 0;

        public static Tool HandTool = new HandForFigureTool();

        public static void AddStep()
        {
            List<Figure> figuresNow = new List<Figure>();
            foreach(Figure figure in Figures)
            {
                figuresNow.Add(figure.Clone());
            }
            HistoryCanvas.Add(figuresNow);
            StepNumber++;
            if (StepNumber != HistoryCanvas.Count)
            {
                HistoryCanvas.RemoveRange(StepNumber - 1, HistoryCanvas.Count - StepNumber);
            }
            Figures.Clear();
            foreach(Figure figure in figuresNow)
            {
                Figures.Add(figure.Clone());
            }
            foreach(Figure figure in HistoryCanvas[StepNumber - 1])
            {
                figure.Select = false;
                figure.SelectRect = null;
            }
            if (HistoryCanvas.Count > 1)
            {
                foreach (Figure figure in HistoryCanvas[StepNumber - 2])
                {
                    figure.Select = false;
                    figure.SelectRect = null;
                }
            }
        }

        public static void gotoPastStep()
        {
            if (StepNumber != 1)
            {
                StepNumber--;
                Figures.Clear();
                foreach (Figure figure in HistoryCanvas[StepNumber - 1])
                {
                    Figures.Add(figure.Clone());
                }
            }
        }

        public static void gotoNextStep()
        {
            if (StepNumber != HistoryCanvas.Count)
            {
                StepNumber++;
                Figures.Clear();
                foreach (Figure figure in HistoryCanvas[StepNumber - 1])
                {
                    Figures.Add(figure.Clone());
                }
            }
        }

        public static Brush BrushNow = null;
        public static string BrushStringNow = "null";
        public static Brush tempBrush = null;
        public static Brush ColorNow = Brushes.Black;
        public static string ColorStringNow = "Black";
        public static string tempStringBrush = "";
        public static double ThicnessNow = 4;
        public static DashStyle DashNow = DashStyles.Solid;
        public static string DashStringhNow = "―――――";
        public static double RoundXNow = 0;
        public static double RoundYNow = 0;


        public static double ScaleRateX = 1;
        public static double ScaleRateY = 1;
        public static double DistanceToPointX;
        public static double DistanceToPointY;
        public static double HandScrollX;
        public static double HandScrollY;
        public static bool FirstPress = true;
        public static bool SecondPress = false;
        public static double CanvasWidth;
        public static double CanvasHeigth;
        
        public static readonly Dictionary<string, Tool> Transform = new Dictionary<string, Tool>()
        {
            { "Line", new LineTool() },
            { "Rectangle", new RectanglTool() },
            { "Ellipse", new EllipseTool() },
            { "RoundRect", new RoundRectTool() },
            { "Pen", new PenTool() },
            { "Hand", new HandTool() },
            { "ZoomRect", new ZoomTool() },
            { "Allotment", new AllotmentTool() },

        };

        public static readonly Dictionary<string, DashStyle> TransformDash = new Dictionary<string, DashStyle>()
        {
            { "0", DashStyles.Solid },
            { "1", DashStyles.Dash },
            { "2", DashStyles.DashDot },
            { "3", DashStyles.DashDotDot },
            { "4", DashStyles.Dot },

        };

        public static readonly Dictionary<string, DashStyle> TransformDashProp = new Dictionary<string, DashStyle>()
        {
            { "―――――", DashStyles.Solid },
            { "— — — — — —", DashStyles.Dash },
            { "— ∙ — ∙ — ∙ — ∙ —", DashStyles.DashDot },
            { "— ∙ ∙ — ∙ ∙ — ∙ ∙ — ", DashStyles.DashDotDot },
            { "∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙", DashStyles.Dot },

        };
        
        public static readonly Dictionary<string, Brush> TransformColor = new Dictionary<string, Brush>()
        {
            { "Black", Brushes.Black },
            { "Gray", Brushes.Gray },
            { "White", Brushes.White },
            { "Red", Brushes.Red },
            { "Orange", Brushes.Orange },
            { "Yellow", Brushes.Yellow },
            { "Green", Brushes.Green },
            { "LightBlue", Brushes.LightBlue },
            { "Blue", Brushes.Blue },
            { "Purple", Brushes.Purple },
            { "null", null }
        };
    }
}
