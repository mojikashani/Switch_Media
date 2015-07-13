using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Switch.Core
{
    public class PatternCache : IPatternCache
    {
        public Queue<DPattern> paterns;
        public Queue<DPattern> colors;
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
                int gray = Convert.ToInt32(255 * rand.NextDouble());
                return new DPattern(DPatternType.Color,"Auto",null,gray+gray*256+gray*256*256);
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
            if (colors.Count == 0)
            {
                startDownloadingPatternsUntilFillup(DPatternType.Color);
                Random rand = new Random(DateTime.Now.Millisecond);
                int gray = Convert.ToInt32(255 * rand.NextDouble());
                return new DPattern(DPatternType.Color, "Auto", null, gray + gray * 256 + gray * 256 * 256);
            }
            else if (colors.Count < fillupThreshod)
            {
                startDownloadingPatternsUntilFillup(DPatternType.Color);
                return colors.Dequeue();
            }
            else
            {
                return colors.Dequeue();
            }
        }

        public void EequeuePattern(DPattern pattern)
        {
            paterns.Enqueue(pattern);
        }
        public void EequeueColor(DPattern pattern)
        {
            colors.Enqueue(pattern);
        }

        public bool IsCacheFill(DPatternType patternType)
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