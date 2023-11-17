using CitizenFX.Core;
using static CitizenFX.FiveM.Native.Natives;
using ScaleformUI.Scaleforms;
using CitizenFX.FiveM;

namespace ScaleformUI
{
    public class Main : BaseScript
    {
        /// <summary>
        /// Provides the current game time in milliseconds.
        /// </summary>
        public static int GameTime = GetNetworkTimeAccurate();

        public static PauseMenuScaleform PauseMenu { get; internal set; }
        public static MediumMessageHandler MedMessageInstance { get; internal set; }
        public static InstructionalButtonsScaleform InstructionalButtons { get; internal set; }
        public static BigMessageHandler BigMessageInstance { get; internal set; }
        public static PopupWarning Warning { get; internal set; }
        public static PlayerListHandler PlayerListInstance { get; internal set; }
        public static MissionSelectorHandler JobMissionSelection { get; internal set; }
        public static BigFeedHandler BigFeed { get; internal set; }
        public static RankBarHandler RankBarInstance { get; internal set; }
        public static CountdownHandler CountdownInstance { get; internal set; }
        public static MultiplayerChatHandler MultiplayerChat { get; internal set; }


        internal static ScaleformWideScreen scaleformUI { get; set; }
        internal static ScaleformWideScreen radialMenu { get; set; }
        internal static ScaleformWideScreen radioMenu { get; set; }
        public Main()
        {
            Warning = new();
            MedMessageInstance = new();
            BigMessageInstance = new();
            PlayerListInstance = new();
            JobMissionSelection = new();
            BigFeed = new();
            PauseMenu = new();
            InstructionalButtons = new();
            InstructionalButtons.Load();
            RankBarInstance = new();
            CountdownInstance = new();
            MultiplayerChat = new();
            scaleformUI = new("scaleformui");
            radialMenu = new("radialmenu");
            radioMenu = new("radiomenu");
            Tick += ScaleformUIThread_Tick;
            Tick += OnUpdateGlobalGameTimerAsync;
            MinimapOverlays.Load();
            EventHandlers["onResourceStop"] += new Action<string>((resName) =>
            {
                if (resName == GetCurrentResourceName())
                {
                    if (Game.IsPaused && GetCurrentFrontendMenuVersion() == -2060115030)
                    {
                        ActivateFrontendMenu((uint)Game.GenerateHash("FE_MENU_VERSION_EMPTY_NO_BACKGROUND"), false, -1);
                        AnimpostfxStop("PauseMenuIn");
                        AnimpostfxPlay("PauseMenuOut", 800, false);
                    }
                    radialMenu?.CallFunction("CLEAR_ALL");
                    radialMenu?.Dispose();
                    radioMenu?.CallFunction("CLEAR_ALL");
                    radioMenu?.Dispose();
                    scaleformUI?.CallFunction("CLEAR_ALL");
                    scaleformUI?.Dispose();
                    PauseMenu?.Dispose();
                }
            });
        }

        private async Coroutine ScaleformUIThread_Tick()
        {
            if (MenuHandler.ableToDraw && !(IsWarningMessageActive() || Warning.IsShowing))
                MenuHandler.ProcessMenus();
            if (Warning._sc != null)
                Warning.Update();
            if (InstructionalButtons._sc != null && (InstructionalButtons.ControlButtons != null && InstructionalButtons.ControlButtons.Count != 0))
                InstructionalButtons.Update();
            if (Game.IsPaused) return;
            if (MedMessageInstance._sc != null)
                MedMessageInstance.Update();
            if (BigMessageInstance._sc != null)
                BigMessageInstance.Update();
            if (PlayerListInstance._sc != null && PlayerListInstance.Enabled)
                PlayerListInstance.Update();
            if (JobMissionSelection._sc != null && JobMissionSelection.Enabled)
                JobMissionSelection.Update();
            if (MultiplayerChat.IsTyping())
                MultiplayerChat.Update();
            if (BigFeed._sc != null)
                BigFeed.Update();
            scaleformUI ??= new("ScaleformUI");
            radialMenu ??= new("RadialMenu");
            if (!PauseMenu.Loaded)
                PauseMenu.Load();
            await Task.FromResult(0);
        }

        /// <summary>
        /// Updates the game time.
        /// </summary>
        /// <returns></returns>
        public async Coroutine OnUpdateGlobalGameTimerAsync()
        {
            GameTime = GetNetworkTimeAccurate();
        }
    }
}
