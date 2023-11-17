using CitizenFX.Core;
using ScaleformUI.Elements;
using ScaleformUI.Menu;
using static CitizenFX.FiveM.Native.Natives;

namespace ScaleformUI.Scaleforms
{
    public class PauseMenuScaleform
    {
        public ScaleformWideScreen _header;
        public ScaleformWideScreen _pause;
        public ScaleformWideScreen _lobby;
        private bool _visible;
        internal bool Loaded => _header is not null && _header.IsLoaded && _pause is not null && _pause.IsLoaded;
        public bool Visible { get => _visible; set => _visible = value; }

        public PauseMenuScaleform()
        {
        }

        public void Load()
        {
            _header = new ScaleformWideScreen("pausemenuheader");
            _pause = new ScaleformWideScreen("pausemenu");
            _lobby = new ScaleformWideScreen("lobbymenu");
        }

        public void SetHeaderTitle(string title, string subtitle = "", bool shiftUpHeader = false)
        {
            _header.CallFunction("SET_HEADER_TITLE", title, subtitle, shiftUpHeader);
        }

        public void SetHeaderDetails(string topDetail, string midDetail, string botDetail)
        {
            _header.CallFunction("SET_HEADER_DETAILS", topDetail, midDetail, botDetail, false);
        }

        public void ShiftCoronaDescription(bool shiftDesc, bool hideTabs)
        {
            _header.CallFunction("SHIFT_CORONA_DESC", shiftDesc, hideTabs);
        }

        public void ShowHeadingDetails(bool show)
        {
            _header.CallFunction("SHOW_HEADING_DETAILS", show);
        }

        public void SetHeaderCharImg(string txd, string charTexturePath, bool show)
        {
            _header.CallFunction("SET_HEADER_CHAR_IMG", txd, charTexturePath, show);
        }

        public void SetHeaderSecondaryImg(string txd, string charTexturePath, bool show)
        {
            _header.CallFunction("SET_HEADER_CREW_IMG", txd, charTexturePath, show);
        }

        public void HeaderGoRight()
        {
            _header.CallFunction("GO_RIGHT");
        }
        public void HeaderGoLeft()
        {
            _header.CallFunction("GO_LEFT");
        }

        public void AddPauseMenuTab(string title, int tabType, int tabContentType, SColor color)
        {
            _header.CallFunction("ADD_HEADER_TAB", title, tabType, color);
            _pause.CallFunction("ADD_TAB", tabContentType);
        }
        public void AddLobbyMenuTab(string title, int tabType, SColor color)
        {
            _header.CallFunction("ADD_HEADER_TAB", title, tabType, color);
        }

        public void SelectTab(int tab)
        {
            _header.CallFunction("SET_TAB_INDEX", tab);
            _pause.CallFunction("SET_TAB_INDEX", tab);
        }

        public void SetFocus(int focus)
        {
            _pause.CallFunction("SET_FOCUS", focus);
        }

        public void AddLeftItem(int tab, int type, string title, SColor itemColor, SColor highlightColor, bool enabled = true)
        {
            if (itemColor != SColor.HUD_None && highlightColor != SColor.HUD_None)
                _pause.CallFunction("ADD_LEFT_ITEM", tab, type, title, enabled, itemColor, highlightColor);
            else if (itemColor != SColor.HUD_None)
                _pause.CallFunction("ADD_LEFT_ITEM", tab, type, title, enabled, itemColor);
            else
                _pause.CallFunction("ADD_LEFT_ITEM", tab, type, title, enabled);
        }

        public void AddRightTitle(int tab, int leftItem, string title)
        {
            _pause.CallFunction("ADD_RIGHT_TITLE", tab, leftItem, title);
        }

        public void AddRightListLabel(int tab, int leftItem, string label, string fontName, int fontId)
        {
            AddTextEntry($"PauseMenu_{tab}_{leftItem}", label);
            BeginScaleformMovieMethod(_pause.Handle, "ADD_RIGHT_LIST_ITEM");
            ScaleformMovieMethodAddParamInt(tab);
            ScaleformMovieMethodAddParamInt(leftItem);
            ScaleformMovieMethodAddParamInt(0);
            BeginTextCommandScaleformString($"PauseMenu_{tab}_{leftItem}");
            EndTextCommandScaleformString_2();
            ScaleformMovieMethodAddParamPlayerNameString(fontName);
            ScaleformMovieMethodAddParamInt(fontId);
            EndScaleformMovieMethod();
        }

