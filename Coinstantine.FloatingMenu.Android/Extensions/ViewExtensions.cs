using Android.Content;
using Android.Graphics;
using Android.Widget;

namespace Coinstantine.FloatingMenu.Android.Extensions
{
    public static class ViewExtensions
    {
        public static void ChangeStatus(this TextView textView, bool enabled)
        {
            if(!enabled)
            {
                textView.SetTextColor(Color.Gray);
            }
        }

        public static int ToPixels(this int dPixels, Context context)
        {
            return (int)(context.Resources.DisplayMetrics.Density * dPixels);
        }
    }
}
