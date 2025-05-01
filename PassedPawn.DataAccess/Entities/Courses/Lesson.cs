using System.ComponentModel.DataAnnotations.Schema;
using PassedPawn.DataAccess.Entities.Courses.Elements;

namespace PassedPawn.DataAccess.Entities.Courses;

public class Lesson : IEntity
{
    public required string Title { get; set; }
    public int LessonNumber { get; set; }

    public int CourseId { get; set; }
    public Course? Course { get; set; }
    public bool Preview { get; set; }

    public ICollection<CourseExercise> Exercises { get; init; } = [];
    public ICollection<CourseExample> Examples { get; init; } = [];
    public ICollection<CourseVideo> Videos { get; init; } = [];
    public ICollection<CourseQuiz> Quizzes { get; init; } = [];
    public int Id { get; set; }

    [NotMapped]
    public IEnumerable<CourseElement> Elements => [..Exercises, ..Examples, ..Videos, ..Quizzes];
}