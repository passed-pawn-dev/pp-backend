using Microsoft.EntityFrameworkCore;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Entities.Courses.Elements;

namespace PassedPawn.DataAccess;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public required DbSet<Student> Students { get; init; }
    public required DbSet<Coach> Coaches { get; init; }
    public required DbSet<Photo> Photos { get; init; }
    public required DbSet<Nationality> Nationalities { get; init; }
    public required DbSet<Course> Courses { get; init; }
    public required DbSet<Lesson> Lessons { get; init; }
    public required DbSet<CoursePuzzle> CoursePuzzles { get; init; }
    public required DbSet<CourseExample> CourseExamples { get; init; }
    public required DbSet<CourseReview> CourseReviews { get; init; }
    public required DbSet<CourseQuiz> CourseQuizes { get; init; }
    public required DbSet<CourseVideo> CourseVideos { get; init; }

    //  dotnet ef database update --project ../PassedPawn.DataAccess/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Course>()
            .Property(course => course.ReleaseDate)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Nationality>().HasData(
            new Nationality { Id = 1, FullName = "United States", ShortName = "USA" },
            new Nationality { Id = 2, FullName = "Canada", ShortName = "CAN" },
            new Nationality { Id = 3, FullName = "United Kingdom", ShortName = "UK" },
            new Nationality { Id = 4, FullName = "Australia", ShortName = "AUS" },
            new Nationality { Id = 5, FullName = "Germany", ShortName = "GER" },
            new Nationality { Id = 6, FullName = "France", ShortName = "FRA" },
            new Nationality { Id = 7, FullName = "Italy", ShortName = "ITA" },
            new Nationality { Id = 8, FullName = "Spain", ShortName = "ESP" },
            new Nationality { Id = 9, FullName = "Japan", ShortName = "JPN" },
            new Nationality { Id = 10, FullName = "China", ShortName = "CHN" },
            new Nationality { Id = 11, FullName = "India", ShortName = "IND" },
            new Nationality { Id = 12, FullName = "Brazil", ShortName = "BRA" },
            new Nationality { Id = 13, FullName = "Mexico", ShortName = "MEX" },
            new Nationality { Id = 14, FullName = "Russia", ShortName = "RUS" },
            new Nationality { Id = 15, FullName = "South Africa", ShortName = "RSA" },
            new Nationality { Id = 16, FullName = "South Korea", ShortName = "KOR" },
            new Nationality { Id = 17, FullName = "Argentina", ShortName = "ARG" },
            new Nationality { Id = 18, FullName = "Saudi Arabia", ShortName = "KSA" },
            new Nationality { Id = 19, FullName = "Turkey", ShortName = "TUR" },
            new Nationality { Id = 20, FullName = "Netherlands", ShortName = "NED" },
            new Nationality { Id = 21, FullName = "Sweden", ShortName = "SWE" },
            new Nationality { Id = 22, FullName = "Switzerland", ShortName = "CHE" },
            new Nationality { Id = 23, FullName = "Poland", ShortName = "POL" },
            new Nationality { Id = 24, FullName = "Egypt", ShortName = "EGY" },
            new Nationality { Id = 25, FullName = "Nigeria", ShortName = "NGA" }
        );
    }
}