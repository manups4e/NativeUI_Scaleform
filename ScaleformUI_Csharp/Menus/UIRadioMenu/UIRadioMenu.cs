using CitizenFX.Core;
using static CitizenFX.FiveM.Native.Natives;
using CitizenFX.FiveM.GUI;
using ScaleformUI.Elements;
using ScaleformUI.Menu;
using ScaleformUI.Menus;
using ScaleformUI.Scaleforms;
using CitizenFX.FiveM;

namespace ScaleformUI.Radio
{
    public delegate void MenuOpenedEvent(UIRadioMenu menu, dynamic data = null);
    public delegate void MenuClosedEvent(UIRadioMenu menu);
    public delegate void IndexChanged(int index);
    public delegate void StationSelected(RadioItem item, int index);
    public enum AnimationDirection
    {
        ZoomOut = -1,
        ZoomIn = 1,
    }
    public class UIRadioMenu : MenuBase
    {
        private bool visible;
        private bool isAnimating;
        private int currentSelection;
        private float oldAngle;
        private bool changed;
        public event MenuOpenedEvent OnMenuOpen;
        public event MenuClosedEvent OnMenuClose;
        public event IndexChanged OnIndexChange;
        public event StationSelected OnStationSelect;
        public AnimationDirection AnimDirection = AnimationDirection.ZoomIn;
        public float AnimationDuration = 1f;
        public List<RadioItem> Stations { get; private set; }
        public UIRadioMenu()
        {
            Stations = new List<RadioItem>();
            InstructionalButtons = new List<InstructionalButton>()
            {
                new InstructionalButton(Control.PhoneSelect, UIMenu._selectTextLocalized),
                new InstructionalButton(Control.PhoneCancel, UIMenu._backTextLocalized)
            };
        }

        public override bool Visible
        {
            get => visible;
            set
            {
                visible = value;
                if (value)
                {
                    BuildMenu();
                    MenuHandler.currentMenu = this;
                    MenuHandler.ableToDraw = true;
                    Main.InstructionalButtons.SetInstructionalButtons(InstructionalButtons);
                    MenuOpenEv(this, null);
                    if (BreadcrumbsHandler.Count == 0)
                        BreadcrumbsHandler.Forward(this, null);
                }
                else
                {
                    Main.InstructionalButtons.ClearButtonList();
                    MenuHandler.ableToDraw = false;
                    MenuHandler.currentMenu = null;
                    Main.radioMenu.CallFunction("CLEAR_ALL");
                    MenuCloseEv(this);
                }
            }
        }

        public int CurrentSelection
        {
            get => currentSelection;
            set
            {
                currentSelection = value;
                if (Visible)
                {
                    Main.radioMenu.CallFunction("SET_POINTER", value, true);
                }
            }
        }
        public void AddInstructionalButton(InstructionalButton button)
        {
            InstructionalButtons.Add(button);
            if (Visible && !(Main.Warning.IsShowing || Main.Warning.IsShowingWithButtons))
                Main.InstructionalButtons.SetInstructionalButtons(InstructionalButtons);
        }

        public void RemoveInstructionalButton(InstructionalButton button)
        {
            if (InstructionalButtons.Contains(button))
                InstructionalButtons.Remove(button);
            if (Visible && !(Main.Warning.IsShowing || Main.Warning.IsShowingWithButtons))
                Main.InstructionalButtons.SetInstructionalButtons(InstructionalButtons);
        }

        public void RemoveInstructionalButton(int index)
        {
            if (index < 0 || index >= InstructionalButtons.Count)
                throw new ArgumentOutOfRangeException("ScaleformUI: Cannot remove with an index less than 0 or more than the count of actual instructional buttons");
            RemoveInstructionalButton(InstructionalButtons[index]);
        }

        public async void BuildMenu()
        {
            Main.radioMenu.CallFunction("CREATE_MENU", true, 0, 0);
            for (int i = 0; i < Stations.Count; i++)
            {
                RadioItem item = Stations[i];
                /*txd, txn, stationName, artist, track, color*/
                Main.radioMenu.CallFunction("ADD_ITEM", item.TextureDictionary, item.TextureName, item.StationName, item.Artist, item.Track);
            }
            Main.radioMenu.CallFunction("LOAD_MENU");
            await animateIn();
        }

