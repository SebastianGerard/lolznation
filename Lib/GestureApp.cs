using Leap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lib
{
    public class GestureApp: Listener
    {
        GestureList gestures = new GestureList();
        Window window = new Window();
        float windowWidth = 1400;
        float windowHeight = 800;
        bool wantToChangeOptions;
        public float sensitivity { get; set; }
        int option = 0;
        Options options;
        bool rightClickDown = false;
        bool fist = false;
        public override void OnInit(Controller controller)
        {
            options = new Options();
            controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
            controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
            controller.EnableGesture(Gesture.GestureType.TYPESWIPE);
            wantToChangeOptions = true;
            sensitivity = 600;
        }
        public override void OnFrame (Controller controller)
	    {
            Leap.Frame frame = controller.Frame();

            gestures = getGetures(frame);
            //process(gestures,frame);
            if (isClick(gestures) )
            {
                if (fist)
                {
                    fist = false;
                    window.leftClickUp();
                }
                wantToChangeOptions = false;
               
                if (hasTwoFingers(frame) )
                {

                    if (option == 1 && rightClickDown)
                    {
                        rightClickDown = false;
                        
                        window.rightClickUp();
                    }
                    else if (option == 1 && !rightClickDown)
                    {
                        rightClickDown = true;
                        window.zoom();
                        window.rightClickDown();
                    }
                }
                
                else window.click();
                
            }
            if (hasFist(frame) && !fist)
            {
                fist = true;
                window.pan();
                window.leftClickDown();
            }
            else if (!hasFist(frame) && fist)
            {
                fist = false;
                window.leftClickUp();
            }
            
            drawCursor(frame);
            changeFocus(gestures);
            selectOptions(gestures);
            
        }

        private bool hasFist(Frame frame)
        {
            Hand hand = frame.Hands.Rightmost;
            return hand.Fingers.Count == 0;
        }
        private bool hasTwoFingers(Frame frame)
        {
            Hand hand = frame.Hands[0];
            return (hand.Fingers.Count == 2);
        }
        private void selectOptions(GestureList gestures)
        {
            if (wantToChangeOptions)
            {
                
                foreach (Gesture gesture in gestures)
                {
                    if(gesture.Type == Gesture.GestureType.TYPESWIPE && gesture.State == Gesture.GestureState.STATESTART)
                    {
                        SwipeGesture swipe = new SwipeGesture(gesture);
                        if (Math.Abs(swipe.Direction.x) > 0.2)
                        {
                            if (swipe.Direction.x > 0 && option < (options.size() - 1))
                            {
                                option++;
                                Console.WriteLine(options.names[option]);
                            }
                            else if (swipe.Direction.x < 0 && option > 0)
                            {
                                option--;
                                Console.WriteLine(options.names[option]);
                            }
                        }

                    }
                    
                }
            }
        }

        private void changeFocus(GestureList gestures)
        {
            foreach (Gesture gesture in gestures)
            {
                if (gesture.Type == Gesture.GestureType.TYPECIRCLE )
                {
                    wantToChangeOptions = true;
                    window.changeFocusMyWindow();
                }
            }
        }

        private bool isClick(GestureList gestures)
        {
            foreach (Gesture gesture in gestures)
            {
                if (gesture.Type == Gesture.GestureType.TYPEKEYTAP)
                {
                    return true;
                }
            }
            return false;
        }

        private void drawCursor(Frame frame)
        {
            InteractionBox interactionBox = frame.InteractionBox;
            Hand hand = frame.Hands.Rightmost;
            
            Pointable pointable = hand.Pointables[0];
            Vector normalizedPosition = interactionBox.NormalizePoint(hand.StabilizedPalmPosition);
                float tx = normalizedPosition.x * windowWidth;
                float ty = windowHeight - normalizedPosition.y * windowHeight;
            window.setCursorPositionXY((int)tx, (int)ty);

        }
        private GestureList getGetures(Frame frame)
        {
            GestureList gestures = frame.Gestures();
            return gestures;
        }

        /*Controller controller = null;
        bool started= false;
        GestureList gestures = new GestureList();
        int lastId = 0;
        Window window = new Window();
        bool onclick = false;
        bool recentlyChanged = false;
        public float sensitivity { get; set; }

        float windowWidth = 1400;
        float windowHeight = 800;
        public GestureApp()
        {
            sensitivity = 300;
            controller = new Controller();
            controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
            controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
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
                //process(gestures,frame);
                drawCursor(frame);
                if (isClick(gestures))
                    window.click();
            }
        }

        private bool isClick(GestureList gestures)
        {
            recentlyChanged = false;
            foreach (Gesture gesture in gestures)
            {
                if (gesture.Type == Gesture.GestureType.TYPEKEYTAP)
                {
                    return true;
                }
            }
            return false;
        }

        private void drawCursor(Frame frame)
        {
            float sumax = 0;
            float sumay = 0;
            int count = 0;
            InteractionBox interactionBox = frame.InteractionBox;
            foreach (Pointable pointable in frame.Pointables)
            {
                Vector normalizedPosition = interactionBox.NormalizePoint(pointable.StabilizedTipPosition);
                float tx = normalizedPosition.x * windowWidth;
                float ty = windowHeight - normalizedPosition.y * windowHeight;
                sumax += tx;
                sumay += ty;
                count++;
            }
            float averagex = sumax / count;
            float averagey = sumay / count;
            window.setCursorPositionXY((int)averagex,(int) averagey);
            
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
            /*currentTime = frame.Timestamp;
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
            }*/
        /*}

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
        }*/
    }
}
