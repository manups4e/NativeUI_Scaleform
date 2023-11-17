using CitizenFX.Core;
using CitizenFX.FiveM;
using ScaleformUI.Menu;
using ScaleformUI.PauseMenus;
using ScaleformUI.Scaleforms;
using static CitizenFX.FiveM.Native.Natives;

namespace ScaleformUI.PauseMenu
{
    public delegate void PauseMenuOpenEvent(TabView menu);
    public delegate void PauseMenuCloseEvent(TabView menu);
    public delegate void PauseMenuTabChanged(TabView menu, BaseTab tab, int tabIndex);
    public delegate void PauseMenuFocusChanged(TabView menu, BaseTab tab, int focusLevel);
    public delegate void LeftItemSelect(TabView menu, TabLeftItem item, int leftItemIndex);
    public delegate void RightItemSelect(TabView menu, SettingsItem item, int leftItemIndex, int rightItemIndex);

    public class TabView : PauseMenuBase
    {
        /*
        ShowCursorThisFrame();
        */
        public string AUDIO_LIBRARY = "HUD_FRONTEND_DEFAULT_SOUNDSET";

        public string AUDIO_UPDOWN = "NAV_UP_DOWN";
        public string AUDIO_LEFTRIGHT = "NAV_LEFT_RIGHT";
        public string AUDIO_SELECT = "SELECT";
        public string AUDIO_BACK = "BACK";
        public string AUDIO_ERROR = "ERROR";
        private bool isBuilding = false;
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string SideStringTop { get; set; }
        public string SideStringMiddle { get; set; }
        public string SideStringBottom { get; set; }
        public Tuple<string, string> HeaderPicture { internal get; set; }
        public Tuple<string, string> CrewPicture { internal get; set; }
        public List<BaseTab> Tabs { get; set; }
        private int index;
        public int LeftItemIndex
        {
            get => leftItemIndex;
            set
            {
                Tabs[Index].LeftItemList[leftItemIndex].Selected = false;
                leftItemIndex = value;
                Tabs[Index].LeftItemList[leftItemIndex].Selected = true;
                SendPauseMenuLeftItemChange();
            }
        }
        public int RightItemIndex
        {
            get => rightItemIndex;
            set
            {
                rightItemIndex = value;
                SendPauseMenuRightItemChange();
            }
        }
        public int FocusLevel
        {
            get => focusLevel;
            set
            {
                focusLevel = value;
                if (_pause is not null)
                    _pause.SetFocus(value);
                SendPauseMenuFocusChange();
            }
        }
        public bool TemporarilyHidden { get; set; }
        public bool HideTabs { get; set; }
        public bool DisplayHeader = true;

        internal PauseMenuScaleform _pause;
        internal bool _loaded;
        internal readonly static string _browseTextLocalized = Game.GetGXTEntry("HUD_INPUT1C");

        public event PauseMenuOpenEvent OnPauseMenuOpen;
        public event PauseMenuCloseEvent OnPauseMenuClose;
        public event PauseMenuTabChanged OnPauseMenuTabChanged;
        public event PauseMenuFocusChanged OnPauseMenuFocusChanged;
        public event LeftItemSelect OnLeftItemChange;
        public event LeftItemSelect OnLeftItemSelect;
        public event RightItemSelect OnRightItemChange;
        public event RightItemSelect OnRightItemSelect;

        public TabView(string title) : this(title, "", "", "", "")
        {
        }
        public TabView(string title, string subtitle) : this(title, subtitle, "", "", "")
        {
        }

        public TabView(string title, string subtitle, string sideTop, string sideMid, string sideBot)
        {
            Title = title;
            SubTitle = subtitle;
            SideStringTop = sideTop;
            SideStringMiddle = sideMid;
            SideStringBottom = sideBot;
            Tabs = new List<BaseTab>();
            index = 0;
            FocusLevel = 0;
            TemporarilyHidden = false;
            InstructionalButtons = new()
            {
                new InstructionalButton(Control.PhoneSelect, UIMenu._selectTextLocalized),
                new InstructionalButton(Control.PhoneCancel, UIMenu._backTextLocalized),
                new InstructionalButton(InputGroup.INPUTGROUP_FRONTEND_BUMPERS, _browseTextLocalized),
            };
            _pause = Main.PauseMenu;
        }

        public override bool Visible
        {
            get { return _visible; }
            set
            {
                Game.IsPaused = value;
                if (value)
                {
                    ActivateFrontendMenu((uint)Game.GenerateHash("FE_MENU_VERSION_EMPTY_NO_BACKGROUND"), true, -1);
                    SendPauseMenuOpen();
                    AnimpostfxPlay("PauseMenuIn", 800, true);
                    Main.InstructionalButtons.SetInstructionalButtons(InstructionalButtons);
                    SetPlayerControl(Game.Player.Handle, false, 0);
                    isBuilding = true;
                    BuildPauseMenu();
                    MenuHandler.currentBase = this;
                }
                else
                {
                    _pause.Dispose();
                    AnimpostfxStop("PauseMenuIn");
                    AnimpostfxPlay("PauseMenuOut", 800, false);
                    SendPauseMenuClose();
                    SetPlayerControl(Game.Player.Handle, true, 0);
                    MenuHandler.currentBase = null;
                    Main.InstructionalButtons.ClearButtonList();
                    ActivateFrontendMenu((uint)Game.GenerateHash("FE_MENU_VERSION_EMPTY_NO_BACKGROUND"), false, -1);
                }
                base.Visible = value;
                _visible = value;
                _pause.Visible = value;
            }
        }

        public int Index
        {
            get => index; set
            {
                Tabs[Index].Visible = false;
                index = value;
                Tabs[Index].Visible = true;
                SendPauseMenuTabChange();
            }
        }

        public void AddTab(BaseTab item)
        {
            item.Parent = this;
            Tabs.Add(item);
        }

        private bool _visible;
        private int focusLevel;
        private int rightItemIndex;
        private int leftItemIndex;
        private int _timer;
        public async void ShowHeader()
        {
            if (String.IsNullOrEmpty(SubTitle) || String.IsNullOrWhiteSpace(SubTitle))
                _pause.SetHeaderTitle(Title);
            else
            {
                _pause.ShiftCoronaDescription(true, false);
                _pause.SetHeaderTitle(Title, SubTitle);
            }
            if (HeaderPicture != null)
                _pause.SetHeaderCharImg(HeaderPicture.Item2, HeaderPicture.Item2, true);
            if (CrewPicture != null)
                _pause.SetHeaderSecondaryImg(CrewPicture.Item1, CrewPicture.Item2, true);
            _pause.SetHeaderDetails(SideStringTop, SideStringMiddle, SideStringBottom);
            _loaded = true;
        }

