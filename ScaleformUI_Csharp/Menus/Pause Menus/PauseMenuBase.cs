using static CitizenFX.FiveM.Native.Natives;
using ScaleformUI.Scaleforms;

namespace ScaleformUI.PauseMenus
{
    public class PauseMenuBase
    {
        public virtual bool Visible
        {
            get => visible;
            set
            {
                visible = value;
                MenuHandler.ableToDraw = value;
            }
        }
        /// <summary>
        /// Players won't be able to close the menu if this is false! Make sure players can close the menu in some way!!!!!!
        /// </summary>
        public bool CanPlayerCloseMenu = true;
        private bool visible;

        public List<InstructionalButton> InstructionalButtons { get; set; }

        public virtual void ProcessControls()
        {

        }
        public virtual void ProcessMouse()
        {

        }
        public virtual void Draw()
        {
            DisableControlAction(0, 199, true);
            DisableControlAction(0, 200, true);
            DisableControlAction(1, 199, true);
            DisableControlAction(1, 200, true);
            DisableControlAction(2, 199, true);
            DisableControlAction(2, 200, true);
        }
    }
}
