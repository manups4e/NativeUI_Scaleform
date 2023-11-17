using CitizenFX.Core;
using CitizenFX.FiveM;
using static CitizenFX.FiveM.Native.Natives;

namespace ScaleformUI.Scaleforms
{
    public enum JobSelectionCardIcon
    {
        NONE = -1,
        CUSTOM_MISSION,
        DEATHMATCH,
        RACE,
        SURVIVAL,
        TEAM_DEATHMATCH,
        UNUSED1,
        GANG_ATTACK,
        UNUSED2,
        BASE_JUMPING,
        VEHICLE_DEATHMATCH,
        RACE_LAND,
        RACE_FOOT,
        RACE_BICYCLE,
        RACE_WATER,
        RACE_AIR,
        LAST_TEAM_STANDING,
        CAPTURE_THE_FLAG,
        HEIST_PREPARATION,
        HEIST,
        RACE_STUNT,
    }

    public class MissionSelectorHandler
    {
        internal ScaleformWideScreen _sc;
        private bool enabled;
        private bool alreadyVoted;
        public int[] Votes = new int[9];
        public int VotedFor = -1;
        public int MaxVotes = 0;
        public int SelectedCard = 0;
        public HudColor VotesColor = HudColor.HUD_COLOUR_BLUE;
        public JobSelectionTitle JobTitle = new("", "");
        public List<JobSelectionCard> Cards = new();
        public List<JobSelectionButton> Buttons = new();

        public void SetTitle(string title)
        {
            JobTitle.Title = title;
        }

        public void SetVotes(int actual, string label = "")
        {
            string tot = $"{actual} / {MaxVotes}";
            if (!string.IsNullOrWhiteSpace(label)) JobTitle.Label = label;
            JobTitle.Votes = tot + " " + JobTitle.Label;
        }

        public void AddCard(JobSelectionCard card)
        {
            if (Cards.Count < 9)
            {
                if (!Cards.Any(x => x.Title == card.Title))
                {
                    Cards.Add(card);
                }
            }
        }
        public void AddButton(JobSelectionButton button)
        {
            if (Buttons.Count < 3)
            {
                Buttons.Add(button);
            }
        }

        public bool Enabled
        {
            get => enabled;
            set
            {
                enabled = value;
                if (enabled)
                    BuildMenu();
                else
                    Dispose();
            }
        }

        public bool AlreadyVoted => alreadyVoted;

        internal void Dispose()
        {
            Votes = new int[9];
            _sc.Dispose();
            _sc = null;
        }

        internal async void BuildMenu()
        {
            await Load();
            SetTitle(JobTitle.Title, JobTitle.Votes);
            foreach (JobSelectionCard card in Cards)
            {
                if (!string.IsNullOrWhiteSpace(card.Txd))
                {
                    while (!HasStreamedTextureDictLoaded(card.Txd))
                    {
                        await BaseScript.Delay(0);
                        RequestStreamedTextureDict(card.Txd, true);
                    }
                }
                SetGridItem(Cards.IndexOf(card), card.Title, card.Txd, card.Txn, 1, 0, (int)card.Icon, false, card.RpMultiplier, card.CashMultiplier, false, (int)card.IconColor, card.ApMultiplier);
                SetStreamedTextureDictAsNoLongerNeeded(card.Txd);
            }

            foreach (JobSelectionButton button in Buttons)
            {
                SetButtonItem(Buttons.IndexOf(button) + 6, button.Text);
            }
            SetSelection(0, Cards[0].Title, Cards[0].Description);

            foreach (MissionDetailsItem detail in Cards[0].Details)
            {
                SetDetailsItem(Cards[0].Details.IndexOf(detail), 0, Cards[0].Details.IndexOf(detail), detail.Type, 0, 0, detail.TextLeft, detail.TextRight, detail.Icon, detail.IconColor, detail.Tick);
            }
        }

