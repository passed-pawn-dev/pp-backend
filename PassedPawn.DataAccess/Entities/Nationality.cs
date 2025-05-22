using System.ComponentModel.DataAnnotations;

namespace PassedPawn.DataAccess.Entities;

public class Nationality : IEntity
{
    [StringLength(25)] public required string FullName { get; set; }

    [StringLength(25)] public required string ShortName { get; set; }
    public int Id { get; set; }
}
