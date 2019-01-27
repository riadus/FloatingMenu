using System.Linq;
using Coinstantine.FloatingMenu.Abstractions;
using UIKit;

namespace Coinstantine.FloatingMenu.iOS.Extensions
{
    public static class UIFontExtensions
    {
        public static UIFont ToUIFont(this string key, IFonts fonts, float size)
        {
            var font = fonts.MenuItemFonts.First(x => x.Key == key);
            var fontFamily = CrossFloatingMenu.GetFontFamily(font.FontFamily);
            return UIFont.FromName(fontFamily, size);
        }

        public static UIColor ToColor(this string key, IFonts fonts)
        {
            var font = fonts.MenuItemFonts.First(x => x.Key == key);
            return font.TextColor.ToUIColor();
        }

        public static string ToCode(this string key, IFonts fonts)
        {
            return fonts.GetCode(key);
        }
    }
}
