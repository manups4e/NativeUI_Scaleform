using CitizenFX.Core;
using ScaleformUI.LobbyMenu;
using ScaleformUI.PauseMenu;
using ScaleformUI.PauseMenus.Elements.Items;
using static CitizenFX.Core.Native.API;

namespace ScaleformUI.PauseMenus.Elements.Panels
{
    public class MinimapPanel
    {
        internal PauseMenuBase Parent { get; set; }
        internal BaseTab ParentTab { get; set; }
        internal Vector2 mapPosition = new Vector2();
        internal float zoomDistance = 0;
        internal bool enabled;
        private bool turnedOn = false;
        private bool IsRadarVisible = !IsRadarHidden();
        internal int localCoronaMapStage = 0;

        public MinimapRoute MinimapRoute;
        public List<FakeBlip> MinimapBlips { get; internal set; }

        public MinimapPanel(PauseMenuBase parent, BaseTab parenttab)
        {
            MinimapBlips = new List<FakeBlip>();
            MinimapRoute = new MinimapRoute();
            Parent = parent;
            ParentTab = parenttab;
        }

        public bool Enabled
        {
            get => enabled;
            set
            {
                if (Parent != null && Parent.Visible)
                {
                    if (Parent is MainView lobby)
                    {
                        if (lobby.listCol[lobby.FocusLevel].Type == "players")
                        {
                            if (lobby.PlayersColumn.Items[lobby.PlayersColumn.CurrentSelection].KeepPanelVisible)
                            {
                                return;
                            }
                        }
                    }
                    else if (Parent is TabView)
                    {
                        if (ParentTab is PlayerListTab plTab)
                        {
                            if (plTab.listCol[plTab.Focus].Type == "players")
                            {
                                if (plTab.PlayersColumn.Items[plTab.PlayersColumn.CurrentSelection].KeepPanelVisible)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
                enabled = value;
                if (enabled)
                {
                    if (localCoronaMapStage == -1)
                        localCoronaMapStage = 0;
                }
                else
                {
                    localCoronaMapStage = -1;
                    if (turnedOn)
                    {
                        IsRadarVisible = !IsRadarHidden();
                        DisplayRadar(false);
                        SetMapFullScreen(false);
                        turnedOn = false;
                    }
                }
                if (Parent != null && Parent.Visible && ParentTab.Visible)
                {
                    if (Parent is MainView lobby)
                    {
                        lobby._pause._lobby.CallFunction("HIDE_MISSION_PANEL", !enabled);
                    }
                    else if (Parent is TabView pause)
                    {
                        pause._pause._pause.CallFunction("HIDE_PLAYERS_TAB_MISSION_PANEL", !enabled);
                    }
                }
            }
        }

         internal void InitializeMapSize()
        {
            float top = float.NegativeInfinity;
            float bottom = float.PositiveInfinity;
            float left = float.PositiveInfinity;
            float right = float.NegativeInfinity;

            foreach (var data in MinimapRoute.CheckPoints)
            {
                top = Math.Max(top, data.Position.Y);
                bottom = Math.Min(bottom, data.Position.Y);
                left = Math.Min(left, data.Position.X);
                right = Math.Max(right, data.Position.X);
            }

            top = Math.Max(top, MinimapRoute.StartPoint.Position.Y);
            bottom = Math.Min(bottom, MinimapRoute.StartPoint.Position.Y);
            left = Math.Min(left, MinimapRoute.StartPoint.Position.X);
            right = Math.Max(right, MinimapRoute.StartPoint.Position.X);

            top = Math.Max(top, MinimapRoute.EndPoint.Position.Y);
            bottom = Math.Min(bottom, MinimapRoute.EndPoint.Position.Y);
            left = Math.Min(left, MinimapRoute.EndPoint.Position.X);
            right = Math.Max(right, MinimapRoute.EndPoint.Position.X);

            Vector3 topLeft = new Vector3(left, top, 0);
            Vector3 bottomRight = new Vector3(right, bottom, 0);

            // Center of square area
            mapPosition = new Vector2((topLeft.X + bottomRight.X) / 2, (topLeft.Y + bottomRight.Y) / 2);

            // Calculate our range and get the correct zoom.
            float DistanceX = Math.Abs(left - right);
            float DistanceY = Math.Abs(top - bottom);

            if (DistanceX > DistanceY)
            {
                zoomDistance = DistanceX / 1.5f;
            }
            else
            {
                zoomDistance = DistanceY / 2.0f;
            }

            RefreshMapPosition(mapPosition);
            LockMinimapAngle(0);

            //!! Draw Debug
            // var blipArea = AddBlipForArea(mapPosition.X, mapPosition.Y, 0.0f, DistanceX, DistanceY);
            // SetBlipAlpha(blipArea, 150);
            // RaceGalleryNextBlipSprite(1);
            // var blipTop = RaceGalleryAddBlip(topLeft.X, topLeft.Y, 0.0f);
            // RaceGalleryNextBlipSprite(1);
            // var blipBottom = RaceGalleryAddBlip(bottomRight.X, bottomRight.Y, 0.0f);
            // ShowNumberOnBlip(blipTop, 1);
            // ShowNumberOnBlip(blipBottom, 2);
        }

        public void RefreshMapPosition(Vector2 position)
        {
            mapPosition = new Vector2(position);
            if (ParentTab is GalleryTab g)
                zoomDistance = g.bigPic ? 600 : 1200;
        }

        internal void SetupBlips()
        {
            foreach (FakeBlip blip in MinimapBlips)
            {
                RaceGalleryNextBlipSprite(blip.Sprite);
                int b = RaceGalleryAddBlip(blip.Position.X, blip.Position.Y, blip.Position.Z);
                if (blip.Scale > 0)
                    SetBlipScale(b, blip.Scale);
                SetBlipColour(b, (int)blip.Color);
            }
        }

        public void MaintainMap()
        {
            switch (localCoronaMapStage)
            {
                case 0:
                    InitializeMap();
                    break;
                case 1:
                    ProcessMap();
                    break;
            }
        }
        public void ProcessMap()
        {
            if (enabled)
            {
                if (!turnedOn)
                {
                    DisplayRadar(IsRadarVisible);
                    SetMapFullScreen(true);
                    turnedOn = true;
                }
            }
            else
            {
                if (turnedOn)
                {
                    IsRadarVisible = !IsRadarHidden();
                    DisplayRadar(false);
                    SetMapFullScreen(false);
                    turnedOn = false;
                    Dispose();
                }
            }
            SetPlayerBlipPositionThisFrame(-5000, -5000);
            RefreshZoom();
        }

        internal void InitializeMapDisplay()
        {
            DeleteWaypoint();
            SetWaypointOff();
            ClearGpsCustomRoute();
            ClearGpsMultiRoute();
            SetPoliceRadarBlips(false);

            MinimapRoute.SetupCustomRoute();
            SetupBlips();
        }

        internal void InitializeMap()
        {
            // Use the data to set up our map.
            InitializeMapSize();
            // Initialise all our blips from data
            InitializeMapDisplay();
            // Set the zoom now
            RefreshZoom();
            localCoronaMapStage = 1;
        }

        /// <summary>
        /// To be called every tick
        /// </summary>
        internal void RefreshZoom()
        {
            if (zoomDistance != 0)
                SetRadarZoomToDistance(zoomDistance);
            LockMinimapPosition(mapPosition.X, mapPosition.Y);
        }

        internal void Dispose()
        {
            localCoronaMapStage = 0;
            enabled = false;
            N_0x2de6c5e2e996f178(0);
            SetPoliceRadarBlips(true);
            DisplayRadar(IsRadarVisible);
            RaceGalleryFullscreen(false);
            ClearRaceGalleryBlips();
            zoomDistance = 0;
            SetRadarZoom(0);
            SetGpsCustomRouteRender(false, 18, 30);
            SetGpsMultiRouteRender(false);
            UnlockMinimapPosition();
            UnlockMinimapAngle();
            DeleteWaypoint();
            ClearGpsCustomRoute();
            ClearGpsFlags();
            MinimapBlips.Clear();
            MinimapRoute = new MinimapRoute();
        }

        public void ClearMinimap()
        {
            MinimapBlips.Clear();
            MinimapRoute = new MinimapRoute();
            localCoronaMapStage = 0;
            zoomDistance = 0;
            ClearRaceGalleryBlips();
            SetRadarZoom(0);
            SetGpsCustomRouteRender(false, 18, 30);
            DeleteWaypoint();
            ClearGpsCustomRoute();
            ClearGpsFlags();
        }
    }
}
