using System.ComponentModel.DataAnnotations;

public class Organizer
{
    
    [Key]
    public string Name { get; set; }  
    public string Password { get; set; }  
}