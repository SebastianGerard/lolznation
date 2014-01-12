using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using Lib;
namespace ConsoleApplication1
{
    class Program
    {
        
        static void Main(string[] args)
        {
            GestureApp gestureApp = new GestureApp();
            Controller controller = new Controller();
            controller.SetPolicyFlags(Controller.PolicyFlag.POLICYBACKGROUNDFRAMES);
            controller.AddListener(gestureApp);
            Console.WriteLine("Press Enter to quit...");
            Console.ReadLine();
        }
    }
}