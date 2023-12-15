﻿namespace ScaleformUI.Menu
{
    internal class PaginationHandler
    {
        private int _currentPageIndex;
        private int _currentMenuIndex;
        private int currentPage;
        private int itemsPerPage;
        private int minItem;
        private int maxItem;
        private int totalItems;
        private int scaleformIndex;

        internal ScrollingType scrollType = ScrollingType.CLASSIC;
        internal int CurrentPage { get => currentPage; set => currentPage = value; }
        internal int ItemsPerPage { get => itemsPerPage; set => itemsPerPage = value; }
        internal int TotalItems { get => totalItems; set => totalItems = value; }
        internal int TotalPages => (int)Math.Ceiling(totalItems / (float)itemsPerPage);
        internal int CurrentPageStartIndex => CurrentPage * itemsPerPage;
        internal int CurrentPageEndIndex
        {
            get
            {
                int index = CurrentPageStartIndex + itemsPerPage - 1;
                if (index >= totalItems)
                    index = totalItems - 1;
                return index;
            }
        }
        internal int CurrentPageIndex { get => _currentPageIndex; set => _currentPageIndex = GetPageIndexFromMenuIndex(value); }
        internal int CurrentMenuIndex { get => _currentMenuIndex; set => _currentMenuIndex = value; }
        internal int MinItem { get => minItem; set => minItem = value; }
        internal int MaxItem { get => maxItem; set => maxItem = value; }
        internal int ScaleformIndex { get => scaleformIndex; set => scaleformIndex = value; }

        internal bool IsItemVisible(int menuIndex)
        {
            return menuIndex >= minItem || menuIndex <= minItem && menuIndex <= maxItem;
        }

        internal int GetScaleformIndex(int menuIndex)
        {
            int id = 0;
            if (minItem <= menuIndex)
            {
                id = menuIndex - minItem;
            }
            else if (minItem > menuIndex && maxItem >= menuIndex)
            {
                id = (menuIndex - maxItem) + (itemsPerPage - 1);
            }
            return id;
        }

        internal int GetMenuIndexFromScaleformIndex(int scaleformIndex)
        {
            int tmpIndex = minItem + scaleformIndex;
            if (tmpIndex >= totalItems)
                tmpIndex = totalItems - 1;
            return tmpIndex;
            /*
            int diff = (totalItems >= itemsPerPage ? itemsPerPage : totalItems) - 1 - scaleformIndex;
            int result = MaxItem - diff;
            if (result < 0)
                result = totalItems + result;
            return result;
            */
        }

        internal int GetPageIndexFromScaleformIndex(int scaleformIndex)
        {
            int menuIndex = GetMenuIndexFromScaleformIndex(scaleformIndex);
            return GetPageIndexFromMenuIndex(menuIndex);
        }

        internal int GetPageFromScaleformIndex(int scaleformIndex)
        {
            int menuIndex = GetMenuIndexFromScaleformIndex(scaleformIndex);
            return GetPage(menuIndex);
        }


        internal int GetPageIndexFromMenuIndex(int menuIndex)
        {
            int page = GetPage(menuIndex);
            int startIndex = page * itemsPerPage;
            return menuIndex - startIndex;
        }

        internal int GetMenuIndexFromPageIndex(int page, int index)
        {
            int initialIndex = page * itemsPerPage;
            return initialIndex + index;
        }

        internal int GetPage(int menuIndex)
        {
            return (int)Math.Floor(menuIndex / (float)itemsPerPage);
        }

        internal int GetPageItemsCount(int page)
        {
            int minItem = page * itemsPerPage;
            int maxItem = minItem + itemsPerPage - 1;
            if (maxItem >= totalItems)
                maxItem = totalItems - 1;
            return (maxItem - minItem) + 1;
        }

        internal int GetMissingItems()
        {
            int count = GetPageItemsCount(currentPage);
            return itemsPerPage - count;
        }

        internal bool GoUp()
        {
            bool overflow = false;
            CurrentMenuIndex--;
            if (CurrentMenuIndex < 0)
            {
                CurrentMenuIndex = TotalItems - 1;
                overflow = TotalPages > 1;
            }
            CurrentPageIndex = CurrentMenuIndex;
            ScaleformIndex--;
            CurrentPage = GetPage(CurrentMenuIndex);
            if (ScaleformIndex < 0)
            {
                if (TotalItems <= itemsPerPage)
                {
                    ScaleformIndex = TotalItems - 1;
                    return false;
                }
                if (scrollType == ScrollingType.ENDLESS || (scrollType == ScrollingType.CLASSIC && !overflow))
                {
                    minItem--;
                    maxItem--;
                    if (minItem < 0)
                        minItem = TotalItems - 1;
                    if (maxItem < 0)
                        maxItem = TotalItems - 1;
                    ScaleformIndex = 0;
                    return true;
                }
                else if (scrollType == ScrollingType.PAGINATED || (scrollType == ScrollingType.CLASSIC && overflow))
                {
                    minItem = CurrentPageStartIndex;
                    maxItem = CurrentPageEndIndex;
                    ScaleformIndex = GetPageIndexFromMenuIndex(CurrentPageEndIndex);
                    if (scrollType == ScrollingType.CLASSIC)
                    {
                        int missingItems = GetMissingItems();
                        if (missingItems > 0)
                        {
                            ScaleformIndex = GetPageIndexFromMenuIndex(CurrentPageEndIndex) + missingItems;
                            minItem = CurrentPageStartIndex - missingItems;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        internal bool GoDown()
        {
            bool overflow = false;
            CurrentMenuIndex++;
            if (CurrentMenuIndex >= TotalItems)
            {
                CurrentMenuIndex = 0;
                overflow = TotalPages > 1;
            }
            CurrentPageIndex = CurrentMenuIndex;
            ScaleformIndex++;
            if (ScaleformIndex >= totalItems)
            {
                ScaleformIndex = 0;
                CurrentPage = GetPage(CurrentMenuIndex);
                return false;
            }
            else if (scaleformIndex > itemsPerPage - 1)
            {
                if (scrollType == ScrollingType.ENDLESS || (scrollType == ScrollingType.CLASSIC && !overflow))
                {
                    CurrentPage = GetPage(CurrentMenuIndex);
                    ScaleformIndex = itemsPerPage - 1;
                    minItem++;
                    maxItem++;
                    if (minItem >= totalItems)
                        minItem = 0;
                    if (maxItem >= totalItems)
                        maxItem = 0;
                    return true;
                }
                else if (scrollType == ScrollingType.PAGINATED || (scrollType == ScrollingType.CLASSIC && overflow))
                {
                    CurrentPage = GetPage(CurrentMenuIndex);
                    minItem = CurrentPageStartIndex;
                    maxItem = CurrentPageEndIndex;
                    ScaleformIndex = 0;
                    return true;
                }
            }
            else if (scrollType == ScrollingType.PAGINATED && scaleformIndex > GetPageIndexFromMenuIndex(CurrentPageEndIndex))
            {
                CurrentPage = GetPage(CurrentMenuIndex);
                minItem = CurrentPageStartIndex;
                maxItem = CurrentPageEndIndex;
                ScaleformIndex = 0;
                return true;
            }
            CurrentPage = GetPage(CurrentMenuIndex);
            return false;
        }

        internal void Reset()
        {
            _currentPageIndex = 0;
            _currentMenuIndex = 0;
            currentPage = 0;
            minItem = 0;
            maxItem = 0;
            totalItems = 0;
            scaleformIndex = 0;
        }

        public override string ToString()
        {
            string str = "";
            str += "_currentMenuIndex: " + _currentMenuIndex + ", ";
            str += "_currentPageIndex: " + _currentPageIndex + ", ";
            str += "currentPage: " + currentPage + ", ";
            str += "CurrentPageStartIndex: " + CurrentPageStartIndex + ", ";
            str += "CurrentPageEndIndex: " + CurrentPageEndIndex + ", ";
            str += "itemsPerPage: " + itemsPerPage + ", ";
            str += "minItem: " + minItem + ", ";
            str += "maxItem: " + maxItem + ", ";
            str += "totalItems: " + totalItems + ", ";
            str += "scaleformIndex: " + scaleformIndex;
            return str;
        }
    }
}
