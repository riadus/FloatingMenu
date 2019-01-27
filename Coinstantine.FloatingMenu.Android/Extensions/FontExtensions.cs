using System.Linq;
using Android.Graphics;
using Coinstantine.FloatingMenu.Abstractions;
using Plugin.CurrentActivity;

namespace Coinstantine.FloatingMenu.Android.Extensions
{
    public static class FontExtensions
    {
        public static Typeface ToFontface(this string key, IFonts fonts)
        {
            var font = fonts.MenuItemFonts.First(x => x.Key == key);
            var fontFamily = CrossFloatingMenu.GetFontFamily(font.FontFamily);
            return Typeface.CreateFromAsset(CrossCurrentActivity.Current.Activity.Assets, fontFamily);
        }

        public static Color ToColor(this string key, IFonts fonts)
        {
            var font = fonts.MenuItemFonts.First(x => x.Key == key);
            return font.TextColor.ToAndroidColor();
        }

        public static string ToCode(this string key, IFonts fonts)
        {
            return fonts.GetCode(key);
        }
    }
}
