using System.ComponentModel.DataAnnotations;

namespace PassedPawn.DataAccess.Entities;

public class Nationality : IEntity
{
    public int Id { get; set; }
    
    [StringLength(25)]
    public required string FullName { get; set; }
    
    [StringLength(25)]
    public required string ShortName { get; set; }

    public int FlagId { get; set; }
    public Photo? Flag { get; set; }
}
