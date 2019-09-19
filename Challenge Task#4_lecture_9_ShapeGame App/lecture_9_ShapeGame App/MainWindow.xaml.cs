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

namespace lecture_9_ShapeGame_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor sensor;
        Skeleton[] totalSkeleton = new Skeleton[6];
        WriteableBitmap colorBitmap;
        byte[] colorPixels;
        Skeleton skeleton;
        Thing thing = new Thing(); // a struct for ball
        double gravity = 0.017;
        int currentSkeletonID = 0;
        int count = 0;
        int flag_release = 0;


        public MainWindow()
        {
            InitializeComponent();
        }

        private struct Thing
        {
            public System.Windows.Point Center;
            public double YVelocity;
            public double XVelocity;
            public Ellipse Shape;
            public bool Hit(System.Windows.Point joint)
            {
                double minDxSquared = this.Shape.RenderSize.Width;
                minDxSquared *= minDxSquared;
                double dist = SquaredDistance(Center.X, Center.Y, joint.X, joint.Y);
                if (dist <= minDxSquared)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        private static double SquaredDistance(double x1, double y1, double x2, double y2)
        {
            return ((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1));
        }


        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            this.sensor = KinectSensor.KinectSensors[0];
            this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
            this.sensor.SkeletonStream.Enable();
            this.sensor.SkeletonFrameReady += skeletonFrameReady;
            // start the sensor.
            this.sensor.Start();

            thing.Shape = new Ellipse();
            thing.Shape.Width = 30; thing.Shape.Height = 30;
            thing.Shape.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 255));
            thing.Center.X = 300; thing.Center.Y = 0;
            thing.Shape.SetValue(Canvas.LeftProperty, thing.Center.X - thing.Shape.Width);
            thing.Shape.SetValue(Canvas.TopProperty, thing.Center.Y - thing.Shape.Width);
            canvas1.Children.Add(thing.Shape);
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.sensor != null && this.sensor.IsRunning)
            {
                this.sensor.Stop();
            }

        }

        void skeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            canvas1.Children.Clear();
            advanceThingPosition();
            canvas1.Children.Add(thing.Shape);

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame == null) { return; }
                skeletonFrame.CopySkeletonDataTo(totalSkeleton);
                skeleton = (from trackskeleton in totalSkeleton
                            where trackskeleton.TrackingState == SkeletonTrackingState.Tracked
                            select trackskeleton).FirstOrDefault();
                if (skeleton == null)
                    return;
                if (skeleton != null && this.currentSkeletonID != skeleton.TrackingId)
                {
                    this.currentSkeletonID = skeleton.TrackingId;
                    int totalTrackedJoints = skeleton.Joints.Where(item => item.TrackingState == JointTrackingState.Tracked).Count();
                    string TrackedTime = DateTime.Now.ToString("hh:mm:ss");
                    string status = "Skeleton Id: " + this.currentSkeletonID + ", total tracked joints: " + totalTrackedJoints + ", TrackTime: " + TrackedTime + "\n";
                    this.textBlock1.Text += status;
                }
                DrawSkeleton(skeleton);
            }

            Point handPt = ScalePosition(skeleton.Joints[JointType.HandRight].Position);
            if (thing.Hit(handPt))
            {
                //this.thing.YVelocity = -1.0 * this.thing.YVelocity;
                //this.thing.XVelocity = -1.0 * this.thing.YVelocity;

                this.thing.YVelocity = (-2.0) * this.thing.YVelocity *
                    Math.Abs(thing.Center.Y - handPt.Y) / (Math.Abs(thing.Center.Y - handPt.Y) + Math.Abs(thing.Center.X - handPt.X));
                this.thing.XVelocity = (-2.0) * this.thing.YVelocity *
                    Math.Abs(thing.Center.X - handPt.X) / (Math.Abs(thing.Center.Y - handPt.Y) + Math.Abs(thing.Center.X - handPt.X));
                if (flag_release == 0)
                {
                    count = count + 1;
                    flag_release = 1;
                }
                this.textBlock_count.Text = count.ToString();
                
            }
            
            Point handPtLeft = ScalePosition(skeleton.Joints[JointType.HandLeft].Position);
            if (thing.Hit(handPtLeft))
            {
                //this.thing.YVelocity = -1.0 * this.thing.YVelocity;
                //this.thing.XVelocity = -1.0 * this.thing.YVelocity;

                this.thing.YVelocity = (-2.0) * this.thing.YVelocity *
                    Math.Abs(thing.Center.Y - handPtLeft.Y) / (Math.Abs(thing.Center.Y - handPtLeft.Y) + Math.Abs(thing.Center.X - handPtLeft.X));
                this.thing.XVelocity = (-2.0) * this.thing.YVelocity *
                    Math.Abs(thing.Center.X - handPtLeft.X) / (Math.Abs(thing.Center.Y - handPtLeft.Y) + Math.Abs(thing.Center.X - handPtLeft.X));
                if (flag_release == 0)
                {
                    count = count + 1;
                    flag_release = 1;
                }
                this.textBlock_count.Text = count.ToString();

            }
            


            Point joint1 = this.ScalePosition(skeleton.Joints[JointType.WristRight].Position);

            if (thing.Center.Y - joint1.Y< -50)
            {
                flag_release = 0;
            }
        }

        private void DrawSkeleton(Skeleton skeleton)
        {
            drawBone(skeleton.Joints[JointType.Head], skeleton.Joints[JointType.ShoulderCenter]);
            drawBone(skeleton.Joints[JointType.ShoulderCenter], skeleton.Joints[JointType.Spine]);

            drawBone(skeleton.Joints[JointType.ShoulderCenter], skeleton.Joints[JointType.ShoulderLeft]);
            drawBone(skeleton.Joints[JointType.ShoulderLeft], skeleton.Joints[JointType.ElbowLeft]);
            drawBone(skeleton.Joints[JointType.ElbowLeft], skeleton.Joints[JointType.WristLeft]);
            drawBone(skeleton.Joints[JointType.WristLeft], skeleton.Joints[JointType.HandLeft]);

            drawBone(skeleton.Joints[JointType.ShoulderCenter], skeleton.Joints[JointType.ShoulderRight]);
            drawBone(skeleton.Joints[JointType.ShoulderRight], skeleton.Joints[JointType.ElbowRight]);
            drawBone(skeleton.Joints[JointType.ElbowRight], skeleton.Joints[JointType.WristRight]);
            drawBone(skeleton.Joints[JointType.WristRight], skeleton.Joints[JointType.HandRight]);

            drawBone(skeleton.Joints[JointType.Spine], skeleton.Joints[JointType.HipCenter]);
            drawBone(skeleton.Joints[JointType.HipCenter], skeleton.Joints[JointType.HipLeft]);
            drawBone(skeleton.Joints[JointType.HipLeft], skeleton.Joints[JointType.KneeLeft]);
            drawBone(skeleton.Joints[JointType.KneeLeft], skeleton.Joints[JointType.AnkleLeft]);
            drawBone(skeleton.Joints[JointType.AnkleLeft], skeleton.Joints[JointType.FootLeft]);

            drawBone(skeleton.Joints[JointType.HipCenter], skeleton.Joints[JointType.HipRight]);
            drawBone(skeleton.Joints[JointType.HipRight], skeleton.Joints[JointType.KneeRight]);
            drawBone(skeleton.Joints[JointType.KneeRight], skeleton.Joints[JointType.AnkleRight]);
            drawBone(skeleton.Joints[JointType.AnkleRight], skeleton.Joints[JointType.FootRight]);

            // more on next slide
        }

        private Point ScalePosition(SkeletonPoint skeletonPoint)
        {
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skeletonPoint, DepthImageFormat.Resolution320x240Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }

        void drawBone(Joint trackedJoint1, Joint trackedJoint2)
        {
            Line bone = new Line();
            bone.Stroke = Brushes.Red;
            bone.StrokeThickness = 3;
            Point joint1 = this.ScalePosition(trackedJoint1.Position);
            bone.X1 = joint1.X;
            bone.Y1 = joint1.Y;

            Point joint2 = this.ScalePosition(trackedJoint2.Position);
            bone.X2 = joint2.X;
            bone.Y2 = joint2.Y;

            canvas1.Children.Add(bone);
        }

        void advanceThingPosition()
        {
            thing.Center.Offset(thing.XVelocity, thing.YVelocity);
            thing.YVelocity += this.gravity;
            thing.Shape.SetValue(Canvas.LeftProperty, thing.Center.X - thing.Shape.Width);
            thing.Shape.SetValue(Canvas.TopProperty, thing.Center.Y - thing.Shape.Width);

            // if goes out of bound, reset position, as well as velocity
            if (thing.Center.Y >= canvas1.Height)
            {
                thing.Center.Y = 0;
                thing.XVelocity = 0;
                thing.YVelocity = 0;
                count = 0;
                this.textBlock_count.Text = count.ToString();

            }
        }
    }
}
