using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Util;
using SwitchMedia.App_Layer;
using Switch.Core;

namespace SwitchMedia.CustomViews
{
    public class MySurfaceView : SurfaceView, ISurfaceHolderCallback
    {
        ISurfaceHolder surfaceHolder;
        Context context;
        IEventHandler myEventHandler;

        public MySurfaceView(Context context, IAttributeSet attr)
            : base(context, attr)
        {
            this.context = context;

            surfaceHolder = Holder;
            surfaceHolder.AddCallback(this);
        }

        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int width, int height)
        {

        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            SetWillNotCacheDrawing(true);
            if(myEventHandler!= null)
                Refresh();
        }

        public void Refresh()
        {
            LinkedList<DView> views = myEventHandler.GetAllView();
            Canvas c = null;
            try
            {
                if (surfaceHolder.Surface.IsValid)
                {
                    c = surfaceHolder.LockCanvas();

                    lock (surfaceHolder)
                    {
                        Paint paint = new Paint();
                        Paint paintShader = new Paint();
                        paint.Color = Color.Black;
                        c.DrawRect(0, 0
                            , this.Width, this.Height, paint);
                        foreach (var view in views)
                        {
                            if(view.ViewType==DViewType.Circle)
                            {
                                Color color=new Color(view.Pattern.Color);
                                color.A=255;
                                paint.Color = color;
                                c.DrawCircle(view.X, view.Y, view.Radius, paint);
                            }else
                            {
                                if (view.Pattern.PatternType == DPatternType.Image)
                                {
                                    BitmapShader shader = new BitmapShader(view.Pattern.Image, Shader.TileMode.Repeat, Shader.TileMode.Repeat);
                                    paintShader.SetStyle(Paint.Style.Fill);
                                    paintShader.SetShader(shader);
                                    c.DrawRect(view.X - view.Radius, view.Y - view.Radius
                                        , view.X + view.Radius, view.Y + view.Radius, paintShader);

                                }else
                                {
                                    Color color = new Color(view.Pattern.Color);
                                    color.A = 255;
                                    paint.Color = color;
                                    c.DrawRect(view.X - view.Radius, view.Y - view.Radius
                                        , view.X + view.Radius, view.Y + view.Radius, paint);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error("MySurfaceView",ex.Message);
            }
            finally
            {
                if (c != null)
                    surfaceHolder.UnlockCanvasAndPost(c);
            }
            
        }

        public void SetEventHandler(IEventHandler _myEventHandler)
        {
            myEventHandler = _myEventHandler;
        }


        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
        }
    }
}