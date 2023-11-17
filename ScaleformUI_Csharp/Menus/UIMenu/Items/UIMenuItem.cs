using static CitizenFX.FiveM.Native.Natives;
using ScaleformUI.Elements;
using ScaleformUI.LobbyMenu;
using ScaleformUI.Menus;
using ScaleformUI.PauseMenu;

namespace ScaleformUI.Menu
{
    public enum BadgeIcon
    {
        NONE,
        LOCK,
        STAR,
        WARNING,
        CROWN,
        MEDAL_BRONZE,
        MEDAL_GOLD,
        MEDAL_SILVER,
        CASH,
        COKE,
        HEROIN,
        METH,
        WEED,
        AMMO,
        ARMOR,
        BARBER,
        CLOTHING,
        FRANKLIN,
        BIKE,
        CAR,
        GUN,
        HEALTH_HEART,
        MAKEUP_BRUSH,
        MASK,
        MICHAEL,
        TATTOO,
        TICK,
        TREVOR,
        FEMALE,
        MALE,
        LOCK_ARENA,
        ADVERSARY,
        BASE_JUMPING,
        BRIEFCASE,
        MISSION_STAR,
        DEATHMATCH,
        CASTLE,
        TROPHY,
        RACE_FLAG,
        RACE_FLAG_PLANE,
        RACE_FLAG_BICYCLE,
        RACE_FLAG_PERSON,
        RACE_FLAG_CAR,
        RACE_FLAG_BOAT_ANCHOR,
        ROCKSTAR,
        STUNT,
        STUNT_PREMIUM,
        RACE_FLAG_STUNT_JUMP,
        SHIELD,
        TEAM_DEATHMATCH,
        VEHICLE_DEATHMATCH,
        MP_AMMO_PICKUP,
        MP_AMMO,
        MP_CASH,
        MP_RP,
        MP_SPECTATING,
        SALE,
        GLOBE_WHITE,
        GLOBE_RED,
        GLOBE_BLUE,
        GLOBE_YELLOW,
        GLOBE_GREEN,
        GLOBE_ORANGE,
        INV_ARM_WRESTLING,
        INV_BASEJUMP,
        INV_MISSION,
        INV_DARTS,
        INV_DEATHMATCH,
        INV_DRUG,
        INV_CASTLE,
        INV_GOLF,
        INV_BIKE,
        INV_BOAT,
        INV_ANCHOR,
        INV_CAR,
        INV_DOLLAR,
        INV_COKE,
        INV_KEY,
        INV_DATA,
        INV_HELI,
        INV_HEORIN,
        INV_KEYCARD,
        INV_METH,
        INV_BRIEFCASE,
        INV_LINK,
        INV_PERSON,
        INV_PLANE,
        INV_PLANE2,
        INV_QUESTIONMARK,
        INV_REMOTE,
        INV_SAFE,
        INV_STEER_WHEEL,
        INV_WEAPON,
        INV_WEED,
        INV_RACE_FLAG_PLANE,
        INV_RACE_FLAG_BICYCLE,
        INV_RACE_FLAG_BOAT_ANCHOR,
        INV_RACE_FLAG_PERSON,
        INV_RACE_FLAG_CAR,
        INV_RACE_FLAG_HELMET,
        INV_SHOOTING_RANGE,
        INV_SURVIVAL,
        INV_TEAM_DEATHMATCH,
        INV_TENNIS,
        INV_VEHICLE_DEATHMATCH,
        AUDIO_MUTE,
        AUDIO_INACTIVE,
        AUDIO_VOL1,
        AUDIO_VOL2,
        AUDIO_VOL3,
        COUNTRY_USA,
        COUNTRY_UK,
        COUNTRY_SWEDEN,
        COUNTRY_KOREA,
        COUNTRY_JAPAN,
        COUNTRY_ITALY,
        COUNTRY_GERMANY,
        COUNTRY_FRANCE,
        BRAND_ALBANY,
        BRAND_ANNIS,
        BRAND_BANSHEE,
        BRAND_BENEFACTOR,
        BRAND_BF,
        BRAND_BOLLOKAN,
        BRAND_BRAVADO,
        BRAND_BRUTE,
        BRAND_BUCKINGHAM,
        BRAND_CANIS,
        BRAND_CHARIOT,
        BRAND_CHEVAL,
        BRAND_CLASSIQUE,
        BRAND_COIL,
        BRAND_DECLASSE,
        BRAND_DEWBAUCHEE,
        BRAND_DILETTANTE,
        BRAND_DINKA,
        BRAND_DUNDREARY,
        BRAND_EMPORER,
        BRAND_ENUS,
        BRAND_FATHOM,
        BRAND_GALIVANTER,
        BRAND_GROTTI,
        BRAND_GROTTI2,
        BRAND_HIJAK,
        BRAND_HVY,
        BRAND_IMPONTE,
        BRAND_INVETERO,
        BRAND_JACKSHEEPE,
        BRAND_LCC,
        BRAND_JOBUILT,
        BRAND_KARIN,
        BRAND_LAMPADATI,
        BRAND_MAIBATSU,
        BRAND_MAMMOTH,
        BRAND_MTL,
        BRAND_NAGASAKI,
        BRAND_OBEY,
        BRAND_OCELOT,
        BRAND_OVERFLOD,
        BRAND_PED,
        BRAND_PEGASSI,
        BRAND_PFISTER,
        BRAND_PRINCIPE,
        BRAND_PROGEN,
        BRAND_PROGEN2,
        BRAND_RUNE,
        BRAND_SCHYSTER,
        BRAND_SHITZU,
        BRAND_SPEEDOPHILE,
        BRAND_STANLEY,
        BRAND_TRUFFADE,
        BRAND_UBERMACHT,
        BRAND_VAPID,
        BRAND_VULCAR,
        BRAND_WEENY,
        BRAND_WESTERN,
        BRAND_WESTERNMOTORCYCLE,
        BRAND_WILLARD,
        BRAND_ZIRCONIUM,
        INFO
    }
    /// <summary>
    /// Simple item with a label.
    /// </summary>
    public class UIMenuItem
    {
        internal int _itemId = 0;
        internal string _formatLeftLabel = "";
        internal string _formatRightLabel = "";
        private bool _selected = false;
        private string _label = "";
        private string _rightLabel = "";
        private bool _enabled;
        private bool blinkDescription;
        private SColor mainColor;
        private SColor highlightColor;
        private SColor textColor = SColor.HUD_White;
        private SColor highlightedTextColor = SColor.HUD_Black;
        private string description;
        private uint descriptionHash;
        internal ItemFont labelFont = ScaleformFonts.CHALET_LONDON_NINETEENSIXTY;
        internal ItemFont rightLabelFont = ScaleformFonts.CHALET_LONDON_NINETEENSIXTY;

