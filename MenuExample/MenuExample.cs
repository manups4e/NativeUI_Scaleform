using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.FiveM.GUI;
using static CitizenFX.FiveM.Native.Natives;

using ScaleformUI;
using ScaleformUI.Elements;
using ScaleformUI.LobbyMenu;
using ScaleformUI.Menu;
using ScaleformUI.PauseMenu;
using ScaleformUI.PauseMenus;
using ScaleformUI.Radial;
using ScaleformUI.Radio;
using ScaleformUI.Scaleforms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.FiveM;

public class MenuExample : BaseScript
{
    private bool enabled = true;
    private string dish = "Banana";
    //private TimerBarPool _timerBarPool;
    private long txd;
    private Random Random = new Random(GetGameTimer());
    #region UIMenu
    public async void ExampleMenu()
    {
        long _titledui = CreateDui("https://i.imgur.com/3yrFYbF.gif", 288, 130);
        CreateRuntimeTextureFromDuiHandle(txd, "bannerbackground", GetDuiHandle(_titledui));

        // first true means add menu Glare scaleform to the menu
        // last true means it's using the alternative title style
        UIMenu exampleMenu = new UIMenu("ScaleformUI", "ScaleformUI ~o~SHOWCASE", new PointF(20, 20), "scaleformui", "bannerbackground", true, true);
        exampleMenu.MaxItemsOnScreen = 7; // To decide max items on screen at time, default 7
        exampleMenu.BuildingAnimation = MenuBuildingAnimation.LEFT_RIGHT;
        exampleMenu.AnimationType = MenuAnimationType.BACK_INOUT;
        exampleMenu.ScrollingType = ScrollingType.CLASSIC;
        exampleMenu.SetMouse(true, false, true, false, false);

        //exampleMenu.CounterColor = HudColor.HUD_COLOUR_PINK;
        // let's add the menu to the Pool
        #region Menu Declaration

        #region Big Message

        UIMenuItem bigMessageItem = new UIMenuItem("~g~Big ~w~Message ~r~Examples", "Select me to switch to the BigMessage menu!");
        UIMenu uiMenuBigMessage = new UIMenu("Big Message", "Big Message");
        exampleMenu.AddItem(bigMessageItem);
        UIMenuListItem uiListBigMessageTransition = new UIMenuListItem("Big Message", new List<dynamic>() { "TRANSITION_OUT", "TRANSITION_UP", "TRANSITION_DOWN" }, 0);
        uiListBigMessageTransition.Description = "Transition type for the big message when disposing";
        uiMenuBigMessage.AddItem(uiListBigMessageTransition);

        UIMenuCheckboxItem uiCheckboxBigMessageManualDispose = new UIMenuCheckboxItem("Manual Dispose", false, "If enabled, you will have to manually dispose the big message");
        uiMenuBigMessage.AddItem(uiCheckboxBigMessageManualDispose);

        UIMenuListItem uiListBigMessageType = new UIMenuListItem("Message Type", new List<dynamic>() { "Mission Passed", "Coloured Shard", "Old Message", "Simple Shard", "Rank Up", "MP Message Large",
            "MP Wasted Message", "Mission Passed: Label" }, 0);
        uiListBigMessageType.Description = "Message type for the big message, press ~INPUT_FRONTEND_ACCEPT~ to show the message";
        uiMenuBigMessage.AddItem(uiListBigMessageType);

        UIMenuItem uiItemBigMessageDispose = new UIMenuItem("Dispose Big Message", "Dispose the big message");
        uiItemBigMessageDispose.Enabled = false;
        uiMenuBigMessage.AddItem(uiItemBigMessageDispose);

        uiMenuBigMessage.OnCheckboxChange += (sender, item, _checked) =>
        {
            if (item == uiCheckboxBigMessageManualDispose)
            {
                if (_checked)
                    uiItemBigMessageDispose.Enabled = true;
                else
                    uiItemBigMessageDispose.Enabled = false;
            }
        };

        uiMenuBigMessage.OnItemSelect += (sender, item, index) =>
        {
            if (item == uiItemBigMessageDispose)
            {
                if (uiCheckboxBigMessageManualDispose.Checked)
                    ScaleformUI.Main.BigMessageInstance.Dispose();
            }
        };

        uiMenuBigMessage.OnListSelect += (sender, item, index) =>
        {
            if (item == uiListBigMessageTransition)
            {
                switch (index)
                {
                    case 0:
                        ScaleformUI.Main.BigMessageInstance.Transition = "TRANSITION_OUT";
                        break;
                    case 1:
                        ScaleformUI.Main.BigMessageInstance.Transition = "TRANSITION_UP";
                        break;
                    case 2:
                        ScaleformUI.Main.BigMessageInstance.Transition = "TRANSITION_Down";
                        break;
                }
            }
            else if (item == uiListBigMessageType)
            {
                switch (index)
                {
                    case 0:
                        ScaleformUI.Main.BigMessageInstance.ShowMissionPassedMessage("Mission Passed", manualDispose: uiCheckboxBigMessageManualDispose.Checked);
                        break;
                    case 1:
                        ScaleformUI.Main.BigMessageInstance.ShowColoredShard("Coloured Shard", "Showing the coloured shared", HudColor.HUD_COLOUR_WHITE, HudColor.HUD_COLOUR_FREEMODE, manualDispose: uiCheckboxBigMessageManualDispose.Checked);
                        break;
                    case 2:
                        ScaleformUI.Main.BigMessageInstance.ShowOldMessage("Old Message", manualDispose: uiCheckboxBigMessageManualDispose.Checked);
                        break;
                    case 3:
                        ScaleformUI.Main.BigMessageInstance.ShowSimpleShard("Simple Shard", "Showing the simple shard", manualDispose: uiCheckboxBigMessageManualDispose.Checked);
                        break;
                    case 4:
                        ScaleformUI.Main.BigMessageInstance.ShowRankupMessage("Rank Up", "Showing the rank up message", 10, manualDispose: uiCheckboxBigMessageManualDispose.Checked);
                        break;
                    case 5:
                        ScaleformUI.Main.BigMessageInstance.ShowMpMessageLarge("MP Message Large", manualDispose: uiCheckboxBigMessageManualDispose.Checked);
                        break;
                    case 6:
                        ScaleformUI.Main.BigMessageInstance.ShowMpWastedMessage("MP Wasted Message", "Wasted", manualDispose: uiCheckboxBigMessageManualDispose.Checked);
                        break;
                    case 7:
                        const string CUSTOM_LABEL = "SCALEFORMUI_CUSTOM_LABEL";
                        AddTextEntry(CUSTOM_LABEL, "ScaleformUI is the best solution!");
                        ScaleformLabel scaleformLabel = CUSTOM_LABEL;
                        ScaleformUI.Main.BigMessageInstance.ShowMissionPassedMessage(scaleformLabel, manualDispose: uiCheckboxBigMessageManualDispose.Checked);
                        break;
                }
            }
        };

        bigMessageItem.Activated += (menu, item) =>
        {
            menu.SwitchTo(uiMenuBigMessage, inheritOldMenuParams: true);
        };

        #endregion

        UIMenuCheckboxItem ketchupItem = new UIMenuCheckboxItem("~g~Scrolling animation enabled? ~b~in a very long label to ~o~test the text scrolling feature!", UIMenuCheckboxStyle.Tick, enabled, "Do you wish to enable the scrolling animation?");
        long _paneldui = CreateDui("https://i.imgur.com/mH0Y65C.gif", 288, 160);
        CreateRuntimeTextureFromDuiHandle(txd, "panelbackground", GetDuiHandle(_paneldui));
        UIMissionDetailsPanel sidePanel = new UIMissionDetailsPanel(PanelSide.Right, "Side Panel", true, "scaleformui", "panelbackground");
        UIFreemodeDetailsItem detailItem1 = new UIFreemodeDetailsItem("Left Label", "RIGHT LABEL", ScaleformFonts.SIGNPAINTER_HOUSESCRIPT, ScaleformFonts.GTAV_TAXI_DIGITAL, BadgeIcon.BRIEFCASE, SColor.FromRandomValues());
        UIFreemodeDetailsItem detailItem2 = new UIFreemodeDetailsItem("Left Label", "RIGHT LABEL", ScaleformFonts.SIGNPAINTER_HOUSESCRIPT, ScaleformFonts.GTAV_TAXI_DIGITAL, BadgeIcon.MISSION_STAR, SColor.FromRandomValues());
        UIFreemodeDetailsItem detailItem3 = new UIFreemodeDetailsItem("Left Label", "RIGHT LABEL", ScaleformFonts.SIGNPAINTER_HOUSESCRIPT, ScaleformFonts.GTAV_TAXI_DIGITAL, BadgeIcon.ARMOR, SColor.FromRandomValues());
        UIFreemodeDetailsItem detailItem4 = new UIFreemodeDetailsItem("Left Label", "RIGHT LABEL", ScaleformFonts.SIGNPAINTER_HOUSESCRIPT, ScaleformFonts.GTAV_TAXI_DIGITAL, BadgeIcon.BRAND_DILETTANTE, SColor.FromRandomValues());
        UIFreemodeDetailsItem detailItem5 = new UIFreemodeDetailsItem("Left Label", "RIGHT LABEL", ScaleformFonts.SIGNPAINTER_HOUSESCRIPT, ScaleformFonts.GTAV_TAXI_DIGITAL, BadgeIcon.COUNTRY_ITALY, SColor.White);
        sidePanel.AddItem(detailItem1);
        sidePanel.AddItem(detailItem2);
        sidePanel.AddItem(detailItem3);
        sidePanel.AddItem(detailItem4);
        sidePanel.AddItem(detailItem5);
        ketchupItem.AddSidePanel(sidePanel);
        ketchupItem.SetLeftBadge(BadgeIcon.STAR);
        exampleMenu.AddItem(ketchupItem);

        UIMenuItem cookItem = new UIMenuItem("Cook! in a very long label to test the text scrolling feature!", "Cook the dish with the appropiate ingredients and ketchup.");
        cookItem.SetRightLabel("rightLabel");
        cookItem.LabelFont = ScaleformFonts.ENGRAVERS_OLD_ENGLISH_MT_STD;
        cookItem.RightLabelFont = ScaleformFonts.PRICEDOWN_GTAV_INT;
        exampleMenu.AddItem(cookItem);
        UIVehicleColourPickerPanel sidePanelB = new UIVehicleColourPickerPanel(PanelSide.Right, "ColorPicker");
        cookItem.AddSidePanel(sidePanelB);
        cookItem.SetRightBadge(BadgeIcon.STAR);
        sidePanelB.OnVehicleColorPickerSelect += (item, panel, value) =>
        {
            Notifications.ShowNotification($"Vehicle Color: {(VehicleColor)value}");
            sidePanelB.Title = ((VehicleColor)value).ToString();
        };

        UIMenuListItem scrollType = new UIMenuListItem("Choose how this menu will scroll!", new List<dynamic>() { "CLASSIC", "PAGINATED", "ENDLESS" }, (int)exampleMenu.ScrollingType);
        exampleMenu.AddItem(scrollType);

        scrollType.OnListChanged += (item, index) =>
        {
            exampleMenu.ScrollingType = (ScrollingType)index;
        };

        UIMenuItem colorItem = new UIMenuItem("UIMenuItem with Colors", "~b~Look!!~r~I can be colored ~y~too!!~w~~n~Every item now supports custom colors!", SColor.HUD_Purple, SColor.HUD_Pink);
        exampleMenu.AddItem(colorItem);

        float dynamicvalue = 0f;
        UIMenuDynamicListItem dynamicItem = new UIMenuDynamicListItem("Dynamic List Item", "Try pressing ~INPUT_FRONTEND_LEFT~ or ~INPUT_FRONTEND_RIGHT~", dynamicvalue.ToString("F3"), async (sender, direction) =>
        {
            if (direction == UIMenuDynamicListItem.ChangeDirection.Left) dynamicvalue -= 0.01f;
            else dynamicvalue += 0.01f;
            return dynamicvalue.ToString("F3");
        });
        dynamicItem.BlinkDescription = true;
        exampleMenu.AddItem(dynamicItem);

        List<dynamic> foodsList = new List<dynamic>
        {
            "LINEAR",
            "QUADRATIC_IN",
            "QUADRATIC_OUT",
            "QUADRATIC_INOUT",
            "CUBIC_IN",
            "CUBIC_OUT",
            "CUBIC_INOUT",
            "QUARTIC_IN",
            "QUARTIC_OUT",
            "QUARTIC_INOUT",
            "SINE_IN",
            "SINE_OUT",
            "SINE_INOUT",
            "BACK_IN",
            "BACK_OUT",
            "BACK_INOUT",
            "CIRCULAR_IN",
            "CIRCULAR_OUT",
            "CIRCULAR_INOUT"
        };

        UIMenuSeparatorItem BlankItem = new UIMenuSeparatorItem("Separator (Jumped)", true);
        UIMenuSeparatorItem BlankItem_2 = new UIMenuSeparatorItem("Separator (not Jumped)", false);
        exampleMenu.AddItem(BlankItem);
        exampleMenu.AddItem(BlankItem_2);

        UIMenuListItem colorListItem = new UIMenuListItem("Choose the scrolling animation", foodsList, (int)exampleMenu.AnimationType, "~BLIP_BARBER~ ~BLIP_INFO_ICON~ ~BLIP_TANK~ ~BLIP_OFFICE~ ~BLIP_CRIM_DRUGS~ ~BLIP_WAYPOINT~ ~INPUTGROUP_MOVE~~n~You can use Blips and Inputs in description as you prefer!~n~⚠ 🐌 ❤️ 🥺 💪🏻 You can use Emojis too!", SColor.HUD_Freemode_dark, SColor.HUD_Freemode);
        colorListItem.BlinkDescription = true;
        exampleMenu.AddItem(colorListItem);

        UIMenuSliderItem slider = new UIMenuSliderItem("Slider Item", "Cool!", true); // by default max is 100 and multipler 5 = 20 steps.
        exampleMenu.AddItem(slider);
        UIMenuProgressItem progress = new UIMenuProgressItem("Slider Progress Item", 10, 0);
        exampleMenu.AddItem(progress);

        UIMenuItem listPanelItem0 = new UIMenuItem("Change Color", "It can be whatever item you want it to be");
        UIMenuColorPanel ColorPanel = new UIMenuColorPanel("Color Panel Example", ColorPanelType.Hair);
        // you can choose between hair palette or makeup palette or custom
        exampleMenu.AddItem(listPanelItem0);
        listPanelItem0.AddPanel(ColorPanel);

        UIMenuItem listPanelItem1 = new UIMenuItem("Custom palette panel");
        UIMenuColorPanel ColorPanelCustom = new UIMenuColorPanel("Custom Palette Example", new List<SColor> { SColor.FromRandomValues(), SColor.FromRandomValues(), SColor.FromRandomValues(), SColor.FromRandomValues(), SColor.FromRandomValues() }, 0);
        exampleMenu.AddItem(listPanelItem1);
        listPanelItem1.AddPanel(ColorPanelCustom);

        UIMenuItem listPanelItem2 = new UIMenuItem("Change Percentage", "It can be whatever item you want it to be");
        UIMenuPercentagePanel PercentagePanel = new UIMenuPercentagePanel("Percentage Panel", "0%", "100%");
        // You can change every text in this Panel
        exampleMenu.AddItem(listPanelItem2);
        listPanelItem2.AddPanel(PercentagePanel);

        UIMenuItem listPanelItem3 = new UIMenuItem("Change Grid Position", "It can be whatever item you want it to be");
        UIMenuGridPanel GridPanel = new UIMenuGridPanel("Up", "Left", "Right", "Down", new System.Drawing.PointF(.5f, .5f));
        UIMenuGridPanel HorizontalGridPanel = new UIMenuGridPanel("Left", "Right", new System.Drawing.PointF(.5f, .5f));
        // you can choose the text in every position and where to place the starting position of the cirlce
        exampleMenu.AddItem(listPanelItem3);
        listPanelItem3.AddPanel(GridPanel);
        listPanelItem3.AddPanel(HorizontalGridPanel);

        UIMenuListItem listPanelItem4 = new UIMenuListItem("Look at Statistics", new List<object> { "Example", "example2" }, 0);
        UIMenuStatisticsPanel statistics = new UIMenuStatisticsPanel();
        exampleMenu.AddItem(listPanelItem4);
        listPanelItem4.AddPanel(statistics);
        statistics.AddStatistics("Look at this!", 0);
        statistics.AddStatistics("I'm a statistic too!", 0);
        statistics.AddStatistics("Am i not?!", 0);
        //you can add as menu statistics you want 
        statistics.UpdateStatistic(0, 10f);
        statistics.UpdateStatistic(1, 50f);
        statistics.UpdateStatistic(2, 100f);
        //and you can get / set their percentage


        #region Windows SubMenu
        UIMenuItem windowsItem = new UIMenuItem("Windows SubMenu item label", "this is the submenu binded item description");
        windowsItem.SetRightLabel(">>>");
        exampleMenu.AddItem(windowsItem);
        UIMenu windowSubmenu = new UIMenu("Windows Menu", "submenu description");

        UIMenuHeritageWindow heritageWindow = new UIMenuHeritageWindow(0, 0);
        UIMenuDetailsWindow statsWindow = new UIMenuDetailsWindow("Parents resemblance", "Dad:", "Mom:", true, new List<UIDetailStat>());
        windowSubmenu.AddWindow(heritageWindow);
        windowSubmenu.AddWindow(statsWindow);
        List<dynamic> momfaces = new List<dynamic>() { "Hannah", "Audrey", "Jasmine", "Giselle", "Amelia", "Isabella", "Zoe", "Ava", "Camilla", "Violet", "Sophia", "Eveline", "Nicole", "Ashley", "Grace", "Brianna", "Natalie", "Olivia", "Elizabeth", "Charlotte", "Emma", "Misty" };
        List<dynamic> dadfaces = new List<dynamic>() { "Benjamin", "Daniel", "Joshua", "Noah", "Andrew", "Joan", "Alex", "Isaac", "Evan", "Ethan", "Vincent", "Angel", "Diego", "Adrian", "Gabriel", "Michael", "Santiago", "Kevin", "Louis", "Samuel", "Anthony", "Claude", "Niko", "John" };
        UIMenuListItem mom = new UIMenuListItem("Mamma", momfaces, 0);
        UIMenuListItem dad = new UIMenuListItem("Papà", dadfaces, 0);
        UIMenuSliderItem newItem = new UIMenuSliderItem("Heritage Slider", "This is Useful on heritage", 100, 5, 50, true);
        windowSubmenu.AddItem(mom);
        windowSubmenu.AddItem(dad);
        windowSubmenu.AddItem(newItem);
        statsWindow.DetailMid = "Dad: " + newItem.Value + "%";
        statsWindow.DetailBottom = "Mom: " + (100 - newItem.Value) + "%";
        statsWindow.DetailStats = new List<UIDetailStat>()
        {
            new UIDetailStat(100-newItem.Value, SColor.HUD_Pink),
            new UIDetailStat(newItem.Value, SColor.HUD_Freemode),
        };

        windowsItem.Activated += (sender, e) =>
        {
            sender.SwitchTo(windowSubmenu, inheritOldMenuParams: true);
        };
        #endregion

        #region Scaleforms SubMenu
        UIMenuItem scaleformItem = new UIMenuItem("Scaleforms Showdown", "Let's try them!");
        scaleformItem.SetRightLabel(">>>");
        exampleMenu.AddItem(scaleformItem);

        UIMenu scaleformMenu = new("Scaleforms Showdown", "Let's try them!");
        UIMenuItem showSimplePopup = new UIMenuItem("Show PopupWarning example", "You can customize it to your needs");
        UIMenuItem showPopupButtons = new UIMenuItem("Show PopupWarning with buttons", "It waits until a button has been pressed!");
        UIMenuListItem customInstr = new UIMenuListItem("SavingNotification", Enum.GetNames(typeof(LoadingSpinnerType)).Cast<dynamic>().ToList(), 0, "InstructionalButtons now give you the ability to dynamically edit, add, remove, customize your buttons, you can even use them outside the menu ~y~without having to run multiple instances of the same scaleform~w~, aren't you happy??");
        UIMenuItem customInstr2 = new UIMenuItem("Add a random InstructionalButton!", "InstructionalButtons now give you the ability to dynamically edit, add, remove, customize your buttons, you can even use them outside the menu ~y~without having to run multiple instances of the same scaleform~w~, aren't you happy??");
        UIMenuItem bigMessage = new UIMenuItem("BigMessage example", "");
        UIMenuItem midMessage = new UIMenuItem("MediumMessage example", "");
        scaleformMenu.AddItem(showSimplePopup);
        scaleformMenu.AddItem(showPopupButtons);
        scaleformMenu.AddItem(customInstr);
        scaleformMenu.AddItem(customInstr2);
        scaleformMenu.AddItem(bigMessage);
        scaleformMenu.AddItem(midMessage);

        scaleformItem.Activated += (sender, args) =>
        {
            sender.SwitchTo(scaleformMenu, inheritOldMenuParams: true);
        };

        #endregion

        #region Notifications SubMenu

        UIMenuItem notificationsItem = new UIMenuItem("This item goes to the notifications", "Let's try them!");
        notificationsItem.SetRightLabel(">>>");
        exampleMenu.AddItem(notificationsItem);

        UIMenu notificationsMenu = new("Notifications Showdown", "Let's try them!");

        List<dynamic> colors = Enum.GetNames(typeof(NotificationColor)).ToList<dynamic>();
        colors.Add("Classic");
        List<dynamic> char_sprites = new List<dynamic>() { "Abigail", "Amanda", "Ammunation", "Andreas", "Antonia", "Ashley", "BankOfLiberty", "BankFleeca", "BankMaze", "Barry", "Beverly", "BikeSite", "BlankEntry", "Blimp", "Blocked", "BoatSite", "BrokenDownGirl", "BugStars", "Call911", "LegendaryMotorsport", "SSASuperAutos", "Castro", "ChatCall", "Chef", "Cheng", "ChengSenior", "Chop", "Cris", "Dave", "Default", "Denise", "DetonateBomb", "DetonatePhone", "Devin", "SubMarine", "Dom", "DomesticGirl", "Dreyfuss", "DrFriedlander", "Epsilon", "EstateAgent", "Facebook", "FilmNoire", "Floyd", "Franklin", "FranklinTrevor", "GayMilitary", "Hao", "HitcherGirl", "Hunter", "Jimmy", "JimmyBoston", "Joe", "Josef", "Josh", "LamarDog", "Lester", "Skull", "LesterFranklin", "LesterMichael", "LifeInvader", "LsCustoms", "LSTI", "Manuel", "Marnie", "Martin", "MaryAnn", "Maude", "Mechanic", "Michael", "MichaelFranklin", "MichaelTrevor", "WarStock", "Minotaur", "Molly", "MorsMutual", "ArmyContact", "Brucie", "FibContact", "RockStarLogo", "Gerald", "Julio", "MechanicChinese", "MerryWeather", "Unicorn", "Mom", "MrsThornhill", "PatriciaTrevor", "PegasusDelivery", "ElitasTravel", "Sasquatch", "Simeon", "SocialClub", "Solomon", "Taxi", "Trevor", "YouTube", "Wade" };

        UIMenuListItem noti1 = new UIMenuListItem("Simple Notification", colors, colors.Count - 1, "Can be colored too! Change color and / or select this item to show the notification.");
        UIMenuListItem noti2 = new UIMenuListItem("Advanced Notification", char_sprites, 0, "Change the char and see the notification example! (It can be colored too like the simple notification)");
        UIMenuItem noti3 = new UIMenuItem("Help Notification", "Insert your text and see the example.");
        UIMenuItem noti4 = new UIMenuItem("Floating Help Notification", "This is tricky, it's a 3D notification, you'll have to input a Vector3 to show it!");
        UIMenuItem noti5 = new UIMenuItem("Stats Notification", "This is the notification you see in GTA:O when you improve one of your skills.");
        UIMenuItem noti6 = new UIMenuItem("VS Notification", "This is the notification you see in GTA:O when you kill someone or get revenge.");
        UIMenuItem noti7 = new UIMenuItem("3D Text", "This is known a lot.. let's you draw a 3D text in a precise world coordinates.");
        UIMenuItem noti8 = new UIMenuItem("Simple Text", "This will let you draw a 2D text on screen, you'll have to input the 2D  (X, Y) coordinates.");
        notificationsMenu.AddItem(noti1);
        notificationsMenu.AddItem(noti2);
        notificationsMenu.AddItem(noti3);
        notificationsMenu.AddItem(noti4);
        notificationsMenu.AddItem(noti5);
        notificationsMenu.AddItem(noti6);
        notificationsMenu.AddItem(noti7);
        notificationsMenu.AddItem(noti8);

        notificationsItem.Activated += (sender, args) =>
        {
            sender.SwitchTo(notificationsMenu, inheritOldMenuParams: true);
        };
        #endregion

        #region PauseMenu Enabler

        UIMenuItem pause = new UIMenuItem("Open Pause Menu");
        exampleMenu.AddItem(pause);
        pause.Activated += (menu, item) =>
        {
            PauseMenuShowcase(menu);
        };

        UIMenuItem itemFilter = new UIMenuItem("Item filtering", "Select this item to filter items based on their labels");
        itemFilter.Activated += async (menu, item) =>
        {
            string filter = await Game.GetUserInput(10);
            menu.FilterMenuItems((mb) => mb.Label.ToLower().Contains(filter.ToLower()));
        };

        UIMenuItem itemSorter = new UIMenuItem("Item sorting", "Activate this item to sort items alphabetically");
        itemSorter.Activated += (menu, item) =>
        {
            menu.SortMenuItems((pair1, pair2) => pair1.Label.ToString().ToLower().CompareTo(pair2.Label.ToString().ToLower()));
        };

        UIMenuItem ResetFiltering = new UIMenuItem("Reset item filters", "Select this item to reset any filtering");
        ResetFiltering.Activated += (menu, item) =>
        {
            menu.ResetFilter();
        };

        exampleMenu.AddItem(itemFilter);
        exampleMenu.AddItem(itemSorter);
        exampleMenu.AddItem(ResetFiltering);

        #endregion

        #endregion

        #region Menu Events

        // here you can handle all the events for the mainMenu and its submenus or items themselves.. there's not a real order and if you want you can place these events 
        // right under the place where their menus/items were declared, i place them here for a creation order.

        // ====================================================================
        // =--------------------------- [Items] ------------------------------=
        // ====================================================================

        slider.OnSliderChanged += (item, index) =>
        {
            Screen.ShowSubtitle($"Slider changed => {index}");
        };

        progress.OnProgressChanged += (item, index) =>
        {
            Screen.ShowSubtitle($"Progress changed => {index}");
        };

        // ====================================================================
        // =--------------------------- [Panels] -----------------------------=
        // ====================================================================
        // THERE ARE NOW EVENT FOR PANELS.. WHEN YOU CHANGE WHAT IS CHANGABLE THE PANEL ITSELF WILL DO WHATEVER YOU TELL HIM TO DO

        ColorPanel.OnColorPanelChange += (item, panel, index) =>
        {
            Notifications.ShowNotification($"ColorPanel index => {index}");
        };

        ColorPanelCustom.OnColorPanelChange += (item, panel, index) =>
        {
            Notifications.ShowNotification($"ColorPanel index => {index}");
        };

        PercentagePanel.OnPercentagePanelChange += (item, panel, index) =>
        {
            Screen.ShowSubtitle("Percentage = " + index + "...");
        };

        GridPanel.OnGridPanelChange += (item, panel, value) =>
        {
            Screen.ShowSubtitle("GridPosition = " + value + "...");
        };

        HorizontalGridPanel.OnGridPanelChange += (item, panel, value) =>
        {
            Screen.ShowSubtitle("HorizontalGridPosition = " + value + "...");
        };

        // ====================================================================
        // =---------------------- [Heritage SubMenu] ------------------------=
        // ====================================================================

        int MomIndex = 0;
        int DadIndex = 0;

        windowSubmenu.OnListChange += async (_sender, _listItem, _newIndex) =>
        {
            if (_listItem == mom)
            {
                MomIndex = _newIndex;
                heritageWindow.Index(MomIndex, DadIndex);
            }
            else if (_listItem == dad)
            {
                DadIndex = _newIndex;
                heritageWindow.Index(MomIndex, DadIndex);
            }
            // This way the heritage window changes only if you change a list item!
        };

        windowSubmenu.OnSliderChange += (sender, item, value) =>
        {
            statsWindow.DetailStats[0].Percentage = 100 - value;
            statsWindow.DetailStats[0].HudColor = SColor.HUD_Pink;
            statsWindow.DetailStats[1].Percentage = value;
            statsWindow.DetailStats[1].HudColor = SColor.HUD_Freemode;
            statsWindow.UpdateStatsToWheel();
            statsWindow.UpdateLabels("Parents resemblance", "Dad: " + value + "%", "Mom: " + (100 - value) + "%");
        };

        // ====================================================================
        // =--------------------- [Scaleforms SubMenu] -----------------------=
        // ====================================================================

        scaleformMenu.OnItemSelect += async (sender, item, index) =>
        {
            if (item == showSimplePopup)
            {
                ScaleformUI.Main.Warning.ShowWarning("This is the title", "This is the subtitle", "This is the prompt.. you have 6 seconds left", "This is the error message, ScaleformUI Ver. 3.0");
                await Delay(1000);
                for (int i = 5; i > -1; i--)
                {
                    ScaleformUI.Main.Warning.UpdateWarning("This is the title", "This is the subtitle", $"This is the prompt.. you have {i} seconds left", "This is the error message, ScaleformUI Ver. 3.0");
                    await Delay(1000);
                }
                ScaleformUI.Main.Warning.Dispose();
            }
            else if (item == showPopupButtons)
            {
                List<InstructionalButton> buttons = new List<InstructionalButton>()
                {
                    new InstructionalButton(Control.FrontendDown, "Accept only with Keyboard", PadCheck.Keyboard),
                    new InstructionalButton(Control.FrontendY, "Cancel only with GamePad", PadCheck.Controller),
                    new InstructionalButton(Control.FrontendX, Control.Detonate, "This will change button if you're using gamepad or keyboard"),
                    new InstructionalButton(new List<Control> { Control.MoveUpOnly, Control.MoveLeftOnly , Control.MoveDownOnly , Control.MoveRightOnly }, "Woow multiple buttons at once??"),
                    new InstructionalButton(InputGroup.INPUTGROUP_LOOK, "InputGroup example")
                };
                ScaleformUI.Main.Warning.ShowWarningWithButtons("This is the title", "This is the subtitle", "This is the prompt, press any button", buttons, "This is the error message, ScaleformUI Ver. 3.0");
                ScaleformUI.Main.Warning.OnButtonPressed += (button) =>
                {
                    Debug.WriteLine($"You pressed a Button => {button.Text}");
                };
            }
            else if (item == customInstr2)
            {
                if (ScaleformUI.Main.InstructionalButtons.ControlButtons.Count >= 6) return;
                ScaleformUI.Main.InstructionalButtons.AddInstructionalButton(new InstructionalButton((Control)new Random().Next(0, 250), "I'm a new button look at me!"));
            }
            else if (item == bigMessage)
            {
                ScaleformUI.Main.BigMessageInstance.ShowSimpleShard("TITLE", "SUBTITLE");
            }
            else if (item == midMessage)
            {
                ScaleformUI.Main.MedMessageInstance.ShowColoredShard("TITLE", "SUBTITLE", HudColor.HUD_COLOUR_FREEMODE);
            }
        };

        customInstr.OnListSelected += (item, index) =>
            {
                if (ScaleformUI.Main.InstructionalButtons.IsSaving) return;
                ScaleformUI.Main.InstructionalButtons.AddSavingText((LoadingSpinnerType)(index + 1), "I'm a saving text", 3000);
            };

        // ====================================================================
        // =------------------- [Notifications SubMenu] ----------------------=
        // ====================================================================

        ScaleformUI.ScaleformUINotification notification = null;
        notificationsMenu.OnListChange += (_menu, _item, _index) =>
        {
            if (_item == noti1)
            {
                if (notification != null)
                    notification.Hide();
                if (_index == (colors.Count - 1))
                    notification = Notifications.ShowNotification("This is a simple notification without color and look how long it is wooow!", true, true);
                else
                {
                    switch (_index)
                    {
                        case 0:
                            notification = Notifications.ShowNotification("This is a simple colored notification and look how long it is wooow!", NotificationColor.Gold, true, true);
                            break;
                        case 1:
                            notification = Notifications.ShowNotification("This is a simple colored notification and look how long it is wooow!", NotificationColor.Red, true, true);
                            break;
                        case 2:
                            notification = Notifications.ShowNotification("This is a simple colored notification and look how long it is wooow!", NotificationColor.Rose, true, true);
                            break;
                        case 3:
                            notification = Notifications.ShowNotification("This is a simple colored notification and look how long it is wooow!", NotificationColor.GreenLight, true, true);
                            break;
                        case 4:
                            notification = Notifications.ShowNotification("This is a simple colored notification and look how long it is wooow!", NotificationColor.GreenDark, true, true);
                            break;
                        case 5:
                            notification = Notifications.ShowNotification("This is a simple colored notification and look how long it is wooow!", NotificationColor.Cyan, true, true);
                            break;
                        case 6:
                            notification = Notifications.ShowNotification("This is a simple colored notification and look how long it is wooow!", NotificationColor.Purple, true, true);
                            break;
                        case 7:
                            notification = Notifications.ShowNotification("This is a simple colored notification and look how long it is wooow!", NotificationColor.Yellow, true, true);
                            break;
                        case 8:
                            notification = Notifications.ShowNotification("This is a simple colored notification and look how long it is wooow!", NotificationColor.Blue, true, true);
                            break;
                    }
                }
            }
            else if (_item == noti2)
            {
                string selectedChar = NotificationChar.Abigail;
                #region SwitchStatement
                switch (_item.Items[_index])
                {
                    case "Abigail":
                        selectedChar = NotificationChar.Abigail;
                        break;
                    case "Amanda":
                        selectedChar = NotificationChar.Amanda;
                        break;
                    case "Ammunation":
                        selectedChar = NotificationChar.Ammunation;
                        break;
                    case "Andreas":
                        selectedChar = NotificationChar.Andreas;
                        break;
                    case "Antonia":
                        selectedChar = NotificationChar.Antonia;
                        break;
                    case "Ashley":
                        selectedChar = NotificationChar.Ashley;
                        break;
                    case "BankOfLiberty":
                        selectedChar = NotificationChar.BankOfLiberty;
                        break;
                    case "BankFleeca":
                        selectedChar = NotificationChar.BankFleeca;
                        break;
                    case "BankMaze":
                        selectedChar = NotificationChar.BankMaze;
                        break;
                    case "Barry":
                        selectedChar = NotificationChar.Barry;
                        break;
                    case "Beverly":
                        selectedChar = NotificationChar.Beverly;
                        break;
                    case "BikeSite":
                        selectedChar = NotificationChar.BikeSite;
                        break;
                    case "BlankEntry":
                        selectedChar = NotificationChar.BlankEntry;
                        break;
                    case "Blimp":
                        selectedChar = NotificationChar.Blimp;
                        break;
                    case "Blocked":
                        selectedChar = NotificationChar.Blocked;
                        break;
                    case "BoatSite":
                        selectedChar = NotificationChar.BoatSite;
                        break;
                    case "BrokenDownGirl":
                        selectedChar = NotificationChar.BrokenDownGirl;
                        break;
                    case "BugStars":
                        selectedChar = NotificationChar.BugStars;
                        break;
                    case "Call911":
                        selectedChar = NotificationChar.Call911;
                        break;
                    case "LegendaryMotorsport":
                        selectedChar = NotificationChar.LegendaryMotorsport;
                        break;
                    case "SSASuperAutos":
                        selectedChar = NotificationChar.SSASuperAutos;
                        break;
                    case "Castro":
                        selectedChar = NotificationChar.Castro;
                        break;
                    case "ChatCall":
                        selectedChar = NotificationChar.ChatCall;
                        break;
                    case "Chef":
                        selectedChar = NotificationChar.Chef;
                        break;
                    case "Cheng":
                        selectedChar = NotificationChar.Cheng;
                        break;
                    case "ChengSenior":
                        selectedChar = NotificationChar.ChengSenior;
                        break;
                    case "Chop":
                        selectedChar = NotificationChar.Chop;
                        break;
                    case "Cris":
                        selectedChar = NotificationChar.Cris;
                        break;
                    case "Dave":
                        selectedChar = NotificationChar.Dave;
                        break;
                    case "Default":
                        selectedChar = NotificationChar.Default;
                        break;
                    case "Denise":
                        selectedChar = NotificationChar.Denise;
                        break;
                    case "DetonateBomb":
                        selectedChar = NotificationChar.DetonateBomb;
                        break;
                    case "DetonatePhone":
                        selectedChar = NotificationChar.DetonatePhone;
                        break;
                    case "Devin":
                        selectedChar = NotificationChar.Devin;
                        break;
                    case "SubMarine":
                        selectedChar = NotificationChar.SubMarine;
                        break;
                    case "Dom":
                        selectedChar = NotificationChar.Dom;
                        break;
                    case "DomesticGirl":
                        selectedChar = NotificationChar.DomesticGirl;
                        break;
                    case "Dreyfuss":
                        selectedChar = NotificationChar.Dreyfuss;
                        break;
                    case "DrFriedlander":
                        selectedChar = NotificationChar.DrFriedlander;
                        break;
                    case "Epsilon":
                        selectedChar = NotificationChar.Epsilon;
                        break;
                    case "EstateAgent":
                        selectedChar = NotificationChar.EstateAgent;
                        break;
                    case "Facebook":
                        selectedChar = NotificationChar.Facebook;
                        break;
                    case "FilmNoire":
                        selectedChar = NotificationChar.FilmNoire;
                        break;
                    case "Floyd":
                        selectedChar = NotificationChar.Floyd;
                        break;
                    case "Franklin":
                        selectedChar = NotificationChar.Franklin;
                        break;
                    case "FranklinTrevor":
                        selectedChar = NotificationChar.FranklinTrevor;
                        break;
                    case "GayMilitary":
                        selectedChar = NotificationChar.GayMilitary;
                        break;
                    case "Hao":
                        selectedChar = NotificationChar.Hao;
                        break;
                    case "HitcherGirl":
                        selectedChar = NotificationChar.HitcherGirl;
                        break;
                    case "Hunter":
                        selectedChar = NotificationChar.Hunter;
                        break;
                    case "Jimmy":
                        selectedChar = NotificationChar.Jimmy;
                        break;
                    case "JimmyBoston":
                        selectedChar = NotificationChar.JimmyBoston;
                        break;
                    case "Joe":
                        selectedChar = NotificationChar.Joe;
                        break;
                    case "Josef":
                        selectedChar = NotificationChar.Josef;
                        break;
                    case "Josh":
                        selectedChar = NotificationChar.Josh;
                        break;
                    case "LamarDog":
                        selectedChar = NotificationChar.LamarDog;
                        break;
                    case "Lester":
                        selectedChar = NotificationChar.Lester;
                        break;
                    case "Skull":
                        selectedChar = NotificationChar.Skull;
                        break;
                    case "LesterFranklin":
                        selectedChar = NotificationChar.LesterFranklin;
                        break;
                    case "LesterMichael":
                        selectedChar = NotificationChar.LesterMichael;
                        break;
                    case "LifeInvader":
                        selectedChar = NotificationChar.LifeInvader;
                        break;
                    case "LsCustoms":
                        selectedChar = NotificationChar.LsCustoms;
                        break;
                    case "LSTI":
                        selectedChar = NotificationChar.LSTI;
                        break;
                    case "Manuel":
                        selectedChar = NotificationChar.Manuel;
                        break;
                    case "Marnie":
                        selectedChar = NotificationChar.Marnie;
                        break;
                    case "Martin":
                        selectedChar = NotificationChar.Martin;
                        break;
                    case "MaryAnn":
                        selectedChar = NotificationChar.MaryAnn;
                        break;
                    case "Maude":
                        selectedChar = NotificationChar.Maude;
                        break;
                    case "Mechanic":
                        selectedChar = NotificationChar.Mechanic;
                        break;
                    case "Michael":
                        selectedChar = NotificationChar.Michael;
                        break;
                    case "MichaelFranklin":
                        selectedChar = NotificationChar.MichaelFranklin;
                        break;
                    case "MichaelTrevor":
                        selectedChar = NotificationChar.MichaelTrevor;
                        break;
                    case "WarStock":
                        selectedChar = NotificationChar.WarStock;
                        break;
                    case "Minotaur":
                        selectedChar = NotificationChar.Minotaur;
                        break;
                    case "Molly":
                        selectedChar = NotificationChar.Molly;
                        break;
                    case "MorsMutual":
                        selectedChar = NotificationChar.MorsMutual;
                        break;
                    case "ArmyContact":
                        selectedChar = NotificationChar.ArmyContact;
                        break;
                    case "Brucie":
                        selectedChar = NotificationChar.Brucie;
                        break;
                    case "FibContact":
                        selectedChar = NotificationChar.FibContact;
                        break;
                    case "RockStarLogo":
                        selectedChar = NotificationChar.RockStarLogo;
                        break;
                    case "Gerald":
                        selectedChar = NotificationChar.Gerald;
                        break;
                    case "Julio":
                        selectedChar = NotificationChar.Julio;
                        break;
                    case "MechanicChinese":
                        selectedChar = NotificationChar.MechanicChinese;
                        break;
                    case "MerryWeather":
                        selectedChar = NotificationChar.MerryWeather;
                        break;
                    case "Unicorn":
                        selectedChar = NotificationChar.Unicorn;
                        break;
                    case "Mom":
                        selectedChar = NotificationChar.Mom;
                        break;
                    case "MrsThornhill":
                        selectedChar = NotificationChar.MrsThornhill;
                        break;
                    case "PatriciaTrevor":
                        selectedChar = NotificationChar.PatriciaTrevor;
                        break;
                    case "PegasusDelivery":
                        selectedChar = NotificationChar.PegasusDelivery;
                        break;
                    case "ElitasTravel":
                        selectedChar = NotificationChar.ElitasTravel;
                        break;
                    case "Sasquatch":
                        selectedChar = NotificationChar.Sasquatch;
                        break;
                    case "Simeon":
                        selectedChar = NotificationChar.Simeon;
                        break;
                    case "SocialClub":
                        selectedChar = NotificationChar.SocialClub;
                        break;
                    case "Solomon":
                        selectedChar = NotificationChar.Solomon;
                        break;
                    case "Taxi":
                        selectedChar = NotificationChar.Taxi;
                        break;
                    case "Trevor":
                        selectedChar = NotificationChar.Trevor;
                        break;
                    case "YouTube":
                        selectedChar = NotificationChar.YouTube;
                        break;
                    case "Wade":
                        selectedChar = NotificationChar.Wade;
                        break;
                }
                #endregion
                if (notification != null) notification.Hide();
                notification = Notifications.ShowAdvancedNotification("This is the title!!", "This is the subtitle!", "This is the main text!!", selectedChar, selectedChar, HudColor.NONE, SColor.AliceBlue, true, NotificationType.Default, true, true);
            }
        };

        notificationsMenu.OnItemSelect += async (_menu, _item, _index) =>
        {
            API.AddTextEntry("FMMC_KEY_TIP8", "Insert text (Max 10 chars):");
            string text = await Game.GetUserInput("", 10); // i set max 50 chars here as example but it can be way more!
            if (_item == noti3)
            {
                Notifications.ShowHelpNotification(text, 5000);
            }
            else if (_item == noti4)
            {
                _text = text;
                _timer = Game.GameTime + 1;
                Tick += FloatingHelpTimer;
            }
            else if (_item == noti5)
            {
                await Notifications.ShowStatNotification(75, 50, text, true, true);
            }
            else if (_item == noti6)
            {
                await Notifications.ShowVSNotification(12, HudColor.HUD_COLOUR_BLUE, Game.PlayerPed, 3, HudColor.HUD_COLOUR_RED);
                // you must specify 1 or 2 peds for this.. in this case i use the player ped twice for the sake of the example.
            }
            else if (_item == noti7)
            {
                _text = text;
                _timer = Game.GameTime + 1;
                Tick += Text3DTimer;
            }
            else if (_item == noti8)
            {
                _text = text;
                _timer = Game.GameTime + 1;
                Tick += TextTimer;
            }
        };

        // ====================================================================
        // =------------------------- [Main Menu] ----------------------------=
        // ====================================================================

        exampleMenu.OnCheckboxChange += (sender, item, checked_) =>
        {
            if (item == ketchupItem)
            {
                enabled = checked_;
                sender.EnableAnimation = enabled;
                colorListItem.Enabled = enabled;
                Notifications.ShowNotification("~r~Menu animation: ~b~" + (enabled ? "Enabled" : "Disabled"));
            }
        };

        exampleMenu.OnItemSelect += (sender, item, index) =>
        {
            if (item == cookItem)
            {
                string output = enabled ? "You have ordered ~b~{0}~w~ ~r~with~w~ ketchup." : "You have ordered ~b~{0}~w~ ~r~without~w~ ketchup.";
                Screen.ShowSubtitle(String.Format(output, dish));
            }
        };

        exampleMenu.OnIndexChange += (sender, index) =>
        {
            //if (sender.MenuItems[index] == cookItem)
            //cookItem.SetLeftBadge(BadgeIcon.NONE);
        };

        exampleMenu.OnListChange += (sender, item, index) =>
        {
            if (item == colorListItem)
                sender.AnimationType = (MenuAnimationType)index;
        };

        exampleMenu.OnMenuOpen += (menu, data) =>
        {
            Screen.ShowSubtitle($"{menu.Title} just opened!", 3000);
            Debug.WriteLine($"{menu.Title} just opened!");
        };

        exampleMenu.OnMenuClose += (menu) =>
        {
            Screen.ShowSubtitle($"{menu.Title} just closed!", 3000);
            Debug.WriteLine($"{menu.Title} just closed!");
        };

        #endregion
        exampleMenu.Visible = true;
    }

