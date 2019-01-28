using System.Linq;

namespace Coinstantine.FloatingMenu.Abstractions
{
    public interface IMenuStyle
    {
        AppColor CrossColor { get; set; }
        IFonts Fonts { get; set; }
        AppColor CircleColor { get; set; }
    }
}
