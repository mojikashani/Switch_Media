using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Switch.Core
{
    public interface IPatternCache
    {
        DPattern DequeuePattern();
        DPattern DequeueColor();
        void EequeuePattern(DPattern pattern);
        void EequeueColor(DPattern pattern);
        bool IsCacheFill(DPatternType patternType);
    }
}