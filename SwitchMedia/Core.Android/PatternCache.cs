using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Switch.Core
{
    public class PatternCache : IPatternCache
    {
        private Queue<DPattern> paterns;
        private Queue<DPattern> colors;
        private int maxLength;
        private int fillupThreshod;
        public delegate void StartDownloadingPatternsUntilFillup(DPatternType patternType);
        private StartDownloadingPatternsUntilFillup startDownloadingPatternsUntilFillup;

        public PatternCache(int _maxLength, int _fillupThreshod, StartDownloadingPatternsUntilFillup _startDownloadingPatternsUntilFillup)
        {
            startDownloadingPatternsUntilFillup = _startDownloadingPatternsUntilFillup;
            maxLength = _maxLength;
            fillupThreshod = _fillupThreshod;
            paterns = new Queue<DPattern>();
            colors = new Queue<DPattern>();
        }

        public DPattern DequeuePattern()
        {
            if(paterns.Count==0)
            {
                startDownloadingPatternsUntilFillup(DPatternType.Image);
                Random rand=new Random(DateTime.Now.Millisecond);
                return new DPattern(DPatternType.Color,"Auto",null,Convert.ToInt32(1677000*rand.NextDouble()));
            }else if(paterns.Count<fillupThreshod)
            {
                startDownloadingPatternsUntilFillup(DPatternType.Image);
                return paterns.Dequeue();
            }else
            {
                return paterns.Dequeue();
            }

        }

        public DPattern DequeueColor()
        {
            if (paterns.Count == 0)
            {
                startDownloadingPatternsUntilFillup(DPatternType.Color);
                Random rand = new Random(DateTime.Now.Millisecond);
                return new DPattern(DPatternType.Color, "Auto", null, Convert.ToInt32(1677000 * rand.NextDouble()));
            }
            else if (paterns.Count < fillupThreshod)
            {
                startDownloadingPatternsUntilFillup(DPatternType.Color);
                return paterns.Dequeue();
            }
            else
            {
                return paterns.Dequeue();
            }
        }

        void EequeuePattern(DPattern pattern)
        {
            paterns.Enqueue(pattern);
        }
        void EequeueColor(DPattern pattern)
        {
            colors.Enqueue(pattern);
        }

        bool IsCacheFill(DPatternType patternType)
        {
            if(patternType==DPatternType.Color)
            {
                if (colors.Count >= maxLength - 1)
                    return true;
            }else
            {
                if (paterns.Count >= maxLength - 1)
                    return true;
            }
            return false;
        }
    }
}