using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestureLib;
using Leap;
namespace ConsoleApplication1
{
    class Program
    {

        public void listener_onGesture(GestureLib.Gesture gesture)
        {
            foreach (GestureLib.Gesture.Direction direction in gesture.directions)
            {
                if (direction.ToString() == "Right")
                {
                    Console.Clear();
                    Console.Write("Right");
                }
                if (direction.ToString() == "Left")
                {
                    Console.Clear();
                    Console.Write("Left");
                }
                if (direction.ToString() == "Up")
                {
                    Console.Clear();
                    Console.Write("Up");
                }
                if (direction.ToString() == "Down")
                {
                    Console.Clear();
                    Console.Write("Down");
                }
            }
        }
            

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
                    }
                    else if (avgVelocity.x < -sensitivity && last_direction != "Left")
                    {
                        Console.Clear();
                        Console.Write("Left");
                        last_direction = "Left";
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