        /// <summary>
        /// The item color when not highlighted
        /// </summary>
        public SColor MainColor
        {
            get => mainColor;
            set
            {
                mainColor = value;
                if (Parent != null && Parent.Visible && Parent.Pagination.IsItemVisible(Parent.MenuItems.IndexOf(this)))
                {
                    Main.scaleformUI.CallFunction("UPDATE_COLORS", Parent.Pagination.GetScaleformIndex(Parent.MenuItems.IndexOf(this)), value, highlightColor, textColor, highlightedTextColor);
                }
            }
        }
        /// <summary>
        /// The item color when highlighted
        /// </summary>
        public SColor HighlightColor
        {
            get => highlightColor;
            set
            {
                highlightColor = value;
                if (Parent != null && Parent.Visible && Parent.Pagination.IsItemVisible(Parent.MenuItems.IndexOf(this)))
                {
                    Main.scaleformUI.CallFunction("UPDATE_COLORS", Parent.Pagination.GetScaleformIndex(Parent.MenuItems.IndexOf(this)), mainColor, value, textColor, highlightedTextColor);
                }
            }
        }
        /// <summary>
        /// The item text color when not highlighted
        /// </summary>

        public SColor TextColor
        {
            get => textColor;
            set
            {
                textColor = value;
                if (Parent != null && Parent.Visible && Parent.Pagination.IsItemVisible(Parent.MenuItems.IndexOf(this)))
                {
                    Main.scaleformUI.CallFunction("UPDATE_COLORS", Parent.Pagination.GetScaleformIndex(Parent.MenuItems.IndexOf(this)), mainColor, highlightColor, value, highlightedTextColor);
                }
            }
        }
        /// <summary>
        /// The item text color when highlighted
        /// </summary>
        public SColor HighlightedTextColor
        {
            get => highlightedTextColor;
            set
            {
                highlightedTextColor = value;
                if (Parent != null && Parent.Visible && Parent.Pagination.IsItemVisible(Parent.MenuItems.IndexOf(this)))
                {
                    Main.scaleformUI.CallFunction("UPDATE_COLORS", Parent.Pagination.GetScaleformIndex(Parent.MenuItems.IndexOf(this)), mainColor, highlightColor, textColor, value);
                }
            }
        }

