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
}
