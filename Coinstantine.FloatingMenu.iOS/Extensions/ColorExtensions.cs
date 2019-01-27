using System;
using Coinstantine.FloatingMenu.Abstractions;
using CoreGraphics;
using UIKit;

namespace Coinstantine.FloatingMenu.iOS.Extensions
{
    public static class ColorExtensions
    {
        public static UIColor ToUIColor(this AppColor appColor)
        {
            if (appColor.Alpha == null)
            {
                return UIColor.FromRGB(appColor.Red / 255f, appColor.Green / 255f, appColor.Blue / 255f);

            }
            return UIColor.FromRGBA((nfloat)appColor.Red / 255f, (nfloat)appColor.Green / 255f, (nfloat)appColor.Blue / 255f, (nfloat)appColor.Alpha / 255f);
        }

        public static CGColor ToCGColor(this AppColor self)
        {
            return self.ToUIColor().CGColor;
        }
    }
}
