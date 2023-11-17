using CitizenFX.Core.Native;
using ScaleformUI.Scaleforms;
using System.Drawing;
using static CitizenFX.FiveM.Native.Natives;

namespace ScaleformUI.Elements
{
    public struct SColor
    {
        private readonly Color mainColor;
        private static Random rnd = new();
        public readonly bool IsEmpty => mainColor.IsEmpty;
        public readonly byte A => mainColor.A;
        public readonly byte B => mainColor.B;
        public readonly byte G => mainColor.G;
        public readonly byte R => mainColor.R;
        #region System Colors
        public static readonly SColor Transparent = FromArgb(16777215);
        public static readonly SColor AliceBlue = FromArgb(-984833);
        public static readonly SColor AntiqueWhite = FromArgb(-332841);
        public static readonly SColor Aqua = FromArgb(-16711681);
        public static readonly SColor Aquamarine = FromArgb(-8388652);
        public static readonly SColor Azure = FromArgb(-983041);
        public static readonly SColor Beige = FromArgb(-657956);
        public static readonly SColor Bisque = FromArgb(-6972);
        public static readonly SColor Black = FromArgb(-16777216);
        public static readonly SColor BlanchedAlmond = FromArgb(-5171);
        public static readonly SColor Blue = FromArgb(-16776961);
        public static readonly SColor BlueViolet = FromArgb(-7722014);
        public static readonly SColor Brown = FromArgb(-5952982);
        public static readonly SColor BurlyWood = FromArgb(-2180985);
        public static readonly SColor CadetBlue = FromArgb(-10510688);
        public static readonly SColor Chartreuse = FromArgb(-8388864);
        public static readonly SColor Chocolate = FromArgb(-2987746);
        public static readonly SColor Coral = FromArgb(-32944);
        public static readonly SColor CornflowerBlue = FromArgb(-10185235);
        public static readonly SColor Cornsilk = FromArgb(-1828);
        public static readonly SColor Crimson = FromArgb(-2354116);
        public static readonly SColor Cyan = FromArgb(-16711681);
        public static readonly SColor DarkBlue = FromArgb(-16777077);
        public static readonly SColor DarkCyan = FromArgb(-16741493);
        public static readonly SColor DarkGoldenrod = FromArgb(-4684277);
        public static readonly SColor DarkGray = FromArgb(-5658199);
        public static readonly SColor DarkGreen = FromArgb(-16751616);
        public static readonly SColor DarkKhaki = FromArgb(-4343957);
        public static readonly SColor DarkMagenta = FromArgb(-7667573);
        public static readonly SColor DarkOliveGreen = FromArgb(-11179217);
        public static readonly SColor DarkOrange = FromArgb(-29696);
        public static readonly SColor DarkOrchid = FromArgb(-6737204);
        public static readonly SColor DarkRed = FromArgb(-7667712);
        public static readonly SColor DarkSalmon = FromArgb(-1468806);
        public static readonly SColor DarkSeaGreen = FromArgb(-7357301);
        public static readonly SColor DarkSlateBlue = FromArgb(-12042869);
        public static readonly SColor DarkSlateGray = FromArgb(-13676721);
        public static readonly SColor DarkTurquoise = FromArgb(-16724271);
        public static readonly SColor DarkViolet = FromArgb(-7077677);
        public static readonly SColor DeepPink = FromArgb(-60269);
        public static readonly SColor DeepSkyBlue = FromArgb(-16728065);
        public static readonly SColor DimGray = FromArgb(-9868951);
        public static readonly SColor DodgerBlue = FromArgb(-14774017);
        public static readonly SColor Firebrick = FromArgb(-5103070);
        public static readonly SColor FloralWhite = FromArgb(-1296);
        public static readonly SColor ForestGreen = FromArgb(-14513374);
        public static readonly SColor Fuchsia = FromArgb(-65281);
        public static readonly SColor Gainsboro = FromArgb(-2302756);
        public static readonly SColor GhostWhite = FromArgb(-460545);
        public static readonly SColor Gold = FromArgb(-10496);
        public static readonly SColor Goldenrod = FromArgb(-2448096);
        public static readonly SColor Gray = FromArgb(-8355712);
        public static readonly SColor Green = FromArgb(-16744448);
        public static readonly SColor GreenYellow = FromArgb(-5374161);
        public static readonly SColor Honeydew = FromArgb(-983056);
        public static readonly SColor HotPink = FromArgb(-38476);
        public static readonly SColor IndianRed = FromArgb(-3318692);
        public static readonly SColor Indigo = FromArgb(-11861886);
        public static readonly SColor Ivory = FromArgb(-16);
        public static readonly SColor Khaki = FromArgb(-989556);
        public static readonly SColor Lavender = FromArgb(-1644806);
        public static readonly SColor LavenderBlush = FromArgb(-3851);
        public static readonly SColor LawnGreen = FromArgb(-8586240);
        public static readonly SColor LemonChiffon = FromArgb(-1331);
        public static readonly SColor LightBlue = FromArgb(-5383962);
        public static readonly SColor LightCoral = FromArgb(-1015680);
        public static readonly SColor LightCyan = FromArgb(-2031617);
        public static readonly SColor LightGoldenrodYellow = FromArgb(-329006);
        public static readonly SColor LightGreen = FromArgb(-7278960);
        public static readonly SColor LightGray = FromArgb(-2894893);
        public static readonly SColor LightPink = FromArgb(-18751);
        public static readonly SColor LightSalmon = FromArgb(-24454);
        public static readonly SColor LightSeaGreen = FromArgb(-14634326);
        public static readonly SColor LightSkyBlue = FromArgb(-7876870);
        public static readonly SColor LightSlateGray = FromArgb(-8943463);
        public static readonly SColor LightSteelBlue = FromArgb(-5192482);
        public static readonly SColor LightYellow = FromArgb(-32);
        public static readonly SColor Lime = FromArgb(-16711936);
        public static readonly SColor LimeGreen = FromArgb(-13447886);
        public static readonly SColor Linen = FromArgb(-331546);
        public static readonly SColor Magenta = FromArgb(-65281);
        public static readonly SColor Maroon = FromArgb(-8388608);
        public static readonly SColor MediumAquamarine = FromArgb(-10039894);
        public static readonly SColor MediumBlue = FromArgb(-16777011);
        public static readonly SColor MediumOrchid = FromArgb(-4565549);
        public static readonly SColor MediumPurple = FromArgb(-7114533);
        public static readonly SColor MediumSeaGreen = FromArgb(-12799119);
        public static readonly SColor MediumSlateBlue = FromArgb(-8689426);
        public static readonly SColor MediumSpringGreen = FromArgb(-16713062);
        public static readonly SColor MediumTurquoise = FromArgb(-12004916);
        public static readonly SColor MediumVioletRed = FromArgb(-3730043);
        public static readonly SColor MidnightBlue = FromArgb(-15132304);
        public static readonly SColor MintCream = FromArgb(-655366);
        public static readonly SColor MistyRose = FromArgb(-6943);
        public static readonly SColor Moccasin = FromArgb(-6987);
        public static readonly SColor NavajoWhite = FromArgb(-8531);
        public static readonly SColor Navy = FromArgb(-16777088);
        public static readonly SColor OldLace = FromArgb(-133658);
        public static readonly SColor Olive = FromArgb(-8355840);
        public static readonly SColor OliveDrab = FromArgb(-9728477);
        public static readonly SColor Orange = FromArgb(-23296);
        public static readonly SColor OrangeRed = FromArgb(-47872);
        public static readonly SColor Orchid = FromArgb(-2461482);
        public static readonly SColor PaleGoldenrod = FromArgb(-1120086);
        public static readonly SColor PaleGreen = FromArgb(-6751336);
        public static readonly SColor PaleTurquoise = FromArgb(-5247250);
        public static readonly SColor PaleVioletRed = FromArgb(-2396013);
        public static readonly SColor PapayaWhip = FromArgb(-4139);
        public static readonly SColor PeachPuff = FromArgb(-9543);
        public static readonly SColor Peru = FromArgb(-3308225);
        public static readonly SColor Pink = FromArgb(-16181);
        public static readonly SColor Plum = FromArgb(-2252579);
        public static readonly SColor PowderBlue = FromArgb(-5185306);
        public static readonly SColor Purple = FromArgb(-8388480);
        public static readonly SColor Red = FromArgb(-65536);
        public static readonly SColor RosyBrown = FromArgb(-4419697);
        public static readonly SColor RoyalBlue = FromArgb(-12490271);
        public static readonly SColor SaddleBrown = FromArgb(-7650029);
        public static readonly SColor Salmon = FromArgb(-360334);
        public static readonly SColor SandyBrown = FromArgb(-744352);
        public static readonly SColor SeaGreen = FromArgb(-13726889);
        public static readonly SColor SeaShell = FromArgb(-2578);
        public static readonly SColor Sienna = FromArgb(-6270419);
        public static readonly SColor Silver = FromArgb(-4144960);
        public static readonly SColor SkyBlue = FromArgb(-7876885);
        public static readonly SColor SlateBlue = FromArgb(-9807155);
        public static readonly SColor SlateGray = FromArgb(-9404272);
        public static readonly SColor Snow = FromArgb(-1286);
        public static readonly SColor SpringGreen = FromArgb(-16711809);
        public static readonly SColor SteelBlue = FromArgb(-12156236);
        public static readonly SColor Tan = FromArgb(-2968436);
        public static readonly SColor Teal = FromArgb(-16744320);
        public static readonly SColor Thistle = FromArgb(-2572328);
        public static readonly SColor Tomato = FromArgb(-40121);
        public static readonly SColor Turquoise = FromArgb(-12525360);
        public static readonly SColor Violet = FromArgb(-1146130);
        public static readonly SColor Wheat = FromArgb(-663885);
        public static readonly SColor White = FromArgb(-1);
        public static readonly SColor WhiteSmoke = FromArgb(-657931);
        public static readonly SColor Yellow = FromArgb(-256);
        public static readonly SColor YellowGreen = FromArgb(-6632142);
        #endregion
        #region HUD_Colors
        public static readonly SColor HUD_None = FromHudColor((HudColor)(-1));
        public static readonly SColor HUD_Pure_white = FromHudColor(0);
        public static readonly SColor HUD_White = FromHudColor((HudColor)1);
        public static readonly SColor HUD_Black = FromHudColor((HudColor)2);
        public static readonly SColor HUD_Grey = FromHudColor((HudColor)3);
        public static readonly SColor HUD_Greylight = FromHudColor((HudColor)4);
        public static readonly SColor HUD_Greydark = FromHudColor((HudColor)5);
        public static readonly SColor HUD_Red = FromHudColor((HudColor)6);
        public static readonly SColor HUD_Redlight = FromHudColor((HudColor)7);
        public static readonly SColor HUD_Reddark = FromHudColor((HudColor)8);
        public static readonly SColor HUD_Blue = FromHudColor((HudColor)9);
        public static readonly SColor HUD_Bluelight = FromHudColor((HudColor)10);
        public static readonly SColor HUD_Bluedark = FromHudColor((HudColor)11);
        public static readonly SColor HUD_Yellow = FromHudColor((HudColor)12);
        public static readonly SColor HUD_Yellowlight = FromHudColor((HudColor)13);
        public static readonly SColor HUD_Yellowdark = FromHudColor((HudColor)14);
        public static readonly SColor HUD_Orange = FromHudColor((HudColor)15);
        public static readonly SColor HUD_Orangelight = FromHudColor((HudColor)16);
        public static readonly SColor HUD_Orangedark = FromHudColor((HudColor)17);
        public static readonly SColor HUD_Green = FromHudColor((HudColor)18);
        public static readonly SColor HUD_Greenlight = FromHudColor((HudColor)19);
        public static readonly SColor HUD_Greendark = FromHudColor((HudColor)20);
        public static readonly SColor HUD_Purple = FromHudColor((HudColor)21);
        public static readonly SColor HUD_Purplelight = FromHudColor((HudColor)22);
        public static readonly SColor HUD_Purpledark = FromHudColor((HudColor)23);
        public static readonly SColor HUD_Pink = FromHudColor((HudColor)24);
        public static readonly SColor HUD_Radar_health = FromHudColor((HudColor)25);
        public static readonly SColor HUD_Radar_armour = FromHudColor((HudColor)26);
        public static readonly SColor HUD_Radar_damage = FromHudColor((HudColor)27);
        public static readonly SColor HUD_Net_player1 = FromHudColor((HudColor)28);
        public static readonly SColor HUD_Net_player2 = FromHudColor((HudColor)29);
        public static readonly SColor HUD_Net_player3 = FromHudColor((HudColor)30);
        public static readonly SColor HUD_Net_player4 = FromHudColor((HudColor)31);
        public static readonly SColor HUD_Net_player5 = FromHudColor((HudColor)32);
        public static readonly SColor HUD_Net_player6 = FromHudColor((HudColor)33);
        public static readonly SColor HUD_Net_player7 = FromHudColor((HudColor)34);
        public static readonly SColor HUD_Net_player8 = FromHudColor((HudColor)35);
        public static readonly SColor HUD_Net_player9 = FromHudColor((HudColor)36);
        public static readonly SColor HUD_Net_player10 = FromHudColor((HudColor)37);
        public static readonly SColor HUD_Net_player11 = FromHudColor((HudColor)38);
        public static readonly SColor HUD_Net_player12 = FromHudColor((HudColor)39);
        public static readonly SColor HUD_Net_player13 = FromHudColor((HudColor)40);
        public static readonly SColor HUD_Net_player14 = FromHudColor((HudColor)41);
        public static readonly SColor HUD_Net_player15 = FromHudColor((HudColor)42);
        public static readonly SColor HUD_Net_player16 = FromHudColor((HudColor)43);
        public static readonly SColor HUD_Net_player17 = FromHudColor((HudColor)44);
        public static readonly SColor HUD_Net_player18 = FromHudColor((HudColor)45);
        public static readonly SColor HUD_Net_player19 = FromHudColor((HudColor)46);
        public static readonly SColor HUD_Net_player20 = FromHudColor((HudColor)47);
        public static readonly SColor HUD_Net_player21 = FromHudColor((HudColor)48);
        public static readonly SColor HUD_Net_player22 = FromHudColor((HudColor)49);
        public static readonly SColor HUD_Net_player23 = FromHudColor((HudColor)50);
        public static readonly SColor HUD_Net_player24 = FromHudColor((HudColor)51);
        public static readonly SColor HUD_Net_player25 = FromHudColor((HudColor)52);
        public static readonly SColor HUD_Net_player26 = FromHudColor((HudColor)53);
        public static readonly SColor HUD_Net_player27 = FromHudColor((HudColor)54);
        public static readonly SColor HUD_Net_player28 = FromHudColor((HudColor)55);
        public static readonly SColor HUD_Net_player29 = FromHudColor((HudColor)56);
        public static readonly SColor HUD_Net_player30 = FromHudColor((HudColor)57);
        public static readonly SColor HUD_Net_player31 = FromHudColor((HudColor)58);
        public static readonly SColor HUD_Net_player32 = FromHudColor((HudColor)59);
        public static readonly SColor HUD_Simpleblip_default = FromHudColor((HudColor)60);
        public static readonly SColor HUD_Menu_blue = FromHudColor((HudColor)61);
        public static readonly SColor HUD_Menu_grey_light = FromHudColor((HudColor)62);
        public static readonly SColor HUD_Menu_blue_extra_dark = FromHudColor((HudColor)63);
        public static readonly SColor HUD_Menu_yellow = FromHudColor((HudColor)64);
        public static readonly SColor HUD_Menu_yellow_dark = FromHudColor((HudColor)65);
        public static readonly SColor HUD_Menu_green = FromHudColor((HudColor)66);
        public static readonly SColor HUD_Menu_grey = FromHudColor((HudColor)67);
        public static readonly SColor HUD_Menu_grey_dark = FromHudColor((HudColor)68);
        public static readonly SColor HUD_Menu_highlight = FromHudColor((HudColor)69);
        public static readonly SColor HUD_Menu_standard = FromHudColor((HudColor)70);
        public static readonly SColor HUD_Menu_dimmed = FromHudColor((HudColor)71);
        public static readonly SColor HUD_Menu_extra_dimmed = FromHudColor((HudColor)72);
        public static readonly SColor HUD_Brief_title = FromHudColor((HudColor)73);
        public static readonly SColor HUD_Mid_grey_mp = FromHudColor((HudColor)74);
        public static readonly SColor HUD_Net_player1_dark = FromHudColor((HudColor)75);
        public static readonly SColor HUD_Net_player2_dark = FromHudColor((HudColor)76);
        public static readonly SColor HUD_Net_player3_dark = FromHudColor((HudColor)77);
        public static readonly SColor HUD_Net_player4_dark = FromHudColor((HudColor)78);
        public static readonly SColor HUD_Net_player5_dark = FromHudColor((HudColor)79);
        public static readonly SColor HUD_Net_player6_dark = FromHudColor((HudColor)80);
        public static readonly SColor HUD_Net_player7_dark = FromHudColor((HudColor)81);
        public static readonly SColor HUD_Net_player8_dark = FromHudColor((HudColor)82);
        public static readonly SColor HUD_Net_player9_dark = FromHudColor((HudColor)83);
        public static readonly SColor HUD_Net_player10_dark = FromHudColor((HudColor)84);
        public static readonly SColor HUD_Net_player11_dark = FromHudColor((HudColor)85);
        public static readonly SColor HUD_Net_player12_dark = FromHudColor((HudColor)86);
        public static readonly SColor HUD_Net_player13_dark = FromHudColor((HudColor)87);
        public static readonly SColor HUD_Net_player14_dark = FromHudColor((HudColor)88);
        public static readonly SColor HUD_Net_player15_dark = FromHudColor((HudColor)89);
        public static readonly SColor HUD_Net_player16_dark = FromHudColor((HudColor)90);
        public static readonly SColor HUD_Net_player17_dark = FromHudColor((HudColor)91);
        public static readonly SColor HUD_Net_player18_dark = FromHudColor((HudColor)92);
        public static readonly SColor HUD_Net_player19_dark = FromHudColor((HudColor)93);
        public static readonly SColor HUD_Net_player20_dark = FromHudColor((HudColor)94);
        public static readonly SColor HUD_Net_player21_dark = FromHudColor((HudColor)95);
        public static readonly SColor HUD_Net_player22_dark = FromHudColor((HudColor)96);
        public static readonly SColor HUD_Net_player23_dark = FromHudColor((HudColor)97);
        public static readonly SColor HUD_Net_player24_dark = FromHudColor((HudColor)98);
        public static readonly SColor HUD_Net_player25_dark = FromHudColor((HudColor)99);
        public static readonly SColor HUD_Net_player26_dark = FromHudColor((HudColor)100);
        public static readonly SColor HUD_Net_player27_dark = FromHudColor((HudColor)101);
        public static readonly SColor HUD_Net_player28_dark = FromHudColor((HudColor)102);
        public static readonly SColor HUD_Net_player29_dark = FromHudColor((HudColor)103);
        public static readonly SColor HUD_Net_player30_dark = FromHudColor((HudColor)104);
        public static readonly SColor HUD_Net_player31_dark = FromHudColor((HudColor)105);
        public static readonly SColor HUD_Net_player32_dark = FromHudColor((HudColor)106);
        public static readonly SColor HUD_Bronze = FromHudColor((HudColor)107);
        public static readonly SColor HUD_Silver = FromHudColor((HudColor)108);
        public static readonly SColor HUD_Gold = FromHudColor((HudColor)109);
        public static readonly SColor HUD_Platinum = FromHudColor((HudColor)110);
        public static readonly SColor HUD_Gang1 = FromHudColor((HudColor)111);
        public static readonly SColor HUD_Gang2 = FromHudColor((HudColor)112);
        public static readonly SColor HUD_Gang3 = FromHudColor((HudColor)113);
        public static readonly SColor HUD_Gang4 = FromHudColor((HudColor)114);
        public static readonly SColor HUD_Same_crew = FromHudColor((HudColor)115);
        public static readonly SColor HUD_Freemode = FromHudColor((HudColor)116);
        public static readonly SColor HUD_Pause_bg = FromHudColor((HudColor)117);
        public static readonly SColor HUD_Friendly = FromHudColor((HudColor)118);
        public static readonly SColor HUD_Enemy = FromHudColor((HudColor)119);
        public static readonly SColor HUD_Location = FromHudColor((HudColor)120);
        public static readonly SColor HUD_Pickup = FromHudColor((HudColor)121);
        public static readonly SColor HUD_Pause_singleplayer = FromHudColor((HudColor)122);
        public static readonly SColor HUD_Freemode_dark = FromHudColor((HudColor)123);
        public static readonly SColor HUD_Inactive_mission = FromHudColor((HudColor)124);
        public static readonly SColor HUD_Damage = FromHudColor((HudColor)125);
        public static readonly SColor HUD_Pinklight = FromHudColor((HudColor)126);
        public static readonly SColor HUD_Pm_mitem_highlight = FromHudColor((HudColor)127);
        public static readonly SColor HUD_Script_variable = FromHudColor((HudColor)128);
        public static readonly SColor HUD_Yoga = FromHudColor((HudColor)129);
        public static readonly SColor HUD_Tennis = FromHudColor((HudColor)130);
        public static readonly SColor HUD_Golf = FromHudColor((HudColor)131);
        public static readonly SColor HUD_Shooting_range = FromHudColor((HudColor)132);
        public static readonly SColor HUD_Flight_school = FromHudColor((HudColor)133);
        public static readonly SColor HUD_North_blue = FromHudColor((HudColor)134);
        public static readonly SColor HUD_Social_club = FromHudColor((HudColor)135);
        public static readonly SColor HUD_Platform_blue = FromHudColor((HudColor)136);
        public static readonly SColor HUD_Platform_green = FromHudColor((HudColor)137);
        public static readonly SColor HUD_Platform_grey = FromHudColor((HudColor)138);
        public static readonly SColor HUD_Facebook_blue = FromHudColor((HudColor)139);
        public static readonly SColor HUD_Ingame_bg = FromHudColor((HudColor)140);
        public static readonly SColor HUD_Darts = FromHudColor((HudColor)141);
        public static readonly SColor HUD_Waypoint = FromHudColor((HudColor)142);
        public static readonly SColor HUD_Michael = FromHudColor((HudColor)143);
        public static readonly SColor HUD_Franklin = FromHudColor((HudColor)144);
        public static readonly SColor HUD_Trevor = FromHudColor((HudColor)145);
        public static readonly SColor HUD_Golf_p1 = FromHudColor((HudColor)146);
        public static readonly SColor HUD_Golf_p2 = FromHudColor((HudColor)147);
        public static readonly SColor HUD_Golf_p3 = FromHudColor((HudColor)148);
        public static readonly SColor HUD_Golf_p4 = FromHudColor((HudColor)149);
        public static readonly SColor HUD_Waypointlight = FromHudColor((HudColor)150);
        public static readonly SColor HUD_Waypointdark = FromHudColor((HudColor)151);
        public static readonly SColor HUD_Panel_light = FromHudColor((HudColor)152);
        public static readonly SColor HUD_Michael_dark = FromHudColor((HudColor)153);
        public static readonly SColor HUD_Franklin_dark = FromHudColor((HudColor)154);
        public static readonly SColor HUD_Trevor_dark = FromHudColor((HudColor)155);
        public static readonly SColor HUD_Objective_route = FromHudColor((HudColor)156);
        public static readonly SColor HUD_Pausemap_tint = FromHudColor((HudColor)157);
        public static readonly SColor HUD_Pause_deselect = FromHudColor((HudColor)158);
        public static readonly SColor HUD_Pm_weapons_purchasable = FromHudColor((HudColor)159);
        public static readonly SColor HUD_Pm_weapons_locked = FromHudColor((HudColor)160);
        public static readonly SColor HUD_End_screen_bg = FromHudColor((HudColor)161);
        public static readonly SColor HUD_Chop = FromHudColor((HudColor)162);
        public static readonly SColor HUD_Pausemap_tint_half = FromHudColor((HudColor)163);
        public static readonly SColor HUD_North_blue_official = FromHudColor((HudColor)164);
        public static readonly SColor HUD_Script_variable_2 = FromHudColor((HudColor)165);
        public static readonly SColor HUD_H = FromHudColor((HudColor)166);
        public static readonly SColor HUD_Hdark = FromHudColor((HudColor)167);
        public static readonly SColor HUD_T = FromHudColor((HudColor)168);
        public static readonly SColor HUD_Tdark = FromHudColor((HudColor)169);
        public static readonly SColor HUD_Hshard = FromHudColor((HudColor)170);
        public static readonly SColor HUD_Controller_michael = FromHudColor((HudColor)171);
        public static readonly SColor HUD_Controller_franklin = FromHudColor((HudColor)172);
        public static readonly SColor HUD_Controller_trevor = FromHudColor((HudColor)173);
        public static readonly SColor HUD_Controller_chop = FromHudColor((HudColor)174);
        public static readonly SColor HUD_Video_editor_video = FromHudColor((HudColor)175);
        public static readonly SColor HUD_Video_editor_audio = FromHudColor((HudColor)176);
        public static readonly SColor HUD_Video_editor_text = FromHudColor((HudColor)177);
        public static readonly SColor HUD_Hb_blue = FromHudColor((HudColor)178);
        public static readonly SColor HUD_Hb_yellow = FromHudColor((HudColor)179);
        public static readonly SColor HUD_Video_editor_score = FromHudColor((HudColor)180);
        public static readonly SColor HUD_Video_editor_audio_fadeout = FromHudColor((HudColor)181);
        public static readonly SColor HUD_Video_editor_text_fadeout = FromHudColor((HudColor)182);
        public static readonly SColor HUD_Video_editor_score_fadeout = FromHudColor((HudColor)183);
        public static readonly SColor HUD_Heist_background = FromHudColor((HudColor)184);
        public static readonly SColor HUD_Video_editor_ambient = FromHudColor((HudColor)185);
        public static readonly SColor HUD_Video_editor_ambient_fadeout = FromHudColor((HudColor)186);
        public static readonly SColor HUD_Gb = FromHudColor((HudColor)187);
        public static readonly SColor HUD_G = FromHudColor((HudColor)188);
        public static readonly SColor HUD_B = FromHudColor((HudColor)189);
        public static readonly SColor HUD_Low_flow = FromHudColor((HudColor)190);
        public static readonly SColor HUD_Low_flow_dark = FromHudColor((HudColor)191);
        public static readonly SColor HUD_G1 = FromHudColor((HudColor)192);
        public static readonly SColor HUD_G2 = FromHudColor((HudColor)193);
        public static readonly SColor HUD_G3 = FromHudColor((HudColor)194);
        public static readonly SColor HUD_G4 = FromHudColor((HudColor)195);
        public static readonly SColor HUD_G5 = FromHudColor((HudColor)196);
        public static readonly SColor HUD_G6 = FromHudColor((HudColor)197);
        public static readonly SColor HUD_G7 = FromHudColor((HudColor)198);
        public static readonly SColor HUD_G8 = FromHudColor((HudColor)199);
        public static readonly SColor HUD_G9 = FromHudColor((HudColor)200);
        public static readonly SColor HUD_G10 = FromHudColor((HudColor)201);
        public static readonly SColor HUD_G11 = FromHudColor((HudColor)202);
        public static readonly SColor HUD_G12 = FromHudColor((HudColor)203);
        public static readonly SColor HUD_G13 = FromHudColor((HudColor)204);
        public static readonly SColor HUD_G14 = FromHudColor((HudColor)205);
        public static readonly SColor HUD_G15 = FromHudColor((HudColor)206);
        public static readonly SColor HUD_Adversary = FromHudColor((HudColor)207);
        public static readonly SColor HUD_Degen_red = FromHudColor((HudColor)208);
        public static readonly SColor HUD_Degen_yellow = FromHudColor((HudColor)209);
        public static readonly SColor HUD_Degen_green = FromHudColor((HudColor)210);
        public static readonly SColor HUD_Degen_cyan = FromHudColor((HudColor)211);
        public static readonly SColor HUD_Degen_blue = FromHudColor((HudColor)212);
        public static readonly SColor HUD_Degen_magenta = FromHudColor((HudColor)213);
        public static readonly SColor HUD_Stunt_1 = FromHudColor((HudColor)214);
        public static readonly SColor HUD_Stunt_2 = FromHudColor((HudColor)215);
        public static readonly SColor HUD_Special_race_series = FromHudColor((HudColor)216);
        public static readonly SColor HUD_Special_race_series_dark = FromHudColor((HudColor)217);
        public static readonly SColor HUD_Cs = FromHudColor((HudColor)218);
        public static readonly SColor HUD_Cs_dark = FromHudColor((HudColor)219);
        public static readonly SColor HUD_Tech_green = FromHudColor((HudColor)220);
        public static readonly SColor HUD_Tech_green_dark = FromHudColor((HudColor)221);
        public static readonly SColor HUD_Tech_red = FromHudColor((HudColor)222);
        public static readonly SColor HUD_Tech_green_very_dark = FromHudColor((HudColor)223);
        #endregion

