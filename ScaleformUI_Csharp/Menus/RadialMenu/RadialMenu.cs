using CitizenFX.Core;
using CitizenFX.FiveM;
using CitizenFX.FiveM.GUI;
using ScaleformUI.Elements;
using ScaleformUI.Menu;
using ScaleformUI.Menus;
using ScaleformUI.Scaleforms;
using System.Drawing;
using static CitizenFX.FiveM.Native.Natives;

namespace ScaleformUI.Radial
{
    public delegate void MenuOpenedEvent(RadialMenu menu, dynamic data = null);
    public delegate void MenuClosedEvent(RadialMenu menu);
    public delegate void SegmentChanged(RadialSegment segment);
    public delegate void IndexChanged(RadialSegment segment, int index);
    public delegate void SegmentSelected(RadialSegment segment);

    public class RadialMenu : MenuBase
    {
        private bool visible;
        private int currentSelection;
        private float oldAngle;
        private bool changed;
        public RadialSegment[] Segments { get; private set; }
        private PointF _offset = new PointF(0, 0);

        public MenuOpenedEvent OnMenuOpen;
        public MenuClosedEvent OnMenuClose;
        public SegmentChanged OnSegmentHighlight;
        public IndexChanged OnSegmentIndexChange;
        public SegmentSelected OnSegmentSelect;
        private bool enable3D = true;

        public bool Enable3D
        {
            get => enable3D;
            set
            {
                enable3D = value;
                if (Visible)
                {
                    Main.radialMenu.CallFunction("ENABLE_3D", value);
                }
            }
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
                    Main.radialMenu.CallFunction("CLEAR_ALL");
                    MenuCloseEv(this);
                }
            }
        }

        public RadialMenu() : this(default) { }

        public RadialMenu(PointF offset)
        {
            _offset = offset;
            Segments = new RadialSegment[8]
            {
                new RadialSegment(0, this),
                new RadialSegment(1, this),
                new RadialSegment(2, this),
                new RadialSegment(3, this),
                new RadialSegment(4, this),
                new RadialSegment(5, this),
                new RadialSegment(6, this),
                new RadialSegment(7, this),
            };
            InstructionalButtons = new List<InstructionalButton>()
            {
                new InstructionalButton(Control.PhoneSelect, UIMenu._selectTextLocalized),
                new InstructionalButton(Control.PhoneCancel, UIMenu._backTextLocalized)
            };
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

        internal async void BuildMenu()
        {
            if (_offset.IsEmpty) _offset = new PointF(Screen.Width / 2, (Screen.Height / 2) - 60);
            Main.radialMenu.CallFunction("CREATE_MENU", enable3D, _offset.X, _offset.Y);
            for (int i = 0; i < 8; i++)
            {
                RadialSegment segment = Segments[i];
                for (int j = 0; j < segment.Items.Count; j++)
                {
                    SegmentItem item = segment.Items[j];
                    Main.radialMenu.CallFunction("ADD_ITEM", i, item.Label, item.Description, item.TextureDict, item.TextureName, item.TextureWidth, item.TextureHeight, item.Color, item.qtty, item.max);
                }
            }
            Main.radialMenu.CallFunction("LOAD_MENU", currentSelection, Segments[0].CurrentSelection, Segments[1].CurrentSelection, Segments[2].CurrentSelection, Segments[3].CurrentSelection, Segments[4].CurrentSelection, Segments[5].CurrentSelection, Segments[6].CurrentSelection, Segments[7].CurrentSelection);
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

                // then we get its value between 0-7.. or -1 if nothing has been pressed.
                finalizedWorkingAngle = (int)Math.Floor(angle != 0 ? normalized_angle / 45 : -1);

                if (currentSelection != finalizedWorkingAngle && finalizedWorkingAngle != -1 && (normalized_angle > oldAngle + 45 || normalized_angle < oldAngle - 45))
                {
                    if (!changed)
                    {
                        Segments[currentSelection].Selected = false;
                        currentSelection = finalizedWorkingAngle;
                        oldAngle = normalized_angle;
                        changed = true;
                    }
                }

                if (changed)
                {
                    Segments[currentSelection].Selected = true;
                    OnSegmentHighlight?.Invoke(Segments[currentSelection]);
                    Main.radialMenu.CallFunction("SET_POINTER", finalizedWorkingAngle, true);
                    changed = false;
                }
            }

            if (Game.IsControlJustPressed(0, Control.WeaponWheelPrev))
            {
                int sel = await Segments[currentSelection].CycleItems(-1);
                OnSegmentIndexChange?.Invoke(Segments[currentSelection], sel);
            }

            if (Game.IsControlJustPressed(0, Control.WeaponWheelNext))
            {
                int sel = await Segments[currentSelection].CycleItems(1);
                OnSegmentIndexChange?.Invoke(Segments[currentSelection], sel);
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

        internal void GoBack()
        {
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

        internal void Select()
        {
            OnSegmentSelect?.Invoke(Segments[currentSelection]);
        }

        internal override void Draw()
        {
            Screen.Hud.HideComponentThisFrame(HudComponent.WeaponWheel);
            Main.radialMenu.Render2D();
        }

        public int CurrentSegment
        {
            get => currentSelection;
            set
            {
                currentSelection = value;
                if (Visible)
                {
                    Main.radialMenu.CallFunction("SET_POINTER", value, true);
                }
            }
        }

        internal void MenuOpenEv(RadialMenu menu, dynamic data)
        {
            OnMenuOpen?.Invoke(menu, data);
        }

        internal void MenuCloseEv(RadialMenu menu)
        {
            OnMenuClose?.Invoke(menu);
        }
    }
}
