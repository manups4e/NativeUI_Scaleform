using CitizenFX.Core;
using CitizenFX.FiveM;
using CitizenFX.FiveM.GUI;
using ScaleformUI.Elements;
using ScaleformUI.Menus;
using ScaleformUI.Scaleforms;
using System.Drawing;
using static CitizenFX.FiveM.Native.Natives;
using Control = CitizenFX.FiveM.Control;

namespace ScaleformUI.Menu
{
    #region Delegates
    public enum Keys
    {
        //
        // Summary:
        //     The bitmask to extract modifiers from a key value.
        Modifiers = -65536,
        //
        // Summary:
        //     No key pressed.
        None = 0,
        //
        // Summary:
        //     The left mouse button.
        LButton = 1,
        //
        // Summary:
        //     The right mouse button.
        RButton = 2,
        //
        // Summary:
        //     The CANCEL key.
        Cancel = 3,
        //
        // Summary:
        //     The middle mouse button (three-button mouse).
        MButton = 4,
        //
        // Summary:
        //     The first x mouse button (five-button mouse).
        XButton1 = 5,
        //
        // Summary:
        //     The second x mouse button (five-button mouse).
        XButton2 = 6,
        //
        // Summary:
        //     The BACKSPACE key.
        Back = 8,
        //
        // Summary:
        //     The TAB key.
        Tab = 9,
        //
        // Summary:
        //     The LINEFEED key.
        LineFeed = 10,
        //
        // Summary:
        //     The CLEAR key.
        Clear = 12,
        //
        // Summary:
        //     The RETURN key.
        Return = 13,
        //
        // Summary:
        //     The ENTER key.
        Enter = 13,
        //
        // Summary:
        //     The SHIFT key.
        ShiftKey = 16,
        //
        // Summary:
        //     The CTRL key.
        ControlKey = 17,
        //
        // Summary:
        //     The ALT key.
        Menu = 18,
        //
        // Summary:
        //     The PAUSE key.
        Pause = 19,
        //
        // Summary:
        //     The CAPS LOCK key.
        Capital = 20,
        //
        // Summary:
        //     The CAPS LOCK key.
        CapsLock = 20,
        //
        // Summary:
        //     The IME Kana mode key.
        KanaMode = 21,
        //
        // Summary:
        //     The IME Hanguel mode key. (maintained for compatibility; use HangulMode)
        HanguelMode = 21,
        //
        // Summary:
        //     The IME Hangul mode key.
        HangulMode = 21,
        //
        // Summary:
        //     The IME Junja mode key.
        JunjaMode = 23,
        //
        // Summary:
        //     The IME final mode key.
        FinalMode = 24,
        //
        // Summary:
        //     The IME Hanja mode key.
        HanjaMode = 25,
        //
        // Summary:
        //     The IME Kanji mode key.
        KanjiMode = 25,
        //
        // Summary:
        //     The ESC key.
        Escape = 27,
        //
        // Summary:
        //     The IME convert key.
        IMEConvert = 28,
        //
        // Summary:
        //     The IME nonconvert key.
        IMENonconvert = 29,
        //
        // Summary:
        //     The IME accept key, replaces System.Windows.Forms.Keys.IMEAceept.
        IMEAccept = 30,
        //
        // Summary:
        //     The IME accept key. Obsolete, use System.Windows.Forms.Keys.IMEAccept instead.
        IMEAceept = 30,
        //
        // Summary:
        //     The IME mode change key.
        IMEModeChange = 31,
        //
        // Summary:
        //     The SPACEBAR key.
        Space = 32,
        //
        // Summary:
        //     The PAGE UP key.
        Prior = 33,
        //
        // Summary:
        //     The PAGE UP key.
        PageUp = 33,
        //
        // Summary:
        //     The PAGE DOWN key.
        Next = 34,
        //
        // Summary:
        //     The PAGE DOWN key.
        PageDown = 34,
        //
        // Summary:
        //     The END key.
        End = 35,
        //
        // Summary:
        //     The HOME key.
        Home = 36,
        //
        // Summary:
        //     The LEFT ARROW key.
        Left = 37,
        //
        // Summary:
        //     The UP ARROW key.
        Up = 38,
        //
        // Summary:
        //     The RIGHT ARROW key.
        Right = 39,
        //
        // Summary:
        //     The DOWN ARROW key.
        Down = 40,
        //
        // Summary:
        //     The SELECT key.
        Select = 41,
        //
        // Summary:
        //     The PRINT key.
        Print = 42,
        //
        // Summary:
        //     The EXECUTE key.
        Execute = 43,
        //
        // Summary:
        //     The PRINT SCREEN key.
        Snapshot = 44,
        //
        // Summary:
        //     The PRINT SCREEN key.
        PrintScreen = 44,
        //
        // Summary:
        //     The INS key.
        Insert = 45,
        //
        // Summary:
        //     The DEL key.
        Delete = 46,
        //
        // Summary:
        //     The HELP key.
        Help = 47,
        //
        // Summary:
        //     The 0 key.
        D0 = 48,
        //
        // Summary:
        //     The 1 key.
        D1 = 49,
        //
        // Summary:
        //     The 2 key.
        D2 = 50,
        //
        // Summary:
        //     The 3 key.
        D3 = 51,
        //
        // Summary:
        //     The 4 key.
        D4 = 52,
        //
        // Summary:
        //     The 5 key.
        D5 = 53,
        //
        // Summary:
        //     The 6 key.
        D6 = 54,
        //
        // Summary:
        //     The 7 key.
        D7 = 55,
        //
        // Summary:
        //     The 8 key.
        D8 = 56,
        //
        // Summary:
        //     The 9 key.
        D9 = 57,
        //
        // Summary:
        //     The A key.
        A = 65,
        //
        // Summary:
        //     The B key.
        B = 66,
        //
        // Summary:
        //     The C key.
        C = 67,
        //
        // Summary:
        //     The D key.
        D = 68,
        //
        // Summary:
        //     The E key.
        E = 69,
        //
        // Summary:
        //     The F key.
        F = 70,
        //
        // Summary:
        //     The G key.
        G = 71,
        //
        // Summary:
        //     The H key.
        H = 72,
        //
        // Summary:
        //     The I key.
        I = 73,
        //
        // Summary:
        //     The J key.
        J = 74,
        //
        // Summary:
        //     The K key.
        K = 75,
        //
        // Summary:
        //     The L key.
        L = 76,
        //
        // Summary:
        //     The M key.
        M = 77,
        //
        // Summary:
        //     The N key.
        N = 78,
        //
        // Summary:
        //     The O key.
        O = 79,
        //
        // Summary:
        //     The P key.
        P = 80,
        //
        // Summary:
        //     The Q key.
        Q = 81,
        //
        // Summary:
        //     The R key.
        R = 82,
        //
        // Summary:
        //     The S key.
        S = 83,
        //
        // Summary:
        //     The T key.
        T = 84,
        //
        // Summary:
        //     The U key.
        U = 85,
        //
        // Summary:
        //     The V key.
        V = 86,
        //
        // Summary:
        //     The W key.
        W = 87,
        //
        // Summary:
        //     The X key.
        X = 88,
        //
        // Summary:
        //     The Y key.
        Y = 89,
        //
        // Summary:
        //     The Z key.
        Z = 90,
        //
        // Summary:
        //     The left Windows logo key (Microsoft Natural Keyboard).
        LWin = 91,
        //
        // Summary:
        //     The right Windows logo key (Microsoft Natural Keyboard).
        RWin = 92,
        //
        // Summary:
        //     The application key (Microsoft Natural Keyboard).
        Apps = 93,
        //
        // Summary:
        //     The computer sleep key.
        Sleep = 95,
        //
        // Summary:
        //     The 0 key on the numeric keypad.
        NumPad0 = 96,
        //
        // Summary:
        //     The 1 key on the numeric keypad.
        NumPad1 = 97,
        //
        // Summary:
        //     The 2 key on the numeric keypad.
        NumPad2 = 98,
        //
        // Summary:
        //     The 3 key on the numeric keypad.
        NumPad3 = 99,
        //
        // Summary:
        //     The 4 key on the numeric keypad.
        NumPad4 = 100,
        //
        // Summary:
        //     The 5 key on the numeric keypad.
        NumPad5 = 101,
        //
        // Summary:
        //     The 6 key on the numeric keypad.
        NumPad6 = 102,
        //
        // Summary:
        //     The 7 key on the numeric keypad.
        NumPad7 = 103,
        //
        // Summary:
        //     The 8 key on the numeric keypad.
        NumPad8 = 104,
        //
        // Summary:
        //     The 9 key on the numeric keypad.
        NumPad9 = 105,
        //
        // Summary:
        //     The multiply key.
        Multiply = 106,
        //
        // Summary:
        //     The add key.
        Add = 107,
        //
        // Summary:
        //     The separator key.
        Separator = 108,
        //
        // Summary:
        //     The subtract key.
        Subtract = 109,
        //
        // Summary:
        //     The decimal key.
        Decimal = 110,
        //
        // Summary:
        //     The divide key.
        Divide = 111,
        //
        // Summary:
        //     The F1 key.
        F1 = 112,
        //
        // Summary:
        //     The F2 key.
        F2 = 113,
        //
        // Summary:
        //     The F3 key.
        F3 = 114,
        //
        // Summary:
        //     The F4 key.
        F4 = 115,
        //
        // Summary:
        //     The F5 key.
        F5 = 116,
        //
        // Summary:
        //     The F6 key.
        F6 = 117,
        //
        // Summary:
        //     The F7 key.
        F7 = 118,
        //
        // Summary:
        //     The F8 key.
        F8 = 119,
        //
        // Summary:
        //     The F9 key.
        F9 = 120,
        //
        // Summary:
        //     The F10 key.
        F10 = 121,
        //
        // Summary:
        //     The F11 key.
        F11 = 122,
        //
        // Summary:
        //     The F12 key.
        F12 = 123,
        //
        // Summary:
        //     The F13 key.
        F13 = 124,
        //
        // Summary:
        //     The F14 key.
        F14 = 125,
        //
        // Summary:
        //     The F15 key.
        F15 = 126,
        //
        // Summary:
        //     The F16 key.
        F16 = 127,
        //
        // Summary:
        //     The F17 key.
        F17 = 128,
        //
        // Summary:
        //     The F18 key.
        F18 = 129,
        //
        // Summary:
        //     The F19 key.
        F19 = 130,
        //
        // Summary:
        //     The F20 key.
        F20 = 131,
        //
        // Summary:
        //     The F21 key.
        F21 = 132,
        //
        // Summary:
        //     The F22 key.
        F22 = 133,
        //
        // Summary:
        //     The F23 key.
        F23 = 134,
        //
        // Summary:
        //     The F24 key.
        F24 = 135,
        //
        // Summary:
        //     The NUM LOCK key.
        NumLock = 144,
        //
        // Summary:
        //     The SCROLL LOCK key.
        Scroll = 145,
        //
        // Summary:
        //     The left SHIFT key.
        LShiftKey = 160,
        //
        // Summary:
        //     The right SHIFT key.
        RShiftKey = 161,
        //
        // Summary:
        //     The left CTRL key.
        LControlKey = 162,
        //
        // Summary:
        //     The right CTRL key.
        RControlKey = 163,
        //
        // Summary:
        //     The left ALT key.
        LMenu = 164,
        //
        // Summary:
        //     The right ALT key.
        RMenu = 165,
        //
        // Summary:
        //     The browser back key (Windows 2000 or later).
        BrowserBack = 166,
        //
        // Summary:
        //     The browser forward key (Windows 2000 or later).
        BrowserForward = 167,
        //
        // Summary:
        //     The browser refresh key (Windows 2000 or later).
        BrowserRefresh = 168,
        //
        // Summary:
        //     The browser stop key (Windows 2000 or later).
        BrowserStop = 169,
        //
        // Summary:
        //     The browser search key (Windows 2000 or later).
        BrowserSearch = 170,
        //
        // Summary:
        //     The browser favorites key (Windows 2000 or later).
        BrowserFavorites = 171,
        //
        // Summary:
        //     The browser home key (Windows 2000 or later).
        BrowserHome = 172,
        //
        // Summary:
        //     The volume mute key (Windows 2000 or later).
        VolumeMute = 173,
        //
        // Summary:
        //     The volume down key (Windows 2000 or later).
        VolumeDown = 174,
        //
        // Summary:
        //     The volume up key (Windows 2000 or later).
        VolumeUp = 175,
        //
        // Summary:
        //     The media next track key (Windows 2000 or later).
        MediaNextTrack = 176,
        //
        // Summary:
        //     The media previous track key (Windows 2000 or later).
        MediaPreviousTrack = 177,
        //
        // Summary:
        //     The media Stop key (Windows 2000 or later).
        MediaStop = 178,
        //
        // Summary:
        //     The media play pause key (Windows 2000 or later).
        MediaPlayPause = 179,
        //
        // Summary:
        //     The launch mail key (Windows 2000 or later).
        LaunchMail = 180,
        //
        // Summary:
        //     The select media key (Windows 2000 or later).
        SelectMedia = 181,
        //
        // Summary:
        //     The start application one key (Windows 2000 or later).
        LaunchApplication1 = 182,
        //
        // Summary:
        //     The start application two key (Windows 2000 or later).
        LaunchApplication2 = 183,
        //
        // Summary:
        //     The OEM Semicolon key on a US standard keyboard (Windows 2000 or later).
        OemSemicolon = 186,
        //
        // Summary:
        //     The OEM 1 key.
        Oem1 = 186,
        //
        // Summary:
        //     The OEM plus key on any country/region keyboard (Windows 2000 or later).
        Oemplus = 187,
        //
        // Summary:
        //     The OEM comma key on any country/region keyboard (Windows 2000 or later).
        Oemcomma = 188,
        //
        // Summary:
        //     The OEM minus key on any country/region keyboard (Windows 2000 or later).
        OemMinus = 189,
        //
        // Summary:
        //     The OEM period key on any country/region keyboard (Windows 2000 or later).
        OemPeriod = 190,
        //
        // Summary:
        //     The OEM question mark key on a US standard keyboard (Windows 2000 or later).
        OemQuestion = 191,
        //
        // Summary:
        //     The OEM 2 key.
        Oem2 = 191,
        //
        // Summary:
        //     The OEM tilde key on a US standard keyboard (Windows 2000 or later).
        Oemtilde = 192,
        //
        // Summary:
        //     The OEM 3 key.
        Oem3 = 192,
        //
        // Summary:
        //     The OEM open bracket key on a US standard keyboard (Windows 2000 or later).
        OemOpenBrackets = 219,
        //
        // Summary:
        //     The OEM 4 key.
        Oem4 = 219,
        //
        // Summary:
        //     The OEM pipe key on a US standard keyboard (Windows 2000 or later).
        OemPipe = 220,
        //
        // Summary:
        //     The OEM 5 key.
        Oem5 = 220,
        //
        // Summary:
        //     The OEM close bracket key on a US standard keyboard (Windows 2000 or later).
        OemCloseBrackets = 221,
        //
        // Summary:
        //     The OEM 6 key.
        Oem6 = 221,
        //
        // Summary:
        //     The OEM singled/double quote key on a US standard keyboard (Windows 2000 or later).
        OemQuotes = 222,
        //
        // Summary:
        //     The OEM 7 key.
        Oem7 = 222,
        //
        // Summary:
        //     The OEM 8 key.
        Oem8 = 223,
        //
        // Summary:
        //     The OEM angle bracket or backslash key on the RT 102 key keyboard (Windows 2000
        //     or later).
        OemBackslash = 226,
        //
        // Summary:
        //     The OEM 102 key.
        Oem102 = 226,
        //
        // Summary:
        //     The PROCESS KEY key.
        ProcessKey = 229,
        //
        // Summary:
        //     Used to pass Unicode characters as if they were keystrokes. The Packet key value
        //     is the low word of a 32-bit virtual-key value used for non-keyboard input methods.
        Packet = 231,
        //
        // Summary:
        //     The ATTN key.
        Attn = 246,
        //
        // Summary:
        //     The CRSEL key.
        Crsel = 247,
        //
        // Summary:
        //     The EXSEL key.
        Exsel = 248,
        //
        // Summary:
        //     The ERASE EOF key.
        EraseEof = 249,
        //
        // Summary:
        //     The PLAY key.
        Play = 250,
        //
        // Summary:
        //     The ZOOM key.
        Zoom = 251,
        //
        // Summary:
        //     A constant reserved for future use.
        NoName = 252,
        //
        // Summary:
        //     The PA1 key.
        Pa1 = 253,
        //
        // Summary:
        //     The CLEAR key.
        OemClear = 254,
        //
        // Summary:
        //     The bitmask to extract a key code from a key value.
        KeyCode = 65535,
        //
        // Summary:
        //     The SHIFT modifier key.
        Shift = 65536,
        //
        // Summary:
        //     The CTRL modifier key.
        Control = 131072,
        //
        // Summary:
        //     The ALT modifier key.
        Alt = 262144
    }

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
    public delegate void VehicleColorPickerSelectEvent(UIMenuItem menu, UIVehicleColourPickerPanel panel, int index);
    public delegate void PercentagePanelChangedEvent(UIMenuItem menu, UIMenuPercentagePanel panel, float value);
    public delegate void GridPanelChangedEvent(UIMenuItem menu, UIMenuGridPanel panel, PointF value);
    public delegate void MenuOpenedEvent(UIMenu menu, dynamic data = null);
    public delegate void MenuClosedEvent(UIMenu menu);
    public delegate void ItemHighlightedEvent(UIMenu menu, UIMenuItem item);

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
        internal PaginationHandler Pagination;

