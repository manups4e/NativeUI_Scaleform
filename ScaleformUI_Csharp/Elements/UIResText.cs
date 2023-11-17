using CitizenFX.Core.Native;
using CitizenFX.FiveM.GUI;
using System.Drawing;
using Font = CitizenFX.FiveM.GUI.Font;
using static CitizenFX.FiveM.Native.Natives;
using CitizenFX.Core;

namespace ScaleformUI.Elements
{
    /// <summary>
    /// A Text object in the 1080 pixels height base system.
    /// </summary>
    public class UIResText : Text
    {
        public UIResText(string caption, Vector2 position, float scale) : base(caption, position, scale)
        {
            TextAlignment = Alignment.Left;
        }

        public UIResText(string caption, Vector2 position, float scale, CitizenFX.Core.Color color)
            : base(caption, position, scale, color)
        {
            TextAlignment = Alignment.Left;
        }

        public UIResText(string caption, Vector2 position, float scale, CitizenFX.Core.Color color, Font font, Alignment justify)
            : base(caption, position, scale, color, font, CitizenFX.FiveM.GUI.Alignment.Left)
        {
            TextAlignment = justify;
        }


        public Alignment TextAlignment { get; set; }

        /// <summary>
        /// Push a long string into the stack.
        /// </summary>
        /// <param name="str"></param>
        public static void AddLongString(string str)
        {
            int utf8ByteCount = System.Text.Encoding.UTF8.GetByteCount(str);

            if (utf8ByteCount == str.Length)
            {
                AddLongStringForAscii(str);
            }
            else
            {
                AddLongStringForUtf8(str);
            }
        }

        private static void AddLongStringForAscii(string input)
        {
            const int maxByteLengthPerString = 99;

            for (int i = 0; i < input.Length; i += maxByteLengthPerString)
            {
                string substr = (input.Substring(i, Math.Min(maxByteLengthPerString, input.Length - i)));
                AddTextComponentString(substr);
            }
        }

        internal static void AddLongStringForUtf8(string input)
        {
            const int maxByteLengthPerString = 99;

            if (maxByteLengthPerString < 0)
            {
                throw new ArgumentOutOfRangeException("maxLengthPerString");
            }
            if (string.IsNullOrEmpty(input) || maxByteLengthPerString == 0)
            {
                return;
            }

            System.Text.Encoding enc = System.Text.Encoding.UTF8;

            int utf8ByteCount = enc.GetByteCount(input);
            if (utf8ByteCount < maxByteLengthPerString)
            {
                AddTextComponentString(input);
                return;
            }

            int startIndex = 0;

            for (int i = 0; i < input.Length; i++)
            {
                int length = i - startIndex;
                if (enc.GetByteCount(input.Substring(startIndex, length)) > maxByteLengthPerString)
                {
                    string substr = (input.Substring(startIndex, length - 1));
                    AddTextComponentString(substr);

                    i -= 1;
                    startIndex = (startIndex + length - 1);
                }
            }
            AddTextComponentString(input.Substring(startIndex, input.Length - startIndex));
        }

        [Obsolete("Use ScreenTools.GetTextWidth instead.", true)]
        public static float MeasureStringWidth(string str, Font font, float scale) => ScreenTools.GetTextWidth(str, font, scale);

        [Obsolete("Use ScreenTools.GetTextWidth instead.", true)]
        public static float MeasureStringWidthNoConvert(string str, Font font, float scale) => ScreenTools.GetTextWidth(str, font, scale);

        /// <summary>
        /// Width of the text wrap box. Set to zero to disable.
        /// </summary>
        public float Wrap { get; set; } = 0;
        /// <summary>
        /// Size of the text wrap box.
        /// </summary>
        [Obsolete("Use UIResText.Wrap instead.", true)]
        public SizeF WordWrap
        {
            get => new SizeF(Wrap, 0);
            set => Wrap = value.Width;
        }

        public void Draw(SizeF offset)
        {
            int screenw = Screen.Resolution.Width;
            int screenh = Screen.Resolution.Height;
            const float height = 1080f;
            float ratio = (float)screenw / screenh;
            float width = height * ratio;

            float x = (Position.X) / width;
            float y = (Position.Y) / height;

            SetTextFont((int)Font);
            SetTextScale(1.0f, Scale);
            SetTextColour(Color.R, Color.G, Color.B, Color.A);
            if (Shadow)
                SetTextDropShadow();
            if (Outline)
                SetTextOutline();
            switch (TextAlignment)
            {
                case Alignment.Center:
                    SetTextCentre(true);
                    break;
                case Alignment.Right:
                    SetTextRightJustify(true);
                    SetTextWrap(0, x);
                    break;
            }

            if (Wrap != 0)
            {
                float xsize = (Position.X + Wrap) / width;
                SetTextWrap(x, xsize);
            }

            SetTextEntry("jamyfafi");
            AddLongString((string)Caption);

            DrawText(x, y);
        }

        //public static void Draw(string caption, int xPos, int yPos, Font font, float scale, UnknownColors color, Alignment alignment, bool Shadow, bool outline, int wordWrap)
        //{
        //    int screenw = Screen.Resolution.Width;
        //    int screenh = Screen.Resolution.Height;
        //    const float height = 1080f;
        //    float ratio = (float)screenw / screenh;
        //    var width = height * ratio;

        //    float x = (xPos) / width;
        //    float y = (yPos) / height;

        //    Function.Call(Hash.SET_TEXT_FONT, (int)font);
        //    Function.Call(Hash.SET_TEXT_SCALE, 1.0f, scale);
        //    Function.Call(Hash.SET_TEXT_COLOUR, color.R, color.G, color.B, color.A);
        //    if (Shadow)
        //        Function.Call(Hash.SET_TEXT_DROP_SHADOW);
        //    if (outline)
        //        Function.Call(Hash.SET_TEXT_OUTLINE);
        //    switch (alignment)
        //    {
        //        case Alignment.Center:
        //            Function.Call(Hash.SET_TEXT_CENTRE, true);
        //            break;
        //        case Alignment.Right:
        //            Function.Call(Hash.SET_TEXT_RIGHT_JUSTIFY, true);
        //            Function.Call(Hash.SET_TEXT_WRAP, 0, x);
        //            break;
        //    }

        //    if (wordWrap != 0)
        //    {
        //        float xsize = (xPos + wordWrap) / width;
        //        Function.Call(Hash.SET_TEXT_WRAP, x, xsize);
        //    }

        //    Function.Call(Hash._SET_TEXT_ENTRY, "jamyfafi");
        //    AddLongString(caption);

        //    Function.Call(Hash._DRAW_TEXT, x, y);
        //}
    }
}
