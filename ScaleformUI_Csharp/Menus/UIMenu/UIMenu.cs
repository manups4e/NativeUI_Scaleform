using CitizenFX.Core;
using CitizenFX.Core.UI;
using ScaleformUI.Elements;
using ScaleformUI.Menus;
using ScaleformUI.Scaleforms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using Control = CitizenFX.Core.Control;

namespace ScaleformUI.Menu
{
    #region Delegates
    public delegate void IndexChangedEvent(UIMenu sender, int newIndex);
    public delegate void ListChangedEvent(UIMenu sender, UIMenuListItem listItem, int newIndex);
    public delegate void SliderChangedEvent(UIMenu sender, UIMenuSliderItem listItem, int newIndex);
    public delegate void ListSelectedEvent(UIMenu sender, UIMenuListItem listItem, int newIndex);
    public delegate void CheckboxChangeEvent(UIMenu sender, UIMenuCheckboxItem checkboxItem, bool Checked);
    public delegate void ItemSelectEvent(UIMenu sender, UIMenuItem selectedItem, int index);
    public delegate void ItemActivatedEvent(UIMenu sender, UIMenuItem selectedItem);
    public delegate void ItemCheckboxEvent(UIMenuCheckboxItem sender, bool Checked);
    public delegate void ItemListEvent(UIMenuListItem sender, int newIndex);
    public delegate void ItemSliderEvent(UIMenuSliderItem sender, int newIndex);
    public delegate void ItemSliderProgressEvent(UIMenuProgressItem sender, int newIndex);
    public delegate void OnProgressChanged(UIMenu menu, UIMenuProgressItem item, int newIndex);
    public delegate void OnProgressSelected(UIMenu menu, UIMenuProgressItem item, int newIndex);
    public delegate void StatItemProgressChange(UIMenu menu, UIMenuStatsItem item, int value);
    public delegate void ColorPanelChangedEvent(UIMenuItem menu, UIMenuColorPanel panel, int index);
    public delegate void VehicleColorPickerSelectEvent(UIMenuItem menu, UIVehicleColourPickerPanel panel, int index, SColor color);
    public delegate void VehicleColorPickerHoverEvent(int index, SColor color);
    public delegate void PercentagePanelChangedEvent(UIMenuItem menu, UIMenuPercentagePanel panel, float value);
    public delegate void GridPanelChangedEvent(UIMenuItem menu, UIMenuGridPanel panel, PointF value);
    public delegate void MenuOpenedEvent(UIMenu menu, dynamic data = null);
    public delegate void MenuClosedEvent(UIMenu menu);
    public delegate void ItemHighlightedEvent(UIMenu menu, UIMenuItem item);
    public delegate void MenuFilteringFailedEvent(UIMenu menu);
    public delegate void ExtensionMethodEvent(UIMenu menu);

    public enum MenuAnimationType
    {
        LINEAR = 0,
        QUADRATIC_IN,
        QUADRATIC_OUT,
        QUADRATIC_INOUT,
        CUBIC_IN,
        CUBIC_OUT,
        CUBIC_INOUT,
        QUARTIC_IN,
        QUARTIC_OUT,
        QUARTIC_INOUT,
        SINE_IN,
        SINE_OUT,
        SINE_INOUT,
        BACK_IN,
        BACK_OUT,
        BACK_INOUT,
        CIRCULAR_IN,
        CIRCULAR_OUT,
        CIRCULAR_INOUT
    }

    public enum MenuBuildingAnimation
    {
        NONE,
        LEFT,
        RIGHT,
        LEFT_RIGHT,
    }

    public enum ScrollingType
    {
        CLASSIC,
        PAGINATED,
        ENDLESS
    }

    public enum MenuAlignment
    {
        LEFT,
        RIGHT
    }

    #endregion

    /// <summary>
    /// Base class for ScaleformUI. Calls the next events: OnIndexChange, OnListChanged, OnCheckboxChange, OnItemSelect, OnMenuOpen, OnMenuClose.
    /// </summary>
    public class UIMenu : MenuBase
    {
        #region Private Fields
        private bool _visible;
        private bool _justOpened = true;
        private bool _itemsDirty = false;
        //internal PaginationHandler Pagination;
        private int _maxItemsOnScreen = 7;
        private int _currentSelection = 0;
        private int _visibleItems = 0;
        private int topEdge = 0;

        internal KeyValuePair<string, string> _customTexture;
        internal KeyValuePair<string, string> _customBGTexture = new KeyValuePair<string, string>("", "");

        internal UIMenuItem ParentItem { get; set; }

        /// <summary>
        /// Players won't be able to close the menu if this is false! Make sure players can close the menu in some way!!!!!!
        /// </summary>
        private bool canPlayerCloseMenu = true;
        //Pagination
        private bool mouseWheelControlEnabled = true;
        private int menuSound;
        private bool _changed = true;
        private bool keyboard = false;
        //Keys
        private readonly Dictionary<MenuControls, List<Tuple<Control, int>>> _keyDictionary = new Dictionary<MenuControls, List<Tuple<Control, int>>>();

        private readonly ScaleformWideScreen _menuGlare;

        private static readonly MenuControls[] _menuControls = Enum.GetValues(typeof(MenuControls)).Cast<MenuControls>().ToArray();

        private bool isBuilding = false;
        private string title;
        private string subtitle;
        private SColor counterColor = SColor.HUD_Freemode;

        public bool Glare { get; set; }
        private float fSavedGlareDirection;
        private Vector2 glarePosition;
        private SizeF glareSize;

        internal readonly static string _selectTextLocalized = Game.GetGXTEntry("HUD_INPUT2");
        internal readonly static string _backTextLocalized = Game.GetGXTEntry("HUD_INPUT3");
        protected readonly SizeF Resolution = ScreenTools.ResolutionMaintainRatio;

        // Button delay
        private int time;
        private int times;
        private int delay = 100;
        private int delayBeforeOverflow = 350;
        private int timeBeforeOverflow;
        #endregion

        #region Public Fields

        public string AUDIO_LIBRARY = "HUD_FRONTEND_DEFAULT_SOUNDSET";

        public string AUDIO_UPDOWN = "NAV_UP_DOWN";
        public string AUDIO_LEFTRIGHT = "NAV_LEFT_RIGHT";
        public string AUDIO_SELECT = "SELECT";
        public string AUDIO_BACK = "BACK";
        public string AUDIO_ERROR = "ERROR";
        public HudColor SubtitleColor = HudColor.NONE;
        internal SColor bannerColor = SColor.HUD_None;
        internal SColor bannerBGColor = SColor.HUD_None;

        public List<UIMenuItem> MenuItems = new List<UIMenuItem>();
        public List<UIMenuItem> _unfilteredMenuItems = new List<UIMenuItem>();

        public bool MouseEdgeEnabled = true;
        public bool ControlDisablingEnabled = true;
        internal bool leftClickEnabled;
        public MenuAlignment MenuAlignment
        {
            get => menuAlignment;
            set
            {
                menuAlignment = value;
                SetMenuOffset(Offset);
                if (Visible)
                {
                    //Main.scaleformUI.CallFunction("SET_MENU_ORIENTATION", (int)menuAlignment);
                    SetMenuData(true);
                }
            }
        }
        public int MaxItemsOnScreen
        {
            get => _maxItemsOnScreen;
            set
            {
                _maxItemsOnScreen = value;
            }
        }


        [Obsolete("Not used anymore")]
        public bool EnableAnimation;
        [Obsolete("Not used anymore")]
        public bool Enabled3DAnimations;
        [Obsolete("Not used anymore")]
        public MenuAnimationType AnimationType;
        [Obsolete("Not used anymore")]
        public MenuBuildingAnimation BuildingAnimation;

        [Obsolete("Not used anymore")]
        public ScrollingType ScrollingType;

        public bool MouseWheelControlEnabled
        {
            get => mouseWheelControlEnabled;
            set
            {
                mouseWheelControlEnabled = value;
                if (value)
                {
                    SetKey(MenuControls.Up, Control.CursorScrollUp);
                    SetKey(MenuControls.Down, Control.CursorScrollDown);
                }
                else
                {
                    ResetKey(MenuControls.Up);
                    ResetKey(MenuControls.Down);
                    SetKey(MenuControls.Up, Control.FrontendUp);
                    SetKey(MenuControls.Down, Control.FrontendDown);
                }
            }
        }

        public ItemFont DescriptionFont
        {
            get => descriptionFont;
            set
            {
                descriptionFont = value;
                if (Visible)
                {
                    //Main.scaleformUI.CallFunction("SET_DESC_FONT", descriptionFont.FontName, descriptionFont.FontID);
                    SetMenuData(true);
                }
            }
        }

        private bool _mouseOnMenu;
        public bool IsMouseOverTheMenu => _mouseOnMenu;

        public bool ResetCursorOnOpen = true;
        private bool mouseControlsEnabled = true;
        public bool AlternativeTitle = false;
        private bool canBuild = true;
        private bool isFading;
        internal bool differentBanner = false;
        internal bool itemless = false;
        public PointF Offset { get; internal set; }

        public List<UIMenuWindow> Windows = new List<UIMenuWindow>();

        #endregion

        #region Events

        /// <summary>
        /// Called when user presses up or down, changing current selection.
        /// </summary>
        public event IndexChangedEvent OnIndexChange;

        /// <summary>
        /// Called when user presses left or right, changing a list position.
        /// </summary>
        public event ListChangedEvent OnListChange;

        /// <summary>
        /// Called when user selects a list item.
        /// </summary>
        public event ListSelectedEvent OnListSelect;

        /// <summary>
        /// Called when user presses left or right, changing a slider position.
        /// </summary>
        public event SliderChangedEvent OnSliderChange;

        /// <summary>
        /// Called when user presses left or right, changing a the index of a color panel.
        /// </summary>
        public event ColorPanelChangedEvent OnColorPanelChange;

        /// <summary>
        /// Called when user changes the value of a percentage panel.
        /// </summary>
        public event PercentagePanelChangedEvent OnPercentagePanelChange;

        /// <summary>
        /// Called when user changes value of a grid panel.
        /// </summary>
        public event GridPanelChangedEvent OnGridPanelChange;

        /// <summary>
        /// Called When user changes progress in a ProgressItem.
        /// </summary>
        public event OnProgressChanged OnProgressChange;

        /// <summary>
        /// Called when user either clicks on a ProgressItem.
        /// </summary>
        public event OnProgressSelected OnProgressSelect;

        /// <summary>
        /// Called when user presses enter on a checkbox item.
        /// </summary>
        public event CheckboxChangeEvent OnCheckboxChange;