    private int _timer = 0;
    private string _text = string.Empty;
    public async Task Text3DTimer()
    {
        Notifications.DrawText3D(_text, Game.PlayerPed.Bones[Bone.SKEL_Head].Position + new Vector3(0, 0, 0.5f), SColor.WhiteSmoke);
        if (Game.GameTime - _timer > 5000) // this is a tricky yet simple way to count time without using Delay and pausing the Thread ;)
            Tick -= Text3DTimer;
        await Task.FromResult(0);
    }
    public async Task TextTimer()
    {
        Notifications.DrawText(0.35f, 0.7f, _text);
        if (Game.GameTime - _timer > 5000) // this is a tricky yet simple way to count time without using Delay and pausing the Thread ;)
            Tick -= TextTimer;
        await Task.FromResult(0);
    }
    public async Task FloatingHelpTimer()
    {
        Notifications.ShowFloatingHelpNotification(_text, Game.PlayerPed.Bones[Bone.SKEL_Head].Position + new Vector3(0, 0, 0.5f));
        // this will show the 3d notification on the head of the ped in 3d world coords
        if (Game.GameTime - _timer > 5000) // this is a tricky yet simple way to count time without using Delay and pausing the Thread ;)
            Tick -= FloatingHelpTimer;
        await Task.FromResult(0);
    }
    #endregion

