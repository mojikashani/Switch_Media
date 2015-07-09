using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Switch.Core
{
    public interface IViewCollection
    {
        void AddView(DView view);
        void UpdateView(DView view, DPattern newPattern);
        void MoveView(DView view, int newX, int newY);
        DView GetView(int x, int y);
        LinkedList<DView> GetAllView();


    }
}