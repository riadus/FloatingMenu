using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coinstantine.FloatingMenu.Abstractions
{
    public interface IFloatingMenu
    {
        void SetStyle(IMenuStyle style);
        Task ShowMenu(IEnumerable<MenuItemContext> items);
        Task HideMenu();
        Task ShowMenuFrom(IEnumerable<MenuItemContext> items, TouchLocation touchLocation);
    }

    public class AppColor
    {
        public int? Alpha { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
    }
}
