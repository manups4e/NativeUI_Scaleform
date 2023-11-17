using CitizenFX.Core;
using static CitizenFX.FiveM.Native.Natives;

namespace ScaleformUI.Scaleforms
{
    public class PlayerListHandler
    {
        private int _start;
        private int _timer;
        public bool Enabled { get; set; }
        internal ScaleformWideScreen _sc { get; set; }
        private int Index { get; set; } = 0;
        public int MaxPages { get; set; } = 1;

        public List<PlayerRow> PlayerRows { get; set; }
        private int currentPage = 0;

        public int CurrentPage
        {
            get => currentPage;
            set
            {
                if (PlayerRows.Count == 0)
                {
                    currentPage = 0;
                    return;
                }
                currentPage = value;
                if (currentPage > 0)
                {
                    Enabled = true;
                    NextPage();
                }
            }
        }
        public string TitleLeftText { get; set; }
        public string TitleRightText { get; set; }
        public int TitleIcon { get; set; }

        public PlayerListHandler()
        {
            PlayerRows = new List<PlayerRow>();
        }

        public async Task Load()
        {
            if (_sc is not null) return;
            _sc = new ScaleformWideScreen("MP_MM_CARD_FREEMODE");
            int timeout = 1000;
            int start = Main.GameTime;
            while (!_sc.IsLoaded && Main.GameTime - start < timeout) await BaseScript.Delay(0);
        }
        public void Dispose()
        {
            if (_sc is null) return;
            Enabled = false;
            Index = 0;
            MaxPages = 1;
            CurrentPage = 0;
            TitleLeftText = "";
            TitleRightText = "";
            TitleIcon = 0;
            _sc.CallFunction("SET_DATA_SLOT_EMPTY");
            _sc.Dispose();
            _sc = null;
            for (int x = 0; x < 1024; x++) // cleaning up in case of a reload, this frees up all ped headshot handles :)
                UnregisterPedheadshot(x);
        }

        public void SetTitle(string left, string right, int icon)
        {
            TitleLeftText = left;
            TitleRightText = right;
            TitleIcon = icon;
        }

        public void AddRow(PlayerRow row)
        {
            PlayerRows.Add(row);
        }

        public void RemoveRow(PlayerRow row)
        {
            PlayerRow r = PlayerRows.FirstOrDefault(x => x.ServerId == row.ServerId);
            if (r != null)
            {
                PlayerRows.Remove(r);
                if (PlayerRows.Any(x => x.RightText.ToLower() == "lobby")) return;
                PlayerRows.Sort((row1, row2) => Convert.ToInt32(row1.RightText).CompareTo(Convert.ToInt32(row2.RightText)));
            }
        }

        public void RemoveRow(int serverId)
        {
            PlayerRow r = PlayerRows.FirstOrDefault(x => x.ServerId == serverId);
            if (r != null)
            {
                PlayerRows.Remove(r);
                PlayerRows.Sort((row1, row2) => Convert.ToInt32(row1.RightText).CompareTo(Convert.ToInt32(row2.RightText)));
            }
        }
        /// <summary>
        /// Used to check if the row from the loop is supposed to be displayed based on the current page view.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private bool IsRowSupposedToShow(int row)
        {
            if (CurrentPage > 0)
            {
                int max = CurrentPage * 16;
                int min = CurrentPage * 16 - 16;

                if (row >= min && row < max) return true;
            }
            return false;
        }

        /// <summary>
        /// Updates the max pages to disaplay based on the raws count.
        /// </summary>
        private void UpdateMaxPages()
        {
            MaxPages = (int)Math.Ceiling(PlayerRows.Count / 16f);
        }

        internal void Update()
        {
            DrawScaleformMovie(_sc.Handle, 0.122f, 0.3f, 0.28f, 0.6f, 255, 255, 255, 255, 0);
            if (_start != 0 && Main.GameTime - _start > _timer)
            {
                CurrentPage = 0;
                Enabled = false;
                _start = 0;
                Dispose();
                return;
            }
        }

        public void NextPage()
        {
            UpdateMaxPages();
            _start = Main.GameTime;
            _timer = 8000;
            BuildMenu();
            if (CurrentPage > MaxPages)
            {
                CurrentPage = 0;
                Enabled = false;
                _start = 0;
                Dispose();
                return;
            }
        }

