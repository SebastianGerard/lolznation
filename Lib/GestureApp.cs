using Leap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Lib
{
    public class GestureApp: Listener
    {
        Window window = new Window();
       
        int x, y;
        Form form;
        public bool debbugMode = true;
        public override void OnInit(Controller controller)
        {
            controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
            controller.EnableGesture(Gesture.GestureType.TYPESWIPE);
            controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
            
        }
        public override void OnFrame (Controller controller)
	    {
            Leap.Frame frame = controller.Frame();
            Leap.GestureList gestures = frame.Gestures();

            if (GestureLib.hastwofingers(frame) && GestureLib.isKeyTap(gestures))
                window.clickRight();
            else
            if (GestureLib.isKeyTap(gestures))
                window.click();
            
            if (GestureLib.isSwapLeft(gestures) && window.myWindowIsFocused())
                Console.WriteLine(OptionsController.Instance.previusOption());
            if (GestureLib.isSwapRight(gestures) && window.myWindowIsFocused())
                Console.WriteLine(OptionsController.Instance.nextOption());

            if (OptionsController.Instance.getOption() == Option.CONTRAST && !window.myWindowIsFocused())
            {
                if (GestureLib.hasonefinger(frame) && GestureLib.areInTouchZone(frame) && !OptionsController.Instance.inAnyOptionInProcess())
                {
                    OptionsController.Instance.inContrast = true;
                    window.contrast();
                    window.rightClickDown();
                }
                else if (GestureLib.hasonefinger(frame) && GestureLib.areOutTouchZone(frame) && OptionsController.Instance.inContrast)
                {
                    OptionsController.Instance.inContrast = false;
                    window.rightClickUp();
                }
            }
            if (OptionsController.Instance.getOption() == Option.ROTATE && !window.myWindowIsFocused())
            {
                if(GestureLib.isACircle(gestures) == DefautGestures.CircleRight)
                    window.rotateRight();
                else if (GestureLib.isACircle(gestures) == DefautGestures.CircleLeft)
                    window.rotateLeft();
                
            }
            if (OptionsController.Instance.getOption() == Option.ZOOMPAN && !window.myWindowIsFocused())
            {
                
                if (GestureLib.hasonefinger(frame) && GestureLib.areInTouchZone(frame) && !OptionsController.Instance.inAnyOptionInProcess())
                {
                    OptionsController.Instance.inPane = true;
                    window.pan();
                    window.leftClickDown();
                }
                else if (GestureLib.hasonefinger(frame) && GestureLib.areOutTouchZone(frame) && OptionsController.Instance.inPane)
                {
                    OptionsController.Instance.inPane = false;
                    window.leftClickUp();
                }
                if (GestureLib.hasTwoFingersEachHand(frame))
                {
                    if (GestureLib.areInTouchZone(frame) && !OptionsController.Instance.inAnyOptionInProcess())
                    {
                        x = window.cursorX();
                        y = window.cursorY();
                        OptionsController.Instance.inZoom = true;
                        window.zoom();
                        window.rightClickDown();
                    }
                    else if (GestureLib.areOutTouchZone(frame) && OptionsController.Instance.inZoom)
                    {
                        OptionsController.Instance.inZoom = false;
                        window.rightClickUp();
                    }
                }
                
            }
            /*if (isClick(gestures) )
            {
                if (fist || rightClickDown)
                {
                    fist = false;
                    rightClickDown = false;
                    window.rightClickUp();
                    window.leftClickUp();
                }
                wantToChangeOptions = false;
                window.click();
                
            }
            if (!wantToChangeOptions)
            {
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

                if (hasTwoFingers(frame) && option == 1)
                {
                   // drawCursorVertically(frame);
                    if (rightClickDown && areOutTouchZone(frame))
                    {
                        rightClickDown = false;
                        window.rightClickUp();
                    }
                    else if (!rightClickDown && areInTouchZone(frame))
                    {
                        rightClickDown = true;
                        window.zoom();
                        window.rightClickDown();
                    }
                }
                if (!hasTwoFingers(frame) && rightClickDown)
                {
                    rightClickDown = false;
                    window.rightClickUp();
                }
                    
            }*/
            if (OptionsController.Instance.inZoom)
                drawCursorVert(frame);
            else
                drawCursor(frame);
            /*changeFocus(gestures);
            selectOptions(gestures);*/
            
        }

        private void drawCursor(Frame frame)
        {
            Vector normalizedPosition = GestureLib.getStabilizedPalmPosition(frame);
                float tx = normalizedPosition.x * window.windowWidth;
                float ty = window.windowHeight - normalizedPosition.y * window.windowHeight;
            window.setCursorPositionXY((int)tx, (int)ty);

        }
        private void drawCursorVert(Frame frame)
        {
            Vector normalizedPosition = GestureLib.getStabilizedPalmPosition(frame);
            float tx =normalizedPosition.x * window.windowWidth;
            int dxInt = (int)tx;
            int dif = x-dxInt;
            //float ty = window.windowHeight - normalizedPosition.y * window.windowHeight;
            window.setCursorPositionXYDefaultX((int)y+dif);

        }
    }
}
