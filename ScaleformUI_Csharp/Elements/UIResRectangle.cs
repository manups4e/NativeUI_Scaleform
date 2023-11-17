using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.FiveM.GUI;
using System.Drawing;
using static CitizenFX.FiveM.Native.Natives;

namespace ScaleformUI
{
    /// <summary>
    /// A rectangle in 1080 pixels height system.
    /// </summary>
    public class UIResRectangle : CitizenFX.FiveM.GUI.Rectangle
    {
        public UIResRectangle()
        { }

        public UIResRectangle(Vector2 pos, Vector2 size) : base(pos, size)
        { }

        public UIResRectangle(Vector2 pos, Vector2 size, CitizenFX.Core.Color color) : base(pos, size, color)
        { }

        public void Draw(SizeF offset)
        {
            if (!Enabled) return;
            int screenw = Screen.Resolution.Width;
            int screenh = Screen.Resolution.Height;
            const float height = 1080f;
            float ratio = (float)screenw / screenh;
            var width = height * ratio;

            float w = Size.X / width;
            float h = Size.Y / height;
            float x = ((Position.X + offset.Width) / width) + w * 0.5f;
            float y = ((Position.Y + offset.Height) / height) + h * 0.5f;

            DrawRect(x, y, w, h, Color.R, Color.G, Color.B, Color.A);
        }

        public static void Draw(float xPos, float yPos, int boxWidth, int boxHeight, CitizenFX.Core.Color color)
        {
            int screenw = Screen.Resolution.Width;
            int screenh = Screen.Resolution.Height;
            const float height = 1080f;
            float ratio = (float)screenw / screenh;
            var width = height * ratio;

            float w = boxWidth / width;
            float h = boxHeight / height;
            float x = ((xPos) / width) + w * 0.5f;
            float y = ((yPos) / height) + h * 0.5f;

            DrawRect(x, y, w, h, color.R, color.G, color.B, color.A);
        }
    }
}