        public void SetHighlight(int idx)
        {
            _sc.CallFunction("SET_HIGHLIGHT", idx);
        }

        public void DisplayMic(int idx, int unk)
        {
            _sc.CallFunction("DISPLAY_MIC", idx, unk);
        }
        public void UpdateSlot(PlayerRow row)
        {
            PlayerRow r = PlayerRows.FirstOrDefault(x => Convert.ToInt32(x.RightText) == Convert.ToInt32(row.RightText));
            //var r = PlayerRows.FirstOrDefault(x => x.ServerId == row.ServerId);
            if (r != null)
            {
                PlayerRows[PlayerRows.IndexOf(r)] = row;
                if (row.CrewLabelText != "")
                    _sc.CallFunction("UPDATE_SLOT", PlayerRows.IndexOf(r), row.RightText, row.Name, row.Color, (int)row.RightIcon, row.IconOverlayText, row.JobPointsText, $"..+{row.CrewLabelText}", (int)row.JobPointsDisplayType, row.TextureString, row.TextureString, row.FriendType);
                else
                    _sc.CallFunction("UPDATE_SLOT", PlayerRows.IndexOf(r), row.RightText, row.Name, row.Color, (int)row.RightIcon, row.IconOverlayText, row.JobPointsText, "", (int)row.JobPointsDisplayType, row.TextureString, row.TextureString, row.FriendType);
            }
        }

        public void SetIcon(int index, ScoreRightIconType icon, string txt)
        {
            PlayerRow row = PlayerRows[index];
            if (row != null)
            {
                _sc.CallFunction("SET_ICON", index, (int)icon, txt);
            }
        }

        public async void BuildMenu()
        {
            await Load();
            List<PlayerRow> rows = new();
            _sc.CallFunction("SET_DATA_SLOT_EMPTY");
            _sc.CallFunction("SET_TITLE", TitleLeftText, TitleRightText, TitleIcon);
            Index = 0;
            foreach (PlayerRow row in PlayerRows)
            {
                if (IsRowSupposedToShow(Index))
                    rows.Add(row);
                Index++;
            }

            Index = 0;
            foreach (PlayerRow row in rows)
            {
                if (string.IsNullOrWhiteSpace(row.CrewLabelText))
                    _sc.CallFunction("SET_DATA_SLOT", Index, row.RightText, row.Name, row.Color, (int)row.RightIcon, row.IconOverlayText, row.JobPointsText, "", (int)row.JobPointsDisplayType, row.TextureString, row.TextureString, row.FriendType);
                else
                    _sc.CallFunction("SET_DATA_SLOT", Index, row.RightText, row.Name, row.Color, (int)row.RightIcon, row.IconOverlayText, row.JobPointsText, $"..+{row.CrewLabelText}", (int)row.JobPointsDisplayType, row.TextureString, row.TextureString, row.FriendType);
                Index++;

            }
            _sc.CallFunction("DISPLAY_VIEW");
        }
    }

    public class PlayerRowConfig
    {
        public string crewName;
        public int jobPoints;
        public bool showJobPointsIcon;
    }
    public enum ScoreDisplayType
    {
        NUMBER_ONLY = 0,
        ICON = 1,
        NONE = 2
    };

    public enum ScoreRightIconType
    {
        NONE = 0,
        INACTIVE_HEADSET = 48,
        MUTED_HEADSET = 49,
        ACTIVE_HEADSET = 47,
        RANK_FREEMODE = 65,
        KICK = 64,
        LOBBY_DRIVER = 79,
        LOBBY_CODRIVER = 80,
        SPECTATOR = 66,
        BOUNTY = 115,
        DEAD = 116,
        DPAD_GANG_CEO = 121,
        DPAD_GANG_BIKER = 122,
        DPAD_DOWN_TARGET = 123
    };

    /// <summary>
    /// Struct used for the player info row options.
    /// </summary>
    public class PlayerRow
    {
        public int ServerId;
        public string Name;
        public string RightText;
        public int Color;
        public string IconOverlayText;
        public string JobPointsText;
        public string CrewLabelText;


        public ScoreDisplayType JobPointsDisplayType;


        public ScoreRightIconType RightIcon;
        public string TextureString;
        public char FriendType;
    }
}
