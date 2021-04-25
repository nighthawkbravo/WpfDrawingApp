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
using Bitmap = System.Drawing.Bitmap;
using System.IO;
using System.Drawing.Imaging;

namespace WpfAppComputerGraphics2
{
    public partial class MainWindow : Window
    {
        private const int width = 800;
        private const int height = 795;

        private Bitmap bm;
        private Bitmap blankBM;

        private List<IShape> layers;        
        private List<Point> CanvasPoints;
        private int clickCount = 0;

        private IShape SelectedShape = null;

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

            bm = new Bitmap(width, height);
            blankBM = new Bitmap(width, height);
            DisplayImage(bm);
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
                bm = shape.Render(bm);
            }
            DisplayImage(bm);
        }
        private void DisplayImage(Bitmap b)
        {
            myImage.Source = Bitmap2BitmapImage(b); ;
        }

        private IShape FindClosestShape(Point p)
        {
            IShape res = null;
            double sum = double.PositiveInfinity;

            foreach (var sh in layers)
            {
                var v = CalcEucliDist(p, sh.GetCenter());
                if (v < sum)
                {
                    sum = v; 
                    res = sh;
                }
            }

            return res;
        }
        private double CalcEucliDist(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.Y - p1.Y, 2) + Math.Pow(p2.X - p1.X, 2));
        }


        private void ResetFlagsAndCP()
        {
            CanvasPoints.Clear();

            lineClickFlag = false;
            circleClickFlag = false;
            polyClickFlag = false;
            selectClickFlag = false;
        }


        // ----- Flags -----

        private bool lineClickFlag = false;

        private bool circleClickFlag = false;
        private bool polyClickFlag = false;
        private bool selectClickFlag = false;


        private void ImageMouseDown(object sender, MouseButtonEventArgs e)
        {
            int x = (int)e.GetPosition(myImage).X;
            int y = (int)e.GetPosition(myImage).Y;

            if (selectClickFlag)
            {
                var p = new Point(x, y);
                CanvasPoints.Add(p);
                clickCount--;
                if (clickCount <= 0)
                {
                    SelectedShape = FindClosestShape(p);
                    CanvasPoints.Clear();
                    selectClickFlag = false;
                    if (SelectedShape != null)
                    {
                        SelectedObject.Text = SelectedShape.GetNameAndCenter();
                        //MessageBox.Show($"{SelectedShape.GetNameAndCenter()}");
                    }
                    else
                    {
                        SelectedObject.Text = "None";
                    }
                }
            } // Select Handling


            if (lineClickFlag)
            {
                CanvasPoints.Add(new Point(x, y));
                clickCount--;
                if (clickCount <= 0)
                {
                    layers.Add(new Line(CanvasPoints[0], CanvasPoints[1]));
                    CanvasPoints.Clear();
                    lineClickFlag = false;
                    RenderLayers();
                }
            } // Line Handling

        }

        // +----- Menu Buttons -----+

        // ----- Mouse Coordinates -----
        private void SelectButton(object sender, RoutedEventArgs e)
        {
            if (!selectClickFlag)
            {
                ResetFlagsAndCP();
                selectClickFlag = true;
                clickCount = 1;                
            }            
        }




        // ----- Line -----
        private void DrawLineButton(object sender, RoutedEventArgs e)
        {
            if (!lineClickFlag)
            {
                ResetFlagsAndCP();
                lineClickFlag = true;
                clickCount = 2;                
            }
        }




        


        private void ClearCanvasButton(object sender, RoutedEventArgs e)
        {
            layers.Clear();
            bm = blankBM;            
            DisplayImage(bm);
        }









        // Converts Bitmaps to BitmapImages.
        private BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        // Converts BitmapImages to Bitmaps
        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }
    }
}
