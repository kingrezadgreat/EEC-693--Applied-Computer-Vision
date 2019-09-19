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
using Microsoft.Kinect;

namespace lecture_6_depth_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor sensor;
        private WriteableBitmap depthBitmap;
        private short[] depthPixels;
        private int frameWidth; // to calculate depthIndex


        public MainWindow()
        {
            InitializeComponent();
        }
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            if (KinectSensor.KinectSensors.Count > 0)
            {
                this.sensor = KinectSensor.KinectSensors[0];
                if (this.sensor != null && !this.sensor.IsRunning)
                {
                    this.sensor.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
                    this.depthPixels = new short[this.sensor.DepthStream.FramePixelDataLength];
                    this.depthBitmap = new WriteableBitmap(this.sensor.DepthStream.FrameWidth, this.sensor.DepthStream.FrameHeight, 96.0, 96.0, PixelFormats.Gray16, null);

                    this.image1.Source = this.depthBitmap;
                    this.sensor.DepthFrameReady += this.depthFrameReady;
                    this.sensor.Start();
                }
                else
                {
                    MessageBox.Show("No device is connected!");
                    this.Close();
                }
            }
        }
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.sensor != null && this.sensor.IsRunning)
            {
                this.sensor.Stop();
            }

        }

        void depthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            using (DepthImageFrame imageFrame = e.OpenDepthImageFrame())
            {
                if (null == imageFrame)
                {
                    return;
                }
                this.frameWidth = imageFrame.Width;
                this.maxDepthField.Text = "" + imageFrame.MaxDepth;
                this.minDepthField.Text = "" + imageFrame.MinDepth;

                imageFrame.CopyPixelDataTo(depthPixels);
                int stride = imageFrame.Width * imageFrame.BytesPerPixel;
                this.depthBitmap.WritePixels(
                    new Int32Rect(0, 0, this.depthBitmap.PixelWidth, this.depthBitmap.PixelHeight),
                    this.depthPixels, stride, 0);
            }
        }

        private void image1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point currentPoint = e.GetPosition(image1);
            this.pixelXField.Text = currentPoint.X.ToString();
            this.pixelYField.Text = currentPoint.Y.ToString();
            int pixelIndex = (int)(currentPoint.X + ((int)currentPoint.Y * this.frameWidth));
            this.depthIndexField.Text = "" + pixelIndex;
            int distancemm = this.depthPixels[pixelIndex] >> DepthImageFrame.PlayerIndexBitmaskWidth;
            this.depthField.Text = "" + distancemm;
        }

    }
}