        /// <summary>
        /// Called when user selects a simple item.
        /// </summary>
        public event ItemSelectEvent OnItemSelect;

        public event MenuOpenedEvent OnMenuOpen;
        public event MenuClosedEvent OnMenuClose;
        public event MenuFilteringFailedEvent OnFilteringFailed;
        public event ExtensionMethodEvent ExtensionMethod;

        /// <summary>
        /// Called every time a Stat item changes value
        /// </summary>
        public event StatItemProgressChange OnStatsItemChanged;
        #endregion

        #region Constructors

        /// <summary>
        /// Basic Menu constructor.
        /// </summary>
        /// <param name="title">Title that appears on the big banner.</param>
        /// <param name="subtitle">Subtitle that appears in capital letters in a small black bar.</param>
        /// <param name="glare">Add menu Glare scaleform?.</param>
        public UIMenu(string title, string subtitle, bool glare = false, bool alternativeTitle = false, MenuAlignment menuAlignment = MenuAlignment.LEFT) : this(title, subtitle, new PointF(0, 0), "", "", glare, alternativeTitle, menuAlignment)
        {
        }


        /// <summary>
        /// Basic Menu constructor with an offset.
        /// </summary>
        /// <param name="title">Title that appears on the big banner.</param>
        /// <param name="subtitle">Subtitle that appears in capital letters in a small black bar. Set to "" if you dont want a subtitle.</param>
        /// <param name="offset">PointF object with X and Y data for offsets. Applied to all menu elements.</param>
        /// <param name="glare">Add menu Glare scaleform?.</param>
        /// <param name="alternativeTitle">Set the alternative type to the title?.</param>
        public UIMenu(string title, string subtitle, PointF offset, bool glare = false, bool alternativeTitle = false, MenuAlignment menuAlignment = MenuAlignment.LEFT) : this(title, subtitle, offset, "", "", glare, alternativeTitle, menuAlignment)
        {
        }

        /// <summary>
        /// Initialise a menu with a custom texture banner.
        /// </summary>
        /// <param name="title">Title that appears on the big banner. Set to "" if you don't want a title.</param>
        /// <param name="subtitle">Subtitle that appears in capital letters in a small black bar. Set to "" if you dont want a subtitle.</param>
        /// <param name="offset">PointF object with X and Y data for offsets. Applied to all menu elements.</param>
        /// <param name="customBanner">Path to your custom texture.</param>
        /// <param name="glare">Add menu Glare scaleform?.</param>
        /// <param name="alternativeTitle">Set the alternative type to the title?.</param>
        public UIMenu(string title, string subtitle, PointF offset, KeyValuePair<string, string> customBanner, bool glare = false, bool alternativeTitle = false, MenuAlignment menuAlignment = MenuAlignment.LEFT) : this(title, subtitle, offset, customBanner.Key, customBanner.Value, glare, alternativeTitle, menuAlignment)
        {
        }


