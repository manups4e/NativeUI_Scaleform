using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.FiveM;
using ScaleformUI.Menu;
using ScaleformUI.PauseMenus;
using ScaleformUI.Scaleforms;
using static CitizenFX.FiveM.Native.Natives;

namespace ScaleformUI.LobbyMenu
{
    public delegate void LobbyMenuOpenEvent(MainView menu);
    public delegate void LobbyMenuCloseEvent(MainView menu);

    public class MainView : PauseMenuBase
    {
        // Button delay

        public string AUDIO_LIBRARY = "HUD_FRONTEND_DEFAULT_SOUNDSET";
        private bool isBuilding = false;
        public string AUDIO_UPDOWN = "NAV_UP_DOWN";
        public string AUDIO_LEFTRIGHT = "NAV_LEFT_RIGHT";
        public string AUDIO_SELECT = "SELECT";
        public string AUDIO_BACK = "BACK";
        public string AUDIO_ERROR = "ERROR";
        public bool ForceFirstSelectionOnFocus { get; set; }
        private int time;
        private bool _firstDrawTick = true;
        private int times;
        private int delay = 150;
        internal bool _newStyle;
        internal List<Column> listCol;
        internal PauseMenuScaleform _pause;
        internal bool _loaded;
        internal readonly static string _browseTextLocalized = Game.GetGXTEntry("HUD_INPUT1C");
        public event LobbyMenuOpenEvent OnLobbyMenuOpen;
        public event LobbyMenuCloseEvent OnLobbyMenuClose;
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string SideStringTop { get; set; }
        public string SideStringMiddle { get; set; }
        public string SideStringBottom { get; set; }
        public Tuple<string, string> HeaderPicture { internal get; set; }
        public Tuple<string, string> CrewPicture { internal get; set; }
        public SettingsListColumn SettingsColumn { get; private set; }
        public MissionsListColumn MissionsColumn { get; private set; }
        public PlayerListColumn PlayersColumn { get; private set; }
        public MissionDetailsPanel MissionPanel { get; private set; }
        public int FocusLevel
        {
            get => focusLevel;
        }

        public bool TemporarilyHidden { get; set; }
        public bool HideTabs { get; set; }
        public bool DisplayHeader = true;

        public MainView(string title, bool newStyle = true) : this(title, "", "", "", "", newStyle)
        {
        }
        public MainView(string title, string subtitle, bool newStyle = true) : this(title, subtitle, "", "", "", newStyle)
        {
        }