        internal KeyValuePair<string, string> _customTexture;

        /// <summary>
        /// Players won't be able to close the menu if this is false! Make sure players can close the menu in some way!!!!!!
        /// </summary>
        private bool canPlayerCloseMenu = true;
        //Pagination
        public int MaxItemsOnScreen
        {
            get => Pagination.ItemsPerPage;
            set
            {
                Pagination.ItemsPerPage = value;
            }
        }

        private bool mouseWheelControlEnabled = true;
        private int menuSound;
        private bool _changed = true;
        private bool keyboard = false;
        //Keys
        private readonly Dictionary<MenuControls, Tuple<List<Keys>, List<Tuple<Control, int>>>> _keyDictionary =
            new Dictionary<MenuControls, Tuple<List<Keys>, List<Tuple<Control, int>>>>();

        private readonly ScaleformWideScreen _menuGlare;

        private static readonly MenuControls[] _menuControls = Enum.GetValues(typeof(MenuControls)).Cast<MenuControls>().ToArray();

        private bool isBuilding = false;
        private MenuBuildingAnimation buildingAnimation = MenuBuildingAnimation.LEFT;
        private string title;
        private string subtitle;
        private SColor counterColor = SColor.HUD_Freemode;

        public bool Glare { get; set; }

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

        public List<UIMenuItem> MenuItems = new List<UIMenuItem>();
        public List<UIMenuItem> _unfilteredMenuItems = new List<UIMenuItem>();