        /// <summary>
        /// Advanced Menu constructor that allows custom title banner.
        /// </summary>
        /// <param name="title">Title that appears on the big banner. Set to "" if you are using a custom banner.</param>
        /// <param name="subtitle">Subtitle that appears in capital letters in a small black bar.</param>
        /// <param name="offset">PointF object with X and Y data for offsets. Applied to all menu elements.</param>
        /// <param name="spriteLibrary">Sprite library name for the banner.</param>
        /// <param name="spriteName">Sprite name for the banner.</param>
        /// <param name="glare">Add menu Glare scaleform?.</param>
        /// <param name="alternativeTitle">Set the alternative type to the title?.</param>
        /// <param name="fadingTime">Set fading time for the menu and the items, set it to 0.0 to disable it.</param>
        public UIMenu(string title, string subtitle, PointF offset, string spriteLibrary, string spriteName, bool glare = false, bool alternativeTitle = false, MenuAlignment menuAlignment = MenuAlignment.LEFT)
        {
            _customTexture = new KeyValuePair<string, string>(spriteLibrary, spriteName);
            Offset = offset;
            WidthOffset = 0;
            Glare = glare;
            _menuGlare = new ScaleformWideScreen("mp_menu_glare");
            Title = title;
            Subtitle = subtitle;
            AlternativeTitle = alternativeTitle;
            MouseWheelControlEnabled = true;
            MenuAlignment = menuAlignment;

            SetMenuOffset(offset);

            SetKey(MenuControls.Up, Control.FrontendUp);
            SetKey(MenuControls.Down, Control.FrontendDown);

            SetKey(MenuControls.Left, Control.FrontendLeft);
            SetKey(MenuControls.Right, Control.FrontendRight);
            SetKey(MenuControls.Select, Control.FrontendAccept);

            SetKey(MenuControls.Back, Control.FrontendCancel);
            SetKey(MenuControls.Back, Control.FrontendPause);
            SetKey(MenuControls.Back, Control.CursorCancel);

            SetKey(MenuControls.PageUp, Control.ScriptedFlyZUp);
            SetKey(MenuControls.PageDown, Control.ScriptedFlyZDown);

            InstructionalButtons = new List<InstructionalButton>()
            {
                new InstructionalButton(Control.FrontendCancel, _backTextLocalized),
                new InstructionalButton(Control.FrontendAccept, _selectTextLocalized),
            };
        }
        /// <summary>
        /// This is an itemless menu, it meas you cannot add items to this menu but you can add a description like on GTA:O
        /// </summary>
        /// <param name="title">Title that appears on the big banner. Set to "" if you are using a custom banner.</param>
        /// <param name="subtitle">Subtitle that appears in capital letters in a small black bar.</param>
        /// <param name="offset">PointF object with X and Y data for offsets. Applied to all menu elements.</param>
        /// <param name="spriteLibrary">Sprite library name for the banner.</param>
        /// <param name="spriteName">Sprite name for the banner.</param>
        /// <param name="glare">Add menu Glare scaleform?.</param>
        /// <param name="alternativeTitle">Set the alternative type to the title?.</param>
        /// <param name="fadingTime">Set fading time for the menu and the items, set it to 0.0 to disable it.</param>
        public UIMenu(string title, string subtitle, string description, PointF offset, string spriteLibrary, string spriteName, bool glare = false, bool alternativeTitle = false, MenuAlignment menuAlignment = MenuAlignment.LEFT)
        {
            _customTexture = new KeyValuePair<string, string>(spriteLibrary, spriteName);
            Offset = offset;
            WidthOffset = 0;
            Glare = glare;
            _menuGlare = new ScaleformWideScreen("mp_menu_glare");
            Title = title;
            Subtitle = subtitle;
            AlternativeTitle = alternativeTitle;
            MouseWheelControlEnabled = true;
            SetMenuOffset(offset);
            MenuAlignment = menuAlignment;

            SetKey(MenuControls.Up, Control.FrontendUp);
            SetKey(MenuControls.Down, Control.FrontendDown);

            SetKey(MenuControls.Left, Control.FrontendLeft);
            SetKey(MenuControls.Right, Control.FrontendRight);
            SetKey(MenuControls.Select, Control.FrontendAccept);

            SetKey(MenuControls.Back, Control.FrontendCancel);
            SetKey(MenuControls.Back, Control.FrontendPause);

            itemless = true;
            AddTextEntry("ScaleformUILongDesc", description);
            InstructionalButtons = new List<InstructionalButton>()
            {
                new InstructionalButton(Control.FrontendCancel, _backTextLocalized),
                new InstructionalButton(Control.FrontendAccept, _selectTextLocalized),
            };
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Extension method that is run on tick every frame while this menu is drawing.
        /// </summary>
        /// <param name="menu">The menu.</param>
        public void RefreshMenu(bool keepIndex = false)
        {
            foreach (UIMenuItem it in MenuItems) it.Selected = false;
            if (Visible)
            {
                BuildUpMenuAsync(true);
                int index = CurrentSelection;
                isBuilding = false;
                CurrentSelection = keepIndex ? index : 0;
                    // restore the previous settings
            }
        }

        public void AddInstructionalButton(InstructionalButton button)
        {
            InstructionalButtons.Add(button);
            if (Visible && !(Main.Warning.IsShowing || Main.Warning.IsShowingWithButtons))
                Main.InstructionalButtons.SetInstructionalButtons(InstructionalButtons);
        }

        public void RemoveInstructionalButton(InstructionalButton button)
        {
            if (InstructionalButtons.Contains(button))
                InstructionalButtons.Remove(button);
            if (Visible && !(Main.Warning.IsShowing || Main.Warning.IsShowingWithButtons))
                Main.InstructionalButtons.SetInstructionalButtons(InstructionalButtons);
        }

        public void RemoveInstructionalButton(int index)
        {
            if (index < 0 || index >= InstructionalButtons.Count)
                throw new ArgumentOutOfRangeException("ScaleformUI: Cannot remove with an index less than 0 or more than the count of actual instructional buttons");
            RemoveInstructionalButton(InstructionalButtons[index]);
        }

        public void ClearInstructionalButtons()
        {
            this.InstructionalButtons.Clear();
        }

        /// <summary>
        /// Change the menu's width. The width is calculated as DefaultWidth + WidthOffset, so a width offset of 10 would enlarge the menu by 10 pixels.
        /// </summary>
        /// <param name="widthOffset">New width offset.</param>
        public void SetMenuWidthOffset(int widthOffset)
        {
            WidthOffset = widthOffset;
        }

        /// <summary>
        /// Set the banner to your own custom texture. Set it to "" if you want to restore the banner.
        /// </summary>
        /// <param name="pathToCustomSprite">Path to your sprite image.</param>
        public void SetBannerType(KeyValuePair<string, string> pathToCustomSprite)
        {
            _customTexture = pathToCustomSprite;
            if (Visible)
            {
                //Main.scaleformUI.CallFunction("UPDATE_MENU_BANNER_TEXTURE", _customTexture.Key, _customTexture.Value);
                SetMenuData(true);
            }
        }

        internal void SetUnderBannerType(KeyValuePair<string, string> pathToCustomSprite)
        {
            _customBGTexture = pathToCustomSprite;
            if (Visible)
            {
                //Main.scaleformUI.CallFunction("UPDATE_MENU_UNDERBANNER_TEXTURE", _customBGTexture.Key, _customBGTexture.Value);
                SetMenuData(true);
            }
        }

        public void SetBannerColor(SColor color)
        {
            bannerColor = color;
            if (Visible)
            {
                //Main.scaleformUI.CallFunction("SET_MENU_BANNER_COLOR", bannerColor.ArgbValue);
                SetMenuData(true);
            }
        }


        public void SetUnderBannerColor(SColor color)
        {
            bannerBGColor = color;
            if (Visible)
            {
                //Main.scaleformUI.CallFunction("SET_MENU_UNDERBANNER_COLOR", bannerBGColor.ArgbValue);
                SetMenuData(true);
            }
        }

        /// <summary>
        /// Add an item to the menu.
        /// </summary>
        /// <param name="item">Item object to be added. Can be normal item, checkbox or list item.</param>
        public void AddItem(UIMenuItem item)
        {
            if (!itemless)
            {
                item.Parent = this;
                MenuItems.Add(item);
                if (Visible)
                {
                    var idx = MenuItems.Count - 1;
                    SendItemToScaleform(idx);
                }
            }
            else throw new Exception("ScaleformUI - You cannot add items to an itemless menu, only a long description");
        }

        /// <summary>
        /// Adds an item to a selected index
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <exception cref="Exception"></exception>
        public void AddItemAt(UIMenuItem item, int index)
        {
            if (!itemless)
            {
                item.Parent = this;
                MenuItems.Insert(index, item);
                if (Visible)
                {
                    SendItemToScaleform(index, false, true);
                }
            }
            else throw new Exception("ScaleformUI - You cannot add items to an itemless menu, only a long description");
        }
        /// <summary>
        /// Add a new Heritage Window to the Menu
        /// </summary>
        /// <param name="window"></param>
        public void AddWindow(UIMenuWindow window)
        {
            if (!itemless)
            {
                window.ParentMenu = this;
                Windows.Add(window);
            }
            else throw new Exception("ScaleformUI - You cannot add windows to an itemless menu, only a long description");
        }

        /// <summary>
        /// Removes Windows at given index
        /// </summary>
        /// <param name="index"></param>
        public void RemoveWindowAt(int index)
        {
            Windows.RemoveAt(index);
        }

        /// <summary>
        /// If an item's description is changed during some events after the menu as been opened this updates the description live
        /// </summary>
        public void UpdateDescription()
        {
            //Main.scaleformUI.CallFunction("UPDATE_ITEM_DESCRIPTION");
            SetMenuData(true);
        }

        /// <summary>
        /// Remove an item at index n.
        /// </summary>
        /// <param name="index">Index to remove the item at.</param>
        public void RemoveItemAt(int index)
        {
            int selectedItem = CurrentSelection;
            if (MenuItems.Count > index)
            {
                MenuItems.RemoveAt(index);
                RefreshMenu();
                if (selectedItem < MenuItems.Count)
                    CurrentSelection = selectedItem;
            }
            else
            {
                throw new Exception("ScaleformUI - Cannot remove an index out of bounds!!");
            }
        }

        public void RemoveItem(UIMenuItem item)
        {
            RemoveItemAt(MenuItems.IndexOf(item));
        }

        /// <summary>
        /// Remove all items from the menu.
        /// </summary>
        public void Clear()
        {
            if (Visible)
                Main.scaleformUI.CallFunction("SET_DATA_SLOT_EMPTY");
            MenuItems.Clear();
            //Pagination.TotalItems = 0;
        }

        /// <summary>
        /// Removes the items that matches the predicate.
        /// </summary>
        /// <param name="predicate">The function to use as the check.</param>
        public void Remove(Func<UIMenuItem, bool> predicate)
        {
            List<UIMenuItem> TempList = new List<UIMenuItem>(MenuItems);
            foreach (UIMenuItem item in TempList)
            {
                if (predicate(item))
                {
                    MenuItems.Remove(item);
                }
            }
        }

        /// <summary>
        /// Set a GTA.Control to control a menu. Can be multiple controls. This applies it to all indexes.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="gtaControl"></param>
        public void SetKey(MenuControls control, Control gtaControl)
        {
            SetKey(control, gtaControl, 0);
            SetKey(control, gtaControl, 1);
            SetKey(control, gtaControl, 2);
        }


        /// <summary>
        /// Set a GTA.Control to control a menu only on a specific index.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="gtaControl"></param>
        /// <param name="controlIndex"></param>
        public void SetKey(MenuControls control, Control gtaControl, int controlIndex)
        {
            if (_keyDictionary.ContainsKey(control))
                _keyDictionary[control].Add(new Tuple<Control, int>(gtaControl, controlIndex));
            else
            {
                _keyDictionary.Add(control, new List<Tuple<Control, int>>(new List<Tuple<Control, int>>()));
                _keyDictionary[control].Add(new Tuple<Control, int>(gtaControl, controlIndex));
            }

        }


        /// <summary>
        /// Remove all controls on a control.
        /// </summary>
        /// <param name="control"></param>
        public void ResetKey(MenuControls control)
        {
            _keyDictionary[control].Clear();
        }

        /// <summary>
        /// Check whether a menucontrol has been pressed.
        /// </summary>
        /// <param name="control">Control to check for.</param>
        /// <param name="key">Key if you're using keys.</param>
        /// <returns></returns>
        public bool HasControlJustBeenPressed(MenuControls control)
        {
            List<Tuple<Control, int>> tmpControls = new List<Tuple<Control, int>>(_keyDictionary[control]);
            return tmpControls.Any(tuple => IsDisabledControlJustPressed(2, (int)tuple.Item1));
        }


        /// <summary>
        /// Check whether a menucontrol has been released.
        /// </summary>
        /// <param name="control">Control to check for.</param>
        /// <param name="key">Key if you're using keys.</param>
        /// <returns></returns>
        public bool HasControlJustBeenReleased(MenuControls control)
        {
            List<Tuple<Control, int>> tmpControls = new List<Tuple<Control, int>>(_keyDictionary[control]);
            return tmpControls.Any(tuple => IsDisabledControlJustReleased(2, (int)tuple.Item1));
        }

        private int _controlCounter;
        private bool enableAnimation = true;
        private MenuAnimationType animationType = MenuAnimationType.LINEAR;

        /// <summary>
        /// Check whether a menucontrol is being pressed.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsControlBeingPressed(MenuControls control)
        {
            List<Tuple<Control, int>> tmpControls = new List<Tuple<Control, int>>(_keyDictionary[control]);
            if (HasControlJustBeenReleased(control)) _controlCounter = 0;
            return tmpControls.Any(tuple => IsDisabledControlPressed(tuple.Item2, (int)tuple.Item1));
        }

        #endregion

        #region Drawing & Processing

        private float Wrap(float value, float min, float max)
        {
            float range = max - min;
            float normalizedValue = (value - min) % range;

            if (normalizedValue < 0)
            {
                normalizedValue += range;
            }

            if (Math.Abs(normalizedValue - range) < float.Epsilon)
            {
                normalizedValue = range;
            }

            return min + normalizedValue;
        }


        /// <summary>
        /// Draw the menu and all of it's components.
        /// </summary>
        internal override async void Draw()
        {
            if (!Visible || Main.Warning.IsShowing) return;
            while (!Main.scaleformUI.IsLoaded) await BaseScript.Delay(0);

            HideHudComponentThisFrame(19);

            Controls.Toggle(!ControlDisablingEnabled);

            SetMenuOffset(Offset);
            Main.scaleformUI.Render2D();

            if (Glare)
            {
                var fRotationTolerance = 0.5f;
                var dir = GetFinalRenderedCamRot(2);
                var fvar = Wrap(dir.Z, 0, 360);
                if (fSavedGlareDirection == 0 || (fSavedGlareDirection - fvar) > fRotationTolerance || (fSavedGlareDirection - fvar) < -fRotationTolerance)
                {
                    fSavedGlareDirection = fvar;
                    _menuGlare.CallFunction("SET_DATA_SLOT", fSavedGlareDirection);
                }
                DrawScaleformMovie(_menuGlare.Handle, glarePosition.X, glarePosition.Y, glareSize.Width, glareSize.Height, 255, 255, 255, 255, 0);
                //_menuGlare.Render2D();
            }

            if (IsUsingController)
            {
                if (keyboard)
                {
                    keyboard = false;
                    _changed = true;
                }
            }
            else
            {
                if (!keyboard)
                {
                    keyboard = true;
                    _changed = true;
                }
            }
            if (_changed)
            {
                UpdateDescription();
                _changed = false;
            }
            mouseCheck();
        }

        internal void CallExtensionMethod()
        {
            ExtensionMethod?.Invoke(this);
        }

        private async void mouseCheck()
        {
            _mouseOnMenu = MouseControlsEnabled && await Main.scaleformUI.CallFunctionReturnValueBool("IS_MOUSE_ON_MENU");
        }

        internal int eventType = 0;
        internal int itemId = 0;
        internal int context = 0;
        internal int unused = 0;
        internal bool success;
        bool wasPressedInPanel;
        bool wasPressedInItem;
        bool cursorPressed;
        bool cursorPressedItem;
        private ItemFont descriptionFont = ScaleformFonts.CHALET_LONDON_NINETEENSIXTY;
        private bool mouseReset = false;
        private MenuAlignment menuAlignment;

        /// <summary>
        /// Process the mouse's position and check if it's hovering over any UI element. Call this in OnTick
        /// </summary>
        internal override async void ProcessMouse()
        {
            if (!Visible || _justOpened || MenuItems.Count == 0 || IsUsingController || !MouseControlsEnabled)
            {
                Game.EnableControlThisFrame(0, Control.LookUpDown);
                Game.EnableControlThisFrame(0, Control.LookLeftRight);
                Game.EnableControlThisFrame(1, Control.LookUpDown);
                Game.EnableControlThisFrame(1, Control.LookLeftRight);
                Game.EnableControlThisFrame(2, Control.LookUpDown);
                Game.EnableControlThisFrame(2, Control.LookLeftRight);
                if (_itemsDirty)
                {
                    MenuItems.Where(i => i.Hovered).ToList().ForEach(i => i.Hovered = false);
                    _itemsDirty = false;
                }
                return;
            }

            SetMouseCursorActiveThisFrame();
            SetInputExclusive(2, 239);
            SetInputExclusive(2, 240);
            SetInputExclusive(2, 237);
            SetInputExclusive(2, 238);

            success = GetScaleformMovieCursorSelection(Main.scaleformUI.Handle, ref eventType, ref context, ref itemId, ref unused);
            if (success && !isBuilding)
            {
                switch (eventType)
                {
                    case 5: // on click
                        switch (context)
                        {
                            case -1:
                                switch (itemId)
                                {
                                    case 2:
                                        GoUp();
                                        break;
                                    case 3:
                                        GoDown();
                                        break;
                                }
                                break;
                            case 0:
                                {
                                    UIMenuItem item = MenuItems[itemId];
                                    if ((MenuItems[itemId] is UIMenuSeparatorItem && (MenuItems[itemId] as UIMenuSeparatorItem).Jumpable) || !MenuItems[itemId].Enabled)
                                    {
                                        Game.PlaySound(AUDIO_ERROR, AUDIO_LIBRARY);
                                        return;
                                    }
                                    if (item.Selected)
                                    {
                                        switch (item._itemId)
                                        {
                                            case 0:
                                            case 2:
                                                Select(false);
                                                break;
                                            case 1:
                                            case 3:
                                            case 4:
                                                {
                                                    int value = await Main.scaleformUI.CallFunctionReturnValueInt("SELECT_ITEM", CurrentSelection);
                                                    switch (MenuItems[CurrentSelection])
                                                    {
                                                        case UIMenuListItem:
                                                            {
                                                                UIMenuListItem it = MenuItems[CurrentSelection] as UIMenuListItem;
                                                                if (it.Index != value)
                                                                {
                                                                    it.Index = value;
                                                                    ListChange(it, it.Index);
                                                                    it.ListChangedTrigger(it.Index);
                                                                }
                                                                else
                                                                {
                                                                    it.ListSelectedTrigger(value);
                                                                    it.ItemActivate(this);
                                                                    ListSelect(it, value);
                                                                }
                                                            }
                                                            break;
                                                        case UIMenuSliderItem:
                                                            {
                                                                UIMenuSliderItem it = (UIMenuSliderItem)MenuItems[CurrentSelection];
                                                                if (it.Value != value)
                                                                {
                                                                    it.Value = value;
                                                                    it.SliderChanged(it.Value);
                                                                    SliderChange(it, it.Value);
                                                                }
                                                                else
                                                                {
                                                                    it.ItemActivate(this);
                                                                    this.ItemSelect(it, CurrentSelection);
                                                                }
                                                            }
                                                            break;
                                                        case UIMenuProgressItem:
                                                            {
                                                                UIMenuProgressItem it = (UIMenuProgressItem)MenuItems[CurrentSelection];
                                                                if (it.Value != value)
                                                                {
                                                                    it.Value = value;
                                                                    it.ProgressChanged(it.Value);
                                                                    ProgressChange(it, it.Value);
                                                                }
                                                                else
                                                                {
                                                                    it.ItemActivate(this);
                                                                    this.ItemSelect(it, CurrentSelection);
                                                                }
                                                            }
                                                            break;
                                                    }
                                                }
                                                break;
                                        }
                                        return;
                                    }
                                    CurrentSelection = itemId;
                                    Game.PlaySound(AUDIO_SELECT, AUDIO_LIBRARY);
                                }
                                break;
                            case 1:
                                {
                                    UIMenuItem item = MenuItems[itemId];
                                    if ((MenuItems[itemId] is UIMenuSeparatorItem && (MenuItems[itemId] as UIMenuSeparatorItem).Jumpable) || !MenuItems[itemId].Enabled)
                                    {
                                        Game.PlaySound(AUDIO_ERROR, AUDIO_LIBRARY);
                                        return;
                                    }
                                    if (item.Selected)
                                    {
                                        if (item._itemId == 1 || item._itemId == 3 || item._itemId == 4)
                                        {
                                            cursorPressedItem = true;
                                            wasPressedInItem = true;
                                        }
                                        return;
                                    }
                                    CurrentSelection = itemId;
                                }
                                break;
                            case 2:
                                {
                                    UIMenuItem item = MenuItems[itemId];
                                    if ((MenuItems[itemId] is UIMenuSeparatorItem && (MenuItems[itemId] as UIMenuSeparatorItem).Jumpable) || !MenuItems[itemId].Enabled)
                                    {
                                        Game.PlaySound(AUDIO_ERROR, AUDIO_LIBRARY);
                                        return;
                                    }
                                    if (item.Selected)
                                    {
                                        if (item._itemId == 1 || item._itemId == 3 || item._itemId == 4)
                                            GoLeft();
                                        return;
                                    }
                                    CurrentSelection = itemId;
                                }
                                break;
                            case 3:
                                {
                                    UIMenuItem item = MenuItems[itemId];
                                    if ((MenuItems[itemId] is UIMenuSeparatorItem && (MenuItems[itemId] as UIMenuSeparatorItem).Jumpable) || !MenuItems[itemId].Enabled)
                                    {
                                        Game.PlaySound(AUDIO_ERROR, AUDIO_LIBRARY);
                                        return;
                                    }
                                    if (item.Selected)
                                    {
                                        if (item._itemId == 1 || item._itemId == 3 || item._itemId == 4)
                                            GoRight();
                                        return;
                                    }
                                    CurrentSelection = itemId;
                                }
                                break;
                            case 10: // panels (10 => context 1, panel_type 0) // ColorPanel
                                {
                                    string res = await Main.scaleformUI.CallFunctionReturnValueString("SELECT_PANEL", CurrentSelection);
                                    string[] split = res.Split(',');
                                    UIMenuColorPanel panel = (UIMenuColorPanel)MenuItems[CurrentSelection].Panels[Convert.ToInt32(split[0])];
                                    panel._value = Convert.ToInt32(split[1]);
                                    ColorPanelChange(panel.ParentItem, panel, panel.CurrentSelection);
                                    panel.PanelChanged();
                                }
                                break;
                            case 14: // panels (14 => context 1, panel_type 4) // ColorPanel
                                {
                                    int res = await Main.scaleformUI.CallFunctionReturnValueInt("SELECT_PANEL", CurrentSelection);
                                    UIMenuColourPickePanel picker = (UIMenuColourPickePanel)MenuItems[CurrentSelection].Panels[res];
                                    if (picker != null)
                                    {
                                        if (itemId != -1)
                                        {
                                            string colString = await Main.scaleformUI.CallFunctionReturnValueString("GET_PICKER_COLOR", itemId);
                                            string[] split = colString.Split(',');
                                            picker._value = itemId;
                                            picker.PickerSelect(SColor.FromArgb(Convert.ToInt32(split[0]), Convert.ToInt32(split[1]), Convert.ToInt32(split[2])));
                                            Game.PlaySound(AUDIO_SELECT, AUDIO_LIBRARY);
                                        }
                                    }
                                }
                                break;
                            case 11: // panels (11 => context 1, panel_type 1) // PercentagePanel
                            case 12: // panels (12 => context 1, panel_type 2) // GridPanel
                                cursorPressed = true;
                                wasPressedInPanel = true;
                                break;
                            case 20: // side panel
                                {
                                    UIVehicleColourPickerPanel panel = (UIVehicleColourPickerPanel)MenuItems[CurrentSelection].SidePanel;
                                    if (itemId != -1)
                                    {
                                        string colString = await Main.scaleformUI.CallFunctionReturnValueString("GET_PICKER_COLOR", itemId);
                                        string[] split = colString.Split(',');
                                        panel._value = itemId;
                                        panel.PickerSelect(SColor.FromArgb(Convert.ToInt32(split[0]), Convert.ToInt32(split[1]), Convert.ToInt32(split[2])));
                                        Game.PlaySound(AUDIO_SELECT, AUDIO_LIBRARY);
                                    }
                                }
                                break;
                        }
                        break;
                    case 6: // on click released
                        cursorPressed = false;
                        wasPressedInPanel = false;
                        cursorPressedItem = false;
                        wasPressedInItem = false;
                        break;
                    case 7: // on click released ouside
                        cursorPressed = false;
                        wasPressedInPanel = false;
                        cursorPressedItem = false;
                        wasPressedInItem = false;
                        SetMouseCursorSprite(1);
                        if (mouseReset)
                            mouseReset = false;
                        break;
                    case 8: // on not hover
                        cursorPressed = false;
                        cursorPressedItem = false;
                        switch (context)
                        {
                            case 0:
                                MenuItems[itemId].Hovered = false;
                                break;
                            case 20:
                                UIVehicleColourPickerPanel panel = (UIVehicleColourPickerPanel)MenuItems[CurrentSelection].SidePanel;
                                panel.PickerRollout();
                                break;
                            case 14:
                                int res = await Main.scaleformUI.CallFunctionReturnValueInt("SELECT_PANEL", CurrentSelection);
                                UIMenuColourPickePanel picker = (UIMenuColourPickePanel)MenuItems[CurrentSelection].Panels[res];
                                picker.PickerRollout();
                                break;
                        }
                        if (!IsMouseOverTheMenu) return;
                        SetMouseCursorSprite(1);
                        if (mouseReset)
                            mouseReset = false;
                        break;
                    case 9: // on hovered
                        switch (context)
                        {
                            case 0:
                                MenuItems[itemId].Hovered = true;
                                break;
                            case 20:
                                UIVehicleColourPickerPanel panel = (UIVehicleColourPickerPanel)MenuItems[CurrentSelection].SidePanel;
                                if (itemId != -1)
                                {
                                    panel.PickerHovered(itemId, VehicleColors.GetColorById(itemId));
                                }
                                break;
                            case 14:
                                int res = await Main.scaleformUI.CallFunctionReturnValueInt("SELECT_PANEL", CurrentSelection);
                                UIMenuColourPickePanel picker = (UIMenuColourPickePanel)MenuItems[CurrentSelection].Panels[res];
                                if (picker != null)
                                {
                                    if (itemId != -1)
                                    {
                                        picker.PickerHovered(itemId, VehicleColors.GetColorById(itemId));
                                    }
                                }
                                break;

                        }
                        SetMouseCursorSprite(5);
                        if (mouseReset)
                            mouseReset = false;
                        break;
                    case 0: // dragged outside
                        cursorPressed = false;
                        cursorPressedItem = false;
                        break;
                    case 1: // dragged inside
                        if(wasPressedInPanel)
                            cursorPressed = true;
                        if(wasPressedInItem)
                        cursorPressedItem = true;
                        break;
                }
            }

            if (cursorPressedItem)
            {
                if (HasSoundFinished(menuSound))
                {
                    menuSound = GetSoundId();
                    PlaySoundFrontend(menuSound, "CONTINUOUS_SLIDER", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                }
                int value = await Main.scaleformUI.CallFunctionReturnValueInt("SELECT_ITEM", CurrentSelection);
                switch (CurrentItem)
                {
                    case UIMenuProgressItem pr:
                        pr._value = value;
                        pr.ProgressChanged(value);
                        ProgressChange(pr, pr.Value);
                        break;
                    case UIMenuSliderItem sl:
                        sl._value = value;
                        sl.SliderChanged(value);
                        SliderChange(sl, sl.Value);
                        break;
                }
            }
            else
            {
                if (!HasSoundFinished(menuSound))
                {
                    await BaseScript.Delay(1);
                    StopSound(menuSound);
                    ReleaseSoundId(menuSound);
                }
            }

            if (cursorPressed)
            {
                if (HasSoundFinished(menuSound))
                {
                    menuSound = GetSoundId();
                    PlaySoundFrontend(menuSound, "CONTINUOUS_SLIDER", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                }

                string res = await Main.scaleformUI.CallFunctionReturnValueString("SET_INPUT_MOUSE_EVENT_CONTINUE");
                string[] split = res.Split(',');
                int selection = Convert.ToInt32(split[0]);
                UIMenuPanel panel = MenuItems[CurrentSelection].Panels[selection];
                switch (panel)
                {
                    case UIMenuGridPanel:
                        UIMenuGridPanel grid = panel as UIMenuGridPanel;
                        grid._value = new(Convert.ToSingle(split[1]), Convert.ToSingle(split[2]));
                        GridPanelChange(panel.ParentItem, grid, grid.CirclePosition);
                        grid.OnGridChange();
                        break;
                    case UIMenuPercentagePanel:
                        UIMenuPercentagePanel perc = panel as UIMenuPercentagePanel;
                        perc._value = Convert.ToSingle(split[1]);
                        PercentagePanelChange(panel.ParentItem, perc, perc.Percentage);
                        perc.PercentagePanelChange();
                        break;
                }
            }
            else
            {
                if (!HasSoundFinished(menuSound))
                {
                    await BaseScript.Delay(1);
                    StopSound(menuSound);
                    ReleaseSoundId(menuSound);
                }
            }

            if (MouseEdgeEnabled)
            {
                float mouseVariance = GetDisabledControlNormal(2, 239);
                if (ScreenTools.IsMouseInBounds(new PointF(0, 0), new SizeF(30, 1080)))
                {
                    if (mouseVariance < (0.05f * 0.75f))
                    {
                        SetMouseCursorSprite(6);
                        float mouseSpeed = 0.05f - mouseVariance;
                        if (mouseSpeed > 0.05f) mouseSpeed = 0.05f;
                        GameplayCamera.RelativeHeading += 70 * mouseSpeed;
                        if (mouseReset)
                            mouseReset = false;
                    }
                }
                else if (ScreenTools.IsMouseInBounds(new PointF(Convert.ToInt32(Resolution.Width - 30f), 0), new SizeF(30, 1080)))
                {
                    if (mouseVariance > (1f - (0.05f * 0.75f)))
                    {
                        float mouseSpeed = 0.05f - (1f - mouseVariance);
                        if (mouseSpeed > 0.05f) mouseSpeed = 0.05f;
                        GameplayCamera.RelativeHeading -= 70 * mouseSpeed;
                        SetMouseCursorSprite(7);
                        if (mouseReset)
                            mouseReset = false;
                    }
                }
                else
                {
                    if (!IsMouseOverTheMenu)
                    {
                        if (!mouseReset)
                            SetMouseCursorSprite(1);
                        mouseReset = true;
                    }
                }
            }
            else
            {
                if (!IsMouseOverTheMenu)
                {
                    if (!mouseReset)
                        SetMouseCursorSprite(1);
                    mouseReset = true;
                }
            }
        }

        public async void GoBack(bool playSound = true)
        {
            if (CanPlayerCloseMenu)
            {
                if (playSound)
                    Game.PlaySound(AUDIO_BACK, AUDIO_LIBRARY);
                if (BreadcrumbsHandler.CurrentDepth == 0)
                {
                    MenuHandler.CloseAndClearHistory();
                    BreadcrumbsHandler.Clear();
                    Main.InstructionalButtons.ClearButtonList();
                }
                else
                {
                    BreadcrumbsHandler.SwitchInProgress = true;
                    MenuBase prevMenu = null;
                    if (BreadcrumbsHandler.CurrentDepth > 0)
                    {
                        prevMenu = BreadcrumbsHandler.PreviousMenu;
                        if (prevMenu is UIMenu uimenu)
                        {
                            if (uimenu.MenuItems.Count == 0)
                            {
                                MenuHandler.CloseAndClearHistory();
                                throw new Exception($"UIMenu {this.Title} previous menu is empty... Closing and clearing history.");
                            }
                        }
                        BreadcrumbsHandler.Backwards();
                    }
                    Visible = false;
                    if (prevMenu != null)
                    {
                        if (prevMenu is UIMenu menu)
                        {
                            menu.differentBanner = _customTexture.Key != menu._customTexture.Key && _customTexture.Value != menu._customTexture.Value;
                            menu.Visible = true;
                        }
                        else
                            prevMenu.Visible = true;
                    }
                    BreadcrumbsHandler.SwitchInProgress = false;
                }
            }
        }

        public async void GoUp()
        {
            do
            {
                await BaseScript.Delay(0);
                CurrentItem.Selected = false;
                _currentSelection--;
                if (_currentSelection < topEdge)
                    topEdge--;
                if (_currentSelection < 0)
                {
                    _currentSelection = MenuItems.Count - 1;
                    topEdge = MenuItems.Count - _visibleItems;
                }
                AddTextEntry("UIMenu_Current_Description", CurrentItem.Description);
            }
            while (CurrentItem._itemId == 6 && ((UIMenuSeparatorItem)CurrentItem).Jumpable);
            Main.scaleformUI.CallFunction("SET_INPUT_EVENT", 8);
            CurrentItem.Selected = true;
            SendPanelsToItemScaleform(_currentSelection);
            SendSidePanelToScaleform(_currentSelection);
            Game.PlaySound(AUDIO_UPDOWN, AUDIO_LIBRARY);
            IndexChange(_currentSelection);
        }
        public async void GoDown()
        {
            do
            {
                await BaseScript.Delay(0);
                CurrentItem.Selected = false;
                _currentSelection++;
                if (_currentSelection >= topEdge + _visibleItems)
                    topEdge++;
                if (_currentSelection >= MenuItems.Count)
                {
                    _currentSelection = 0;
                    topEdge = 0;
                }
                AddTextEntry("UIMenu_Current_Description", CurrentItem.Description);
            }
            while (CurrentItem._itemId == 6 && ((UIMenuSeparatorItem)CurrentItem).Jumpable);
            Main.scaleformUI.CallFunction("SET_INPUT_EVENT", 9);
            CurrentItem.Selected = true;
            SendPanelsToItemScaleform(_currentSelection);
            SendSidePanelToScaleform(_currentSelection);
            Game.PlaySound(AUDIO_UPDOWN, AUDIO_LIBRARY);
            IndexChange(_currentSelection);
        }

        public async void GoLeft()
        {
            if (!MenuItems[CurrentSelection].Enabled)
            {
                Game.PlaySound(AUDIO_ERROR, AUDIO_LIBRARY);
                return;
            }
            switch (MenuItems[CurrentSelection])
            {
                case UIMenuListItem:
                    {
                        UIMenuListItem it = (UIMenuListItem)MenuItems[CurrentSelection];
                        it.Index--;
                        ListChange(it, it.Index);
                        it.ListChangedTrigger(it.Index);
                        break;
                    }
                case UIMenuDynamicListItem:
                    {
                        UIMenuDynamicListItem it = (UIMenuDynamicListItem)MenuItems[CurrentSelection];
                        string newItem = await it.Callback(it, ChangeDirection.Left);
                        it.CurrentListItem = newItem;
                        break;
                    }
                case UIMenuSliderItem:
                    {
                        UIMenuSliderItem it = (UIMenuSliderItem)MenuItems[CurrentSelection];
                        it.Value--;
                        SliderChange(it, it.Value);
                        break;
                    }
                case UIMenuProgressItem:
                    {
                        UIMenuProgressItem it = (UIMenuProgressItem)MenuItems[CurrentSelection];
                        it.Value--;
                        ProgressChange(it, it.Value);
                        break;
                    }
                case UIMenuStatsItem:
                    {
                        UIMenuStatsItem it = (UIMenuStatsItem)MenuItems[CurrentSelection];
                        it.Value--;
                        StatItemChange(it, it.Value);
                        break;
                    }
            }
            Game.PlaySound(AUDIO_LEFTRIGHT, AUDIO_LIBRARY);
        }

        public async void GoRight()
        {
            if (!MenuItems[CurrentSelection].Enabled)
            {
                Game.PlaySound(AUDIO_ERROR, AUDIO_LIBRARY);
                return;
            }
            switch (MenuItems[CurrentSelection])
            {
                case UIMenuListItem:
                    {
                        UIMenuListItem it = (UIMenuListItem)MenuItems[CurrentSelection];
                        it.Index++;
                        ListChange(it, it.Index);
                        it.ListChangedTrigger(it.Index);
                        break;
                    }
                case UIMenuDynamicListItem:
                    {
                        UIMenuDynamicListItem it = (UIMenuDynamicListItem)MenuItems[CurrentSelection];
                        string newItem = await it.Callback(it, ChangeDirection.Right);
                        it.CurrentListItem = newItem;
                        break;
                    }
                case UIMenuSliderItem:
                    {
                        UIMenuSliderItem it = (UIMenuSliderItem)MenuItems[CurrentSelection];
                        it.Value++;
                        SliderChange(it, it.Value);
                        break;
                    }
                case UIMenuProgressItem:
                    {
                        UIMenuProgressItem it = (UIMenuProgressItem)MenuItems[CurrentSelection];
                        it.Value++;
                        ProgressChange(it, it.Value);
                        break;
                    }
                case UIMenuStatsItem:
                    {
                        UIMenuStatsItem it = (UIMenuStatsItem)MenuItems[CurrentSelection];
                        it.Value++;
                        StatItemChange(it, it.Value);
                        break;
                    }
            }
            Game.PlaySound(AUDIO_LEFTRIGHT, AUDIO_LIBRARY);
        }

        public void Select(bool playSound)
        {
            if (!MenuItems[CurrentSelection].Enabled)
            {
                Game.PlaySound(AUDIO_ERROR, AUDIO_LIBRARY);
                return;
            }

            if (playSound) Game.PlaySound(AUDIO_SELECT, AUDIO_LIBRARY);
            switch (MenuItems[CurrentSelection])
            {
                case UIMenuCheckboxItem:
                    {
                        UIMenuCheckboxItem it = (UIMenuCheckboxItem)MenuItems[CurrentSelection];
                        it.Checked = !it.Checked;
                        CheckboxChange(it, it.Checked);
                        it.CheckboxEventTrigger();
                        break;
                    }

                case UIMenuListItem:
                    {
                        UIMenuListItem it = (UIMenuListItem)MenuItems[CurrentSelection];
                        ListSelect(it, it.Index);
                        it.ListSelectedTrigger(it.Index);
                        break;
                    }

                default:
                    ItemSelect(MenuItems[CurrentSelection], CurrentSelection);
                    MenuItems[CurrentSelection].ItemActivate(this);
                    break;
            }
        }
        /// <summary>
        /// Process control-stroke. Call this in the OnTick event.
        /// </summary>
        internal override void ProcessControl()
        {
            if (!Main.scaleformUI.IsLoaded) return;
            if (!Visible || Main.Warning.IsShowing) return;
            if (_justOpened)
            {
                _justOpened = false;
                return;
            }

            if (UpdateOnscreenKeyboard() == 0 || IsWarningMessageActive() || BreadcrumbsHandler.SwitchInProgress || isFading) return;

            if (HasControlJustBeenReleased(MenuControls.Back))
            {
                GoBack();
            }

            if (isBuilding || MenuItems.Count == 0)
            {
                return;
            }

            if (HasControlJustBeenPressed(MenuControls.Up))
            {
                GoUp();
                timeBeforeOverflow = Main.GameTime;
            }
            else if (IsControlBeingPressed(MenuControls.Up) && Main.GameTime - timeBeforeOverflow > delayBeforeOverflow)
            {
                if (Main.GameTime - time > delay)
                {
                    ButtonDelay();
                    GoUp();
                }
            }

            if (HasControlJustBeenPressed(MenuControls.Down))
            {
                GoDown();
                timeBeforeOverflow = Main.GameTime;
            }
            else if (IsControlBeingPressed(MenuControls.Down) && Main.GameTime - timeBeforeOverflow > delayBeforeOverflow)
            {
                if (Main.GameTime - time > delay)
                {
                    ButtonDelay();
                    GoDown();
                }
            }

            if (HasControlJustBeenPressed(MenuControls.Left))
            {
                GoLeft();
                timeBeforeOverflow = Main.GameTime;
            }
            else if (IsControlBeingPressed(MenuControls.Left) && Main.GameTime - timeBeforeOverflow > delayBeforeOverflow)
            {
                if (Main.GameTime - time > delay)
                {
                    ButtonDelay();
                    GoLeft();
                }
            }

            if (HasControlJustBeenPressed(MenuControls.Right))
            {
                GoRight();
                timeBeforeOverflow = Main.GameTime;
            }
            else if (IsControlBeingPressed(MenuControls.Right) && Main.GameTime - timeBeforeOverflow > delayBeforeOverflow)
            {
                if (Main.GameTime - time > delay)
                {
                    ButtonDelay();
                    GoRight();
                }
            }

            if (HasControlJustBeenPressed(MenuControls.Select))
            {
                Select(true);
            }

            //if (HasControlJustBeenPressed(MenuControls.PageUp))
            //{
            //    var index = CurrentSelection - Pagination.ItemsPerPage;
            //    if (index < 0)
            //    {
            //        var pagIndex = Pagination.GetPageIndexFromMenuIndex(CurrentSelection);
            //        var newPage = Pagination.TotalPages - 1;
            //        index = Pagination.GetMenuIndexFromPageIndex(newPage, pagIndex);
            //        var menuMaxItem = MenuItems.Count - 1;
            //        if (index > menuMaxItem)
            //            index = menuMaxItem;
            //    }
            //    CurrentSelection = index;
            //    IndexChange(CurrentSelection);
            //}
            //else if (HasControlJustBeenPressed(MenuControls.PageDown))
            //{
            //    var index = CurrentSelection + Pagination.ItemsPerPage;
            //    if (index >= MenuItems.Count && Pagination.CurrentPage < Pagination.TotalPages - 1)
            //    {
            //        index = MenuItems.Count - 1;
            //    }
            //    else if (index >= MenuItems.Count && Pagination.CurrentPage == Pagination.TotalPages - 1)
            //    {
            //        var pagIndex = Pagination.GetPageIndexFromMenuIndex(CurrentSelection);
            //        var newPage = 0;
            //        index = Pagination.GetMenuIndexFromPageIndex(newPage, pagIndex);
            //    }
            //    CurrentSelection = index;
            //    IndexChange(CurrentSelection);
            //}


            // IsControlBeingPressed doesn't run every frame so I had to use this
            if (HasControlJustBeenReleased(MenuControls.Up) || HasControlJustBeenReleased(MenuControls.Down) || HasControlJustBeenReleased(MenuControls.Left) || HasControlJustBeenReleased(MenuControls.Right))
            {
                times = 0;
                delay = 100;
            }
        }

        private void ButtonDelay()
        {
            // Increment the "changed indexes" counter
            times++;

            // Each time "times" is a multiple of 5 we decrease the delay.
            // Min delay for the scaleform is 50.. less won't change due to the
            // awaiting time for the scaleform itself.
            if (times % 5 == 0)
            {
                delay -= 10;
                if (delay < 50) delay = 50;
            }
            // Reset the time to the current game timer.
            time = Main.GameTime;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Change whether this menu is visible to the user.
        /// </summary>
        public override bool Visible
        {
            get { return _visible; }
            set
            {
                _justOpened = value;
                _itemsDirty = value;
                if (value)
                {
                    if (_visible) return;
                    _visible = value;
                    if (!itemless && this.MenuItems.Count == 0)
                    {
                        MenuHandler.CloseAndClearHistory();
                        throw new Exception($"UIMenu {this.Title} menu is empty... Closing and clearing history.");
                    }

                    Main.InstructionalButtons.SetInstructionalButtons(this.InstructionalButtons);
                    canBuild = true;
                    MenuHandler.currentMenu = this;
                    MenuHandler.ableToDraw = true;
                    BuildUpMenuAsync();
                    MenuOpenEv(this, null);
                    timeBeforeOverflow = Main.GameTime;
                    if (BreadcrumbsHandler.Count == 0)
                        BreadcrumbsHandler.Forward(this, null);
                    AddTextEntry("UIMenu_Current_Description", CurrentItem.Description);
                }
                else
                {
                    _visible = value;
                    Main.InstructionalButtons.ClearButtonList();
                    canBuild = false;
                    MenuCloseEv(this);
                    MenuHandler.ableToDraw = false;
                    MenuHandler.currentMenu = null;
                    _unfilteredMenuItems.Clear();
                    AddTextEntry("UIMenu_Current_Description", "");
                }
                if (!value) return;
                if (!ResetCursorOnOpen) return;
                SetCursorLocation(0.5f, 0.5f);
                Screen.Hud.CursorSprite = CursorSprite.Normal;
            }
        }

        internal async void BuildUpMenuAsync(bool itemsOnly = false, bool skipViewInitialization = false)
        {
            isBuilding = true;
            while (!Main.scaleformUI.IsLoaded) await BaseScript.Delay(0);
            if (!itemsOnly)
            {
                SetMenuData(skipViewInitialization);
                SetWindows();
            }

            if (!Visible) return;
            SendItems();

            //Pagination.ScaleformIndex = Pagination.GetScaleformIndex(CurrentSelection);
            Main.scaleformUI.CallFunction("SET_VISIBLE", _visible, CurrentSelection, topEdge);
            if (CurrentItem is UIMenuSeparatorItem sp && sp.Jumpable)
                GoDown();
            SendPanelsToItemScaleform(CurrentSelection);
            SendSidePanelToScaleform(CurrentSelection);
            Main.scaleformUI.CallFunction("ENABLE_MOUSE", MouseControlsEnabled);
            isBuilding = false;
        }

        private void SetMenuData(bool skipViewInitialization)
        {
            if (itemless)
            {
                BeginScaleformMovieMethod(Main.scaleformUI.Handle, "SET_MENU_DATA");
                PushScaleformMovieMethodParameterString(Title);
                PushScaleformMovieMethodParameterString(SubtitleColor != HudColor.NONE ? "~" + SubtitleColor + "~" + Subtitle : Subtitle);
                PushScaleformMovieMethodParameterFloat(Offset.X);
                PushScaleformMovieMethodParameterFloat(Offset.Y);
                PushScaleformMovieMethodParameterBool(AlternativeTitle);
                PushScaleformMovieMethodParameterString(_customTexture.Key);
                PushScaleformMovieMethodParameterString(_customTexture.Value);
                PushScaleformMovieFunctionParameterInt(MaxItemsOnScreen);
                PushScaleformMovieFunctionParameterInt(MenuItems.Count);
                PushScaleformMovieFunctionParameterInt(counterColor.ArgbValue);
                PushScaleformMovieMethodParameterString(descriptionFont.FontName);
                PushScaleformMovieFunctionParameterInt(descriptionFont.FontID);
                PushScaleformMovieFunctionParameterInt(bannerColor.ArgbValue);
                PushScaleformMovieFunctionParameterBool(true);
                BeginTextCommandScaleformString("ScaleformUILongDesc");
                EndTextCommandScaleformString_2();
                PushScaleformMovieMethodParameterString(_customBGTexture.Key);
                PushScaleformMovieMethodParameterString(_customBGTexture.Value);
                PushScaleformMovieFunctionParameterInt((int)menuAlignment);
                PushScaleformMovieFunctionParameterBool(true);
                EndScaleformMovieMethod();
                isBuilding = false;
                return;
            }
            Main.scaleformUI.CallFunction("SET_MENU_DATA", Title, SubtitleColor != HudColor.NONE ? "~" + SubtitleColor + "~" + Subtitle : Subtitle, Offset.X, Offset.Y, AlternativeTitle, _customTexture.Key, _customTexture.Value, MaxItemsOnScreen, MenuItems.Count, counterColor, descriptionFont.FontName, descriptionFont.FontID, bannerColor.ArgbValue, false, "", _customBGTexture.Key, _customBGTexture.Value, (int)menuAlignment, skipViewInitialization);
        }

        internal void SetWindows(bool update = false)
        {
            if (Windows.Count == 0)
            {
                Main.scaleformUI.CallFunction("SET_WINDOWS_SLOT_DATA_EMPTY");
                return;
            }
            if(!update)
            Main.scaleformUI.CallFunction("SET_WINDOWS_SLOT_DATA_EMPTY");
            var str = update ? "UPDATE_WINDOWS_SLOT_DATA" : "SET_WINDOWS_SLOT_DATA";
            for (int i = 0; i < Windows.Count; i++)
            {
                UIMenuWindow wind = Windows[i];
                switch (wind)
                {
                    case UIMenuHeritageWindow:
                        UIMenuHeritageWindow her = (UIMenuHeritageWindow)wind;
                        Main.scaleformUI.CallFunction(str, i, her.id, her.Mom, her.Dad);
                        break;
                    case UIMenuDetailsWindow:
                        UIMenuDetailsWindow det = (UIMenuDetailsWindow)wind;
                        Main.scaleformUI.CallFunction(str, i, det.id, det.DetailBottom, det.DetailMid, det.DetailTop, det.DetailLeft.Txd, det.DetailLeft.Txn, det.DetailLeft.Pos.X, det.DetailLeft.Pos.Y, det.DetailLeft.Size.Width, det.DetailLeft.Size.Height);
                        if (det.StatWheelEnabled)
                        {
                            for (var j = 0; j < det.DetailStats.Count; j++)
                            {
                                UIDetailStat stat = det.DetailStats[j];
                                Main.scaleformUI.CallFunction("SET_WINDOWS_SLOT_EXTRA_DATA", i, j, stat.Percentage, stat.HudColor);
                            }
                        }
                        break;
                }
            }
            if(!update)
            Main.scaleformUI.CallFunction("SHOW_WINDOWS");
        }

        private void SendItems()
        {
            Main.scaleformUI.CallFunction("SET_DATA_SLOT_EMPTY");
            for (int i = 0; i < MenuItems.Count; i++)
            {
                if (MenuItems.Count <= i)
                    break;
                SendItemToScaleform(i);
                if (_visibleItems < MaxItemsOnScreen)
                    _visibleItems++;
            }
        }

        internal void SendSidePanelToScaleform(int i, bool update = false)
        {
            int index = i - topEdge;
            var item = MenuItems[i];
            if (item.SidePanel == null || !item.Enabled || index < 0 || index > MaxItemsOnScreen)
            {
                Main.scaleformUI.CallFunction("SET_SIDE_PANEL_DATA_SLOT_EMPTY");
                return;
            }
            if (!update) Main.scaleformUI.CallFunction("SET_SIDE_PANEL_DATA_SLOT_EMPTY");

            var str = update ? "UPDATE_SIDE_PANEL_DATA_SLOT" : "SET_SIDE_PANEL_DATA_SLOT";

            switch (item.SidePanel)
            {
                case UIMissionDetailsPanel:
                    UIMissionDetailsPanel mis = (UIMissionDetailsPanel)item.SidePanel;
                    Main.scaleformUI.CallFunction(str, index, 0, (int)mis.PanelSide, (int)mis._titleType, mis.Title, mis.TitleColor, mis.TextureDict, mis.TextureName);
                    foreach (UIFreemodeDetailsItem _it in mis.Items)
                    {
                        Main.scaleformUI.CallFunction("SET_SIDE_PANEL_SLOT", index, _it.Type, _it.TextLeft, _it.TextRight, (int)_it.Icon, _it.IconColor, _it.Tick, _it._labelFont.FontName, _it._labelFont.FontID, _it._rightLabelFont.FontName, _it._rightLabelFont.FontID);
                    }
                    break;
                case UIVehicleColourPickerPanel:
                    UIVehicleColourPickerPanel cp = (UIVehicleColourPickerPanel)item.SidePanel;
                    Main.scaleformUI.CallFunction(str, index, 1, (int)cp.PanelSide, (int)cp._titleType, cp.Title, cp.TitleColor);
                    break;
            }
            if (!update)
                Main.scaleformUI.CallFunction("SHOW_SIDE_PANEL");
        }

        internal void SendPanelsToItemScaleform(int i, bool update = false)
        {
            int index = i - topEdge;
            var item = MenuItems[i];

            if (item.Panels.Count == 0 || !item.Enabled || index < 0 || index > MaxItemsOnScreen)
            {
                Main.scaleformUI.CallFunction("SET_PANEL_DATA_SLOT_EMPTY");
                return;
            }
            if (!update) Main.scaleformUI.CallFunction("SET_PANEL_DATA_SLOT_EMPTY");

            var str = update ? "UPDATE_PANEL_DATA_SLOT" : "SET_PANEL_DATA_SLOT";
            foreach (UIMenuPanel panel in item.Panels)
            {
                int pan = item.Panels.IndexOf(panel);
                switch (panel)
                {
                    case UIMenuColorPanel:
                        UIMenuColorPanel cp = (UIMenuColorPanel)panel;
                        Main.scaleformUI.CallFunction(str, index, pan, 0, cp.Title, (int)cp.ColorPanelColorType, cp.CurrentSelection, cp.CustomColors is not null ? string.Join(",", cp.CustomColors.Select(x => x.ArgbValue)) : "");
                        break;
                    case UIMenuPercentagePanel:
                        UIMenuPercentagePanel pp = (UIMenuPercentagePanel)panel;
                        Main.scaleformUI.CallFunction(str, index, pan, 1, pp.Title, pp.Min, pp.Max, pp.Percentage);
                        break;
                    case UIMenuGridPanel:
                        UIMenuGridPanel gp = (UIMenuGridPanel)panel;
                        Main.scaleformUI.CallFunction(str, index, pan, 2, gp.TopLabel, gp.RightLabel, gp.LeftLabel, gp.BottomLabel, gp.CirclePosition.X, gp.CirclePosition.Y, true, (int)gp.GridType);
                        break;
                    case UIMenuStatisticsPanel:
                        UIMenuStatisticsPanel sp = (UIMenuStatisticsPanel)panel;
                        Main.scaleformUI.CallFunction(str, index, pan, 3, string.Join(",", sp.Items));
                        break;
                    case UIMenuColourPickePanel:
                        Main.scaleformUI.CallFunction(str, index, pan, 4);
                        break;
                }
            }
            if (!update) 
                Main.scaleformUI.CallFunction("SHOW_PANELS");
        }

        internal void SendItemToScaleform(int i, bool update = false, bool newItem = false)
        {
            UIMenuItem item = MenuItems[i];
            string str = "SET_DATA_SLOT";
            if (update)
                str = "UPDATE_DATA_SLOT";
            if (newItem)
                str = "SET_DATA_SLOT_SPLICE";

            BeginScaleformMovieMethod(Main.scaleformUI.Handle, str);
            // here start
            PushScaleformMovieFunctionParameterInt(i); // slot, menuIndex
            PushScaleformMovieFunctionParameterInt(item._itemId);//id
            PushScaleformMovieMethodParameterString(item._formatLeftLabel);
            PushScaleformMovieFunctionParameterBool(item.Enabled);
            PushScaleformMovieFunctionParameterBool(item.BlinkDescription);
            switch (item)
            {
                case UIMenuDynamicListItem:
                    UIMenuDynamicListItem dit = (UIMenuDynamicListItem)item;
                    var curString = dit.Selected ? (dit.CurrentListItem.StartsWith("~") ? dit.CurrentListItem : "~s~" + dit.CurrentListItem).ToString().Replace("~w~", "~l~").Replace("~s~", "~l~") : (dit.CurrentListItem.StartsWith("~") ? dit.CurrentListItem : "~s~" + dit.CurrentListItem).ToString().Replace("~l~", "~s~");
                    if (!dit.Enabled)
                       curString = curString.ReplaceRstarColorsWith("~c~");
                    PushScaleformMovieMethodParameterString(curString);
                    PushScaleformMovieFunctionParameterInt(0);
                    PushScaleformMovieFunctionParameterInt(dit.MainColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(dit.HighlightColor.ArgbValue);
                    break;
                case UIMenuCheckboxItem:
                    UIMenuCheckboxItem check = (UIMenuCheckboxItem)item;
                    PushScaleformMovieFunctionParameterInt((int)check.Style);
                    PushScaleformMovieMethodParameterBool(check.Checked);
                    PushScaleformMovieFunctionParameterInt(check.MainColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(check.HighlightColor.ArgbValue);
                    break;
                case UIMenuSliderItem:
                    UIMenuSliderItem prItem = (UIMenuSliderItem)item;
                    PushScaleformMovieFunctionParameterInt(prItem._max);
                    PushScaleformMovieFunctionParameterInt(prItem._multiplier);
                    PushScaleformMovieFunctionParameterInt(prItem.Value);
                    PushScaleformMovieFunctionParameterInt(prItem.MainColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(prItem.HighlightColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(prItem.SliderColor.ArgbValue);
                    PushScaleformMovieFunctionParameterBool(prItem._heritage);
                    break;
                case UIMenuProgressItem:
                    UIMenuProgressItem slItem = (UIMenuProgressItem)item;
                    PushScaleformMovieFunctionParameterInt(slItem._max);
                    PushScaleformMovieFunctionParameterInt(slItem._multiplier);
                    PushScaleformMovieFunctionParameterInt(slItem.Value);
                    PushScaleformMovieFunctionParameterInt(slItem.MainColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(slItem.HighlightColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(slItem.SliderColor.ArgbValue);
                    break;
                case UIMenuStatsItem:
                    UIMenuStatsItem statsItem = (UIMenuStatsItem)item;
                    PushScaleformMovieFunctionParameterInt(statsItem.Value);
                    PushScaleformMovieFunctionParameterInt(statsItem.Type);
                    PushScaleformMovieFunctionParameterInt(statsItem.Color.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(statsItem.MainColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(statsItem.HighlightColor.ArgbValue);
                    break;
                case UIMenuSeparatorItem:
                    UIMenuSeparatorItem separatorItem = (UIMenuSeparatorItem)item;
                    PushScaleformMovieFunctionParameterBool(separatorItem.Jumpable);
                    PushScaleformMovieFunctionParameterInt(item.MainColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(item.HighlightColor.ArgbValue);
                    break;
                default:
                    PushScaleformMovieFunctionParameterInt(item.MainColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(item.HighlightColor.ArgbValue);
                    break;
            }
            PushScaleformMovieMethodParameterString(item._formatRightLabel);
            PushScaleformMovieFunctionParameterInt((int)item.LeftBadge);
            PushScaleformMovieMethodParameterString(item.customLeftBadge.Key);
            PushScaleformMovieMethodParameterString(item.customLeftBadge.Value);
            PushScaleformMovieFunctionParameterInt((int)item.RightBadge);
            PushScaleformMovieMethodParameterString(item.customRightBadge.Key);
            PushScaleformMovieMethodParameterString(item.customRightBadge.Value);
            PushScaleformMovieMethodParameterString(item.labelFont.FontName);
            PushScaleformMovieMethodParameterString(item.rightLabelFont.FontName);
            EndScaleformMovieMethod();
        }

        /// <summary>
        /// Allows controlling all mouse aspects of the Menu.
        /// </summary>
        /// <param name="enableMouseControls">Enable mouse control</param>
        /// <param name="enableEdge">Enables edge camera rotation</param>
        /// <param name="isWheelEnabled">Enables mouse wheel to scroll items</param>
        /// <param name="resetCursorOnOpen">Resets cursor's position on menu open</param>
        /// <param name="leftClickSelect">If MouseControls are not enabled and this is true, left click selects the current item without pointing</param>
        public void SetMouse(bool enableMouseControls, bool enableEdge, bool isWheelEnabled, bool resetCursorOnOpen, bool leftClickSelect)
        {
            MouseControlsEnabled = enableMouseControls;
            MouseEdgeEnabled = enableEdge;
            MouseWheelControlEnabled = isWheelEnabled;
            ResetCursorOnOpen = resetCursorOnOpen;
            leftClickEnabled = leftClickSelect;
            if (leftClickSelect && !MouseControlsEnabled)
            {
                SetKey(MenuControls.Select, Control.CursorAccept);

            }
            else
            {
                ResetKey(MenuControls.Select);
                SetKey(MenuControls.Select, Control.FrontendAccept);
            }
        }

        /// <summary>
        /// This function is obsolete and will be removed soon
        /// </summary>
        [Obsolete("Not used anymore")]
        public void SetAnimations(bool enableScrollingAnim, bool enable3DAnim, MenuAnimationType scrollingAnim = MenuAnimationType.QUADRATIC_IN, MenuBuildingAnimation buildingAnim = MenuBuildingAnimation.LEFT_RIGHT, float fadingTime = 0.1f)
        {
        }

        /// <summary>
        /// Sorts menu items based on the desired predicated
        /// </summary>
        /// <param name="compare"></param>
        public void SortMenuItems(Comparison<UIMenuItem> compare)
        {
            if (itemless) throw new("ScaleformUI - You can't compare or sort an itemless menu");
            try
            {
                MenuItems[CurrentSelection].Selected = false;
                _unfilteredMenuItems = MenuItems.ToList();
                Clear();
                List<UIMenuItem> list = _unfilteredMenuItems.ToList();
                list.Sort(compare);
                MenuItems = list.ToList();
                BuildUpMenuAsync(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ScaleformUI - " + ex.ToString());
            }
        }

        /// <summary>
        /// Filters menu items based on the desired predicate
        /// </summary>
        /// <param name="predicate"></param>
        public void FilterMenuItems(Func<UIMenuItem, bool> predicate)
        {
            if (itemless) throw new("ScaleformUI - You can't compare or sort an itemless menu");
            try
            {
                _unfilteredMenuItems = MenuItems.ToList();
                var filteredItems = MenuItems.Where(predicate.Invoke).ToList();
                if (filteredItems.Count == 0)
                {
                    Debug.WriteLine($"^1ScaleformUI - No items were found, resetting the filter");
                    return;
                }
                MenuItems[CurrentSelection].Selected = false;
                Clear();
                MenuItems = filteredItems;
                topEdge = 0;
                CurrentSelection = 0;
                BuildUpMenuAsync(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("^1ScaleformUI - " + ex.ToString());
                OnFilteringFailed?.Invoke(this);
            }
        }

        /// <summary>
        /// Resets filtering/ordering of items going back to the original order.
        /// </summary>
        public void ResetFilter()
        {
            if (itemless) throw new("ScaleformUI - You can't compare or sort an itemless menu");
            try
            {
                MenuItems[CurrentSelection].Selected = false;
                Clear();
                MenuItems = _unfilteredMenuItems.ToList();
                BuildUpMenuAsync(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ScaleformUI - " + ex.ToString());
            }
        }

        public UIMenuItem CurrentItem
        {
            get => MenuItems[CurrentSelection];
            set => CurrentSelection = MenuItems.Any(x => x.Label == value.Label && x.Description == value.Description) ? MenuItems.IndexOf(value) : 0;
        }


        /// <summary>
        ///  Set's the menus offset after initialization.
        /// </summary>
        /// <param name="offset"></param>
        public async void SetMenuOffset(PointF offset)
        {
            Offset = offset;
            if (!Visible) return;
            float safezone = (1.0f - (float)decimal.Round(Convert.ToDecimal(GetSafeZoneSize()), 2)) * 100f * 0.005f;
            bool rightAlign = MenuAlignment == MenuAlignment.RIGHT;
            var pos1080 = ScreenTools.ConvertScaleformCoordsToResolutionCoords(Offset.X, Offset.Y);
            var screenCoords = ScreenTools.ConvertResolutionCoordsToScreenCoords(pos1080.X, pos1080.Y);

            var startPoint = 0.45f + safezone;
            if (rightAlign)
                startPoint = 1.225f - safezone;
            
            glarePosition = new Vector2(screenCoords.X + startPoint, screenCoords.Y + 0.45f + safezone);
            if (rightAlign)
            {
                int w = 0, h = 0;
                GetScreenActiveResolution(ref w, ref h);
                screenCoords = ScreenTools.ConvertResolutionCoordsToScreenCoords(1920 - pos1080.X, pos1080.Y);
                glarePosition = new Vector2(screenCoords.X - 1 + startPoint, screenCoords.Y + 0.45f + safezone);
            }
            glareSize = new SizeF(ScreenTools.GetWideScreen() ? 1.35f : 1f, 1f);
            SetMenuData(true);
        }

        /// <summary>
        /// Returns the current selected item's index.
        /// Change the current selected item to index. Use this after you add or remove items dynamically.
        /// </summary>
        public int CurrentSelection
        {
            get { return MenuItems.Count == 0 ? 0 : _currentSelection; }
            set
            {
                if (CurrentSelection < MenuItems.Count)
                    MenuItems[CurrentSelection].Selected = false;
                _currentSelection = Math.Max(0, Math.Min(value, MenuItems.Count - 1));

                if (_currentSelection > topEdge + _visibleItems)
                    topEdge = Math.Max(0, Math.Min(_currentSelection, MenuItems.Count - 1 - _visibleItems));
                else if (_currentSelection < topEdge)
                    topEdge = _currentSelection;


                if (_visible)
                {
                    AddTextEntry("UIMenu_Current_Description", CurrentItem.Description);
                    Main.scaleformUI.CallFunction("SET_CURRENT_SELECTION", _currentSelection, topEdge);
                    SendPanelsToItemScaleform(_currentSelection);
                    SendSidePanelToScaleform(_currentSelection);
                }
                MenuItems[value].Selected = true;
            }
        }


        /// <summary>
        /// Returns false if last input was made with mouse and keyboard, true if it was made with a controller.
        /// </summary>
        public static bool IsUsingController => !IsInputDisabled(2);


        /// <summary>
        /// Returns the amount of items in the menu.
        /// </summary>
        public int Size => MenuItems.Count;


        /// <summary>
        /// Returns the title object.
        /// </summary>
        public string Title
        {
            get => title;
            set
            {
                title = value;
                if (Visible)
                {
                    //Main.scaleformUI.CallFunction("UPDATE_TITLE_SUBTITLE", title, SubtitleColor != HudColor.NONE ? "~" + SubtitleColor + "~" + Subtitle : Subtitle, AlternativeTitle);
                    SetMenuData(true);
                }
            }
        }


        /// <summary>
        /// Returns the subtitle object.
        /// </summary>
        public string Subtitle
        {
            get => subtitle;
            set
            {
                subtitle = value;
                if (Visible)
                {
                    //Main.scaleformUI.CallFunction("UPDATE_TITLE_SUBTITLE", title, SubtitleColor != HudColor.NONE ? "~" + SubtitleColor + "~" + Subtitle : Subtitle, AlternativeTitle);
                    SetMenuData(true);
                }
            }
        }

        /// <summary>
        /// Set the CounterText color.
        /// </summary>
        public SColor CounterColor
        {
            get => counterColor;
            set
            {
                counterColor = value;
                if (Visible)
                {
                    //Main.scaleformUI.CallFunction("SET_COUNTER_COLOR", counterColor);
                    SetMenuData(true);
                }
            }
        }

        /// <summary>
        /// Returns the current width offset.
        /// </summary>
        public int WidthOffset { get; private set; }
        public bool MouseControlsEnabled
        {
            get => mouseControlsEnabled;
            set
            {
                mouseControlsEnabled = value;
                if (Visible)
                {
                    //Main.scaleformUI.CallFunction("ENABLE_MOUSE", value);
                    SetMenuData(true);
                }
            }
        }

        public bool CanPlayerCloseMenu
        {
            get => canPlayerCloseMenu;
            set
            {
                canPlayerCloseMenu = value;
                if (value)
                {
                    InstructionalButtons.Insert(1, new InstructionalButton(Control.FrontendCancel, _backTextLocalized));
                }
                else
                {
                    InstructionalButtons.RemoveAt(1);
                }
            }
        }

        #endregion

        #region Event Invokers
        protected virtual void IndexChange(int newindex)
        {
            OnIndexChange?.Invoke(this, newindex);
        }

        internal virtual void ListChange(UIMenuListItem sender, int newindex)
        {
            OnListChange?.Invoke(this, sender, newindex);
        }

        internal virtual void ProgressChange(UIMenuProgressItem sender, int newindex)
        {
            OnProgressChange?.Invoke(this, sender, newindex);
        }

        protected virtual void ListSelect(UIMenuListItem sender, int newindex)
        {
            OnListSelect?.Invoke(this, sender, newindex);
        }

        protected virtual void SliderChange(UIMenuSliderItem sender, int newindex)
        {
            OnSliderChange?.Invoke(this, sender, newindex);
        }

        protected virtual void ItemSelect(UIMenuItem selecteditem, int index)
        {
            OnItemSelect?.Invoke(this, selecteditem, index);
        }

        protected virtual void CheckboxChange(UIMenuCheckboxItem sender, bool Checked)
        {
            OnCheckboxChange?.Invoke(this, sender, Checked);
        }

        public virtual void StatItemChange(UIMenuStatsItem item, int value)
        {
            OnStatsItemChanged?.Invoke(this, item, value);
        }

        public virtual void MenuOpenEv(UIMenu menu, dynamic data)
        {
            OnMenuOpen?.Invoke(menu, data);
        }

        public virtual void MenuCloseEv(UIMenu menu)
        {
            OnMenuClose?.Invoke(menu);
            if (menu._unfilteredMenuItems.Count > 0)
                menu.ResetFilter();
        }

        internal virtual void ColorPanelChange(UIMenuItem item, UIMenuColorPanel panel, int index)
        {
            OnColorPanelChange?.Invoke(item, panel, index);
        }
        internal virtual void PercentagePanelChange(UIMenuItem item, UIMenuPercentagePanel panel, float index)
        {
            OnPercentagePanelChange?.Invoke(item, panel, index);
        }
        internal virtual void GridPanelChange(UIMenuItem item, UIMenuGridPanel panel, PointF index)
        {
            OnGridPanelChange?.Invoke(item, panel, index);
        }

        #endregion

        public enum MenuControls
        {
            Up,
            Down,
            Left,
            Right,
            Select,
            Back,
            PageUp,
            PageDown
        }

        public override bool Equals(object obj)
        {
            return obj is UIMenu menu && menu.Title == Title && menu.Subtitle == Subtitle;
        }

    }
}

