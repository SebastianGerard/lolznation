using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestureLib;
using Leap;
using System.Windows.Forms;
using System.Runtime.InteropServices;
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
        
            

        static void Main(string[] args)
        {
            Controller controller = new Controller();
            string last_direction = "None";
            while (true)
            {
                Leap.Frame frame = controller.Frame();
                // Get the first hand
                Hand hand = frame.Hands[0];

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
                    if (avgVelocity.y > sensitivity && last_direction!="Up")
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

                    if (avgVelocity.x > sensitivity && last_direction != "Right")
                    {
                        Console.Clear();
                        Console.Write("Right");
                        last_direction = "Right";
                        IntPtr calculatorHandle = FindWindow("WindowsForms10.Window.8.app.0.bf7d44_r9_ad1", "PHENIX - Vafk,T,6 [ClearCanvas DICOM Viewer (Source) - Not for Diagnostic Use | Modified Installation]");

                        // Verify that Calculator is a running process.
                        if (calculatorHandle == IntPtr.Zero)
                        {
                            MessageBox.Show("Calculator is not running.");
                            return;
                        }

                        // Make Calculator the foreground application and send it 
                        // a set of calculations.
                        SetForegroundWindow(calculatorHandle);
                        SendKeys.SendWait("a");
                    }
                    else if (avgVelocity.x < -sensitivity && last_direction != "Left")
                    {
                        Console.Clear();
                        Console.Write("Left");
                        last_direction = "Left";
                        IntPtr calculatorHandle = FindWindow("WindowsForms10.Window.8.app.0.bf7d44_r9_ad1", "PHENIX - Vafk,T,6 [ClearCanvas DICOM Viewer (Source) - Not for Diagnostic Use | Modified Installation]");

                        // Verify that Calculator is a running process.
                        if (calculatorHandle == IntPtr.Zero)
                        {
                            MessageBox.Show("Calculator is not running.");
                            return;
                        }

                        // Make Calculator the foreground application and send it 
                        // a set of calculations.
                        SetForegroundWindow(calculatorHandle);
                        SendKeys.SendWait(",");
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
                    Console.Write("X: "+avgPos.x+" Y: "+avgPos.y+" Z: "+avgPos.z);
                }
            }
            
        }
    }
}
