using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Runtime;
using System.Drawing;
namespace Lib
{
    class Window
    {

        [DllImport("user32.dll")]
        private static extern void mouse_event(UInt32 dwFlags, UInt32 dx, UInt32 dy, UInt32 dwData, IntPtr dwExtraInfo);

        private const UInt32 MouseEventRightDown = 0x0008;
        private const UInt32 MouseEventRightUp = 0x0010;
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;

        public static void SendRightClickDown(UInt32 posX, UInt32 posY)
        {
            mouse_event(MouseEventRightDown, posX, posY, 0, new System.IntPtr());
            
        } 
        public static void SendRightClickUp(UInt32 posX, UInt32 posY)
        {
            
            mouse_event(MouseEventRightUp, posX, posY, 0, new System.IntPtr());
            
        }
        public static void SendLeftClickUp(UInt32 posX, UInt32 posY)
        {

            mouse_event(MOUSEEVENTF_LEFTUP, posX, posY, 0, new System.IntPtr());

        }
        public static void SendLeftClickDown(UInt32 posX, UInt32 posY)
        {

            mouse_event(MOUSEEVENTF_LEFTDOWN, posX, posY, 0, new System.IntPtr());

        } 
        
       
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
            string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("USER32.DLL")]
        public static extern IntPtr GetForegroundWindow();

        //WindowsForms10.Window.8.app.0.bf7d44_r9_ad1
        //Explorer [ClearCanvas DICOM Viewer (Source) - Not for Diagnostic Use | Modified Installation]
        string clearCanvasClass = "WindowsForms10.Window.8.app.0.bf7d44_r9_ad1";
        string clearCanvasName = "PHENIX - Vafk,T,6 [ClearCanvas DICOM Viewer (Source) - Not for Diagnostic Use | Modified Installation]";

        string myWindowsClass = "ConsoleWindowClass";
        string myWindowsName = "file:///C:/Users/Sebastian/Documents/Visual Studio 2012/Projects/ConsoleApplication1/ConsoleApplication1/bin/Debug/ConsoleApplication1.EXE";
        bool myWindowFocused = true;
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

        public bool myWindowIsFocused()
        {
            return myWindowFocused;
        }

        public void changeFocusClearCanvas()
        {
            myWindowFocused = false;
            changeFocus(clearCanvasClass,clearCanvasName);
        }

        public void changeFocusMyWindow()
        {
            myWindowFocused = true;
            changeFocus(myWindowsClass,myWindowsName);
        }

        public void setCursorPositionXY(int X, int Y)
        {
            SetCursorPos(X, Y);
        }

        public void click()
        {
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            SendLeftClickDown((UInt32)X, (UInt32)Y);
            SendLeftClickUp((UInt32)X, (UInt32)Y);
        }
        public void rightClickDown()
        {
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            SendRightClickDown((UInt32)X, (UInt32)Y);
        }
        public void rightClickUp()
        {
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            SendRightClickUp((UInt32)X, (UInt32)Y);
        }
        public void zoomStart()
        {
            changeFocusClearCanvas();
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            SendRightClickDown((UInt32)X, (UInt32)Y);
            changeFocusMyWindow();
        }
        public void zoomStop()
        {
            changeFocusClearCanvas();
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            SendRightClickUp((UInt32)X, (UInt32)Y);
            changeFocusMyWindow();
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


        internal void zoom()
        {
            SendKeys.SendWait("z");
        }

        internal void pan()
        {
            SendKeys.SendWait("p");
        }

        internal void leftClickDown()
        {
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            SendLeftClickDown((UInt32)X, (UInt32)Y);
        }

        internal void leftClickUp()
        {
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            SendLeftClickUp((UInt32)X, (UInt32)Y);
        }
    }
}
