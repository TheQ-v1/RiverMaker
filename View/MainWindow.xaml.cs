using RiverMaker.Controllers;
using RiverMaker.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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

namespace RiverMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static double RIVER_WIDTH_COEFFICIENT = 3;
        public static double RIVER_DEFAULT_THICKNESS_TO_SHOW = 2;
        Bitmap bimage;
        System.Windows.Controls.Image image;
        GridStorageController cnt;
        public MainWindow()
        {
            InitializeComponent();
            image = new System.Windows.Controls.Image();
            bimage = new Bitmap(@"C:\Users\Nazar\Documents\Visual Studio 2015\Projects\RiverMaker\height2smallmod.PNG");
            cnt = new GridStorageController(25, bimage);
            cnt.LinkPoints();
            cnt.storage.DefineWaterIncome();
            //cnt.GetB(3000, bimage.Width, bimage.Height + 100);
            //var points = cnt.BuildStorage(200, bimage);
            //image = new System.Windows.Controls.Image();
            System.Windows.Media.Imaging.BitmapSource b = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bimage.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(bimage.Width, bimage.Height));
            image.Source = b;
            image.Stretch = Stretch.UniformToFill;
            //canvas.Children.Add(image);

        }

        private void imageSelectBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private System.Windows.Shapes.Rectangle getPointToDraw(Models.Point p)
        {
            System.Windows.Shapes.Rectangle res = new System.Windows.Shapes.Rectangle();
            res.Width = res.Height = 4;
            res.Fill = System.Windows.Media.Brushes.Red;
            res.SetValue(Canvas.LeftProperty, p.X * bimage.Width);
            res.SetValue(Canvas.TopProperty, p.Y * bimage.Height);
            return res;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            setCanvas();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void image_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
            foreach (var p in cnt.storage.points)
            {
                canvas.Children.Add(getPointToDraw(p.ThisPoint));
            }
        }

        private void setCanvas()
        {
            canvas.Children.Clear();
            canvas.Children.Add(image);
            if (this.ShowDotsCB.IsChecked == true)
            {
                foreach (var p in cnt.storage.points)
                {
                    canvas.Children.Add(getPointToDraw(p.ThisPoint));
                }
            }

            if(this.ShowThinChans.IsChecked == true)
            {
                makeRivers(0);
            }
            else
            {
                makeRivers(
                    ChansToShowThicknessTB.Text == "Channel thickness" ? 
                    RIVER_DEFAULT_THICKNESS_TO_SHOW :
                    double.Parse(ChansToShowThicknessTB.Text, CultureInfo.InvariantCulture)
                    );
            }

            if(this.showLongestRvrCB.IsChecked == true)
            {
                var start = cnt.storage.GetLongestRiverInStorage();
                Polyline p = new Polyline();
                
                while(start != null)
                {
                    p.Points.Add(
                    new System.Windows.Point(start.ThisPoint.X * bimage.Width, start.ThisPoint.Y * bimage.Height)
                    );
                    start = start.Output;
                }

                p.StrokeThickness = 4;
                p.Stroke = System.Windows.Media.Brushes.Red;

                canvas.Children.Add(p);
            }
        }

        private void makeRivers(double minimalWaterIncome)
        {
            Line l;
            PointStorageBase.LinkedPoint p;
            for(int i = 0; i < cnt.storage.pointsCount; ++i)
            {
                p = cnt.storage.points[i];
                if (p.ThisPoint.WaterIncome >= minimalWaterIncome && p.Output != null)
                {
                    l = new Line();
                    l.X1 = p.ThisPoint.X * bimage.Width;
                    l.Y1 = p.ThisPoint.Y * bimage.Height;

                    l.X2 = p.Output.ThisPoint.X * bimage.Width;
                    l.Y2 = p.Output.ThisPoint.Y * bimage.Height;

                    l.StrokeThickness = this.ignoreThicknessCB.IsChecked == true ?
                        1 : 
                        p.ThisPoint.WaterIncome * RIVER_WIDTH_COEFFICIENT;

                    l.Stroke = System.Windows.Media.Brushes.Aquamarine;

                    canvas.Children.Add(l);
                }

                if(p.ThisPoint.WaterIncome >= minimalWaterIncome &&
                    p.OutputSameLevel.Count != 0 &&
                    this.sameLvlCB.IsChecked == true)
                {
                    for (int k = 0; k < p.OutputSameLevel.Count; ++k)
                    {
                        l = new Line();
                        l.X1 = p.ThisPoint.X * bimage.Width;
                        l.Y1 = p.ThisPoint.Y * bimage.Height;

                        l.X2 = p.OutputSameLevel[k].ThisPoint.X * bimage.Width;
                        l.Y2 = p.OutputSameLevel[k].ThisPoint.Y * bimage.Height;

                        l.StrokeThickness = this.ignoreThicknessCB.IsChecked == true ?
                        1 :
                        p.ThisPoint.WaterIncome * RIVER_WIDTH_COEFFICIENT;

                        l.Stroke = System.Windows.Media.Brushes.Blue;

                        canvas.Children.Add(l);
                    }
                }
            }
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            if(PointsCountTB.Text != "Points count")
            {
                cnt = new GridStorageController(int.Parse(PointsCountTB.Text), bimage);
                cnt.LinkPoints();
                cnt.storage.DefineWaterIncome();
            }
            setCanvas();
        }
    }
}
