using System.Collections.Generic;

namespace Coinstantine.FloatingMenu.Abstractions
{
    public interface IFonts
    {
        IEnumerable<MenuItemFont> MenuItemFonts { get; set; }
        string GetCode(string value);
        string GetFontFamily(string key);
    }
}
