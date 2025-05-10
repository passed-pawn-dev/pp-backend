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
    public bool SortDesc { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 3;
    public string? CoachName { get; init; }
    public GetAllCoursesCourseSize? CourseSize { get; init; }
}

public enum GetAllCoursesSortOrder
{
    Price, AverageScore, Popularity
}

public enum GetAllCoursesCourseSize
{
    Small, Big
}