        public void SelectCard(int idx)
        {
            if (idx < 6)
            {
                SetSelection(idx, Cards[idx].Title, Cards[idx].Description);
                foreach (MissionDetailsItem detail in Cards[idx].Details)
                {
                    SetDetailsItem(Cards[idx].Details.IndexOf(detail), idx, Cards[idx].Details.IndexOf(detail), detail.Type, 0, 0, detail.TextLeft, detail.TextRight, detail.Icon, detail.IconColor, detail.Tick);
                }
            }
            else
            {
                SetSelection(idx, Buttons[idx - 6].Text, Buttons[idx - 6].Description);
                foreach (MissionDetailsItem detail in Buttons[idx - 6].Details)
                {
                    SetDetailsItem(Buttons[idx - 6].Details.IndexOf(detail), idx, Buttons[idx - 6].Details.IndexOf(detail), detail.Type, 0, 0, detail.TextLeft, detail.TextRight, detail.Icon, detail.IconColor, detail.Tick);
                }
            }
        }

        internal void UpdateOwnVote(int idx, int oldidx, bool showCheckMark = false, bool flashBG = false)
        {
            if (idx == oldidx) return;

            for (int i = 0; i < 9; i++)
            {
                if (Votes[i] < 0) Votes[i] = 0;
                _sc.CallFunction("SET_GRID_ITEM_VOTE", i, Votes[i], (int)VotesColor, showCheckMark, flashBG);
            }
            int votes = Votes.ToList().Sum();
            SetVotes(votes);
            SetTitle(JobTitle.Title, JobTitle.Votes);
        }

        public void ShowPlayerVote(int idx, string playerName, HudColor color, bool showCheckMark = false, bool flashBG = false)
        {
            Votes[idx]++;
            int r = 0, g = 0, b = 0, a = 0;
            GetHudColour((int)color, ref r, ref g, ref b, ref a);
            _sc.CallFunction("SHOW_PLAYER_VOTE", idx, playerName, r, g, b);
            int votes = Votes.ToList().Sum();
            SetVotes(votes);
            SetTitle(JobTitle.Title, JobTitle.Votes);
            _sc.CallFunction("SET_GRID_ITEM_VOTE", idx, Votes[idx], (int)VotesColor, showCheckMark, flashBG);
        }


        internal async Task Load()
        {
            if (_sc != null) return;
            _sc = new("MP_NEXT_JOB_SELECTION");
            int timeout = 1000;
            int start = Main.GameTime;
            while (!_sc.IsLoaded && Main.GameTime - start < timeout) await BaseScript.Delay(0);
        }

