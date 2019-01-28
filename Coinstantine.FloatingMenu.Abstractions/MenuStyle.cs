namespace Coinstantine.FloatingMenu.Abstractions
{
    public class MenuStyle : IMenuStyle
    {
        public MenuStyle()
        {
            Fonts = new AppFonts();
        }

        public AppColor CrossColor { get; set; }
        public IFonts Fonts { get; set; }
        public AppColor CircleColor { get; set; }
    }
}