        public ItemFont LabelFont
        {
            get => labelFont;
            set
            {
                labelFont = value;
                if (Parent != null && Parent.Visible && Parent.Pagination.IsItemVisible(Parent.MenuItems.IndexOf(this)))
                {
                    Main.scaleformUI.CallFunction("SET_ITEM_LABEL_FONT", Parent.Pagination.GetScaleformIndex(Parent.MenuItems.IndexOf(this)), labelFont.FontName, labelFont.FontID);
                }
                if (ParentColumn != null && ParentColumn.Parent.Visible)
                {
                    if (ParentColumn.Parent is MainView lobby)
                        lobby._pause._lobby.CallFunction("SET_SETTINGS_ITEM_LABEL_FONT", ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), labelFont.FontName, labelFont.FontID);
                    else if (ParentColumn.Parent is TabView pause)
                        pause._pause._pause.CallFunction("SET_PLAYERS_TAB_SETTINGS_ITEM_LABEL_FONT", ParentColumn.ParentTab, ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), labelFont.FontName, labelFont.FontID);
                }
            }
        }

        public ItemFont RightLabelFont
        {
            get => rightLabelFont;
            set
            {
                rightLabelFont = value;
                if (Parent != null && Parent.Visible && Parent.Pagination.IsItemVisible(Parent.MenuItems.IndexOf(this)))
                {
                    Main.scaleformUI.CallFunction("SET_ITEM_RIGHT_LABEL_FONT", Parent.Pagination.GetScaleformIndex(Parent.MenuItems.IndexOf(this)), rightLabelFont.FontName, rightLabelFont.FontID);
                }
                if (ParentColumn != null && ParentColumn.Parent.Visible)
                {
                    if (ParentColumn.Parent is MainView lobby)
                        lobby._pause._lobby.CallFunction("SET_SETTINGS_ITEM_RIGHT_LABEL_FONT", ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), labelFont.FontName, labelFont.FontID);
                    else if (ParentColumn.Parent is TabView pause)
                        pause._pause._pause.CallFunction("SET_PLAYERS_TAB_SETTINGS_ITEM_RIGHT_LABEL_FONT", ParentColumn.ParentTab, ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), labelFont.FontName, labelFont.FontID);
                }
            }
        }

        public List<UIMenuPanel> Panels = new();

        public UIMenuSidePanel SidePanel { get; set; }


        // Allows you to attach data to a menu item if you want to identify the menu item without having to put identification info in the visible text or description.
        // Taken from MenuAPI (Thanks Tom).
        public dynamic ItemData { get; set; }

        /// <summary>
        /// Called when user selects the current item.
        /// </summary>
        public event ItemActivatedEvent Activated;

        /// <summary>
        /// Called when user "highlights" the current item.
        /// </summary>
        public event ItemHighlightedEvent Highlighted;

        /// <summary>
        /// Basic menu button.
        /// </summary>
        /// <param name="text">Button label.</param>
        public UIMenuItem(string text) : this(text, "") { }

        /// <summary>
        /// Basic menu button with description.
        /// </summary>
        /// <param name="text">Button label.</param>
        /// <param name="description">Description.</param>
        public UIMenuItem(string text, string description) : this(text, description, SColor.HUD_Panel_light, SColor.HUD_White, SColor.HUD_White, SColor.HUD_Black) { }

        /// <summary>
        /// Basic menu button with description.
        /// </summary>
        /// <param name="text">Button label.</param>
        /// <param name="descriptionHash">Description label hash.</param>
        public UIMenuItem(string text, uint descriptionHash) : this(text, descriptionHash, SColor.HUD_Panel_light, SColor.HUD_White, SColor.HUD_White, SColor.HUD_Black) { }

        public UIMenuItem(string text, string description, SColor mainColor, SColor highlightColor) : this(text, description, mainColor, highlightColor, SColor.HUD_White, SColor.HUD_Black) { }
        public UIMenuItem(string text, uint descriptionHash, SColor mainColor, SColor highlightColor) : this(text, descriptionHash, mainColor, highlightColor, SColor.HUD_White, SColor.HUD_Black) { }

        /// <summary>
        /// Basic menu item with description and colors.
        /// </summary>
        /// <param name="text">Item's label.</param>
        /// <param name="description">Item's description</param>
        /// <param name="color">Main Color</param>
        /// <param name="highlightColor">Highlighted Color</param>
        /// <param name="textColor">Text's main color</param>
        /// <param name="highlightedTextColor">Highlighted text color</param>
        public UIMenuItem(string text, string description, SColor color, SColor highlightColor, SColor textColor, SColor highlightedTextColor)
        {
            _enabled = true;
            MainColor = color;
            HighlightColor = highlightColor;
            TextColor = textColor;
            HighlightedTextColor = highlightedTextColor;
            Label = text;
            Description = description;
        }

        /// <summary>
        /// Basic menu item with description and colors.
        /// </summary>
        /// <param name="text">Item's label.</param>
        /// <param name="descriptionHash">Item's description label hash obtained with (uint)GetHashKey(label)</param>
        /// <param name="color">Main Color</param>
        /// <param name="highlightColor">Highlighted Color</param>
        /// <param name="textColor">Text's main color</param>
        /// <param name="highlightedTextColor">Highlighted text color</param>
        public UIMenuItem(string text, uint descriptionHash, SColor color, SColor highlightColor, SColor textColor, SColor highlightedTextColor)
        {
            _enabled = true;
            MainColor = color;
            HighlightColor = highlightColor;
            TextColor = textColor;
            HighlightedTextColor = highlightedTextColor;
            Label = text;
            this.description = string.Empty;
            DescriptionHash = descriptionHash;
        }

        /// <summary>
        /// Should the Info symbol blink?
        /// </summary>
        public bool BlinkDescription
        {
            get => blinkDescription;
            set
            {
                blinkDescription = value;
                if (Parent != null && Parent.Visible && Parent.Pagination.IsItemVisible(Parent.MenuItems.IndexOf(this)))
                {
                    Main.scaleformUI.CallFunction("SET_BLINK_DESC", Parent.Pagination.GetScaleformIndex(Parent.MenuItems.IndexOf(this)), blinkDescription);
                }
                if (ParentColumn != null && ParentColumn.Parent.Visible)
                {
                    if (ParentColumn.Parent is MainView lobby)
                        lobby._pause._lobby.CallFunction("UPDATE_SETTINGS_ITEM_BLINK_DESC", ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), blinkDescription);
                    else if (ParentColumn.Parent is TabView pause)
                        pause._pause._pause.CallFunction("UPDATE_PLAYERS_TAB_SETTINGS_ITEM_BLINK_DESC", ParentColumn.ParentTab, ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), blinkDescription);
                }
            }
        }

        /// <summary>
        /// Whether this item is currently selected.
        /// </summary>
        public virtual bool Selected
        {
            get => _selected;
            internal set
            {
                _selected = value;
                if (value)
                {
                    _formatLeftLabel = _formatLeftLabel.Replace("~w~", "~l~");
                    _formatLeftLabel = _formatLeftLabel.Replace("~s~", "~l~");
                    if (!string.IsNullOrWhiteSpace(_formatRightLabel))
                    {
                        _formatRightLabel = _formatRightLabel.Replace("~w~", "~l~");
                        _formatRightLabel = _formatRightLabel.Replace("~s~", "~l~");
                    }
                    Highlighted?.Invoke(Parent, this);
                }
                else
                {
                    _formatLeftLabel = _formatLeftLabel.Replace("~l~", "~s~");
                    if (!string.IsNullOrWhiteSpace(_formatRightLabel))
                    {
                        _formatRightLabel = _formatRightLabel.Replace("~l~", "~s~");
                    }
                }
                if (Parent != null && Parent.Pagination.IsItemVisible(Parent.MenuItems.IndexOf(this)))
                {
                    Main.scaleformUI.CallFunction("SET_ITEM_LABELS", Parent.Pagination.GetScaleformIndex(Parent.MenuItems.IndexOf(this)), _formatLeftLabel, _formatRightLabel);
                }
                if (ParentColumn != null && ParentColumn.Parent != null && ParentColumn.Parent.Visible)
                {
                    if (ParentColumn.Parent is MainView lobby)
                        lobby._pause._lobby.CallFunction("UPDATE_SETTINGS_ITEM_LABELS", ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), _formatLeftLabel, _formatRightLabel);
                    else if (ParentColumn.Parent is TabView pause)
                        pause._pause._pause.CallFunction("UPDATE_PLAYERS_TAB_SETTINGS_ITEM_LABELS", ParentColumn.ParentTab, ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), _formatLeftLabel, _formatRightLabel);
                }
            }
        }


        /// <summary>
        /// Whether this item is currently being hovered on with a mouse.
        /// </summary>
        public virtual bool Hovered { get; internal set; }

        /// <summary>
        /// This item's description.
        /// </summary>
        public virtual string Description
        {
            get => description;
            set
            {
                description = value;
                if (descriptionHash != 0) descriptionHash = 0;
                if (Parent != null && Parent.Visible && Parent.Pagination.IsItemVisible(Parent.MenuItems.IndexOf(this)))
                {
                    AddTextEntry($"menu_{BreadcrumbsHandler.CurrentDepth}_desc_{Parent.MenuItems.IndexOf(this)}", description);
                    BeginScaleformMovieMethod(Main.scaleformUI.Handle, "UPDATE_ITEM_DESCRIPTION");
                    ScaleformMovieMethodAddParamInt(Parent.Pagination.GetScaleformIndex(Parent.MenuItems.IndexOf(this)));
                    BeginTextCommandScaleformString($"menu_{BreadcrumbsHandler.CurrentDepth}_desc_{Parent.MenuItems.IndexOf(this)}");
                    EndTextCommandScaleformString_2();
                    EndScaleformMovieMethod();
                }
                if (ParentColumn != null && ParentColumn.Parent.Visible)
                {
                    if (ParentColumn.Parent is MainView lobby)
                    {
                        AddTextEntry($"lobbymenu_desc_{ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this))}", description);
                        BeginScaleformMovieMethod(lobby._pause._lobby.Handle, "UPDATE_SETTINGS_ITEM_DESCRIPTION");
                        ScaleformMovieMethodAddParamInt(ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)));
                        BeginTextCommandScaleformString($"lobbymenu_desc_{ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this))}");
                        EndTextCommandScaleformString_2();
                        EndScaleformMovieMethod();
                    }
                    else if (ParentColumn.Parent is TabView pause)
                    {
                        AddTextEntry($"pausemenu_{ParentColumn.ParentTab}_desc_{ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this))}", description);
                        BeginScaleformMovieMethod(pause._pause._pause.Handle, "UPDATE_PLAYERS_TAB_SETTINGS_ITEM_DESCRIPTION");
                        ScaleformMovieMethodAddParamInt(ParentColumn.ParentTab);
                        ScaleformMovieMethodAddParamInt(ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)));
                        BeginTextCommandScaleformString($"pausemenu_{ParentColumn.ParentTab}_desc_{ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this))}");
                        EndTextCommandScaleformString_2();
                        EndScaleformMovieMethod();
                    }
                }
            }
        }
        /// <summary>
        /// Sets the item's description by a label's hash (used by (uint)GetHashKey(label))
        /// </summary>
        public virtual uint DescriptionHash
        {
            get => descriptionHash;
            set
            {
                descriptionHash = value;
                if (!string.IsNullOrWhiteSpace(description))
                    description = string.Empty;
                if (Parent != null && Parent.Visible && Parent.Pagination.IsItemVisible(Parent.MenuItems.IndexOf(this)))
                {
                    BeginScaleformMovieMethod(Main.scaleformUI.Handle, "UPDATE_ITEM_DESCRIPTION");
                    ScaleformMovieMethodAddParamInt(Parent.Pagination.GetScaleformIndex(Parent.MenuItems.IndexOf(this)));
                    BeginTextCommandScaleformString("STRTNM1");
                    AddTextComponentSubstringTextLabelHashKey(descriptionHash);
                    EndTextCommandScaleformString_2();
                    EndScaleformMovieMethod();
                }
                if (ParentColumn != null && ParentColumn.Parent.Visible)
                {
                    if (ParentColumn.Parent is MainView lobby)
                    {
                        BeginScaleformMovieMethod(lobby._pause._lobby.Handle, "UPDATE_SETTINGS_ITEM_DESCRIPTION");
                        ScaleformMovieMethodAddParamInt(ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)));
                        BeginTextCommandScaleformString("STRTNM1");
                        AddTextComponentSubstringTextLabelHashKey(descriptionHash);
                        EndTextCommandScaleformString_2();
                        EndScaleformMovieMethod();
                    }
                    else if (ParentColumn.Parent is TabView pause)
                    {
                        BeginScaleformMovieMethod(pause._pause._pause.Handle, ""); // da aggiungere
                        ScaleformMovieMethodAddParamInt(ParentColumn.ParentTab);
                        ScaleformMovieMethodAddParamInt(ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)));
                        BeginTextCommandScaleformString("STRTNM1");
                        AddTextComponentSubstringTextLabelHashKey(descriptionHash);
                        EndTextCommandScaleformString_2();
                        EndScaleformMovieMethod();
                    }
                }
            }
        }


        /// <summary>
        /// Whether this item is enabled or disabled (text is greyed out and you cannot select it).
        /// </summary>
        public virtual bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                if (!value)
                    _formatLeftLabel = _formatLeftLabel.ReplaceRstarColorsWith("~c~");
                else
                    Label = _label;
                if (Parent != null && Parent.Visible && Parent.Pagination.IsItemVisible(Parent.MenuItems.IndexOf(this)))
                {
                    Main.scaleformUI.CallFunction("SET_ITEM_LABELS", Parent.Pagination.GetScaleformIndex(Parent.MenuItems.IndexOf(this)), _formatLeftLabel, _formatRightLabel);
                    Main.scaleformUI.CallFunction("ENABLE_ITEM", Parent.Pagination.GetScaleformIndex(Parent.MenuItems.IndexOf(this)), _enabled);
                }
                if (ParentColumn != null && ParentColumn.Parent.Visible && ParentColumn.Pagination.IsItemVisible(ParentColumn.Items.IndexOf(this)))
                {
                    if (ParentColumn.Parent is MainView lobby)
                    {
                        lobby._pause._lobby.CallFunction("UPDATE_SETTINGS_ITEM_LABELS", ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), _formatLeftLabel, _formatRightLabel);
                        lobby._pause._lobby.CallFunction("ENABLE_SETTINGS_ITEM", ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), _enabled);
                    }
                    else if (ParentColumn.Parent is TabView pause)
                    {
                        pause._pause._pause.CallFunction("UPDATE_PLAYERS_TAB_SETTINGS_ITEM_LABELS", ParentColumn.ParentTab, ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), _formatLeftLabel, _formatRightLabel);
                        pause._pause._pause.CallFunction("ENABLE_PLAYERS_TAB_SETTINGS_ITEM", ParentColumn.ParentTab, ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), _enabled);
                    }
                }
            }
        }

        internal virtual void ItemActivate(UIMenu sender)
        {
            Activated?.Invoke(sender, this);
        }



        /// <summary>
        /// Returns this item's label.
        /// </summary>
        public virtual string Label
        {
            get => _label;
            set
            {
                _label = value;
                _formatLeftLabel = value.StartsWith("~") ? value : "~s~" + value;
                if (_selected)
                {
                    _formatLeftLabel = _formatLeftLabel.Replace("~w~", "~l~");
                    _formatLeftLabel = _formatLeftLabel.Replace("~s~", "~l~");
                }
                else
                {
                    _formatLeftLabel = _formatLeftLabel.Replace("~l~", "~s~");
                }
                if (Parent != null && Parent.Visible && Parent.Pagination.IsItemVisible(Parent.MenuItems.IndexOf(this)))
                {
                    Main.scaleformUI.CallFunction("SET_LEFT_LABEL", Parent.Pagination.GetScaleformIndex(Parent.MenuItems.IndexOf(this)), _formatLeftLabel);
                }
                if (ParentColumn != null && ParentColumn.Parent.Visible)
                {
                    if (ParentColumn.Parent is MainView lobby)
                        lobby._pause._lobby.CallFunction("UPDATE_SETTINGS_ITEM_LABEL", ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), _formatLeftLabel);
                    else if (ParentColumn.Parent is TabView pause)
                        pause._pause._pause.CallFunction("UPDATE_PLAYERS_TAB_SETTINGS_ITEM_LABEL", ParentColumn.ParentTab, ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), _formatLeftLabel);
                }
            }
        }


        /// <summary>
        /// Set the left badge. Set it to None to remove the badge.
        /// </summary>
        /// <param name="badge"></param>
        public virtual void SetLeftBadge(BadgeIcon badge)
        {
            LeftBadge = badge;
            if (Parent != null && Parent.Visible && Parent.Pagination.IsItemVisible(Parent.MenuItems.IndexOf(this)))
            {
                Main.scaleformUI.CallFunction("SET_LEFT_BADGE", Parent.Pagination.GetScaleformIndex(Parent.MenuItems.IndexOf(this)), (int)badge);
            }
            if (ParentColumn != null && ParentColumn.Parent.Visible)
            {
                if (ParentColumn.Parent is MainView lobby)
                    lobby._pause._lobby.CallFunction("SET_SETTINGS_ITEM_LEFT_BADGE", ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), (int)badge);
                else if (ParentColumn.Parent is TabView pause)
                    pause._pause._pause.CallFunction("SET_PLAYERS_TAB_SETTINGS_ITEM_LEFT_BADGE", ParentColumn.ParentTab, ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), (int)badge);
            }
        }

        /// <summary>
        /// Set the right badge. Set it to None to remove the badge.
        /// </summary>
        /// <param name="badge"></param>
        public virtual void SetRightBadge(BadgeIcon badge)
        {
            RightBadge = badge;
            if (Parent != null && Parent.Visible && Parent.Pagination.IsItemVisible(Parent.MenuItems.IndexOf(this)))
            {
                Main.scaleformUI.CallFunction("SET_RIGHT_BADGE", Parent.Pagination.GetScaleformIndex(Parent.MenuItems.IndexOf(this)), (int)badge);
            }
            if (ParentColumn != null && ParentColumn.Parent.Visible)
            {
                if (ParentColumn.Parent is MainView lobby)
                    lobby._pause._lobby.CallFunction("SET_SETTINGS_ITEM_RIGHT_BADGE", ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), (int)badge);
                else if (ParentColumn.Parent is TabView pause)
                    pause._pause._pause.CallFunction("SET_PLAYERS_TAB_SETTINGS_ITEM_RIGHT_BADGE", ParentColumn.ParentTab, ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), (int)badge);
            }
        }


        /// <summary>
        /// Set the right label.
        /// </summary>
        /// <param name="text">Text as label. Set it to "" to remove the label.</param>
        public virtual void SetRightLabel(string text)
        {
            RightLabel = text;
        }

        /// <summary>
        /// Returns the current right label.
        /// </summary>
        public virtual string RightLabel
        {
            get => _rightLabel;
            private set
            {
                _rightLabel = value;
                _formatRightLabel = value.StartsWith("~") ? value : "~s~" + value;
                if (_selected)
                {
                    _formatRightLabel = _formatRightLabel.Replace("~w~", "~l~");
                    _formatRightLabel = _formatRightLabel.Replace("~s~", "~l~");
                }
                else
                {
                    _formatRightLabel = _formatRightLabel.Replace("~l~", "~s~");
                }
                if (Parent != null && Parent.Visible && Parent.Pagination.IsItemVisible(Parent.MenuItems.IndexOf(this)))
                {
                    Main.scaleformUI.CallFunction("SET_RIGHT_LABEL", Parent.Pagination.GetScaleformIndex(Parent.MenuItems.IndexOf(this)), _formatRightLabel);
                }
                if (ParentColumn != null && ParentColumn.Parent.Visible)
                {
                    if (ParentColumn.Parent is MainView lobby)
                        lobby._pause._lobby.CallFunction("UPDATE_SETTINGS_ITEM_LABEL_RIGHT", ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), _formatRightLabel);
                    else if (ParentColumn.Parent is TabView pause)
                        pause._pause._pause.CallFunction("UPDATE_PLAYERS_TAB_SETTINGS_ITEM_LABEL_RIGHT", ParentColumn.ParentTab, ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), _formatRightLabel);
                }
            }
        }

        /// <summary>
        /// Returns the current left badge.
        /// </summary>
        public virtual BadgeIcon LeftBadge { get; private set; } = BadgeIcon.NONE;

        /// <summary>
        /// Add a Panel to the UIMenuItem
        /// </summary>
        /// <param name="panel"></param>
        public virtual void AddPanel(UIMenuPanel panel)
        {
            Panels.Add(panel);
            panel.SetParentItem(this);
        }

        /// <summary>
        /// Removes a panel at a defined Index
        /// </summary>
        /// <param name="Index"></param>
        public virtual void RemovePanelAt(int Index)
        {
            Panels.RemoveAt(Index);
            if (Parent != null && Parent.Visible && Parent.Pagination.IsItemVisible(Parent.MenuItems.IndexOf(this)))
            {
                Main.scaleformUI.CallFunction("REMOVE_PANEL", Parent.Pagination.GetScaleformIndex(Parent.MenuItems.IndexOf(this)), Index);
            }
        }

        /// <summary>
        /// Adds a side panel to the menu binded to this item
        /// </summary>
        /// <param name="panel">the panel to add</param>
        public virtual void AddSidePanel(UIMenuSidePanel panel)
        {
            panel.SetParentItem(this);
            SidePanel = panel;
            if (Parent != null && Parent.Visible && Parent.Pagination.IsItemVisible(Parent.MenuItems.IndexOf(this)))
            {
                switch (panel)
                {
                    case UIMissionDetailsPanel:
                        UIMissionDetailsPanel mis = (UIMissionDetailsPanel)panel;
                        Main.scaleformUI.CallFunction("ADD_SIDE_PANEL_TO_ITEM", Parent.Pagination.GetScaleformIndex(Parent.MenuItems.IndexOf(this)), 0, (int)mis.PanelSide, (int)mis._titleType, mis.Title, mis.TitleColor, mis.TextureDict, mis.TextureName);
                        foreach (UIFreemodeDetailsItem _it in mis.Items)
                            Main.scaleformUI.CallFunction("ADD_MISSION_DETAILS_DESC_ITEM", Parent.Pagination.GetScaleformIndex(Parent.MenuItems.IndexOf(this)), _it.Type, _it.TextLeft, _it.TextRight, (int)_it.Icon, _it.IconColor, _it.Tick);
                        break;
                }
            }
        }

        /// <summary>
        /// Removes the side panel from this item
        /// </summary>
        public virtual void RemoveSidePanel()
        {
            SidePanel = null;
            if (Parent != null && Parent.Visible && Parent.Pagination.IsItemVisible(Parent.MenuItems.IndexOf(this)))
            {
                Main.scaleformUI.CallFunction("REMOVE_SIDE_PANEL_TO_ITEM", Parent.Pagination.GetScaleformIndex(Parent.MenuItems.IndexOf(this)));
            }
        }

        /// <summary>
        /// Returns the current right badge.
        /// </summary>
        public virtual BadgeIcon RightBadge { get; private set; }

        public static string GetSpriteDictionary(BadgeIcon icon)
        {
            switch (icon)
            {
                case BadgeIcon.MALE:
                case BadgeIcon.FEMALE:
                case BadgeIcon.AUDIO_MUTE:
                case BadgeIcon.AUDIO_INACTIVE:
                case BadgeIcon.AUDIO_VOL1:
                case BadgeIcon.AUDIO_VOL2:
                case BadgeIcon.AUDIO_VOL3:
                    return "mpleaderboard";
                case BadgeIcon.INV_ARM_WRESTLING:
                case BadgeIcon.INV_BASEJUMP:
                case BadgeIcon.INV_MISSION:
                case BadgeIcon.INV_DARTS:
                case BadgeIcon.INV_DEATHMATCH:
                case BadgeIcon.INV_DRUG:
                case BadgeIcon.INV_CASTLE:
                case BadgeIcon.INV_GOLF:
                case BadgeIcon.INV_BIKE:
                case BadgeIcon.INV_BOAT:
                case BadgeIcon.INV_ANCHOR:
                case BadgeIcon.INV_CAR:
                case BadgeIcon.INV_DOLLAR:
                case BadgeIcon.INV_COKE:
                case BadgeIcon.INV_KEY:
                case BadgeIcon.INV_DATA:
                case BadgeIcon.INV_HELI:
                case BadgeIcon.INV_HEORIN:
                case BadgeIcon.INV_KEYCARD:
                case BadgeIcon.INV_METH:
                case BadgeIcon.INV_BRIEFCASE:
                case BadgeIcon.INV_LINK:
                case BadgeIcon.INV_PERSON:
                case BadgeIcon.INV_PLANE:
                case BadgeIcon.INV_PLANE2:
                case BadgeIcon.INV_QUESTIONMARK:
                case BadgeIcon.INV_REMOTE:
                case BadgeIcon.INV_SAFE:
                case BadgeIcon.INV_STEER_WHEEL:
                case BadgeIcon.INV_WEAPON:
                case BadgeIcon.INV_WEED:
                case BadgeIcon.INV_RACE_FLAG_PLANE:
                case BadgeIcon.INV_RACE_FLAG_BICYCLE:
                case BadgeIcon.INV_RACE_FLAG_BOAT_ANCHOR:
                case BadgeIcon.INV_RACE_FLAG_PERSON:
                case BadgeIcon.INV_RACE_FLAG_CAR:
                case BadgeIcon.INV_RACE_FLAG_HELMET:
                case BadgeIcon.INV_SHOOTING_RANGE:
                case BadgeIcon.INV_SURVIVAL:
                case BadgeIcon.INV_TEAM_DEATHMATCH:
                case BadgeIcon.INV_TENNIS:
                case BadgeIcon.INV_VEHICLE_DEATHMATCH:
                    return "mpinventory";
                case BadgeIcon.ADVERSARY:
                case BadgeIcon.BASE_JUMPING:
                case BadgeIcon.BRIEFCASE:
                case BadgeIcon.MISSION_STAR:
                case BadgeIcon.DEATHMATCH:
                case BadgeIcon.CASTLE:
                case BadgeIcon.TROPHY:
                case BadgeIcon.RACE_FLAG:
                case BadgeIcon.RACE_FLAG_PLANE:
                case BadgeIcon.RACE_FLAG_BICYCLE:
                case BadgeIcon.RACE_FLAG_PERSON:
                case BadgeIcon.RACE_FLAG_CAR:
                case BadgeIcon.RACE_FLAG_BOAT_ANCHOR:
                case BadgeIcon.ROCKSTAR:
                case BadgeIcon.STUNT:
                case BadgeIcon.STUNT_PREMIUM:
                case BadgeIcon.RACE_FLAG_STUNT_JUMP:
                case BadgeIcon.SHIELD:
                case BadgeIcon.TEAM_DEATHMATCH:
                case BadgeIcon.VEHICLE_DEATHMATCH:
                    return "commonmenutu";
                case BadgeIcon.MP_AMMO_PICKUP:
                case BadgeIcon.MP_AMMO:
                case BadgeIcon.MP_CASH:
                case BadgeIcon.MP_RP:
                case BadgeIcon.MP_SPECTATING:
                    return "mphud";
                case BadgeIcon.SALE:
                    return "mpshopsale";
                case BadgeIcon.GLOBE_WHITE:
                case BadgeIcon.GLOBE_RED:
                case BadgeIcon.GLOBE_BLUE:
                case BadgeIcon.GLOBE_YELLOW:
                case BadgeIcon.GLOBE_GREEN:
                case BadgeIcon.GLOBE_ORANGE:
                    return "mprankbadge";
                case BadgeIcon.COUNTRY_USA:
                case BadgeIcon.COUNTRY_UK:
                case BadgeIcon.COUNTRY_SWEDEN:
                case BadgeIcon.COUNTRY_KOREA:
                case BadgeIcon.COUNTRY_JAPAN:
                case BadgeIcon.COUNTRY_ITALY:
                case BadgeIcon.COUNTRY_GERMANY:
                case BadgeIcon.COUNTRY_FRANCE:
                case BadgeIcon.BRAND_ALBANY:
                case BadgeIcon.BRAND_ANNIS:
                case BadgeIcon.BRAND_BANSHEE:
                case BadgeIcon.BRAND_BENEFACTOR:
                case BadgeIcon.BRAND_BF:
                case BadgeIcon.BRAND_BOLLOKAN:
                case BadgeIcon.BRAND_BRAVADO:
                case BadgeIcon.BRAND_BRUTE:
                case BadgeIcon.BRAND_BUCKINGHAM:
                case BadgeIcon.BRAND_CANIS:
                case BadgeIcon.BRAND_CHARIOT:
                case BadgeIcon.BRAND_CHEVAL:
                case BadgeIcon.BRAND_CLASSIQUE:
                case BadgeIcon.BRAND_COIL:
                case BadgeIcon.BRAND_DECLASSE:
                case BadgeIcon.BRAND_DEWBAUCHEE:
                case BadgeIcon.BRAND_DILETTANTE:
                case BadgeIcon.BRAND_DINKA:
                case BadgeIcon.BRAND_DUNDREARY:
                case BadgeIcon.BRAND_EMPORER:
                case BadgeIcon.BRAND_ENUS:
                case BadgeIcon.BRAND_FATHOM:
                case BadgeIcon.BRAND_GALIVANTER:
                case BadgeIcon.BRAND_GROTTI:
                case BadgeIcon.BRAND_HIJAK:
                case BadgeIcon.BRAND_HVY:
                case BadgeIcon.BRAND_IMPONTE:
                case BadgeIcon.BRAND_INVETERO:
                case BadgeIcon.BRAND_JACKSHEEPE:
                case BadgeIcon.BRAND_JOBUILT:
                case BadgeIcon.BRAND_KARIN:
                case BadgeIcon.BRAND_LAMPADATI:
                case BadgeIcon.BRAND_MAIBATSU:
                case BadgeIcon.BRAND_MAMMOTH:
                case BadgeIcon.BRAND_MTL:
                case BadgeIcon.BRAND_NAGASAKI:
                case BadgeIcon.BRAND_OBEY:
                case BadgeIcon.BRAND_OCELOT:
                case BadgeIcon.BRAND_OVERFLOD:
                case BadgeIcon.BRAND_PED:
                case BadgeIcon.BRAND_PEGASSI:
                case BadgeIcon.BRAND_PFISTER:
                case BadgeIcon.BRAND_PRINCIPE:
                case BadgeIcon.BRAND_PROGEN:
                case BadgeIcon.BRAND_SCHYSTER:
                case BadgeIcon.BRAND_SHITZU:
                case BadgeIcon.BRAND_SPEEDOPHILE:
                case BadgeIcon.BRAND_STANLEY:
                case BadgeIcon.BRAND_TRUFFADE:
                case BadgeIcon.BRAND_UBERMACHT:
                case BadgeIcon.BRAND_VAPID:
                case BadgeIcon.BRAND_VULCAR:
                case BadgeIcon.BRAND_WEENY:
                case BadgeIcon.BRAND_WESTERN:
                case BadgeIcon.BRAND_WESTERNMOTORCYCLE:
                case BadgeIcon.BRAND_WILLARD:
                case BadgeIcon.BRAND_ZIRCONIUM:
                    return "mpcarhud";
                case BadgeIcon.BRAND_GROTTI2:
                case BadgeIcon.BRAND_LCC:
                case BadgeIcon.BRAND_PROGEN2:
                case BadgeIcon.BRAND_RUNE:
                    return "mpcarhud2";
                case BadgeIcon.INFO:
                    return "shared";
                default:
                    return "commonmenu";
            }
        }

        /// <summary>
        /// Get the sprite name for the given icon depending on the selected state of the item.
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="selected"></param>
        /// <returns></returns>
        static string GetSpriteName(BadgeIcon icon, bool selected)
        {
            switch (icon)
            {

                case BadgeIcon.AMMO: return selected ? "shop_ammo_icon_b" : "shop_ammo_icon_a";
                case BadgeIcon.ARMOR: return selected ? "shop_armour_icon_b" : "shop_armour_icon_a";
                case BadgeIcon.BARBER: return selected ? "shop_barber_icon_b" : "shop_barber_icon_a";
                case BadgeIcon.BIKE: return selected ? "shop_garage_bike_icon_b" : "shop_garage_bike_icon_a";
                case BadgeIcon.CAR: return selected ? "shop_garage_icon_b" : "shop_garage_icon_a";
                case BadgeIcon.CASH: return "mp_specitem_cash";
                case BadgeIcon.CLOTHING: return selected ? "shop_clothing_icon_b" : "shop_clothing_icon_a";
                case BadgeIcon.COKE: return "mp_specitem_coke";
                case BadgeIcon.CROWN: return "mp_hostcrown";
                case BadgeIcon.FRANKLIN: return selected ? "shop_franklin_icon_b" : "shop_franklin_icon_a";
                case BadgeIcon.GUN: return selected ? "shop_gunclub_icon_b" : "shop_gunclub_icon_a";
                case BadgeIcon.HEALTH_HEART: return selected ? "shop_health_icon_b" : "shop_health_icon_a";
                case BadgeIcon.HEROIN: return "mp_specitem_heroin";
                case BadgeIcon.LOCK: return "shop_lock";
                case BadgeIcon.MAKEUP_BRUSH: return selected ? "shop_makeup_icon_b" : "shop_makeup_icon_a";
                case BadgeIcon.MASK: return selected ? "shop_mask_icon_b" : "shop_mask_icon_a";
                case BadgeIcon.MEDAL_BRONZE: return "mp_medal_bronze";
                case BadgeIcon.MEDAL_GOLD: return "mp_medal_gold";
                case BadgeIcon.MEDAL_SILVER: return "mp_medal_silver";
                case BadgeIcon.METH: return "mp_specitem_meth";
                case BadgeIcon.MICHAEL: return selected ? "shop_michael_icon_b" : "shop_michael_icon_a";
                case BadgeIcon.STAR: return "shop_new_star";
                case BadgeIcon.TATTOO: return selected ? "shop_tattoos_icon_b" : "shop_tattoos_icon_a";
                case BadgeIcon.TICK: return "shop_tick_icon";
                case BadgeIcon.TREVOR: return selected ? "shop_trevor_icon_b" : "shop_trevor_icon_a";
                case BadgeIcon.WARNING: return "mp_alerttriangle";
                case BadgeIcon.WEED: return "mp_specitem_weed";
                case BadgeIcon.MALE: return "leaderboard_male_icon";
                case BadgeIcon.FEMALE: return "leaderboard_female_icon";
                case BadgeIcon.LOCK_ARENA: return "shop_lock_arena";
                case BadgeIcon.ADVERSARY: return "adversary";
                case BadgeIcon.BASE_JUMPING: return "base_jumping";
                case BadgeIcon.BRIEFCASE: return "capture_the_flag";
                case BadgeIcon.MISSION_STAR: return "custom_mission";
                case BadgeIcon.DEATHMATCH: return "deathmatch";
                case BadgeIcon.CASTLE: return "gang_attack";
                case BadgeIcon.TROPHY: return "last_team_standing";
                case BadgeIcon.RACE_FLAG: return "race";
                case BadgeIcon.RACE_FLAG_PLANE: return "race_air";
                case BadgeIcon.RACE_FLAG_BICYCLE: return "race_bicycle";
                case BadgeIcon.RACE_FLAG_PERSON: return "race_foot";
                case BadgeIcon.RACE_FLAG_CAR: return "race_land";
                case BadgeIcon.RACE_FLAG_BOAT_ANCHOR: return "race_water";
                case BadgeIcon.ROCKSTAR: return "rockstar";
                case BadgeIcon.STUNT: return "stunt";
                case BadgeIcon.STUNT_PREMIUM: return "stunt_premium";
                case BadgeIcon.RACE_FLAG_STUNT_JUMP: return "stunt_race";
                case BadgeIcon.SHIELD: return "survival";
                case BadgeIcon.TEAM_DEATHMATCH: return "team_deathmatch";
                case BadgeIcon.VEHICLE_DEATHMATCH: return "vehicle_deathmatch";
                case BadgeIcon.MP_AMMO_PICKUP: return "ammo_pickup";
                case BadgeIcon.MP_AMMO: return "mp_anim_ammo";
                case BadgeIcon.MP_CASH: return "mp_anim_cash";
                case BadgeIcon.MP_RP: return "mp_anim_rp";
                case BadgeIcon.MP_SPECTATING: return "spectating";
                case BadgeIcon.SALE: return "saleicon";
                case BadgeIcon.GLOBE_WHITE:
                case BadgeIcon.GLOBE_RED:
                case BadgeIcon.GLOBE_BLUE:
                case BadgeIcon.GLOBE_YELLOW:
                case BadgeIcon.GLOBE_GREEN:
                case BadgeIcon.GLOBE_ORANGE:
                    return "globe";
                case BadgeIcon.INV_ARM_WRESTLING: return "arm_wrestling";
                case BadgeIcon.INV_BASEJUMP: return "basejump";
                case BadgeIcon.INV_MISSION: return "custom_mission";
                case BadgeIcon.INV_DARTS: return "darts";
                case BadgeIcon.INV_DEATHMATCH: return "deathmatch";
                case BadgeIcon.INV_DRUG: return "drug_trafficking";
                case BadgeIcon.INV_CASTLE: return "gang_attack";
                case BadgeIcon.INV_GOLF: return "golf";
                case BadgeIcon.INV_BIKE: return "mp_specitem_bike";
                case BadgeIcon.INV_BOAT: return "mp_specitem_boat";
                case BadgeIcon.INV_ANCHOR: return "mp_specitem_boatpickup";
                case BadgeIcon.INV_CAR: return "mp_specitem_car";
                case BadgeIcon.INV_DOLLAR: return "mp_specitem_cash";
                case BadgeIcon.INV_COKE: return "mp_specitem_coke";
                case BadgeIcon.INV_KEY: return "mp_specitem_cuffkeys";
                case BadgeIcon.INV_DATA: return "mp_specitem_data";
                case BadgeIcon.INV_HELI: return "mp_specitem_heli";
                case BadgeIcon.INV_HEORIN: return "mp_specitem_heroin";
                case BadgeIcon.INV_KEYCARD: return "mp_specitem_keycard";
                case BadgeIcon.INV_METH: return "mp_specitem_meth";
                case BadgeIcon.INV_BRIEFCASE: return "mp_specitem_package";
                case BadgeIcon.INV_LINK: return "mp_specitem_partnericon";
                case BadgeIcon.INV_PERSON: return "mp_specitem_ped";
                case BadgeIcon.INV_PLANE: return "mp_specitem_plane";
                case BadgeIcon.INV_PLANE2: return "mp_specitem_plane2";
                case BadgeIcon.INV_QUESTIONMARK: return "mp_specitem_randomobject";
                case BadgeIcon.INV_REMOTE: return "mp_specitem_remote";
                case BadgeIcon.INV_SAFE: return "mp_specitem_safe";
                case BadgeIcon.INV_STEER_WHEEL: return "mp_specitem_steer_wheel";
                case BadgeIcon.INV_WEAPON: return "mp_specitem_weapons";
                case BadgeIcon.INV_WEED: return "mp_specitem_weed";
                case BadgeIcon.INV_RACE_FLAG_PLANE: return "race_air";
                case BadgeIcon.INV_RACE_FLAG_BICYCLE: return "race_bike";
                case BadgeIcon.INV_RACE_FLAG_BOAT_ANCHOR: return "race_boat";
                case BadgeIcon.INV_RACE_FLAG_PERSON: return "race_foot";
                case BadgeIcon.INV_RACE_FLAG_CAR: return "race_land";
                case BadgeIcon.INV_RACE_FLAG_HELMET: return "race_offroad";
                case BadgeIcon.INV_SHOOTING_RANGE: return "shooting_range";
                case BadgeIcon.INV_SURVIVAL: return "survival";
                case BadgeIcon.INV_TEAM_DEATHMATCH: return "team_deathmatch";
                case BadgeIcon.INV_TENNIS: return "tennis";
                case BadgeIcon.INV_VEHICLE_DEATHMATCH: return "vehicle_deathmatch";
                case BadgeIcon.AUDIO_MUTE: return "leaderboard_audio_mute";
                case BadgeIcon.AUDIO_INACTIVE: return "leaderboard_audio_inactive";
                case BadgeIcon.AUDIO_VOL1: return "leaderboard_audio_1";
                case BadgeIcon.AUDIO_VOL2: return "leaderboard_audio_2";
                case BadgeIcon.AUDIO_VOL3: return "leaderboard_audio_3";
                case BadgeIcon.COUNTRY_USA: return "vehicle_card_icons_flag_usa";
                case BadgeIcon.COUNTRY_UK: return "vehicle_card_icons_flag_uk";
                case BadgeIcon.COUNTRY_SWEDEN: return "vehicle_card_icons_flag_sweden";
                case BadgeIcon.COUNTRY_KOREA: return "vehicle_card_icons_flag_korea";
                case BadgeIcon.COUNTRY_JAPAN: return "vehicle_card_icons_flag_japan";
                case BadgeIcon.COUNTRY_ITALY: return "vehicle_card_icons_flag_italy";
                case BadgeIcon.COUNTRY_GERMANY: return "vehicle_card_icons_flag_germany";
                case BadgeIcon.COUNTRY_FRANCE: return "vehicle_card_icons_flag_france";
                case BadgeIcon.BRAND_ALBANY: return "albany";
                case BadgeIcon.BRAND_ANNIS: return "annis";
                case BadgeIcon.BRAND_BANSHEE: return "banshee";
                case BadgeIcon.BRAND_BENEFACTOR: return "benefactor";
                case BadgeIcon.BRAND_BF: return "bf";
                case BadgeIcon.BRAND_BOLLOKAN: return "bollokan";
                case BadgeIcon.BRAND_BRAVADO: return "bravado";
                case BadgeIcon.BRAND_BRUTE: return "brute";
                case BadgeIcon.BRAND_BUCKINGHAM: return "buckingham";
                case BadgeIcon.BRAND_CANIS: return "canis";
                case BadgeIcon.BRAND_CHARIOT: return "chariot";
                case BadgeIcon.BRAND_CHEVAL: return "cheval";
                case BadgeIcon.BRAND_CLASSIQUE: return "classique";
                case BadgeIcon.BRAND_COIL: return "coil";
                case BadgeIcon.BRAND_DECLASSE: return "declasse";
                case BadgeIcon.BRAND_DEWBAUCHEE: return "dewbauchee";
                case BadgeIcon.BRAND_DILETTANTE: return "dilettante";
                case BadgeIcon.BRAND_DINKA: return "dinka";
                case BadgeIcon.BRAND_DUNDREARY: return "dundreary";
                case BadgeIcon.BRAND_EMPORER: return "emporer";
                case BadgeIcon.BRAND_ENUS: return "enus";
                case BadgeIcon.BRAND_FATHOM: return "fathom";
                case BadgeIcon.BRAND_GALIVANTER: return "galivanter";
                case BadgeIcon.BRAND_GROTTI: return "grotti";
                case BadgeIcon.BRAND_HIJAK: return "hijak";
                case BadgeIcon.BRAND_HVY: return "hvy";
                case BadgeIcon.BRAND_IMPONTE: return "imponte";
                case BadgeIcon.BRAND_INVETERO: return "invetero";
                case BadgeIcon.BRAND_JACKSHEEPE: return "jacksheepe";
                case BadgeIcon.BRAND_JOBUILT: return "jobuilt";
                case BadgeIcon.BRAND_KARIN: return "karin";
                case BadgeIcon.BRAND_LAMPADATI: return "lampadati";
                case BadgeIcon.BRAND_MAIBATSU: return "maibatsu";
                case BadgeIcon.BRAND_MAMMOTH: return "mammoth";
                case BadgeIcon.BRAND_MTL: return "mtl";
                case BadgeIcon.BRAND_NAGASAKI: return "nagasaki";
                case BadgeIcon.BRAND_OBEY: return "obey";
                case BadgeIcon.BRAND_OCELOT: return "ocelot";
                case BadgeIcon.BRAND_OVERFLOD: return "overflod";
                case BadgeIcon.BRAND_PED: return "ped";
                case BadgeIcon.BRAND_PEGASSI: return "pegassi";
                case BadgeIcon.BRAND_PFISTER: return "pfister";
                case BadgeIcon.BRAND_PRINCIPE: return "principe";
                case BadgeIcon.BRAND_PROGEN: return "progen";
                case BadgeIcon.BRAND_SCHYSTER: return "schyster";
                case BadgeIcon.BRAND_SHITZU: return "shitzu";
                case BadgeIcon.BRAND_SPEEDOPHILE: return "speedophile";
                case BadgeIcon.BRAND_STANLEY: return "stanley";
                case BadgeIcon.BRAND_TRUFFADE: return "truffade";
                case BadgeIcon.BRAND_UBERMACHT: return "ubermacht";
                case BadgeIcon.BRAND_VAPID: return "vapid";
                case BadgeIcon.BRAND_VULCAR: return "vulcar";
                case BadgeIcon.BRAND_WEENY: return "weeny";
                case BadgeIcon.BRAND_WESTERN: return "western";
                case BadgeIcon.BRAND_WESTERNMOTORCYCLE: return "westernmotorcycle";
                case BadgeIcon.BRAND_WILLARD: return "willard";
                case BadgeIcon.BRAND_ZIRCONIUM: return "zirconium";
                case BadgeIcon.BRAND_GROTTI2: return "grotti_2";
                case BadgeIcon.BRAND_LCC: return "lcc";
                case BadgeIcon.BRAND_PROGEN2: return "progen";
                case BadgeIcon.BRAND_RUNE: return "rune";
                case BadgeIcon.INFO: return "info_icon_32";
                default:
                    break;
            }
            return "";
        }

        /// <summary>
        /// Returns the menu this item is in.
        /// </summary>
        public UIMenu Parent { get; internal set; }

        /// <summary>
        /// Returns the lobby this item is in.
        /// </summary>
        public SettingsListColumn ParentColumn { get; internal set; }
    }
}
