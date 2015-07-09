using Switch.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SwitchMedia.App_Layer
{
    public class EventHandler : IEventHandler
    {
        public const int CACHE_MAX_LENGTH= 30;
        public const int CACHE_FILLUP_THRESHOD= 10;
        private IPatternCache patternCache;
        private IViewCollection viewCollection;
        private IMyHttpClient myHttpClient;
        private ISurfaceViewRefresher surfaceViewRefresher;
        private int minRadius;
        private int maxRadius;
        private int screenWidth;
        private int screenHeight;
        private bool isImageThreadRunning = false;
        private bool isColorThreadRunning = false;
        public EventHandler(int _screenWidth, int _screenHeight, int _minRadius, int _maxRadius)
        {
            screenHeight = _screenHeight;
            screenWidth = _screenWidth;
            minRadius = _minRadius;
            maxRadius = _maxRadius;
            patternCache = new PatternCache(CACHE_MAX_LENGTH, CACHE_FILLUP_THRESHOD, new PatternCache.StartDownloadingPatternsUntilFillup(StartDownloadingPatternsUntilFillup));
            viewCollection = new ViewCollection();
            myHttpClient = new MyHttpClient();
            surfaceViewRefresher = new SurfaceViewRefresher();

        }

        public void onScreenClick(int x, int y)
        {
            if(viewCollection.GetView(x, y)==null)
            {
                DView view = null;
                Random rand=new Random(DateTime.Now.Millisecond);
                int radius = minRadius + (int)(rand.NextDouble() * (maxRadius - minRadius));
                int screenX = Convert.ToInt32(screenWidth * rand.NextDouble());
                int screenY = Convert.ToInt32(screenHeight * rand.NextDouble());
                if ( rand.Next(10)%2 == 1)//circle
                {
                    view = new DView(DViewType.Circle, patternCache.DequeueColor(), screenX, screenY, radius);
                }else //square
                {
                    view = new DView(DViewType.Square, patternCache.DequeuePattern(), screenX, screenY, radius);
                }
                viewCollection.AddView(view);

                surfaceViewRefresher.Refresh();
            }
        }
        public void onScreenDoubleClick(int x, int y)
        {
            DView view = viewCollection.GetView(x, y);
            if(view!=null)
            {
                if (view.ViewType == DViewType.Circle)
                {
                    viewCollection.UpdateView(view, patternCache.DequeueColor());
                }else
                {
                    viewCollection.UpdateView(view, patternCache.DequeuePattern());
                }
                surfaceViewRefresher.Refresh();
            }

        }
        public void onScreenDrag(int preX, int preY, int toX, int toY)
        {
            DView view = viewCollection.GetView(preX, preY);
            if (view != null)
            {
                viewCollection.MoveView(view, toX, toY);

                surfaceViewRefresher.Refresh();
            }
        }

        private void StartDownloadingPatternsUntilFillup(DPatternType patternType)
        {
            if (patternType == DPatternType.Color)
            {
                if (!isImageThreadRunning)
                {
                    Thread thread = new Thread(new ThreadStart(downloadingPatterns));
                    thread.Start();
                }
            }
            else
            {
                if (!isColorThreadRunning)
                {
                    Thread thread = new Thread(new ThreadStart(downloadingColors));
                    thread.Start();
                }
            }
        }

        private void downloadingPatterns()
        {
            isImageThreadRunning = true;
            while (!patternCache.IsCacheFill(DPatternType.Image))
            {
               patternCache.EequeuePattern(myHttpClient.DownloadPattern());
            }
            isImageThreadRunning = false;
        }

        private void downloadingColors()
        {
            isColorThreadRunning = true;
            while (!patternCache.IsCacheFill(DPatternType.Color))
            {
                patternCache.EequeueColor(myHttpClient.DownloadColor());               
            }
            isColorThreadRunning = false;
        }

    }
}