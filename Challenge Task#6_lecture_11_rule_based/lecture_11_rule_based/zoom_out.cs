using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace lecture_11_rule_based
{
    public class zoom_out : GestureBase
    {
        private double validatePosition;

        private double startingPostion;

        public zoom_out() : base(GestureType.zoom_out) { }

        protected override bool ValidateGestureStartCondition(Skeleton skeleton)
        {
            //Console.WriteLine("ValidateGestureStartCondition");

            var handRightPosition = skeleton.Joints[JointType.HandRight].Position;
            var handLeftPosition = skeleton.Joints[JointType.HandLeft].Position;
            var shoulderRightPosition = skeleton.Joints[JointType.ShoulderRight].Position;
            var spinePosition = skeleton.Joints[JointType.Spine].Position;
            var hipPosition = skeleton.Joints[JointType.HipCenter].Position;


            if ((handRightPosition.Y < shoulderRightPosition.Y) && (handLeftPosition.Y < shoulderRightPosition.Y) &&
                (handRightPosition.Y > hipPosition.Y) &&
                (handLeftPosition.Y > hipPosition.Y) &&
                (Math.Abs(handLeftPosition.X - handRightPosition.X) > 0.75))
            {
                //Console.WriteLine("ValidateGestureStartCondition --> yes");
                validatePosition = Math.Abs(handLeftPosition.X - handRightPosition.X);
                startingPostion = Math.Abs(handLeftPosition.X - handRightPosition.X);
                return true;
            }
            return false;
        }

        protected override bool IsGestureValid(Skeleton skeletonData)
        {
            //Console.WriteLine("IsGestureValid");

            var currentHandRightPoisition = skeletonData.Joints[JointType.HandRight].Position;
            var currentHandLeftPoisition = skeletonData.Joints[JointType.HandLeft].Position;
            if (validatePosition < Math.Abs(currentHandRightPoisition.X - currentHandLeftPoisition.X))
            {
                return false;
            }
            //Console.WriteLine("IsGestureValid-->yes");

            validatePosition = Math.Abs(currentHandRightPoisition.X - currentHandLeftPoisition.X);

            return true;
        }

        protected override bool ValidateGestureEndCondition(Skeleton skeleton)
        {
            var currentHandRightPoisition = skeleton.Joints[JointType.HandRight].Position;
            var currentHandLeftPoisition = skeleton.Joints[JointType.HandLeft].Position;

            double distance = Math.Abs(currentHandLeftPoisition.X - currentHandRightPoisition.X);
            float currentshoulderDiff = GestureHelper.GetJointDistance(skeleton.Joints[JointType.HandRight], skeleton.Joints[JointType.HandLeft]);

            //Console.WriteLine("ValidateGestureEndCondition");
            //Console.WriteLine(currentshoulderDiff);
            if (currentshoulderDiff < 0.25)
                return true;

            return false;
        }

        protected override bool ValidateBaseCondition(Skeleton skeleton)
        {
            var handRightPosition = skeleton.Joints[JointType.HandRight].Position;
            var handLeftPosition = skeleton.Joints[JointType.HandLeft].Position;
            var shoulderRightPosition = skeleton.Joints[JointType.ShoulderRight].Position;
            var spinePosition = skeleton.Joints[JointType.Spine].Position;
            var hipPosition = skeleton.Joints[JointType.HipCenter].Position;

            if ((handRightPosition.Y < shoulderRightPosition.Y) &&
                (handLeftPosition.Y < shoulderRightPosition.Y) &&
                (handRightPosition.Y > hipPosition.Y) &&
                (handLeftPosition.Y > hipPosition.Y))
            {
                //shoulderDiff = GestureHelper.GetJointDistance(skeleton.Joints[JointType.HandRight], skeleton.Joints[JointType.ShoulderLeft]);
                validatePosition = Math.Abs(handLeftPosition.X - handRightPosition.X);
                startingPostion = Math.Abs(handLeftPosition.X - handRightPosition.X);
                return true;
            }
            return false;
        }

    }
}
