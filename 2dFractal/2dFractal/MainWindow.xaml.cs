using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Image = System.Windows.Controls.Image;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Rectangle = System.Drawing.Rectangle;

namespace _2dFractal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CreateAndShowMainWindow();
        }

        private void CreateAndShowMainWindow()
        {
            // Create the application's main window
            //mainWindow = new Window();
            //mainWindow.Title = "Writeable Bitmap";
            mainWindow.Height = _height;
            mainWindow.Width = _width;
            mainWindow.ResizeMode = ResizeMode.CanMinimize;

            // Define the Image element
            _random.Stretch = Stretch.None;
            _random.Margin = new Thickness(0);

            // Define a StackPanel to host Controls
            StackPanel myStackPanel = new StackPanel();
            myStackPanel.Orientation = Orientation.Vertical;
            myStackPanel.Height = _height;
            myStackPanel.Width = _width;
            myStackPanel.VerticalAlignment = VerticalAlignment.Top;
            myStackPanel.HorizontalAlignment = HorizontalAlignment.Center;

            // Add the Image to the parent StackPanel
            myStackPanel.Children.Add(_random);

            // Add the StackPanel as the Content of the Parent Window Object
            mainWindow.Content = myStackPanel;
            mainWindow.Show();
            dispatcherTimer_Tick(null, null);
            // DispatcherTimer setup
            // The DispatcherTimer will be used to update _random every
            //    second with a new random set of colors.
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            //dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.IsEnabled = true;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
        //  System.Windows.Threading.DispatcherTimer.Tick handler
        //
        //  Updates the Image element with new random colors
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //Update the color array with new random colors
            //Random value = new Random();
            //value.NextBytes(_colorArray);

            var calcService = new CalculationService(_width, _height);

            //calcService.GetPointColour();

            var bytesToDraw = new byte[_arraySize];

            var pixels = new byte[_width][];
            
            var pixelss = new ConcurrentDictionary<Point, Color>();

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    var point = new Point(i, j);
                    pixelss.TryAdd(point, calcService.GetPointColour(point));
                }
            }

            var byteCounter = 0;

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    var point = new Point(i, j);
                    bytesToDraw[byteCounter] = pixelss[point].B;
                    byteCounter++;
                    bytesToDraw[byteCounter] = pixelss[point].G;
                    byteCounter++;
                    bytesToDraw[byteCounter] = pixelss[point].R;
                    byteCounter++;
                    bytesToDraw[byteCounter] = pixelss[point].A;
                    byteCounter++;
                }
            }
            
            //Update writeable bitmap with the colorArray to the image.
            _wb.WritePixels(_rect, bytesToDraw, _stride, 0);
            
            //Set the Image source.
            _random.Source = _wb;

        }

        private const int _width = 600;
        private const int _height = 600;
        
        private Image _random = new Image();
        // Create the writeable bitmap will be used to write and update.
        private static WriteableBitmap _wb =
            new WriteableBitmap(_width, _height, 96, 96, PixelFormats.Bgra32, null);
        // Define the rectangle of the writeable image we will modify. 
        // The size is that of the writeable bitmap.
        private static Int32Rect _rect = new Int32Rect(0, 0, _wb.PixelWidth, _wb.PixelHeight);

        // Calculate the number of bytes per pixel. 
        private static int _bytesPerPixel = (_wb.Format.BitsPerPixel + 7) / 8;
        // Stride is bytes per pixel times the number of pixels.
        // Stride is the byte width of a single rectangle row.
        private static int _stride = _wb.PixelWidth * _bytesPerPixel;

        // Create a byte array for a the entire size of bitmap.
        private static int _arraySize = _stride * _wb.PixelHeight;
        private static byte[] _colorArray = new byte[_arraySize];
    }
}