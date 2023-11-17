using CitizenFX.Core;
using ScaleformUI.Elements;
using System.Drawing;

namespace ScaleformUI.PauseMenus
{
    public enum CrewHierarchy
    {
        Leader = 0,
        Commissioner,
        Liutenant = 3, // 2 looks exactly like 1.. so dunno.. let's start on 3
        Representative,
        Muscle,
        Generic
    }

    public class CrewTag
    {
        public string TAG { get; internal set; } = "";
        public CrewTag()
        {
            TAG = "";
        }
        public CrewTag(string tag, bool crewTypeIsPrivate, bool crewTagContainsRockstar, CrewHierarchy level, CitizenFX.Core.Color crewColor)
        {
            try
            {
                string result = "";
                result += crewTypeIsPrivate ? "(" : " ";
                result += crewTagContainsRockstar ? "*" : " ";
                result += ((int)level).ToString();
                result += tag.ToUpper();
                result += $"#{crewColor.R:X2}{crewColor.G:X2}{crewColor.B:X2}";
                TAG = result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
        public CrewTag(string tag, bool crewTypeIsPrivate, bool crewTagContainsRockstar, CrewHierarchy level, SColor crewColor)
        {
            try
            {
                string result = "";
                result += crewTypeIsPrivate ? "(" : " ";
                result += crewTagContainsRockstar ? "*" : " ";
                result += ((int)level).ToString();
                result += tag.ToUpper();
                result += crewColor.HexValue;
                TAG = result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
    }
}