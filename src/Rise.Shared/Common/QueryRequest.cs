namespace Rise.Shared.Common;


public static partial class QueryRequest
{
    public class SkipTake
    {
        public string SearchTerm { get; set; } = string.Empty;

        public int Skip { get; set; }
        public int Take { get; set; } = 20;

        public string? OrderBy { get; set; }
        public bool OrderDescending { get; set; }

        public Dictionary<string, object?> Filters { get; set; } = new();
    }
    
    
    public class Cursor<TKey>
    {
        public string SearchTerm { get; set; } = string.Empty;

        public TKey? LastKey { get; set; }
        public int PageSize { get; set; } = 20;

        public string? OrderBy { get; set; }
        public bool OrderDescending { get; set; }

        public Dictionary<string, object?> Filters { get; set; } = new();
    }
}

