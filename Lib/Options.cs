using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib
{
    public class Options
    {
        public List<string> names;
        public Options()
        {
            names = new List<string>();
            names.Add("NONE");
            names.Add("ZOOM");
            names.Add("PAN");
        }
        public int size()
        {
            return names.Count;
        }
        
    }
}
