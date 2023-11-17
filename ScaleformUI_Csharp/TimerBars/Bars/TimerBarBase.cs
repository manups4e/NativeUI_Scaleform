using CitizenFX.Core;
using CitizenFX.FiveM.GUI;
using ScaleformUI.Elements;
using System.Drawing;
using Font = CitizenFX.FiveM.GUI.Font;

namespace ScaleformUI
{
    public abstract class TimerBarBase
    {
        public string Label { get; set; }
        public Font LabelFont { get; set; }

        public TimerBarBase(string label, Font font = Font.ChaletLondon)
        {
            Label = label;
            LabelFont = font;
        }

        public virtual void Draw(int interval)
        {
            SizeF res = ScreenTools.ResolutionMaintainRatio;
            PointF safe = ScreenTools.SafezoneBounds;
            new UIResText(Label, new Vector2((int)res.Width - safe.X - 180, (int)res.Height - safe.Y - (30 + (4 * interval))), 0.3f, new CitizenFX.Core.Color(SColor.White.ToColor().ToArgb()), LabelFont, Alignment.Right).Draw();

            new Sprite("timerbars", "all_black_bg", new PointF((int)res.Width - safe.X - 298, (int)res.Height - safe.Y - (40 + (4 * interval))), new SizeF(300, 37), 0f, System.Drawing.Color.FromArgb(180, 255, 255, 255)).Draw();

            Screen.Hud.HideComponentThisFrame(HudComponent.AreaName);
            Screen.Hud.HideComponentThisFrame(HudComponent.StreetName);
            Screen.Hud.HideComponentThisFrame(HudComponent.VehicleName);
        }
    }
}
