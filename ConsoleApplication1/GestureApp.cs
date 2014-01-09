﻿using Leap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleApplication1
{
    public class GestureApp
    {
        Controller controller = null;
        bool started= false;
        GestureList gestures = new GestureList();
        int lastId = 0;
        Window window = new Window();
        public float sensitivity { get; set; }
        public GestureApp()
        {
            sensitivity = 300;
            controller = new Controller();
            controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
        }
        public void start()
        {
            started = true;
            Thread t = new Thread(listen);
            t.Start();
        }
        public void stop()
        {
            if (started)
                started = false;
        }

        private void listen()
        {
            while (started)
            {
                Leap.Frame frame = controller.Frame();
                
                gestures = getGetures(frame);
                process(gestures,frame);
            }
        }
        private GestureList getGetures(Frame frame)
        {
            GestureList gestures = frame.Gestures();
            return gestures;
        }
        private void process(GestureList gestures, Frame frame)
        {
            processMoveHandUp(frame);
            processMoveHandRight(frame);
             for (int i = 0; i < gestures.Count; i++) {
                 Leap.Gesture gesture = gestures[i];
                 if(gesture.Type == Gesture.GestureType.TYPECIRCLE)
                    processCircle(gesture);
                 
             }
        }

        private long currentTime;
        private long previousTime;
        private long timeChange;

       

            

        private void processMoveHandUp(Frame frame)
        {
            currentTime = frame.Timestamp;
            timeChange = currentTime - previousTime;

            if (timeChange > 10000)
            {
                if (frame.Hands.Count>0)
                {
                    // Get the first finger in the list of fingers
                    Finger finger = frame.Fingers[0];
                    // Get the closest screen intercepting a ray projecting from the finger
                    Screen screen = controller.c

                    if (screen != null && screen.IsValid)
                    {
                        // Get the velocity of the finger tip
                        var tipVelocity = (int)finger.TipVelocity.Magnitude;

                        // Use tipVelocity to reduce jitters when attempting to hold
                        // the cursor steady
                        if (tipVelocity > 25)
                        {
                            var xScreenIntersect = screen.Intersect(finger, true).x;
                            var yScreenIntersect = screen.Intersect(finger, true).y;

                            if (xScreenIntersect.ToString() != "NaN")
                            {
                                var x = (int)(xScreenIntersect * screen.WidthPixels);
                                var y = (int)(screen.HeightPixels - (yScreenIntersect * screen.HeightPixels));

                                Console.WriteLine("Screen intersect X: " + xScreenIntersect.ToString());
                                Console.WriteLine("Screen intersect Y: " + yScreenIntersect.ToString());
                                Console.WriteLine("Width pixels: " + screen.WidthPixels.ToString());
                                Console.WriteLine("Height pixels: " + screen.HeightPixels.ToString());

                                Console.WriteLine("\n");

                                Console.WriteLine("x: " + x.ToString());
                                Console.WriteLine("y: " + y.ToString());

                                Console.WriteLine("\n");

                                Console.WriteLine("Tip velocity: " + tipVelocity.ToString());

                                // Move the cursor
                                MouseCursor.MoveCursor(x, y);

                                Console.WriteLine("\n" + new String('=', 40) + "\n");
                            }

                        }
                    }

                }

                previousTime = currentTime;
            }
        }

        private void processMoveHandRight(Frame frame)
        {
            //throw new NotImplementedException();
        }

        private void processCircle(Gesture gesture)
        {
            CircleGesture circle = new CircleGesture(gesture);
            if (circle.State == Gesture.GestureState.STATESTART)
            {
                if (lastId != circle.Id)
                {
                    if (circle.Pointable.Direction.AngleTo(circle.Normal) <= Math.PI / 4)
                    {
                        window.zoomin();
                    }
                    else
                    {
                        window.zoomout();
                    }
                    lastId = circle.Id;
                }
            }
        }
    }
}