    public void CreateRadialMenu()
    {
        RadialMenu menu = new RadialMenu();

        long imgdui = CreateDui("https://giphy.com/embed/ckT59CvStmUsU", 64, 64);
        CreateRuntimeTextureFromDuiHandle(txd, "item1", GetDuiHandle(imgdui));

        long imgdui1 = CreateDui("https://giphy.com/embed/10bTCLE8GtHHS8", 96, 64);
        CreateRuntimeTextureFromDuiHandle(txd, "item2", GetDuiHandle(imgdui1));

        long imgdui2 = CreateDui("https://giphy.com/embed/nHyZigjdO4hEodq9fv", 64, 64);
        CreateRuntimeTextureFromDuiHandle(txd, "item3", GetDuiHandle(imgdui2));

        SegmentItem item = new SegmentItem("This is the label!", "~BLIP_INFO_ICON~ This is the description.. it's multiline so it can be very long!", "scaleformui", "item1", 64, 64, SColor.HUD_Freemode);
        SegmentItem item1 = new SegmentItem("It's so long it scrolls automatically! Isn't this amazing?", "~BLIP_INFO_ICON~ This is the description.. it's multiline so it can be very long!", "scaleformui", "item2", 96, 64, SColor.HUD_Green);
        SegmentItem item2 = new SegmentItem("Label 3", "~BLIP_INFO_ICON~ This is the description.. it's multiline so it can be very long!", "scaleformui", "item3", 64, 64, SColor.HUD_Red);
        item.SetQuantity(8000, 9999);
        item1.SetQuantity(50, 100);
        item2.SetQuantity(5000, 0);

        for (int i = 0; i < 8; i++)
        {
            menu.Segments[i].AddItem(item);
            menu.Segments[i].AddItem(item1);
            menu.Segments[i].AddItem(item2);
        }

        menu.OnMenuOpen += (menu, _) =>
        {
            Screen.ShowSubtitle("Radial Menu opened!");
        };

        menu.OnMenuClose += (menu) =>
        {
            Screen.ShowSubtitle("Radial Menu closed!");
        };

        menu.OnSegmentHighlight += (segment) =>
        {
            Screen.ShowSubtitle($"Segment {segment.Index} highlighted!");
        };

        menu.OnSegmentIndexChange += (segment, index) =>
        {
            Screen.ShowSubtitle($"Segment {segment.Index}, index changed to {index}!");
        };

        menu.OnSegmentSelect += (segment) =>
        {
            Screen.ShowSubtitle($"Segment {segment.Index} selected!");
        };

        menu.CurrentSegment = 1;
        menu.Visible = true;
    }

