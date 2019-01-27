using UIKit;

namespace Coinstantine.FloatingMenu.iOS.Menu
{
    public class ItemIcon : UILabel
    {
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            Layer.CornerRadius = Frame.Width / 2;
			Layer.BackgroundColor = UIColor.White.CGColor;
            BackgroundColor = UIColor.Clear;
        }
    }
}