        public async void BuildPauseMenu()
        {
            ShowHeader();
            foreach (BaseTab tab in Tabs)
            {
                int tabIndex = Tabs.IndexOf(tab);
                switch (tab)
                {
                    case TextTab simpleTab:
                        {
                            _pause.AddPauseMenuTab(simpleTab.Title, 0, simpleTab._type, simpleTab.TabColor);
                            if (!string.IsNullOrWhiteSpace(simpleTab.TextTitle))
                                _pause.AddRightTitle(tabIndex, 0, simpleTab.TextTitle);
                            foreach (BasicTabItem it in simpleTab.LabelsList)
                                _pause.AddRightListLabel(tabIndex, 0, it.Label, it.LabelFont.FontName, it.LabelFont.FontID);
                            if (!(string.IsNullOrWhiteSpace(simpleTab.TextureDict) && string.IsNullOrWhiteSpace(simpleTab.TextureName)))
                                _pause._pause.CallFunction("UPDATE_BASE_TAB_BACKGROUND", tabIndex, simpleTab.TextureDict, simpleTab.TextureName);
                        }
                        break;
                    case SubmenuTab submenu:
                        {
                            _pause.AddPauseMenuTab(submenu.Title, 1, submenu._type, submenu.TabColor);
                            foreach (TabLeftItem item in submenu.LeftItemList)
                            {
                                int itemIndex = tab.LeftItemList.IndexOf(item);
                                _pause.AddLeftItem(tabIndex, (int)item.ItemType, item._formatLeftLabel, item.MainColor, item.HighlightColor, item.Enabled);

                                _pause._pause.CallFunction("SET_LEFT_ITEM_LABEL_FONT", tabIndex, itemIndex, item._labelFont.FontName, item._labelFont.FontID);
                                //_pause._pause.CallFunction("SET_LEFT_ITEM_RIGHT_LABEL_FONT", tabIndex, itemIndex, item._labelFont.FontName, item._labelFont.FontID);

                                if (!string.IsNullOrWhiteSpace(item.RightTitle))
                                {
                                    if (item.ItemType == LeftItemType.Keymap)
                                        _pause.AddKeymapTitle(tabIndex, itemIndex, item.RightTitle, item.KeymapRightLabel_1, item.KeymapRightLabel_2);
                                    else
                                        _pause.AddRightTitle(tabIndex, itemIndex, item.RightTitle);
                                }


                                foreach (BasicTabItem ii in item.ItemList)
                                {
                                    switch (ii)
                                    {
                                        default:
                                            {
                                                _pause.AddRightListLabel(tabIndex, itemIndex, ii.Label, ii.LabelFont.FontName, ii.LabelFont.FontID);
                                            }
                                            break;
                                        case StatsTabItem:
                                            {
                                                StatsTabItem sti = ii as StatsTabItem;
                                                switch (sti.Type)
                                                {
                                                    case StatItemType.Basic:
                                                        _pause.AddRightStatItemLabel(tabIndex, itemIndex, sti.Label, sti.RightLabel, sti.LabelFont, sti.rightLabelFont);
                                                        break;
                                                    case StatItemType.ColoredBar:
                                                        _pause.AddRightStatItemColorBar(tabIndex, itemIndex, sti.Label, sti.Value, sti.ColoredBarColor, sti.labelFont);
                                                        break;
                                                }
                                            }
                                            break;
                                        case SettingsItem:
                                            {
                                                SettingsItem sti = ii as SettingsItem;
                                                switch (sti.ItemType)
                                                {
                                                    case SettingsItemType.Basic:
                                                        _pause.AddRightSettingsBaseItem(tabIndex, itemIndex, sti.Label, sti.RightLabel, sti.Enabled);
                                                        break;
                                                    case SettingsItemType.ListItem:
                                                        SettingsListItem lis = (SettingsListItem)sti;
                                                        _pause.AddRightSettingsListItem(tabIndex, itemIndex, lis.Label, lis.ListItems, lis.ItemIndex, lis.Enabled);
                                                        break;
                                                    case SettingsItemType.ProgressBar:
                                                        SettingsProgressItem prog = (SettingsProgressItem)sti;
                                                        _pause.AddRightSettingsProgressItem(tabIndex, itemIndex, prog.Label, prog.MaxValue, prog.ColoredBarColor, prog.Value, prog.Enabled);
                                                        break;
                                                    case SettingsItemType.MaskedProgressBar:
                                                        SettingsProgressItem prog_alt = (SettingsProgressItem)sti;
                                                        _pause.AddRightSettingsProgressItemAlt(tabIndex, itemIndex, sti.Label, prog_alt.MaxValue, prog_alt.ColoredBarColor, prog_alt.Value, prog_alt.Enabled);
                                                        break;
                                                    case SettingsItemType.CheckBox:
                                                        while (!HasStreamedTextureDictLoaded("commonmenu"))
                                                        {
                                                            await BaseScript.Delay(0);
                                                            RequestStreamedTextureDict("commonmenu", true);
                                                        }
                                                        SettingsCheckboxItem check = (SettingsCheckboxItem)sti;
                                                        _pause.AddRightSettingsCheckboxItem(tabIndex, itemIndex, check.Label, check.CheckBoxStyle, check.IsChecked, check.Enabled);
                                                        break;
                                                    case SettingsItemType.SliderBar:
                                                        SettingsSliderItem slid = (SettingsSliderItem)sti;
                                                        _pause.AddRightSettingsSliderItem(tabIndex, itemIndex, slid.Label, slid.MaxValue, slid.ColoredBarColor, slid.Value, slid.Enabled);
                                                        break;
                                                }
                                            }
                                            break;
                                        case KeymapItem:
                                            KeymapItem ki = ii as KeymapItem;
                                            if (IsInputDisabled(2))
                                                _pause.AddKeymapItem(tabIndex, itemIndex, ki.Label, ki.PrimaryKeyboard, ki.SecondaryKeyboard);
                                            else
                                                _pause.AddKeymapItem(tabIndex, itemIndex, ki.Label, ki.PrimaryGamepad, ki.SecondaryGamepad);
                                            UpdateKeymapItems();
                                            break;
                                    }
                                }

                                if (item.ItemType == LeftItemType.Info || item.ItemType == LeftItemType.Statistics || item.ItemType == LeftItemType.Settings)
                                {
                                    if (!(string.IsNullOrWhiteSpace(item.TextureDict) && string.IsNullOrWhiteSpace(item.TextureName)))
                                        _pause._pause.CallFunction("UPDATE_LEFT_ITEM_RIGHT_BACKGROUND", tabIndex, itemIndex, item.TextureDict, item.TextureName, (int)item.LeftItemBGType);
                                }

                            }
                        }
                        break;
                    case PlayerListTab plTab:
                        {
                            _pause.AddPauseMenuTab(plTab.Title, 1, plTab._type, plTab.TabColor);
                            switch (plTab.listCol.Count)
                            {
                                case 1:
                                    _pause._pause.CallFunction("CREATE_PLAYERS_TAB_COLUMNS", tabIndex, plTab.listCol[0].Type);
                                    break;
                                case 2:
                                    _pause._pause.CallFunction("CREATE_PLAYERS_TAB_COLUMNS", tabIndex, plTab.listCol[0].Type, plTab.listCol[1].Type);
                                    break;
                                case 3:
                                    _pause._pause.CallFunction("CREATE_PLAYERS_TAB_COLUMNS", tabIndex, plTab.listCol[0].Type, plTab.listCol[1].Type, plTab.listCol[2].Type);
                                    break;
                            }
                            _pause._pause.CallFunction("SET_PLAYERS_TAB_NEWSTYLE", tabIndex, plTab._newStyle);
                            if (plTab.listCol.Any(x => x.Type == "settings"))
                            {
                                plTab.SettingsColumn.Parent = this;
                                plTab.SettingsColumn.ParentTab = Tabs.IndexOf(plTab);
                                plTab.SettingsColumn.isBuilding = true;
                                buildSettings(plTab);
                            }
                            if (plTab.listCol.Any(x => x.Type == "players"))
                            {
                                plTab.PlayersColumn.Parent = this;
                                plTab.PlayersColumn.ParentTab = Tabs.IndexOf(plTab);
                                plTab.PlayersColumn.isBuilding = true;
                                buildPlayers(plTab);
                            }
                            if (plTab.listCol.Any(x => x.Type == "missions"))
                            {
                                plTab.MissionsColumn.isBuilding = true;
                                plTab.MissionsColumn.Parent = this;
                                plTab.MissionsColumn.ParentTab = Tabs.IndexOf(plTab);
                                buildMissions(plTab);
                            }
                            if (plTab.listCol.Any(x => x.Type == "panel"))
                            {
                                plTab.MissionPanel.Parent = this;
                                plTab.MissionPanel.ParentTab = Tabs.IndexOf(plTab);
                                _pause._pause.CallFunction("ADD_PLAYERS_TAB_MISSION_PANEL_PICTURE", tabIndex, plTab.MissionPanel.TextureDict, plTab.MissionPanel.TextureName);
                                _pause._pause.CallFunction("SET_PLAYERS_TAB_MISSION_PANEL_TITLE", tabIndex, plTab.MissionPanel.Title);
                                if (plTab.MissionPanel.Items.Count > 0)
                                {
                                    foreach (UIFreemodeDetailsItem item in plTab.MissionPanel.Items)
                                    {
                                        _pause._pause.CallFunction("ADD_PLAYERS_TAB_MISSION_PANEL_ITEM", tabIndex, item.Type, item.TextLeft, item.TextRight, (int)item.Icon, item.IconColor, item.Tick, item._labelFont.FontName, item._labelFont.FontID, item._rightLabelFont.FontName, item._rightLabelFont.FontID);
                                    }
                                }
                            }
                            while ((plTab.SettingsColumn != null && plTab.SettingsColumn.isBuilding) || (plTab.PlayersColumn != null && plTab.PlayersColumn.isBuilding) || (plTab.MissionsColumn != null && plTab.MissionsColumn.isBuilding)) await BaseScript.Delay(0);
                            plTab.updateFocus(0);
                        }
                        break;
                }
            }
            isBuilding = false;
        }

