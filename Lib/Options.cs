using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib
{
    public class OptionsController
    {
        
        private List<Option> optionsList;
        public bool inZoom;
        public bool inPane;
        public bool inExtraZoom;
        public int optionSelected;
        public bool inContrast;
        public OptionsController()
        {
            optionsList = new List<Option>();
            optionsList.Add(Option.NONE);
            optionsList.Add(Option.ZOOMPAN);
            optionsList.Add(Option.CONTRAST);
            optionsList.Add(Option.ROTATE);
            optionSelected = 0;
            inContrast = false;
            inZoom = false;
            inPane = false;
            inExtraZoom = false;
        }
        public bool inAnyOptionInProcess()
        {
            return inZoom || inPane || inContrast;
        }
        public Option nextOption()
        {
            if (optionSelected < optionsList.Count - 1)
            {
                optionSelected++;
                return optionsList[optionSelected];
            }
            return optionsList[optionSelected];
        }
        public Option getOption()
        {
            return optionsList[optionSelected];
        }
        public Option previusOption()
        {
            if (optionSelected > 0)
            {
                optionSelected--;
                return optionsList[optionSelected];
            }
            return optionsList[optionSelected];
        }
    }
}