        public bool MouseEdgeEnabled = true;
        public bool ControlDisablingEnabled = true;
        private bool enabled3DAnimations;
        internal bool leftClickEnabled;

        public bool EnableAnimation
        {
            get => enableAnimation;
            set
            {
                enableAnimation = value;
                if (Visible)
                {
                    Main.scaleformUI.CallFunction("ENABLE_SCROLLING_ANIMATION", enableAnimation);
                }
            }
        }

        public bool Enabled3DAnimations
        {
            get => enabled3DAnimations;
            set
            {
                enabled3DAnimations = value;
                if (Visible)
                {
                    Main.scaleformUI.CallFunction("ENABLE_3D_ANIMATIONS", enabled3DAnimations);
                }
            }
        }

        public MenuAnimationType AnimationType
        {
            get => animationType;
            set
            {
                animationType = value;
                if (Visible)
                {
                    Main.scaleformUI.CallFunction("CHANGE_SCROLLING_ANIMATION_TYPE", (int)animationType);
                }
            }
        }

        public MenuBuildingAnimation BuildingAnimation
        {
            get => buildingAnimation;
            set
            {
                buildingAnimation = value;
                if (Visible)
                {
                    Main.scaleformUI.CallFunction("CHANGE_BUILDING_ANIMATION_TYPE", (int)buildingAnimation);
                }
            }
        }

