using CitizenFX.Core;
using static CitizenFX.FiveM.Native.Natives;
using ScaleformUI.Menu;
using ScaleformUI.Menus;
using ScaleformUI.PauseMenus;
using ScaleformUI.Radial;
using ScaleformUI.Radio;
using CitizenFX.FiveM;

namespace ScaleformUI
{
    // TODO: Add ClearHistory() / CloseAndClearBreadcrumbs() method to handle close and reset.

    /// <summary>
    /// Helper class that handles all of your Menus. After instatiating it, you will have to add your menu by using the Add method.
    /// </summary>
    public static class MenuHandler
    {
        internal static MenuBase currentMenu;
        internal static PauseMenuBase currentBase;
        internal static bool ableToDraw;
        private static Ped _ped;
        internal static Ped PlayerPed
        {
            get
            {
                int handle = PlayerPedId();

                if (_ped is null || handle != _ped.Handle)
                {
                    _ped = new Ped(handle);
                }

                return _ped;
            }
        }

        public static MenuBase CurrentMenu
        {
            get => currentMenu;
            private set => currentMenu = value;
        }

        public static PauseMenuBase CurrentPauseMenu
        {
            get => currentBase;
            private set => currentBase = value;
        }

        public static async Task SwitchTo(this MenuBase currentMenu, MenuBase newMenu, int newMenuCurrentSelection = 0, bool inheritOldMenuParams = false, dynamic data = null)
        {
            if (currentMenu == null)
                throw new ArgumentNullException("The menu you're switching from cannot be null.");
            if (currentMenu != MenuHandler.currentMenu)
                throw new Exception("The menu you're switching from must be opened.");
            if (newMenu == null)
                throw new ArgumentNullException("The menu you're switching to cannot be null.");
            if (newMenu == currentMenu)
                throw new Exception("You cannot switch a menu to itself.");
            if (newMenu is UIMenu menu && menu.MenuItems.Count == 0)
                throw new Exception("You cannot switch to an empty menu.");
            if (BreadcrumbsHandler.SwitchInProgress) return;

            BreadcrumbsHandler.SwitchInProgress = true;

            if (currentMenu is UIMenu old)
            {
                await old.FadeOutMenu();
                currentMenu.Visible = false;
                if (newMenu is UIMenu newer)
                {
                    if (inheritOldMenuParams)
                    {
                        if (old._customTexture.Key != null && old._customTexture.Value != null)
                            newer.SetBannerType(old._customTexture);
                        newer.Offset = old.Offset;
                        newer.AlternativeTitle = old.AlternativeTitle;
                        newer.MaxItemsOnScreen = old.MaxItemsOnScreen;
                        newer.AnimationType = old.AnimationType;
                        newer.BuildingAnimation = old.BuildingAnimation;
                        newer.ScrollingType = old.ScrollingType;
                        newer.Glare = old.Glare;
                        newer.EnableAnimation = old.EnableAnimation;
                        newer.Enabled3DAnimations = old.Enabled3DAnimations;
                        newer.fadingTime = old.fadingTime;
                        newer.SetMouse(old.MouseControlsEnabled, old.MouseEdgeEnabled, old.MouseWheelControlEnabled, old.ResetCursorOnOpen, old.leftClickEnabled);
                    }
                    newer.CurrentSelection = newMenuCurrentSelection != 0 ? newMenuCurrentSelection : 0;
                }
                else if (newMenu is RadialMenu rad)
                    rad.CurrentSegment = newMenuCurrentSelection != 0 ? newMenuCurrentSelection : 0;
                else if (newMenu is UIRadioMenu radio)
                    radio.CurrentSelection = newMenuCurrentSelection != 0 ? newMenuCurrentSelection : 0;
            }


            newMenu.Visible = true;
            BreadcrumbsHandler.Forward(newMenu, data);
            BreadcrumbsHandler.SwitchInProgress = false;
        }

        /// <summary>
        /// Processes all of your visible menus' controls.
        /// </summary>
        internal static void ProcessControl()
        {
            if (currentMenu != null && currentBase == null)
                currentMenu?.ProcessControl();
            else
                currentBase?.ProcessControls();
        }


        /// <summary>
        /// Processes all of your visible menus' mouses.
        /// </summary>
        internal static void ProcessMouse()
        {
            if (currentMenu != null && currentBase == null)
                currentMenu?.ProcessMouse();
            else
                currentBase?.ProcessMouse();
        }


        /// <summary>
        /// Draws all visible menus.
        /// </summary>
        internal static void Draw()
        {
            if (Main.Warning.IsShowing || Main.Warning.IsShowingWithButtons) return;
            if (currentMenu != null && currentBase == null)
                currentMenu?.Draw();
            else
                currentBase?.Draw();
        }

        /// <summary>
        /// Checks if any menu is currently visible.
        /// </summary>
        /// <returns>true if one menu is visible, false if not.</returns>
        public static bool IsAnyMenuOpen => currentMenu?.Visible ?? false || BreadcrumbsHandler.Count > 0;

        public static bool IsAnyPauseMenuOpen => currentBase?.Visible ?? false;

        /// <summary>
        /// Process all of your menus' functions. Call this in a tick event.
        /// </summary>
        public static async void ProcessMenus()
        {
            Draw();
            ProcessControl();
            ProcessMouse();
        }

        /// <summary>
        /// Closes any opened ScaleformUI menu or PauseMenu menu.
        /// </summary>
        public static void CloseAndClearHistory()
        {
            ableToDraw = false;
            if (currentMenu != null)
                currentMenu.Visible = false;
            if (currentBase != null)
                currentBase.Visible = false;
            BreadcrumbsHandler.Clear();
            Main.InstructionalButtons.ClearButtonList();
        }
    }
}
