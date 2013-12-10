using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
namespace ConsoleApplication1
{
    class Program
    {
        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
            string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("USER32.DLL")]
        public static extern IntPtr GetForegroundWindow();

        static void Main(string[] args)
        {
            Thread first = new Thread(handler);
            first.Start();
          
            
            
        }
        public static void changeFocus(string _class,string _caption)
        {
            IntPtr calculatorHandle = FindWindow(_class, _caption);

            // Verify that Calculator is a running process.
            if (calculatorHandle == IntPtr.Zero)
            {
                MessageBox.Show("Program is not running.");
                return;
            }

            // Make Calculator the foreground application and send it 
            // a set of calculations.
            if (calculatorHandle != GetForegroundWindow())
                SetForegroundWindow(calculatorHandle);
        }
        private static void handler()
        {
            Controller controller = new Controller();
            controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
            string last_direction = "None";
            float sweptAngle = 0;
            int lastId = 0;
            while (true)
            {
                Leap.Frame frame = controller.Frame();

                GestureList gestures = frame.Gestures ();
                for (int i = 0; i < gestures.Count; i++) {
                        Leap.Gesture gesture = gestures [i];
                        Console.SetCursorPosition(0, 0);
                        Console.Clear();
                        switch (gesture.Type) {
                        case Leap.Gesture.GestureType.TYPECIRCLE:
                                
                                CircleGesture circle = new CircleGesture (gesture);
                                
                    // Calculate clock direction using the angle between circle normal and pointable
                                String clockwiseness = "None";
                                
                                
                                if (circle.State != Gesture.GestureState.STATESTART)
                                {

                                    CircleGesture previousUpdate = new CircleGesture(controller.Frame(1).Gesture(circle.Id));
                                    sweptAngle = (circle.Progress - previousUpdate.Progress) * 360;
                                }

                                else
                                {
                                    if (lastId!=circle.Id)
                                    {
                                        if (circle.Pointable.Direction.AngleTo(circle.Normal) <= Math.PI / 4)
                                        {
                                            //Clockwise if angle is less than 90 degrees
                                            clockwiseness = "clockwise";
                                            changeFocus("WindowsForms10.Window.8.app.0.bf7d44_r9_ad1", "PHENIX - Vafk,T,6 [ClearCanvas DICOM Viewer (Source) - Not for Diagnostic Use | Modified Installation]");
                                            SendKeys.SendWait("a");
                                            changeFocus("ConsoleWindowClass", "file:///C:/Users/Sebastian/Documents/Visual Studio 2012/Projects/ConsoleApplication1/ConsoleApplication1/bin/Debug/ConsoleApplication1.EXE");

                                        }
                                        else
                                        {
                                            clockwiseness = "counterclockwise";
                                            changeFocus("WindowsForms10.Window.8.app.0.bf7d44_r9_ad1", "PHENIX - Vafk,T,6 [ClearCanvas DICOM Viewer (Source) - Not for Diagnostic Use | Modified Installation]");
                                            SendKeys.SendWait(",");
                                            changeFocus("ConsoleWindowClass", "file:///C:/Users/Sebastian/Documents/Visual Studio 2012/Projects/ConsoleApplication1/ConsoleApplication1/bin/Debug/ConsoleApplication1.EXE");

                                        }
                                    }
                                    
                                }
                                Console.Write ("Circle id: " + circle.Id
                               + ", " + circle.State
                               + ", progress: " + circle.Progress
                               + ", radius: " + circle.Radius
                               + ", angle: " + sweptAngle
                               + ", " + clockwiseness);
                                lastId = circle.Id;
                                break;
                                
                        default:
                                Console.Write ("Unknown gesture type.");
                                break;
                        }
                }
                // Get the first hand
                /*Hand hand = frame.Hands[0];

                // Check if the hand has any fingers
                FingerList fingers = hand.Fingers;
                if (!fingers.IsEmpty)
                {
                    // Calculate the hand's average finger tip position
                    Vector avgPos = Vector.Zero;
                    Vector avgVelocity = Vector.Zero;
                    foreach (Finger finger in fingers)
                    {
                        avgPos += finger.TipPosition;
                        avgVelocity += finger.TipVelocity;
                    }
                    avgPos /= fingers.Count;
                    avgVelocity /= fingers.Count;

                    int sensitivity = 300;
                    if (avgVelocity.y > sensitivity && last_direction != "Up")
                    {
                        Console.Clear();
                        Console.Write("Up");
                        last_direction = "Up";
                    }
                    else if (avgVelocity.y < -sensitivity && last_direction != "Down")
                    {
                        Console.Clear();
                        Console.Write("Down");
                        last_direction = "Down";
                    }

                    if (avgVelocity.x > sensitivity && avgPos.x%20==0)
                    {
                        //Console.Clear();
                        Console.Write("Right");
                        last_direction = "Right";
                        changeFocus("WindowsForms10.Window.8.app.0.bf7d44_r9_ad1", "PHENIX - Vafk,T,6 [ClearCanvas DICOM Viewer (Source) - Not for Diagnostic Use | Modified Installation]");
                        SendKeys.SendWait("a");
                        changeFocus("ConsoleWindowClass", "file:///C:/Users/Sebastian/Documents/Visual Studio 2012/Projects/ConsoleApplication1/ConsoleApplication1/bin/Debug/ConsoleApplication1.EXE");

                    }
                    else if (avgVelocity.x < -sensitivity)
                    {
                        //Console.Clear();
                        Console.Write("Left");
                        last_direction = "Left";
                        changeFocus("WindowsForms10.Window.8.app.0.bf7d44_r9_ad1", "PHENIX - Vafk,T,6 [ClearCanvas DICOM Viewer (Source) - Not for Diagnostic Use | Modified Installation]");
                        SendKeys.SendWait(",");
                        changeFocus("ConsoleWindowClass", "file:///C:/Users/Sebastian/Documents/Visual Studio 2012/Projects/ConsoleApplication1/ConsoleApplication1/bin/Debug/ConsoleApplication1.EXE");


                    }

                    if (avgVelocity.z < -sensitivity && last_direction != "Forward")
                    {
                        Console.Clear();
                        Console.Write("Forward");
                        last_direction = "Forward";
                    }
                    else if (avgVelocity.z > sensitivity && last_direction != "Backward")
                    {
                        Console.Clear();
                        Console.Write("Backward");
                        last_direction = "Backward";
                    }

                    Console.SetCursorPosition(0, 3);
                    Console.Write("X: " + avgPos.x + " Y: " + avgPos.y + " Z: " + avgPos.z);*/
                }
            }
        }
    }
