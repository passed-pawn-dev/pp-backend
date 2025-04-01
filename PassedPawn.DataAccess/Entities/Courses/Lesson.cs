using System.ComponentModel.DataAnnotations.Schema;
using PassedPawn.DataAccess.Entities.Courses.Elements;

namespace PassedPawn.DataAccess.Entities.Courses;

public class Lesson : IEntity
{
    public int LessonNumber { get; set; }

    public int CourseId { get; set; }
    public Course? Course { get; set; }

    public int? VideoId { get; set; }
    public CourseVideo? Video { get; set; }

    public ICollection<CourseExercise> Exercises { get; init; } = [];
    public ICollection<CourseExample> Examples { get; init; } = [];
    public int Id { get; set; }

    [NotMapped]
    public IEnumerable<CourseElement> Elements => [..Exercises, ..Examples];
}