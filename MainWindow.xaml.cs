using System;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using WpfAppComputerGraphics2.Shapes;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using Bitmap = System.Drawing.Bitmap;
using ColorDialog = System.Windows.Forms.ColorDialog;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using System.Text;

namespace WpfAppComputerGraphics2
{
    public partial class MainWindow : Window
    {
        private const int width = 795;
        private const int height = 795;
        private const int PolyStopRadius = 7;

        private Bitmap bm;

        private List<IShape> layers;
        private List<Point> CanvasPoints;
        private int clickCount = 0;

        private IShape SelectedShape = null;
        private IShape cir = null;

        private int LineThickValue;
        private int PolyThickValue;
        private int RectThickValue;
        public bool AliasingFlag = false;

        private BrushConverter bc = new BrushConverter();
        private string mylightRed = "#FF5555";
        private string mylightGreen = "#42f548";

        private Color ChoosenColor = Color.Black;

        private string AllShapes;

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
            int thick = 1;
            if (Int32.TryParse(LineThickBox.Text, out thick) && thick % 2 != 0)
            {
                LineThickBox.Background = (Brush)bc.ConvertFrom(mylightGreen);
                LineThickValue = thick;

                if (SelectedShape != null && SelectedShape is Line)
                {
                    var line = (Line)SelectedShape;
                    layers.Remove(SelectedShape);

                    layers.Add(new Line(line.P1, line.P2, ChoosenColor, LineThickValue));

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
            if (Int32.TryParse(PolyThickBox.Text, out thick) && thick % 2 != 0)
            {
                PolyThickBox.Background = (Brush)bc.ConvertFrom(mylightGreen);
                PolyThickValue = thick;

                if (SelectedShape != null && SelectedShape is Polygon)
                {
                    var poly = (Polygon)SelectedShape;
                    List<Point> points = new List<Point>();

                    foreach (var po in poly.GetPoints())
                    {
                        points.Add(po);
                    }

                    layers.Remove(SelectedShape);

                    layers.Add(new Polygon(points, ChoosenColor, false, PolyThickValue));

                    SelectedShape = null;
                    SelectedObject.Text = "None";
                    RenderLayers();
                }
            }
            else
            {
                PolyThickBox.Background = (Brush)bc.ConvertFrom(mylightRed);
            }
        }
        private void RectThickChange(object sender, TextChangedEventArgs e)
        {
            int thick = 1;
            if (Int32.TryParse(RectThickBox.Text, out thick) && thick % 2 != 0)
            {
                RectThickBox.Background = (Brush)bc.ConvertFrom(mylightGreen);
                RectThickValue = thick;

                if (SelectedShape != null && SelectedShape is Rectangle)
                {
                    var rect = (Rectangle)SelectedShape;                    

                    layers.Remove(SelectedShape);

                    layers.Add(new Rectangle(rect.P1, rect.P2, ChoosenColor, false, RectThickValue));

                    SelectedShape = null;
                    SelectedObject.Text = "None";
                    RenderLayers();
                }
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
                bm = shape.Render(bm, AliasingFlag);
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
            polyDrawFlag = false;
            polyMoveFlag = false;
            polyMoveVertexFlag = false;
            polyMoveEdgeFlag = false;
            capsuleDrawFlag = false;
            selectShapeFlag = false;
            rectDrawFlag = false;
            rectMoveFlag = false;
            rectMoveVertexFlag = false;
            rectMoveEdgeFlag = false;
            floodflag = false;
        }

        // ----- Flags -----
        private bool lineDrawFlag = false;
        private bool lineMoveFlag = false;

        private bool circleDrawFlag = false;
        private bool circleMoveFlag = false;
        private bool circleMoveEPFlag = false;

        private bool polyDrawFlag = false;
        private bool polyMoveFlag = false;
        private bool polyMoveVertexFlag = false;
        private bool polyMoveEdgeFlag = false;

        private bool rectDrawFlag = false;
        private bool rectMoveFlag = false;
        private bool rectMoveVertexFlag = false;
        private bool rectMoveEdgeFlag = false;

        private bool capsuleDrawFlag = false;
        private bool floodflag = false;

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
                    layers.Add(new Line(CanvasPoints[0], CanvasPoints[1], ChoosenColor, LineThickValue));
                    lineDrawFlag = false;
                    EndStepOfMouseDown();
                }
            } // Line Drawing 
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
                    layers.Add(new Line(CanvasPoints[1], otherEP, ChoosenColor, line.Thickness));

