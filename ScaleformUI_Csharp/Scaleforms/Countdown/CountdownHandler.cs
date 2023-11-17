using CitizenFX.Core;
using static CitizenFX.FiveM.Native.Natives;

namespace ScaleformUI.Scaleforms
{
    public class CountdownHandler
    {
        private const string SCALEFORM_NAME = "COUNTDOWN";
        private ScaleformWideScreen _sc;

        public CountdownHandler() { }

        /// <summary>
        /// This will start a countdown and play the audio for each step, default is 3, 2, 1, GO
        /// method is awaitable and will return when the countdown shows "GO"
        /// </summary>
        /// <param name="number">number to start counting down from</param>
        /// <param name="hudColor">hud colour for the background of the countdown number</param>
        /// <param name="countdownAudioName">audio name for countdown e.g. 321, 3_2_1, Countdown_321, Countdown_1</param>
        /// <param name="countdownAudioRef">audio reference for countdown e.g. Car_Club_Races_Pursuit_Series_Sounds, HUD_MINI_GAME_SOUNDSET, Island_Race_Soundset, DLC_AW_Frontend_Sounds, DLC_Air_Race_Frontend_Sounds, Island_Race_Soundset, DLC_Stunt_Race_Frontend_Sounds</param>
        /// <param name="goAudioName">audio name for GO message e.g. Go, Countdown_Go</param>
        /// <param name="goAudioRef">audio ref for Go message e.g. Car_Club_Races_Pursuit_Series_Sounds, HUD_MINI_GAME_SOUNDSET, Island_Race_Soundset, DLC_AW_Frontend_Sounds, DLC_Air_Race_Frontend_Sounds, Island_Race_Soundset, DLC_Stunt_Race_Frontend_Sounds</param>
        public async Task Start(
            int number = 3,
            HudColor hudColor = HudColor.HUD_COLOUR_GREEN,
            string countdownAudioName = "321",
            string countdownAudioRef = "Car_Club_Races_Pursuit_Series_Sounds",
            string goAudioName = "Go",
            string goAudioRef = "Car_Club_Races_Pursuit_Series_Sounds")
        {
            await Load();

            if (_sc.IsLoaded)
                DisplayCountdown();

            int r = 255, g = 255, b = 255, a = 255;
            GetHudColour((int)hudColor, ref r, ref g, ref b, ref a);

            int gameTime = Main.GameTime;

            while (number >= 0)
            {
                await BaseScript.Delay(0);
                if ((GetNetworkTimeAccurate() - gameTime) > 1000)
                {
                    PlaySoundFrontend(-1, countdownAudioName, countdownAudioRef, true);
                    gameTime = Main.GameTime;
                    ShowMessage(number, r, g, b);
                    number--;
                }
            }

            PlaySoundFrontend(-1, goAudioName, goAudioRef, true);
            ShowMessage("CNTDWN_GO", r, g, b);

            Dispose();
        }

        private async Task Load()
        {
            if (_sc is not null) return;

            RequestScriptAudioBank("HUD_321_GO", false);
            _sc = new ScaleformWideScreen(SCALEFORM_NAME);
            int timeout = 1000;
            int start = Main.GameTime;
            while (!_sc.IsLoaded && Main.GameTime - start < timeout) await BaseScript.Delay(0);
        }

        private async void Dispose()
        {
            // Delay the dispose to allow the scaleform to finish playing
            // this allows the player to act on the last message
            int gameTime = Main.GameTime;
            while (Main.GameTime - gameTime < 1000) await BaseScript.Delay(0);
            _sc.Dispose();
            _sc = null;
        }

        private void ShowMessage(int number, int r = 255, int g = 255, int b = 255)
        {
            ShowMessage($"{number}", r, g, b);
        }

        private void ShowMessage(string message, int r = 255, int g = 255, int b = 255)
        {
            _sc.CallFunction("SET_MESSAGE", message, r, g, b, true);
            _sc.CallFunction("FADE_MP", message, r, g, b);
        }

        private async Task DisplayCountdown()
        {
            while (_sc != null && _sc.IsLoaded)
            {
                _sc.Render2D();
                await BaseScript.Delay(0);
            }
        }
    }
}
