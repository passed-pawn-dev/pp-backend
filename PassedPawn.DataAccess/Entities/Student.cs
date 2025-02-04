using PassedPawn.DataAccess.Entities.Courses;

namespace PassedPawn.DataAccess.Entities;

public class Student : User
{
    public List<Course> Courses { get; init; } = [];
    public List<CourseExercise> Puzzles { get; init; } = [];
}