                    lineMoveFlag = false;
                    EndStepOfMouseDown();
                }
            } // Line Moving 
            if (circleDrawFlag)
            {
                CanvasPoints.Add(new Point(x, y));
                clickCount--;
                if (clickCount <= 0)
                {
                    layers.Add(new Circle(CanvasPoints[0], CanvasPoints[1], ChoosenColor, false));
                    circleDrawFlag = false;
                    EndStepOfMouseDown();
                }
            } // Circle Drawing 
            if (circleMoveFlag)
            {
                CanvasPoints.Add(new Point(x, y));
                clickCount--;
                if (clickCount <= 0)
                {
                    var cir = (Circle)SelectedShape;
                    var c = cir.GetCenter();
                    var ep = cir.EdgePoint;
                    var f = cir.Fill;

                    System.Windows.Point p1 = new System.Windows.Point(c.X, c.Y);
                    System.Windows.Point p2 = new System.Windows.Point(CanvasPoints[0].X, CanvasPoints[0].Y);
                    System.Windows.Point p3 = new System.Windows.Point(ep.X, ep.Y);

                    Vector vectorFromCenter = System.Windows.Point.Subtract(p1, p2);
                    var p4 = System.Windows.Point.Subtract(p3, vectorFromCenter);

                    var newEp = new Point((int)p4.X, (int)p4.Y);


                    layers.Remove(SelectedShape);
                    layers.Add(new Circle(CanvasPoints[0], newEp, ChoosenColor, f));

                    circleMoveFlag = false;
                    EndStepOfMouseDown();
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
                    var f = cir.Fill;

                    layers.Remove(SelectedShape);
                    layers.Add(new Circle(c, CanvasPoints[0], ChoosenColor, f));

                    circleMoveEPFlag = false;
                    EndStepOfMouseDown();
                }
            } // Circle Edgepoint Moving
            if (polyDrawFlag)
            {
                if (CanvasPoints.Count == 0)
                {
                    CanvasPoints.Add(new Point(x, y));
                    cir = new Circle(new Point(x, y), new Point(x + PolyStopRadius, y), Color.Green, false);
                    layers.Add(cir);
                    RenderLayers();
                }
                else
                {
                    var p = new Point(x, y);
                    if (CalcEucliDist(p, CanvasPoints[0]) > PolyStopRadius)
                    {
                        CanvasPoints.Add(p);
                    }
                    else
                    {
                        List<Point> pts = new List<Point>();
                        foreach (var po in CanvasPoints)
                        {
                            pts.Add(po);
                        }
                        layers.Add(new Polygon(pts, ChoosenColor, false, PolyThickValue));
                        layers.Remove(cir);
                        EndStepOfMouseDown();
                    }
                }
            } // Polygon Drawing
            if (polyMoveFlag)
            {
                CanvasPoints.Add(new Point(x, y));
                clickCount--;
                if (clickCount <= 0)
                {
                    var poly = (Polygon)SelectedShape;
                    int t = poly.Thickness;
                    var c = poly.GetCenter();
                    var f = cir.Fill;

                    List<Point> newPoints = new List<Point>();

                    Vector vectorFromCenter = System.Windows.Point.Subtract(new System.Windows.Point(c.X, c.Y),
                                                                            new System.Windows.Point(CanvasPoints[0].X, CanvasPoints[0].Y));

                    foreach (var po in poly.GetPoints())
                    {
                        var tmpPo = new System.Windows.Point(po.X, po.Y);
                        var tmpPo2 = System.Windows.Point.Subtract(tmpPo, vectorFromCenter);

                        newPoints.Add(new Point((int)tmpPo2.X, (int)tmpPo2.Y));
                    }


                    layers.Remove(SelectedShape);
                    layers.Add(new Polygon(newPoints, ChoosenColor, f, t));

                    polyMoveFlag = false;
                    EndStepOfMouseDown();
                }
            } // Polygon Moving
            if (polyMoveVertexFlag)
            {
                CanvasPoints.Add(new Point(x, y));
                clickCount--;
                if (clickCount <= 0)
                {
                    var poly = (Polygon)SelectedShape;
                    var vertex = poly.FindClosestPoint(CanvasPoints[0]);
                    int t = poly.Thickness;
                    var f = cir.Fill;

                    List<Point> newPoints = new List<Point>();

                    foreach (var po in poly.GetPoints())
                    {
                        newPoints.Add(po);
                    }

                    newPoints[newPoints.FindIndex(i => i == vertex)] = CanvasPoints[1];

                    layers.Remove(SelectedShape);
                    layers.Add(new Polygon(newPoints, ChoosenColor, f, t));


                    polyMoveVertexFlag = false;
                    EndStepOfMouseDown();
                }
            } // Polygon Moving Vertex
            if (polyMoveEdgeFlag)
            {
                CanvasPoints.Add(new Point(x, y));
                clickCount--;
                if (clickCount <= 0)
                {
                    var poly = (Polygon)SelectedShape;
                    int t = poly.Thickness;
                    var Edge = poly.FindClosestEdge(CanvasPoints[0]);
                    var f = cir.Fill;

                    List<Point> newPoints = new List<Point>();

                    Vector vectorFromCenter = System.Windows.Point.Subtract(new System.Windows.Point(Edge.GetCenter().X, Edge.GetCenter().Y),
                                                                            new System.Windows.Point(CanvasPoints[1].X, CanvasPoints[1].Y));

                    var tmpPo = new System.Windows.Point(Edge.P1.X, Edge.P1.Y);
                    var tmpPo2 = System.Windows.Point.Subtract(tmpPo, vectorFromCenter);

                    var tmpPo3 = new System.Windows.Point(Edge.P2.X, Edge.P2.Y);
                    var tmpPo4 = System.Windows.Point.Subtract(tmpPo3, vectorFromCenter);

                    var res1 = new Point((int)tmpPo2.X, (int)tmpPo2.Y);
                    var res2 = new Point((int)tmpPo4.X, (int)tmpPo4.Y);

                    foreach (var po in poly.GetPoints())
                    {
                        newPoints.Add(po);
                    }

                    newPoints[newPoints.FindIndex(i => i == Edge.P1)] = res1;
                    newPoints[newPoints.FindIndex(i => i == Edge.P2)] = res2;

                    layers.Remove(SelectedShape);
                    layers.Add(new Polygon(newPoints, ChoosenColor, f, t));

                    polyMoveEdgeFlag = false;
                    EndStepOfMouseDown();
                }
            } // Polygon moving Edge
            if (capsuleDrawFlag)
            {
                CanvasPoints.Add(new Point(x, y));
                clickCount--;
                if (clickCount <= 0)
                {
                    layers.Add(new Capsule(CanvasPoints[0], CanvasPoints[1], CanvasPoints[2], ChoosenColor));
                    capsuleDrawFlag = false;
                    EndStepOfMouseDown();
                }
            } // Capsule Drawing 
            if (rectDrawFlag)
            {
                CanvasPoints.Add(new Point(x, y));
                clickCount--;
                if (clickCount <= 0)
                {
                    layers.Add(new Rectangle(CanvasPoints[0], CanvasPoints[1], ChoosenColor, false, RectThickValue));
                    rectDrawFlag = false;
                    EndStepOfMouseDown();
                }
            } // Rectangle Drawing 
            if (rectMoveFlag)
            {
                CanvasPoints.Add(new Point(x, y));
                clickCount--;
                if (clickCount <= 0)
                {
                    var rect = (Rectangle)SelectedShape;
                    var rectColor = rect.myColor;
                    int t = rect.Thickness;
                    var c = rect.GetCenter();
                    var f = cir.Fill;

                    Vector vectorFromCenter = System.Windows.Point.Subtract(new System.Windows.Point(c.X, c.Y),
                                                                            new System.Windows.Point(CanvasPoints[0].X, CanvasPoints[0].Y));

                    var tmpPo = new System.Windows.Point(rect.P1.X, rect.P1.Y);
                    var tmpPo2 = System.Windows.Point.Subtract(tmpPo, vectorFromCenter);

                    var tmpPo3 = new System.Windows.Point(rect.P2.X, rect.P2.Y);
                    var tmpPo4 = System.Windows.Point.Subtract(tmpPo3, vectorFromCenter);

                    layers.Remove(SelectedShape);
                    layers.Add(new Rectangle(new Point((int)tmpPo2.X, (int)tmpPo2.Y), new Point((int)tmpPo4.X, (int)tmpPo4.Y), rectColor, f, t));

                    rectMoveFlag = false;
                    EndStepOfMouseDown();
                }
            } // Rectangle Moving
            if (rectMoveVertexFlag)
            {
                CanvasPoints.Add(new Point(x, y));
                clickCount--;
                if (clickCount <= 0)
                {
                    var rect = (Rectangle)SelectedShape;
                    var vertex = rect.FindClosestPoint(CanvasPoints[0]);
                    int t = rect.Thickness;
                    var f = cir.Fill;

                    List<Point> newPoints = new List<Point>();

                    foreach (var po in rect.GetPoints())
                    {
                        newPoints.Add(po);
                    }

                    var neighbors = rect.FindNeighbors(vertex);

                    int xdif = (CanvasPoints[1].X - vertex.X);
                    int ydif = (CanvasPoints[1].Y - vertex.Y);

                    foreach (var p in neighbors)
                    {
                        if (p.X == vertex.X)
                        {
                            newPoints[newPoints.FindIndex(i => i == p)] = new Point(p.X + xdif, p.Y);
                        }
                        if (p.Y == vertex.Y)
                        {
                            newPoints[newPoints.FindIndex(i => i == p)] = new Point(p.X, p.Y + ydif);
                        }
                    }

                    newPoints[newPoints.FindIndex(i => i == vertex)] = CanvasPoints[1];

                    layers.Remove(SelectedShape);
                    layers.Add(new Rectangle(newPoints, ChoosenColor, f, t));

                    rectMoveVertexFlag = false;
                    EndStepOfMouseDown();
                }
            } // Rectangle Moving Vertex
            if (rectMoveEdgeFlag)
            {
                CanvasPoints.Add(new Point(x, y));
                clickCount--;
                if (clickCount <= 0)
                {
                    var rect = (Rectangle)SelectedShape;
                    int t = rect.Thickness;
                    var Edge = rect.FindClosestEdge(CanvasPoints[0]);
                    var f = cir.Fill;

                    List<Point> newPoints = new List<Point>();

                    foreach (var po in rect.GetPoints())
                    {
                        newPoints.Add(po);
                    }

                    Point res1;
                    Point res2;

                    if (Edge.P1.X == Edge.P2.X)
                    {
                        int xdif = (CanvasPoints[1].X - Edge.GetCenter().X);
                        res1 = new Point(Edge.P1.X+xdif, Edge.P1.Y);
                        res2 = new Point(Edge.P2.X+xdif, Edge.P2.Y);
                    }
                    else //if (Edge.P1.Y == Edge.P2.Y)
                    {
                        int ydif = (CanvasPoints[1].Y - Edge.GetCenter().Y);
                        res1 = new Point(Edge.P1.X, Edge.P1.Y+ydif);
                        res2 = new Point(Edge.P2.X, Edge.P2.Y+ydif);
                    }

                                      

                    newPoints[newPoints.FindIndex(i => i == Edge.P1)] = res1;
                    newPoints[newPoints.FindIndex(i => i == Edge.P2)] = res2;

                    layers.Remove(SelectedShape);
                    layers.Add(new Rectangle(newPoints, ChoosenColor, f, t));

                    rectMoveEdgeFlag = false;
                    EndStepOfMouseDown();
                }
            } // Rectangle Moving Edge
            if (floodflag)
            {
                CanvasPoints.Add(new Point(x, y));
                clickCount--;
                if (clickCount <= 0)
                {
                    FloodFillSim(CanvasPoints[0]);
                    floodflag = false;
                    CanvasPoints.Clear();
                    SelectedShape = null;
                    SelectedObject.Text = "None";
                }
            } // Rectangle Moving Edge

        }

        private void EndStepOfMouseDown()
        {
            CanvasPoints.Clear();
            SelectedShape = null;
            SelectedObject.Text = "None";
            RenderLayers();
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

        // ----- Polygon -----
        private void DrawPolyButton(object sender, RoutedEventArgs e)
        {
            if (!polyDrawFlag)
            {
                ResetFlagsAndCP();
                polyDrawFlag = true;
            }
        }
        private void MovePolygonButton(object sender, RoutedEventArgs e)
        {
            if (!polyMoveFlag && SelectedShape != null && SelectedShape is Polygon)
            {
                ResetFlagsAndCP();
                polyMoveFlag = true;
                clickCount = 1;
            }
        }
        private void MovePolyVertexButton(object sender, RoutedEventArgs e)
        {
            if (!polyMoveVertexFlag && SelectedShape != null && SelectedShape is Polygon)
            {
                ResetFlagsAndCP();
                polyMoveVertexFlag = true;
                clickCount = 2;
            }
        }
        private void MovePolyEdgeButton(object sender, RoutedEventArgs e)
        {
            if (!polyMoveEdgeFlag && SelectedShape != null && SelectedShape is Polygon)
            {
                ResetFlagsAndCP();
                polyMoveEdgeFlag = true;
                clickCount = 2;
            }
        }

        // ----- Rectangle -----
        private void DrawRectButton(object sender, RoutedEventArgs e)
        {
            if (!rectDrawFlag)
            {
                ResetFlagsAndCP();
                rectDrawFlag = true;
                clickCount = 2;
            }
        }
        private void MoveRectButton(object sender, RoutedEventArgs e)
        {
            if (!rectMoveFlag && SelectedShape != null && SelectedShape is Rectangle)
            {
                ResetFlagsAndCP();
                rectMoveFlag = true;
                clickCount = 1;
            }
        }
        private void MoveRectVertexButton(object sender, RoutedEventArgs e)
        {
            if (!rectMoveVertexFlag && SelectedShape != null && SelectedShape is Rectangle)
            {
                ResetFlagsAndCP();
                rectMoveVertexFlag = true;
                clickCount = 2;
            }
        }
        private void MoveRectEdgeButton(object sender, RoutedEventArgs e)
        {
            if (!rectMoveEdgeFlag && SelectedShape != null && SelectedShape is Rectangle)
            {
                ResetFlagsAndCP();
                rectMoveEdgeFlag = true;
                clickCount = 2;
            }
        }        

        // ----- Lab Assignment -----
        private void DrawCapsuleButton(object sender, RoutedEventArgs e)
        {
            if (!capsuleDrawFlag)
            {
                ResetFlagsAndCP();
                capsuleDrawFlag = true;
                clickCount = 3;
            }
        }

        private void FloodFill(object sender, RoutedEventArgs e)
        {
            if (!floodflag)
            {
                ResetFlagsAndCP();
                floodflag = true;
                clickCount = 1;
            }
        }

        private void FloodFillSim(Point sp)
        {
            foreach (var shape in layers)
            {
                bm = shape.Render(bm, AliasingFlag);
            }
            FloodFillA(sp.X, sp.Y);            
        }
        public void FloodFillA(int x, int y)
        {
            if (bm.GetPixel(x, y).A == Color.Black.A) return;

            Stack<Point> points = new Stack<Point>();
            points.Push(new Point(x, y));
            bm.SetPixel(x, y, Color.Black);

            while (points.Count > 0)
            {
                Point pt = points.Pop();
                if (pt.X > 0) CheckPoint(points, pt.X - 1, pt.Y, Color.Black);
                if (pt.Y > 0) CheckPoint(points, pt.X, pt.Y - 1, Color.Black);
                if (pt.X < Width - 1) CheckPoint(points, pt.X + 1, pt.Y, Color.Black);
                if (pt.Y < Height - 1) CheckPoint(points, pt.X, pt.Y + 1, Color.Black);
            }
            DisplayImage(bm);
        }
        private void CheckPoint(Stack<Point> points, int x, int y, Color nc)
        {
            var p = new Point(x, y);
            if(!IsValidCoor(p)) return;
            if (!IsValidColor(p)) return;

            points.Push(p);
            bm.SetPixel(x, y, nc);
        }
        private bool IsValidColor(Point p)
        {
            return bm.GetPixel(p.X, p.Y).A != Color.Black.A;
        }
        private bool IsValidCoor(Point p)
        {
            if (p.X > 0 && p.X < width-1 && p.Y > 0 && p.Y < height-1) return true;
            return false;
        }

        
        
        


        // ----- More menu stuff -----

        private void FillShapeButton(object sender, RoutedEventArgs e)
        {
            switch (SelectedShape)
            {
                case Rectangle rect:
                    if (rect.Fill)
                    {
                        var r = new Rectangle(rect.GetPoints(), rect.myColor, false, rect.Thickness);

                        layers.Remove(SelectedShape);
                        layers.Add(r);
                    }
                    else
                    {
                        var r = new Rectangle(rect.GetPoints(), rect.myColor, true, rect.Thickness);
                        layers.Remove(SelectedShape);
                        layers.Add(r);
                    }
                    break;
                case Polygon poly:
                    if (poly.Fill)
                    {
                        var p = new Polygon(poly.GetPoints(), poly.myColor, false, poly.Thickness);

                        layers.Remove(SelectedShape);
                        layers.Add(p);
                    }
                    else
                    {
                        var p = new Polygon(poly.GetPoints(), poly.myColor, true, poly.Thickness);                        
                        layers.Remove(SelectedShape);
                        layers.Add(p);
                    }
                    break;
                case Circle cir:
                    if(cir.Fill)
                    {
                        var c = new Circle(cir.Center, cir.EdgePoint, cir.myColor, false);                        

                        layers.Remove(SelectedShape);
                        layers.Add(c);
                    }
                    else
                    {
                        var c = new Circle(cir.Center, cir.EdgePoint, cir.myColor, true);
                        layers.Remove(SelectedShape);
                        layers.Add(c);
                    }

                    break;
                default:
                    return;
            }
            RenderLayers();
            SelectedShape = null;
            SelectedObject.Text = "None";
        }

        private void SwitchAntiAliasingButton(object sender, RoutedEventArgs e)
        {
            if(AliasingFlag)
            {
                AliasingFlag = false;
                AntiAText.Text = "OFF Anti-Aliasing";
            }
            else
            {
                AliasingFlag = true;
                AntiAText.Text = "ON Anti-Aliasing";
                
            }
            RenderLayers();
        }

        private void SelectColorButton(object sender, RoutedEventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //ChoosenColor
                var mediacolor = System.Windows.Media.Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B);

                ColorButton.Foreground = new SolidColorBrush(mediacolor);
                ChoosenColor = System.Drawing.Color.FromArgb(mediacolor.A, mediacolor.R, mediacolor.G, mediacolor.B);
            }
        }
        private void ClearCanvasButton(object sender, RoutedEventArgs e)
        {
            layers.Clear();
            RenderLayers();
        }
        private void SaveVectorsButton(object sender, RoutedEventArgs e)
        {
            AllShapes = "BEGIN\n";
            foreach(var sh in layers)
            {
                AllShapes = AllShapes + $"{sh.Save()}\n";
            }
            AllShapes = AllShapes + "END";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) File.WriteAllText(saveFileDialog.FileName, AllShapes);
        }
        private void LoadVectorsButton(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) AllShapes = File.ReadAllText(openFileDialog.FileName);


            string firstline, line = "";
            StringBuilder sb;
            StringReader strReader = new StringReader(AllShapes);

            firstline = strReader.ReadLine();
            if(firstline != "BEGIN")
            {
                MessageBox.Show("File Not Formated");
                return;
            }
            while (true)
            {
                line = strReader.ReadLine();
                sb = new StringBuilder(line);
                if (sb.ToString(0, 3) == "BEG") continue;
                if (sb.ToString(0, 3) == "END") break;


                if (sb.ToString(0, 4) == "Line")
                {
                    List<int> nums = new List<int>();

                    int i;
                    string tmp = "";
                    for (i = 4; sb[i] != ':'; i++)
                    {
                        if(sb[i] != ',')
                        {
                            tmp += sb[i];
                        }
                        else
                        {
                            nums.Add(Int32.Parse(tmp));
                            tmp = "";
                        }
                    }
                    i++;
                    for (; sb[i] != ';'; i++)
                    {
                        if (sb[i] != ',')
                        {
                            tmp += sb[i];
                        }
                        else
                        {
                            nums.Add(Int32.Parse(tmp));
                            tmp = "";
                        }
                    }
                    layers.Add(new Line(new Point(nums[4], nums[5]), new Point(nums[6], nums[7]), Color.FromArgb(255, nums[1], nums[2], nums[3]), nums[0]));
                }
                if (sb.ToString(0, 6) == "Circle")
                {
                    List<int> nums = new List<int>();

                    int i;
                    string tmp = "";
                    for (i = 6; sb[i] != ':'; i++)
                    {
                        if (sb[i] != ',')
                        {
                            tmp += sb[i];
                        }
                        else
                        {
                            nums.Add(Int32.Parse(tmp));
                            tmp = "";
                        }
                    }
                    i++;
                    for (; sb[i] != ';'; i++)
                    {
                        if (sb[i] != ',')
                        {
                            tmp += sb[i];
                        }
                        else
                        {
                            nums.Add(Int32.Parse(tmp));
                            tmp = "";
                        }
                    }
                    layers.Add(new Circle(new Point(nums[4], nums[5]), new Point(nums[6], nums[7]), Color.FromArgb(255, nums[0], nums[1], nums[2]), Num2Bool(nums[3])));
                }
                if (sb.ToString(0, 7) == "Polygon")
                {
                    List<int> lenAndRGBs = new List<int>(5);
                    List<int> coords = new List<int>();
                    List<Point> pts = new List<Point>();



                    int i;
                    string tmp = "";
                    for (i = 7; sb[i] != ':'; i++)
                    {
                        if (sb[i] != ',')
                        {
                            tmp += sb[i];
                        }
                        else
                        {
                            lenAndRGBs.Add(Int32.Parse(tmp));
                            tmp = "";
                        }
                    }
                    i++;
                    for (; sb[i] != ';'; i++)
                    {
                        if (sb[i] != ',')
                        {
                            tmp += sb[i];
                        }
                        else
                        {
                            coords.Add(Int32.Parse(tmp));
                            tmp = "";
                        }
                    }


                    for (int j = 0; j < coords.Count - 1; j+=2)
                    {
                        pts.Add(new Point(coords[j], coords[j + 1]));
                    }
                    layers.Add(new Polygon(pts, Color.FromArgb(255, lenAndRGBs[1], lenAndRGBs[2], lenAndRGBs[3]), Num2Bool(lenAndRGBs[4])));
                }
                if (sb.ToString(0, 4) == "Rect")
                {
                    List<int> nums = new List<int>();

                    int i;
                    string tmp = "";
                    for (i = 4; sb[i] != ':'; i++)
                    {
                        if (sb[i] != ',')
                        {
                            tmp += sb[i];
                        }
                        else
                        {
                            nums.Add(Int32.Parse(tmp));
                            tmp = "";
                        }
                    }
                    i++;
                    for (; sb[i] != ';'; i++)
                    {
                        if (sb[i] != ',')
                        {
                            tmp += sb[i];
                        }
                        else
                        {
                            nums.Add(Int32.Parse(tmp));
                            tmp = "";
                        }
                    }
                    layers.Add(new Rectangle(new Point(nums[5], nums[6]), new Point(nums[7], nums[8]), Color.FromArgb(255, nums[1], nums[2], nums[3]), Num2Bool(nums[4]), nums[0]));                    
                }
            }
            RenderLayers();
        }

        private bool Num2Bool(int i)
        {
            if (i == 1) return true;
            return false;
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
