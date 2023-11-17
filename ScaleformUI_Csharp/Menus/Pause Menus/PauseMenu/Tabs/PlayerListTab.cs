using static CitizenFX.FiveM.Native.Natives;
using ScaleformUI.Elements;
using ScaleformUI.LobbyMenu;

namespace ScaleformUI.PauseMenu
{
    public delegate void ColumnFocusedEvent(Column column, int index);
    public class PlayerListTab : BaseTab
    {
        private const int V = 2;
        private bool _focused;
        private int focus = 0;
        internal List<Column> listCol;
        internal bool _newStyle;
        public event ColumnFocusedEvent OnFocusChanged;

        public bool ForceFirstSelectionOnFocus { get; set; }

        public override bool Focused
        {
            get { return _focused; }
            set
            {
                _focused = value;
            }
        }
        public int Focus
        {
            get => focus;
        }
        public SettingsListColumn SettingsColumn { get; private set; }
        public PlayerListColumn PlayersColumn { get; private set; }
        public MissionsListColumn MissionsColumn { get; private set; }
        public MissionDetailsPanel MissionPanel { get; private set; }
        public PlayerListTab(string name, SColor color, bool newStyle = true) : base(name, color)
        {
            _type = V;
            _newStyle = newStyle;
        }

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

        internal async void updateFocus(int column, bool isMouse = false)
        {
            bool goingLeft = column < focus;
            int f = column;
            if (f < 0)
                f = listCol.Count - 1;
            else if (f > listCol.Count - 1)
                f = 0;
            if (listCol[f].Type != "players")
            {
                if (PlayersColumn != null && PlayersColumn.Items.Count > 0 && !PlayersColumn.Items[PlayersColumn.CurrentSelection].KeepPanelVisible)
                    ClearPedInPauseMenu();
            }
            focus = f;
            if (listCol[focus].Type == "panel")
            {
                if (goingLeft)
                    updateFocus(focus - 1, isMouse);
                else
                    updateFocus(focus + 1, isMouse);
                return;
            }
            if (Parent != null && Parent.Visible)
            {
                int idx = await Parent._pause._pause.CallFunctionReturnValueInt("SET_PLAYERS_TAB_FOCUS", Parent.Tabs.IndexOf(this), focus);
                if (!isMouse)
                {
                    switch (listCol[Focus].Type)
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
            OnFocusChanged?.Invoke(listCol[Focus], Focus);
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
                if (this.Parent != null)
                {
                    col.Parent = this.Parent;
                    col.ParentTab = Parent.Tabs.IndexOf(this);
                }

                switch (col)
                {
                    case SettingsListColumn:
                        SettingsColumn = col as SettingsListColumn;
                        SettingsColumn.Order = columns.IndexOf(col);
                        break;
                    case PlayerListColumn:
                        PlayersColumn = col as PlayerListColumn;
                        PlayersColumn.Order = columns.IndexOf(col);
                        break;
                    case MissionsListColumn:
                        MissionsColumn = col as MissionsListColumn;
                        MissionsColumn.Order = columns.IndexOf(col);
                        break;
                    case MissionDetailsPanel:
                        MissionPanel = col as MissionDetailsPanel;
                        MissionPanel.Order = columns.IndexOf(col);
                        break;
                }
            }
        }
    }
}