using CitizenFX.Core;
using CitizenFX.Core.Native;
using ScaleformUI.Elements;
using ScaleformUI.Menu;
using ScaleformUI.PauseMenu;
using static CitizenFX.FiveM.Native.Natives;


namespace ScaleformUI.LobbyMenu
{
    public delegate void PlayerItemSelected(LobbyItem item, int index);
    public class PlayerListColumn : Column
    {
        internal bool isBuilding = false;
        public event IndexChanged OnIndexChanged;
        public List<LobbyItem> Items { get; private set; }
        private List<LobbyItem> _unfilteredItems;
        public event PlayerItemSelected OnPlayerItemActivated;

        public ScrollingType ScrollingType { get => Pagination.scrollType; set => Pagination.scrollType = value; }

        public PlayerListColumn(string label, SColor color, ScrollingType scrollType = ScrollingType.CLASSIC) : base(label, color)
        {
            Items = new List<LobbyItem>();
            Type = "players";
            Pagination = new PaginationHandler
            {
                ItemsPerPage = 12,
                scrollType = scrollType
            };
        }

        public void AddPlayer(LobbyItem item)
        {
            item.ParentColumn = this;
            Items.Add(item);
            Pagination.TotalItems = Items.Count;
            if (Parent != null && Parent.Visible)
            {
                if (Pagination.TotalItems <= Pagination.ItemsPerPage)
                {
                    int sel = CurrentSelection;
                    Pagination.MinItem = Pagination.CurrentPageStartIndex;
                    if (Pagination.scrollType == ScrollingType.CLASSIC && Pagination.TotalPages > 1)
                    {
                        int missingItems = Pagination.GetMissingItems();
                        if (missingItems > 0)
                        {
                            Pagination.ScaleformIndex = Pagination.GetPageIndexFromMenuIndex(Pagination.CurrentPageEndIndex) + missingItems;
                            Pagination.MinItem = Pagination.CurrentPageStartIndex - missingItems;
                        }
                    }
                    Pagination.MaxItem = Pagination.CurrentPageEndIndex;
                    _itemCreation(Pagination.CurrentPage, Items.Count - 1, false);
                    if (Parent is TabView pause)
                    {
                        if ((pause.Tabs[ParentTab] as PlayerListTab).listCol[(pause.Tabs[ParentTab] as PlayerListTab).Focus] == this)
                            CurrentSelection = sel;
                    }
                }
            }
        }

        internal void _itemCreation(int page, int pageIndex, bool before, bool isOverflow = false)
        {
            int menuIndex = Pagination.GetMenuIndexFromPageIndex(page, pageIndex);
            if (!before)
            {
                if (Pagination.GetPageItemsCount(page) < Pagination.ItemsPerPage && Pagination.TotalPages > 1)
                {
                    if (Pagination.scrollType == ScrollingType.ENDLESS)
                    {
                        if (menuIndex > Pagination.TotalItems - 1)
                        {
                            menuIndex -= Pagination.TotalItems;
                            Pagination.MaxItem = menuIndex;
                        }
                    }
                    else if (Pagination.scrollType == ScrollingType.CLASSIC && isOverflow)
                    {
                        int missingItems = Pagination.ItemsPerPage - Pagination.GetPageItemsCount(page);
                        menuIndex -= missingItems;
                    }
                    else if (Pagination.scrollType == ScrollingType.PAGINATED)
                        if (menuIndex >= Items.Count) return;
                }
            }

            LobbyItem item = Items[menuIndex];
            switch (item)
            {
                case FriendItem:
                    FriendItem fi = (FriendItem)item;
                    if (Parent is MainView lobby)
                        lobby._pause._lobby.CallFunction("ADD_PLAYER_ITEM", before, menuIndex, 1, 1, fi.Label, fi.ItemColor, fi.ColoredTag, fi.iconL, fi.boolL, fi.iconR, fi.boolR, fi.Status, fi.StatusColor, fi.Rank, fi.CrewTag.TAG, fi.KeepPanelVisible);
                    else if (Parent is TabView pause)
                        pause._pause._pause.CallFunction("ADD_PLAYERS_TAB_PLAYER_ITEM", ParentTab, before, menuIndex, 1, 1, fi.Label, fi.ItemColor, fi.ColoredTag, fi.iconL, fi.boolL, fi.iconR, fi.boolR, fi.Status, fi.StatusColor, fi.Rank, fi.CrewTag.TAG, fi.KeepPanelVisible);
                    break;
            }
            item.Panel?.UpdatePanel(true);
        }


        public void Clear()
        {
            if (Parent is MainView lobby)
                lobby._pause._lobby.CallFunction("CLEAR_PLAYERS_COLUMN");
            else if (Parent is TabView pause)
                pause._pause._pause.CallFunction("CLEAR_PLAYERS_TAB_PLAYERS_COLUMN", ParentTab);
            Items.Clear();
            Pagination.Reset();
        }

