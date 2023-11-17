using CitizenFX.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaleformUI
{
    public class MarkersHandler : BaseScript
    {

        private static List<Marker> _markerList = new List<Marker>();

        public MarkersHandler()
        {
            Tick += MainHandler;
        }

        public async Coroutine MainHandler()
        {
            if (_markerList.Count == 0) return;

            for (int i = 0; i < FilterMarkers().Count; i++)
                FilterMarkers()[i].Draw();
            await WaitUntilNextFrame(); ;
        }

        private List<Marker> FilterMarkers() => _markerList.Where(x => MenuHandler.PlayerPed.IsInRangeOf(x.Position, x.Distance)).ToList();

        /// <summary>
        /// Adds a marker to the list
        /// </summary>
        /// <param name="marker"></param>
        public static void AddMarker(Marker marker)
        {
            if (!_markerList.Contains(marker))
                _markerList.Add(marker);
        }

        /// <summary>
        /// Removes the marker from the list
        /// </summary>
        /// <param name="marker"></param>
        public static void RemoveMarker(Marker marker)
        {
            if (_markerList.Contains(marker))
                _markerList.Remove(marker);
        }
    }
}
