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
        Window window = new Window();
        OptionsController optionsController = new OptionsController();
        int x, y;

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
            
            if (GestureLib.isKeyTap(gestures))
                window.click();
            if (GestureLib.isSwapLeft(gestures) && window.myWindowIsFocused())
                Console.WriteLine(optionsController.previusOption());
            if (GestureLib.isSwapRight(gestures) && window.myWindowIsFocused())
                Console.WriteLine(optionsController.nextOption());

            if (optionsController.getOption() == Option.CONTRAST)
            {
                if (GestureLib.hasonefinger(frame) && GestureLib.areInTouchZone(frame) && !optionsController.inAnyOptionInProcess())
                {
                    optionsController.inContrast = true;
                    window.contrast();
                    window.rightClickDown();
                }
                else if (GestureLib.hasonefinger(frame) && GestureLib.areOutTouchZone(frame) && optionsController.inContrast)
                {
                    optionsController.inContrast = false;
                    window.rightClickUp();
                }
            }
            if (optionsController.getOption() == Option.ROTATE)
            {
                if(GestureLib.isACircle(gestures) == DefautGestures.CircleRight)
                    window.rotateRight();
                else if (GestureLib.isACircle(gestures) == DefautGestures.CircleLeft)
                    window.rotateLeft();
                
            }
            if (optionsController.getOption() == Option.ZOOMPAN)
            {
                
                if (GestureLib.hasonefinger(frame) && GestureLib.areInTouchZone(frame) && !optionsController.inAnyOptionInProcess())
                {
                    optionsController.inPane = true;
                    window.pan();
                    window.leftClickDown();
                }
                else if (GestureLib.hasonefinger(frame) && GestureLib.areOutTouchZone(frame) && optionsController.inPane)
                {
                    optionsController.inPane = false;
                    window.leftClickUp();
                }
                if (GestureLib.hasTwoFingersEachHand(frame))
                {
                    if (GestureLib.areInTouchZone(frame) && !optionsController.inAnyOptionInProcess())
                    {
                        x = window.cursorX();
                        y = window.cursorY();
                        optionsController.inZoom = true;
                        window.zoom();
                        window.rightClickDown();
                    }
                    else if (GestureLib.areOutTouchZone(frame) && optionsController.inZoom)
                    {
                        optionsController.inZoom = false;
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
            if (optionsController.inZoom)
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