        public SColor(string hexColor)
        {
            if (hexColor.StartsWith("#"))
            {
                string argbHexString = hexColor.Substring(1);
                int argbValue = int.Parse(argbHexString, System.Globalization.NumberStyles.HexNumber);
                mainColor = Color.FromArgb(argbValue);
            }
            else throw new Exception("Invalid Hex value");
        }

        public SColor(Color color)
        {
            mainColor = color;
        }

        public SColor(HudColor color)
        {
            mainColor = FromHudColor(color).mainColor;
        }


        public static SColor FromHudColor(HudColor color)
        {
            int r = 0, g = 0, b = 0, a = 0;
            GetHudColour((int)color, ref r, ref g, ref b, ref a);
            return FromArgb(a, r, g, b);
        }

        public static SColor FromRandomValues()
        {
            return FromColor(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
        }

        public static SColor FromArgb(int argb) => new(Color.FromArgb(argb));

        public static SColor FromArgb(int alpha, int red, int green, int blue) => new(Color.FromArgb(alpha, red, green, blue));

        public static SColor FromArgb(int alpha, Color baseColor) => new(Color.FromArgb(alpha, baseColor));

        public static SColor FromArgb(int red, int green, int blue) => FromArgb(byte.MaxValue, red, green, blue);

        public static SColor FromColor(Color color) => new(color);

        public float GetBrightness() => mainColor.GetBrightness();

        public float GetHue() => mainColor.GetHue();

        public float GetSaturation() => mainColor.GetSaturation();

        public int ArgbValue => ToArgb();
        public string HexValue => ToHex();
        public int ToArgb() => mainColor.ToArgb();
        public string ToHex() => $"#{mainColor.A:X2}{mainColor.R:X2}{mainColor.G:X2}{mainColor.B:X2}";
        public KnownColor ToKnownColor() => mainColor.ToKnownColor();
        public Color ToColor() => mainColor;

        public static bool operator ==(SColor left, SColor right) => left.mainColor == right.mainColor;

        public static bool operator !=(SColor left, SColor right) => !(left == right);

        public override bool Equals(object? obj) => obj is SColor other && Equals(other);

        public bool Equals(SColor other) => this == other;

        public override string ToString()
        {
            return mainColor.ToString() + $" - INT: {ToArgb()} - UINT: {(uint)ToArgb()}  - HEX: {ToHex()}";
        }
        public override int GetHashCode()
        {
            return mainColor.GetHashCode();
        }
    }
}
