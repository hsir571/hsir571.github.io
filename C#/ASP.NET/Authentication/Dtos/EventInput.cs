using System.ComponentModel.DataAnnotations;

public class EventInput
{
    [Required(ErrorMessage = "Start is required.")]
    [RegularExpression(@"^\d{8}T\d{6}Z$", ErrorMessage = "The format of Start should be yyyyMMddTHHmmssZ.")]
    public string Start { get; set; }

    [Required(ErrorMessage = "End is required.")]
    [RegularExpression(@"^\d{8}T\d{6}Z$", ErrorMessage = "The format of End should be yyyyMMddTHHmmssZ.")]
    public string End { get; set; }

    [Required(ErrorMessage = "Summary is required.")]
    public string Summary { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Location is required.")]
    public string Location { get; set; }
}
