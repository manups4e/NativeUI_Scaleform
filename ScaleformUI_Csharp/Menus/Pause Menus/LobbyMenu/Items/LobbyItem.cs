using CitizenFX.Core;
using static CitizenFX.FiveM.Native.Natives;
using ScaleformUI.LobbyMenu;
using ScaleformUI.PauseMenu;
using CitizenFX.FiveM;

namespace ScaleformUI
{
    public class LobbyItem
    {
        internal int _type;
        private bool _enabled = true;
        private bool _selected;
        private Ped clonePed;
        private Ped _clonePed;
        private Ped _clonePedForPauseMenu;
        private bool _clonePedAsleep = true;
        private bool _clonePedLighting = false;
        private bool keepPanelVisible;

        /// <summary>
        /// Whether this item is currently selected.
        /// </summary>
        public virtual bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
            }
        }

        /// <summary>
        /// Keeps this item Stats panel visible if there's one.
        /// </summary>
        public bool KeepPanelVisible
        {
            get =>
                keepPanelVisible;
            set
            {
                keepPanelVisible = value;
                if (ParentColumn != null && ParentColumn.Parent != null && ParentColumn.Parent.Visible)
                {
                    Panel?.UpdatePanel();
                    if (ParentColumn.Parent is MainView lobby)
                    {
                        if (lobby.PlayersColumn.Items[lobby.PlayersColumn.CurrentSelection] == this)
                        {
                            UpdateClone();
                            lobby._pause._lobby.CallFunction("SET_PLAYERS_STAT_PANEL_PERMANENT", ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), keepPanelVisible);
                        }
                    }
                    else if (ParentColumn.Parent is TabView pause)
                    {
                        if (pause.Tabs[pause.Index] is PlayerListTab tab)
                        {
                            if (tab.PlayersColumn.Items[tab.PlayersColumn.CurrentSelection] == this)
                            {
                                UpdateClone();
                                pause._pause._lobby.CallFunction("SET_PLAYERS_TAB_PLAYERS_STAT_PANEL_PERMANENT", ParentColumn.ParentTab, ParentColumn.Pagination.GetScaleformIndex(ParentColumn.Items.IndexOf(this)), keepPanelVisible);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Cloned ped for the pause menu. This is the ped that will be shown in the pause menu.
        /// The ped is put under the players position when the menu is opened.
        /// </summary>
        public Ped ClonePed
        {
            get => clonePed;
            set
            {
                clonePed = value;
                if (clonePed != null)
                    CreateClonedPed();
                else
                    ClearPedInPauseMenu();
            }
        }

        public bool ClonePedAsleep
        {
            get => _clonePedAsleep;
            set
            {
                _clonePedAsleep = value;
                // Don't ask me why its in reverse, it just is.
                // They should have called it SetPauseMenuPedAwakeState
                SetPauseMenuPedSleepState(!_clonePedAsleep);
            }
        }

        public bool ClonePedLighting
        {
            get => _clonePedLighting;
            set
            {
                _clonePedLighting = value;
                SetPauseMenuPedLighting(_clonePedLighting);
            }
        }

        public void SetOffline()
        {
            ClonePedLighting = false;
            ClonePedAsleep = true;
        }

        public void SetOnline()
        {
            ClonePedLighting = true;
            ClonePedAsleep = false;
        }

        internal void CreateClonedPed()
        {
            // create a ped that we can use for the pause menu and hide it
            if (_clonePedForPauseMenu is null || !_clonePedForPauseMenu.Exists())
            {
                _clonePedForPauseMenu = clonePed.Clone();
                HidePed(_clonePedForPauseMenu);
            }

            if (ParentColumn != null && ParentColumn.Parent != null && ParentColumn.Parent.Visible)
            {
                Panel?.UpdatePanel();
                if (ParentColumn.Parent is MainView lobby)
                {
                    if (lobby.PlayersColumn.Items[lobby.PlayersColumn.CurrentSelection] == this)
                    {
                        UpdateClone();
                    }
                }
                else if (ParentColumn.Parent is TabView pause)
                {
                    if (pause.Tabs[pause.Index] is PlayerListTab tab)
                    {
                        if (tab.PlayersColumn.Items[tab.PlayersColumn.CurrentSelection] == this)
                        {
                            UpdateClone();
                        }
                    }
                }
            }
        }

        private void HidePed(Ped ped)
        {
            ped.IsVisible = true;
            ped.IsInvincible = true;
            ped.IsCollisionEnabled = false;
            ped.IsPositionFrozen = true;
            ped.IsPersistent = true;
            ped.Position = ped.Position + new Vector3(0, 0, -50f);
        }

        private async void UpdateClone()
        {
            // delete the old ped if it exists
            if (_clonePed is not null && _clonePed.Exists())
            {
                _clonePed.Delete();
            }

            // clone the ped we cached away for the pause menu
            _clonePed = new Ped(ClonePed(ClonePed.Handle, false, true, true));
            await BaseScript.Delay(1);
            HidePed(_clonePed);
            GivePedToPauseMenu(_clonePed.Handle, 2);
            SetPauseMenuPedSleepState(!_clonePedAsleep);
            if (ParentColumn != null && ParentColumn.Parent != null && ParentColumn.Parent.Visible)
            {
                if (ParentColumn.Parent is MainView lobby)
                {
                    SetPauseMenuPedLighting(_clonePedLighting);
                }
                else if (ParentColumn.Parent is TabView pause)
                {
                    SetPauseMenuPedLighting(_clonePedLighting && pause.FocusLevel > 0);
                }
            }
        }

        public void Dispose()
        {
            if (_clonePed != null)
            {
                if (_clonePed.Exists())
                {
                    _clonePed.Delete();
                }
            }
            if (_clonePedForPauseMenu != null)
            {
                if (_clonePedForPauseMenu.Exists())
                {
                    _clonePedForPauseMenu.Delete();
                }
            }
        }

        /// <summary>
        /// Whether this item is currently being hovered on with a mouse.
        /// </summary>
        public virtual bool Hovered { get; set; }

        /// <summary>
        /// Whether this item is enabled or disabled (text is greyed out and you cannot select it).
        /// </summary>
        public virtual bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                if (ParentColumn != null)
                {
                    int it = ParentColumn.Items.IndexOf(this);
                    //ParentColumn.Parent._pause._lobby.CallFunction("ENABLE_ITEM", it, _enabled);
                }
            }
        }

        /// <summary>
        /// Returns the lobby this item is in.
        /// </summary>
        public PlayerListColumn ParentColumn { get; internal set; }
        public PlayerStatsPanel Panel { get; internal set; }
    }
}
