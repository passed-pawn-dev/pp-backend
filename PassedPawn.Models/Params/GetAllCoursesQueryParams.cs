namespace PassedPawn.Models.Params;

public class GetAllCoursesQueryParams
{
    public int? EloRangeStart { get; init; }
    public int? EloRangeEnd { get; init; }
    public string? Name { get; init; }
    public int? MinPrice { get; init; }
    public int? MaxPrice { get; init; }
    public bool OnlyBought { get; init; } = false;
    public GetAllCoursesSortOrder SortBy { get; init; } = GetAllCoursesSortOrder.Popularity;
    public bool SortDesc { get; init; } = false;
}

public enum GetAllCoursesSortOrder
{
    Price, AverageScore, Popularity
}
