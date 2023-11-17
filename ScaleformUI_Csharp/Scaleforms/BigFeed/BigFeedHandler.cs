using CitizenFX.Core;
using static CitizenFX.FiveM.Native.Natives;

namespace ScaleformUI.Scaleforms
{
    public class BigFeedHandler
    {
        internal ScaleformWideScreen _sc;
        private bool enabled;
        private string title = "";
        private string subtitle = "";
        private string bodyText = "";
        private string textureDictionary = "";
        private string textureName = "";

        /// <summary>
        /// Decides if the feed is centered or right aligned, *MUST* be called before Enabled
        /// </summary>
        public bool RightAligned { get; set; }
        /// <summary>
        /// If true disables all kind of notifications while the feed is showing
        /// </summary>
        public bool DisabledNotifications { get; set; }
        /// <summary>
        /// The title of the loading screen.
        /// </summary>
        public string Title
        {
            get => title;
            set
            {
                title = value;
                if (Enabled)
                {
                    UpdateInfo();
                }
            }
        }

        /// <summary>
        /// The subtitle of the loading screen.
        /// </summary>
        public string Subtitle
        {
            get => subtitle;
            set
            {
                subtitle = value;
                if (Enabled)
                {
                    UpdateInfo();
                }
            }
        }

        /// <summary>
        /// The description of the loading screen.
        /// </summary>
        public string BodyText
        {
            get => bodyText;
            set
            {
                bodyText = value;
                if (Enabled)
                {
                    UpdateInfo();
                }
            }
        }

        /// <summary>
        /// The Texture Dictionary (TXD) where the texture is loaded.
        /// </summary>
        public string TextureDictionary { get => textureDictionary; private set => textureDictionary = value; }

        /// <summary>
        /// The texture in the dictionary.
        /// </summary>
        public string TextureName { get => textureName; private set => textureName = value; }

        public bool Enabled
        {
            get => enabled;
            set
            {
                enabled = value;
                if (value)
                {
                    _sc.CallFunction("SETUP_BIGFEED", RightAligned);
                    _sc.CallFunction("HIDE_ONLINE_LOGO");
                    _sc.CallFunction("FADE_IN_BIGFEED");
                    if (DisabledNotifications)
                        ThefeedCommentTeleportPoolOn();
                    UpdateInfo();
                }
                else
                {
                    _sc.CallFunction("END_BIGFEED");
                    //_sc.CallFunction("END_BIGFEED");
                    if (DisabledNotifications)
                        ThefeedCommentTeleportPoolOff();
                }
            }
        }

        public BigFeedHandler() { Load(); }

        private async void Load()
        {
            if (_sc != null) return;
            _sc = new ScaleformWideScreen("GTAV_ONLINE");
            int timeout = 1000;
            int start = Main.GameTime;
            while (!_sc.IsLoaded && Main.GameTime - start < timeout) await BaseScript.Delay(0);
            _sc.CallFunction("HIDE_ONLINE_LOGO");
        }
        private void Dispose()
        {
            _sc.Dispose();
            _sc = null;
        }

        public void UpdatePicture(string txd, string txn)
        {
            TextureDictionary = txd;
            TextureName = txn;
            if (!enabled) return;
            _sc.CallFunction("SET_BIGFEED_IMAGE", txd, txn);
        }

        private void UpdateInfo()
        {
            if (!enabled) return;
            AddTextEntry("scaleform_ui_bigFeed", bodyText);
            BeginScaleformMovieMethod(_sc.Handle, "SET_BIGFEED_INFO");
            PushScaleformMovieFunctionParameterString(string.Empty);
            BeginTextCommandScaleformString("scaleform_ui_bigFeed");
            EndTextCommandScaleformString_2();
            PushScaleformMovieFunctionParameterInt(0);
            PushScaleformMovieFunctionParameterString(textureDictionary);
            PushScaleformMovieFunctionParameterString(textureName);
            PushScaleformMovieFunctionParameterString(subtitle);
            PushScaleformMovieFunctionParameterString(string.Empty);
            PushScaleformMovieFunctionParameterString(title);
            PushScaleformMovieFunctionParameterInt(0);
            EndScaleformMovieMethod();
        }

        internal async void Update()
        {
            if (enabled)
                _sc.Render2D();
        }
    }
}
