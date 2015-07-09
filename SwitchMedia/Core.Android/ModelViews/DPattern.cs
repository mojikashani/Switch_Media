using Android.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Switch.Core
{
    public enum DPatternType { Color, Image }
    public class DPattern
    {
        public DPattern(DPatternType patternType, string title, Bitmap image, int color)
        {
            PatternType = patternType;
            Title = title;
            Image = image;
            Color = color;
        }
        public DPatternType PatternType { get; set; }
        public string Title { get; set; }
        public Bitmap Image { get; set; }
        public int Color { get; set; }
    }
}