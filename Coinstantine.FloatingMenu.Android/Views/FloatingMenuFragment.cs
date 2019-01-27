using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Coinstantine.FloatingMenu.Abstractions;

namespace Coinstantine.FloatingMenu.Android.Views
{
    public class FloatingMenuFragment : DialogFragment
    {
        private readonly Activity _activity;
        private readonly IMenuStyle _menuStyle;
        private readonly IEnumerable<MenuItemContext> _items;
        private readonly Func<Task> _dismissTask;

        public FloatingMenuFragment(Activity activity, IEnumerable<MenuItemContext> items, IMenuStyle menuStyle, Func<Task> dismissTask)
        {
            _activity = activity;
            _menuStyle = menuStyle;
            _items = items;
            _dismissTask = dismissTask;
        }

        public override void OnStart()
        {
            base.OnStart();
            if (Dialog != null)
            {
                var backgroundColor = new ColorDrawable(Color.Black);
                backgroundColor.SetAlpha(0);
                Dialog.Window.SetBackgroundDrawable(backgroundColor);
                Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var layout = new CircleViewsLayout(Context, _activity, _dismissTask, _menuStyle);
            layout.Build(_items);

            Dialog.Window.Attributes.WindowAnimations = Resource.Style.FloatingMenuDialogAnimation;
            return layout;
        }
    }
}
