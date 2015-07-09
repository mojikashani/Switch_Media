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
using Switch.Core;

namespace SwitchMedia.App_Layer
{
    public interface IMyHttpClient
    {
        DPattern DownloadPattern();
        DPattern DownloadColor();
    }
}