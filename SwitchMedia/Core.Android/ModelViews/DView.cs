using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Switch.Core
{
    public enum DViewType { Circle, Square}
    public class DView
    {
        public DView(DViewType viewType, DPattern pattern, int x, int y, int radius)
        {
            ViewType = viewType;
            Pattern = pattern;
            X = x;
            Y = y;
            Radius = radius;
        }
        public DViewType ViewType { get; set; }
        public DPattern Pattern { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Radius { get; set; }        
    }
}