        public ScrollingType ScrollingType
        {
            get => scrollingType;
            set
            {
                Pagination.scrollType = value;
                scrollingType = value;
            }
        }


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
                    SetKey(MenuControls.Up, Control.PhoneUp);
                    SetKey(MenuControls.Down, Control.PhoneDown);
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
                    Main.scaleformUI.CallFunction("SET_DESC_FONT", descriptionFont.FontName, descriptionFont.FontID);
                }
            }
        }

        public bool ResetCursorOnOpen = true;
        private bool mouseControlsEnabled = true;
        public bool AlternativeTitle = false;
        private bool canBuild = true;
        private bool isFading;
        internal float fadingTime = 0.1f;
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
        public UIMenu(string title, string subtitle, bool glare = false, bool alternativeTitle = false, float fadingTime = 0.1f) : this(title, subtitle, new PointF(0, 0), "", "", glare, alternativeTitle, fadingTime)
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
        public UIMenu(string title, string subtitle, PointF offset, bool glare = false, bool alternativeTitle = false, float fadingTime = 0.1f) : this(title, subtitle, offset, "", "", glare, alternativeTitle, fadingTime)
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
        public UIMenu(string title, string subtitle, PointF offset, KeyValuePair<string, string> customBanner, bool glare = false, bool alternativeTitle = false, float fadingTime = 0.1f) : this(title, subtitle, offset, customBanner.Key, customBanner.Value, glare, alternativeTitle, fadingTime)
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
        public UIMenu(string title, string subtitle, PointF offset, string spriteLibrary, string spriteName, bool glare = false, bool alternativeTitle = false, float fadingTime = 0.1f)
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
            Pagination = new PaginationHandler();
            Pagination.ItemsPerPage = 7;
            this.fadingTime = fadingTime;

            SetKey(MenuControls.Up, Control.PhoneUp);
            SetKey(MenuControls.Down, Control.PhoneDown);

            SetKey(MenuControls.Left, Control.PhoneLeft);
            SetKey(MenuControls.Right, Control.PhoneRight);
            SetKey(MenuControls.Select, Control.FrontendAccept);

            SetKey(MenuControls.Back, Control.PhoneCancel);
            SetKey(MenuControls.Back, Control.FrontendPause);

            InstructionalButtons = new List<InstructionalButton>()
            {
                new InstructionalButton(Control.PhoneSelect, _selectTextLocalized),
                new InstructionalButton(Control.PhoneCancel, _backTextLocalized)
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
        public UIMenu(string title, string subtitle, string description, PointF offset, string spriteLibrary, string spriteName, bool glare = false, bool alternativeTitle = false, float fadingTime = 0.1f)
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
            Pagination = new PaginationHandler();
            Pagination.ItemsPerPage = 7;
            this.fadingTime = fadingTime;

            SetKey(MenuControls.Up, Control.PhoneUp);
            SetKey(MenuControls.Down, Control.PhoneDown);

            SetKey(MenuControls.Left, Control.PhoneLeft);
            SetKey(MenuControls.Right, Control.PhoneRight);
            SetKey(MenuControls.Select, Control.FrontendAccept);

            SetKey(MenuControls.Back, Control.PhoneCancel);
            SetKey(MenuControls.Back, Control.FrontendPause);

            InstructionalButtons = new List<InstructionalButton>()
            {
                new InstructionalButton(Control.PhoneSelect, _selectTextLocalized),
                new InstructionalButton(Control.PhoneCancel, _backTextLocalized)
            };
            itemless = true;
            AddTextEntry("ScaleformUILongDesc", description);
        }

        #endregion

        #region Public Methods

        public async Task FadeOutMenu()
        {
            Main.scaleformUI.CallFunction("FADE_OUT_MENU");
            do
            {
                await BaseScript.Delay(0);
                isFading = await Main.scaleformUI.CallFunctionReturnValueBool("GET_IS_FADING");
            } while (isFading);
        }
        public async Task FadeInMenu()
        {
            Main.scaleformUI.CallFunction("FADE_IN_MENU");
            do
            {
                await BaseScript.Delay(0);
                isFading = await Main.scaleformUI.CallFunctionReturnValueBool("GET_IS_FADING");
            } while (isFading);
        }
        public async Task FadeOutItems()
        {
            Main.scaleformUI.CallFunction("FADE_OUT_ITEMS");
            do
            {
                await BaseScript.Delay(0);
                isFading = await Main.scaleformUI.CallFunctionReturnValueBool("GET_IS_FADING");
            } while (isFading);
        }
        public async Task FadeInItems()
        {
            Main.scaleformUI.CallFunction("FADE_IN_ITEMS");
            do
            {
                await BaseScript.Delay(0);
                isFading = await Main.scaleformUI.CallFunctionReturnValueBool("GET_IS_FADING");
            } while (isFading);
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
        }

        /// <summary>
        /// Add an item to the menu.
        /// </summary>
        /// <param name="item">Item object to be added. Can be normal item, checkbox or list item.</param>
        public void AddItem(UIMenuItem item)
        {
            if (!itemless)
            {
                int selectedItem = CurrentSelection;
                item.Parent = this;
                MenuItems.Add(item);
                if (Visible)
                    CurrentSelection = selectedItem;
                Pagination.TotalItems = MenuItems.Count;
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
            BeginScaleformMovieMethod(Main.scaleformUI.Handle, "UPDATE_ITEM_DESCRIPTION");
            ScaleformMovieMethodAddParamInt(Pagination.GetScaleformIndex(CurrentSelection));
            BeginTextCommandScaleformString($"menu_{BreadcrumbsHandler.CurrentDepth}_desc_{CurrentSelection}");
            EndTextCommandScaleformString_2();
            EndScaleformMovieMethod();
        }

        /// <summary>
        /// Remove an item at index n.
        /// </summary>
        /// <param name="index">Index to remove the item at.</param>
        public void RemoveItemAt(int index)
        {
            int selectedItem = CurrentSelection;
            MenuItems.RemoveAt(index);
            if (Visible) Main.scaleformUI.CallFunction("REMOVE_ITEM", index);
            Pagination.TotalItems = MenuItems.Count;
            CurrentSelection = selectedItem;
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
            Main.scaleformUI.CallFunction("CLEAR_ITEMS");
            MenuItems.Clear();
            Pagination.Reset();
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
        /// Set a key to control a menu. Can be multiple keys for each control.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="keyToSet"></param>
        public void SetKey(MenuControls control, Keys keyToSet)
        {
            if (_keyDictionary.ContainsKey(control))
                _keyDictionary[control].Item1.Add(keyToSet);
            else
            {
                _keyDictionary.Add(control,
                    new Tuple<List<Keys>, List<Tuple<Control, int>>>(new List<Keys>(), new List<Tuple<Control, int>>()));
                _keyDictionary[control].Item1.Add(keyToSet);
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
                _keyDictionary[control].Item2.Add(new Tuple<Control, int>(gtaControl, controlIndex));
            else
            {
                _keyDictionary.Add(control,
                    new Tuple<List<Keys>, List<Tuple<Control, int>>>(new List<Keys>(), new List<Tuple<Control, int>>()));
                _keyDictionary[control].Item2.Add(new Tuple<Control, int>(gtaControl, controlIndex));
            }

        }


        /// <summary>
        /// Remove all controls on a control.
        /// </summary>
        /// <param name="control"></param>
        public void ResetKey(MenuControls control)
        {
            _keyDictionary[control].Item1.Clear();
            _keyDictionary[control].Item2.Clear();
        }

        /// <summary>
        /// Check whether a menucontrol has been pressed.
        /// </summary>
        /// <param name="control">Control to check for.</param>
        /// <param name="key">Key if you're using keys.</param>
        /// <returns></returns>
        public bool HasControlJustBeenPressed(MenuControls control, Keys key = Keys.None)
        {
            List<Keys> tmpKeys = new List<Keys>(_keyDictionary[control].Item1);
            List<Tuple<Control, int>> tmpControls = new List<Tuple<Control, int>>(_keyDictionary[control].Item2);

            if (key != Keys.None)
            {
                //if (tmpKeys.Any(Game.IsKeyPressed))
                //    return true;
            }
            if (tmpControls.Any(tuple => Game.IsControlJustPressed(tuple.Item2, tuple.Item1)))
                return true;
            return false;
        }


        /// <summary>
        /// Check whether a menucontrol has been released.
        /// </summary>
        /// <param name="control">Control to check for.</param>
        /// <param name="key">Key if you're using keys.</param>
        /// <returns></returns>
        public bool HasControlJustBeenReleased(MenuControls control, Keys key = Keys.None)
        {
            List<Keys> tmpKeys = new List<Keys>(_keyDictionary[control].Item1);
            List<Tuple<Control, int>> tmpControls = new List<Tuple<Control, int>>(_keyDictionary[control].Item2);

            if (key != Keys.None)
            {
                //if (tmpKeys.Any(Game.IsKeyPressed))
                //    return true;
            }
            if (tmpControls.Any(tuple => Game.IsControlJustReleased(tuple.Item2, tuple.Item1)))
                return true;
            return false;
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
        public bool IsControlBeingPressed(MenuControls control, Keys key = Keys.None)
        {
            List<Keys> tmpKeys = new List<Keys>(_keyDictionary[control].Item1);
            List<Tuple<Control, int>> tmpControls = new List<Tuple<Control, int>>(_keyDictionary[control].Item2);
            if (HasControlJustBeenReleased(control, key)) _controlCounter = 0;
            if (tmpControls.Any(tuple => Game.IsControlPressed(tuple.Item2, tuple.Item1)))
                return true;
            return false;
        }

        #endregion

        #region Drawing & Processing
        /// <summary>
        /// Draw the menu and all of it's components.
        /// </summary>
        internal override async void Draw()
        {
            if (!Visible || Main.Warning.IsShowing) return;
            while (!Main.scaleformUI.IsLoaded) await BaseScript.Delay(0);

            HideHudComponentThisFrame(19);

            Controls.Toggle(!ControlDisablingEnabled);

            Main.scaleformUI.Render2D();

            if (Glare)
            {
                _menuGlare.CallFunction("SET_DATA_SLOT", GameplayCamera.RelativeHeading);
                SizeF _glareSize = new SizeF(1.0f, 1f);
                PointF gl = new PointF((Offset.X / Screen.Width) + 0.4499f, (Offset.Y / Screen.Height) + 0.449f);

                DrawScaleformMovie(_menuGlare.Handle, gl.X, gl.Y, _glareSize.Width, _glareSize.Height, 255, 255, 255, 255, 0);
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
        }

        bool eventBool = false;
        int eventType = 0;
        int itemId = 0;
        int context = 0;
        int unused = 0;
        bool cursorPressed;
        private ItemFont descriptionFont = ScaleformFonts.CHALET_LONDON_NINETEENSIXTY;
        private ScrollingType scrollingType = ScrollingType.CLASSIC;

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

            bool success = GetScaleformMovieCursorSelection(Main.scaleformUI.Handle, ref eventBool, ref eventType, ref context, ref itemId);

            if (success && !isBuilding)
            {
                switch (eventType)
                {
                    case 5: // on click
                        switch (context)
                        {
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
                                                    BeginScaleformMovieMethod(Main.scaleformUI.Handle, "SELECT_ITEM");
                                                    ScaleformMovieMethodAddParamInt(Pagination.GetScaleformIndex(CurrentSelection));
                                                    int ret = EndScaleformMovieMethodReturnValue();
                                                    while (!IsScaleformMovieMethodReturnValueReady(ret)) await BaseScript.Delay(0);
                                                    int value = GetScaleformMovieMethodReturnValueInt(ret);
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
                                    Main.scaleformUI.CallFunction("SET_COUNTER_QTTY", CurrentSelection + 1, MenuItems.Count);
                                    Game.PlaySound(AUDIO_SELECT, AUDIO_LIBRARY);
                                }
                                break;
                            case 10: // panels (10 => context 1, panel_type 0) // ColorPanel
                                {
                                    string res = await Main.scaleformUI.CallFunctionReturnValueString("SELECT_PANEL", Pagination.GetScaleformIndex(CurrentSelection));
                                    string[] split = res.Split(',');
                                    UIMenuColorPanel panel = (UIMenuColorPanel)MenuItems[CurrentSelection].Panels[Convert.ToInt32(split[0])];
                                    panel._value = Convert.ToInt32(split[1]);
                                    ColorPanelChange(panel.ParentItem, panel, panel.CurrentSelection);
                                    panel.PanelChanged();
                                }
                                break;
                            case 11: // panels (11 => context 1, panel_type 1) // PercentagePanel
                                cursorPressed = true;
                                break;
                            case 12: // panels (12 => context 1, panel_type 2) // GridPanel
                                cursorPressed = true;
                                break;
                            case 2: // side panel
                                {
                                    UIVehicleColourPickerPanel panel = (UIVehicleColourPickerPanel)MenuItems[CurrentSelection].SidePanel;
                                    if (itemId != -1)
                                    {
                                        panel._value = itemId;
                                        panel.PickerSelect();
                                        Game.PlaySound(AUDIO_SELECT, AUDIO_LIBRARY);
                                    }
                                }
                                break;
                        }
                        break;
                    case 6: // on click released
                        cursorPressed = false;
                        break;
                    case 7: // on click released ouside
                        cursorPressed = false;
                        SetMouseCursorSprite(1);
                        break;
                    case 8: // on not hover
                        cursorPressed = false;
                        if (context == 0)
                        {
                            MenuItems[itemId].Hovered = false;
                        }
                        SetMouseCursorSprite(1);
                        break;
                    case 9: // on hovered
                        if (context == 0)
                        {
                            MenuItems[itemId].Hovered = true;
                        }
                        SetMouseCursorSprite(5);
                        break;
                    case 0: // dragged outside
                        cursorPressed = false;
                        break;
                    case 1: // dragged inside
                        cursorPressed = true;
                        break;
                }
            }

            if (cursorPressed)
            {
                if (HasSoundFinished(menuSound))
                {
                    menuSound = GetSoundId();
                    PlaySoundFrontend(menuSound, "CONTINUOUS_SLIDER", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                }

                BeginScaleformMovieMethod(Main.scaleformUI.Handle, "SET_INPUT_MOUSE_EVENT_CONTINUE");
                int ret = EndScaleformMovieMethodReturnValue();
                while (!IsScaleformMovieMethodReturnValueReady(ret)) await BaseScript.Delay(0);
                string res = GetScaleformMovieMethodReturnValueString(ret);
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

            if (ScreenTools.IsMouseInBounds(new PointF(0, 0), new SizeF(30, 1080)) && MouseEdgeEnabled)
            {
                GameplayCamera.RelativeHeading += 5f;
                SetCursorSprite(6);
            }
            else if (ScreenTools.IsMouseInBounds(new PointF(Convert.ToInt32(Resolution.Width - 30f), 0), new SizeF(30, 1080)) && MouseEdgeEnabled)
            {
                GameplayCamera.RelativeHeading -= 5f;
                SetCursorSprite(7);
            }
            else if (MouseEdgeEnabled && !MenuItems.Any(x => x.Hovered))
            {
                SetCursorSprite(1);
            }
        }

        public async void GoBack(bool playSound = true)
        {
            if (CanPlayerCloseMenu)
            {
                if (playSound)
                    Game.PlaySound(AUDIO_BACK, AUDIO_LIBRARY);
                await FadeOutMenu();
                if (BreadcrumbsHandler.CurrentDepth == 0)
                {
                    Visible = false;
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
                        prevMenu.Visible = true;
                    BreadcrumbsHandler.SwitchInProgress = false;
                }
            }
        }

        public async void GoUp()
        {
            try
            {
                if (isBuilding) return;
                MenuItems[CurrentSelection].Selected = false;
                do
                {
                    await BaseScript.Delay(0);
                    bool overflow = CurrentSelection == 0 && Pagination.TotalPages > 1;
                    if (Pagination.GoUp())
                    {
                        if (scrollingType == ScrollingType.ENDLESS || (scrollingType == ScrollingType.CLASSIC && !overflow))
                        {
                            _itemCreation(Pagination.GetPage(CurrentSelection), Pagination.CurrentPageIndex, true);
                            Main.scaleformUI.CallFunction("SET_INPUT_EVENT", 8, delay);
                        }
                        else if (scrollingType == ScrollingType.PAGINATED || (scrollingType == ScrollingType.CLASSIC && overflow))
                        {
                            isBuilding = true;
                            await FadeOutItems();
                            isFading = true;
                            Main.scaleformUI.CallFunction("CLEAR_ITEMS");
                            int i = 0;
                            int max = Pagination.ItemsPerPage;
                            while (i < max)
                            {
                                await BaseScript.Delay(0);
                                if (!Visible) return;
                                _itemCreation(Pagination.CurrentPage, i, false, true);
                                i++;
                            }
                            isBuilding = false;
                        }
                    }
                }
                while (MenuItems[CurrentSelection] is UIMenuSeparatorItem sp && sp.Jumpable);
                Main.scaleformUI.CallFunction("SET_CURRENT_ITEM", Pagination.ScaleformIndex);
                Main.scaleformUI.CallFunction("SET_COUNTER_QTTY", CurrentSelection + 1, MenuItems.Count);
                MenuItems[CurrentSelection].Selected = true;
                if (isFading)
                    await FadeInItems();
                IndexChange(CurrentSelection);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
        public async void GoDown()
        {
            try
            {
                if (isBuilding) return;
                MenuItems[CurrentSelection].Selected = false;
                do
                {
                    await BaseScript.Delay(0);
                    bool overflow = CurrentSelection == MenuItems.Count - 1 && Pagination.TotalPages > 1;
                    if (Pagination.GoDown())
                    {
                        if (scrollingType == ScrollingType.ENDLESS || (scrollingType == ScrollingType.CLASSIC && !overflow))
                        {
                            _itemCreation(Pagination.GetPage(CurrentSelection), Pagination.CurrentPageIndex, false);
                            Main.scaleformUI.CallFunction("SET_INPUT_EVENT", 9, delay);
                        }
                        else if (scrollingType == ScrollingType.PAGINATED || (scrollingType == ScrollingType.CLASSIC && overflow))
                        {
                            isBuilding = true;
                            await FadeOutItems();
                            isFading = true;
                            Main.scaleformUI.CallFunction("CLEAR_ITEMS");
                            int i = 0;
                            int max = Pagination.ItemsPerPage;
                            while (i < max)
                            {
                                await BaseScript.Delay(0);
                                if (!Visible) return;
                                _itemCreation(Pagination.CurrentPage, i, false);
                                i++;
                            }
                            isBuilding = false;
                        }
                    }
                }
                while (MenuItems[CurrentSelection] is UIMenuSeparatorItem sp && sp.Jumpable);
                Main.scaleformUI.CallFunction("SET_CURRENT_ITEM", Pagination.ScaleformIndex);
                Main.scaleformUI.CallFunction("SET_COUNTER_QTTY", CurrentSelection + 1, MenuItems.Count);
                MenuItems[CurrentSelection].Selected = true;
                if (isFading)
                    await FadeInItems();
                IndexChange(CurrentSelection);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        public async void GoLeft()
        {
            if (!MenuItems[CurrentSelection].Enabled)
            {
                Game.PlaySound(AUDIO_ERROR, AUDIO_LIBRARY);
                return;
            }
            BeginScaleformMovieMethod(Main.scaleformUI.Handle, "SET_INPUT_EVENT");
            ScaleformMovieMethodAddParamInt(10);
            int ret = EndScaleformMovieMethodReturnValue();
            while (!IsScaleformMovieMethodReturnValueReady(ret)) await BaseScript.Delay(0);
            int res = GetScaleformMovieFunctionReturnInt(ret);
            switch (MenuItems[CurrentSelection])
            {
                case UIMenuListItem:
                    {
                        UIMenuListItem it = (UIMenuListItem)MenuItems[CurrentSelection];
                        it.Index = res;
                        ListChange(it, it.Index);
                        it.ListChangedTrigger(it.Index);
                        break;
                    }
                case UIMenuDynamicListItem:
                    {
                        UIMenuDynamicListItem it = (UIMenuDynamicListItem)MenuItems[CurrentSelection];
                        string newItem = await it.Callback(it, UIMenuDynamicListItem.ChangeDirection.Left);
                        it.CurrentListItem = newItem;
                        break;
                    }
                case UIMenuSliderItem:
                    {
                        UIMenuSliderItem it = (UIMenuSliderItem)MenuItems[CurrentSelection];
                        it.Value = res;
                        SliderChange(it, it.Value);
                        break;
                    }
                case UIMenuProgressItem:
                    {
                        UIMenuProgressItem it = (UIMenuProgressItem)MenuItems[CurrentSelection];
                        it.Value = res;
                        ProgressChange(it, it.Value);
                        break;
                    }
                case UIMenuStatsItem:
                    {
                        UIMenuStatsItem it = (UIMenuStatsItem)MenuItems[CurrentSelection];
                        it.Value = res;
                        StatItemChange(it, it.Value);
                        break;
                    }
            }
        }

        public async void GoRight()
        {
            if (!MenuItems[CurrentSelection].Enabled)
            {
                Game.PlaySound(AUDIO_ERROR, AUDIO_LIBRARY);
                return;
            }
            BeginScaleformMovieMethod(Main.scaleformUI.Handle, "SET_INPUT_EVENT");
            ScaleformMovieMethodAddParamInt(11);
            int ret = EndScaleformMovieMethodReturnValue();
            while (!IsScaleformMovieMethodReturnValueReady(ret)) await BaseScript.Delay(0);
            int res = GetScaleformMovieFunctionReturnInt(ret);
            switch (MenuItems[CurrentSelection])
            {
                case UIMenuListItem:
                    {
                        UIMenuListItem it = (UIMenuListItem)MenuItems[CurrentSelection];
                        it.Index = res;
                        ListChange(it, it.Index);
                        it.ListChangedTrigger(it.Index);
                        break;
                    }
                case UIMenuDynamicListItem:
                    {
                        UIMenuDynamicListItem it = (UIMenuDynamicListItem)MenuItems[CurrentSelection];
                        string newItem = await it.Callback(it, UIMenuDynamicListItem.ChangeDirection.Right);
                        it.CurrentListItem = newItem;
                        break;
                    }
                case UIMenuSliderItem:
                    {
                        UIMenuSliderItem it = (UIMenuSliderItem)MenuItems[CurrentSelection];
                        it.Value = res;
                        SliderChange(it, it.Value);
                        break;
                    }
                case UIMenuProgressItem:
                    {
                        UIMenuProgressItem it = (UIMenuProgressItem)MenuItems[CurrentSelection];
                        it.Value = res;
                        ProgressChange(it, it.Value);
                        break;
                    }
                case UIMenuStatsItem:
                    {
                        UIMenuStatsItem it = (UIMenuStatsItem)MenuItems[CurrentSelection];
                        it.Value = res;
                        StatItemChange(it, it.Value);
                        break;
                    }
            }
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
        internal override void ProcessControl(Keys key = Keys.None)
        {
            if (!Main.scaleformUI.IsLoaded) return;
            if (!Visible || Main.Warning.IsShowing) return;
            if (_justOpened)
            {
                _justOpened = false;
                return;
            }

            if (UpdateOnscreenKeyboard() == 0 || IsWarningMessageActive() || BreadcrumbsHandler.SwitchInProgress || isFading) return;

            if (HasControlJustBeenReleased(MenuControls.Back, key))
            {
                GoBack();
            }

            if (isBuilding || MenuItems.Count == 0)
            {
                return;
            }

            if (HasControlJustBeenPressed(MenuControls.Up, key))
            {
                GoUp();
                timeBeforeOverflow = Main.GameTime;
            }
            else if (IsControlBeingPressed(MenuControls.Up, key) && Main.GameTime - timeBeforeOverflow > delayBeforeOverflow)
            {
                if (Main.GameTime - time > delay)
                {
                    ButtonDelay();
                    GoUp();
                }
            }

            if (HasControlJustBeenPressed(MenuControls.Down, key))
            {
                GoDown();
                timeBeforeOverflow = Main.GameTime;
            }
            else if (IsControlBeingPressed(MenuControls.Down, key) && Main.GameTime - timeBeforeOverflow > delayBeforeOverflow)
            {
                if (Main.GameTime - time > delay)
                {
                    ButtonDelay();
                    GoDown();
                }
            }

            if (HasControlJustBeenPressed(MenuControls.Left, key))
            {
                GoLeft();
                timeBeforeOverflow = Main.GameTime;
            }
            else if (IsControlBeingPressed(MenuControls.Left, key) && Main.GameTime - timeBeforeOverflow > delayBeforeOverflow)
            {
                if (Main.GameTime - time > delay)
                {
                    ButtonDelay();
                    GoLeft();
                }
            }

            if (HasControlJustBeenPressed(MenuControls.Right, key))
            {
                GoRight();
                timeBeforeOverflow = Main.GameTime;
            }
            else if (IsControlBeingPressed(MenuControls.Right, key) && Main.GameTime - timeBeforeOverflow > delayBeforeOverflow)
            {
                if (Main.GameTime - time > delay)
                {
                    ButtonDelay();
                    GoRight();
                }
            }

            if (HasControlJustBeenPressed(MenuControls.Select, key))
            {
                Select(true);
            }

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

        /// <summary>
        /// Process keystroke. Call this in the OnKeyDown event.
        /// </summary>
        /// <param name="key"></param>
        public void ProcessKey(Keys key)
        {
            if ((from MenuControls menuControl in _menuControls
                 select new List<Keys>(_keyDictionary[menuControl].Item1))
                .Any(tmpKeys => tmpKeys.Any(k => k == key)))
            {
                ProcessControl(key);
            }
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
                _visible = value;
                _justOpened = value;
                _itemsDirty = value;
                if (value)
                {
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
                }
                else
                {
                    Main.InstructionalButtons.ClearButtonList();
                    canBuild = false;
                    MenuCloseEv(this);
                    MenuHandler.ableToDraw = false;
                    MenuHandler.currentMenu = null;
                    _unfilteredMenuItems.Clear();
                    Main.scaleformUI.CallFunction("CLEAR_ALL");
                }
                if (!value) return;
                if (!ResetCursorOnOpen) return;
                SetCursorLocation(0.5f, 0.5f);
                Screen.Hud.CursorSprite = CursorSprite.Normal;
            }
        }

        internal async void BuildUpMenuAsync(bool itemsOnly = false)
        {
            isBuilding = true;
            bool _animEnabled = EnableAnimation;
            if (itemless)
            {
                EnableAnimation = false;
                while (!Main.scaleformUI.IsLoaded) await BaseScript.Delay(0);
                //Main.scaleformUI.CallFunction("CREATE_MENU", Title, Subtitle, Offset.X, Offset.Y, AlternativeTitle, _customTexture.Key, _customTexture.Value, MaxItemsOnScreen, MenuItems.Count, EnableAnimation, (int)AnimationType, (int)buildingAnimation, counterColor, descriptionFont.FontName, descriptionFont.FontID, fadingTime, true);
                BeginScaleformMovieMethod(Main.scaleformUI.Handle, "CREATE_MENU");
                PushScaleformMovieMethodParameterString(Title);
                PushScaleformMovieMethodParameterString(Subtitle);
                PushScaleformMovieMethodParameterFloat(Offset.X);
                PushScaleformMovieMethodParameterFloat(Offset.Y);
                PushScaleformMovieMethodParameterBool(AlternativeTitle);
                PushScaleformMovieMethodParameterString(_customTexture.Key);
                PushScaleformMovieMethodParameterString(_customTexture.Value);
                PushScaleformMovieFunctionParameterInt(MaxItemsOnScreen);
                PushScaleformMovieFunctionParameterInt(MenuItems.Count);
                PushScaleformMovieFunctionParameterBool(EnableAnimation);
                PushScaleformMovieFunctionParameterInt((int)AnimationType);
                PushScaleformMovieFunctionParameterInt((int)buildingAnimation);
                PushScaleformMovieFunctionParameterInt(counterColor.ArgbValue);
                PushScaleformMovieMethodParameterString(descriptionFont.FontName);
                PushScaleformMovieFunctionParameterInt(descriptionFont.FontID);
                PushScaleformMovieMethodParameterFloat(fadingTime);
                PushScaleformMovieFunctionParameterBool(true);
                BeginTextCommandScaleformString("ScaleformUILongDesc");
                EndTextCommandScaleformString_2();
                EndScaleformMovieMethod();
                await FadeInMenu();
                isBuilding = false;
                return;
            }
            if (!itemsOnly)
            {
                EnableAnimation = false;
                while (!Main.scaleformUI.IsLoaded) await BaseScript.Delay(0);
                Main.scaleformUI.CallFunction("CREATE_MENU", Title, Subtitle, Offset.X, Offset.Y, AlternativeTitle, _customTexture.Key, _customTexture.Value, MaxItemsOnScreen, MenuItems.Count, EnableAnimation, (int)AnimationType, (int)buildingAnimation, counterColor, descriptionFont.FontName, descriptionFont.FontID, fadingTime, false);
                if (Windows.Count > 0)
                {
                    foreach (UIMenuWindow wind in Windows)
                    {
                        switch (wind)
                        {
                            case UIMenuHeritageWindow:
                                UIMenuHeritageWindow her = (UIMenuHeritageWindow)wind;
                                Main.scaleformUI.CallFunction("ADD_WINDOW", her.id, her.Mom, her.Dad);
                                break;
                            case UIMenuDetailsWindow:
                                UIMenuDetailsWindow det = (UIMenuDetailsWindow)wind;
                                Main.scaleformUI.CallFunction("ADD_WINDOW", det.id, det.DetailBottom, det.DetailMid, det.DetailTop, det.DetailLeft.Txd, det.DetailLeft.Txn, det.DetailLeft.Pos.X, det.DetailLeft.Pos.Y, det.DetailLeft.Size.Width, det.DetailLeft.Size.Height);
                                if (det.StatWheelEnabled)
                                {
                                    foreach (UIDetailStat stat in det.DetailStats)
                                        Main.scaleformUI.CallFunction("ADD_STATS_DETAILS_WINDOW_STATWHEEL", Windows.IndexOf(det), stat.Percentage, stat.HudColor);
                                }
                                break;
                        }

                    }
                }
                int timer = Main.GameTime;

                if (MenuItems.Count == 0)
                {
                    while (MenuItems.Count == 0)
                    {
                        await BaseScript.Delay(0);
                        if (Main.GameTime - timer > 150)
                        {
                            Main.scaleformUI.CallFunction("SET_CURRENT_ITEM", Pagination.GetPageIndexFromMenuIndex(CurrentSelection));
                            return;
                        }
                    }
                }
            }

            int i = 0;
            int max = Pagination.ItemsPerPage;
            if (MenuItems.Count < max)
                max = MenuItems.Count;
            Pagination.MinItem = Pagination.CurrentPageStartIndex;

            if (scrollingType == ScrollingType.CLASSIC && Pagination.TotalPages > 1)
            {
                int missingItems = Pagination.GetMissingItems();
                if (missingItems > 0)
                {
                    Pagination.ScaleformIndex = Pagination.GetPageIndexFromMenuIndex(Pagination.CurrentPageEndIndex) + missingItems;
                    Pagination.MinItem = Pagination.CurrentPageStartIndex - missingItems;
                }
            }


            Pagination.MaxItem = Pagination.CurrentPageEndIndex;
            while (i < max)
            {
                await BaseScript.Delay(0);
                if (!Visible) return;
                _itemCreation(Pagination.CurrentPage, i, false, true);
                i++;
            }

            Pagination.ScaleformIndex = Pagination.GetScaleformIndex(CurrentSelection);

            MenuItems[CurrentSelection].Selected = true;
            Main.scaleformUI.CallFunction("SET_CURRENT_ITEM", Pagination.ScaleformIndex);
            Main.scaleformUI.CallFunction("SET_COUNTER_QTTY", CurrentSelection + 1, MenuItems.Count);

            if (MenuItems[CurrentSelection] is UIMenuSeparatorItem)
            {
                if ((MenuItems[CurrentSelection] as UIMenuSeparatorItem).Jumpable)
                {
                    GoDown();
                }
            }
            Main.scaleformUI.CallFunction("ENABLE_MOUSE", MouseControlsEnabled);
            Main.scaleformUI.CallFunction("ENABLE_3D_ANIMATIONS", enabled3DAnimations);
            EnableAnimation = _animEnabled;
            await FadeInMenu();
            isBuilding = false;
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
                SetKey(MenuControls.Select, Control.Attack);

            }
            else
            {
                ResetKey(MenuControls.Select);
                SetKey(MenuControls.Select, Control.FrontendAccept);
            }
        }

        /// <summary>
        /// Sorts menu items based on the desired predicated
        /// </summary>
        /// <param name="compare"></param>
        public void SortMenuItems(Comparison<UIMenuItem> compare)
        {
            if (itemless) throw new("ScaleformUI - You can't compare or sort an itemless menu");
            MenuItems[CurrentSelection].Selected = false;
            _unfilteredMenuItems = MenuItems.ToList();
            Clear();
            List<UIMenuItem> list = _unfilteredMenuItems.ToList();
            list.Sort(compare);
            MenuItems = list.ToList();
            Pagination.TotalItems = MenuItems.Count;
            BuildUpMenuAsync(true);
        }

        /// <summary>
        /// Filters menu items based on the desired predicate
        /// </summary>
        /// <param name="predicate"></param>
        public void FilterMenuItems(Func<UIMenuItem, bool> predicate)
        {
            if (itemless) throw new("ScaleformUI - You can't compare or sort an itemless menu");
            MenuItems[CurrentSelection].Selected = false;
            _unfilteredMenuItems = MenuItems.ToList();
            Clear();
            MenuItems = _unfilteredMenuItems.Where(predicate.Invoke).ToList();
            Pagination.TotalItems = MenuItems.Count;
            BuildUpMenuAsync(true);
        }

        /// <summary>
        /// Resets filtering/ordering of items going back to the original order.
        /// </summary>
        public void ResetFilter()
        {
            if (itemless) throw new("ScaleformUI - You can't compare or sort an itemless menu");
            MenuItems[CurrentSelection].Selected = false;
            Clear();
            MenuItems = _unfilteredMenuItems.ToList();
            Pagination.TotalItems = MenuItems.Count;
            BuildUpMenuAsync(true);
        }


        private void _itemCreation(int page, int pageIndex, bool before, bool isOverflow = false)
        {
            if (itemless) throw new("ScaleformUI - You can't add items to an itemless menu");
            int menuIndex = Pagination.GetMenuIndexFromPageIndex(page, pageIndex);
            bool missing = false;
            if (!before)
            {
                if (Pagination.GetPageItemsCount(page) < Pagination.ItemsPerPage && Pagination.TotalPages > 1)
                {
                    if (scrollingType == ScrollingType.ENDLESS)
                    {
                        if (menuIndex > Pagination.TotalItems - 1)
                        {
                            menuIndex -= Pagination.TotalItems;
                            Pagination.MaxItem = menuIndex;
                            missing = true;
                        }
                    }
                    else if (scrollingType == ScrollingType.CLASSIC && isOverflow)
                    {
                        int missingItems = Pagination.ItemsPerPage - Pagination.GetPageItemsCount(page);
                        menuIndex -= missingItems;
                    }
                    else if (scrollingType == ScrollingType.PAGINATED)
                        if (menuIndex >= MenuItems.Count) return;
                }
            }

            int scaleformIndex = Pagination.GetScaleformIndex(menuIndex);
            if (scrollingType == ScrollingType.ENDLESS && missing)
            {
                scaleformIndex = menuIndex + (Pagination.GetScaleformIndex(Pagination.TotalItems));
            }

            UIMenuItem item = MenuItems[menuIndex];
            AddTextEntry($"menu_{BreadcrumbsHandler.CurrentDepth}_desc_{menuIndex}", item.Description);

            BeginScaleformMovieMethod(Main.scaleformUI.Handle, "ADD_ITEM");
            PushScaleformMovieFunctionParameterBool(before);
            PushScaleformMovieFunctionParameterInt(item._itemId);
            PushScaleformMovieFunctionParameterInt(menuIndex);
            PushScaleformMovieMethodParameterString(item._formatLeftLabel);
            if (item.DescriptionHash != 0 && string.IsNullOrWhiteSpace(item.Description))
            {
                BeginTextCommandScaleformString("STRTNM1");
                AddTextComponentSubstringTextLabelHashKey(item.DescriptionHash);
                EndTextCommandScaleformString_2();
            }
            else
            {
                BeginTextCommandScaleformString($"menu_{BreadcrumbsHandler.CurrentDepth}_desc_{menuIndex}");
                EndTextCommandScaleformString_2();
            }
            PushScaleformMovieFunctionParameterBool(item.Enabled);
            PushScaleformMovieFunctionParameterBool(item.BlinkDescription);
            switch (item)
            {
                case UIMenuDynamicListItem:
                    UIMenuDynamicListItem dit = (UIMenuDynamicListItem)item;
                    PushScaleformMovieMethodParameterString(dit.CurrentListItem);
                    PushScaleformMovieFunctionParameterInt(0);
                    PushScaleformMovieFunctionParameterInt(dit.MainColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(dit.HighlightColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(dit.TextColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(dit.HighlightedTextColor.ArgbValue);
                    EndScaleformMovieMethod();
                    break;
                case UIMenuListItem:
                    UIMenuListItem it = (UIMenuListItem)item;
                    AddTextEntry($"listitem_{menuIndex}_list", string.Join(",", it.Items));
                    BeginTextCommandScaleformString($"listitem_{menuIndex}_list");
                    EndTextCommandScaleformString();
                    PushScaleformMovieFunctionParameterInt(it.Index);
                    PushScaleformMovieFunctionParameterInt(it.MainColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(it.HighlightColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(it.TextColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(it.HighlightedTextColor.ArgbValue);
                    EndScaleformMovieMethod();
                    break;
                case UIMenuCheckboxItem:
                    UIMenuCheckboxItem check = (UIMenuCheckboxItem)item;
                    PushScaleformMovieFunctionParameterInt((int)check.Style);
                    PushScaleformMovieMethodParameterBool(check.Checked);
                    PushScaleformMovieFunctionParameterInt(check.MainColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(check.HighlightColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(check.TextColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(check.HighlightedTextColor.ArgbValue);
                    EndScaleformMovieMethod();
                    break;
                case UIMenuSliderItem:
                    UIMenuSliderItem prItem = (UIMenuSliderItem)item;
                    PushScaleformMovieFunctionParameterInt(prItem._max);
                    PushScaleformMovieFunctionParameterInt(prItem._multiplier);
                    PushScaleformMovieFunctionParameterInt(prItem.Value);
                    PushScaleformMovieFunctionParameterInt(prItem.MainColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(prItem.HighlightColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(prItem.TextColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(prItem.HighlightedTextColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(prItem.SliderColor.ArgbValue);
                    PushScaleformMovieFunctionParameterBool(prItem._heritage);
                    EndScaleformMovieMethod();
                    break;
                case UIMenuProgressItem:
                    UIMenuProgressItem slItem = (UIMenuProgressItem)item;
                    PushScaleformMovieFunctionParameterInt(slItem._max);
                    PushScaleformMovieFunctionParameterInt(slItem._multiplier);
                    PushScaleformMovieFunctionParameterInt(slItem.Value);
                    PushScaleformMovieFunctionParameterInt(slItem.MainColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(slItem.HighlightColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(slItem.TextColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(slItem.HighlightedTextColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(slItem.SliderColor.ArgbValue);
                    EndScaleformMovieMethod();
                    break;
                case UIMenuStatsItem:
                    UIMenuStatsItem statsItem = (UIMenuStatsItem)item;
                    PushScaleformMovieFunctionParameterInt(statsItem.Value);
                    PushScaleformMovieFunctionParameterInt(statsItem.Type);
                    PushScaleformMovieFunctionParameterInt(statsItem.Color.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(statsItem.MainColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(statsItem.HighlightColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(statsItem.TextColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(statsItem.HighlightedTextColor.ArgbValue);
                    EndScaleformMovieMethod();
                    break;
                case UIMenuSeparatorItem:
                    UIMenuSeparatorItem separatorItem = (UIMenuSeparatorItem)item;
                    PushScaleformMovieFunctionParameterBool(separatorItem.Jumpable);
                    PushScaleformMovieFunctionParameterInt(item.MainColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(item.HighlightColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(item.TextColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(item.HighlightedTextColor.ArgbValue);
                    EndScaleformMovieMethod();
                    break;
                default:
                    PushScaleformMovieFunctionParameterInt(item.MainColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(item.HighlightColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(item.TextColor.ArgbValue);
                    PushScaleformMovieFunctionParameterInt(item.HighlightedTextColor.ArgbValue);
                    EndScaleformMovieMethod();
                    Main.scaleformUI.CallFunction("SET_RIGHT_LABEL", scaleformIndex, item._formatRightLabel);
                    if (item.RightBadge != BadgeIcon.NONE)
                        Main.scaleformUI.CallFunction("SET_RIGHT_BADGE", scaleformIndex, (int)item.RightBadge);
                    break;
            }
            Main.scaleformUI.CallFunction("SET_ITEM_LABEL_FONT", scaleformIndex, item.labelFont.FontName, item.labelFont.FontID);
            Main.scaleformUI.CallFunction("SET_ITEM_RIGHT_LABEL_FONT", scaleformIndex, item.rightLabelFont.FontName, item.rightLabelFont.FontID);
            if (item.LeftBadge != BadgeIcon.NONE)
                Main.scaleformUI.CallFunction("SET_LEFT_BADGE", scaleformIndex, (int)item.LeftBadge);
            if (item.SidePanel != null)
            {
                switch (item.SidePanel)
                {
                    case UIMissionDetailsPanel:
                        UIMissionDetailsPanel mis = (UIMissionDetailsPanel)item.SidePanel;
                        Main.scaleformUI.CallFunction("ADD_SIDE_PANEL_TO_ITEM", scaleformIndex, 0, (int)mis.PanelSide, (int)mis._titleType, mis.Title, mis.TitleColor, mis.TextureDict, mis.TextureName);
                        foreach (UIFreemodeDetailsItem _it in mis.Items)
                        {
                            Main.scaleformUI.CallFunction("ADD_MISSION_DETAILS_DESC_ITEM", scaleformIndex, _it.Type, _it.TextLeft, _it.TextRight, (int)_it.Icon, _it.IconColor, _it.Tick, _it._labelFont.FontName, _it._labelFont.FontID, _it._rightLabelFont.FontName, _it._rightLabelFont.FontID);
                        }
                        break;
                    case UIVehicleColourPickerPanel:
                        UIVehicleColourPickerPanel cp = (UIVehicleColourPickerPanel)item.SidePanel;
                        Main.scaleformUI.CallFunction("ADD_SIDE_PANEL_TO_ITEM", scaleformIndex, 1, (int)cp.PanelSide, (int)cp._titleType, cp.Title, cp.TitleColor);
                        break;
                }
            }
            if (item.Panels.Count == 0) return;

            foreach (UIMenuPanel panel in item.Panels)
            {
                int pan = item.Panels.IndexOf(panel);
                switch (panel)
                {
                    case UIMenuColorPanel:
                        UIMenuColorPanel cp = (UIMenuColorPanel)panel;
                        Main.scaleformUI.CallFunction("ADD_PANEL", scaleformIndex, 0, cp.Title, (int)cp.ColorPanelColorType, cp.CurrentSelection, cp.CustomColors is not null ? string.Join(",", cp.CustomColors.Select(x => x.ArgbValue)) : "");
                        break;
                    case UIMenuPercentagePanel:
                        UIMenuPercentagePanel pp = (UIMenuPercentagePanel)panel;
                        Main.scaleformUI.CallFunction("ADD_PANEL", scaleformIndex, 1, pp.Title, pp.Min, pp.Max, pp.Percentage);
                        break;
                    case UIMenuGridPanel:
                        UIMenuGridPanel gp = (UIMenuGridPanel)panel;
                        Main.scaleformUI.CallFunction("ADD_PANEL", scaleformIndex, 2, gp.TopLabel, gp.RightLabel, gp.LeftLabel, gp.BottomLabel, gp.CirclePosition.X, gp.CirclePosition.Y, true, (int)gp.GridType);
                        break;
                    case UIMenuStatisticsPanel:
                        UIMenuStatisticsPanel sp = (UIMenuStatisticsPanel)panel;
                        Main.scaleformUI.CallFunction("ADD_PANEL", scaleformIndex, 3);
                        if (sp.Items.Count > 0)
                        {
                            foreach (StatisticsForPanel stat in sp.Items)
                            {
                                Main.scaleformUI.CallFunction("ADD_STATISTIC_TO_PANEL", scaleformIndex, pan, stat.Text, stat.Value);
                            }
                        }
                        break;
                }
            }
        }


        /// <summary>
        /// Returns the current selected item's index.
        /// Change the current selected item to index. Use this after you add or remove items dynamically.
        /// </summary>
        public int CurrentSelection
        {
            get { return MenuItems.Count == 0 ? 0 : Pagination.CurrentMenuIndex; }
            set
            {
                if (value < 0)
                {
                    Pagination.CurrentMenuIndex = 0;
                }
                else if (value >= MenuItems.Count)
                {
                    Pagination.CurrentMenuIndex = MenuItems.Count - 1;
                }
                MenuItems[CurrentSelection].Selected = false;

                Pagination.CurrentMenuIndex = value;
                Pagination.CurrentPage = Pagination.GetPage(Pagination.CurrentMenuIndex);
                Pagination.CurrentPageIndex = value;
                Pagination.ScaleformIndex = Pagination.GetScaleformIndex(value);

                if (_visible)
                    Main.scaleformUI.CallFunction("SET_CURRENT_ITEM", Pagination.GetScaleformIndex(Pagination.CurrentMenuIndex));

                MenuItems[CurrentSelection].Selected = true;
                //ScaleformUI._ui.CallFunction("SET_CURRENT_ITEM", _activeItem);
            }
        }

        public UIMenuItem CurrentItem
        {
            get => MenuItems[CurrentSelection];
            set => CurrentSelection = MenuItems.Any(x => x.Label == value.Label && x.Description == value.Description) ? MenuItems.IndexOf(value) : 0;
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
                    Main.scaleformUI.CallFunction("UPDATE_TITLE_SUBTITLE", title, subtitle, AlternativeTitle);
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
                    Main.scaleformUI.CallFunction("UPDATE_TITLE_SUBTITLE", title, subtitle, AlternativeTitle);
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
                    Main.scaleformUI.CallFunction("SET_COUNTER_COLOR", counterColor);
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
                    Main.scaleformUI.CallFunction("ENABLE_MOUSE", value);
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
                    InstructionalButtons = new List<InstructionalButton>()
                    {
                        new InstructionalButton(Control.PhoneSelect, _selectTextLocalized),
                        new InstructionalButton(Control.PhoneCancel, _backTextLocalized)
                    };
                }
                else
                {
                    InstructionalButtons = new List<InstructionalButton>()
                    {
                        new InstructionalButton(Control.PhoneSelect, _selectTextLocalized),
                    };
                }
                if (Visible)
                {
                    Main.InstructionalButtons.SetInstructionalButtons(InstructionalButtons);
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
        }

        protected virtual void ColorPanelChange(UIMenuItem item, UIMenuColorPanel panel, int index)
        {
            OnColorPanelChange?.Invoke(item, panel, index);
        }
        protected virtual void PercentagePanelChange(UIMenuItem item, UIMenuPercentagePanel panel, float index)
        {
            OnPercentagePanelChange?.Invoke(item, panel, index);
        }
        protected virtual void GridPanelChange(UIMenuItem item, UIMenuGridPanel panel, PointF index)
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
            Back
        }

    }
}

