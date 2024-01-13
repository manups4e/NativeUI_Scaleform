﻿using ScaleformUI.Elements;
using ScaleformUI.Menu;
using ScaleformUI.PauseMenus;

namespace ScaleformUI.LobbyMenu
{
    public delegate void IndexChanged(int index);
    public class Column
    {
        internal bool isBuilding = false;
        public int ParentTab { get; internal set; }
        public PauseMenuBase Parent { get; internal set; }
        internal PaginationHandler Pagination { get; set; }
        public string Label { get; internal set; }
        public SColor Color { get; internal set; }
        public int Order { get; internal set; }
        public string Type { get; internal set; }

        public Column(string label, SColor color)
        {
            Label = label;
            Color = color;
        }
    }
}
