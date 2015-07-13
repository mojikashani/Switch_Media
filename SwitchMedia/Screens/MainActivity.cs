using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SwitchMedia.CustomViews;
using SwitchMedia.App_Layer;

namespace SwitchMedia
{
    [Activity(Label = "SwitchMedia", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private MySurfaceView mySurfaceView;
        private IEventHandler eventHandler;
        private int oldX=0, oldY=0;
        private DateTime oldTime=DateTime.Now;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            mySurfaceView = FindViewById<MySurfaceView>(Resource.Id.mySurfaceView);
            var metrics = Resources.DisplayMetrics;
            eventHandler = new MyEventHandler(metrics.WidthPixels, metrics.HeightPixels, metrics.WidthPixels / 10, metrics.WidthPixels / 5, surfaceViewRefresh);
            mySurfaceView.SetEventHandler(eventHandler);
            mySurfaceView.Touch += SurfaceViewOnTouch;

        }

        private void SurfaceViewOnTouch(object sender, View.TouchEventArgs touchEventArgs)
        {
            int x=(int)touchEventArgs.Event.GetX();
            int y=(int)touchEventArgs.Event.GetY();
            int d2=(oldX-x)*(oldX-x)+(oldY-y)*(oldY-y);

            switch (touchEventArgs.Event.Action)// & MotionEventArgs.Mask)
            {
                case MotionEventActions.Down:
                    
                    
                    if(DateTime.Now-oldTime<new TimeSpan(0,0,0,0,400)&&d2<1000)
                    {
                        eventHandler.onScreenDoubleClick(x, y);
                    }
                    eventHandler.onScreenClick(x, y);
                    break;
                case MotionEventActions.Move:
                    eventHandler.onScreenDrag(oldX, oldY, x, y);
                    break;

                case MotionEventActions.Up:
                    break;

            }


            oldTime = DateTime.Now;
            oldX = x;
            oldY = y;
            
        }

        private void surfaceViewRefresh()
        {
            mySurfaceView.Refresh();
        }

    }
}