        public void RemovePlayer(int id)
        {
            if (Parent != null && Parent.Visible)
            {
                if (Parent is MainView lobby)
                    lobby._pause._lobby.CallFunction("REMOVE_PLAYER_ITEM", id);
                else if (Parent is TabView pause)
                    pause._pause._lobby.CallFunction("REMOVE_PLAYERS_TAB_PLAYER_ITEM", ParentTab, id);
            }
            Items.RemoveAt(id);
        }

        internal async void GoUp()
        {
            try
            {
                ClearPedInPauseMenu();
                Items[CurrentSelection].Selected = false;
                bool overflow = CurrentSelection == 0 && Pagination.TotalPages > 1;
                if (Pagination.GoUp())
                {
                    if (Pagination.scrollType == ScrollingType.ENDLESS || (Pagination.scrollType == ScrollingType.CLASSIC && !overflow))
                    {
                        _itemCreation(Pagination.GetPage(CurrentSelection), Pagination.CurrentPageIndex, true);
                        if (Parent is MainView lobby)
                            lobby._pause._lobby.CallFunction("SET_INPUT_EVENT", 8, 100);
                        else if (Parent is TabView pause)
                            pause._pause._pause.CallFunction("SET_INPUT_EVENT", 8, 100);
                    }
                    else if (Pagination.scrollType == ScrollingType.PAGINATED || (Pagination.scrollType == ScrollingType.CLASSIC && overflow))
                    {
                        if (Parent is MainView lobby)
                            lobby._pause._lobby.CallFunction("CLEAR_PLAYERS_COLUMN");
                        else if (Parent is TabView pause)
                            pause._pause._pause.CallFunction("CLEAR_PLAYERS_TAB_PLAYERS_COLUMN", ParentTab);
                        int i = 0;
                        int max = Pagination.ItemsPerPage;
                        while (i < max)
                        {
                            await BaseScript.Delay(0);
                            if (!Parent.Visible) return;
                            _itemCreation(Pagination.CurrentPage, i, false, true);
                            i++;
                        }
                    }
                }
                if (Parent is MainView _lobby)
                {
                    _lobby._pause._lobby.CallFunction("SET_PLAYERS_SELECTION", Pagination.ScaleformIndex);
                    _lobby._pause._lobby.CallFunction("SET_PLAYERS_QTTY", CurrentSelection + 1, Items.Count);
                }
                else if (Parent is TabView _pause)
                {
                    _pause._pause._pause.CallFunction("SET_PLAYERS_TAB_PLAYERS_SELECTION", ParentTab, Pagination.ScaleformIndex);
                    _pause._pause._pause.CallFunction("SET_PLAYERS_TAB_PLAYERS_QTTY", ParentTab, CurrentSelection + 1, Items.Count);
                }

                Items[CurrentSelection].Selected = true;
                if (Items[CurrentSelection].ClonePed != null)
                    Items[CurrentSelection].CreateClonedPed();
                IndexChangedEvent();
                Pagination.GetPageIndexFromScaleformIndex(Pagination.ScaleformIndex);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        internal async void GoDown()
        {
            try
            {
                ClearPedInPauseMenu();
                Items[CurrentSelection].Selected = false;
                bool overflow = CurrentSelection == Items.Count - 1 && Pagination.TotalPages > 1;
                if (Pagination.GoDown())
                {
                    if (Pagination.scrollType == ScrollingType.ENDLESS || (Pagination.scrollType == ScrollingType.CLASSIC && !overflow))
                    {
                        _itemCreation(Pagination.GetPage(CurrentSelection), Pagination.CurrentPageIndex, false);
                        if (Parent is MainView lobby)
                            lobby._pause._lobby.CallFunction("SET_INPUT_EVENT", 9, 100);
                        else if (Parent is TabView pause)
                            pause._pause._pause.CallFunction("SET_INPUT_EVENT", 9, 100);
                    }
                    else if (Pagination.scrollType == ScrollingType.PAGINATED || (Pagination.scrollType == ScrollingType.CLASSIC && overflow))
                    {
                        if (Parent is MainView lobby)
                            lobby._pause._lobby.CallFunction("CLEAR_PLAYERS_COLUMN");
                        else if (Parent is TabView pause)
                            pause._pause._pause.CallFunction("CLEAR_PLAYERS_TAB_PLAYERS_COLUMN", ParentTab);
                        int i = 0;
                        int max = Pagination.ItemsPerPage;
                        while (i < max)
                        {
                            await BaseScript.Delay(0);
                            if (!Parent.Visible) return;
                            _itemCreation(Pagination.CurrentPage, i, false);
                            i++;
                        }
                    }
                }
                if (Parent is MainView _lobby)
                {
                    _lobby._pause._lobby.CallFunction("SET_PLAYERS_SELECTION", Pagination.ScaleformIndex);
                    _lobby._pause._lobby.CallFunction("SET_PLAYERS_QTTY", CurrentSelection + 1, Items.Count);
                }
                else if (Parent is TabView _pause)
                {
                    _pause._pause._pause.CallFunction("SET_PLAYERS_TAB_PLAYERS_SELECTION", ParentTab, Pagination.ScaleformIndex);
                    _pause._pause._pause.CallFunction("SET_PLAYERS_TAB_PLAYERS_QTTY", ParentTab, CurrentSelection + 1, Items.Count);
                }

                Items[CurrentSelection].Selected = true;
                if (Items[CurrentSelection].ClonePed != null)
                    Items[CurrentSelection].CreateClonedPed();
                IndexChangedEvent();
                Pagination.GetPageIndexFromScaleformIndex(Pagination.ScaleformIndex);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        public int CurrentSelection
        {
            get { return Items.Count == 0 ? 0 : Pagination.CurrentMenuIndex; }
            set
            {
                ClearPedInPauseMenu();
                if (value < 0)
                {
                    Pagination.CurrentMenuIndex = 0;
                }
                else if (value >= Items.Count)
                {
                    Pagination.CurrentMenuIndex = Items.Count - 1;
                }
                if (Pagination.TotalItems > 0)
                {
                    Items[CurrentSelection].Selected = false;

                    Pagination.CurrentMenuIndex = value;
                    Pagination.CurrentPage = Pagination.GetPage(Pagination.CurrentMenuIndex);
                    Pagination.CurrentPageIndex = Pagination.GetPageIndexFromMenuIndex(value);
                    Pagination.ScaleformIndex = Pagination.GetScaleformIndex(value);

                    if (Parent != null && Parent.Visible)
                    {
                        if (Parent is MainView lobby)
                        {
                            lobby._pause._lobby.CallFunction("SET_PLAYERS_SELECTION", Pagination.GetScaleformIndex(Pagination.CurrentMenuIndex));
                            lobby._pause._lobby.CallFunction("SET_PLAYERS_QTTY", CurrentSelection + 1, Items.Count);
                            Items[CurrentSelection].Selected = true;
                            if (lobby.listCol[0].Type == "players" || (lobby.listCol.Any(x => x.Type == "players") && Items.Count > 0 && Items[0].KeepPanelVisible))
                            {
                                if (Items[CurrentSelection].ClonePed != null)
                                    Items[CurrentSelection].CreateClonedPed();
                            }
                        }
                        else if (Parent is TabView pause)
                        {
                            pause._pause._pause.CallFunction("SET_PLAYERS_TAB_PLAYERS_SELECTION", ParentTab, Pagination.GetScaleformIndex(Pagination.CurrentMenuIndex));
                            pause._pause._pause.CallFunction("SET_PLAYERS_TAB_PLAYERS_QTTY", ParentTab, CurrentSelection + 1, Items.Count);
                            if (pause.Index == pause.Tabs.IndexOf(pause.Tabs[ParentTab]) && pause.FocusLevel == 1)
                            {
                                Items[CurrentSelection].Selected = true;
                                if (Items[CurrentSelection].ClonePed != null)
                                    Items[CurrentSelection].CreateClonedPed();
                            }
                        }
                    }
                }
            }
        }

        public void SortMissions(Comparison<LobbyItem> compare)
        {
            Items[CurrentSelection].Selected = false;
            _unfilteredItems = Items.ToList();
            Clear();
            List<LobbyItem> list = _unfilteredItems.ToList();
            list.Sort(compare);
            Items = list.ToList();
            Pagination.TotalItems = Items.Count;
            if (Parent != null && Parent.Visible)
            {
                if (Parent is MainView lobby)
                    lobby.buildPlayers();
                else if (Parent is TabView pause)
                    pause.buildPlayers(pause.Tabs[ParentTab] as PlayerListTab);
            }
        }

        public void FilterMissions(Func<LobbyItem, bool> predicate)
        {
            Items[CurrentSelection].Selected = false;
            _unfilteredItems = Items.ToList();
            Clear();
            Items = _unfilteredItems.Where(predicate.Invoke).ToList();
            Pagination.TotalItems = Items.Count;
            if (Parent != null && Parent.Visible)
            {
                if (Parent is MainView lobby)
                    lobby.buildPlayers();
                else if (Parent is TabView pause)
                    pause.buildPlayers(pause.Tabs[ParentTab] as PlayerListTab);
            }
        }

        public void ResetFilter()
        {
            Items[CurrentSelection].Selected = false;
            Clear();
            Items = _unfilteredItems.ToList();
            Pagination.TotalItems = Items.Count;
            if (Parent != null && Parent.Visible)
            {
                if (Parent is MainView lobby)
                    lobby.buildPlayers();
                else if (Parent is TabView pause)
                    pause.buildPlayers(pause.Tabs[ParentTab] as PlayerListTab);
            }
        }
        public void SelectItem()
        {
            OnPlayerItemActivated?.Invoke(Items[CurrentSelection], CurrentSelection);
        }

        public void IndexChangedEvent()
        {
            OnIndexChanged?.Invoke(CurrentSelection);
        }
    }
}
