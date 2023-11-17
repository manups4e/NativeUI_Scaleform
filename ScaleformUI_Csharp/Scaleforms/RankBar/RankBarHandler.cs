using CitizenFX.Core;
using static CitizenFX.FiveM.Native.Natives;

namespace ScaleformUI.Scaleforms
{
    public class RankBarHandler
    {
        private const int HUD_COMPONENT_ID = 19;

        private HudColor _rankBarColor = HudColor.HUD_COLOUR_FREEMODE;

        /// <summary>
        /// Set the color of the Rank Bar when displayed
        /// </summary>
        public HudColor Color
        {
            get => _rankBarColor;
            set
            {
                _rankBarColor = value;
            }
        }

        public RankBarHandler() { }

        public async Task Load()
        {
            int timeout = 1000;

            int start = Main.GameTime;
            RequestHudScaleform(HUD_COMPONENT_ID);
            while (!HasHudScaleformLoaded(HUD_COMPONENT_ID) && Main.GameTime - start < timeout)
            {
                await BaseScript.Delay(0);
            }
        }

        /// <summary>
        /// Shows the rank bar
        /// </summary>
        /// <param name="limitStart">Floor value of experience e.g. 0</param>
        /// <param name="limitEnd">Ceiling value of experience e.g. 800</param>
        /// <param name="previousValue">Previous Experience value </param>
        /// <param name="currentValue">New Experience value</param>
        /// <param name="currentRank">Current rank</param>
        public async void SetScores(int limitStart, int limitEnd, int previousValue, int currentValue, int currentRank)
        {
            await Load();

            // Color has to be set else it will be white by default
            BeginScaleformMovieMethodHudComponent(HUD_COMPONENT_ID, "SET_COLOUR");
            PushScaleformMovieFunctionParameterInt((int)_rankBarColor);
            EndScaleformMovieMethod();

            // this will set an update the score
            BeginScaleformMovieMethodHudComponent(HUD_COMPONENT_ID, "SET_RANK_SCORES");
            PushScaleformMovieFunctionParameterInt(limitStart);
            PushScaleformMovieFunctionParameterInt(limitEnd);
            PushScaleformMovieFunctionParameterInt(previousValue);
            PushScaleformMovieFunctionParameterInt(currentValue);
            PushScaleformMovieFunctionParameterInt(currentRank);
            PushScaleformMovieFunctionParameterInt(100);
            EndScaleformMovieMethod();
        }

        public void Remove()
        {
            if (HasHudScaleformLoaded(HUD_COMPONENT_ID))
            {
                BeginScaleformScriptHudMovieMethod(HUD_COMPONENT_ID, "REMOVE");
                EndScaleformMovieMethod();
            }
        }

        public async void OverrideAnimationSpeed(int speed = 1000)
        {
            await Load();
            BeginScaleformScriptHudMovieMethod(HUD_COMPONENT_ID, "OVERRIDE_ANIMATION_SPEED");
            PushScaleformMovieFunctionParameterInt(speed);
            EndScaleformMovieMethod();
        }

        public async void OverrideOnscreenDuration(int duration = 4000)
        {
            await Load();
            BeginScaleformScriptHudMovieMethod(HUD_COMPONENT_ID, "OVERRIDE_ONSCREEN_DURATION");
            PushScaleformMovieFunctionParameterInt(duration);
            EndScaleformMovieMethod();
        }
    }
}
