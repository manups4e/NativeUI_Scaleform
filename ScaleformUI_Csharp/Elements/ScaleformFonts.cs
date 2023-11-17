using CitizenFX.Core.Native;
using static CitizenFX.FiveM.Native.Natives;

namespace ScaleformUI.Elements
{
    public static class ScaleformFonts
    {
        public static ItemFont CHALET_LONDON_NINETEENSIXTY = new("$Font2");
        public static ItemFont SIGNPAINTER_HOUSESCRIPT = new("$Font5");
        public static ItemFont GTAV_LEADERBOARD = new("$GTAVLeaderboard"); // weird symbols.. use only numbers!
        public static ItemFont CHALET_COMPRIME_COLOGNE_SIXTY = new("$Font2_cond");
        public static ItemFont CHALET_LONDON_NINETEENSIXTY_NUMB = new("$FixedWidthNumbers");
        public static ItemFont PRICEDOWN_GTAV_INT = new("$gtaCash");
        public static ItemFont CHALET_COMPRIME_COLOGNESIXTY_NOT_GAMERNAME = new("$Font2_cond_NOT_GAMERNAME");
        public static ItemFont GTAV_TIMER_FIXED_COND = new("$Font2_cond_MPTimer");
        public static ItemFont ROCKSTAR_TAG = new("$RockstarTAG");
        public static ItemFont HANDSTYLE_HEIST = ItemFont.RegisterFont("font_lib_editor", "$HandstyleHeist");
        public static ItemFont REDEMPTION = ItemFont.RegisterFont("font_lib_editor", "$Redemption");
        public static ItemFont GTA_SUBTRACT = ItemFont.RegisterFont("font_lib_editor", "$Subtract");
        public static ItemFont HELVETICA_55 = ItemFont.RegisterFont("font_lib_editor", "$Helvetica55");
        public static ItemFont DIN = ItemFont.RegisterFont("font_lib_editor", "$Din");
        public static ItemFont HELVETICA_107 = ItemFont.RegisterFont("font_lib_editor", "$Helvetica107");
        public static ItemFont UNIVERS = ItemFont.RegisterFont("font_lib_editor", "$Univers");
        public static ItemFont SUBTRACT = ItemFont.RegisterFont("font_lib_heist4", "$Subtract");
        public static ItemFont HELVETICANEUE_LT_47_LIGHTCN = ItemFont.RegisterFont("font_lib_sc", "$SOCIAL_CLUB_COND_REG");
        public static ItemFont HELVETICANEUE_LT_67_MDCN = ItemFont.RegisterFont("font_lib_sc", "$SOCIAL_CLUB_COND_BOLD");
        public static ItemFont GTAV_TAXI_DIGITAL = ItemFont.RegisterFont("font_lib_taxi", "$Taxi_font");
        public static ItemFont GTAV_COURIER = ItemFont.RegisterFont("font_lib_typewriter", "$Courier");
        public static ItemFont ANNA_SC_ITC_TT = ItemFont.RegisterFont("font_lib_org", "$AnnaSC");
        public static ItemFont BAUHAUSITCTT = ItemFont.RegisterFont("font_lib_org", "$Bauhaus");
        public static ItemFont BOOKMANMDITCTT = ItemFont.RegisterFont("font_lib_org", "$Bookman");
        public static ItemFont ENGRAVERS_OLD_ENGLISH_MT_STD = ItemFont.RegisterFont("font_lib_org", "$EngraversOldEnglish");
        public static ItemFont HELVETICA_NEUE_LT_COM_95_BLK = ItemFont.RegisterFont("font_lib_org", "$HelveticaBLK");
        public static ItemFont HELVETICA_NEUE_LT_COM_96_BLKIT = ItemFont.RegisterFont("font_lib_org", "$HelveticaBLKI");
        public static ItemFont LUBALINGRAPHMDITCTT = ItemFont.RegisterFont("font_lib_org", "$Lubalin");
        public static ItemFont MISTRAL_STD = ItemFont.RegisterFont("font_lib_org", "$Mistral");
        public static ItemFont STENBERGITC_TT = ItemFont.RegisterFont("font_lib_org", "$Stenberg");
        public static ItemFont STENCIL_STD = ItemFont.RegisterFont("font_lib_org", "$Stencil");
        public static ItemFont TIMES_NEW_ROMAN = ItemFont.RegisterFont("font_lib_org", "$Times");
        public static ItemFont TRADE_GOTHIC_LT_COM_BOLD = ItemFont.RegisterFont("font_lib_org", "$TradeGothic");
        public static ItemFont ITC_MACHINE_STD = ItemFont.RegisterFont("font_lib_org", "$Machine");
        public static ItemFont HELVETICANEUELT_W1G_55_ROMAN = ItemFont.RegisterFont("font_lib_org", "$WebFont1_Hbold");
        public static ItemFont HELVETICANEUELT_W1G_55_ROMAN_2 = ItemFont.RegisterFont("font_lib_org", "$WebFont2_Hitalic");
        public static ItemFont TIMES_NEW_ROMAN_CE = ItemFont.RegisterFont("font_lib_org", "$WebFont3_Times");
        public static ItemFont COURIER_TWELVE_MT_STD = ItemFont.RegisterFont("font_lib_org", "$WebFont4_Courier");
        public static ItemFont BELL_GOTHIC_BLACK = ItemFont.RegisterFont("font_lib_org", "$WebFont5_BellGothic");
    }

    public class ItemFont
    {
        public string FontName;
        public int FontID = 0;
        internal ItemFont(string fontName)
        {
            this.FontName = fontName;
        }
        /// <summary>
        /// Creates a new ItemFont, to register a new font please use ItemFont.RegisterFont()
        /// </summary>
        /// <param name="fontName">Name of the font</param>
        /// <param name="fontId">Its registered id</param>
        public ItemFont(string fontName, int fontId)
        {
            this.FontName = fontName;
            this.FontID = fontId;
        }

        public static ItemFont RegisterFont(string gfxName, string fontName)
        {
            ItemFont ret = new ItemFont(fontName);
            RegisterFontFile(gfxName);
            ret.FontID = RegisterFontId(fontName);
            return ret;
        }
    }
}
