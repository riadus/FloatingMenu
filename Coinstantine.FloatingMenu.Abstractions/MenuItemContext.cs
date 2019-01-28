using System.Windows.Input;

namespace Coinstantine.FloatingMenu.Abstractions
{
    public class MenuItemContext
    {
        public string IconText { get; set; }
        public string Text { get; set; }
        public ICommand SelectionCommand { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}
