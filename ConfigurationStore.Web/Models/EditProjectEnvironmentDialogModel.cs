using System.ComponentModel.DataAnnotations;

using ConfigurationStore.Web.Models.Validation.Environments;

namespace ConfigurationStore.Web.Models;

public class EditProjectEnvironmentDialogModel
{
    public required int ProjectId { get; set; }
    public required string ProjectName { get; set; }

    [Required]
    [StringLength(128, ErrorMessage = "Environment name must be at least 2, and at most 128 characters long", MinimumLength = 2)]
    [DataType(DataType.Text)]
    [Display(Name = "Environment Name")]
    [UniqueEnvironmentName]
    public string Name { get; set; } = "";

    public bool Accepted { get; set; }

    public required string AcceptButtonText { get; set; }
}