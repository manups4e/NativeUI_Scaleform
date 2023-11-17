
using CitizenFX.Core;
using ScaleformUI.Elements;
using System.Drawing;
using Font = CitizenFX.FiveM.GUI.Font;

namespace ScaleformUI
{
    public class ProgressTimerBar : TimerBarBase
    {
        /// <summary>
        /// Bar percentage. Goes from 0 to 1.
        /// </summary>
        public float Percentage { get; set; }

        public SColor BackgroundColor { get; set; }
        public SColor ForegroundColor { get; set; }

        private UIResRectangle _background;
        private UIResRectangle _foreground;

        public ProgressTimerBar(string label) : base(label)
        {
            BackgroundColor = SColor.HUD_Reddark;
            ForegroundColor = SColor.HUD_Red;
            _background = new UIResRectangle(new Vector2(0, 0), new Vector2(150, 15), new CitizenFX.Core.Color(BackgroundColor.ToColor().ToArgb()));
            _foreground = new UIResRectangle(new Vector2(0, 0), new Vector2(0, 15), new CitizenFX.Core.Color(BackgroundColor.ToColor().ToArgb()));
        }

        public ProgressTimerBar(string label, Font labelFont) : base(label, labelFont)
        {
            BackgroundColor = SColor.HUD_Reddark;
            ForegroundColor = SColor.HUD_Red;
            _background = new UIResRectangle(new Vector2(0, 0), new Vector2(150, 15), new CitizenFX.Core.Color(BackgroundColor.ToColor().ToArgb()));
            _foreground = new UIResRectangle(new Vector2(0, 0), new Vector2(0, 15), new CitizenFX.Core.Color(BackgroundColor.ToColor().ToArgb()));
        }

        public ProgressTimerBar(string label, System.Drawing.Color background, System.Drawing.Color foreground) : base(label)
        {
            BackgroundColor = SColor.FromColor(background);
            ForegroundColor = SColor.FromColor(foreground);
            _background = new UIResRectangle(new Vector2(0, 0), new Vector2(150, 15), new CitizenFX.Core.Color(BackgroundColor.ToColor().ToArgb()));
            _foreground = new UIResRectangle(new Vector2(0, 0), new Vector2(0, 15), new CitizenFX.Core.Color(ForegroundColor.ToColor().ToArgb()));
        }

        public ProgressTimerBar(string label, System.Drawing.Color background, System.Drawing.Color foreground, Font labelFont) : base(label, labelFont)
        {
            BackgroundColor = SColor.FromColor(background);
            ForegroundColor = SColor.FromColor(foreground);
            _background = new UIResRectangle(new Vector2(0, 0), new Vector2(150, 15), new CitizenFX.Core.Color(BackgroundColor.ToColor().ToArgb()));
            _foreground = new UIResRectangle(new Vector2(0, 0), new Vector2(0, 15), new CitizenFX.Core.Color(BackgroundColor.ToColor().ToArgb()));
        }

        public override void Draw(int interval)
        {
            SizeF res = ScreenTools.ResolutionMaintainRatio;
            PointF safe = ScreenTools.SafezoneBounds;

            base.Draw(interval);

            Vector2 start = new Vector2((int)res.Width - safe.X - 160, (int)res.Height - safe.Y - (28 + (4 * interval)));

            _background.Position = start;
            _foreground.Position = start;

            _foreground.Size = new Vector2(150 * Percentage, 15);

            // In case someone decides to change colors while drawing..
            _background.Color = new CitizenFX.Core.Color(BackgroundColor.ToColor().ToArgb());
            _foreground.Color = new CitizenFX.Core.Color(BackgroundColor.ToColor().ToArgb());

            _background.Draw();
            _foreground.Draw();
        }
    }
}
