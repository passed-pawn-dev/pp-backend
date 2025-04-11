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
    public required DbSet<CourseExercise> CourseExercises { get; init; }
    public required DbSet<CourseExample> CourseExamples { get; init; }
    public required DbSet<CourseReview> CourseReviews { get; init; }
    public required DbSet<CourseQuiz> CourseQuizes { get; init; }
    public required DbSet<CourseVideo> CourseVideos { get; init; }

    //  dotnet ef database update --project ../PassedPawn.DataAccess/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Nationality>().HasData(
            new Nationality { Id = 1, FullName = "United States", ShortName = "USA", FlagId = 1 },
            new Nationality { Id = 2, FullName = "Canada", ShortName = "CAN", FlagId = 2 },
            new Nationality { Id = 3, FullName = "United Kingdom", ShortName = "UK", FlagId = 3 },
            new Nationality { Id = 4, FullName = "Australia", ShortName = "AUS", FlagId = 4 },
            new Nationality { Id = 5, FullName = "Germany", ShortName = "GER", FlagId = 5 },
            new Nationality { Id = 6, FullName = "France", ShortName = "FRA", FlagId = 6 },
            new Nationality { Id = 7, FullName = "Italy", ShortName = "ITA", FlagId = 7 },
            new Nationality { Id = 8, FullName = "Spain", ShortName = "ESP", FlagId = 8 },
            new Nationality { Id = 9, FullName = "Japan", ShortName = "JPN", FlagId = 9 },
            new Nationality { Id = 10, FullName = "China", ShortName = "CHN", FlagId = 10 },
            new Nationality { Id = 11, FullName = "India", ShortName = "IND", FlagId = 11 },
            new Nationality { Id = 12, FullName = "Brazil", ShortName = "BRA", FlagId = 12 },
            new Nationality { Id = 13, FullName = "Mexico", ShortName = "MEX", FlagId = 13 },
            new Nationality { Id = 14, FullName = "Russia", ShortName = "RUS", FlagId = 14 },
            new Nationality { Id = 15, FullName = "South Africa", ShortName = "RSA", FlagId = 15 },
            new Nationality { Id = 16, FullName = "South Korea", ShortName = "KOR", FlagId = 16 },
            new Nationality { Id = 17, FullName = "Argentina", ShortName = "ARG", FlagId = 17 },
            new Nationality { Id = 18, FullName = "Saudi Arabia", ShortName = "KSA", FlagId = 18 },
            new Nationality { Id = 19, FullName = "Turkey", ShortName = "TUR", FlagId = 19 },
            new Nationality { Id = 20, FullName = "Netherlands", ShortName = "NED", FlagId = 20 },
            new Nationality { Id = 21, FullName = "Sweden", ShortName = "SWE", FlagId = 21 },
            new Nationality { Id = 22, FullName = "Switzerland", ShortName = "CHE", FlagId = 22 },
            new Nationality { Id = 23, FullName = "Poland", ShortName = "POL", FlagId = 23 },
            new Nationality { Id = 24, FullName = "Egypt", ShortName = "EGY", FlagId = 24 },
            new Nationality { Id = 25, FullName = "Nigeria", ShortName = "NGA", FlagId = 25 }
        );

        modelBuilder.Entity<Photo>().HasData(
            new Photo { Id = 1 },
            new Photo { Id = 2 },
            new Photo { Id = 3 },
            new Photo { Id = 4 },
            new Photo { Id = 5 },
            new Photo { Id = 6 },
            new Photo { Id = 7 },
            new Photo { Id = 8 },
            new Photo { Id = 9 },
            new Photo { Id = 10 },
            new Photo { Id = 11 },
            new Photo { Id = 12 },
            new Photo { Id = 13 },
            new Photo { Id = 14 },
            new Photo { Id = 15 },
            new Photo { Id = 16 },
            new Photo { Id = 17 },
            new Photo { Id = 18 },
            new Photo { Id = 19 },
            new Photo { Id = 20 },
            new Photo { Id = 21 },
            new Photo { Id = 22 },
            new Photo { Id = 23 },
            new Photo { Id = 24 },
            new Photo { Id = 25 }
        );
    }
}