        public void AddRightStatItemLabel(int tab, int leftItem, string label, string rightLabel, ItemFont labelFont, ItemFont rLabelFont)
        {
            _pause.CallFunction("ADD_RIGHT_LIST_ITEM", tab, leftItem, 1, 0, label, rightLabel, -1, labelFont.FontName, labelFont.FontID, rLabelFont.FontName, rLabelFont.FontID);
        }

        public void AddRightStatItemColorBar(int tab, int leftItem, string label, int value, SColor barColor, ItemFont labelFont)
        {
            _pause.CallFunction("ADD_RIGHT_LIST_ITEM", tab, leftItem, 1, 1, label, value, barColor, labelFont.FontName, labelFont.FontID);
        }

        public void AddRightSettingsBaseItem(int tab, int leftItem, string label, string rightLabel, bool enabled)
        {
            _pause.CallFunction("ADD_RIGHT_LIST_ITEM", tab, leftItem, 2, 0, label, enabled, rightLabel);
        }

        public void AddRightSettingsListItem(int tab, int leftItem, string label, List<dynamic> items, int startIndex, bool enabled)
        {
            string stringList = string.Join(",", items);
            _pause.CallFunction("ADD_RIGHT_LIST_ITEM", tab, leftItem, 2, 1, label, enabled, stringList, startIndex);
        }

        public void AddRightSettingsProgressItem(int tab, int leftItem, string label, int max, SColor color, int index, bool enabled)
        {
            _pause.CallFunction("ADD_RIGHT_LIST_ITEM", tab, leftItem, 2, 2, label, enabled, max, color, index);
        }
        public void AddRightSettingsProgressItemAlt(int tab, int leftItem, string label, int max, SColor color, int index, bool enabled)
        {
            _pause.CallFunction("ADD_RIGHT_LIST_ITEM", tab, leftItem, 2, 3, label, enabled, max, color, index);
        }

        public void AddRightSettingsSliderItem(int tab, int leftItem, string label, int max, SColor color, int index, bool enabled)
        {
            _pause.CallFunction("ADD_RIGHT_LIST_ITEM", tab, leftItem, 2, 5, label, enabled, max, color, index);
        }
        public void AddRightSettingsCheckboxItem(int tab, int leftItem, string label, UIMenuCheckboxStyle style, bool check, bool enabled)
        {
            _pause.CallFunction("ADD_RIGHT_LIST_ITEM", tab, leftItem, 2, 4, label, enabled, (int)style, check);
        }

        public void AddKeymapTitle(int tab, int leftItem, string title, string rightLabel_1, string rightLabel_2)
        {
            _pause.CallFunction("ADD_RIGHT_TITLE", tab, leftItem, title, rightLabel_1, rightLabel_2);
        }
        public void AddKeymapItem(int tab, int leftItem, string label, string control1, string control2)
        {
            BeginScaleformMovieMethod(_pause.Handle, "ADD_RIGHT_LIST_ITEM");
            ScaleformMovieMethodAddParamInt(tab);
            ScaleformMovieMethodAddParamInt(leftItem);
            ScaleformMovieMethodAddParamInt(3);
            PushScaleformMovieFunctionParameterString(label);
            BeginTextCommandScaleformString("string");
            AddTextComponentScaleform(control1);
            EndTextCommandScaleformString_2();
            BeginTextCommandScaleformString("string");
            AddTextComponentScaleform(control2);
            EndTextCommandScaleformString_2();
            EndScaleformMovieMethod();
        }
        public void UpdateKeymap(int tab, int leftItem, int rightItem, string control1, string control2)
        {
            BeginScaleformMovieMethod(_pause.Handle, "UPDATE_KEYMAP_ITEM");
            ScaleformMovieMethodAddParamInt(tab);
            ScaleformMovieMethodAddParamInt(leftItem);
            ScaleformMovieMethodAddParamInt(rightItem);
            BeginTextCommandScaleformString("string");
            AddTextComponentScaleform(control1);
            EndTextCommandScaleformString_2();
            BeginTextCommandScaleformString("string");
            AddTextComponentScaleform(control2);
            EndTextCommandScaleformString_2();
            EndScaleformMovieMethod();
        }

