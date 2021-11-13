﻿using System;
using System.Drawing;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using NativeUI;
using CitizenFX.Core.Native;
using System.Linq;
using System.Threading.Tasks;
using NativeUI.PauseMenu;
using NativeUI.PauseMenu.Items;
using NativeUI.PauseMenu.Tabs;

public class MenuExample : BaseScript
{
	private bool ketchup = true;
	private string dish = "Banana";
	private MenuPool _menuPool;

	public void AddMenuKetchup(UIMenu menu)
	{
		var newitem = new UIMenuCheckboxItem("Add ketchup?", UIMenuCheckboxStyle.Cross, ketchup, "Do you wish to add ketchup?");
		menu.AddItem(newitem);
		menu.OnCheckboxChange += (sender, item, checked_) =>
		{
			if (item == newitem)
			{
				ketchup = checked_;
				Notifications.ShowNotification("~r~Ketchup status: ~b~" + ketchup);
			}
		};
	}

	public void HeritageMenu(UIMenu menu)
	{
		var heritagemenu = _menuPool.AddSubMenu(menu, "Heritage Menu");
		var heritageWindow = new UIMenuHeritageWindow(0, 0);
		heritagemenu.AddWindow(heritageWindow);
		List<dynamic> momfaces = new List<dynamic>() { "Hannah", "Audrey", "Jasmine", "Giselle", "Amelia", "Isabella", "Zoe", "Ava", "Camilla", "Violet", "Sophia", "Eveline", "Nicole", "Ashley", "Grace", "Brianna", "Natalie", "Olivia", "Elizabeth", "Charlotte", "Emma", "Misty" };
		List<dynamic> dadfaces = new List<dynamic>() { "Benjamin", "Daniel", "Joshua", "Noah", "Andrew", "Joan", "Alex", "Isaac", "Evan", "Ethan", "Vincent", "Angel", "Diego", "Adrian", "Gabriel", "Michael", "Santiago", "Kevin", "Louis", "Samuel", "Anthony", "Claude", "Niko", "John" };
		List<dynamic> lista = new List<dynamic>();
		for (int i = 0; i < 101; i++) lista.Add(i);
		var mom = new UIMenuListItem("Mamma", momfaces, 0);
		var dad = new UIMenuListItem("Papà", dadfaces, 0);
		var newItem = new UIMenuSliderItem("Heritage Slider", "This is Useful on heritage", true);
		heritagemenu.AddItem(mom);
		heritagemenu.AddItem(dad);
		heritagemenu.AddItem(newItem);
		int MomIndex = 0;
		int DadIndex = 0;
		heritagemenu.OnListChange += (_sender, _listItem, _newIndex) =>
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
	}

	public void AddMenuFoods(UIMenu menu)
	{
		var foods = new List<dynamic>
		{
			"Banana",
			"Apple",
			"Pizza",
			"Quartilicious",
			0xF00D, // Dynamic!
        };
		var newitem = new UIMenuListItem("Food", foods, 0);
		menu.AddItem(newitem);
		menu.OnListChange += (sender, item, index) =>
		{
			if (item == newitem)
			{
				dish = item.Items[index].ToString();
				Notifications.ShowNotification("Preparing ~b~" + dish + "~w~...");
			}

		};
	}