        bool eventBool = false;
        int eventType = 0;
        int itemId = 0;
        int context = 0;
        int unused = 0;
        internal void Update()
        {
            _sc.Render2D();
            Game.DisableAllControlsThisFrame(0);
            Game.DisableAllControlsThisFrame(1);
            Game.DisableAllControlsThisFrame(2);
            if (IsUsingKeyboard(2))
            {
                SetMouseCursorActiveThisFrame();
                SetInputExclusive(2, 239);
                SetInputExclusive(2, 240);
                SetInputExclusive(2, 237);
                SetInputExclusive(2, 238);

                bool success = GetScaleformMovieCursorSelection(_sc.Handle, ref eventBool, ref eventType, ref context, ref itemId);
                if (success)
                {
                    switch (eventType)
                    {
                        case 5:
                            if (SelectedCard != context)
                            {
                                SelectedCard = context;
                                SelectCard(context);
                            }
                            else
                            {
                                if (SelectedCard < 6)
                                {
                                    if (alreadyVoted)
                                    {
                                        int old = VotedFor;
                                        Votes[VotedFor] -= 1;
                                        if (old != SelectedCard)
                                        {
                                            VotedFor = SelectedCard;
                                            Votes[VotedFor] += 1;
                                        }
                                        UpdateOwnVote(VotedFor, old);
                                        Cards[SelectedCard].CardSelected();
                                    }
                                    else
                                    {
                                        alreadyVoted = true;
                                        VotedFor = SelectedCard;
                                        Votes[VotedFor] += 1;
                                        UpdateOwnVote(VotedFor, -1);
                                        Cards[SelectedCard].CardSelected();
                                    }
                                }
                                else
                                {
                                    JobSelectionButton btn = Buttons[SelectedCard - 6];

                                    if (btn.Selectable)
                                    {
                                        if (alreadyVoted)
                                        {
                                            int old = VotedFor;
                                            Votes[VotedFor] -= 1;
                                            if (old != SelectedCard)
                                            {
                                                VotedFor = SelectedCard;
                                                Votes[VotedFor] += 1;
                                            }
                                            UpdateOwnVote(VotedFor, old);
                                        }
                                        else
                                        {
                                            alreadyVoted = true;
                                            VotedFor = SelectedCard;
                                            Votes[VotedFor] += 1;
                                            UpdateOwnVote(VotedFor, -1);
                                        }
                                    }
                                    btn.ButtonPressed();
                                }
                            }
                            break;
                    }
                }
            }
            if (Game.IsDisabledControlJustPressed(2, Control.FrontendUp))
            {
                if ((SelectedCard - 3) >= 0 && (SelectedCard - 3) <= 8)
                {
                    SelectedCard -= 3;
                    SelectCard(SelectedCard);
                }
            }
            if (Game.IsDisabledControlJustPressed(2, Control.FrontendDown))
            {
                if ((SelectedCard + 3) >= 0 && (SelectedCard + 3) <= 8)
                {
                    SelectedCard += 3;
                    SelectCard(SelectedCard);
                }
            }
            if (Game.IsDisabledControlJustPressed(2, Control.FrontendLeft))
            {
                if ((SelectedCard - 1) >= 0 && (SelectedCard - 1) <= 8)
                {
                    SelectedCard -= 1;
                    SelectCard(SelectedCard);
                }

            }
            if (Game.IsDisabledControlJustPressed(2, Control.FrontendRight))
            {
                if ((SelectedCard + 1) >= 0 && (SelectedCard + 1) <= 8)
                {
                    SelectedCard += 1;
                    SelectCard(SelectedCard);
                }
            }
            if (Game.IsDisabledControlJustPressed(2, Control.FrontendAccept))
            {
                if (SelectedCard < 6)
                {
                    if (alreadyVoted)
                    {
                        int old = VotedFor;
                        Votes[VotedFor] -= 1;
                        if (old != SelectedCard)
                        {
                            VotedFor = SelectedCard;
                            Votes[VotedFor] += 1;
                        }
                        UpdateOwnVote(VotedFor, old);
                        Cards[SelectedCard].CardSelected();
                    }
                    else
                    {
                        alreadyVoted = true;
                        VotedFor = SelectedCard;
                        Votes[VotedFor] += 1;
                        UpdateOwnVote(VotedFor, -1);
                        Cards[SelectedCard].CardSelected();
                    }
                }
                else
                {
                    JobSelectionButton btn = Buttons[SelectedCard - 6];

                    if (btn.Selectable)
                    {
                        if (alreadyVoted)
                        {
                            int old = VotedFor;
                            Votes[VotedFor] -= 1;
                            if (old != SelectedCard)
                            {
                                VotedFor = SelectedCard;
                                Votes[VotedFor] += 1;
                            }
                            UpdateOwnVote(VotedFor, old);
                        }
                        else
                        {
                            alreadyVoted = true;
                            VotedFor = SelectedCard;
                            Votes[VotedFor] += 1;
                            UpdateOwnVote(VotedFor, -1);
                        }
                    }
                    btn.ButtonPressed();
                }
            }
            /*
        elseif IsControlJustPressed(2, 192) then --TAB
            if player_list_shown then
                player_list_shown = false
            else
                player_list_shown = true
            end
            toggle_player_list()
        elseif IsControlJustPressed(2, 322) then --ESC
            show_next_job_selection = false
            SwitchInPlayer(GetPlayerPed(-1))
           */
        }

        private void SetTitle(string left, string votes)
        {
            _sc.CallFunction("SET_TITLE", left, votes);
        }

        private void SetGridItem(int id, string title, string txd, string txn, int loadtype, int verified_type, int icon, bool check, int rp_multiplier, int cash_multiplier, bool disabled, int iconColor, int ap_multiplier)
        {
            _sc.CallFunction("SET_GRID_ITEM", id, title, txd, txn, loadtype, verified_type, icon, check, rp_multiplier, cash_multiplier, disabled, iconColor, ap_multiplier);
        }
        private void SetButtonItem(int id, string title)
        {
            _sc.CallFunction("SET_GRID_ITEM", id, title, "", "", -1, -1, -1, false, -1, -1, false, 1, -1);
        }

