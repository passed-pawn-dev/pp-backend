using PassedPawn.DataAccess.Entities.Courses;

namespace PassedPawn.DataAccess.Entities;

public class Student : User
{
    public List<CourseExercise> Puzzles { get; set; } = [];
}