        public void SetRightSettingsItemBool(int tab, int leftItem, int rightItem, bool value)
        {
            _pause.CallFunction("SET_RIGHT_SETTINGS_ITEM_VALUE", tab, leftItem, rightItem, value);
        }
        public void SetRightSettingsItemIndex(int tab, int leftItem, int rightItem, int value)
        {
            _pause.CallFunction("SET_RIGHT_SETTINGS_ITEM_VALUE", tab, leftItem, rightItem, value);
        }
        public void SetRightSettingsItemValue(int tab, int leftItem, int rightItem, int value)
        {
            _pause.CallFunction("SET_RIGHT_SETTINGS_ITEM_VALUE", tab, leftItem, rightItem, value);
        }

        public void UpdateItemRightLabel(int tab, int leftItem, int rightItem, string label)
        {
            _pause.CallFunction("UPDATE_RIGHT_ITEM_RIGHT_LABEL", tab, leftItem, rightItem, label);
        }

        public void UpdateStatsItem(int tab, int leftItem, int rightItem, string label, string rightLabel)
        {
            _pause.CallFunction("UPDATE_RIGHT_STATS_ITEM", tab, leftItem, rightItem, label, rightLabel);
        }
        public void UpdateStatsItem(int tab, int leftItem, int rightItem, string label, int value, SColor color)
        {
            _pause.CallFunction("UPDATE_RIGHT_STATS_ITEM", tab, leftItem, rightItem, label, value, color);
        }

        public void UpdateItemColoredBar(int tab, int leftItem, int rightItem, SColor color)
        {
            if (color == SColor.HUD_None)
                _pause.CallFunction("UPDATE_COLORED_BAR_COLOR", tab, leftItem, rightItem, (int)HudColor.HUD_COLOUR_WHITE);
            else
                _pause.CallFunction("UPDATE_COLORED_BAR_COLOR", tab, leftItem, rightItem, color);
        }
        public async Task<string> SendInputEvent(int direction)
        {
            BeginScaleformMovieMethod(_pause.Handle, "SET_INPUT_EVENT");
            ScaleformMovieMethodAddParamInt(direction);
            int ret = EndScaleformMovieMethodReturnValue();
            while (!IsScaleformMovieMethodReturnValueReady(ret)) await BaseScript.Delay(0);
            string res = GetScaleformMovieFunctionReturnString(ret);
            return res;
        }

        public void SendScrollEvent(int direction)
        {
            _pause.CallFunction("SET_SCROLL_EVENT", direction);
        }
        public async Task<string> SendClickEvent()
        {
            BeginScaleformMovieMethod(_pause.Handle, "MOUSE_CLICK_EVENT");
            int ret = EndScaleformMovieMethodReturnValue();
            while (!IsScaleformMovieMethodReturnValueReady(ret)) await BaseScript.Delay(0);
            string res = GetScaleformMovieFunctionReturnString(ret);
            return res;
        }

        public void Dispose()
        {
            _visible = false;
            _pause?.CallFunction("CLEAR_ALL");
            _lobby?.CallFunction("CLEAR_ALL");
            _header?.CallFunction("CLEAR_ALL");
            _pause?.Dispose();
            _lobby?.Dispose();
            _header?.Dispose();
        }

        public void Draw(bool isLobby = false)
        {
            if (_visible && GetCurrentFrontendMenuVersion() == -2060115030)
            {
                SetScriptGfxDrawBehindPausemenu(true);
                DrawScaleformMovie(_header.Handle, 0.501f, 0.162f, 0.6782f, 0.145f, 255, 255, 255, 255, 0);
                if (!isLobby)
                    DrawScaleformMovie(_pause.Handle, 0.6617187f, 0.7226667f, 1, 1, 255, 255, 255, 255, 0);
                else
                    DrawScaleformMovie(_lobby.Handle, 0.6617187f, 0.7226667f, 1, 1, 255, 255, 255, 255, 0);
            }
        }
    }
}
