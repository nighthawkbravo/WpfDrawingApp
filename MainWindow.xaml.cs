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
using WpfAppComputerGraphics2.Shapes;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;


namespace WpfAppComputerGraphics2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // The BitmapImage that is being drawn on.
        private BitmapImage bmi;

        private List<IShape> layers;

        private bool lineClickFlag = false;
        private int lineClickNum = 0;

        private bool circleClickFlag = false;
        private bool polyClickFlag = false;
        private List<Point> CanvasPoints;

        private int LineThickValue;
        private int PolyThickValue;

        private BrushConverter bc = new BrushConverter();
        private string mylightRed = "#FF5555";
        private string mylightGreen = "#42f548";

        public MainWindow()
        {
            InitializeComponent();
            layers = new List<IShape>();
            CanvasPoints = new List<Point>();
        }

        private void LineThickChange(object sender, TextChangedEventArgs e)
        {
            int thick=0;
            if(Int32.TryParse(LineThickBox.Text, out thick) && thick % 2 != 0)
            {
                LineThickBox.Background = (Brush)bc.ConvertFrom(mylightGreen);
                LineThickValue = thick;
            }
            else
            {
                LineThickBox.Background = (Brush)bc.ConvertFrom(mylightRed);
            }
        }
        private void PolyThickChange(object sender, TextChangedEventArgs e)
        {
            int thick = 0;
            if (Int32.TryParse(LineThickBox.Text, out thick) && thick % 2 != 0)
            {
                PolyThickBox.Background = (Brush)bc.ConvertFrom(mylightGreen);
                PolyThickValue = thick;
            }
            else
            {
                PolyThickBox.Background = (Brush)bc.ConvertFrom(mylightRed);
            }
        }
        private void ImageMouseMove(object sender, MouseEventArgs e)
        {
            var p = e.GetPosition(myImage);
            MouseXText.Text = $"X: {p.X:0.}";
            MouseYText.Text = $"Y: {p.Y:0.}";
        }
        private void ImageMouseLeave(object sender, MouseEventArgs e)
        {
            MouseXText.Text = $"X: -";
            MouseYText.Text = $"Y: -";
        }
        private void RenderLayers()
        {
            foreach(var shape in layers)
            {
                bmi = shape.Render(bmi);
            }
            DisplayImageFromBMI(bmi);
            //MessageBox.Show("Rendering");
        }
        private void DisplayImageFromBMI(BitmapImage bmi)
        {
            myImage.Source = bmi;
        }
        private void ResetFlagsAndCP()
        {
            CanvasPoints.Clear();

            lineClickFlag = false;
            circleClickFlag = false;
            polyClickFlag = false;
        }


        private void ImageMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (lineClickFlag)
            {
                int x = (int)e.GetPosition(myImage).X;
                int y = (int)e.GetPosition(myImage).Y;
                CanvasPoints.Add(new Point(x, y));
                lineClickNum--;
                if (lineClickNum <= 0)
                {
                    layers.Add(new Line(CanvasPoints[0], CanvasPoints[1]));
                    //MessageBox.Show($"{CanvasPoints[0]} - {CanvasPoints[1]}");
                    CanvasPoints.Clear();
                    lineClickFlag = false;
                    RenderLayers();
                }
            } // Line Handling
        }

        // ----- Menu Buttons -----

        // ----- Line -----
        private void DrawLineButton(object sender, RoutedEventArgs e)
        {
            if (!lineClickFlag)
            {
                ResetFlagsAndCP();
                lineClickFlag = true;
                lineClickNum = 2;                
            }
        }







        private void ClearCanvasButton(object sender, RoutedEventArgs e)
        {
            layers.Clear();
            DisplayImageFromBMI(null);
        }
    }
}
