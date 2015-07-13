using Switch.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SwitchMedia.App_Layer
{
    public class MyEventHandler : IEventHandler
    {
        public const int CACHE_MAX_LENGTH= 50;
        public const int CACHE_FILLUP_THRESHOD= 30;
        private IPatternCache patternCache;
        private IViewCollection viewCollection;
        private IMyHttpClient myHttpClient;
        private int minRadius;
        private int maxRadius;
        private int screenWidth;
        private int screenHeight;
        private bool isImageThreadRunning = false;
        private bool isColorThreadRunning = false;
        public delegate void SurfaceViewRefresh();
        private SurfaceViewRefresh surfaceViewRefresh;
        public MyEventHandler(int _screenWidth, int _screenHeight, int _minRadius, int _maxRadius, SurfaceViewRefresh _surfaceViewRefresh)
        {
            surfaceViewRefresh = _surfaceViewRefresh;
            screenHeight = _screenHeight;
            screenWidth = _screenWidth;
            minRadius = _minRadius;
            maxRadius = _maxRadius;
            patternCache = new PatternCache(CACHE_MAX_LENGTH, CACHE_FILLUP_THRESHOD, new PatternCache.StartDownloadingPatternsUntilFillup(StartDownloadingPatternsUntilFillup));
            //patternCache = new PatternCacheTest();
            viewCollection = new ViewCollection();
            myHttpClient = new MyHttpClient();

            StartDownloadingPatternsUntilFillup(DPatternType.Color);
            StartDownloadingPatternsUntilFillup(DPatternType.Image);

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

                surfaceViewRefresh();
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

                surfaceViewRefresh();

            }

        }
        public void onScreenDrag(int preX, int preY, int toX, int toY)
        {
            DView view = viewCollection.GetView(preX, preY);
            if (view != null)
            {
                viewCollection.MoveView(view, toX, toY);

                surfaceViewRefresh();
            }
        }

        private void StartDownloadingPatternsUntilFillup(DPatternType patternType)
        {
            if (patternType == DPatternType.Image)
            {
                if (!isImageThreadRunning)
                    downloadingPatterns();
            }
            else
            {
                if (!isColorThreadRunning)
                    downloadingColors();
            }
        }

        private async void downloadingPatterns()
        {
            isImageThreadRunning = true;
            await Task.Run(() =>
            {
                while (!patternCache.IsCacheFill(DPatternType.Image))
                {
                    patternCache.EequeuePattern(myHttpClient.DownloadPattern());
                    
                }
            });
            isImageThreadRunning = false;
        }

        private async void downloadingColors()
        {
            isColorThreadRunning = true;
            await Task.Run(() => 
            {
                while (!patternCache.IsCacheFill(DPatternType.Color))
                {
                patternCache.EequeueColor(myHttpClient.DownloadColor());               
                }
            });
                        
            isColorThreadRunning = false;            
        }

        public LinkedList<DView> GetAllView()
        {
            return viewCollection.GetAllView();
        }
    }
}