        bool canBuild = true;
        internal async void buildSettings(PlayerListTab tab)
        {
            int i = 0;
            int tab_id = Tabs.IndexOf(tab);
            int max = tab.SettingsColumn.Pagination.ItemsPerPage;
            if (tab.SettingsColumn.Items.Count < max)
                max = tab.SettingsColumn.Items.Count;

            tab.SettingsColumn.Pagination.MinItem = tab.SettingsColumn.Pagination.CurrentPageStartIndex;
            if (tab.SettingsColumn.Pagination.scrollType == ScrollingType.CLASSIC && tab.SettingsColumn.Pagination.TotalPages > 1)
            {
                int missingItems = tab.SettingsColumn.Pagination.GetMissingItems();
                if (missingItems > 0)
                {
                    tab.SettingsColumn.Pagination.ScaleformIndex = tab.SettingsColumn.Pagination.GetPageIndexFromMenuIndex(tab.SettingsColumn.Pagination.CurrentPageEndIndex) + missingItems;
                    tab.SettingsColumn.Pagination.MinItem = tab.SettingsColumn.Pagination.CurrentPageStartIndex - missingItems;
                }
            }
            tab.SettingsColumn.Pagination.MaxItem = tab.SettingsColumn.Pagination.CurrentPageEndIndex;

            while (i < max)
            {
                await BaseScript.Delay(0);
                if (!Visible) return;
                tab.SettingsColumn._itemCreation(tab.SettingsColumn.Pagination.CurrentPage, i, false, true);
                i++;
            }
            tab.SettingsColumn.CurrentSelection = 0;
            tab.SettingsColumn.Pagination.ScaleformIndex = tab.SettingsColumn.Pagination.GetScaleformIndex(tab.SettingsColumn.CurrentSelection);
            tab.SettingsColumn.Items[0].Selected = false;
            _pause._pause.CallFunction("SET_PLAYERS_TAB_SETTINGS_SELECTION", tab_id, tab.SettingsColumn.Pagination.ScaleformIndex);
            _pause._pause.CallFunction("SET_PLAYERS_TAB_SETTINGS_QTTY", tab_id, tab.SettingsColumn.CurrentSelection + 1, tab.SettingsColumn.Items.Count);
            tab.SettingsColumn.isBuilding = false;
        }

        internal async void buildPlayers(PlayerListTab tab)
        {
            int i = 0;
            int tab_id = Tabs.IndexOf(tab);
            int max = tab.PlayersColumn.Pagination.ItemsPerPage;
            if (tab.PlayersColumn.Items.Count < max)
                max = tab.PlayersColumn.Items.Count;

            tab.PlayersColumn.Pagination.MinItem = tab.PlayersColumn.Pagination.CurrentPageStartIndex;
            if (tab.PlayersColumn.Pagination.scrollType == ScrollingType.CLASSIC && tab.PlayersColumn.Pagination.TotalPages > 1)
            {
                int missingItems = tab.PlayersColumn.Pagination.GetMissingItems();
                if (missingItems > 0)
                {
                    tab.PlayersColumn.Pagination.ScaleformIndex = tab.PlayersColumn.Pagination.GetPageIndexFromMenuIndex(tab.PlayersColumn.Pagination.CurrentPageEndIndex) + missingItems;
                    tab.PlayersColumn.Pagination.MinItem = tab.PlayersColumn.Pagination.CurrentPageStartIndex - missingItems;
                }
            }
            tab.PlayersColumn.Pagination.MaxItem = tab.PlayersColumn.Pagination.CurrentPageEndIndex;

            while (i < max)
            {
                await BaseScript.Delay(0);
                if (!Visible) return;
                tab.PlayersColumn._itemCreation(tab.PlayersColumn.Pagination.CurrentPage, i, false, true);
                i++;
            }
            tab.PlayersColumn.CurrentSelection = 0;
            tab.PlayersColumn.Pagination.ScaleformIndex = tab.PlayersColumn.Pagination.GetScaleformIndex(tab.PlayersColumn.CurrentSelection);
            tab.PlayersColumn.Items[0].Selected = false;
            _pause._pause.CallFunction("SET_PLAYERS_TAB_PLAYERS_SELECTION", tab_id, tab.PlayersColumn.Pagination.ScaleformIndex);
            _pause._pause.CallFunction("SET_PLAYERS_TAB_PLAYERS_QTTY", tab_id, tab.PlayersColumn.CurrentSelection + 1, tab.PlayersColumn.Items.Count);
            tab.PlayersColumn.isBuilding = false;
        }

        internal async void buildMissions(PlayerListTab tab)
        {
            int i = 0;
            int tab_id = Tabs.IndexOf(tab);
            int max = tab.MissionsColumn.Pagination.ItemsPerPage;
            if (tab.MissionsColumn.Items.Count < max)
                max = tab.MissionsColumn.Items.Count;

            tab.MissionsColumn.Pagination.MinItem = tab.MissionsColumn.Pagination.CurrentPageStartIndex;
            if (tab.MissionsColumn.Pagination.scrollType == ScrollingType.CLASSIC && tab.MissionsColumn.Pagination.TotalPages > 1)
            {
                int missingItems = tab.MissionsColumn.Pagination.GetMissingItems();
                if (missingItems > 0)
                {
                    tab.MissionsColumn.Pagination.ScaleformIndex = tab.MissionsColumn.Pagination.GetPageIndexFromMenuIndex(tab.MissionsColumn.Pagination.CurrentPageEndIndex) + missingItems;
                    tab.MissionsColumn.Pagination.MinItem = tab.MissionsColumn.Pagination.CurrentPageStartIndex - missingItems;
                }
            }
            tab.MissionsColumn.Pagination.MaxItem = tab.MissionsColumn.Pagination.CurrentPageEndIndex;

            while (i < max)
            {
                await BaseScript.Delay(0);
                if (!Visible) return;
                tab.MissionsColumn._itemCreation(tab.MissionsColumn.Pagination.CurrentPage, i, false, true);
                i++;
            }
            tab.MissionsColumn.CurrentSelection = 0;
            tab.MissionsColumn.Pagination.ScaleformIndex = tab.MissionsColumn.Pagination.GetScaleformIndex(tab.MissionsColumn.CurrentSelection);
            tab.MissionsColumn.Items[0].Selected = false;
            _pause._pause.CallFunction("SET_PLAYERS_TAB_MISSIONS_SELECTION", tab_id, tab.MissionsColumn.Pagination.ScaleformIndex);
            _pause._pause.CallFunction("SET_PLAYERS_TAB_MISSIONS_QTTY", tab_id, tab.MissionsColumn.CurrentSelection + 1, tab.MissionsColumn.Items.Count);
            tab.MissionsColumn.isBuilding = false;
        }

