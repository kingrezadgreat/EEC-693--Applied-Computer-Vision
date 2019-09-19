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

        private WriteableBitmap colorBitmap;
        private byte[] colorPixels;

        private int frameWidth; // to calculate depthIndex
        int stride;



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
                    this.sensor.ColorStream.Enable();


                    this.depthPixels = new short[this.sensor.DepthStream.FramePixelDataLength];
                    this.depthBitmap = new WriteableBitmap(this.sensor.DepthStream.FrameWidth, this.sensor.DepthStream.FrameHeight, 96.0, 96.0, PixelFormats.Gray16, null);


                    this.colorPixels = new byte[this.sensor.DepthStream.FramePixelDataLength * 4];
                    this.colorBitmap = new WriteableBitmap(this.sensor.DepthStream.FrameWidth, this.sensor.DepthStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

                    this.image1.Source = this.colorBitmap;

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

                //stride = imageFrame.Width * imageFrame.BytesPerPixel;
                stride = imageFrame.Width * 4;

                Console.WriteLine(depthPixels.Max());

                for (int i = 0; i < depthPixels.Length; i++)
                {
                    /*
                    if (depthPixels[i] < 1000)
                    {
                        this.colorPixels[4 * i] = 50;
                        this.colorPixels[4 * i + 1] = 50;
                        this.colorPixels[4 * i + 2] = 50;
                        this.colorPixels[4 * i + 3] = 0;
                    }

                    if (depthPixels[i] >= 1000)
                    {
                        this.colorPixels[4 * i] = 255;
                        this.colorPixels[4 * i + 1] = 51;
                        this.colorPixels[4 * i + 2] = 255;
                        this.colorPixels[4 * i + 3] = 0;
                    }
                    */



                    
                    if (depthPixels[i] < 2000)
                    {
                        this.colorPixels[4 * i] = 50;
                        this.colorPixels[4 * i + 1] = 50;
                        this.colorPixels[4 * i + 2] = 50;
                        this.colorPixels[4 * i + 3] = 0;
                    }

                    if (depthPixels[i] >= 2000  && depthPixels[i] < 5000)
                    {
                        this.colorPixels[4 * i] = 200;
                        this.colorPixels[4 * i + 1] = 200;
                        this.colorPixels[4 * i + 2] = 200;
                        this.colorPixels[4 * i + 3] = 0;
                    }

                    
                    if (depthPixels[i] >= 5000 && depthPixels[i] < 10000)
                    {
                        this.colorPixels[4 * i] = 200;
                        this.colorPixels[4 * i + 1] = 153;
                        this.colorPixels[4 * i + 2] = 0;
                        this.colorPixels[4 * i + 3] = 0;
                    }

                    if (depthPixels[i] >= 10000 && depthPixels[i] < 15000)
                    {
                        this.colorPixels[4 * i] = 76;
                        this.colorPixels[4 * i + 1] = 153;
                        this.colorPixels[4 * i + 2] = 0;
                        this.colorPixels[4 * i + 3] = 0;
                    }

                    if (depthPixels[i] >= 15000 && depthPixels[i] < 20000)
                    {
                        this.colorPixels[4 * i] = 0;
                        this.colorPixels[4 * i + 1] = 255;
                        this.colorPixels[4 * i + 2] = 255;
                        this.colorPixels[4 * i + 3] = 0;
                    }

                    if (depthPixels[i] >= 20000 && depthPixels[i] < 25000)
                    {
                        this.colorPixels[4 * i] = 0;
                        this.colorPixels[4 * i + 1] = 0;
                        this.colorPixels[4 * i + 2] = 255;
                        this.colorPixels[4 * i + 3] = 0;
                    }

                    if (depthPixels[i] >= 25000)
                    {
                        this.colorPixels[4 * i] = 255;
                        this.colorPixels[4 * i + 1] = 51;
                        this.colorPixels[4 * i + 2] = 255;
                        this.colorPixels[4 * i + 3] = 0;
                    }
                    




                }
                
                this.colorBitmap.WritePixels(
                        new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight),
                        this.colorPixels, stride, 0);
                        
                
                /*
                this.colorBitmap.WritePixels(
                    new Int32Rect(0, 0, this.depthBitmap.PixelWidth, this.depthBitmap.PixelHeight),
                    this.colorPixels, stride, 0);
                    */
                    



                //colorConvert(depthPixels);
                /*
                this.depthBitmap.WritePixels(
                    new Int32Rect(0, 0, this.depthBitmap.PixelWidth, this.depthBitmap.PixelHeight),
                    this.depthPixels, stride, 0);
                    */
            }
        }

        
        private void colorConvert(short[] grey)
        {
            this.colorPixels = new byte[grey.Length*4];
            Console.WriteLine(grey.Max());
            for (int i=0; i< grey.Length; i++)
            {
                if (grey[i] < 1000)
                {
                    this.colorPixels[4*i] = 50;
                    this.colorPixels[4*i+1] = 50;
                    this.colorPixels[4*i+2] = 50;
                    this.colorPixels[4*i+3] = 0;
                }

                if (grey[i] >= 1000 && grey[i] < 2000)
                {
                    this.colorPixels[4 * i] = 200;
                    this.colorPixels[4 * i + 1] = 200;
                    this.colorPixels[4 * i + 2] = 200;
                    this.colorPixels[4 * i + 3] = 0;
                }

                if (grey[i] >= 2000 && grey[i] < 3000)
                {
                    this.colorPixels[4 * i] = 200;
                    this.colorPixels[4 * i + 1] = 200;
                    this.colorPixels[4 * i + 2] = 200;
                    this.colorPixels[4 * i + 3] = 0;
                }

                //this.sensor.DepthFrameReady += this.depthFrameReady;
                //this.sensor.ColorFrameReady;
                //int stride = imageFrame.Width * imageFrame.BytesPerPixel;
                Console.WriteLine(stride);

                this.colorBitmap.WritePixels(
                    new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight),
                    this.colorPixels, stride, 0);




            }

        }
        

        void colorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            //Console.WriteLine("hello");
            using (ColorImageFrame imageFrame = e.OpenColorImageFrame())
            {
                if (null == imageFrame)
                    return;

                imageFrame.CopyPixelDataTo(colorPixels);
                int stride = imageFrame.Width * imageFrame.BytesPerPixel;
                Console.WriteLine(imageFrame.BytesPerPixel);

                // Write the pixel data into our bitmap
                this.colorBitmap.WritePixels(
                    new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight),
                        this.colorPixels, stride, 0);
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