    public void CreateRadioMenu()
    {
        long imgdui1 = API.CreateDui("https://giphy.com/embed/10bTCLE8GtHHS8", 96, 64);
        API.CreateRuntimeTextureFromDuiHandle(txd, "item2", API.GetDuiHandle(imgdui1));

        UIRadioMenu menu = new UIRadioMenu();
        menu.AnimationDuration = 1f;
        menu.AnimDirection = AnimationDirection.ZoomOut;
        for (int i = 0; i < 25; i++)
        {
            RadioItem station = new RadioItem("Station " + (i + 1), "Artist " + (i + 1), "Track " + +(i + 1), "scaleformui", "item2");
            menu.AddStation(station);
        }

        menu.OnMenuOpen += (_menu, _) =>
        {
            Screen.ShowSubtitle("Radio Menu opened!");
        };

        menu.OnMenuClose += (_menu) =>
        {
            Screen.ShowSubtitle("Radio Menu closed!");
        };

        menu.OnIndexChange += (index) =>
        {
            Screen.ShowSubtitle($"Index {index} highlighted!");
        };

        menu.OnStationSelect += (station, index) =>
        {
            Screen.ShowSubtitle($"Selected station with index {index}!");
        };

        menu.Visible = true;
    }

    public async void PauseMenuShowcase(UIMenu _menu)
    {
        UIMenu mainMenu = _menu;
        // tabview is the main menu.. the container of all the tabs.
        TabView pauseMenu = new TabView("PauseMenu example", "Look there's a subtitle too!", "Detail 1", "Detail 2", "Detail 3");

        int mugshot = API.RegisterPedheadshot(Game.PlayerPed.Handle);
        while (!API.IsPedheadshotReady(mugshot)) await BaseScript.Delay(1);
        string mugtxd = API.GetPedheadshotTxdString(mugshot);
        pauseMenu.HeaderPicture = new(mugtxd, mugtxd);

        TextTab basicTab = new TextTab("TabTextItem", "This is the title!", SColor.HUD_Freemode);

        long bg_dui = API.CreateDui("https://giphy.com/embed/sxwk9hGlsULcYm6hDX", 1280, 720);
        API.CreateRuntimeTextureFromDuiHandle(txd, "pausebigbg", API.GetDuiHandle(bg_dui));

        basicTab.UpdateBackground("scaleformui", "pausebigbg");
        basicTab.AddItem(new BasicTabItem("~y~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        basicTab.LabelsList[0].LabelFont = ScaleformFonts.HANDSTYLE_HEIST;
        basicTab.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~r~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        basicTab.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~b~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        basicTab.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~g~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        basicTab.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        basicTab.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        basicTab.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        basicTab.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        basicTab.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        basicTab.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        basicTab.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        basicTab.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~r~Use the mouse wheel to scroll the text!!"));
        pauseMenu.AddTab(basicTab);

        SubmenuTab multiItemTab = new SubmenuTab("TabSubMenu", SColor.HUD_Freemode);
        pauseMenu.AddTab(multiItemTab);
        TabLeftItem first = new TabLeftItem("1 - Empty", LeftItemType.Empty);
        TabLeftItem second = new TabLeftItem("2 - Info", LeftItemType.Info);
        TabLeftItem third = new TabLeftItem("3 - Statistics", LeftItemType.Statistics);
        TabLeftItem fourth = new TabLeftItem("4 - Settings", LeftItemType.Settings);
        TabLeftItem fifth = new TabLeftItem("5 - Keymaps", LeftItemType.Keymap);

        long _bginfo = API.CreateDui("https://giphy.com/embed/bG1oRM2Qp2kN3MTZCO", 480, 480);
        API.CreateRuntimeTextureFromDuiHandle(txd, "pauseinfobg", API.GetDuiHandle(_bginfo));

        long _bgstats = API.CreateDui("https://giphy.com/embed/xT9IgsHTiYHILDGDM4", 480, 480);
        API.CreateRuntimeTextureFromDuiHandle(txd, "pausestatsbg", API.GetDuiHandle(_bgstats));

        long _bgsets = API.CreateDui("https://giphy.com/embed/xT9IgsHTiYHILDGDM4", 480, 480);
        API.CreateRuntimeTextureFromDuiHandle(txd, "pausesetsbg", API.GetDuiHandle(_bgsets));

        second.UpdateBackground("scaleformui", "pauseinfobg", LeftItemBGType.Full);
        third.UpdateBackground("scaleformui", "pausestatsbg", LeftItemBGType.Masked);
        fourth.UpdateBackground("scaleformui", "pausesetsbg", LeftItemBGType.Resized);

        multiItemTab.AddLeftItem(first);
        multiItemTab.AddLeftItem(second);
        multiItemTab.AddLeftItem(third);
        multiItemTab.AddLeftItem(fourth);
        multiItemTab.AddLeftItem(fifth);

        second.RightTitle = "Info Title!!";
        second.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~y~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        second.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~r~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        second.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~b~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        second.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~g~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        second.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        second.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        second.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        second.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        second.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        second.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        second.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
        second.AddItem(new BasicTabItem("~BLIP_INFO_ICON~ ~r~Use the mouse wheel to scroll the text!!"));

        StatsTabItem _labelStatItem = new StatsTabItem("Item's Label", "Item's right label");
        StatsTabItem _coloredBarStatItem0 = new StatsTabItem("Item's Label", 0, SColor.HUD_Orange);
        StatsTabItem _coloredBarStatItem1 = new StatsTabItem("Item's Label", 25, SColor.HUD_Red);
        StatsTabItem _coloredBarStatItem2 = new StatsTabItem("Item's Label", 50, SColor.HUD_Blue);
        StatsTabItem _coloredBarStatItem3 = new StatsTabItem("Item's Label", 75, SColor.HUD_Green);
        StatsTabItem _coloredBarStatItem4 = new StatsTabItem("Item's Label", 100, SColor.HUD_Purple);

        third.AddItem(_labelStatItem);
        third.AddItem(_coloredBarStatItem0);
        third.AddItem(_coloredBarStatItem1);
        third.AddItem(_coloredBarStatItem2);
        third.AddItem(_coloredBarStatItem3);
        third.AddItem(_coloredBarStatItem4);

        List<dynamic> itemList = new List<dynamic>() { "This", "Is", "The", "List", "Super", "Power", "Wooow" };
        SettingsItem _settings1 = new SettingsItem("Item's Label", "Item's right Label");
        SettingsItem _settings2 = new SettingsListItem("Item's Label", itemList, 0);
        SettingsItem _settings3 = new SettingsProgressItem("Item's Label", 100, 25, false, SColor.HUD_Freemode);
        SettingsItem _settings4 = new SettingsProgressItem("Item's Label", 100, 75, true, SColor.HUD_Pink);
        SettingsItem _settings5 = new SettingsCheckboxItem("Item's Label", UIMenuCheckboxStyle.Tick, true);
        SettingsItem _settings6 = new SettingsSliderItem("Item's Label", 100, 50, SColor.HUD_Red);
        fourth.AddItem(_settings1);
        fourth.AddItem(_settings2);
        fourth.AddItem(_settings3);
        fourth.AddItem(_settings4);
        fourth.AddItem(_settings5);
        fourth.AddItem(_settings6);

        fifth.RightTitle = "ACTION";
        fifth.KeymapRightLabel_1 = "PRIMARY";
        fifth.KeymapRightLabel_2 = "SECONDARY";
        KeymapItem key1 = new KeymapItem("Simple Keymap", "~INPUT_FRONTEND_ACCEPT~", "~INPUT_VEH_EXIT~");
        KeymapItem key2 = new KeymapItem("Advanced Keymap", "~INPUT_SPRINT~ + ~INPUT_CONTEXT~", "", "", "~INPUTGROUP_FRONTEND_TRIGGERS~");
        fifth.AddItem(key1);
        fifth.AddItem(key2);
        fifth.AddItem(key1);
        fifth.AddItem(key2);
        fifth.AddItem(key1);
        fifth.AddItem(key2);
        fifth.AddItem(key1);
        fifth.AddItem(key2);

        PlayerListTab playersTab = new("PLAYERLIST", SColor.HUD_Freemode, false);
        List<Column> columns = new List<Column>()
        {
            new SettingsListColumn("COLUMN SETTINGS", SColor.HUD_Red), // color will be ignored for PauseMenu
            //new PlayerListColumn("COLUMN PLAYERS", SColor.HUD_Orange), // color will be ignored for PauseMenu
            new MissionsListColumn("COLUMN MISSIONS", SColor.HUD_Orange), // color will be ignored for PauseMenu
            new MissionDetailsPanel("COLUMN INFO PANEL", SColor.HUD_Green), // color will be ignored for PauseMenu
        };
        playersTab.SetUpColumns(columns);
        pauseMenu.AddTab(playersTab);

        UIMenuItem n1 = new("Base Item", "Basic Description");
        UIMenuListItem n2 = new("List Item", new List<dynamic> { "item1", "item2", "item3" }, 0, "List Description");
        UIMenuCheckboxItem n3 = new("Checkbox Item", UIMenuCheckboxStyle.Tick, true, "Checkbox Description");
        UIMenuSliderItem n4 = new("Slider Item", "Slider Description", 100, 10, 50, false);
        UIMenuProgressItem n5 = new("Progress Item", 100, 50, "Progress Description");
        playersTab.SettingsColumn.AddSettings(n1);
        playersTab.SettingsColumn.AddSettings(n2);
        playersTab.SettingsColumn.AddSettings(n3);
        playersTab.SettingsColumn.AddSettings(n4);
        playersTab.SettingsColumn.AddSettings(n5);

        n1.Activated += (sender, args) =>
        {
            playersTab.SelectColumn(1);
        };


        MissionItem mission1 = new MissionItem("Mission 1");
        MissionItem mission2 = new MissionItem("Mission 2");
        MissionItem mission3 = new MissionItem("Mission 3");
        MissionItem mission4 = new MissionItem("Mission 4");
        MissionItem mission5 = new MissionItem("Mission 5");

        mission1.SetLeftIcon((BadgeIcon)API.GetRandomIntInRange(1, 179), SColor.FromRandomValues());
        mission1.SetRightIcon((BadgeIcon)API.GetRandomIntInRange(1, 179), SColor.FromRandomValues());
        mission2.SetLeftIcon((BadgeIcon)API.GetRandomIntInRange(1, 179), SColor.FromRandomValues());
        mission2.SetRightIcon((BadgeIcon)API.GetRandomIntInRange(1, 179), SColor.FromRandomValues());
        mission3.SetLeftIcon((BadgeIcon)API.GetRandomIntInRange(1, 179), SColor.FromRandomValues());
        mission3.SetRightIcon((BadgeIcon)API.GetRandomIntInRange(1, 179), SColor.FromRandomValues());
        mission4.SetLeftIcon((BadgeIcon)API.GetRandomIntInRange(1, 179), SColor.FromRandomValues());
        mission4.SetRightIcon((BadgeIcon)API.GetRandomIntInRange(1, 179), SColor.FromRandomValues());
        mission5.SetLeftIcon((BadgeIcon)API.GetRandomIntInRange(1, 179), SColor.FromRandomValues());
        mission5.SetRightIcon((BadgeIcon)API.GetRandomIntInRange(1, 179), SColor.FromRandomValues(), true);

        playersTab.MissionsColumn.AddMissionItem(mission1);
        playersTab.MissionsColumn.AddMissionItem(mission2);
        playersTab.MissionsColumn.AddMissionItem(mission3);
        playersTab.MissionsColumn.AddMissionItem(mission4);
        playersTab.MissionsColumn.AddMissionItem(mission5);

        playersTab.SettingsColumn.OnIndexChanged += (i) =>
        {
            if (playersTab.MissionsColumn.Items.Count > 0)
            {
                playersTab.MissionsColumn.Clear();
            }

            MissionItem mission1 = new MissionItem("Mission 1");
            MissionItem mission2 = new MissionItem("Mission 2");
            MissionItem mission3 = new MissionItem("Mission 3");
            MissionItem mission4 = new MissionItem("Mission 4");
            MissionItem mission5 = new MissionItem("Mission 5");

            mission1.SetLeftIcon((BadgeIcon)API.GetRandomIntInRange(1, 179), SColor.FromRandomValues());
            mission1.SetRightIcon((BadgeIcon)API.GetRandomIntInRange(1, 179), SColor.FromRandomValues());
            mission2.SetLeftIcon((BadgeIcon)API.GetRandomIntInRange(1, 179), SColor.FromRandomValues());
            mission2.SetRightIcon((BadgeIcon)API.GetRandomIntInRange(1, 179), SColor.FromRandomValues());
            mission3.SetLeftIcon((BadgeIcon)API.GetRandomIntInRange(1, 179), SColor.FromRandomValues());
            mission3.SetRightIcon((BadgeIcon)API.GetRandomIntInRange(1, 179), SColor.FromRandomValues());
            mission4.SetLeftIcon((BadgeIcon)API.GetRandomIntInRange(1, 179), SColor.FromRandomValues());
            mission4.SetRightIcon((BadgeIcon)API.GetRandomIntInRange(1, 179), SColor.FromRandomValues());
            mission5.SetLeftIcon((BadgeIcon)API.GetRandomIntInRange(1, 179), SColor.FromRandomValues());
            mission5.SetRightIcon((BadgeIcon)API.GetRandomIntInRange(1, 179), SColor.FromRandomValues(), true);

            playersTab.MissionsColumn.AddMissionItem(mission1);
            playersTab.MissionsColumn.AddMissionItem(mission2);
            playersTab.MissionsColumn.AddMissionItem(mission3);
            playersTab.MissionsColumn.AddMissionItem(mission4);
            playersTab.MissionsColumn.AddMissionItem(mission5);
        };

        long _paneldui = API.CreateDui("https://i.imgur.com/mH0Y65C.gif", 288, 160);
        API.CreateRuntimeTextureFromDuiHandle(txd, "lobby_panelbackground", API.GetDuiHandle(_paneldui));

        playersTab.MissionPanel.UpdatePanelPicture("scaleformui", "lobby_panelbackground");
        playersTab.MissionPanel.Title = "ScaleformUI - Title";
        UIFreemodeDetailsItem missionItem1 = new("Hellooooo", "I'm here too!", false, ScaleformFonts.GTAV_COURIER, ScaleformFonts.HANDSTYLE_HEIST);
        UIFreemodeDetailsItem missionItem2 = new("Hellooooo", "I'm here too!", ScaleformFonts.GTAV_COURIER, ScaleformFonts.HANDSTYLE_HEIST, BadgeIcon.COUNTRY_ITALY, SColor.HUD_Pure_white, true);
        UIFreemodeDetailsItem missionItem3 = new("Hellooooo", "I'm here too!", true, ScaleformFonts.GTAV_COURIER, ScaleformFonts.HANDSTYLE_HEIST);
        UIFreemodeDetailsItem missionItem4 = new("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat");
        playersTab.MissionPanel.AddItem(missionItem1);
        playersTab.MissionPanel.AddItem(missionItem2);
        playersTab.MissionPanel.AddItem(missionItem3);
        playersTab.MissionPanel.AddItem(missionItem4);


        /*

        CrewTag crew1 = new CrewTag("hello", false, false, CrewHierarchy.Leader, SColor.HUD_Green);
        CrewTag crew2 = new CrewTag("evry1", false, false, CrewHierarchy.Commissioner, SColor.HUD_Pink);
        CrewTag crew3 = new CrewTag("look", false, false, CrewHierarchy.Liutenant, SColor.HUD_Blue);
        CrewTag crew4 = new CrewTag("at", false, false, CrewHierarchy.Representative, SColor.HUD_Orange);
        CrewTag crew5 = new CrewTag("this", false, false, CrewHierarchy.Muscle, SColor.HUD_Red);

        FriendItem friend = new FriendItem(Game.Player.Name + " #1", SColor.HUD_Green, true, API.GetRandomIntInRange(15, 55), "Online", crew1);
        FriendItem friend2 = new FriendItem(Game.Player.Name + " #2", SColor.HUD_Pink, true, API.GetRandomIntInRange(15, 55), "Offline", crew2);
        FriendItem friend3 = new FriendItem(Game.Player.Name + " #3", SColor.HUD_Blue, true, API.GetRandomIntInRange(15, 55), "Online", crew3);
        FriendItem friend4 = new FriendItem(Game.Player.Name + " #4", SColor.HUD_Orange, true, API.GetRandomIntInRange(15, 55), "Offline", crew4);
        FriendItem friend5 = new FriendItem(Game.Player.Name + " #5", SColor.HUD_Red, true, API.GetRandomIntInRange(15, 55), "Busy", crew5);

        //friend.ClonePed = Game.PlayerPed;
        //friend2.ClonePed = Game.PlayerPed;
        //friend3.ClonePed = Game.PlayerPed;
        //friend4.ClonePed = Game.PlayerPed;
        //friend5.ClonePed = Game.PlayerPed;

        friend.SetOnline();
        friend3.SetOnline();
        friend5.SetOnline();

        playersTab.PlayersColumn.AddPlayer(friend);
        playersTab.PlayersColumn.AddPlayer(friend2);
        playersTab.PlayersColumn.AddPlayer(friend3);
        playersTab.PlayersColumn.AddPlayer(friend4);
        playersTab.PlayersColumn.AddPlayer(friend5);


        PlayerStatsPanel panel = new PlayerStatsPanel("Player 1", SColor.HUD_Green)
        {
            Description = "",
            HasPlane = true,
            HasHeli = true,
        };
        panel.RankInfo.RankLevel = 150;
        panel.RankInfo.LowLabel = "This is the low label";
        panel.RankInfo.MidLabel = "This is the middle label";
        panel.RankInfo.UpLabel = "This is the upper label";
        panel.HardwareVisible = false;
        panel.AddStat(new PlayerStatsPanelStatItem("Statistic 1", "Description 1", 50));
        panel.AddStat(new PlayerStatsPanelStatItem("Statistic 2", "Description 2", 100));
        panel.AddStat(new PlayerStatsPanelStatItem("Statistic 3", "Description 3", API.GetRandomIntInRange(30, 150)));
        panel.AddStat(new PlayerStatsPanelStatItem("Statistic 4", "Description 4", API.GetRandomIntInRange(30, 150)));
        panel.AddStat(new PlayerStatsPanelStatItem("Statistic 5", "Description 5", API.GetRandomIntInRange(30, 150)));
        panel.AddStat(new PlayerStatsPanelStatItem("Statistic 6", "Description 6", API.GetRandomIntInRange(30, 150)));
        UIFreemodeDetailsItem descriptionStatItem1 = new("Hellooooo", "I'm here too!", false, ScaleformFonts.GTAV_COURIER, ScaleformFonts.HANDSTYLE_HEIST);
        UIFreemodeDetailsItem descriptionStatItem2 = new("Hellooooo", "I'm here too!", ScaleformFonts.GTAV_COURIER, ScaleformFonts.HANDSTYLE_HEIST, BadgeIcon.COUNTRY_ITALY, SColor.HUD_Pure_white, true);
        UIFreemodeDetailsItem descriptionStatItem3 = new("Hellooooo", "I'm here too!", true, ScaleformFonts.GTAV_COURIER, ScaleformFonts.HANDSTYLE_HEIST);
        UIFreemodeDetailsItem descriptionStatItem4 = new("Hellooooo", "I'm here too!", false, ScaleformFonts.GTAV_COURIER, ScaleformFonts.HANDSTYLE_HEIST);
        UIFreemodeDetailsItem descriptionStatItem5 = new("Hellooooo", "I'm here too!", ScaleformFonts.GTAV_COURIER, ScaleformFonts.HANDSTYLE_HEIST, BadgeIcon.COUNTRY_ITALY, SColor.HUD_Pure_white, true);
        UIFreemodeDetailsItem descriptionStatItem6 = new("Hellooooo", "I'm here too!", true, ScaleformFonts.GTAV_COURIER, ScaleformFonts.HANDSTYLE_HEIST);
        UIFreemodeDetailsItem descriptionStatItem7 = new("Hellooooo", "I'm here too!", false, ScaleformFonts.GTAV_COURIER, ScaleformFonts.HANDSTYLE_HEIST);
        UIFreemodeDetailsItem descriptionStatItem8 = new("Hellooooo", "I'm here too!", ScaleformFonts.GTAV_COURIER, ScaleformFonts.HANDSTYLE_HEIST, BadgeIcon.COUNTRY_ITALY, SColor.HUD_Pure_white, true);
        UIFreemodeDetailsItem descriptionStatItem9 = new("Hellooooo", "I'm here too!", true, ScaleformFonts.GTAV_COURIER, ScaleformFonts.HANDSTYLE_HEIST);
        panel.AddDescriptionStatItem(descriptionStatItem1);
        panel.AddDescriptionStatItem(descriptionStatItem2);
        panel.AddDescriptionStatItem(descriptionStatItem3);
        panel.AddDescriptionStatItem(descriptionStatItem4);
        panel.AddDescriptionStatItem(descriptionStatItem5);
        panel.AddDescriptionStatItem(descriptionStatItem6);
        panel.AddDescriptionStatItem(descriptionStatItem7);
        //panel.AddDescriptionStatItem(descriptionStatItem8);
        //panel.AddDescriptionStatItem(descriptionStatItem9);
        friend.AddPanel(panel);

        PlayerStatsPanel panel2 = new PlayerStatsPanel("Player 2", SColor.HUD_Pink)
        {
            Description = "This is the description for Player 2!!",
            HasPlane = true,
            HasHeli = true,
            HasVehicle = true
        };
        panel2.RankInfo.RankLevel = 15;
        panel2.RankInfo.LowLabel = "This is the low label";
        panel2.RankInfo.MidLabel = "This is the middle label";
        panel2.RankInfo.UpLabel = "This is the upper label";
        panel2.AddStat(new PlayerStatsPanelStatItem("Statistic 1", "Description 1", API.GetRandomIntInRange(30, 150)));
        panel2.AddStat(new PlayerStatsPanelStatItem("Statistic 2", "Description 2", API.GetRandomIntInRange(30, 150)));
        panel2.AddStat(new PlayerStatsPanelStatItem("Statistic 3", "Description 3", API.GetRandomIntInRange(30, 150)));
        panel2.AddStat(new PlayerStatsPanelStatItem("Statistic 4", "Description 4", API.GetRandomIntInRange(30, 150)));
        panel2.AddStat(new PlayerStatsPanelStatItem("Statistic 5", "Description 5", API.GetRandomIntInRange(30, 150)));
        friend2.AddPanel(panel2);

        PlayerStatsPanel panel3 = new PlayerStatsPanel("Player 3", SColor.HUD_Freemode)
        {
            Description = "This is the description for Player 3!!",
            HasPlane = true,
            HasHeli = true,
            HasBoat = true
        };
        panel3.RankInfo.RankLevel = 10;
        panel3.RankInfo.LowLabel = "This is the low label";
        panel3.RankInfo.MidLabel = "This is the middle label";
        panel3.RankInfo.UpLabel = "This is the upper label";
        panel3.AddStat(new PlayerStatsPanelStatItem("Statistic 1", "Description 1", API.GetRandomIntInRange(30, 150)));
        panel3.AddStat(new PlayerStatsPanelStatItem("Statistic 2", "Description 2", API.GetRandomIntInRange(30, 150)));
        panel3.AddStat(new PlayerStatsPanelStatItem("Statistic 3", "Description 3", API.GetRandomIntInRange(30, 150)));
        panel3.AddStat(new PlayerStatsPanelStatItem("Statistic 4", "Description 4", API.GetRandomIntInRange(30, 150)));
        panel3.AddStat(new PlayerStatsPanelStatItem("Statistic 5", "Description 5", API.GetRandomIntInRange(30, 150)));
        friend3.AddPanel(panel3);

        PlayerStatsPanel panel4 = new PlayerStatsPanel("Player 4", SColor.HUD_Orange)
        {
            Description = "This is the description for Player 4!!",
            HasPlane = true,
            HasHeli = true,
        };
        panel4.RankInfo.RankLevel = 1000;
        panel4.RankInfo.LowLabel = "This is the low label";
        panel4.RankInfo.MidLabel = "This is the middle label";
        panel4.RankInfo.UpLabel = "This is the upper label";
        panel4.AddStat(new PlayerStatsPanelStatItem("Statistic 1", "Description 1", API.GetRandomIntInRange(30, 150)));
        panel4.AddStat(new PlayerStatsPanelStatItem("Statistic 2", "Description 2", API.GetRandomIntInRange(30, 150)));
        panel4.AddStat(new PlayerStatsPanelStatItem("Statistic 3", "Description 3", API.GetRandomIntInRange(30, 150)));
        panel4.AddStat(new PlayerStatsPanelStatItem("Statistic 4", "Description 4", API.GetRandomIntInRange(30, 150)));
        panel4.AddStat(new PlayerStatsPanelStatItem("Statistic 5", "Description 5", API.GetRandomIntInRange(30, 150)));
        friend4.AddPanel(panel4);

        PlayerStatsPanel panel5 = new PlayerStatsPanel("Player 5", SColor.HUD_Red)
        {
            Description = "This is the description for Player 5!!",
            HasPlane = true,
            HasHeli = true,
        };
        panel5.RankInfo.RankLevel = 22;
        panel5.RankInfo.LowLabel = "This is the low label";
        panel5.RankInfo.MidLabel = "This is the middle label";
        panel5.RankInfo.UpLabel = "This is the upper label";
        panel5.AddStat(new PlayerStatsPanelStatItem("Statistic 1", "Description 1", API.GetRandomIntInRange(30, 150)));
        panel5.AddStat(new PlayerStatsPanelStatItem("Statistic 2", "Description 2", API.GetRandomIntInRange(30, 150)));
        panel5.AddStat(new PlayerStatsPanelStatItem("Statistic 3", "Description 3", API.GetRandomIntInRange(30, 150)));
        panel5.AddStat(new PlayerStatsPanelStatItem("Statistic 4", "Description 4", API.GetRandomIntInRange(30, 150)));
        panel5.AddStat(new PlayerStatsPanelStatItem("Statistic 5", "Description 5", API.GetRandomIntInRange(30, 150)));
        friend5.AddPanel(panel5);
        */
        pauseMenu.OnPauseMenuOpen += (menu) =>
        {
            Screen.ShowSubtitle(menu.Title + " Opened!");
            Debug.WriteLine(menu.Title + " Opened!");
            if (mainMenu != null)
                mainMenu.Visible = false;
        };

        pauseMenu.OnPauseMenuClose += async (menu) =>
        {
            Screen.ShowSubtitle(menu.Title + " Closed!");
            Debug.WriteLine(menu.Title + " Closed!");
            // to prevent the pause menu to close the menu too!

            // clear the player list
            foreach (BaseTab tab in menu.Tabs)
            {
                if (tab is PlayerListTab)
                {
                    PlayerListTab t = tab as PlayerListTab;
                    if (t.PlayersColumn != null)
                        t.PlayersColumn.Items.ForEach(item => item.Dispose());
                }
            }

            await BaseScript.Delay(250);
            if (mainMenu != null)
                mainMenu.Visible = true;
        };

        pauseMenu.OnPauseMenuTabChanged += (menu, tab, tabIndex) =>
        {
            Screen.ShowSubtitle(tab.Title + " Selected!");
            Debug.WriteLine(tab.Title + " Selected!");
        };

        pauseMenu.OnPauseMenuFocusChanged += (menu, tab, focusLevel) =>
        {
            Screen.ShowSubtitle(tab.Title + " Focus at level => ~y~" + focusLevel + "~w~!");
            Debug.WriteLine(tab.Title + " Focus at level => ~y~" + focusLevel + "~w~!");

            if (focusLevel == 1)
            {
                if (tab is TextTab)
                {
                    List<InstructionalButton> buttons = new List<InstructionalButton>()
                    {
                        new InstructionalButton(Control.PhoneCancel, Game.GetGXTEntry("HUD_INPUT3")),
                        new InstructionalButton(Control.LookUpDown, "Scroll text", PadCheck.Controller),
                        new InstructionalButton(InputGroup.INPUTGROUP_CURSOR_SCROLL, "Scroll text", PadCheck.Keyboard)

                    };

                    ScaleformUI.Main.InstructionalButtons.SetInstructionalButtons(buttons);
                }
            }
            else if (focusLevel == 0)
                ScaleformUI.Main.InstructionalButtons.SetInstructionalButtons(menu.InstructionalButtons);

        };

        pauseMenu.OnLeftItemChange += (menu, leftItem, leftItemIndex) =>
        {
            Screen.ShowSubtitle(menu.Tabs[menu.Index].Title + " Focus at level => ~y~" + menu.FocusLevel + "~w~, and left Item ~o~N° " + (leftItemIndex + 1) + "~w~ selected!");
            Debug.WriteLine(menu.Tabs[menu.Index].Title + " Focus at level => ~y~" + menu.FocusLevel + "~w~, and left Item ~o~N° " + (leftItemIndex + 1) + "~w~ selected!");
        };

        pauseMenu.OnLeftItemSelect += (menu, leftItem, leftItemIndex) =>
        {
            Screen.ShowSubtitle(menu.Tabs[menu.Index].Title + " Focus at level => ~y~" + menu.FocusLevel + "~w~, and left Item ~o~N° " + (leftItemIndex + 1) + "~w~ selected!");
            Debug.WriteLine(menu.Tabs[menu.Index].Title + " Focus at level => ~y~" + menu.FocusLevel + "~w~, and left Item ~o~N° " + (leftItemIndex + 1) + "~w~ selected!");
        };

        pauseMenu.OnRightItemChange += (menu, item, leftItemIndex, rightItemIndex) =>
        {
            Screen.ShowSubtitle(menu.Tabs[menu.Index].Title + " Focus at level => ~y~" + menu.FocusLevel + "~w~, left Item ~o~N° " + (leftItemIndex + 1) + "~w~ and right Item ~b~N° " + (rightItemIndex + 1) + "~w~ selected!");
            Debug.WriteLine(menu.Tabs[menu.Index].Title + " Focus at level => ~y~" + menu.FocusLevel + "~w~, left Item ~o~N° " + (leftItemIndex + 1) + "~w~ and right Item ~b~N° " + (rightItemIndex + 1) + "~w~ selected!");
        };

        pauseMenu.OnRightItemSelect += (menu, item, leftItemIndex, rightItemIndex) =>
        {
            Screen.ShowSubtitle(menu.Tabs[menu.Index].Title + "~w~, left Item ~o~N° " + (leftItemIndex + 1) + "~w~ and right Item ~b~N° " + (rightItemIndex + 1) + "~w~ of type ~p~" + item.ItemType + "~w~ selected!");
            Debug.WriteLine(menu.Tabs[menu.Index].Title + "~w~, left Item ~o~N° " + (leftItemIndex + 1) + "~w~ and right Item ~b~N° " + (rightItemIndex + 1) + "~w~ of type ~p~" + item.ItemType + "~w~ selected!");
        };
        pauseMenu.Visible = true;
        //API.UnregisterPedheadshot(mugshot);
    }

    public async void LobbyPauseMenuShowcase(UIMenu _menu)
    {
        UIMenu mainMenu = _menu;
        // tabview is the main menu.. the container of all the tabs.
        MainView pauseMenu = new("Lobby Menu", "ScaleformUI for you by Manups4e!", "Detail 1", "Detail 2", "Detail 3", false);
        pauseMenu.CanPlayerCloseMenu = true;
        // this is a showcase... CanPlayerCloseMenu is always defaulted to true.. if false players won't be able to close the menu!
        List<Column> columns = new List<Column>()
        {
            new SettingsListColumn("COLUMN SETTINGS", SColor.HUD_Red),
            new PlayerListColumn("COLUMN PLAYERS", SColor.HUD_Orange),
            new MissionDetailsPanel("COLUMN INFO PANEL", SColor.HUD_Green),
        };
        pauseMenu.SetUpColumns(columns);
        int mugshot = API.RegisterPedheadshot(Game.PlayerPed.Handle);
        while (!API.IsPedheadshotReady(mugshot)) await BaseScript.Delay(1);
        string ped_txd = API.GetPedheadshotTxdString(mugshot);
        pauseMenu.HeaderPicture = new(ped_txd, ped_txd);

        UIMenuItem item = new UIMenuItem("UIMenuItem", "UIMenuItem description");
        UIMenuListItem item1 = new UIMenuListItem("~g~UIMenuListItem", new List<dynamic>() { "This", "is", "a", "Test" }, 0, "UIMenuListItem description");
        UIMenuCheckboxItem item2 = new UIMenuCheckboxItem("~b~UIMenuCheckboxItem", true, "UIMenuCheckboxItem description");
        UIMenuSliderItem item3 = new UIMenuSliderItem("~p~UIMenuSliderItem", "UIMenuSliderItem description", 100, 5, 50, false);
        UIMenuProgressItem item4 = new UIMenuProgressItem("~o~UIMenuProgressItem", 10, 5, "UIMenuProgressItem description");
        item.LabelFont = ScaleformFonts.ENGRAVERS_OLD_ENGLISH_MT_STD;
        item.BlinkDescription = true;
        item1.Enabled = false;
        pauseMenu.SettingsColumn.AddSettings(item);
        pauseMenu.SettingsColumn.AddSettings(item1);
        pauseMenu.SettingsColumn.AddSettings(item2);
        pauseMenu.SettingsColumn.AddSettings(item3);
        pauseMenu.SettingsColumn.AddSettings(item4);

        item1.OnListChanged += (item, idx) =>
        {
            Screen.ShowSubtitle("ListItem selected, Value => ~y~ " + item.Items[idx].ToString() + "~s~~w~");
        };

        item.Activated += (_, item) =>
        {
            Screen.ShowSubtitle($"~y~ {item.Label} ~s~~w~ has been selected!");
        };

        CrewTag crew1 = new CrewTag("hello", false, false, CrewHierarchy.Leader, SColor.HUD_Green);
        CrewTag crew2 = new CrewTag("evry1", false, false, CrewHierarchy.Commissioner, SColor.HUD_Pink);
        CrewTag crew3 = new CrewTag("look", false, false, CrewHierarchy.Liutenant, SColor.HUD_Blue);
        CrewTag crew4 = new CrewTag("at", false, false, CrewHierarchy.Representative, SColor.HUD_Orange);
        CrewTag crew5 = new CrewTag("this", false, false, CrewHierarchy.Muscle, SColor.HUD_Red);

        FriendItem friend = new FriendItem(Game.Player.Name, SColor.HUD_Green, true, API.GetRandomIntInRange(15, 55), "Status", crew1);
        FriendItem friend2 = new FriendItem(Game.Player.Name, SColor.HUD_Pink, true, API.GetRandomIntInRange(15, 55), "Status", crew2);
        FriendItem friend3 = new FriendItem(Game.Player.Name, SColor.HUD_Blue, true, API.GetRandomIntInRange(15, 55), "Status", crew3);
        FriendItem friend4 = new FriendItem(Game.Player.Name, SColor.HUD_Orange, true, API.GetRandomIntInRange(15, 55), "Status", crew4);
        FriendItem friend5 = new FriendItem(Game.Player.Name, SColor.HUD_Red, true, API.GetRandomIntInRange(15, 55), "Status", crew5);

        friend.SetLeftIcon(LobbyBadgeIcon.IS_CONSOLE_PLAYER);
        friend2.SetLeftIcon(LobbyBadgeIcon.SPECTATOR);
        friend3.SetLeftIcon(LobbyBadgeIcon.INACTIVE_HEADSET);
        friend4.SetLeftIcon(BadgeIcon.COUNTRY_ITALY);
        friend5.SetLeftIcon(BadgeIcon.CASTLE);

        friend.ClonePed = Game.PlayerPed;
        friend2.ClonePed = Game.PlayerPed;
        friend3.ClonePed = Game.PlayerPed;
        friend4.ClonePed = Game.PlayerPed;
        friend5.ClonePed = Game.PlayerPed;

        PlayerStatsPanel panel = new PlayerStatsPanel("Player 1", SColor.HUD_Green)
        {
            Description = "This is the description for Player 1!!",
            HasPlane = true,
            HasHeli = true,
        };
        panel.RankInfo.RankLevel = 150;
        panel.RankInfo.LowLabel = "This is the low label";
        panel.RankInfo.MidLabel = "This is the middle label";
        panel.RankInfo.UpLabel = "This is the upper label";
        panel.AddStat(new PlayerStatsPanelStatItem("Statistic 1", "Description 1", API.GetRandomIntInRange(30, 150)));
        panel.AddStat(new PlayerStatsPanelStatItem("Statistic 2", "Description 2", API.GetRandomIntInRange(30, 150)));
        panel.AddStat(new PlayerStatsPanelStatItem("Statistic 3", "Description 3", API.GetRandomIntInRange(30, 150)));
        panel.AddStat(new PlayerStatsPanelStatItem("Statistic 4", "Description 4", API.GetRandomIntInRange(30, 150)));
        panel.AddStat(new PlayerStatsPanelStatItem("Statistic 5", "Description 5", API.GetRandomIntInRange(30, 150)));
        friend.AddPanel(panel);

        PlayerStatsPanel panel2 = new PlayerStatsPanel("Player 2", SColor.HUD_Menu_yellow)
        {
            Description = "This is the description for Player 2!!",
            HasPlane = true,
            HasVehicle = true,
        };
        panel2.RankInfo.RankLevel = 70;
        panel2.RankInfo.LowLabel = "This is the low label";
        panel2.RankInfo.MidLabel = "This is the middle label";
        panel2.RankInfo.UpLabel = "This is the upper label";
        panel2.AddStat(new PlayerStatsPanelStatItem("Statistic 1", "Description 1", API.GetRandomIntInRange(30, 150)));
        panel2.AddStat(new PlayerStatsPanelStatItem("Statistic 2", "Description 2", API.GetRandomIntInRange(30, 150)));
        panel2.AddStat(new PlayerStatsPanelStatItem("Statistic 3", "Description 3", API.GetRandomIntInRange(30, 150)));
        panel2.AddStat(new PlayerStatsPanelStatItem("Statistic 4", "Description 4", API.GetRandomIntInRange(30, 150)));
        panel2.AddStat(new PlayerStatsPanelStatItem("Statistic 5", "Description 5", API.GetRandomIntInRange(30, 150)));
        friend2.AddPanel(panel2);

        PlayerStatsPanel panel3 = new PlayerStatsPanel("Player 3", SColor.HUD_Pink)
        {
            Description = "This is the description for Player 3!!",
            HasPlane = true,
            HasHeli = true,
            HasVehicle = true
        };
        panel3.RankInfo.RankLevel = 15;
        panel3.RankInfo.LowLabel = "This is the low label";
        panel3.RankInfo.MidLabel = "This is the middle label";
        panel3.RankInfo.UpLabel = "This is the upper label";
        panel3.AddStat(new PlayerStatsPanelStatItem("Statistic 1", "Description 1", API.GetRandomIntInRange(30, 150)));
        panel3.AddStat(new PlayerStatsPanelStatItem("Statistic 2", "Description 2", API.GetRandomIntInRange(30, 150)));
        panel3.AddStat(new PlayerStatsPanelStatItem("Statistic 3", "Description 3", API.GetRandomIntInRange(30, 150)));
        panel3.AddStat(new PlayerStatsPanelStatItem("Statistic 4", "Description 4", API.GetRandomIntInRange(30, 150)));
        panel3.AddStat(new PlayerStatsPanelStatItem("Statistic 5", "Description 5", API.GetRandomIntInRange(30, 150)));
        friend3.AddPanel(panel3);

        PlayerStatsPanel panel4 = new PlayerStatsPanel("Player 4", SColor.HUD_Freemode)
        {
            Description = "This is the description for Player 4!!",
            HasPlane = true,
            HasHeli = true,
            HasBoat = true
        };
        panel4.RankInfo.RankLevel = 10;
        panel4.RankInfo.LowLabel = "This is the low label";
        panel4.RankInfo.MidLabel = "This is the middle label";
        panel4.RankInfo.UpLabel = "This is the upper label";
        panel4.AddStat(new PlayerStatsPanelStatItem("Statistic 1", "Description 1", API.GetRandomIntInRange(30, 150)));
        panel4.AddStat(new PlayerStatsPanelStatItem("Statistic 2", "Description 2", API.GetRandomIntInRange(30, 150)));
        panel4.AddStat(new PlayerStatsPanelStatItem("Statistic 3", "Description 3", API.GetRandomIntInRange(30, 150)));
        panel4.AddStat(new PlayerStatsPanelStatItem("Statistic 4", "Description 4", API.GetRandomIntInRange(30, 150)));
        panel4.AddStat(new PlayerStatsPanelStatItem("Statistic 5", "Description 5", API.GetRandomIntInRange(30, 150)));
        friend4.AddPanel(panel4);

        PlayerStatsPanel panel5 = new PlayerStatsPanel("Player 5", SColor.HUD_Orange)
        {
            Description = "This is the description for Player 5!!",
            HasPlane = true,
            HasHeli = true,
        };
        panel5.RankInfo.RankLevel = 1000;
        panel5.RankInfo.LowLabel = "This is the low label";
        panel5.RankInfo.MidLabel = "This is the middle label";
        panel5.RankInfo.UpLabel = "This is the upper label";
        panel5.AddStat(new PlayerStatsPanelStatItem("Statistic 1", "Description 1", API.GetRandomIntInRange(30, 150)));
        panel5.AddStat(new PlayerStatsPanelStatItem("Statistic 2", "Description 2", API.GetRandomIntInRange(30, 150)));
        panel5.AddStat(new PlayerStatsPanelStatItem("Statistic 3", "Description 3", API.GetRandomIntInRange(30, 150)));
        panel5.AddStat(new PlayerStatsPanelStatItem("Statistic 4", "Description 4", API.GetRandomIntInRange(30, 150)));
        panel5.AddStat(new PlayerStatsPanelStatItem("Statistic 5", "Description 5", API.GetRandomIntInRange(30, 150)));
        friend5.AddPanel(panel5);

        PlayerStatsPanel panel6 = new PlayerStatsPanel("Player 6", SColor.HUD_Red)
        {
            Description = "This is the description for Player 6!!",
            HasPlane = true,
            HasHeli = true,
        };
        panel6.RankInfo.RankLevel = 22;
        panel6.RankInfo.LowLabel = "This is the low label";
        panel6.RankInfo.MidLabel = "This is the middle label";
        panel6.RankInfo.UpLabel = "This is the upper label";
        panel6.AddStat(new PlayerStatsPanelStatItem("Statistic 1", "Description 1", API.GetRandomIntInRange(30, 150)));
        panel6.AddStat(new PlayerStatsPanelStatItem("Statistic 2", "Description 2", API.GetRandomIntInRange(30, 150)));
        panel6.AddStat(new PlayerStatsPanelStatItem("Statistic 3", "Description 3", API.GetRandomIntInRange(30, 150)));
        panel6.AddStat(new PlayerStatsPanelStatItem("Statistic 4", "Description 4", API.GetRandomIntInRange(30, 150)));
        panel6.AddStat(new PlayerStatsPanelStatItem("Statistic 5", "Description 5", API.GetRandomIntInRange(30, 150)));
        friend5.AddPanel(panel6);

        pauseMenu.PlayersColumn.AddPlayer(friend);
        pauseMenu.PlayersColumn.AddPlayer(friend2);
        pauseMenu.PlayersColumn.AddPlayer(friend3);
        pauseMenu.PlayersColumn.AddPlayer(friend4);
        pauseMenu.PlayersColumn.AddPlayer(friend5);

        long _paneldui = API.CreateDui("https://i.imgur.com/mH0Y65C.gif", 288, 160);
        API.CreateRuntimeTextureFromDuiHandle(txd, "lobby_panelbackground", API.GetDuiHandle(_paneldui));

        pauseMenu.MissionPanel.UpdatePanelPicture("scaleformui", "lobby_panelbackground");
        pauseMenu.MissionPanel.Title = "ScaleformUI - Title";
        UIFreemodeDetailsItem missionItem1 = new("Hellooooo", "I'm here too!", false, ScaleformFonts.GTAV_COURIER, ScaleformFonts.HANDSTYLE_HEIST);
        UIFreemodeDetailsItem missionItem2 = new("Hellooooo", "I'm here too!", ScaleformFonts.GTAV_COURIER, ScaleformFonts.HANDSTYLE_HEIST, BadgeIcon.COUNTRY_ITALY, SColor.HUD_Pure_white, true);
        UIFreemodeDetailsItem missionItem3 = new("Hellooooo", "I'm here too!", true, ScaleformFonts.GTAV_COURIER, ScaleformFonts.HANDSTYLE_HEIST);
        UIFreemodeDetailsItem missionItem4 = new("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat");
        //UIFreemodeDetailsItem missionItem4 = new("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat", "", false);
        pauseMenu.MissionPanel.AddItem(missionItem1);
        pauseMenu.MissionPanel.AddItem(missionItem2);
        pauseMenu.MissionPanel.AddItem(missionItem3);
        pauseMenu.MissionPanel.AddItem(missionItem4);

        pauseMenu.Visible = true;
        await BaseScript.Delay(1000);
        item1.Label = "Pippo";
        pauseMenu.SettingsColumn.CurrentSelection = 1;
        //API.UnregisterPedheadshot(mugshot);
    }

    bool feedOpen = false;
    Random r = new Random();
    public MenuExample()
    {/*
        _timerBarPool = new TimerBarPool();
        TextTimerBar textTimerBar = new TextTimerBar("Label", "Caption", CitizenFX.Core.UI.Font.Pricedown);
        _timerBarPool.Add(textTimerBar);
        TextTimerBar textTimerBar2 = new TextTimerBar("Other", "Caption", CitizenFX.Core.UI.Font.ChaletComprimeCologne, CitizenFX.Core.UI.Font.HouseScript);
        _timerBarPool.Add(textTimerBar2);
        */
        txd = API.CreateRuntimeTxd("scaleformui");

        // We create a marker on the peds position, adds it to the MarkerHandler
        Marker playerMarker = new Marker(MarkerType.VerticalCylinder, Game.PlayerPed.Position, new Vector3(1.5f), 5f, SColor.Cyan, true);
        MarkersHandler.AddMarker(playerMarker);

        Tick += async () =>
        {
            //_timerBarPool.Draw();

            //If the player is in drawing range for the marker, the marker will draw automatically and the DrawText will show itself (true if the ped enters the marker)
            if (playerMarker.IsInRange)
                Notifications.DrawText(text: $"IsInMarker => {playerMarker.IsInMarker}");

            if (Game.IsControlJustPressed(0, Control.SelectCharacterMichael) && !MenuHandler.IsAnyMenuOpen) // Our menu enabler (to exit menu simply press Back on the main menu)
                ExampleMenu();

            if (Game.IsControlJustPressed(0, Control.DropAmmo) && !Game.IsControlPressed(0, Control.Sprint) && !MenuHandler.IsAnyMenuOpen)
                CreateRadialMenu();
            if (Game.IsControlJustPressed(0, Control.DropAmmo) && Game.IsControlPressed(0, Control.Sprint) && !MenuHandler.IsAnyMenuOpen)
                CreateRadioMenu();

            // to open the pause menu without opening the normal menu.
            if (Game.IsControlJustPressed(0, Control.SelectCharacterFranklin) && !MenuHandler.IsAnyMenuOpen && !MenuHandler.IsAnyPauseMenuOpen)
                PauseMenuShowcase(null);
            if (Game.IsControlJustPressed(0, Control.SelectCharacterTrevor) && !MenuHandler.IsAnyMenuOpen && !MenuHandler.IsAnyPauseMenuOpen)
                LobbyPauseMenuShowcase(null);

            /*
            if (Game.IsControlJustPressed(0, Control.Detonate) || Game.IsDisabledControlJustPressed(0, Control.Detonate))
            {

                long _wallp = API.CreateDui("https://images8.alphacoders.com/132/1322626.png", 2560, 1440);
                API.CreateRuntimeTextureFromDuiHandle(txd, "wallp", API.GetDuiHandle(_wallp));

                int overlay1 = await MinimapOverlays.AddSizedOverlayToMap("scaleformui", "bannerbackground", 365, -422, centered: true);
                World.CreateBlip(new Vector3(365, -422, 45));

                int overlay2 = await MinimapOverlays.AddSizedOverlayToMap("scaleformui", "wallp", 2000, 1000, centered: true);
            }
            */

            if (Game.IsControlJustPressed(0, (Control)170)) // F3
            {
                if (ScaleformUI.Main.JobMissionSelection.Enabled)
                {
                    ScaleformUI.Main.JobMissionSelection.Enabled = false;
                    return;
                }

                long txd = API.CreateRuntimeTxd("test");
                long _paneldui = API.CreateDui("https://i.imgur.com/mH0Y65C.gif", 288, 160);
                API.CreateRuntimeTextureFromDuiHandle(txd, "panelbackground", API.GetDuiHandle(_paneldui));

                ScaleformUI.Main.JobMissionSelection.SetTitle("MISSION SELECTOR");
                ScaleformUI.Main.JobMissionSelection.MaxVotes = 3;
                ScaleformUI.Main.JobMissionSelection.SetVotes(0, "VOTES");
                ScaleformUI.Main.JobMissionSelection.Cards = new List<JobSelectionCard>();

                JobSelectionCard card = new JobSelectionCard("Test 1", "~y~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat", "test", "panelbackground", 12, 15, JobSelectionCardIcon.BASE_JUMPING, HudColor.HUD_COLOUR_FREEMODE, 2, new List<MissionDetailsItem>()
                {
                    new MissionDetailsItem("Left Label", "Right Label", 0, HudColor.HUD_COLOUR_FREEMODE),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)1, HudColor.HUD_COLOUR_GOLD),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)2, HudColor.HUD_COLOUR_PURPLE),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)3, HudColor.HUD_COLOUR_GREEN),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)4, HudColor.HUD_COLOUR_WHITE, true),
                });
                ScaleformUI.Main.JobMissionSelection.AddCard(card);

                JobSelectionCard card1 = new JobSelectionCard("Test 2", "~y~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat", "test", "panelbackground", 12, 15, JobSelectionCardIcon.BASE_JUMPING, HudColor.HUD_COLOUR_FREEMODE, 2, new List<MissionDetailsItem>()
                {
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)5, HudColor.HUD_COLOUR_FREEMODE),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)6, HudColor.HUD_COLOUR_GOLD),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)7, HudColor.HUD_COLOUR_PURPLE),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)8, HudColor.HUD_COLOUR_GREEN),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)9, HudColor.HUD_COLOUR_WHITE, true),
                });
                ScaleformUI.Main.JobMissionSelection.AddCard(card1);

                JobSelectionCard card2 = new JobSelectionCard("Test 3", "~y~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat", "test", "panelbackground", 12, 15, JobSelectionCardIcon.BASE_JUMPING, HudColor.HUD_COLOUR_FREEMODE, 2, new List<MissionDetailsItem>()
                {
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)10, HudColor.HUD_COLOUR_FREEMODE),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)11, HudColor.HUD_COLOUR_GOLD),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)12, HudColor.HUD_COLOUR_PURPLE),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)13, HudColor.HUD_COLOUR_GREEN),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)14, HudColor.HUD_COLOUR_WHITE, true),
                });
                ScaleformUI.Main.JobMissionSelection.AddCard(card2);

                JobSelectionCard card3 = new JobSelectionCard("Test 4", "~y~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat", "test", "panelbackground", 12, 15, JobSelectionCardIcon.BASE_JUMPING, HudColor.HUD_COLOUR_FREEMODE, 2, new List<MissionDetailsItem>()
                {
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)15, HudColor.HUD_COLOUR_FREEMODE),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)16, HudColor.HUD_COLOUR_GOLD),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)17, HudColor.HUD_COLOUR_PURPLE),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)18, HudColor.HUD_COLOUR_GREEN),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)19, HudColor.HUD_COLOUR_WHITE, true),
                });
                ScaleformUI.Main.JobMissionSelection.AddCard(card3);

                JobSelectionCard card4 = new JobSelectionCard("Test 5", "~y~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat", "test", "panelbackground", 12, 15, JobSelectionCardIcon.BASE_JUMPING, HudColor.HUD_COLOUR_FREEMODE, 2, new List<MissionDetailsItem>()
                {
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)20, HudColor.HUD_COLOUR_FREEMODE),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)21, HudColor.HUD_COLOUR_GOLD),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)22, HudColor.HUD_COLOUR_PURPLE),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)23, HudColor.HUD_COLOUR_GREEN),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)24, HudColor.HUD_COLOUR_WHITE, true),
                });
                ScaleformUI.Main.JobMissionSelection.AddCard(card4);

                JobSelectionCard card5 = new JobSelectionCard("Test 6", "~y~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat", "test", "panelbackground", 12, 15, JobSelectionCardIcon.BASE_JUMPING, HudColor.HUD_COLOUR_FREEMODE, 2, new List<MissionDetailsItem>()
                {
                    new MissionDetailsItem("Left Label", "Right Label", 0, HudColor.HUD_COLOUR_FREEMODE),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)1, HudColor.HUD_COLOUR_GOLD),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)2, HudColor.HUD_COLOUR_PURPLE),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)3, HudColor.HUD_COLOUR_GREEN),
                    new MissionDetailsItem("Left Label", "Right Label", (JobIcon)4, HudColor.HUD_COLOUR_WHITE, true),
                });
                ScaleformUI.Main.JobMissionSelection.AddCard(card5);

                ScaleformUI.Main.JobMissionSelection.Buttons = new List<JobSelectionButton>()
                {
                    new JobSelectionButton("Test1", "description test", new List<MissionDetailsItem>()) {Selectable = false },

                    new JobSelectionButton("Test2", "description test", new List<MissionDetailsItem>()) {Selectable = false },

                    new JobSelectionButton("Test3", "description test", new List<MissionDetailsItem>()) {Selectable = true },
                };
                ScaleformUI.Main.JobMissionSelection.Buttons[0].OnButtonPressed += () =>
                {
                    Screen.ShowSubtitle($"Button Pressed => {ScaleformUI.Main.JobMissionSelection.Buttons[0].Text}");
                };

                ScaleformUI.Main.JobMissionSelection.Enabled = true;

                await Delay(1000);
                ScaleformUI.Main.JobMissionSelection.ShowPlayerVote(2, "PlayerName", HudColor.HUD_COLOUR_GREEN, true, true);
            }
            if (Game.IsControlJustPressed(0, Control.DropWeapon)) // F9
            {
                feedOpen = !feedOpen;
                ScaleformUI.Main.BigFeed.Title = "Super Title!";
                ScaleformUI.Main.BigFeed.Subtitle = "Super Subtitle";
                ScaleformUI.Main.BigFeed.BodyText = "~input_context~ 🥳 ~y~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat";
                ScaleformUI.Main.BigFeed.UpdatePicture("", ""); // it doesn't support DUI runtime textures!
                ScaleformUI.Main.BigFeed.RightAligned = true; // false to center align it
                ScaleformUI.Main.BigFeed.Enabled = feedOpen;
            }
        };
    }
}