        private bool controller = false;
        public override async void Draw()
        {
            if (!Visible || TemporarilyHidden || isBuilding) return;
            base.Draw();
            _pause.Draw();
            UpdateKeymapItems();
        }

        private void UpdateKeymapItems()
        {
            if (!IsInputDisabled(2))
            {
                if (!controller)
                {
                    controller = true;
                    if (Tabs[Index] is SubmenuTab)
                    {
                        foreach (TabLeftItem lItem in (Tabs[Index] as SubmenuTab).LeftItemList)
                        {
                            int idx = (Tabs[Index] as SubmenuTab).LeftItemList.IndexOf(lItem);
                            if (lItem.ItemType == LeftItemType.Keymap)
                            {
                                for (int i = 0; i < lItem.ItemList.Count; i++)
                                {
                                    KeymapItem item = (KeymapItem)lItem.ItemList[i];
                                    _pause.UpdateKeymap(Index, idx, i, item.PrimaryGamepad, item.SecondaryGamepad);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (controller)
                {
                    controller = false;
                    if (Tabs[Index] is SubmenuTab)
                    {
                        foreach (TabLeftItem lItem in (Tabs[Index] as SubmenuTab).LeftItemList)
                        {
                            int idx = (Tabs[Index] as SubmenuTab).LeftItemList.IndexOf(lItem);
                            if (lItem.ItemType == LeftItemType.Keymap)
                            {
                                for (int i = 0; i < lItem.ItemList.Count; i++)
                                {
                                    KeymapItem item = (KeymapItem)lItem.ItemList[i];
                                    _pause.UpdateKeymap(Index, idx, i, item.PrimaryKeyboard, item.SecondaryKeyboard);
                                }
                            }
                        }
                    }
                }
            }
        }

        public async void Select(bool playSound)
        {
            switch (FocusLevel)
            {
                case 0:
                    FocusLevel++;
                    if (Tabs[Index] is PlayerListTab pl)
                    {
                        int selection = pl._newStyle ? pl.Focus : 0;
                        switch (pl.listCol[selection].Type)
                        {
                            case "settings":
                                pl.SettingsColumn.Items[pl.SettingsColumn.CurrentSelection].Selected = true;
                                break;
                            case "players":
                                pl.PlayersColumn.Items[pl.PlayersColumn.CurrentSelection].Selected = true;
                                if (pl.PlayersColumn.Items[pl.PlayersColumn.CurrentSelection].KeepPanelVisible)
                                    pl.PlayersColumn.Items[pl.PlayersColumn.CurrentSelection].CreateClonedPed();
                                break;
                            case "missions":
                                pl.MissionsColumn.Items[pl.MissionsColumn.CurrentSelection].Selected = true;
                                break;
                        }
                        if (pl.listCol.Any(x => x.Type == "players"))
                            SetPauseMenuPedLighting(FocusLevel != 0);
                    }
                    else if (Tabs[Index] is SubmenuTab)
                    {
                        Tabs[Index].LeftItemList[LeftItemIndex].Selected = true;
                    }
                    if (Tabs[Index].LeftItemList.All(x => !x.Enabled)) break;
                    while (!Tabs[Index].LeftItemList[leftItemIndex].Enabled)
                    {
                        await BaseScript.Delay(0);
                        LeftItemIndex++;
                        _pause._pause.CallFunction("SELECT_LEFT_ITEM_INDEX", leftItemIndex);
                    }
                    break;
                case 1:
                    {
                        if (Tabs[Index] is SubmenuTab)
                        {
                            TabLeftItem leftItem = Tabs[Index].LeftItemList[LeftItemIndex];
                            if (!leftItem.Enabled)
                            {
                                Game.PlaySound(AUDIO_ERROR, AUDIO_LIBRARY);
                                return;
                            }
                            if (leftItem.ItemType == LeftItemType.Settings)
                            {
                                FocusLevel = 2;
                                if (leftItem.ItemList.All(x => !(x as SettingsItem).Enabled)) break;
                                while (!(leftItem.ItemList[rightItemIndex] as SettingsItem).Enabled)
                                {
                                    await BaseScript.Delay(0);
                                    rightItemIndex++;
                                    _pause._pause.CallFunction("SELECT_RIGHT_ITEM_INDEX", rightItemIndex);
                                }
                            }
                            SendPauseMenuLeftItemSelect();
                        }
                        else if (Tabs[Index] is PlayerListTab plTab)
                        {
                            switch (plTab.listCol[plTab.Focus].Type)
                            {
                                case "settings":
                                    UIMenuItem item = plTab.SettingsColumn.Items[plTab.SettingsColumn.CurrentSelection];
                                    if (!item.Enabled)
                                    {
                                        Game.PlaySound(AUDIO_ERROR, AUDIO_LIBRARY);
                                        return;
                                    }
                                    switch (item)
                                    {
                                        case UIMenuCheckboxItem:
                                            {
                                                UIMenuCheckboxItem it = item as UIMenuCheckboxItem;
                                                it.Checked = !it.Checked;
                                                it.CheckboxEventTrigger();
                                                plTab.SettingsColumn.SelectItem();
                                                break;
                                            }

                                        case UIMenuListItem:
                                            {
                                                UIMenuListItem it = item as UIMenuListItem;
                                                it.ListSelectedTrigger(it.Index);
                                                plTab.SettingsColumn.SelectItem();
                                                break;
                                            }

                                        default:
                                            item.ItemActivate(null);
                                            plTab.SettingsColumn.SelectItem();
                                            break;
                                    }
                                    _pause._pause.CallFunction("SET_INPUT_EVENT", 16);
                                    break;
                                case "missions":
                                    MissionItem mitem = plTab.MissionsColumn.Items[plTab.MissionsColumn.CurrentSelection];
                                    mitem.ActivateMission(plTab);
                                    plTab.MissionsColumn.SelectItem();
                                    break;
                                case "players":
                                    plTab.PlayersColumn.SelectItem();
                                    break;
                            }
                        }
                    }
                    break;
                case 2:
                    {
                        _pause._pause.CallFunction("SET_INPUT_EVENT", 16);
                        TabLeftItem leftItem = Tabs[Index].LeftItemList[LeftItemIndex];
                        if (leftItem.ItemType == LeftItemType.Settings)
                        {
                            if (leftItem.ItemList[RightItemIndex] is SettingsItem rightItem)
                            {
                                if (!rightItem.Enabled)
                                {
                                    Game.PlaySound(AUDIO_ERROR, AUDIO_LIBRARY);
                                    return;
                                }

                                switch (rightItem.ItemType)
                                {
                                    case SettingsItemType.ListItem:
                                        (rightItem as SettingsListItem).ListSelected();
                                        break;
                                    case SettingsItemType.CheckBox:
                                        (rightItem as SettingsCheckboxItem).IsChecked = !(rightItem as SettingsCheckboxItem).IsChecked!;
                                        break;
                                    case SettingsItemType.MaskedProgressBar:
                                    case SettingsItemType.ProgressBar:
                                        (rightItem as SettingsProgressItem).ProgressSelected();
                                        break;
                                    case SettingsItemType.SliderBar:
                                        (rightItem as SettingsSliderItem).SliderSelected();
                                        break;
                                    default:
                                        rightItem.Activated();
                                        break;
                                }
                                SendPauseMenuRightItemSelect();
                            }
                        }
                    }
                    break;
            }
            if (playSound) Game.PlaySound(AUDIO_SELECT, AUDIO_LIBRARY);
        }

        public void GoBack()
        {
            Game.PlaySound(AUDIO_BACK, AUDIO_LIBRARY);
            if (FocusLevel > 0)
            {
                if (Tabs[Index] is not PlayerListTab)
                {
                    FocusLevel--;
                    Tabs[Index].LeftItemList[LeftItemIndex].Selected = focusLevel == 1;
                }
                else if (Tabs[Index] is PlayerListTab pl)
                {
                    if (pl._newStyle)
                    {
                        FocusLevel--;
                        SetPauseMenuPedLighting(FocusLevel != 0);
                        if (pl.listCol.Any(x => x.Type == "settings"))
                            pl.SettingsColumn.Items[pl.SettingsColumn.CurrentSelection].Selected = false;
                        if (pl.listCol.Any(x => x.Type == "players"))
                            pl.PlayersColumn.Items[pl.PlayersColumn.CurrentSelection].Selected = false;
                        if (pl.listCol.Any(x => x.Type == "missions"))
                            pl.MissionsColumn.Items[pl.MissionsColumn.CurrentSelection].Selected = false;
                    }
                    else
                    {
                        if (FocusLevel == 1)
                        {
                            switch (pl.listCol[pl.Focus].Type)
                            {
                                case "settings":
                                    pl.SettingsColumn.Items[pl.SettingsColumn.CurrentSelection].Selected = false;
                                    break;
                                case "players":
                                    pl.PlayersColumn.Items[pl.PlayersColumn.CurrentSelection].Selected = false;
                                    ClearPedInPauseMenu();
                                    break;
                                case "missions":
                                    pl.MissionsColumn.Items[pl.MissionsColumn.CurrentSelection].Selected = false;
                                    break;
                            }
                            if (pl.Focus == 0)
                            {
                                FocusLevel--;
                                return;
                            }
                            pl.updateFocus(pl.Focus - 1);
                            return;
                        }
                    }
                }
            }
            else
            {
                if (CanPlayerCloseMenu) Visible = false;
            }
        }

        public async void GoUp()
        {
            if (Tabs[Index] is PlayerListTab plTab && FocusLevel == 1)
            {
                switch (plTab.listCol[plTab.Focus].Type)
                {
                    case "players":
                        plTab.PlayersColumn.GoUp();
                        break;
                    case "settings":
                        plTab.SettingsColumn.GoUp();
                        break;
                    case "missions":
                        plTab.MissionsColumn.GoUp();
                        break;
                }
                Game.PlaySound(AUDIO_UPDOWN, AUDIO_LIBRARY);
                return;
            }
            int retVal = await _pause._pause.CallFunctionReturnValueInt("SET_INPUT_EVENT", 8);
            if (retVal != -1)
            {
                if (FocusLevel == 1)
                {
                    LeftItemIndex = retVal;
                }
                else if (FocusLevel == 2)
                {
                    RightItemIndex = retVal;
                }
            }
        }

        public async void GoDown()
        {
            if (Tabs[Index] is PlayerListTab plTab && FocusLevel == 1)
            {
                switch (plTab.listCol[plTab.Focus].Type)
                {
                    case "players":
                        plTab.PlayersColumn.GoDown();
                        break;
                    case "settings":
                        plTab.SettingsColumn.GoDown();
                        break;
                    case "missions":
                        plTab.MissionsColumn.GoDown();
                        break;
                }
                Game.PlaySound(AUDIO_UPDOWN, AUDIO_LIBRARY);
                return;
            }

            int retVal = await _pause._pause.CallFunctionReturnValueInt("SET_INPUT_EVENT", 9);
            if (retVal != -1)
            {
                if (FocusLevel == 1)
                {
                    LeftItemIndex = retVal;
                }
                else if (FocusLevel == 2)
                {
                    RightItemIndex = retVal;
                }
            }
        }

        public async void GoLeft()
        {
            int retVal = await _pause._pause.CallFunctionReturnValueInt("SET_INPUT_EVENT", 10);
            switch (FocusLevel)
            {
                case 0:
                    ClearPedInPauseMenu();
                    _pause.HeaderGoLeft();
                    if (Tabs[Index] is SubmenuTab)
                    {
                        Tabs[Index].LeftItemList[LeftItemIndex].Selected = false;
                    }
                    Tabs[Index].Visible = false;
                    Index = retVal;
                    Tabs[Index].Visible = true;
                    if (Tabs[Index] is PlayerListTab _plTab)
                    {
                        if (_plTab.listCol.Any(x => x.Type == "settings") && _plTab.SettingsColumn != null && _plTab.SettingsColumn.Items.Count > 0)
                        {
                            UIMenuItem item = _plTab.SettingsColumn.Items[_plTab.SettingsColumn.CurrentSelection];
                            if (item is not UIMenuListItem && item is not UIMenuSliderItem && item is not UIMenuProgressItem)
                            {
                                item.Selected = false;
                            }
                        }
                        if (_plTab.listCol.Any(x => x.Type == "missions") && _plTab.MissionsColumn != null && _plTab.MissionsColumn.Items.Count > 0)
                            _plTab.MissionsColumn.Items[_plTab.MissionsColumn.CurrentSelection].Selected = false;
                        if (_plTab.listCol.Any(x => x.Type == "players"))
                        {
                            _plTab.PlayersColumn.Items[_plTab.PlayersColumn.CurrentSelection].Selected = false;
                            if (_plTab.listCol[0].Type == "players" || _plTab.PlayersColumn.Items[_plTab.PlayersColumn.CurrentSelection].KeepPanelVisible)
                            {
                                if (_plTab.PlayersColumn.Items[_plTab.PlayersColumn.CurrentSelection].ClonePed != null)
                                    _plTab.PlayersColumn.Items[_plTab.PlayersColumn.CurrentSelection].CreateClonedPed();
                                else
                                    ClearPedInPauseMenu();
                            }
                            else
                            {
                                ClearPedInPauseMenu();
                            }
                        }
                        else ClearPedInPauseMenu();
                    }
                    break;
                case 1:
                    {
                        if (Tabs[Index] is PlayerListTab plTab)
                        {
                            switch (plTab.listCol[plTab.Focus].Type)
                            {
                                case "settings":
                                    {
                                        UIMenuItem item = plTab.SettingsColumn.Items[plTab.SettingsColumn.CurrentSelection];
                                        if (!item.Enabled)
                                        {
                                            if (plTab._newStyle)
                                            {
                                                plTab.SettingsColumn.Items[plTab.SettingsColumn.CurrentSelection].Selected = false;
                                                plTab.updateFocus(plTab.Focus - 1);
                                            }
                                            else
                                            {
                                                Game.PlaySound("ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                                            }
                                            return;
                                        }

                                        if (item is UIMenuListItem it)
                                        {
                                            it.Index = retVal;
                                            //ListChange(it, it.Index);
                                            it.ListChangedTrigger(it.Index);
                                        }
                                        else if (item is UIMenuSliderItem slit)
                                        {
                                            slit.Value = retVal;
                                            slit.SliderChanged(slit.Value);
                                            //SliderChange(it, it.Value);
                                        }
                                        else if (item is UIMenuProgressItem prit)
                                        {
                                            prit.Value = retVal;
                                            prit.ProgressChanged(prit.Value);
                                            //ProgressChange(it, it.Value);
                                        }
                                        else
                                        {
                                            if (plTab._newStyle)
                                            {
                                                plTab.SettingsColumn.Items[plTab.SettingsColumn.CurrentSelection].Selected = false;
                                                plTab.updateFocus(plTab.Focus - 1);
                                            }
                                        }
                                    }
                                    break;
                                case "missions":
                                    if (plTab._newStyle)
                                    {
                                        plTab.MissionsColumn.Items[plTab.MissionsColumn.CurrentSelection].Selected = false;
                                        plTab.updateFocus(plTab.Focus - 1);
                                    }
                                    break;
                                case "panel":
                                    plTab.updateFocus(plTab.Focus - 1);
                                    break;
                                case "players":
                                    if (plTab._newStyle)
                                    {
                                        plTab.PlayersColumn.Items[plTab.PlayersColumn.CurrentSelection].Selected = false;
                                        plTab.updateFocus(plTab.Focus - 1);
                                    }
                                    else
                                    {
                                        if (plTab.PlayersColumn.Items[plTab.PlayersColumn.CurrentSelection].ClonePed != null)
                                            plTab.PlayersColumn.Items[plTab.PlayersColumn.CurrentSelection].CreateClonedPed();
                                    }
                                    break;
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        SettingsItem rightItem = Tabs[Index].LeftItemList[LeftItemIndex].ItemList[RightItemIndex] as SettingsItem;
                        switch (rightItem.ItemType)
                        {
                            case SettingsItemType.ListItem:
                                (rightItem as SettingsListItem).ItemIndex = retVal;
                                (rightItem as SettingsListItem).ListChanged();
                                break;
                            case SettingsItemType.SliderBar:
                                (rightItem as SettingsSliderItem).Value = retVal;
                                (rightItem as SettingsSliderItem).SliderChanged();
                                break;
                            case SettingsItemType.ProgressBar:
                            case SettingsItemType.MaskedProgressBar:
                                (rightItem as SettingsProgressItem).Value = retVal;
                                (rightItem as SettingsProgressItem).ProgressChanged();
                                break;
                        }

                        break;
                    }
            }
        }

        public async void GoRight()
        {
            int retVal = await _pause._pause.CallFunctionReturnValueInt("SET_INPUT_EVENT", 11);

            switch (FocusLevel)
            {
                case 0:
                    ClearPedInPauseMenu();
                    _pause.HeaderGoRight();
                    if (Tabs[Index] is SubmenuTab)
                    {
                        Tabs[Index].LeftItemList[LeftItemIndex].Selected = false;
                    }
                    Tabs[Index].Visible = false;
                    Index = retVal;
                    Tabs[Index].Visible = true;
                    if (Tabs[Index] is PlayerListTab _plTab)
                    {
                        if (_plTab.listCol.Any(x => x.Type == "settings") && _plTab.SettingsColumn != null && _plTab.SettingsColumn.Items.Count > 0)
                        {
                            UIMenuItem item = _plTab.SettingsColumn.Items[_plTab.SettingsColumn.CurrentSelection];
                            if (item is not UIMenuListItem && item is not UIMenuSliderItem && item is not UIMenuProgressItem)
                            {
                                item.Selected = false;
                            }
                        }
                        if (_plTab.listCol.Any(x => x.Type == "missions") && _plTab.MissionsColumn != null && _plTab.MissionsColumn.Items.Count > 0)
                            _plTab.MissionsColumn.Items[_plTab.MissionsColumn.CurrentSelection].Selected = false;
                        if (_plTab.listCol.Any(x => x.Type == "players"))
                        {
                            _plTab.PlayersColumn.Items[_plTab.PlayersColumn.CurrentSelection].Selected = false;
                            if (_plTab.listCol[0].Type == "players" || _plTab.PlayersColumn.Items[_plTab.PlayersColumn.CurrentSelection].KeepPanelVisible)
                            {
                                if (_plTab.PlayersColumn.Items[_plTab.PlayersColumn.CurrentSelection].ClonePed != null)
                                    _plTab.PlayersColumn.Items[_plTab.PlayersColumn.CurrentSelection].CreateClonedPed();
                                else
                                    ClearPedInPauseMenu();
                            }
                            else
                                ClearPedInPauseMenu();
                        }
                        else ClearPedInPauseMenu();
                    }
                    break;
                case 1:
                    {
                        if (Tabs[Index] is PlayerListTab plTab)
                        {
                            switch (plTab.listCol[plTab.Focus].Type)
                            {
                                case "settings":
                                    {
                                        UIMenuItem item = plTab.SettingsColumn.Items[plTab.SettingsColumn.CurrentSelection];
                                        if (!item.Enabled)
                                        {
                                            if (plTab._newStyle)
                                            {
                                                plTab.SettingsColumn.Items[plTab.SettingsColumn.CurrentSelection].Selected = false;
                                                plTab.updateFocus(plTab.Focus + 1);
                                            }
                                            else
                                            {
                                                Game.PlaySound("ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                                            }
                                            return;
                                        }

                                        if (item is UIMenuListItem it)
                                        {
                                            it.Index = retVal;
                                            //ListChange(it, it.Index);
                                            it.ListChangedTrigger(it.Index);
                                        }
                                        else if (item is UIMenuSliderItem slit)
                                        {
                                            slit.Value = retVal;
                                            slit.SliderChanged(slit.Value);
                                            //SliderChange(it, it.Value);
                                        }
                                        else if (item is UIMenuProgressItem prit)
                                        {
                                            prit.Value = retVal;
                                            prit.ProgressChanged(prit.Value);
                                            //ProgressChange(it, it.Value);
                                        }
                                        else
                                        {
                                            if (plTab._newStyle)
                                            {
                                                plTab.SettingsColumn.Items[plTab.SettingsColumn.CurrentSelection].Selected = false;
                                                plTab.updateFocus(plTab.Focus + 1);
                                            }
                                        }
                                    }
                                    break;
                                case "missions":
                                    if (plTab._newStyle)
                                    {
                                        plTab.MissionsColumn.Items[plTab.MissionsColumn.CurrentSelection].Selected = false;
                                        plTab.updateFocus(plTab.Focus + 1);
                                    }
                                    break;
                                case "panel":
                                    plTab.updateFocus(plTab.Focus + 1);
                                    break;
                                case "players":
                                    if (plTab._newStyle)
                                    {
                                        plTab.PlayersColumn.Items[plTab.PlayersColumn.CurrentSelection].Selected = false;
                                        plTab.updateFocus(plTab.Focus + 1);
                                    }
                                    break;
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        SettingsItem rightItem = Tabs[Index].LeftItemList[LeftItemIndex].ItemList[RightItemIndex] as SettingsItem;
                        switch (rightItem.ItemType)
                        {
                            case SettingsItemType.ListItem:
                                (rightItem as SettingsListItem).ItemIndex = retVal;
                                (rightItem as SettingsListItem).ListChanged();
                                break;
                            case SettingsItemType.SliderBar:
                                (rightItem as SettingsSliderItem).Value = retVal;
                                (rightItem as SettingsSliderItem).SliderChanged();
                                break;
                            case SettingsItemType.ProgressBar:
                            case SettingsItemType.MaskedProgressBar:
                                (rightItem as SettingsProgressItem).Value = retVal;
                                (rightItem as SettingsProgressItem).ProgressChanged();
                                break;
                        }

                        break;
                    }
            }

        }

        private bool firstTick = true;
        private bool eventBool = false;
        private int eventType = 0;
        private int itemId = 0;
        private int context = 0;
        private int unused = 0;

        public override async void ProcessMouse()
        {
            if (!IsUsingKeyboard(2))
            {
                return;
            }
            // check for is using keyboard (2) to use Mouse or not.
            SetMouseCursorActiveThisFrame();
            SetInputExclusive(2, 239);
            SetInputExclusive(2, 240);
            SetInputExclusive(2, 237);
            SetInputExclusive(2, 238);

            bool successHeader = GetScaleformMovieCursorSelection(Main.PauseMenu._header.Handle, ref eventBool, ref eventType, ref context, ref itemId);
            if (successHeader)
            {
                switch (eventType)
                {
                    case 5: // on click pressed
                        switch (context)
                        {
                            case -1:
                                _pause.SelectTab(itemId);
                                FocusLevel = 1;
                                Index = itemId;
                                if (Tabs[Index] is PlayerListTab tab)
                                {
                                    if (tab.PlayersColumn.Items[tab.PlayersColumn.CurrentSelection].ClonePed != null)
                                        tab.PlayersColumn.Items[tab.PlayersColumn.CurrentSelection].CreateClonedPed();
                                    else
                                        ClearPedInPauseMenu();
                                }
                                else
                                    ClearPedInPauseMenu();
                                Game.PlaySound("SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                                if (Tabs[Index].LeftItemList.All(x => !x.Enabled)) break;
                                Tabs[Index].LeftItemList[LeftItemIndex].Selected = true;
                                while (!Tabs[Index].LeftItemList[leftItemIndex].Enabled)
                                {
                                    await BaseScript.Delay(0);
                                    LeftItemIndex++;
                                    _pause._pause.CallFunction("SELECT_LEFT_ITEM_INDEX", leftItemIndex);
                                }
                                break;
                                /* TODO: CHANGE IT WITH SPRITE LIKE THE ACTUAL PAUSE MENU
                            case 1:
                                switch (itemId)
                                {
                                    case 0:
                                        _pause.HeaderGoLeft();
                                        break;
                                    case 1:
                                        _pause.HeaderGoRight();
                                        break;
                                }
                                break;
                                */
                        }
                        break;
                }
            }

            bool successPause = GetScaleformMovieCursorSelection(Main.PauseMenu._header.Handle, ref eventBool, ref eventType, ref context, ref itemId);
            if (successPause)
            {
                switch (eventType)
                {
                    case 5: // on click pressed
                        if (FocusLevel == 1 && Tabs[Index] is PlayerListTab tab)
                        {
                            int foc = tab.Focus;
                            int curSel = 0;
                            if (tab._newStyle)
                            {
                                switch (tab.listCol[foc].Type)
                                {
                                    case "settings":
                                        curSel = tab.SettingsColumn.CurrentSelection;
                                        tab.SettingsColumn.Items[tab.SettingsColumn.CurrentSelection].Selected = false;
                                        break;
                                    case "players":
                                        curSel = tab.PlayersColumn.CurrentSelection;
                                        tab.PlayersColumn.Items[tab.PlayersColumn.CurrentSelection].Selected = false;
                                        break;
                                    case "missions":
                                        curSel = tab.MissionsColumn.CurrentSelection;
                                        tab.MissionsColumn.Items[tab.MissionsColumn.CurrentSelection].Selected = false;
                                        break;
                                }
                            }
                            tab.updateFocus(context, true);
                            int index = tab.listCol[tab.Focus].Pagination.GetMenuIndexFromScaleformIndex(itemId);
                            switch (tab.listCol[tab.Focus].Type)
                            {
                                case "settings":
                                    tab.SettingsColumn.CurrentSelection = index;
                                    break;
                                case "players":
                                    tab.PlayersColumn.CurrentSelection = index;
                                    break;
                                case "missions":
                                    tab.MissionsColumn.CurrentSelection = index;
                                    break;
                            }
                            if (curSel != index) Game.PlaySound(AUDIO_UPDOWN, AUDIO_LIBRARY);

                            if (foc == tab.Focus && curSel == index)
                                Select(false);
                            return;
                        }
                        switch (context)
                        {
                            case 0: // going from unfocused to focused or playerListTab player selected
                                FocusLevel = 1;
                                if (Tabs[Index] is not PlayerListTab)
                                {
                                    if (Tabs[Index].LeftItemList.All(x => !x.Enabled)) break;
                                    Tabs[Index].LeftItemList[LeftItemIndex].Selected = true;
                                    while (!Tabs[Index].LeftItemList[leftItemIndex].Enabled)
                                    {
                                        await BaseScript.Delay(0);
                                        LeftItemIndex++;
                                        _pause._pause.CallFunction("SELECT_LEFT_ITEM_INDEX", leftItemIndex);
                                    }
                                }
                                else
                                {
                                    PlayerListTab _tab = Tabs[Index] as PlayerListTab;
                                    switch (_tab.listCol[_tab.Focus].Type)
                                    {
                                        case "settings":
                                            _tab.SettingsColumn.Items[_tab.SettingsColumn.CurrentSelection].Selected = true;
                                            break;
                                        case "players":
                                            _tab.PlayersColumn.Items[_tab.PlayersColumn.CurrentSelection].Selected = true;
                                            if (_tab.PlayersColumn.Items[_tab.PlayersColumn.CurrentSelection].ClonePed != null)
                                                _tab.PlayersColumn.Items[_tab.PlayersColumn.CurrentSelection].CreateClonedPed();
                                            break;
                                        case "missions":
                                            _tab.MissionsColumn.Items[_tab.MissionsColumn.CurrentSelection].Selected = true;
                                            break;
                                    }
                                    if (_tab.listCol.Any(x => x.Type == "players"))
                                        SetPauseMenuPedLighting(FocusLevel != 0);
                                }
                                break;
                            case 1: // left item in subitem tab pressed or playerListTab item selected
                                if (Tabs[Index] is not PlayerListTab)
                                {
                                    if (FocusLevel != 1)
                                    {
                                        if (!Tabs[Index].LeftItemList[LeftItemIndex].Enabled)
                                        {
                                            Game.PlaySound(AUDIO_ERROR, AUDIO_LIBRARY);
                                            return;
                                        }
                                        FocusLevel = 1;
                                    }
                                    else if (focusLevel == 1)
                                    {
                                        if (!Tabs[Index].LeftItemList[LeftItemIndex].Enabled)
                                        {
                                            Game.PlaySound(AUDIO_ERROR, AUDIO_LIBRARY);
                                            return;
                                        }
                                        if (Tabs[Index].LeftItemList[LeftItemIndex].ItemType == LeftItemType.Settings)
                                        {
                                            FocusLevel = 2;
                                            _pause._pause.CallFunction("SELECT_RIGHT_ITEM_INDEX", 0);
                                            RightItemIndex = 0;
                                        }
                                    }
                                    LeftItemIndex = itemId;
                                    _pause._pause.CallFunction("SELECT_LEFT_ITEM_INDEX", itemId);
                                    Tabs[Index].LeftItemList[LeftItemIndex].Activated();
                                    SendPauseMenuLeftItemSelect();
                                }
                                break;
                            case 2:// right settings item in subitem tab pressed
                                if (!(Tabs[Index].LeftItemList[leftItemIndex].ItemList[itemId] as SettingsItem).Enabled)
                                {
                                    Game.PlaySound(AUDIO_ERROR, AUDIO_LIBRARY);
                                    return;
                                }

                                if (FocusLevel != 2)
                                    FocusLevel = 2;
                                if (Tabs[Index].LeftItemList[leftItemIndex].ItemList[itemId] is SettingsItem)
                                {
                                    //(Tabs[Index].LeftItemList[leftItemIndex].ItemList[RightItemIndex] as SettingsTabItem).Activated();
                                    if ((Tabs[Index].LeftItemList[leftItemIndex].ItemList[itemId] as SettingsItem).Selected)
                                    {
                                        SettingsItem item = (Tabs[Index].LeftItemList[leftItemIndex].ItemList[RightItemIndex] as SettingsItem);
                                        switch (item.ItemType)
                                        {
                                            case SettingsItemType.ListItem:
                                                (item as SettingsListItem).ListSelected();
                                                break;
                                            case SettingsItemType.CheckBox:
                                                (item as SettingsCheckboxItem).IsChecked = !(item as SettingsCheckboxItem).IsChecked!;
                                                break;
                                            case SettingsItemType.MaskedProgressBar:
                                            case SettingsItemType.ProgressBar:
                                                (item as SettingsProgressItem).ProgressSelected();
                                                break;
                                            case SettingsItemType.SliderBar:
                                                (item as SettingsSliderItem).SliderSelected();
                                                break;
                                            default:
                                                item.Activated();
                                                break;
                                        }
                                        SendPauseMenuRightItemSelect();
                                        return;
                                    }
                                    (Tabs[Index].LeftItemList[leftItemIndex].ItemList[RightItemIndex] as SettingsItem).Selected = false;
                                    RightItemIndex = itemId;
                                    _pause._pause.CallFunction("SELECT_RIGHT_ITEM_INDEX", itemId);
                                    (Tabs[Index].LeftItemList[leftItemIndex].ItemList[RightItemIndex] as SettingsItem).Selected = true;
                                }

                                break;
                        }
                        break;
                    case 6: // on click released
                        break;
                    case 7: // on click released ouside
                        break;
                    case 0: // dragged outside
                    case 8: // on not hover
                        {
                            if (Tabs[Index] is PlayerListTab plTab)
                            {
                                if (FocusLevel == 1)
                                {
                                    int index = plTab.listCol[context].Pagination.GetMenuIndexFromScaleformIndex(itemId);
                                    switch (plTab.listCol[context].Type)
                                    {
                                        case "settings":
                                            plTab.SettingsColumn.Items[index].Hovered = false;
                                            break;
                                        case "players":
                                            plTab.PlayersColumn.Items[index].Hovered = false;
                                            break;
                                        case "missions":
                                            plTab.MissionsColumn.Items[index].Hovered = false;
                                            break;
                                    }
                                    return;
                                }
                            }

                            switch (context)
                            {
                                case 1: // left item in subitem tab pressed
                                    Tabs[Index].LeftItemList[itemId].Hovered = false;
                                    break;
                                case 2:// right settings item in subitem tab pressed
                                    BasicTabItem curIt = Tabs[Index].LeftItemList[LeftItemIndex].ItemList[itemId];
                                    if (curIt is SettingsItem)
                                    {
                                        (curIt as SettingsItem).Hovered = false;
                                    }
                                    break;
                            }
                        }
                        break;
                    case 9: // on hovered
                        {
                            if (Tabs[Index] is PlayerListTab plTab)
                            {
                                if (FocusLevel == 1)
                                {
                                    int index = plTab.listCol[context].Pagination.GetMenuIndexFromScaleformIndex(itemId);
                                    switch (plTab.listCol[context].Type)
                                    {
                                        case "settings":
                                            plTab.SettingsColumn.Items[index].Hovered = true;
                                            break;
                                        case "players":
                                            plTab.PlayersColumn.Items[index].Hovered = true;
                                            break;
                                        case "missions":
                                            plTab.MissionsColumn.Items[index].Hovered = true;
                                            break;
                                    }
                                    return;
                                }
                            }

                            switch (context)
                            {
                                case 1: // left item in subitem tab pressed
                                    foreach (TabLeftItem item in Tabs[Index].LeftItemList)
                                        item.Hovered = Tabs[Index].LeftItemList.IndexOf(item) == itemId && item.Enabled;
                                    break;
                                case 2:// right settings item in subitem tab pressed
                                    foreach (BasicTabItem curIt in Tabs[Index].LeftItemList[LeftItemIndex].ItemList)
                                    {
                                        int idx = Tabs[Index].LeftItemList[LeftItemIndex].ItemList.IndexOf(curIt);
                                        if (curIt is SettingsItem)
                                        {
                                            (curIt as SettingsItem).Hovered = itemId == idx && (curIt as SettingsItem).Enabled;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case 1: // dragged inside
                        break;
                }
            }
        }

        public override async void ProcessControls()
        {
            if (firstTick)
            {
                firstTick = false;
                return;
                // without this shit the menu goes on focus without need if opened from another menu.
            }

            if (!Visible || TemporarilyHidden) return;

            if (Game.IsControlJustPressed(2, Control.PhoneUp))
                GoUp();
            else if (Game.IsControlJustPressed(2, Control.PhoneDown))
                GoDown();
            else if (Game.IsControlJustPressed(2, Control.PhoneLeft))
                GoLeft();
            else if (Game.IsControlJustPressed(2, Control.PhoneRight))
                GoRight();
            else if (Game.IsControlJustPressed(2, Control.FrontendLb) || (Game.IsControlJustPressed(2, (Control)192) && Game.IsControlPressed(2, Control.Sprint) && IsUsingKeyboard(2)))
            {
                if (FocusLevel > 0) GoBack();
                GoLeft();
            }
            else if (Game.IsControlJustPressed(2, Control.FrontendRb) || (Game.IsControlJustPressed(2, (Control)192) && IsUsingKeyboard(2)))
            {
                if (FocusLevel > 0) GoBack();
                GoRight();
            }
            else if (Game.IsControlJustPressed(2, Control.FrontendAccept))
                Select(false);
            else if (Game.IsControlJustReleased(2, Control.PhoneCancel))
                GoBack();

            if (Game.IsControlJustPressed(1, Control.CursorScrollUp))
                _pause.SendScrollEvent(-1);
            else if (Game.IsControlJustPressed(1, Control.CursorScrollDown))
                _pause.SendScrollEvent(1);

            if (Game.IsControlPressed(2, Control.LookUpOnly) && !IsUsingKeyboard(2))
            {
                if (Main.GameTime - _timer > 175)
                {
                    _pause.SendScrollEvent(-1);
                    _timer = Main.GameTime;
                }
            }
            else if (Game.IsControlPressed(2, Control.LookDownOnly) && !IsUsingKeyboard(2))
            {
                if (Main.GameTime - _timer > 175)
                {
                    _pause.SendScrollEvent(1);
                    _timer = Main.GameTime;
                }
            }
        }

        internal void SendPauseMenuOpen()
        {
            OnPauseMenuOpen?.Invoke(this);
        }

        internal void SendPauseMenuClose()
        {
            OnPauseMenuClose?.Invoke(this);
        }

        internal void SendPauseMenuTabChange()
        {
            OnPauseMenuTabChanged?.Invoke(this, Tabs[Index], Index);
        }

        internal void SendPauseMenuFocusChange()
        {
            OnPauseMenuFocusChanged?.Invoke(this, Tabs[Index], FocusLevel);
        }

        internal void SendPauseMenuLeftItemChange()
        {
            OnLeftItemChange?.Invoke(this, Tabs[Index].LeftItemList[LeftItemIndex], LeftItemIndex);
        }

        internal void SendPauseMenuLeftItemSelect()
        {
            OnLeftItemSelect?.Invoke(this, Tabs[Index].LeftItemList[LeftItemIndex], LeftItemIndex);
        }
        internal void SendPauseMenuRightItemChange()
        {
            OnRightItemChange?.Invoke(this, Tabs[Index].LeftItemList[LeftItemIndex].ItemList[RightItemIndex] as SettingsItem, LeftItemIndex, RightItemIndex);
        }
        internal void SendPauseMenuRightItemSelect()
        {
            OnRightItemSelect?.Invoke(this, Tabs[Index].LeftItemList[LeftItemIndex].ItemList[RightItemIndex] as SettingsItem, LeftItemIndex, RightItemIndex);
        }
    }
}