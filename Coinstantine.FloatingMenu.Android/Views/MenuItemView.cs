using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.Constraints;
using Android.Util;
using Android.Views;
using Android.Widget;
using Coinstantine.FloatingMenu.Abstractions;
using Coinstantine.FloatingMenu.Android.Extensions;

namespace Coinstantine.FloatingMenu.Android.Views
{
    public class MenuItemView : ConstraintLayout
    {
        private readonly MenuItemContext _menuItemContext;
        private readonly IMenuStyle _menuStyle;

        public MenuItemView(Context context, MenuItemContext menuItemContext, IMenuStyle menuStyle) : base(context)
        {
            _menuItemContext = menuItemContext;
            _menuStyle = menuStyle;
            InitializeComponents();
        }

        public void Populate()
        {
            _iconTextView.ChangeStatus(_menuItemContext.IsEnabled);

            _iconTextView.Text = _menuItemContext.IconText.ToCode(_menuStyle.Fonts);
            _iconTextView.Typeface = _menuItemContext.IconText.ToFontface(_menuStyle.Fonts);

            _titleTextView.Text = _menuItemContext.Text;
            _titleTextView.ChangeStatus(_menuItemContext.IsEnabled);

            _button.Click -= _button_Click;
            _button.Click += _button_Click;
        }

        void _button_Click(object sender, System.EventArgs e)
        {
            _menuItemContext.SelectionCommand.Execute(null);
        }

        private TextView _iconTextView { get; set; }
        private TextView _titleTextView { get; set; }
        private Button _button { get; set; }
        private View _iconContainer { get; set; }

        public void CreateView()
        {
            InitializeComponents();
            SetClipChildren(false);

            Background = new ColorDrawable(_menuStyle.CircleColor.ToAndroidColor());
        }

        private void InitializeComponents()
        {
            _iconContainer = BuildIconContainer();
            _iconTextView = BuildIcon();
            _button = BuildButton();
            CircleIcon();
            _titleTextView = BuildTitle();
            var constraintLayout = new ConstraintLayout(Context)
            {
                LayoutParameters = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
                {
                    BottomToBottom = LayoutParams.ParentId,
                    TopToTop = LayoutParams.ParentId
                },
                Id = GenerateViewId()
            };

            var size = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 30, Resources.DisplayMetrics);
            var iconContainerLayoutParams = new LayoutParams(size, size)
            {
                BottomToBottom = LayoutParams.ParentId,
                TopToTop = LayoutParams.ParentId
            };
            constraintLayout.AddView(_iconContainer, iconContainerLayoutParams);

            var iconLayoutParams = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
            {
                BottomToBottom = LayoutParams.ParentId,
                TopToTop = LayoutParams.ParentId,
                LeftToLeft = _iconContainer.Id,
                RightToRight = _iconContainer.Id
            };

            constraintLayout.AddView(_iconTextView, iconLayoutParams);

            AddView(constraintLayout);
            var editTextLayoutParams = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
            {
                LeftMargin = 30,
                TopToTop = LayoutParams.ParentId,
                BottomToBottom = LayoutParams.ParentId,
                LeftToRight = constraintLayout.Id
            };
            AddView(_titleTextView, editTextLayoutParams);

            var buttonLayoutParams = new LayoutParams(0, ViewGroup.LayoutParams.WrapContent)
            {
                LeftToLeft = LayoutParams.ParentId,
                RightToRight = LayoutParams.ParentId,
            };

            AddView(_button, buttonLayoutParams);
        }

        internal void CircleIcon()
        {
            _iconContainer.SetBackgroundResource(Resource.Drawable.floating_menu_circle_view);
        }

        internal void BigIcon()
        {
            _iconTextView.SetTextSize(ComplexUnitType.Dip, 30);
        }

        private View BuildIconContainer()
        {
            var iconContainer = new View(Context)
            {
                Id = GenerateViewId()
            };
            return iconContainer;
        }

        private TextView BuildIcon()
        {
            var textView = new TextView(Context);
            textView.SetTextColor(_menuItemContext.IconText.ToColor(_menuStyle.Fonts));
            return textView;
        }

        private TextView BuildTitle()
        {
            var textView = new TextView(Context);
            textView.SetTextColor(Color.White);
            textView.Typeface = Typeface.DefaultBold;
            return textView;
        }

        private Button BuildButton()
        {
            return new Button(Context)
            {
                Background = new ColorDrawable(Color.Transparent)
            };
        }
    }
}
