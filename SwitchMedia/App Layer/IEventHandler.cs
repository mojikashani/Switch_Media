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

namespace SwitchMedia.App_Layer
{
    public interface IEventHandler
    {
        bool onScreenClick(int x, int y);
        bool onScreenDoubleClick(int x, int y);
        bool onScreenDrag(int preX, int preY, int toX, int toY);
    }
}