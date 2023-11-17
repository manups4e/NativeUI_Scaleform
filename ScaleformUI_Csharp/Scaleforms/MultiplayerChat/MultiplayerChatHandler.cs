using CitizenFX.Core;
using CitizenFX.FiveM;

namespace ScaleformUI.Scaleforms
{
    public class MultiplayerChatHandler
    {
        private const string SCALEFORM_NAME = "MULTIPLAYER_CHAT";
        private ScaleformWideScreen _sc;

        private bool _isTyping;
        private int _start = 0;
        private int _duration = 5000;

        public MultiplayerChatHandler()
        {
        }

        public async Task Load()
        {
            if (_sc is not null) return;
            _sc = new ScaleformWideScreen(SCALEFORM_NAME);
            int timeout = 1000;
            int start = Main.GameTime;
            while (!_sc.IsLoaded && Main.GameTime - start < timeout)
            {
                Debug.WriteLine(SCALEFORM_NAME + ": loading");
                await BaseScript.Delay(0);
            }
        }

        private void Dispose()
        {
            _sc.Dispose();
            _sc = null;
        }

        public void SetFocus(ChatVisibility visibility, ChatScope scope = ChatScope.Global, string scopeText = "All", string playerName = "", HudColor color = HudColor.HUD_COLOUR_PURE_WHITE)
        {
            if (visibility == ChatVisibility.Typing)
                _isTyping = true;
            else if (visibility == ChatVisibility.Hidden)
                _isTyping = false;
            else if (visibility == ChatVisibility.Default)
                _start = Main.GameTime;
            _sc.CallFunction("SET_FOCUS", (int)visibility, (int)scope, scopeText, playerName, (int)color);
        }

        public void AddMessage(string playerName, string message, ChatScope scope, bool teamOnly, HudColor color)
        {
            _sc.CallFunction("ADD_MESSAGE", playerName, message, (int)scope, teamOnly, (int)color);
        }

        public void Close()
        {
            SetFocus(ChatVisibility.Hidden);
            _start = 0;
            _isTyping = false;
        }

        public void Show() => SetFocus(ChatVisibility.Default);
        public void StartTyping() => SetFocus(ChatVisibility.Typing);
        public void Hide() => SetFocus(ChatVisibility.Hidden);
        public void Reset() => _sc.CallFunction("RESET");
        public void PageUp()
        {
            if (!_isTyping) return;
            _sc.CallFunction("PAGE_UP");
        }

        public void PageDown()
        {
            if (!_isTyping) return;
            _sc.CallFunction("PAGE_DOWN");
        }

        public void DeleteText()
        {
            if (!_isTyping) return;
            _sc.CallFunction("DELETE_TEXT");
        }

        public void AddText(string text)
        {
            if (!_isTyping) return;
            _sc.CallFunction("ADD_TEXT", text);
        }

        public void CompleteText()
        {
            if (!_isTyping) return;
            _sc.CallFunction("COMPLETE_TEXT");
        }

        public void AbortText()
        {
            if (!_isTyping) return;
            _sc.CallFunction("ABORT_TEXT");
        }

        public bool IsTyping() => _isTyping;
        public void SetDuration(int duration) => _duration = duration;

        public void Update()
        {
            if (_sc is null || !_sc.IsLoaded) return;
            _sc.Render2D();

            if (!_isTyping && Main.GameTime - _start > _duration)
                Close();
        }
    }

    public enum ChatScope
    {
        Global = 0,
        Team = 1,
        All = 2,
        Clan = 3,
    }

    public enum ChatVisibility
    {
        Hidden = 0,
        Default = 1,
        Typing = 2,
    }
}