        public MainView(string title, string subtitle, string sideTop, string sideMid, string sideBot, bool newStyle = true)
        {
            Title = title;
            SubTitle = subtitle;
            SideStringTop = sideTop;
            SideStringMiddle = sideMid;
            SideStringBottom = sideBot;
            Index = 0;
            TemporarilyHidden = false;
            InstructionalButtons = new()
            {
                new InstructionalButton(Control.PhoneSelect, UIMenu._selectTextLocalized),
                new InstructionalButton(Control.PhoneCancel, UIMenu._backTextLocalized),
            };
            _newStyle = newStyle;
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
                    BuildPauseMenu();
                    SendPauseMenuOpen();
                    AnimpostfxPlay("PauseMenuIn", 800, true);
                    Main.InstructionalButtons.SetInstructionalButtons(InstructionalButtons);
                    SetPlayerControl(Game.Player.Handle, false, 0);
                    _firstDrawTick = true;
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
                    ActivateFrontendMenu((uint)Game.GenerateHash("FE_MENU_VERSION_EMPTY_NO_BACKGROUND"), false, -1);
                    Main.InstructionalButtons.ClearButtonList();
                }
                base.Visible = value;
                _visible = value;
                _pause.Visible = value;
            }
        }

        public int Index;
        private bool _visible;
        private int focusLevel;

        public void SelectColumn(Column column)
        {
            int f = column.Order;
            SelectColumn(f);
        }
        public void SelectColumn(int column)
        {
            int f = column;
            if (f < 0)
                f = listCol.Count - 1;
            else if (f > listCol.Count - 1)
                f = 0;
            updateFocus(f);
        }

        private async void updateFocus(int value, bool isMouse = false)
        {
            bool goingLeft = value < focusLevel;
            int f = value;
            if (f < 0)
                f = listCol.Count - 1;
            else if (f > listCol.Count - 1)
                f = 0;
            if (listCol[f].Type != "players")
            {
                if (PlayersColumn != null && PlayersColumn.Items.Count > 0 && !PlayersColumn.Items[PlayersColumn.CurrentSelection].KeepPanelVisible)
                    ClearPedInPauseMenu();
            }
            focusLevel = f;
            if (listCol[focusLevel].Type == "panel")
            {
                if (goingLeft)
                    updateFocus(focusLevel - 1, isMouse);
                else
                    updateFocus(focusLevel + 1, isMouse);
                return;
            }
            if (Visible)
            {
                int idx = await _pause._lobby.CallFunctionReturnValueInt("SET_FOCUS", focusLevel);
                if (!isMouse)
                {
                    switch (listCol[FocusLevel].Type)
                    {
                        case "players":
                            PlayersColumn.CurrentSelection = PlayersColumn.Pagination.GetMenuIndexFromScaleformIndex(ForceFirstSelectionOnFocus ? 0 : idx);
                            if (!goingLeft || _newStyle)
                                PlayersColumn.IndexChangedEvent();
                            break;
                        case "settings":
                            SettingsColumn.CurrentSelection = SettingsColumn.Pagination.GetMenuIndexFromScaleformIndex(ForceFirstSelectionOnFocus ? 0 : idx);
                            if (!goingLeft || _newStyle)
                                SettingsColumn.IndexChangedEvent();
                            break;
                        case "missions":
                            MissionsColumn.CurrentSelection = MissionsColumn.Pagination.GetMenuIndexFromScaleformIndex(ForceFirstSelectionOnFocus ? 0 : idx);
                            if (!goingLeft || _newStyle)
                                MissionsColumn.IndexChangedEvent();
                            break;
                    }
                }
            }
        }

        public void SetUpColumns(List<Column> columns)
        {
            if (columns.Count > 3)
                throw new Exception("You must have 3 columns!");
            if (columns.Count == 3 && columns[2] is PlayerListColumn)
                throw new Exception("For panel designs reasons, you can't have Players list in 3rd column!");

            listCol = columns;
            foreach (Column col in columns)
            {
                switch (col)
                {
                    case SettingsListColumn:
                        SettingsColumn = col as SettingsListColumn;
                        SettingsColumn.Parent = this;
                        SettingsColumn.Order = columns.IndexOf(col);
                        break;
                    case PlayerListColumn:
                        PlayersColumn = col as PlayerListColumn;
                        PlayersColumn.Parent = this;
                        PlayersColumn.Order = columns.IndexOf(col);
                        break;
                    case MissionsListColumn:
                        MissionsColumn = col as MissionsListColumn;
                        MissionsColumn.Parent = this;
                        MissionsColumn.Order = columns.IndexOf(col);
                        break;
                    case MissionDetailsPanel:
                        MissionPanel = col as MissionDetailsPanel;
                        MissionPanel.Parent = this;
                        MissionPanel.Order = columns.IndexOf(col);
                        break;
                }
            }
        }

        public void ShowHeader()
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
            _pause.AddLobbyMenuTab(listCol[0].Label, 2, listCol[0].Color);
            _pause.AddLobbyMenuTab(listCol[1].Label, 2, listCol[1].Color);
            _pause.AddLobbyMenuTab(listCol[2].Label, 2, listCol[2].Color);
            _pause._header.CallFunction("SET_ALL_HIGHLIGHTS", true, (int)HudColor.HUD_COLOUR_PAUSE_BG);

            _loaded = true;
        }

        private bool canBuild = true;
        public async void BuildPauseMenu()
        {
            isBuilding = true;
            ShowHeader();
            switch (listCol.Count)
            {
                case 1:
                    _pause._lobby.CallFunction("CREATE_MENU", listCol[0].Type);
                    break;
                case 2:
                    _pause._lobby.CallFunction("CREATE_MENU", listCol[0].Type, listCol[1].Type);
                    break;
                case 3:
                    _pause._lobby.CallFunction("CREATE_MENU", listCol[0].Type, listCol[1].Type, listCol[2].Type);
                    break;
            }
            _pause._lobby.CallFunction("SET_NEWSTYLE", _newStyle);
            if (listCol.Any(x => x is PlayerListColumn))
                buildPlayers();

            if (listCol.Any(x => x is SettingsListColumn))
                buildSettings();

            if (listCol.Any(x => x is MissionsListColumn))
                buildMissions();

            if (listCol.Any(x => x is MissionDetailsPanel))
            {
                _pause._lobby.CallFunction("ADD_MISSION_PANEL_PICTURE", MissionPanel.TextureDict, MissionPanel.TextureName);
                _pause._lobby.CallFunction("SET_MISSION_PANEL_TITLE", MissionPanel.Title);
                if (MissionPanel.Items.Count > 0)
                {
                    foreach (UIFreemodeDetailsItem item in MissionPanel.Items)
                    {
                        _pause._lobby.CallFunction("ADD_MISSION_PANEL_ITEM", item.Type, item.TextLeft, item.TextRight, (int)item.Icon, item.IconColor, item.Tick, item._labelFont.FontName, item._labelFont.FontID, item._rightLabelFont.FontName, item._rightLabelFont.FontID);
                    }
                }
            }

            _pause._lobby.CallFunction("LOAD_MENU");
            while ((SettingsColumn != null && SettingsColumn.isBuilding) || (PlayersColumn != null && PlayersColumn.isBuilding) || (MissionsColumn != null && MissionsColumn.isBuilding)) await BaseScript.Delay(0);

            isBuilding = false;
        }

        internal async void buildSettings()
        {
            if (SettingsColumn.Items.Count > 0)
            {
                SettingsColumn.isBuilding = true;
                int i = 0;
                int max = SettingsColumn.Pagination.ItemsPerPage;
                if (SettingsColumn.Items.Count < max)
                    max = SettingsColumn.Items.Count;

                SettingsColumn.Pagination.MinItem = SettingsColumn.Pagination.CurrentPageStartIndex;
                if (SettingsColumn.Pagination.scrollType == ScrollingType.CLASSIC && SettingsColumn.Pagination.TotalPages > 1)
                {
                    int missingItems = SettingsColumn.Pagination.GetMissingItems();
                    if (missingItems > 0)
                    {
                        SettingsColumn.Pagination.ScaleformIndex = SettingsColumn.Pagination.GetPageIndexFromMenuIndex(SettingsColumn.Pagination.CurrentPageEndIndex) + missingItems;
                        SettingsColumn.Pagination.MinItem = SettingsColumn.Pagination.CurrentPageStartIndex - missingItems;
                    }
                }
                SettingsColumn.Pagination.MaxItem = SettingsColumn.Pagination.CurrentPageEndIndex;

                while (i < max)
                {
                    await BaseScript.Delay(0);
                    if (!Visible) return;
                    SettingsColumn._itemCreation(SettingsColumn.Pagination.CurrentPage, i, false, true);
                    i++;
                }
                SettingsColumn.CurrentSelection = 0;
                SettingsColumn.Pagination.ScaleformIndex = SettingsColumn.Pagination.GetScaleformIndex(SettingsColumn.CurrentSelection);
                SettingsColumn.Items[0].Selected = true;
                _pause._lobby.CallFunction("SET_SETTINGS_SELECTION", SettingsColumn.Pagination.ScaleformIndex);
                _pause._lobby.CallFunction("SET_SETTINGS_QTTY", SettingsColumn.CurrentSelection + 1, SettingsColumn.Items.Count);
                SettingsColumn.isBuilding = false;
            }

        }

        internal async void buildPlayers()
        {
            if (PlayersColumn.Items.Count > 0)
            {
                PlayersColumn.isBuilding = true;
                int i = 0;
                int max = PlayersColumn.Pagination.ItemsPerPage;
                if (PlayersColumn.Items.Count < max)
                    max = PlayersColumn.Items.Count;

                PlayersColumn.Pagination.MinItem = PlayersColumn.Pagination.CurrentPageStartIndex;
                if (PlayersColumn.Pagination.scrollType == ScrollingType.CLASSIC && PlayersColumn.Pagination.TotalPages > 1)
                {
                    int missingItems = PlayersColumn.Pagination.GetMissingItems();
                    if (missingItems > 0)
                    {
                        PlayersColumn.Pagination.ScaleformIndex = PlayersColumn.Pagination.GetPageIndexFromMenuIndex(PlayersColumn.Pagination.CurrentPageEndIndex) + missingItems;
                        PlayersColumn.Pagination.MinItem = PlayersColumn.Pagination.CurrentPageStartIndex - missingItems;
                    }
                }
                PlayersColumn.Pagination.MaxItem = PlayersColumn.Pagination.CurrentPageEndIndex;

                while (i < max)
                {
                    await BaseScript.Delay(0);
                    if (!Visible) return;
                    PlayersColumn._itemCreation(PlayersColumn.Pagination.CurrentPage, i, false, true);
                    i++;
                }
                PlayersColumn.CurrentSelection = 0;
                PlayersColumn.Pagination.ScaleformIndex = PlayersColumn.Pagination.GetScaleformIndex(PlayersColumn.CurrentSelection);
                PlayersColumn.Items[0].Selected = true;
                _pause._lobby.CallFunction("SET_PLAYERS_SELECTION", PlayersColumn.Pagination.ScaleformIndex);
                _pause._lobby.CallFunction("SET_PLAYERS_QTTY", PlayersColumn.CurrentSelection + 1, PlayersColumn.Items.Count);
                PlayersColumn.isBuilding = false;
            }
        }

        internal async void buildMissions()
        {
            if (MissionsColumn.Items.Count > 0)
            {
                MissionsColumn.isBuilding = true;
                int i = 0;
                int max = MissionsColumn.Pagination.ItemsPerPage;
                if (MissionsColumn.Items.Count < max)
                    max = MissionsColumn.Items.Count;

                MissionsColumn.Pagination.MinItem = MissionsColumn.Pagination.CurrentPageStartIndex;
                if (MissionsColumn.Pagination.scrollType == ScrollingType.CLASSIC && MissionsColumn.Pagination.TotalPages > 1)
                {
                    int missingItems = MissionsColumn.Pagination.GetMissingItems();
                    if (missingItems > 0)
                    {
                        MissionsColumn.Pagination.ScaleformIndex = MissionsColumn.Pagination.GetPageIndexFromMenuIndex(MissionsColumn.Pagination.CurrentPageEndIndex) + missingItems;
                        MissionsColumn.Pagination.MinItem = MissionsColumn.Pagination.CurrentPageStartIndex - missingItems;
                    }
                }
                MissionsColumn.Pagination.MaxItem = MissionsColumn.Pagination.CurrentPageEndIndex;

                while (i < max)
                {
                    await BaseScript.Delay(0);
                    if (!Visible) return;
                    MissionsColumn._itemCreation(MissionsColumn.Pagination.CurrentPage, i, false, true);
                    i++;
                }
                MissionsColumn.CurrentSelection = 0;
                MissionsColumn.Pagination.ScaleformIndex = MissionsColumn.Pagination.GetScaleformIndex(MissionsColumn.CurrentSelection);
                MissionsColumn.Items[0].Selected = true;
                _pause._lobby.CallFunction("SET_MISSIONS_SELECTION", MissionsColumn.Pagination.ScaleformIndex);
                _pause._lobby.CallFunction("SET_MISSIONS_QTTY", MissionsColumn.CurrentSelection + 1, MissionsColumn.Items.Count);
                MissionsColumn.isBuilding = false;
            }

        }

        private bool controller = false;
        public override async void Draw()
        {
            if (!Visible || TemporarilyHidden || isBuilding) return;
            base.Draw();
            _pause.Draw(true);
            if (_firstDrawTick)
            {
                _pause._lobby.CallFunction("FADE_IN");
                _firstDrawTick = false;
            }
        }

        private bool eventBool = false;
        private int eventType = 0;
        private int itemId = 0;
        private int context = 0;
        //private int unused = 0;
        private bool cursorPressed;
        public override async void ProcessMouse()
        {
            if (!IsInputDisabled(2))
            {
                return;
            }
            SetMouseCursorActiveThisFrame();
            SetInputExclusive(2, 239);
            SetInputExclusive(2, 240);
            SetInputExclusive(2, 237);
            SetInputExclusive(2, 238);

            bool success = GetScaleformMovieCursorSelection(_pause._lobby.Handle, ref eventBool, ref eventType, ref context, ref itemId);
            if (success)
            {
                switch (eventType)
                {
                    case 5:
                        {
                            int foc = FocusLevel;
                            int curSel = 0;
                            if (_newStyle)
                            {
                                switch (listCol[foc].Type)
                                {
                                    case "settings":
                                        curSel = SettingsColumn.CurrentSelection;
                                        SettingsColumn.Items[SettingsColumn.CurrentSelection].Selected = false;
                                        break;
                                    case "players":
                                        curSel = PlayersColumn.CurrentSelection;
                                        PlayersColumn.Items[PlayersColumn.CurrentSelection].Selected = false;
                                        break;
                                    case "missions":
                                        curSel = MissionsColumn.CurrentSelection;
                                        MissionsColumn.Items[MissionsColumn.CurrentSelection].Selected = false;
                                        break;
                                }
                            }
                            updateFocus(context, true);
                            int index = listCol[FocusLevel].Pagination.GetMenuIndexFromScaleformIndex(itemId);
                            switch (listCol[FocusLevel].Type)
                            {
                                case "settings":
                                    SettingsColumn.CurrentSelection = index;
                                    break;
                                case "players":
                                    PlayersColumn.CurrentSelection = index;
                                    break;
                                case "missions":
                                    MissionsColumn.CurrentSelection = index;
                                    break;
                            }
                            if (curSel != index) Game.PlaySound(AUDIO_UPDOWN, AUDIO_LIBRARY);

                            if (foc == FocusLevel && curSel == index)
                                Select();
                        }
                        break;
                    case 8:
                        {
                            int index = listCol[context].Pagination.GetMenuIndexFromScaleformIndex(itemId);
                            switch (listCol[context].Type)
                            {
                                case "settings":
                                    SettingsColumn.Items[itemId].Hovered = false;
                                    break;
                                case "players":
                                    PlayersColumn.Items[itemId].Hovered = false;
                                    break;
                                case "missions":
                                    MissionsColumn.Items[itemId].Hovered = false;
                                    break;
                            }
                        }
                        break;
                    case 9:
                        {
                            int index = listCol[context].Pagination.GetMenuIndexFromScaleformIndex(itemId);
                            switch (listCol[context].Type)
                            {
                                case "settings":
                                    SettingsColumn.Items[itemId].Hovered = true;
                                    break;
                                case "players":
                                    PlayersColumn.Items[itemId].Hovered = true;
                                    break;
                                case "missions":
                                    MissionsColumn.Items[itemId].Hovered = true;
                                    break;
                            }
                        }
                        break;
                }
            }
        }

        public override async void ProcessControls()
        {
            if (!Visible || TemporarilyHidden) return;
            if (Game.IsControlPressed(2, Control.PhoneUp))
            {
                if (Main.GameTime - time > delay)
                {
                    ButtonDelay();
                    GoUp();
                }
            }
            else if (Game.IsControlPressed(2, Control.PhoneDown))
            {
                if (Main.GameTime - time > delay)
                {
                    ButtonDelay();
                    GoDown();
                }
            }

            else if (Game.IsControlPressed(2, Control.PhoneLeft))
            {
                if (Main.GameTime - time > delay)
                {
                    ButtonDelay();
                    GoLeft();
                }
            }
            else if (Game.IsControlPressed(2, Control.PhoneRight))
            {
                if (Main.GameTime - time > delay)
                {
                    ButtonDelay();
                    GoRight();
                }
            }

            else if (Game.IsControlJustPressed(2, Control.FrontendAccept))
            {
                Select();
            }

            else if (Game.IsControlJustReleased(2, Control.PhoneCancel))
            {
                GoBack();
            }

            if (Game.IsControlJustPressed(1, Control.CursorScrollUp))
            {
                _pause.SendScrollEvent(-1);
            }
            else if (Game.IsControlJustPressed(1, Control.CursorScrollDown))
            {
                _pause.SendScrollEvent(1);
            }

            // IsControlBeingPressed doesn't run every frame so I had to use this
            if (!Game.IsControlPressed(2, Control.PhoneUp) && !Game.IsControlPressed(2, Control.PhoneDown) && !Game.IsControlPressed(2, Control.PhoneLeft) && !Game.IsControlPressed(2, Control.PhoneRight))
            {
                times = 0;
                delay = 150;
            }
        }

        public async void Select()
        {
            string result = await _pause._lobby.CallFunctionReturnValueString("SET_INPUT_EVENT", 16);

            int[] split = result.Split(',').Select(int.Parse).ToArray();
            switch (listCol[FocusLevel].Type)
            {
                case "settings":
                    UIMenuItem item = SettingsColumn.Items[SettingsColumn.CurrentSelection];
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
                                SettingsColumn.SelectItem();
                                break;
                            }

                        case UIMenuListItem:
                            {
                                UIMenuListItem it = item as UIMenuListItem;
                                it.ListSelectedTrigger(it.Index);
                                SettingsColumn.SelectItem();
                                break;
                            }

                        default:
                            item.ItemActivate(null);
                            SettingsColumn.SelectItem();
                            break;
                    }
                    _pause._lobby.CallFunction("SET_INPUT_EVENT", 16);
                    break;
                case "missions":
                    MissionItem mitem = MissionsColumn.Items[MissionsColumn.CurrentSelection];
                    mitem.ActivateMission(null);
                    MissionsColumn.SelectItem();
                    break;
                case "players":
                    PlayersColumn.SelectItem();
                    break;
            }
        }

        public async void GoBack()
        {
            if (_newStyle)
            {
                if (CanPlayerCloseMenu)
                    Visible = false;
            }
            else
            {
                switch (listCol[FocusLevel].Type)
                {
                    case "settings":
                        SettingsColumn.Items[SettingsColumn.CurrentSelection].Selected = false;
                        break;
                    case "players":
                        PlayersColumn.Items[PlayersColumn.CurrentSelection].Selected = false;
                        ClearPedInPauseMenu();
                        break;
                    case "missions":
                        MissionsColumn.Items[MissionsColumn.CurrentSelection].Selected = false;
                        break;
                }
                if (FocusLevel == 0)
                {
                    if (CanPlayerCloseMenu)
                        Visible = false;
                    return;
                }
                updateFocus(FocusLevel - 1);
            }
        }

        public async void GoUp()
        {
            if (listCol[FocusLevel].Type == "settings")
                SettingsColumn.GoUp();
            else if (listCol[FocusLevel].Type == "missions")
                MissionsColumn.GoUp();
            else if (listCol[FocusLevel].Type == "players")
                PlayersColumn.GoUp();
        }

        public async void GoDown()
        {
            if (listCol[FocusLevel].Type == "settings")
                SettingsColumn.GoDown();
            else if (listCol[FocusLevel].Type == "missions")
                MissionsColumn.GoDown();
            else if (listCol[FocusLevel].Type == "players")
                PlayersColumn.GoDown();
        }

        public async void GoLeft()
        {
            string result = await _pause._lobby.CallFunctionReturnValueString("SET_INPUT_EVENT", 10);

            int[] split = result.Split(',').Select(int.Parse).ToArray();

            if (_newStyle)
            {
                if (listCol.Any(x => x.Type == "settings"))
                {
                    UIMenuItem item = SettingsColumn.Items[SettingsColumn.CurrentSelection];
                    if (item is not UIMenuListItem && item is not UIMenuSliderItem && item is not UIMenuProgressItem)
                    {
                        item.Selected = false;
                    }
                }
                if (listCol.Any(x => x.Type == "missions"))
                    MissionsColumn.Items[MissionsColumn.CurrentSelection].Selected = false;
                if (listCol.Any(x => x.Type == "players"))
                {
                    PlayersColumn.Items[PlayersColumn.CurrentSelection].Selected = false;
                    if (listCol[0].Type == "players" || PlayersColumn.Items[PlayersColumn.CurrentSelection].KeepPanelVisible)
                    {
                        if (PlayersColumn.Items[PlayersColumn.CurrentSelection].ClonePed != null)
                            PlayersColumn.Items[PlayersColumn.CurrentSelection].CreateClonedPed();
                        else
                            ClearPedInPauseMenu();
                    }
                    else
                        ClearPedInPauseMenu();
                }
                else
                    ClearPedInPauseMenu();
            }

            switch (listCol[focusLevel].Type)
            {
                case "settings":
                    {
                        UIMenuItem item = SettingsColumn.Items[SettingsColumn.CurrentSelection];
                        if (!item.Enabled)
                        {
                            if (_newStyle)
                            {
                                SettingsColumn.Items[SettingsColumn.CurrentSelection].Selected = false;
                                updateFocus(focusLevel - 1);
                            }
                            else
                            {
                                Game.PlaySound("ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                            }
                            return;
                        }

                        if (item is UIMenuListItem it)
                        {
                            it.Index = split[2];
                            //ListChange(it, it.Index);
                            it.ListChangedTrigger(it.Index);
                        }
                        else if (item is UIMenuSliderItem slit)
                        {
                            slit.Value = split[2];
                            slit.SliderChanged(slit.Value);
                            //SliderChange(it, it.Value);
                        }
                        else if (item is UIMenuProgressItem prit)
                        {
                            prit.Value = split[2];
                            prit.ProgressChanged(prit.Value);
                            //ProgressChange(it, it.Value);
                        }
                        else
                        {
                            if (_newStyle)
                            {
                                SettingsColumn.Items[SettingsColumn.CurrentSelection].Selected = false;
                                updateFocus(focusLevel - 1);
                            }
                        }
                    }
                    break;
                case "missions":
                    if (_newStyle)
                    {
                        MissionsColumn.Items[MissionsColumn.CurrentSelection].Selected = false;
                        updateFocus(focusLevel - 1);
                    }
                    break;
                case "panel":
                    updateFocus(focusLevel - 1);
                    break;
                case "players":
                    if (_newStyle)
                    {
                        PlayersColumn.Items[PlayersColumn.CurrentSelection].Selected = false;
                        updateFocus(focusLevel - 1);
                    }
                    else
                    {
                        if (PlayersColumn.Items[PlayersColumn.CurrentSelection].ClonePed != null)
                            PlayersColumn.Items[PlayersColumn.CurrentSelection].CreateClonedPed();
                    }
                    break;
            }
        }

        public async void GoRight()
        {
            string result = await _pause._lobby.CallFunctionReturnValueString("SET_INPUT_EVENT", 11);

            int[] split = result.Split(',').Select(int.Parse).ToArray();

            if (_newStyle)
            {
                if (listCol.Any(x => x.Type == "settings"))
                {
                    UIMenuItem item = SettingsColumn.Items[SettingsColumn.CurrentSelection];
                    if (item is not UIMenuListItem && item is not UIMenuSliderItem && item is not UIMenuProgressItem)
                    {
                        item.Selected = false;
                    }
                }
                if (listCol.Any(x => x.Type == "missions"))
                    MissionsColumn.Items[MissionsColumn.CurrentSelection].Selected = false;
                if (listCol.Any(x => x.Type == "players"))
                {
                    PlayersColumn.Items[PlayersColumn.CurrentSelection].Selected = false;
                    if (listCol[0].Type == "players" || PlayersColumn.Items[PlayersColumn.CurrentSelection].KeepPanelVisible)
                    {
                        if (PlayersColumn.Items[PlayersColumn.CurrentSelection].ClonePed != null)
                            PlayersColumn.Items[PlayersColumn.CurrentSelection].CreateClonedPed();
                        else
                            ClearPedInPauseMenu();
                    }
                    else
                        ClearPedInPauseMenu();
                }
                else
                    ClearPedInPauseMenu();
            }
            switch (listCol[focusLevel].Type)
            {
                case "settings":
                    {
                        UIMenuItem item = SettingsColumn.Items[SettingsColumn.CurrentSelection];
                        if (!item.Enabled)
                        {
                            if (_newStyle)
                            {
                                SettingsColumn.Items[SettingsColumn.CurrentSelection].Selected = false;
                                updateFocus(focusLevel + 1);
                            }
                            else
                            {
                                Game.PlaySound("ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                            }
                            return;
                        }

                        if (item is UIMenuListItem it)
                        {
                            it.Index = split[2];
                            //ListChange(it, it.Index);
                            it.ListChangedTrigger(it.Index);
                        }
                        else if (item is UIMenuSliderItem slit)
                        {
                            slit.Value = split[2];
                            slit.SliderChanged(slit.Value);
                            //SliderChange(it, it.Value);
                        }
                        else if (item is UIMenuProgressItem prit)
                        {
                            prit.Value = split[2];
                            prit.ProgressChanged(prit.Value);
                            //ProgressChange(it, it.Value);
                        }
                        else
                        {
                            if (_newStyle)
                            {
                                SettingsColumn.Items[SettingsColumn.CurrentSelection].Selected = false;
                                updateFocus(focusLevel + 1);
                            }
                        }
                    }
                    break;
                case "missions":
                    if (_newStyle)
                    {
                        MissionsColumn.Items[MissionsColumn.CurrentSelection].Selected = false;
                        updateFocus(focusLevel + 1);
                    }
                    break;
                case "panel":
                    updateFocus(focusLevel + 1);
                    break;
                case "players":
                    if (_newStyle)
                    {
                        PlayersColumn.Items[PlayersColumn.CurrentSelection].Selected = false;
                        updateFocus(focusLevel + 1);
                    }
                    else
                    {
                        if (PlayersColumn.Items[PlayersColumn.CurrentSelection].ClonePed != null)
                            PlayersColumn.Items[PlayersColumn.CurrentSelection].CreateClonedPed();
                    }
                    break;
            }
        }

        void ButtonDelay()
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

        internal void SendPauseMenuOpen()
        {
            OnLobbyMenuOpen?.Invoke(this);
        }

        internal void SendPauseMenuClose()
        {
            OnLobbyMenuClose?.Invoke(this);
        }



    }
}