using PassedPawn.Models.Enums;

namespace PassedPawn.DataAccess.Entities;

public class User : IEntity
{
    public int Id { get; set; }

    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    public required string Email { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public int? PhotoId { get; set; }
    
    // relacja
    public Photo? Photo { get; set; }
    
    public DateOnly DateOfBirth { get; set; }
    
    public int Elo { get; set; }
    
    public ChessTitle? ChessTitle { get; set; }

    public int NationalityId { get; set; }
    
    public Nationality? Nationality { get; set; }
    
    // Helpers
    // Nie zapisze sie w bazie danych bo nie ma setera (mamy nadzieje) najwyzej zrobimy z tego funkcje
    public int Age => DateTime.Now.Year - DateOfBirth.Year;
}