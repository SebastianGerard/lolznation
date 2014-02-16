using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leap;
namespace Lib
{
    public static class GestureLib
    {
        static int lastId = 0;

        //public static DefautGestures DefautGestures
        //{
        //    get
        //    {
        //        throw new System.NotImplementedException();
        //    }
        //    set
        //    {
        //    }
        //}
    
        public static  bool areOutTouchZone(Frame frame)
        {
            Hand hand = frame.Hands.Rightmost;
            Finger finger = hand.Fingers[0];

            return finger.TouchZone == Pointable.Zone.ZONEHOVERING;
        }
        public static bool areInTouchZone(Frame frame)
        {
            Hand hand = frame.Hands.Rightmost;
            Finger finger = hand.Fingers[0];
            return finger.TouchDistance <= 0 && (finger.TouchZone == Pointable.Zone.ZONETOUCHING);
        }

        public static bool hastwofingers(Frame frame)
        {
            Hand hand = frame.Hands.Rightmost;
            return hand.Fingers.Count == 2;
        }
        public static bool hasonefinger(Frame frame)
        {
            Hand hand = frame.Hands.Rightmost;
            return hand.Fingers.Count == 1 && frame.Hands.Count == 1;
        }
        public static bool hasFist(Frame frame)
        {
            Hand hand = frame.Hands.Rightmost;
            return hand.Fingers.Count == 0;
        }
        public static bool hasTwoFingersEachHand(Frame frame)
        {
            Hand handizq = frame.Hands[0];
            Hand handder = frame.Hands[1];
            return (handizq.Fingers.Count == 1 && handder.Fingers.Count == 1);
        }
       
        public static bool isKeyTap(GestureList gestures)
        {
            foreach (Gesture gesture in gestures)
            {
                if(gesture.Type == Gesture.GestureType.TYPEKEYTAP)
                    return true;
            }
            return false;
        }
        public static bool isSwapRight(GestureList gestures)
        {
            foreach (Gesture gesture in gestures)
            {
                if (gesture.Type == Gesture.GestureType.TYPESWIPE && gesture.State == Gesture.GestureState.STATESTART)
                {
                    SwipeGesture swipe = new SwipeGesture(gesture);
                    if (Math.Abs(swipe.Direction.x) > 0.2)
                    {
                        if (swipe.Direction.x < 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public static bool isSwapLeft(GestureList gestures)
        {
            foreach (Gesture gesture in gestures)
            {
                if (gesture.Type == Gesture.GestureType.TYPESWIPE && gesture.State == Gesture.GestureState.STATESTART)
                {
                    SwipeGesture swipe = new SwipeGesture(gesture);
                    if (Math.Abs(swipe.Direction.x) > 0.2)
                    {
                        if (swipe.Direction.x > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public static Vector getStabilizedPalmPosition(Frame frame)
        {
            InteractionBox interactionBox = frame.InteractionBox;
            Hand hand = frame.Hands.Rightmost;
            Vector normalizedPosition = interactionBox.NormalizePoint(hand.StabilizedPalmPosition);
            return normalizedPosition;
        }
        public static DefautGestures isACircle(GestureList gestures)
        {
            foreach (Gesture gesture in gestures)
            {
                if (gesture.Type == Gesture.GestureType.TYPECIRCLE)
                {
                    CircleGesture circle = new CircleGesture(gesture);
                    if (circle.State == Gesture.GestureState.STATESTART)
                    {
                        if (lastId != circle.Id)
                        {
                            
                            if (circle.Pointable.Direction.AngleTo(circle.Normal) <= Math.PI / 4)
                            {
                                return DefautGestures.CircleRight;
                            }
                            else
                                return DefautGestures.CircleLeft;
                            
                        }
                    }
                }
                
            }
            
            return DefautGestures.None;
        }
        

    }
}
