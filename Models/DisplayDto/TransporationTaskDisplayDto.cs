using System;
using System.ComponentModel.DataAnnotations;

namespace MobileAPIS4.Models.DisplayDto;

public class TransporationTaskDisplayDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int StartDistrictId { get; set; }
    public string StartStreetAddress { get; set; }
    public int DestinationDistrictId { get; set; }
    public string DestinationStreetAddress { get; set; }
    public decimal Weight { get; set; }
    public DateTime RequiredCompletionDate { get; set; }
    public DateTime? ActualCompletionDate { get; set; }
    public int VehicleTeamAdministrator { get; set; }
    public string Remark { get; set; }
    public int StatusId { get; set; }
    public string StatusName { get; set; }
}