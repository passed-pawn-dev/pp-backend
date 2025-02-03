namespace PassedPawn.Models.DTOs.Course;

public class UserCourseDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public int LessonNumber { get; init; }
}