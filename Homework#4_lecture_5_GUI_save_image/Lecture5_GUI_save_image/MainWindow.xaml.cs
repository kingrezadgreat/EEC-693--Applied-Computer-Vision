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
using System.IO;
using System.Windows.Threading;

namespace Lecture5_GUI_save_image
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor sensor;
        private WriteableBitmap colorBitmap;
        private byte[] colorPixels;

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
                    this.sensor.ColorStream.Enable();
                    this.colorPixels = new byte[this.sensor.ColorStream.FramePixelDataLength];
                    this.colorBitmap = new WriteableBitmap(this.sensor.ColorStream.FrameWidth, this.sensor.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);
                    this.image1.Source = this.colorBitmap;
                    this.sensor.ColorFrameReady += this.colorFrameReady;
                    this.sensor.Start();
                }
                else
                {
                    MessageBox.Show("No device is connected!"); this.Close();
                }
            }
        }

        void colorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame imageFrame = e.OpenColorImageFrame())
            {
                if (null == imageFrame)
                    return;

                imageFrame.CopyPixelDataTo(colorPixels);
                int stride = imageFrame.Width * imageFrame.BytesPerPixel;

                // Write the pixel data into our bitmap
                this.colorBitmap.WritePixels(
                    new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight),
                        this.colorPixels, stride, 0);
            }
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.sensor != null && this.sensor.IsRunning)
            {
                this.sensor.Stop();
            }
        }




        private void ChangeToHighResolution(object sender, RoutedEventArgs e)
        {
            if (this.sensor != null)
            {
                this.normalresolution.IsChecked = false;
                this.sensor.ColorStream.Enable(ColorImageFormat.RgbResolution1280x960Fps12);
                this.colorPixels = new byte[this.sensor.ColorStream.FramePixelDataLength];
            }
        }

        private void ChangeToNormalResolution(object sender, RoutedEventArgs e)
        {
            if (this.sensor != null)
            {
                this.highresolution.IsChecked = false;
                this.sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                this.colorPixels = new byte[this.sensor.ColorStream.FramePixelDataLength];
            }
        }

        private void IncrementAngle(object sender, RoutedEventArgs e)
        {
            int angleValue = this.sensor.ElevationAngle + 1;
            if (angleValue < sensor.MaxElevationAngle)
            {
                this.sensor.ElevationAngle = angleValue;
                this.AngleBox.Text = "" + angleValue;
            }
        }

        private void DecrementAngle(object sender, RoutedEventArgs e)
        {
            int angleValue = this.sensor.ElevationAngle - 1;
            if (angleValue > sensor.MinElevationAngle) 
            {
                this.sensor.ElevationAngle = angleValue;
                this.AngleBox.Text = "" + angleValue;
            }
        }


        private void SaveImage1(object sender, RoutedEventArgs e)
        {
            SaveImage();
        }

        private void SaveImage()
        {
            using (FileStream fileStream = new FileStream(string.Format("{0}.jpg", Guid.NewGuid().ToString()), System.IO.FileMode.Create))
            {
                BitmapSource imageSource = (BitmapSource)image1.Source;
                JpegBitmapEncoder jpegEncoder = new JpegBitmapEncoder();
                jpegEncoder.Frames.Add(BitmapFrame.Create(imageSource));
                jpegEncoder.Save(fileStream);
                fileStream.Close();
            }
        }

        private void PeriodicSavingChanged(object sender, RoutedEventArgs e)
        {
            if (checkBox1.IsChecked == true)
            {
                StartTimer();
            }
            else {
                this.timer.Stop();
            }
        }

        private DispatcherTimer timer = new DispatcherTimer();
        public void StartTimer()
        {
            this.timer.Interval = new TimeSpan(0, 0, 10);
            this.timer.Start(); this.timer.Tick += this.handleTickEvent;
        }

        public void StopTimer()
        {
            this.timer.Stop(); this.timer.Tick -= this.handleTickEvent;
        }

        public void handleTickEvent(object sender, object e)
        {
            if (this.sensor.IsRunning && this.sensor.ColorStream.IsEnabled)
            {
                this.SaveImage();
            }
        }


    }
}
