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
    public class MyHttpClient :IMyHttpClient
    {
        public Switch.Core.DPattern DownloadPattern()
        {
            throw new NotImplementedException();
        }

        public Switch.Core.DPattern DownloadColor()
        {
            throw new NotImplementedException();
        }
    }
}