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

namespace lecture_7_1_skeleton
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor sensor;
        Skeleton[] totalSkeleton = new Skeleton[6];
        Point mappedPoint2;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            this.sensor = KinectSensor.KinectSensors[0];
            this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
            this.sensor.SkeletonStream.Enable();
            this.sensor.SkeletonFrameReady += skeletonFrameReady;
            // start the sensor.
            this.sensor.Start();
        }


        void skeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame == null)
                {
                    return;
                }
                skeletonFrame.CopySkeletonDataTo(totalSkeleton);
                Skeleton firstSkeleton = (from trackskeleton in totalSkeleton
                                          where trackskeleton.TrackingState == SkeletonTrackingState.Tracked
                                          select trackskeleton).FirstOrDefault();
                if (firstSkeleton == null)
                {
                    return;
                }
                if (firstSkeleton.Joints[JointType.HandRight].TrackingState == JointTrackingState.Tracked)
                {
                    this.MapJointsWithUIElement(firstSkeleton);
                }
            }
        }

        private void MapJointsWithUIElement(Skeleton skeleton)
        {
            Point mappedPoint = ScalePosition(skeleton.Joints[JointType.HandRight].Position);
            Canvas.SetLeft(righthand, mappedPoint.X);
            Canvas.SetTop(righthand, mappedPoint.Y);
            this.textBox1.Text = "x="+mappedPoint.X+", y="+mappedPoint.Y;

            Line bone = new Line();
            bone.Stroke = Brushes.Red;
            bone.StrokeThickness = 10;

            bone.X1 = mappedPoint.X;
            bone.Y1 = mappedPoint.Y;

            bone.X2 = mappedPoint.X+1;
            bone.Y2 = mappedPoint.Y+1;

            if (bone.X1 > 500 && bone.Y1<50)
            {
                canvas1.Children.Clear();
            }
            if (bone.X1 > 500 && bone.Y1>200)
            {
                bone.Stroke = Brushes.Yellow;
            }



            canvas1.Children.Add(bone);

 
        }

        private Point ScalePosition(SkeletonPoint skeletonPoint)
        {
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.
                      MapSkeletonPointToDepthPoint(skeletonPoint, DepthImageFormat.
                                 Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }


        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.sensor != null && this.sensor.IsRunning)
            {
                this.sensor.Stop();
            }

        }


    }
}
