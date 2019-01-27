using System;
using Coinstantine.FloatingMenu.Abstractions;
using Coinstantine.FloatingMenu.iOS.Extensions;
using CoreGraphics;
using UIKit;

namespace Coinstantine.FloatingMenu.iOS.Menu
{
    public class ItemView : UIView
    {
        private readonly IFonts _fonts;

        public nfloat IconWidth { get; }

        public CGPoint PreferedLocation { get; set; }

        public double RightLimit { get; set; }

		public MenuItemContext DataContext { get; set; }

        public ItemView(nfloat iconWidth, IFonts fonts)
        {
            IconWidth = iconWidth;
            _fonts = fonts;
        }

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			if (DataContext.SelectionCommand != null)
			{
				AddGestureRecognizer(new UITapGestureRecognizer(() => DataContext.SelectionCommand.Execute(null)));
			}

			var icon = new ItemIcon
			{
				Frame = new CGRect(CGPoint.Empty, new CGSize(IconWidth, IconWidth)),
				Text = DataContext.IconText.ToCode(_fonts),
				TextAlignment = UITextAlignment.Center,
				Enabled = DataContext.IsEnabled,
				Font = DataContext.IconText.ToUIFont(_fonts, 16),
				TextColor = DataContext.IconText.ToColor(_fonts)
            };

            var label = new UILabel
            {
                Text = DataContext.Text,
                Enabled = DataContext.IsEnabled,
                TextColor = UIColor.White,
                Font = UIFont.BoldSystemFontOfSize(16),
                Lines = 0
			};
			var labelSize = label.SizeThatFits(CGSize.Empty);
			var labelLocation = new CGPoint(IconWidth + 10, 3);
            if(labelSize.Width + labelLocation.X > RightLimit / 2)
            {
                labelSize.Width = ((nfloat) RightLimit / 2f) - labelLocation.X;
                labelSize.Height *= 2;
            }

			label.Frame = new CGRect(labelLocation, labelSize);
			Frame = new CGRect(PreferedLocation, new CGSize(IconWidth + labelSize.Width + 50, labelSize.Height + 20));
			Add(icon);
			Add(label);
		}
	}
}
