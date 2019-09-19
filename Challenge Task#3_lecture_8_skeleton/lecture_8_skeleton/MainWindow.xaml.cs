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
//using System.Drawing;
//using System.Drawing.Printing;



namespace lecture_8_skeleton
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
        int currentSkeletonID = 0;

        public MainWindow()
        {
            InitializeComponent();
        }



        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            this.sensor = KinectSensor.KinectSensors[0];
            //this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
            this.sensor.SkeletonStream.Enable();
            this.sensor.SkeletonFrameReady += skeletonFrameReady;
            // start the sensor.
            this.sensor.Start();
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
        }

        private void DrawSkeleton(Skeleton skeleton)
        {
            System.Windows.Point joint1 = this.ScalePosition(skeleton.Joints[JointType.ElbowRight].Position);
            System.Windows.Point joint2 = this.ScalePosition(skeleton.Joints[JointType.HipCenter].Position);
            System.Windows.Point joint3 = this.ScalePosition(skeleton.Joints[JointType.Spine].Position);
            System.Windows.Point joint4 = this.ScalePosition(skeleton.Joints[JointType.ShoulderRight].Position);

            Point P1 = new Point(); //elbow
            Point P2 = new Point(); // hip
            Point P3 = new Point(); //spine
            Point P4 = new Point(); // shoulder


            P1.X = joint1.X;
            P1.Y = joint1.Y;
            P2.X = joint2.X;
            P2.Y = joint2.Y;
            P3.X = joint3.X;
            P3.Y = joint3.Y;
            P4.X = joint4.X;
            P4.Y = joint4.Y;

            Size size_val = new Size(5, 5);
            System.Drawing.Pen blackPen = new System.Drawing.Pen(System.Drawing.Color.Black, 3);

            AddCircularArcGraph(P2, P1, size_val);

            /*


            double P12 = Math.Sqrt( Math.Pow(P1.X - P2.X, 2) + Math.Pow(P1.Y - P2.Y, 2));
            double P13 = Math.Sqrt( Math.Pow(P1.X - P3.X, 2) + Math.Pow(P1.Y - P3.Y, 2));
            double P23 = Math.Sqrt( Math.Pow(P2.X - P3.X, 2) + Math.Pow(P2.Y - P3.Y, 2));

            double angle = Math.Acos((Math.Pow(P13, 2) + Math.Pow(P23, 2) - Math.Pow(P12, 2))/(2*P13*P23));
            angle = angle * 180 / Math.PI;
            Console.WriteLine("P13");
            Console.WriteLine(P13);
            */

            
            double a = P1.X - P4.X;
            double b = P1.Y - P4.Y;
            double c = P2.X - P3.X;
            double d = P2.Y - P3.Y; ;

            double atanA = Math.Atan2(a, b);
            double atanB = Math.Atan2(c, d);
            //double angle = (atanA - atanB) * (-180 / Math.PI);
            //double angle = (atanB) * (-180 / Math.PI);
            double angle1 = (atanA) * (-180 / Math.PI);
            double angle2 = (atanB) * (-180 / Math.PI);

            double angle = -(angle1 - angle2);





            TextBox txt = new TextBox();
            txt.Text = angle.ToString();
            Canvas.SetLeft(txt,P1.X+0.1);
            Canvas.SetTop(txt, P1.Y+0.1);

            canvas1.Children.Add(txt);


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



        }


        private System.Windows.Point ScalePosition(SkeletonPoint skeletonPoint)
        {
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skeletonPoint, DepthImageFormat.Resolution320x240Fps30);
            return new System.Windows.Point(depthPoint.X, depthPoint.Y);
        }

        void drawBone(Joint trackedJoint1, Joint trackedJoint2)
        {
            Line bone = new Line();
            bone.Stroke = System.Windows.Media.Brushes.Red;
            bone.StrokeThickness = 3;
            System.Windows.Point joint1 = this.ScalePosition(trackedJoint1.Position);
            bone.X1 = joint1.X;
            bone.Y1 = joint1.Y;

            System.Windows.Point joint2 = this.ScalePosition(trackedJoint2.Position);
            bone.X2 = joint2.X;
            bone.Y2 = joint2.Y;

            System.Drawing.Pen blackPen = new System.Drawing.Pen(System.Drawing.Color.Black, 3);

            // Reactangle with specifies x1, 
            // y1, x2, y2 respectively 
            //System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, 100, 200);

            // Create start and sweep angles on ellipse. 
            //float startAngle = 45.0F;
            //float sweepAngle = 270.0F;

            // Draw arc to screen. 
            //myShape = new Arc(new Coordinate(1, 1, 1), 5, 20);

            //Graphics.DrawArc(blackPen, rect, startAngle, sweepAngle);

            // more on next slide

            canvas1.Children.Add(bone);

        }

        private void AddCircularArcGraph(Point startPoint, Point endPoint, Size size)
        {
            PathFigure pf = new PathFigure();
            pf.StartPoint = new Point(startPoint.X, startPoint.Y);
            ArcSegment arcSegment = new ArcSegment();
            arcSegment.Point = new Point(endPoint.X, endPoint.Y);
            arcSegment.Size = size;
            arcSegment.SweepDirection = SweepDirection.Counterclockwise;

            PathSegmentCollection psc = new PathSegmentCollection();
            psc.Add(arcSegment); pf.Segments = psc;

            PathFigureCollection pfc = new PathFigureCollection(); pfc.Add(pf);
            PathGeometry pg = new PathGeometry(); pg.Figures = pfc;

            var path = new System.Windows.Shapes.Path();
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 1; path.Data = pg;

            Console.WriteLine(size);
            Console.WriteLine(startPoint.X);

            canvas1.Children.Add(path);
        }
    }
}