        private void SetSelection(int index, string title, string description, bool hideHighlight = false)
        {
            _sc.CallFunction("SET_SELECTION", index, title, description, hideHighlight);
        }

        private void SetDetailsItem(int id, int menu_id, int unique_id, int type, int initial_index, int is_selectable, string lText, string rText, JobIcon icon, HudColor iconColor = HudColor.HUD_COLOUR_WHITE, bool tick = false)
        {
            _sc.CallFunction("SET_DETAILS_ITEM", id, menu_id, unique_id, type == 3 ? 4 : type, initial_index, is_selectable, lText, rText, (int)icon, (int)iconColor, tick);
        }
    }


    public class JobSelectionTitle
    {
        public string Title { get; set; }
        public string Label { get; set; }
        public string Votes { get; set; }

        public JobSelectionTitle(string title, string label)
        {
            Title = title;
            Label = label;
        }
    }

    public delegate void OnCardSelected(JobSelectionCard card);
    public class JobSelectionCard
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Txd { get; set; }
        public string Txn { get; set; }
        public int RpMultiplier { get; set; }
        public int CashMultiplier { get; set; }
        public JobSelectionCardIcon Icon { get; set; }
        public HudColor IconColor { get; set; }
        public int ApMultiplier { get; set; }
        public List<MissionDetailsItem> Details { get; set; } // NON PIU DI 4
        public OnCardSelected OnCardSelected;


        public JobSelectionCard(string title, string description, string txd, string txn, int rpMult, int cashMult, JobSelectionCardIcon icon, HudColor iconColor, int apMultiplier, List<MissionDetailsItem> details)
        {
            this.Title = title;
            this.Description = description;
            this.Txd = txd;
            this.Txn = txn;
            RpMultiplier = rpMult;
            CashMultiplier = cashMult;
            Icon = icon;
            IconColor = iconColor;
            ApMultiplier = apMultiplier;
            Details = details;
        }
        internal void CardSelected()
        {
            OnCardSelected?.Invoke(this);
        }
    }

    public delegate void JobSelectionButtonEvent();
    public class JobSelectionButton
    {
        public string Text { get; set; }
        public string Description { get; set; }
        public List<MissionDetailsItem> Details { get; set; } // NON PIU DI 5
        public event JobSelectionButtonEvent OnButtonPressed;
        public bool Selectable = true;
        public JobSelectionButton(string text, string description, List<MissionDetailsItem> details)
        {
            Text = text;
            Description = description;
            Details = details;
        }
        internal void ButtonPressed()
        {
            OnButtonPressed?.Invoke();
        }
    }

    public enum JobIcon
    {
        GTAOMission = 0,
        Deathmatch = 1,
        RaceFinish = 2,
        GTAOSurvival = 3,
        TeamDeathmatch = 4,
        Castle = 6,
        Parachute = 8,
        VehicleDeathmatch = 9,
        RaceCar = 10,
        RaceFoot = 11,
        RaceSea = 12,
        RaceBike = 13,
        RaceAir = 14,
        LastTeamStanding = 15,
        Briefcase = 16,
        RaceStunt = 18
    }

    public class MissionDetailsItem
    {
        public JobIcon Icon;
        public string TextLeft;
        public string TextRight;
        public HudColor IconColor;
        public int Type;
        public bool Tick;
        public MissionDetailsItem(string textLeft, string textRight, bool separator)
        {
            Type = separator ? 4 : 0;
            TextLeft = textLeft;
            TextRight = textRight;
        }

        public MissionDetailsItem(string textLeft, string textRight, JobIcon icon, HudColor iconColor = HudColor.NONE, bool tick = false)
        {
            Type = 2;
            TextLeft = textLeft;
            TextRight = textRight;
            Icon = icon;
            IconColor = iconColor;
            Tick = tick;
        }
    }
}