	public void AddScaleformMenu(UIMenu menu)
	{
		var scaleformMenu = _menuPool.AddSubMenu(menu, "Scaleforms Showdown");
		UIMenuItem showSimplePopup = new UIMenuItem("Show PopupWarning example", "You can customize it to your needs");
		UIMenuItem showPopupButtons = new UIMenuItem("Show PopupWarning with buttons", "It waits until a button has been pressed!");
		UIMenuListItem customInstr = new UIMenuListItem("SavingNotification", Enum.GetNames(typeof(LoadingSpinnerType)).Cast<dynamic>().ToList(), 0, "InstructionalButtons now give you the ability to dynamically edit, add, remove, customize your buttons, you can even use them outside the menu ~y~without having to run multiple instances of the same scaleform~w~, aren't you happy??");
		UIMenuItem customInstr2 = new UIMenuItem("Add a random InstructionalButton!", "InstructionalButtons now give you the ability to dynamically edit, add, remove, customize your buttons, you can even use them outside the menu ~y~without having to run multiple instances of the same scaleform~w~, aren't you happy??");
		scaleformMenu.AddItem(showSimplePopup);
		scaleformMenu.AddItem(showPopupButtons);
		scaleformMenu.AddItem(customInstr);
		scaleformMenu.AddItem(customInstr2);

		scaleformMenu.OnItemSelect += async (sender, item, index) =>
		{
			if(item == showSimplePopup)
			{
				NativeUIScaleform.Warning.ShowWarning("This is the title", "This is the subtitle", "This is the prompt.. you have 6 seconds left", "This is the error message, NativeUI Ver. 3.0");
				await Delay(1000);
				for (int i=5; i > -1; i--)
				{
					NativeUIScaleform.Warning.UpdateWarning("This is the title", "This is the subtitle", $"This is the prompt.. you have {i} seconds left", "This is the error message, NativeUI Ver. 3.0");
					await Delay(1000);
				}
				NativeUIScaleform.Warning.Dispose();
			}
			else if (item == showPopupButtons)
			{
				List<InstructionalButton> buttons = new List<InstructionalButton>()
				{
					new InstructionalButton(Control.FrontendAccept, "Accept only with Keyboard", PadCheck.Keyboard),
					new InstructionalButton(Control.FrontendY, "Cancel only with GamePad", PadCheck.Controller),
					new InstructionalButton(Control.FrontendX, Control.Detonate, "This will change button if you're using gamepad or keyboard"),
					new InstructionalButton(new List<Control> { Control.MoveUpOnly, Control.MoveLeftOnly , Control.MoveDownOnly , Control.MoveRightOnly }, "Woow multiple buttons at once??")
				};
				NativeUIScaleform.Warning.ShowWarningWithButtons("This is the title", "This is the subtitle", "This is the prompt, press any button", buttons, "This is the error message, NativeUI Ver. 3.0");
				NativeUIScaleform.Warning.OnButtonPressed += (button) =>
				{
					Debug.WriteLine($"You pressed a Button => {button.Text}");
				};
			}
			else if (item == customInstr2)
			{
				if (NativeUIScaleform.InstructionalButtons.ControlButtons.Count >= 6) return;
				NativeUIScaleform.InstructionalButtons.AddInstructionalButton(new InstructionalButton((Control)new Random().Next(0, 250), "I'm a new button look at me!"));
			}
		};

		customInstr.OnListSelected += (item, index) =>
		{
			if (NativeUIScaleform.InstructionalButtons.IsSaving) return;
			NativeUIScaleform.InstructionalButtons.AddSavingText((LoadingSpinnerType)(index + 1), "I'm a saving text", 3000);
		};
	}
	public void NotificationShowdown(UIMenu menu)
	{
		UIMenu notifications = _menuPool.AddSubMenu(menu, "Notifications Showdown");
		var colors = Enum.GetNames(typeof(NotificationColor)).ToList<dynamic>();
		colors.Add("Classic");
		var char_sprites = new List<dynamic>() { "Abigail", "Amanda", "Ammunation", "Andreas", "Antonia", "Ashley", "BankOfLiberty", "BankFleeca", "BankMaze", "Barry", "Beverly", "BikeSite", "BlankEntry", "Blimp", "Blocked", "BoatSite", "BrokenDownGirl", "BugStars", "Call911", "LegendaryMotorsport", "SSASuperAutos", "Castro", "ChatCall", "Chef", "Cheng", "ChengSenior", "Chop", "Cris", "Dave", "Default", "Denise", "DetonateBomb", "DetonatePhone", "Devin", "SubMarine", "Dom", "DomesticGirl", "Dreyfuss", "DrFriedlander", "Epsilon", "EstateAgent", "Facebook", "FilmNoire", "Floyd", "Franklin", "FranklinTrevor", "GayMilitary", "Hao", "HitcherGirl", "Hunter", "Jimmy", "JimmyBoston", "Joe", "Josef", "Josh", "LamarDog", "Lester", "Skull", "LesterFranklin", "LesterMichael", "LifeInvader", "LsCustoms", "LSTI", "Manuel", "Marnie", "Martin", "MaryAnn", "Maude", "Mechanic", "Michael", "MichaelFranklin", "MichaelTrevor", "WarStock", "Minotaur", "Molly", "MorsMutual", "ArmyContact", "Brucie", "FibContact", "RockStarLogo", "Gerald", "Julio", "MechanicChinese", "MerryWeather", "Unicorn", "Mom", "MrsThornhill", "PatriciaTrevor", "PegasusDelivery", "ElitasTravel", "Sasquatch", "Simeon", "SocialClub", "Solomon", "Taxi", "Trevor", "YouTube", "Wade" };

		var noti1 = new UIMenuListItem("Simple Notification", colors, colors.Count - 1, "Can be colored too! Change color and / or select this item to show the notification.");
		var noti2 = new UIMenuListItem("Advanced Notification", char_sprites, 0, "Change the char and see the notification example! (It can be colored too like the simple notification)");
		var noti3 = new UIMenuItem("Help Notification", "Insert your text and see the example, you can insert buttons using ~INPUT_NAME~ (for example try using ~INPUT_CONTEXT~ and you'll get the E button)");
		var noti4 = new UIMenuItem("Floating Help Notification", "This is tricky, it's a 3D notification, you'll have to input a Vector3 to show it!");
		var noti5 = new UIMenuItem("Stats Notification", "This is the notification you see in GTA:O when you improve one of your skills.");
		var noti6 = new UIMenuItem("VS Notification", "This is the notification you see in GTA:O when you kill someone or get revenge.");
		var noti7 = new UIMenuItem("3D Text", "This is known a lot.. let's you draw a 3D text in a precise world coordinates.");
		var noti8 = new UIMenuItem("Simple Text", "This will let you draw a 2D text on screen, you'll have to input the 2D  (X, Y) coordinates.");
		notifications.AddItem(noti1);
		notifications.AddItem(noti2);
		notifications.AddItem(noti3);
		notifications.AddItem(noti4);
		notifications.AddItem(noti5);
		notifications.AddItem(noti6);
		notifications.AddItem(noti7);
		notifications.AddItem(noti8);

		NativeUI.NativeUINotification notification = null;
		notifications.OnListChange += (_menu, _item, _index) =>
		{
			if(_item == noti1)
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
				notification = Notifications.ShowAdvancedNotification("This is the title!!", "This is the subtitle!", "This is the main text!!", selectedChar, selectedChar, HudColor.NONE, Colors.AliceBlue, true, NotificationType.Default, true, true);
			}
		};

