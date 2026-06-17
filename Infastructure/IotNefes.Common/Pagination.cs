namespace IotNefes.Common
{
    public class PagedRequest
    {
        private int _pageSize = 10;
        private int _page = 1;

        public int Page
        {
            get => _page;
            set => _page = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value switch
            {
                <= 10 => 10,
                <= 50 => 50,
                _ => 100
            };
        }
    }

    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = [];
        public int Page { get; set; }
        public int PageSize { get; set; }
        public long TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPrevious => Page > 1;
        public bool HasNext => Page < TotalPages;
    }

    public class CursorPagedResult<T>
    {
        public List<T> Items { get; set; } = [];
        public int Limit { get; set; }
        public string? NextCursor { get; set; }
        public bool HasNext => !string.IsNullOrWhiteSpace(NextCursor);
    }
}
