using System;

namespace Utilities.DTOs
{
    public abstract class PagedResultBase
    {
        public int CurrentPage { get; set; }
        private int _pageCount;
        public int PageCount
        {
            get
            {
                var _pageCount = (double)RowCount / PageSize;
                return (int)Math.Ceiling(_pageCount);
            }
            set => _pageCount = value;
        }
        public int PageSize { get; set; }

        public int RowCount { get; set; }

        public int FirstRowOnPage => (CurrentPage - 1) * PageSize + 1;

        public int LastRowOnPage => Math.Min(CurrentPage * PageSize, RowCount);
    }
}
