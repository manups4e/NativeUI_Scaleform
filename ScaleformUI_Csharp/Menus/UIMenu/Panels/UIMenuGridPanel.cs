using CitizenFX.Core;
using static CitizenFX.FiveM.Native.Natives;
using System.Drawing;

namespace ScaleformUI.Menu
{
    public enum GridType
    {
        Full,
        Horizontal
    }

    public class UIMenuGridPanel : UIMenuPanel
    {
        public string TopLabel { get; set; }
        public string LeftLabel { get; set; }
        public string RightLabel { get; set; }
        public string BottomLabel { get; set; }
        public event GridPanelChangedEvent OnGridPanelChange;
        public GridType GridType = GridType.Full;

        private UIMenuGridAudio Audio;
        internal PointF _value;
        public PointF CirclePosition
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                _setValue(value);
            }
        }

        /// <summary>
        /// Creates <see langword="abstract"/>full grid panels with x,y values
        /// </summary>
        /// <param name="TopText"></param>
        /// <param name="LeftText"></param>
        /// <param name="RightText"></param>
        /// <param name="BottomText"></param>
        /// <param name="circlePosition"></param>
        public UIMenuGridPanel(string TopText, string LeftText, string RightText, string BottomText, PointF circlePosition)
        {
            TopLabel = TopText ?? "Up";
            LeftLabel = LeftText ?? "Left";
            RightLabel = RightText ?? "Right";
            BottomLabel = BottomText ?? "Down";
            GridType = GridType.Full;
            _value = circlePosition;
        }

        /// <summary>
        /// Creates an horizontal Panel with y fixed at 0.5 and variable x
        /// </summary>
        /// <param name="LeftText"></param>
        /// <param name="RightText"></param>
        /// <param name="circlePosition"></param>
        public UIMenuGridPanel(string LeftText, string RightText, PointF circlePosition)
        {
            TopLabel = "";
            BottomLabel = "";
            LeftLabel = LeftText ?? "Left";
            RightLabel = RightText ?? "Right";
            GridType = GridType.Horizontal;
            _value = circlePosition;
        }

        /*
        internal void UpdateParent( float X, float Y)
        {
            ParentItem.Parent.ListChange(ParentItem, ParentItem.Index);
            ParentItem.ListChangedTrigger(ParentItem.Index);
        }*/

        private void _setValue(PointF value)
        {
            int it = ParentItem.Parent.Pagination.GetScaleformIndex(ParentItem.Parent.MenuItems.IndexOf(ParentItem));
            int van = ParentItem.Panels.IndexOf(this);
            Main.scaleformUI.CallFunction("SET_GRID_PANEL_VALUE_RETURN_VALUE", it, van, value.X, value.Y);
        }

        public async void SetMousePosition(PointF mouse)
        {
            int it = ParentItem.Parent.Pagination.GetScaleformIndex(ParentItem.Parent.MenuItems.IndexOf(ParentItem));
            int van = ParentItem.Panels.IndexOf(this);
            BeginScaleformMovieMethod(Main.scaleformUI.Handle, "SET_GRID_PANEL_POSITION_RETURN_VALUE");
            ScaleformMovieMethodAddParamInt(0);
            ScaleformMovieMethodAddParamInt(1);
            ScaleformMovieMethodAddParamFloat(mouse.X);
            ScaleformMovieMethodAddParamFloat(mouse.Y);
            int ret = EndScaleformMovieMethodReturnValue();
            while (!IsScaleformMovieMethodReturnValueReady(ret)) await BaseScript.Delay(0);
            string res = GetScaleformMovieMethodReturnValueString(ret);
            string[] returned = res.Split(',');
            _value = new PointF(Convert.ToSingle(returned[0]), Convert.ToSingle(returned[1]));
        }

        internal void OnGridChange()
        {
            OnGridPanelChange?.Invoke(ParentItem, this, CirclePosition);
        }
    }


    public class UIMenuGridAudio
    {
        public string Slider;
        public string Library;
        public int Id;
        public UIMenuGridAudio(string slider, string library, int id)
        {
            Slider = slider;
            Library = library;
            Id = id;
        }
    }
}
