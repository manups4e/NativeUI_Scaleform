using CitizenFX.Core;
using CitizenFX.FiveM.GUI;
using ScaleformUI.Elements;
using System.Drawing;
using Font = CitizenFX.FiveM.GUI.Font;

namespace ScaleformUI
{
    public class TextTimerBar : TimerBarBase
    {
        public string Caption { get; set; }
        public SColor CaptionColor { get; set; } = SColor.White;
        public Font CaptionFont { get; set; } = Font.ChaletLondon;
        public bool Enabled;
        public TextTimerBar(string label, string text) : base(label)
        {
            Caption = text;
        }
        public TextTimerBar(string label, string text, Font labelFont) : base(label, labelFont)
        {
            Caption = text;
        }
        public TextTimerBar(string label, string text, Font labelFont, Font captionFont) : base(label, labelFont)
        {
            Caption = text;
            CaptionFont = captionFont;
        }
        public TextTimerBar(string label, string text, System.Drawing.Color captionColor) : base(label)
        {
            Caption = text;
            CaptionColor = SColor.FromColor(captionColor);
        }
        public TextTimerBar(string label, string text, System.Drawing.Color captionColor, Font labelFont) : base(label, labelFont)
        {
            Caption = text;
            CaptionColor = SColor.FromColor(captionColor);
        }
        public TextTimerBar(string label, string text, System.Drawing.Color captionColor, Font labelFont, Font captionFont) : base(label, labelFont)
        {
            Caption = text;
            CaptionColor = SColor.FromColor(captionColor);
            CaptionFont = captionFont;
        }

        public override void Draw(int interval)
        {
            if (!Enabled) return;
            SizeF res = ScreenTools.ResolutionMaintainRatio;
            PointF safe = ScreenTools.SafezoneBounds;

            base.Draw(interval);
            new UIResText(Caption, new Vector2((int)res.Width - safe.X - 10, (int)res.Height - safe.Y - (42 + (4 * interval))), 0.5f, new CitizenFX.Core.Color(CaptionColor.ToColor().ToArgb()), LabelFont, Alignment.Right).Draw();
        }
    }
}
