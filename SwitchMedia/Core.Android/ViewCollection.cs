using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Switch.Core
{
    public class ViewCollection : IViewCollection
    {
        private LinkedList<DView> views;

        public ViewCollection()
        {
            views = new LinkedList<DView>();
        }
        public void AddView(DView view)
        {
            views.AddFirst(view);
        }

        public void UpdateView(DView view, DPattern newPattern)
        {
            view.Pattern = newPattern;
        }

        public void MoveView(DView view, int newX, int newY)
        {
            view.X = newX;
            view.Y = newY;
            views.Remove(view);
            views.AddFirst(view);
        }

        public DView GetView(int x, int y)
        {
            DView view = null;
            foreach(var viw in views)
            {
                if(viw.ViewType==DViewType.Circle)
                {
                    if(Math.Sqrt((viw.X-x)*(viw.X-x)+(viw.Y-y)*(viw.Y-y))<=viw.Radius)
                    {
                        view = viw;
                        break;
                    }
                }else if(viw.ViewType==DViewType.Square)
                {
                    if(Math.Abs(viw.X-x)<viw.Radius&&Math.Abs(viw.Y-y)<viw.Radius)
                    {
                        view = viw;
                        break;
                    }
                }
            }

            return view;
        }

        public LinkedList<DView> GetAllView()
        {
            return views;
        }
    }
}
