using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Switch.Core
{
    public class PatternCacheTest : IPatternCache
    {
        public delegate void StartDownloadingPatternsUntilFillup(DPatternType patternType);
        
        public PatternCacheTest()
        {
            
        }

        public DPattern DequeuePattern()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            return new DPattern(DPatternType.Color, "Auto", null, Convert.ToInt32(16770000 * rand.NextDouble()));
        }

        public DPattern DequeueColor()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            return new DPattern(DPatternType.Color, "Auto", null, Convert.ToInt32(16770000 * rand.NextDouble()));
        }

        public void EequeuePattern(DPattern pattern)
        {
            
        }
        public void EequeueColor(DPattern pattern)
        {
            
        }

        public bool IsCacheFill(DPatternType patternType)
        {
            return true;
        }
    }
}