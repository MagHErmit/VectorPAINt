using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Paint2.Paint;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Paint2
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }

        bool ClikOnCanvas = false;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            MyCanvas.Children.Add(Globals.FigureHost);
            ButtonGeneration.Generation();
            Globals.AddStep();
        }

        private void Invalidate()
        {
            Globals.FigureHost.Children.Clear();
            var drawingVisual = new DrawingVisual();
            var drawingContext = drawingVisual.RenderOpen();
            foreach (var figure in Globals.Figures)
            {
                figure.Draw(drawingContext);
                if(figure.SelectRect != null)
                {
                    figure.SelectRect.Draw(drawingContext);
                }
            }

            drawingContext.Close();
            Globals.FigureHost.Children.Add(drawingVisual);
        }

        private void MyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Globals.ToolNow.MouseDown(e.GetPosition(MyCanvas));
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                Globals.tempBrush = Globals.BrushNow;
                Globals.BrushNow = Globals.ColorNow;
                Globals.ColorNow = Globals.tempBrush;
                Globals.tempStringBrush = Globals.BrushStringNow;
                Globals.BrushStringNow = Globals.ColorStringNow;
                Globals.ColorStringNow = Globals.tempStringBrush;
                Globals.ToolNow.MouseDown(e.GetPosition(MyCanvas));
                Globals.tempBrush = Globals.BrushNow;
                Globals.BrushNow = Globals.ColorNow;
                Globals.ColorNow = Globals.tempBrush;
                Globals.tempStringBrush = Globals.BrushStringNow;
                Globals.BrushStringNow = Globals.ColorStringNow;
                Globals.ColorStringNow = Globals.tempStringBrush;
            }
            ClikOnCanvas = true;
            Invalidate();
        }

        private void MyCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (ClikOnCanvas)
            {
                Globals.ToolNow.MouseMove(e.GetPosition(MyCanvas));
                if (Globals.ToolNow == Globals.Transform["Hand"])
                {
                    ScrollViewerCanvas.ScrollToVerticalOffset(Globals.HandScrollY);
                    ScrollViewerCanvas.ScrollToHorizontalOffset(Globals.HandScrollX);
                }
                Invalidate();
            }   
        }
        private void MyCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ClikOnCanvas)
            {
                Globals.ToolNow.MouseUp(e.GetPosition(MyCanvas));

                if (Globals.ToolNow != Globals.Transform["Allotment"] & Globals.ToolNow != Globals.Transform["ZoomRect"] & Globals.ToolNow != Globals.Transform["Hand"])
                {
                    Globals.AddStep();
                    gotoPastStep.IsEnabled = true;
                    gotoSecondStep.IsEnabled = false;
                }
                if (Globals.ToolNow == Globals.Transform["ZoomRect"])
                {
                    MyCanvas.LayoutTransform = new ScaleTransform(Globals.ScaleRateX, Globals.ScaleRateY);
                    ScrollViewerCanvas.ScrollToVerticalOffset(Globals.DistanceToPointY * Globals.ScaleRateY);
                    ScrollViewerCanvas.ScrollToHorizontalOffset(Globals.DistanceToPointX * Globals.ScaleRateX);
                }
                if (Globals.ToolNow == Globals.HandTool)
                {
                    Globals.ToolNow = Globals.Transform["Allotment"];
                }
                ClikOnCanvas = false;
                Invalidate();
            }
        }

        private void MyCanvas_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (ClikOnCanvas)
            {
                Globals.ToolNow.MouseUp(e.GetPosition(MyCanvas));

                if (Globals.ToolNow != Globals.Transform["Allotment"] & Globals.ToolNow != Globals.Transform["ZoomRect"] & Globals.ToolNow != Globals.Transform["Hand"] & Globals.ToolNow != Globals.HandTool)
                {
                    Globals.AddStep();
                    gotoPastStep.IsEnabled = true;
                    gotoSecondStep.IsEnabled = false;
                }
                if (Globals.ToolNow == Globals.Transform["ZoomRect"])
                {
                    MyCanvas.LayoutTransform = new ScaleTransform(Globals.ScaleRateX, Globals.ScaleRateY);
                    ScrollViewerCanvas.ScrollToVerticalOffset(Globals.DistanceToPointY * Globals.ScaleRateY);
                    ScrollViewerCanvas.ScrollToHorizontalOffset(Globals.DistanceToPointX * Globals.ScaleRateX);
                }
                ClikOnCanvas = false;
                Invalidate();
            }
        }

        public void ButtonChangeTool(object sender, RoutedEventArgs e)
        {
            Globals.ToolNow = Globals.Transform[(sender as System.Windows.Controls.Button).Tag.ToString()];
            if((sender as System.Windows.Controls.Button).Tag.ToString() == "RoundRect")
            {
                textBoxRoundRectX.IsEnabled = true;
                textBoxRoundRectY.IsEnabled = true;
            }
            else
            {
                textBoxRoundRectX.IsEnabled = false;
                textBoxRoundRectY.IsEnabled = false;
            }
            foreach (Figure figure in Globals.Figures)
            {
                figure.UnSelected();
            }
            Invalidate();
            PropToolBarPanel.Children.Clear();
        }

        public void ButtonChangeColor(object sender, RoutedEventArgs e)
        {
            if (Globals.FirstPress== true){
                Globals.ColorNow = Globals.TransformColor[(sender as System.Windows.Controls.Button).Tag.ToString()];
                Globals.ColorStringNow = (sender as System.Windows.Controls.Button).Tag.ToString();
                if((sender as System.Windows.Controls.Button).Background == null) { button_firstColor.Background = Brushes.Gray; }
                else { button_firstColor.Background = (sender as System.Windows.Controls.Button).Background; }
                
            }
            else
            {
                Globals.BrushNow = Globals.TransformColor[(sender as System.Windows.Controls.Button).Tag.ToString()];
                Globals.BrushStringNow = (sender as System.Windows.Controls.Button).Tag.ToString();
                if ((sender as System.Windows.Controls.Button).Background == null) { button_secondColor.Background = Brushes.Gray; }
                else { button_secondColor.Background = (sender as System.Windows.Controls.Button).Background; }
            }
        }

        private void ThiknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Globals.ThicnessNow = ThiknessSlider.Value;
        }

        private void FirstColor(object sender, RoutedEventArgs e)
        {
            Globals.FirstPress = true;
            Globals.SecondPress = false;
            button_firstColor.BorderThickness = new Thickness(5);
            button_secondColor.BorderThickness = new Thickness(0);
        }

        private void SecondColor(object sender, RoutedEventArgs e)
        {
            Globals.FirstPress = false;
            Globals.SecondPress = true;
            button_secondColor.BorderThickness = new Thickness(5);
            button_firstColor.BorderThickness = new Thickness(0);
        }

        private void MyCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            Globals.CanvasHeigth = MyCanvas.Height;
            Globals.CanvasWidth = MyCanvas.Width;
        }

        private void MyCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Globals.CanvasHeigth = MyCanvas.Height;
            Globals.CanvasWidth = MyCanvas.Width;
        }

        public void CleanMyCanvas(object sender, RoutedEventArgs e)
        {
            Globals.FigureHost.Children.Clear();
            Globals.Figures.Clear();
            Globals.StepNumber = 0;
            Globals.HistoryCanvas.Clear();
            Globals.AddStep();
            gotoPastStep.IsEnabled = false;
            gotoSecondStep.IsEnabled = false;
        }

        public void MinusZoomMyCanvas(object sender, RoutedEventArgs e)
        {
            MyCanvas.LayoutTransform = new ScaleTransform(1, 1);
            ScrollViewerCanvas.ScrollToVerticalOffset(0);
            ScrollViewerCanvas.ScrollToHorizontalOffset(0);
        }

        private void ChangeSelectionDash(object sender, SelectionChangedEventArgs e)
        {
            Globals.DashNow = Globals.TransformDash[comboBoxDash.SelectedIndex.ToString()];
            if (comboBoxDash.SelectedIndex.ToString() == "0")
            {
                Globals.DashStringhNow = "―――――";
            }
            if (comboBoxDash.SelectedIndex.ToString() == "1")
            {
                Globals.DashStringhNow = "— — — — — —";
            }
            if (comboBoxDash.SelectedIndex.ToString() == "2")
            {
                Globals.DashStringhNow = "— ∙ — ∙ — ∙ — ∙ —";
            }
            if (comboBoxDash.SelectedIndex.ToString() == "3")
            {
                Globals.DashStringhNow = "— ∙ ∙ — ∙ ∙ — ∙ ∙ — ";
            }
            if (comboBoxDash.SelectedIndex.ToString() == "4")
            {
                Globals.DashStringhNow = "∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙";
            }
        }

        private void textBoxRoundRectX_TextChanged(object sender, TextChangedEventArgs e)
        {
            Globals.RoundXNow = Convert.ToDouble(textBoxRoundRectX.Text);
        }

        private void textBoxRoundRectY_TextChanged(object sender, TextChangedEventArgs e)
        {
            Globals.RoundYNow = Convert.ToDouble(textBoxRoundRectY.Text);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.Figures.Count != 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Сохранить как";
                sfd.OverwritePrompt = true;
                sfd.CheckPathExists = true;
                sfd.Filter = "Files(*.bin)|*.bin";
                sfd.ShowDialog();
                if (sfd.FileName != "")
                {
                    FileStream file = (FileStream)sfd.OpenFile();
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(file, Globals.Figures);
                    file.Close();
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Канвас пуст((((");
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.Figures.Count != 0)
            {
                DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Сохранить текущее изображение?", "", MessageBoxButtons.YesNo);
                if(dialogResult == System.Windows.Forms.DialogResult.Yes)
                {
                    SaveButton_Click(sender,e);
                }
            }
            Globals.Figures.Clear();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Files(*.bin)|*.bin";
            ofd.Title = "Открыть";
            ofd.ShowDialog();
            if (ofd.FileName != "")
            {
                Stream file = (FileStream)ofd.OpenFile();
                BinaryFormatter deserializer = new BinaryFormatter();
                Globals.Figures = (List<Figure>)deserializer.Deserialize(file);
                file.Close();
                Invalidate();
            }
            Globals.HistoryCanvas.Clear();
            Globals.StepNumber = 0;
            Globals.AddStep();
            gotoPastStep.IsEnabled = false;
            gotoSecondStep.IsEnabled = false;
        }

        private void gotoPastCondition_Click(object sender, RoutedEventArgs e)
        {
            Globals.gotoPastStep();
            if(Globals.StepNumber == 1)
            {
                gotoPastStep.IsEnabled = false;
            }
            gotoSecondStep.IsEnabled = true;
            Invalidate();
        }

        private void gotoNextStep_Click(object sender, RoutedEventArgs e)
        {
            Globals.gotoNextStep();
            if(Globals.StepNumber == Globals.HistoryCanvas.Count)
            { 
                gotoSecondStep.IsEnabled = false;
            }
            gotoPastStep.IsEnabled = true;
            Invalidate();
        }

        public void changeRoundX (object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            foreach (Figure figure in Globals.Figures)
            {
                if (figure.Select == true)
                {
                    figure.ChangeRoundX(e.NewValue);
                }
            }
            Invalidate();
        }

        public void changeRoundY(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            foreach (Figure figure in Globals.Figures)
            {
                if (figure.Select == true)
                {
                    figure.ChangeRoundY(e.NewValue);
                }
            }
            Invalidate();
        }

        public void ChangeStrokeColor(object sender, RoutedEventArgs e)
        {
            foreach(Figure figure in Globals.Figures)
            {
                if (figure.Select == true)
                {
                    figure.ChangePen(Globals.TransformColor[(sender as System.Windows.Controls.Button).Tag.ToString()], (sender as System.Windows.Controls.Button).Tag.ToString());
                }
            }
            Globals.AddStep();
            gotoPastStep.IsEnabled = true;
            gotoSecondStep.IsEnabled = false;
            Invalidate();
        }

        public void ChangeBrushColor(object sender, RoutedEventArgs e)
        {
            foreach (Figure figure in Globals.Figures)
            {
                if (figure.Select == true)
                {
                    figure.ChangePen(Globals.TransformColor[(sender as System.Windows.Controls.Button).Tag.ToString()], (sender as System.Windows.Controls.Button).Tag.ToString(), new bool());
                }
            }
            Globals.AddStep();
            gotoPastStep.IsEnabled = true;
            gotoSecondStep.IsEnabled = false;
            Invalidate();
        }

        public void ChangeDash(object sender, RoutedEventArgs e)
        {
            foreach (Figure figure in Globals.Figures)
            {
                if (figure.Select == true)
                {
                    figure.ChangePen(Globals.TransformDashProp[(sender as System.Windows.Controls.Button).Content.ToString()], (sender as System.Windows.Controls.Button).Content.ToString());
                }
            }
            Globals.AddStep();
            gotoPastStep.IsEnabled = true;
            gotoSecondStep.IsEnabled = false;
            Invalidate();
        }

        public void ClearSelectedFigure(object sender, RoutedEventArgs e)
        {
            foreach (Figure figure in Globals.Figures.ToArray())
            {
                if(figure.Select == true)
                {
                    Globals.Figures.Remove(figure);
                }
            }
            PropToolBarPanel.Children.Clear();
            Globals.AddStep();
            gotoPastStep.IsEnabled = true;
            gotoSecondStep.IsEnabled = false;
            Invalidate();
        }

        public void RowThicnessChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            foreach (Figure figure in Globals.Figures)
            {
                if (figure.Select == true)
                {
                    figure.ChangePen(e.NewValue);
                }
            }
            Invalidate();
        }

        public void HandForSelectedFigure(object sender, RoutedEventArgs e)
        {
            Globals.ToolNow = Globals.HandTool;
        }

        public void SldMouseUp(object sender, MouseButtonEventArgs e)
        {
            Globals.AddStep();
            gotoPastStep.IsEnabled = true;
            gotoSecondStep.IsEnabled = false;
        }

    }
}
