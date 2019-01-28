# FloatingMenu
Floating menu for Xamarin.iOS and Xamarin.Android with Font icons (like FontAwesome or others).

## Demo of FloatingMenu in Coinstantine App
![Demo on iOS](https://github.com/riadus/FloatingMenu/blob/master/Images/FloatingMenuiOS.gif)
![Demo on Android](https://github.com/riadus/FloatingMenu/blob/master/Images/FloatingMenuAndroid.gif)

## Setup

### iOS
#### Register the service in the DI Container
You will need to register the service to be able to use it.
```csharp
// With MvvmCross
Mvx.RegisterType(() => Coinstantine.FloatingMenu.CrossFloatingMenu.Current);
```
#### Add the fonts
You need to add the fonts you'll be using for the icons. 
First, you will have to add the font files (otf, ttf) to the iOS resources folder (under Fonts for instance). And register them in the PList.info file
```xml
<key>UIAppFonts</key>
<array>
  <string>Fonts/Font Awesome 5 Free-Regular-400.otf</string>
  <string>Fonts/Font Awesome 5 Brands-Regular-400.otf</string>
  <string>Fonts/Font Awesome 5 Free-Solid-900.otf</string>
  <string>Fonts/icomoon.ttf</string>
</array>
```
Then, you have to add a `key` per font. The same key needs to be used in the core project.

```csharp
CrossFloatingMenu.AddFont(key, fontFamily);
CrossFloatingMenu.AddFont("FontAwesomeSolid", "FontAwesome5FreeSolid");
CrossFloatingMenu.AddFont("Icomoon", "icomoon");
```

If you don't know the family name of your font, you can retreive those using ```UIFontExtensions.PrintAllFonts()```. This method will print all the fonts registered in your iOS app.

### Android
#### Register the service in the DI Container
You will need to register the service to be able to use it.
```csharp
// With MvvmCross
Mvx.RegisterType(() => Coinstantine.FloatingMenu.CrossFloatingMenu.Current);
```
#### Set the resolver of the current activity
You will also have to set the resolver of the current activity. This function is used to get the current activity while shouwing the menu.
If you're using [Current Activity Plugin](https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/CurrentActivity) from James Montemagno, you can set the resolver in this way in SplashScreenActivity.
```csharp
protected override void OnCreate(Bundle bundle)
  {
      base.OnCreate(bundle);
      ...
      CrossCurrentActivity.Current.Init(this, bundle);
      CrossFloatingMenu.SetCurrentActivityResolver(() => CrossCurrentActivity.Current.Activity);
      ...
  }
```
#### Add the fonts
You need to add the font files (otf, ttf) to the Android Asset folder.
Then, you have to add the same font keys you did for iOS. Remember those keys need to be used in the core projet.
```csharp
CrossFloatingMenu.AddFont(key, familyName);
CrossFloatingMenu.AddFont("FontAwesomeSolid", "Font Awesome 5 Free-Solid-900.otf");
CrossFloatingMenu.AddFont("Icomoon", "icomoon.ttf");
```
Here, the family name is the name file, including the extension.

### Core

#### IFloatingMenu
The interaction with the menu happens with this service ```IFloatingMenu```

```csharp
public interface IFloatingMenu
{
    void SetStyle(IMenuStyle style);
    Task ShowMenu(IEnumerable<MenuItemContext> items);
    Task HideMenu();
    Task ShowMenuFrom(IEnumerable<MenuItemContext> items, TouchLocation touchLocation);
}
```
#### MenuItemContext

```csharp
public class MenuItemContext
{
    public string IconText { get; set; }
    public string Text { get; set; }
    public ICommand SelectionCommand { get; set; }
    public bool IsEnabled { get; set; } = true;
}
```

Every icon shown on the menu is a ```MenuItemContext```. To show the menu you have to provide a list of MenuItemContexts
* *IconText* : is the key of your icon. The one you define while setting the style. (wallet in the example, see below)
* *Text* : is the text that will be shown on the right of the icon
* *SelectionCommand* : is the command that will be executed when the item is selected
* *IsEnabled* : if false, will show the icon and the text grayed out.

```csharp
Items = new List<MenuItemContext>
            {
                new MenuItemContext
                {
                    Text = "Your wallet",
                    IconText = "wallet",
                    SelectionCommand = WalletCommand,
                },
                new MenuItemContext
                {
                    Text = "See your aidrops",
                    IconText = "airdrop",
                    IsEnabled = false,
                },
                ...
              }              
```
```csharp
public IMvxCommand WalletCommand => new MvxCommand(Wallet);
private void Wallet()
{
    FloatingMenu.HideMenu();
    NavigationService.ShowWallet();
}
```

#### Menu Style
Before showing the menu, you need to define its style
```csharp
public interface IMenuStyle
{
    AppColor CrossColor { get; set; }
    IFonts Fonts { get; set; }
    AppColor CircleColor { get; set; }
}
```
* *CrossColor* : using an AppColor object that defines Alpha, Red, Green and Blue [0 - 255], will give the color of the cross on the middle of the screen. Used to close the menu.
* *CircleColor* : using also an AppColor, will give the main color of the menu. The smalled circle will be 100% opaque and the biggest the circle is, the less opaque it will be.
##### IFonts
You can use ```AppFonts``` which needs to be populated with a list of ```MenuItemFont```
##### MenuItemFont
```csharp
public class MenuItemFont
{
    public string FontFamily { get; set; }
    public string Code { get; set; }
    public string Key { get; set; }
    public AppColor TextColor { get; set; }
}
```
* *FontFamily* : is the font key you defined in the setup of every plateform. ("FontAwesomeSolid" for example)
* *Key* : the key of the icon you want to use. Can be anything readable. ("wallet" for example)
* *Code* : the code of the icon in the font ("\uf555" is the code to define to use wallet icon of Font Awesome)

```csharp
var style = new MenuStyle
            {
                CrossColor = new AppColor { Red = XX, Blue = XX, Green = XX },
                CircleColor = new AppColor { Red = XX, Blue = XX, Green = XX },

                Fonts = new AppFonts
                {
                    MenuItemFonts = new List<MenuItemFont>
                    {
                        new MenuItemFont
                        {
                            Key = "wallet",
                            Code = "\uf555",
                            FontFamily = "FontAwesomeSolid",
                            TextColor = new AppColor { Red = XX, Blue = XX, Green = XX }
                        },
                        new MenuItemFont
                        {
                            Key = "airdrop",
                            Code = "\ue901",
                            FontFamily = "Icomoon",
                            TextColor = new AppColor { Red = XX, Blue = XX, Green = XX }
                        }
                  },
                  ...
            }
            
FloatingMenu.SetStyle(style);            
```
Then you call show the menu. 
```csharp
FloatingMenu.ShowMenu(Items);
```

For iOS, you can also have the menu opened from a specific point of the screen.

```csharp
TouchLocation touchLocation = new TouchLocation(x, y);
FloatingMenu.ShowMenu(Items, touchLocation);
```











