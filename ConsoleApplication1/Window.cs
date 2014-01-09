using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Runtime;
namespace ConsoleApplication1
{
    class Window
    {

        [DllImport("user32.dll")]
        private static extern void mouse_event(UInt32 dwFlags, UInt32 dx, UInt32 dy, UInt32 dwData, IntPtr dwExtraInfo);

        private const UInt32 MouseEventRightDown = 0x0008;
        private const UInt32 MouseEventRightUp = 0x0010;

        public static void SendRightClickDown(UInt32 posX, UInt32 posY)
        {
            mouse_event(MouseEventRightDown, posX, posY, 0, new System.IntPtr());
            
        } 
        public static void SendRightClickUp(UInt32 posX, UInt32 posY)
        {
            
            mouse_event(MouseEventRightUp, posX, posY, 0, new System.IntPtr());
            
        } 
        

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
            string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("USER32.DLL")]
        public static extern IntPtr GetForegroundWindow();


        string clearCanvasClass = "WindowsForms10.Window.8.app.0.bf7d44_r9_ad1";
        string clearCanvasName = "PHENIX - Vafk,T,6 [ClearCanvas DICOM Viewer (Source) - Not for Diagnostic Use | Modified Installation]";

        string myWindowsClass = "ConsoleWindowClass";
        string myWindowsName = "file:///C:/Users/Sebastian/Documents/Visual Studio 2012/Projects/ConsoleApplication1/ConsoleApplication1/bin/Debug/ConsoleApplication1.EXE";

        private static bool changeFocus(string _class, string _caption)
        {
            IntPtr calculatorHandle = FindWindow(_class, _caption);

            // Verify that Calculator is a running process.
            if (calculatorHandle == IntPtr.Zero)
            {
                return false;
            }

            // Make Calculator the foreground application and send it 
            // a set of calculations.
            if (calculatorHandle != GetForegroundWindow())
                SetForegroundWindow(calculatorHandle);
            return true;
        }
        public void changeFocusClearCanvas()
        {
            changeFocus(clearCanvasClass,clearCanvasName);
        }

        public void changeFocusMyWindow()
        {
            changeFocus(myWindowsClass,myWindowsName);
        }

        public void zoomin()
        {
            changeFocusClearCanvas();
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            SendRightClickDown((UInt32)X, (UInt32)Y);
            SetCursorPos(Cursor.Position.X, Cursor.Position.Y + 3);
            SendRightClickUp((UInt32)X, (UInt32)Y);
             //SendKeys.SendWait("a");
            changeFocusMyWindow();
        }
        public void zoomout()
        {
            changeFocusClearCanvas();
            //SendKeys.SendWait(",");
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            SendRightClickDown((UInt32)X, (UInt32)Y);
            SetCursorPos(Cursor.Position.X, Cursor.Position.Y - 3);
            SendRightClickUp((UInt32)X, (UInt32)Y);
            changeFocusMyWindow();
        }

    }
}
