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
        private const int width = 795;
        private const int height = 795;

        private Bitmap bm;

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
            DisplayImage(bm);
        }

        private void LineThickChange(object sender, TextChangedEventArgs e)
        {
            int thick=1;
            if(Int32.TryParse(LineThickBox.Text, out thick) && thick % 2 != 0)
            {
                LineThickBox.Background = (Brush)bc.ConvertFrom(mylightGreen);
                LineThickValue = thick;

                if (SelectedShape != null && SelectedShape is Line)
                {
                    var line = (Line)SelectedShape;
                    layers.Remove(SelectedShape);
                    
                    layers.Add(new Line(line.P1, line.P2, LineThickValue));

                    SelectedShape = null;
                    SelectedObject.Text = "None";
                    RenderLayers();
                }
            }
            else
            {
                LineThickBox.Background = (Brush)bc.ConvertFrom(mylightRed);
            }
        }
        private void PolyThickChange(object sender, TextChangedEventArgs e)
        {
            int thick = 1;
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
            bm = new Bitmap(width, height);
            foreach (var shape in layers)
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
            clickCount = 0;

            lineDrawFlag = false;
            lineMoveFlag = false;
            circleDrawFlag = false;
            circleMoveFlag = false;
            circleMoveEPFlag = false;
            polyClickFlag = false;
            selectShapeFlag = false;
        }


        // ----- Flags -----

        private bool lineDrawFlag = false;
        private bool lineMoveFlag = false;

        private bool circleDrawFlag = false;
        private bool circleMoveFlag = false;
        private bool circleMoveEPFlag = false;

        private bool polyClickFlag = false;
        private bool selectShapeFlag = false;


        private void ImageMouseDown(object sender, MouseButtonEventArgs e)
        {
            int x = (int)e.GetPosition(myImage).X;
            int y = (int)e.GetPosition(myImage).Y;

            if (selectShapeFlag)
            {
                var p = new Point(x, y);
                CanvasPoints.Add(p);
                clickCount--;
                if (clickCount <= 0)
                {
                    SelectedShape = FindClosestShape(p);
                    CanvasPoints.Clear();
                    selectShapeFlag = false;
                    if (SelectedShape != null)
                    {
                        SelectedObject.Text = SelectedShape.GetNameAndCenter();                        
                    }
                    else
                    {
                        SelectedObject.Text = "None";
                    }
                }
            } // Select Handling
            if (lineDrawFlag)
            {
                CanvasPoints.Add(new Point(x, y));
                clickCount--;
                if (clickCount <= 0)
                {
                    layers.Add(new Line(CanvasPoints[0], CanvasPoints[1], LineThickValue));
                    CanvasPoints.Clear();
                    lineDrawFlag = false;
                    RenderLayers();
                }
            } // Line Creating 
            if (lineMoveFlag)
            {
                CanvasPoints.Add(new Point(x, y));
                clickCount--;
                if (clickCount <= 0)
                {
                    var line = (Line)SelectedShape;
                    var endPoint = line.FindClosestPoint(CanvasPoints[0]);
                    var otherEP = line.GetOtherPoint(endPoint);
                    layers.Remove(SelectedShape);
                    layers.Add(new Line(CanvasPoints[1], otherEP, line.Thickness));
                    
                    lineMoveFlag = false;
                    CanvasPoints.Clear();
                    SelectedShape = null;
                    SelectedObject.Text = "None";
                    RenderLayers();
                }
            } // Line Moving 
            if (circleDrawFlag)
            {
                CanvasPoints.Add(new Point(x, y));
                clickCount--;
                if (clickCount <= 0)
                {
                    layers.Add(new Circle(CanvasPoints[0], CanvasPoints[1]));
                    CanvasPoints.Clear();
                    circleDrawFlag = false;
                    RenderLayers();
                }
            } // Circle Creating 
            if (circleMoveFlag)
            {
                CanvasPoints.Add(new Point(x, y));
                clickCount--;
                if (clickCount <= 0)
                {
                    var cir = (Circle)SelectedShape;
                    var c = cir.GetCenter();
                    var ep = cir.EdgePoint;

                    System.Windows.Point p1 = new System.Windows.Point(c.X, c.Y);
                    System.Windows.Point p2 = new System.Windows.Point(CanvasPoints[0].X, CanvasPoints[0].Y);
                    System.Windows.Point p3 = new System.Windows.Point(ep.X, ep.Y);

                    Vector vectorFromCenter = System.Windows.Point.Subtract(p1, p2);
                    var p4 = System.Windows.Point.Subtract(p3, vectorFromCenter);

                    var newEp = new Point((int) p4.X, (int) p4.Y);


                    layers.Remove(SelectedShape);
                    layers.Add(new Circle(CanvasPoints[0], newEp));

                    circleMoveFlag = false;
                    CanvasPoints.Clear();
                    SelectedShape = null;
                    SelectedObject.Text = "None";
                    RenderLayers();
                }
            } // Circle Moving
            if (circleMoveEPFlag)
            {
                CanvasPoints.Add(new Point(x, y));
                clickCount--;
                if (clickCount <= 0)
                {
                    var cir = (Circle)SelectedShape;
                    var c = cir.Center;

                    layers.Remove(SelectedShape);
                    layers.Add(new Circle(c, CanvasPoints[0]));

                    circleMoveEPFlag = false;
                    CanvasPoints.Clear();
                    SelectedShape = null;
                    SelectedObject.Text = "None";
                    RenderLayers();
                }
            } // Circle Edgepoint Moving

        }

        // +----- Menu Buttons -----+

        // ----- Mouse Coordinates -----
        private void SelectButton(object sender, RoutedEventArgs e)
        {
            if (!selectShapeFlag)
            {
                ResetFlagsAndCP();
                selectShapeFlag = true;
                clickCount = 1;                
            }            
        }
        private void DeleteShapeButton(object sender, RoutedEventArgs e)
        {
            if (SelectedShape != null)
            {
                ResetFlagsAndCP();
                string name = "Shape";

                if (SelectedShape is Line) name = "Line";
                if (SelectedShape is Circle) name = "Circle";
                if (SelectedShape is Polygon) name = "Polygon";

                layers.Remove(SelectedShape);
                RenderLayers();
                SelectedObject.Text = $"Deleted {name}";
            }
        }

        // ----- Line -----
        private void DrawLineButton(object sender, RoutedEventArgs e)
        {
            if (!lineDrawFlag)
            {
                ResetFlagsAndCP();
                lineDrawFlag = true;
                clickCount = 2;                
            }
        }
        private void MoveLineEndpointButton(object sender, RoutedEventArgs e)
        {
            if (!lineMoveFlag && SelectedShape != null && SelectedShape is Line)
            {
                ResetFlagsAndCP();
                lineMoveFlag = true;
                clickCount = 2;
            }
        }

        // ----- Circle -----
        private void DrawCircleButton(object sender, RoutedEventArgs e)
        {
            if (!circleDrawFlag)
            {
                ResetFlagsAndCP();
                circleDrawFlag = true;
                clickCount = 2;
            }
        }
        private void MoveCircleButton(object sender, RoutedEventArgs e)
        {
            if (!circleMoveFlag && SelectedShape != null && SelectedShape is Circle)
            {
                ResetFlagsAndCP();
                circleMoveFlag = true;
                clickCount = 1;
            }
        }
        private void MoveCircleEP(object sender, RoutedEventArgs e)
        {
            if (!circleMoveEPFlag && SelectedShape != null && SelectedShape is Circle)
            {
                ResetFlagsAndCP();
                circleMoveEPFlag = true;
                clickCount = 1;
            }
        }






        private void ClearCanvasButton(object sender, RoutedEventArgs e)
        {
            layers.Clear();
            RenderLayers();
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
