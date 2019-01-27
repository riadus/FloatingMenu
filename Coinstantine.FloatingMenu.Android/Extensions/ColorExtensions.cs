using Android.Graphics;
using Coinstantine.FloatingMenu.Abstractions;

namespace Coinstantine.FloatingMenu.Android.Extensions
{
    public static class ColorExtensions
    {
        public static Color ToAndroidColor(this AppColor appColor)
        {
            if (appColor.Alpha == null)
            {
                return new Color(appColor.Red, appColor.Green, appColor.Blue);

            }
            return new Color(appColor.Red, appColor.Green, appColor.Blue, appColor.Alpha.Value);
        }
    }
}
