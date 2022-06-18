using System.Reflection.Metadata.Ecma335;

namespace MobileAPIS4.Models.DisplayDto;

public class StaffDisplayDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public string Telephone { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
    public string Photo { get; set; }
}