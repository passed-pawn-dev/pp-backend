﻿using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Entities.Courses.Elements;

namespace PassedPawn.DataAccess.Entities;

public class Student : User
{
    public List<Course> Courses { get; init; } = [];
    public List<CoursePuzzle> Puzzles { get; init; } = [];
}