        public void AddStation(RadioItem station)
        {
            station.Parent = this;
            Stations.Add(station);
        }

        internal override async void ProcessControl(Keys key = Keys.None)
        {
            Controls.Toggle(false);
            // block camera movements
            Game.DisableControlThisFrame(0, Control.LookLeftRight);
            Game.DisableControlThisFrame(0, Control.LookUpDown);

            // take mouse/gamepad LStick
            float x = (float)Math.Floor(GetDisabledControlNormal(2, 13) * 1000);
            float y = (float)Math.Floor(GetDisabledControlNormal(2, 12) * 1000);

            // math.atan2 returns 0 when mouse/gamepad LStick move right, because y=0 and x is not negative..
            // as a workaround.. i set y = 1, this way left is checked correctly
            if (x > 0 && y == 0) y = 1;
            float angle = 0;
            float normalized_angle = 0;
            int finalizedWorkingAngle = -1;

            if (x > 400 || y > 400 || x < -400 || y < -400)
            {

                // we convert controls into angle degrees (rad) and then into -180° -> 180°
                angle = (float)(Math.Atan2(y, x) * (180 / Math.PI));
                // normalize the angle into 0-360° range
                normalized_angle = (float)Math.Floor(angle != 0 ? (angle + 450) % 360 : 0);

                // then we get its value between 0-N.. or -1 if nothing has been pressed.
                float step = 360 / (float)Stations.Count;
                finalizedWorkingAngle = (int)Math.Floor(angle != 0 ? normalized_angle / step : -1);

                if (currentSelection != finalizedWorkingAngle && finalizedWorkingAngle != -1 && (normalized_angle > oldAngle + step || normalized_angle < oldAngle - step))
                {
                    if (!changed)
                    {
                        Stations[currentSelection].Selected = false;
                        currentSelection = finalizedWorkingAngle;
                        oldAngle = normalized_angle;
                        changed = true;
                    }
                }

                if (changed)
                {
                    Stations[currentSelection].Selected = true;
                    OnIndexChange?.Invoke(currentSelection);
                    Main.radioMenu.CallFunction("SET_POINTER", finalizedWorkingAngle, true);
                    changed = false;
                }
            }

            if (Game.IsControlJustPressed(0, Control.FrontendCancel))
            {
                GoBack();
            }

            if (Game.IsControlJustPressed(0, Control.FrontendAccept))
            {
                Select();
            }
        }
        internal async void GoBack()
        {
            await animateOut();
            if (BreadcrumbsHandler.CurrentDepth == 0)
            {
                Visible = false;
                BreadcrumbsHandler.Clear();
                Main.InstructionalButtons.ClearButtonList();
            }
            else
            {
                BreadcrumbsHandler.SwitchInProgress = true;
                MenuBase prevMenu = BreadcrumbsHandler.PreviousMenu;
                BreadcrumbsHandler.Backwards();
                Visible = false;
                prevMenu.Visible = true;
                BreadcrumbsHandler.SwitchInProgress = false;
            }
        }

        private async Task animateIn()
        {
            Main.radioMenu.CallFunction("ANIMATE_IN", AnimationDuration, (int)AnimDirection, "zoom");
            do
            {
                await BaseScript.Delay(0);
                isAnimating = await Main.radioMenu.CallFunctionReturnValueBool("GET_IS_ANIMATING");
            } while (isAnimating);
        }
        private async Task animateOut()
        {
            Main.radioMenu.CallFunction("ANIMATE_OUT", AnimationDuration, (int)AnimDirection, "zoom");
            do
            {
                await BaseScript.Delay(0);
                isAnimating = await Main.radioMenu.CallFunctionReturnValueBool("GET_IS_ANIMATING");
            } while (isAnimating);
        }

        internal void Select()
        {
            OnStationSelect?.Invoke(Stations[currentSelection], currentSelection);
        }

        internal override void Draw()
        {
            Screen.Hud.HideComponentThisFrame(HudComponent.WeaponWheel);
            Main.radioMenu.Render2D();
        }

        internal void MenuOpenEv(UIRadioMenu menu, dynamic data)
        {
            OnMenuOpen?.Invoke(menu, data);
        }

        internal void MenuCloseEv(UIRadioMenu menu)
        {
            OnMenuClose?.Invoke(menu);
        }

    }
}
