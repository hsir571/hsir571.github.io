using System.ComponentModel.DataAnnotations;

public class Users
{
    [Key]
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Address { get; set; } 
}