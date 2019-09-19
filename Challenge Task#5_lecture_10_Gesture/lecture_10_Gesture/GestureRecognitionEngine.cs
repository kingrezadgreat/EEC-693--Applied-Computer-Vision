﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace lecture_10_Gesture
{
    public enum GestureType
    {
        HandsClapping,
        LeftHandsRaised,
        RightHandsRaised
    }

    public enum RecognitionResult
    {
        Unknown,
        Failed,
        Success
    }

    public class GestureEventArgs : EventArgs
    {
        public GestureType gsType { get; internal set; }
        public RecognitionResult Result { get; internal set; }
        public GestureEventArgs(GestureType t, RecognitionResult result)
        {
            this.Result = result;
            this.gsType = t;
        }
    }

    public class GestureRecognitionEngine
    {
        public GestureRecognitionEngine() { }
        public event EventHandler<GestureEventArgs> GestureRecognized;
        public Skeleton Skeleton { get; set; }
        public GestureType GestureType { get; set; }

        public void StartRecognize(GestureType t)
        {
            this.GestureType = t;
            switch (t)
            {
                case GestureType.HandsClapping:
                    this.MatchHandClappingGesture(this.Skeleton);
                    break;
                case GestureType.LeftHandsRaised:
                    this.MatchLeftHandsRaisedGesture(this.Skeleton);
                    break;
                case GestureType.RightHandsRaised:
                    this.MatchRightHandsRaisedGesture(this.Skeleton);
                    break;
                default:
                    break;
            }
        }

        float previousDistance = 0.0f;
        private void MatchHandClappingGesture(Skeleton skeleton)
        {
            if (skeleton == null) { return; }
            if (skeleton.Joints[JointType.HandRight].TrackingState == JointTrackingState.Tracked
                    && skeleton.Joints[JointType.HandLeft].TrackingState == JointTrackingState.Tracked)
            {
                float currentDistance =
                    GetJointDistance(skeleton.Joints[JointType.HandRight],
                        skeleton.Joints[JointType.HandLeft]);
                if (currentDistance < 0.1f && previousDistance > 0.1f)
                {
                    if (this.GestureRecognized != null)
                    {
                        this.GestureRecognized(this, new GestureEventArgs(GestureType.HandsClapping, RecognitionResult.Success));
                    }
                }
                previousDistance = currentDistance;
            }
        }

        private void MatchLeftHandsRaisedGesture(Skeleton skeleton)
        {
            if (skeleton == null) { return; }
            float threshold = 0.3f;
            if (skeleton.Joints[JointType.HandLeft].Position.Y >
                skeleton.Joints[JointType.Head].Position.Y + threshold)
            {
                if (this.GestureRecognized != null)
                {
                    this.GestureRecognized(this, new GestureEventArgs(GestureType.LeftHandsRaised, RecognitionResult.Success));
                }
            }
        }

        private void MatchRightHandsRaisedGesture(Skeleton skeleton)
        {
            if (skeleton == null) { return; }
            float threshold = 0.3f;
            if (skeleton.Joints[JointType.HandRight].Position.Y >
                skeleton.Joints[JointType.Head].Position.Y + threshold)
            {
                if (this.GestureRecognized != null)
                {
                    this.GestureRecognized(this, new GestureEventArgs(GestureType.RightHandsRaised, RecognitionResult.Success));
                }
            }
        }



        private float GetJointDistance(Joint firstJoint, Joint secondJoint)
        {
            float distanceX = firstJoint.Position.X - secondJoint.Position.X;
            float distanceY = firstJoint.Position.Y - secondJoint.Position.Y;
            float distanceZ = firstJoint.Position.Z - secondJoint.Position.Z;
            return (float)Math.Sqrt(Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2) + Math.Pow(distanceZ, 2));
        }
    }
}
