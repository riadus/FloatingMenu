using System.Collections.Generic;
using System.Linq;

namespace Coinstantine.FloatingMenu.Abstractions
{
    public class AppFonts : IFonts
    {
        public IEnumerable<MenuItemFont> MenuItemFonts { get; set; }

        public string GetCode(string key)
        {
            return MenuItemFonts.FirstOrDefault(x => x.Key == key)?.Code;
        }

        public string GetFontFamily(string key)
        {
            return MenuItemFonts.FirstOrDefault(x => x.Key == key)?.FontFamily;
        }
    }
}
