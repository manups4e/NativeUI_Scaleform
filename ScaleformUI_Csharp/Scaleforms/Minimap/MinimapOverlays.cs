using CitizenFX.Core;
using static CitizenFX.FiveM.Native.Natives;

namespace ScaleformUI.Scaleforms
{
    public static class MinimapOverlays
    {
        internal static Dictionary<int, KeyValuePair<string, string>> minimaps = new();
        internal static int overlay = 0;

        internal static async Task Load()
        {
            overlay = AddMinimapOverlay("files/MINIMAP_LOADER.gfx");
            while (!HasMinimapOverlayLoaded(overlay)) await BaseScript.Delay(0);
            SetMinimapOverlayDisplay(overlay, 0f, 0f, 100f, 100f, 100f);
        }

        private static async Task<int> addOverlay(string method, string txd, string txn, float x, float y, float r, float w, float h, int a, bool centered)
        {
            if (overlay == 0) await Load();

            if (!HasStreamedTextureDictLoaded(txd))
            {
                RequestStreamedTextureDict(txd, false);
                while (!HasStreamedTextureDictLoaded(txd)) await BaseScript.Delay(0);
            }

            CallMinimapScaleformFunction(overlay, method);
            ScaleformMovieMethodAddParamTextureNameString(txd);
            ScaleformMovieMethodAddParamTextureNameString(txn);
            ScaleformMovieMethodAddParamFloat(x);
            ScaleformMovieMethodAddParamFloat(y);
            ScaleformMovieMethodAddParamFloat(r);
            ScaleformMovieMethodAddParamFloat(w);
            ScaleformMovieMethodAddParamFloat(h);
            ScaleformMovieMethodAddParamInt(a);
            ScaleformMovieMethodAddParamBool(centered);
            EndScaleformMovieMethod();

            SetStreamedTextureDictAsNoLongerNeeded(txd);

            minimaps.Add(minimaps.Count + 1, new(txd, txn));
            return minimaps.Count;

        }

        /// <summary>
        /// Adds a custom minimap in any place of the map specified
        /// </summary>
        /// <param name="textureDict">The texture dictionary</param>
        /// <param name="textureName">The texture name</param>
        /// <param name="x">X world coordinate</param>
        /// <param name="y">Y world coordinate</param>
        /// <param name="rotation">Rotation of the texture in degrees (default 0)</param>
        /// <param name="width">The width of the texture in pixels (default -1 to leave on original size, else this will override original width)</param>
        /// <param name="height">The height of the texture in pixels (default -1 to leave on original size, else this will override original height)</param>
        /// <param name="alpha">The alpha transparency value of the texture (default 100)</param>
        /// <param name="centered"><see langword="true"/> to center the texture to the given coordinates, <see langword="false"/> to center it on its top-left corner</param>
        /// <returns>The overlay ID in Dictionary</returns>
        public static async Task<int> AddSizedOverlayToMap(string textureDict, string textureName, float x, float y, float rotation = 0, float width = -1, float height = -1, int alpha = 100, bool centered = false)
        {
            return await addOverlay("ADD_SIZED_OVERLAY", textureDict, textureName, x, y, rotation, width, height, alpha, centered);
        }

        /// <summary>
        /// Adds a custom minimap in any place of the map specified
        /// </summary>
        /// <param name="textureDict">The texture dictionary</param>
        /// <param name="textureName">The texture name</param>
        /// <param name="x">X world coordinate</param>
        /// <param name="y">Y world coordinate</param>
        /// <param name="rotation">Rotation of the texture in degrees (default 0)</param>
        /// <param name="xScale">The horizontal scale (percentage) of the texture (default 100)</param>
        /// <param name="yScale">The vertical scale (percentage) of the texture (default 100)</param>
        /// <param name="alpha">The alpha transparency value of the texture (default 100)</param>
        /// <param name="centered"><see langword="true"/> to center the texture to the given coordinates, <see langword="false"/> to center it on its top-left corner</param>
        /// <returns>The overlay ID in Dictionary</returns>
        public static async Task<int> AddScaledOverlayToMap(string textureDict, string textureName, float x, float y, float rotation = 0, float xScale = 100f, float yScale = 100f, int alpha = 100, bool centered = false)
        {
            return await addOverlay("ADD_SCALED_OVERLAY", textureDict, textureName, x, y, rotation, xScale, yScale, alpha, centered);
        }

        /// <summary>
        /// Remove an overlay from the ingame Map
        /// </summary>
        /// <param name="overlayId">Id of the overlay to remove</param>
        public static async void RemoveOverlayFromMinimap(int overlayId)
        {
            if (overlay == 0) await Load();
            CallMinimapScaleformFunction(overlay, "REM_OVERLAY");
            ScaleformMovieMethodAddParamInt(overlayId - 1);
            EndScaleformMovieMethod();
            minimaps.Remove(overlayId);
        }
    }
}