		notifications.OnItemSelect += async (_menu, _item, _index) =>
		{
			API.AddTextEntry("FMMC_KEY_TIP8", "Insert text (Max 50 chars):");
			string text = await Game.GetUserInput("", 50); // i set max 50 chars here as example but it can be way more!
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
				Notifications.ShowStatNotification(75, 50, text, true, true);
			}
			else if (_item == noti6)
			{
				Notifications.ShowVSNotification(Game.PlayerPed, HudColor.HUD_COLOUR_BLUE, HudColor.HUD_COLOUR_RED);
				// you must specify 1 or 2 peds for this.. in this case i use the player ped twice for the sake of the example.
			}
			else if (_item == noti7)
			{
				_text = text;
				_timer = Game.GameTime+1;
				Tick += Text3DTimer;
			}
			else if (_item == noti8)
			{
				_text = text;
				_timer = Game.GameTime + 1;
				Tick += TextTimer;
			}
		};
	}

	private int _timer = 0;
	private string _text = string.Empty;
	public async Task Text3DTimer()
	{
		Notifications.DrawText3D(_text, Game.PlayerPed.Bones[Bone.SKEL_Head].Position + new Vector3(0, 0, 0.5f), Colors.WhiteSmoke);
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
		Notifications.ShowFloatingHelpNotification(_text, Game.PlayerPed.Bones[Bone.SKEL_Head].Position + new Vector3(0, 0, 0.5f), 5000);
		// this will show the 3d notification on the head of the ped in 3d world coords
		if (Game.GameTime - _timer > 5000) // this is a tricky yet simple way to count time without using Delay and pausing the Thread ;)
			Tick -= FloatingHelpTimer;
		await Task.FromResult(0);
	}

	public void AddMenuCook(UIMenu menu)
	{
		var newitem = new UIMenuItem("Cook!", "Cook the dish with the appropiate ingredients and ketchup.");
		menu.AddItem(newitem);
		newitem.SetLeftBadge(BadgeIcon.STAR);
		newitem.SetRightBadge(BadgeIcon.TICK);
		menu.OnItemSelect += (sender, item, index) =>
		{
			if (item == newitem)
			{
				string output = ketchup ? "You have ordered ~b~{0}~w~ ~r~with~w~ ketchup." : "You have ordered ~b~{0}~w~ ~r~without~w~ ketchup.";
				Screen.ShowSubtitle(String.Format(output, dish));
			}
		};

		menu.OnIndexChange += (sender, index) =>
		{
			if (sender.MenuItems[index] == newitem)
				newitem.SetLeftBadge(BadgeIcon.NONE);
		};

		var colorItem = new UIMenuItem("UIMenuItem with Colors", "~b~Look!!~r~I can be colored ~y~too!!~w~", HudColor.HUD_COLOUR_PURPLE, HudColor.HUD_COLOUR_PINK);
		menu.AddItem(colorItem);

		var foods = new List<dynamic>
		{
			"Banana",
			"Apple",
			"Pizza",
			"Quartilicious",
			0xF00D, // Dynamic!
        };

		var BlankItem = new UIMenuSeparatorItem();
		menu.AddItem(BlankItem);

		var colorListItem = new UIMenuListItem("Colored ListItem.. Really?", foods, 0, "~b~Look!!~r~I can be colored ~y~too!!~w~", HudColor.HUD_COLOUR_PURPLE, HudColor.HUD_COLOUR_PINK);
		menu.AddItem(colorListItem);

		var Slider = new UIMenuSliderItem("Slider Item", "Cool!", true); // by default max is 100 and multipler 5 = 20 steps.
		menu.AddItem(Slider);
		Slider.OnSliderChanged += (item, index) =>
		{
			Screen.ShowSubtitle($"Slider changed => {index}");
		};
		var Progress = new UIMenuProgressItem("Slider Progress Item", 10, 0);
		menu.AddItem(Progress);
		Progress.OnSliderChanged += (item, index) =>
		{
			Screen.ShowSubtitle($"Progress changed => {index}");
		};

		var listPanelItem1 = new UIMenuItem("Change Color","It can be whatever item you want it to be");
		var ColorPanel = new UIMenuColorPanel("Color Panel Example", UIMenuColorPanel.ColorPanelType.Hair);
		// you can choose between hair palette or makeup palette
		menu.AddItem(listPanelItem1);
		listPanelItem1.AddPanel(ColorPanel);

		var listPanelItem2 = new UIMenuItem("Change Percentage", "It can be whatever item you want it to be");
		var PercentagePanel = new UIMenuPercentagePanel("Percentage Panel Example", "0%", "100%");
		// You can change every text in this Panel
		menu.AddItem(listPanelItem2);
		listPanelItem2.AddPanel(PercentagePanel);

		var listPanelItem3 = new UIMenuItem("Change Grid Position", "It can be whatever item you want it to be");
		var GridPanel = new UIMenuGridPanel("Up", "Left", "Right", "Down", new System.Drawing.PointF(.5f, .5f));
		var HorizontalGridPanel = new UIMenuGridPanel("Left", "Right", new System.Drawing.PointF(.5f, .5f));
		// you can choose the text in every position and where to place the starting position of the cirlce
		menu.AddItem(listPanelItem3);
		listPanelItem3.AddPanel(GridPanel);
		listPanelItem3.AddPanel(HorizontalGridPanel);

		var listPanelItem4 = new UIMenuListItem("Look at Statistics", new List<object> { "Example", "example2" }, 0);
		var statistics = new UIMenuStatisticsPanel();
		menu.AddItem(listPanelItem4);
		listPanelItem4.AddPanel(statistics);
		statistics.AddStatistics("Look at this!", 0);
		statistics.AddStatistics("I'm a statistic too!", 0);
		statistics.AddStatistics("Am i not?!", 0);
		//you can add as menu statistics you want 
		statistics.SetPercentage(0, 10f);
		statistics.SetPercentage(1, 50f);
		statistics.SetPercentage(2, 100f);
		//and you can get / set their percentage


		// THERE ARE NOW EVENT FOR PANELS.. WHEN YOU CHANGE WHAT IS CHANGABLE THE PANEL ITSELF WILL DO WHATEVER YOU TELL HIM TO DO

		ColorPanel.OnColorPanelChange += (item, panel, index) =>
		{
			Notifications.ShowNotification($"ColorPanel index => {index}");
		};
		
		PercentagePanel.OnPercentagePanelChange += (item, panel, index) => {
			Screen.ShowSubtitle("Percentage = " + index + "...");
		};
		
		GridPanel.OnGridPanelChange += (item, panel, value) => {
			Screen.ShowSubtitle("GridPosition = " + value + "...");
		};

		HorizontalGridPanel.OnGridPanelChange += (item, panel, value) => {
			Screen.ShowSubtitle("HorizontalGridPosition = " + value + "...");
		};
	}

	public void AddMenuAnotherMenu(UIMenu menu)
	{
		var submenu = _menuPool.AddSubMenu(menu, "Another Menu");
		for (int i = 0; i < 20; i++)
			submenu.AddItem(new UIMenuItem("PageFiller", "Sample description that takes more than one line. Moreso, it takes way more than two lines since it's so long. Wow, check out this length!"));
	}

	public void HandleMenuEvents(UIMenu menu)
	{
		menu.OnMenuStateChanged += (oldMenu, newMenu, state) =>
		{
			if(state == MenuState.Opened)
			{
				Debug.WriteLine($"{newMenu.Title} just opened!");
			}
			else if (state == MenuState.ChangeForward)
			{
				Debug.WriteLine($"{oldMenu.Title} => {newMenu.Title}");
			}
			else if (state == MenuState.ChangeBackward)
			{
				Debug.WriteLine($"{newMenu.Title} <= {oldMenu.Title}");
			}
			else if (state == MenuState.Closed)
			{
				Debug.WriteLine($"{oldMenu.Title} just closed!");
			}
		};
	}

	public void PauseMenuExample(UIMenu menu)
    {
		UIMenuItem pause = new UIMenuItem("Open Pause Menu");
		menu.AddItem(pause);
		pause.Activated += (a, b) =>
		{
			PauseMenuShowcase(a);
		};
	}

	public void PauseMenuShowcase(UIMenu mainMenu)
    {
		TabView pauseMenu = new TabView("PauseMenu example", "Look there's a subtitle too!", "Detail 1", "Detail 2", "Detail 3");
		_menuPool.AddPauseMenu(pauseMenu);
		TabTextItem basicTab = new TabTextItem("TabTextItem", "This is the title!");
		basicTab.AddItem(new BasicTabItem("~y~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		basicTab.AddItem(new BasicTabItem("~r~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		basicTab.AddItem(new BasicTabItem("~b~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		basicTab.AddItem(new BasicTabItem("~g~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		basicTab.AddItem(new BasicTabItem("~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		basicTab.AddItem(new BasicTabItem("~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		basicTab.AddItem(new BasicTabItem("~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		basicTab.AddItem(new BasicTabItem("~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		basicTab.AddItem(new BasicTabItem("~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		basicTab.AddItem(new BasicTabItem("~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		basicTab.AddItem(new BasicTabItem("~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		basicTab.AddItem(new BasicTabItem("~r~Use the mouse wheel to scroll the text!!"));
		pauseMenu.AddTab(basicTab);

		TabSubmenuItem multiItemTab = new TabSubmenuItem("TabSubMenu");
		pauseMenu.AddTab(multiItemTab);
		TabLeftItem first = new TabLeftItem("1 - Empty", LeftItemType.Empty);
		TabLeftItem second = new TabLeftItem("2 - Info", LeftItemType.Info);
		TabLeftItem third = new TabLeftItem("3 - Statistics", LeftItemType.Statistics);
		TabLeftItem fourth = new TabLeftItem("4 - Settings", LeftItemType.Settings);
		multiItemTab.AddLeftItem(first);
		multiItemTab.AddLeftItem(second);
		multiItemTab.AddLeftItem(third);
		multiItemTab.AddLeftItem(fourth);

		second.TextTitle = "Info Title!!";
		second.AddItem(new BasicTabItem("~y~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		second.AddItem(new BasicTabItem("~r~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		second.AddItem(new BasicTabItem("~b~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		second.AddItem(new BasicTabItem("~g~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		second.AddItem(new BasicTabItem("~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		second.AddItem(new BasicTabItem("~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		second.AddItem(new BasicTabItem("~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		second.AddItem(new BasicTabItem("~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		second.AddItem(new BasicTabItem("~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		second.AddItem(new BasicTabItem("~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		second.AddItem(new BasicTabItem("~p~Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"));
		second.AddItem(new BasicTabItem("~r~Use the mouse wheel to scroll the text!!"));

		StatsTabItem _labelStatItem = new StatsTabItem("Item's Label", "Item's right label");
		StatsTabItem _coloredBarStatItem0 = new StatsTabItem("Item's Label", 0, HudColor.HUD_COLOUR_ORANGE);
		StatsTabItem _coloredBarStatItem1 = new StatsTabItem("Item's Label", 25, HudColor.HUD_COLOUR_RED);
		StatsTabItem _coloredBarStatItem2 = new StatsTabItem("Item's Label", 50, HudColor.HUD_COLOUR_BLUE);
		StatsTabItem _coloredBarStatItem3 = new StatsTabItem("Item's Label", 75, HudColor.HUD_COLOUR_GREEN);
		StatsTabItem _coloredBarStatItem4 = new StatsTabItem("Item's Label", 100, HudColor.HUD_COLOUR_PURPLE);

		third.AddItem(_labelStatItem);
		third.AddItem(_coloredBarStatItem0);
		third.AddItem(_coloredBarStatItem1);
		third.AddItem(_coloredBarStatItem2);
		third.AddItem(_coloredBarStatItem3);
		third.AddItem(_coloredBarStatItem4);

		List<dynamic> itemList = new List<dynamic>() { "This", "Is", "The", "List", "Super", "Power", "Wooow" };
		SettingsTabItem _settings1 = new SettingsTabItem("Item's Label", "Item's right Label");
		SettingsTabItem _settings2 = new SettingsTabItem("Item's Label", itemList, 0);
		SettingsTabItem _settings3 = new SettingsTabItem("Item's Label", 100, 25, false, HudColor.HUD_COLOUR_FREEMODE);
		SettingsTabItem _settings4 = new SettingsTabItem("Item's Label", 100, 75, true, HudColor.HUD_COLOUR_PINK);
		SettingsTabItem _settings5 = new SettingsTabItem("Item's Label", UIMenuCheckboxStyle.Tick, true);
		SettingsTabItem _settings6 = new SettingsTabItem("Item's Label", 100, 50, HudColor.HUD_COLOUR_RED);
		fourth.AddItem(_settings1);
		fourth.AddItem(_settings2);
		fourth.AddItem(_settings3);
		fourth.AddItem(_settings4);
		fourth.AddItem(_settings5);
		fourth.AddItem(_settings6);

		pauseMenu.OnPauseMenuOpen += (menu) =>
		{
			Screen.ShowSubtitle(menu.Title + " Opened!");
			mainMenu.Visible = false;
		};
		pauseMenu.OnPauseMenuClose += (menu) =>
		{
			Screen.ShowSubtitle(menu.Title + " Closed!");
			mainMenu.Visible = true;
		};

		pauseMenu.OnPauseMenuTabChanged += (menu, tab, tabIndex) =>
		{
			Screen.ShowSubtitle(tab.Title + " Selected!");
		};

		pauseMenu.OnPauseMenuFocusChanged += (menu, tab, focusLevel) =>
		{
			Screen.ShowSubtitle(tab.Title + " Focus at level => ~y~"+ focusLevel +"~w~!");
			if(focusLevel == 1)
            {
				if(tab is TabTextItem)
                {
					List<InstructionalButton> buttons = new List<InstructionalButton>()
					{
						new InstructionalButton(Control.PhoneCancel, Game.GetGXTEntry("HUD_INPUT3")),
						new InstructionalButton(Control.LookUpDown, "Scroll text", PadCheck.Controller),
						new InstructionalButton(InputGroup.INPUTGROUP_CURSOR_SCROLL, "Scroll text", PadCheck.Keyboard)
					
					};

					NativeUIScaleform.InstructionalButtons.SetInstructionalButtons(buttons);
                }
            }
		};

		pauseMenu.OnLeftItemChange += (menu, tabIndex, focusLevel, leftItemIndex) =>
		{
			Screen.ShowSubtitle(menu.Tabs[tabIndex].Title + " Focus at level => ~y~" + focusLevel + "~w~, and left Item ~o~N° " + (leftItemIndex+1) + "~w~ selected!");
		};

		pauseMenu.OnRightItemChange += (menu, tabIndex, focusLevel, leftItemIndex, rightItemIndex) =>
		{
			Screen.ShowSubtitle(menu.Tabs[tabIndex].Title + " Focus at level => ~y~" + focusLevel + "~w~, left Item ~o~N° " + (leftItemIndex + 1) + "~w~ and right Item ~b~N° " + (rightItemIndex+1) + "~w~ selected!");
		};
		pauseMenu.Visible = true;
	}

	public MenuExample()
	{
		_menuPool = new MenuPool();
		var mainMenu = new UIMenu("Native UI", "NATIVEUI SHOWCASE", new PointF(50, 50), true); // true means add menu Glare scaleform to the menu
		_menuPool.Add(mainMenu);
		HeritageMenu(mainMenu);
		AddScaleformMenu(mainMenu);
		NotificationShowdown(mainMenu);
		AddMenuKetchup(mainMenu);
		AddMenuFoods(mainMenu);
		AddMenuCook(mainMenu);
		HandleMenuEvents(mainMenu);
		AddMenuAnotherMenu(mainMenu);
		PauseMenuExample(mainMenu);
		_menuPool.RefreshIndex();


		// We create a marker on the peds position, adds it to the MarkerHandler
		Marker playerMarker = new Marker(MarkerType.VerticalCylinder, Game.PlayerPed.Position, new Vector3(1.5f), 5f, Colors.Cyan, true);
		MarkersHandler.AddMarker(playerMarker);

		Tick += async () =>
		{
			_menuPool.ProcessMenus();

			//If the player is in drawing range for the marker, the marker will draw automatically and the DrawText will show itself (true if the ped enters the marker)
			if(playerMarker.IsInRange)
				Notifications.DrawText($"IsInMarker => {playerMarker.IsInMarker}");

			if (Game.IsControlJustPressed(0, Control.SelectCharacterMichael)) // Our menu on/off switch
				//ExampleMenu();
				mainMenu.Visible = !mainMenu.Visible;
			await Task.FromResult(0);
		};
	}
}
