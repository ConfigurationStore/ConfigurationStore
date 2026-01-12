using System.ComponentModel.DataAnnotations;

using ConfigurationStore.Web.Models.Validation.Projects;

namespace ConfigurationStore.Web.Models;

public class EditProjectDialogModel
{
    [Required]
    [StringLength(128, ErrorMessage = "Project name must be at least 6, and at most 128 characters long", MinimumLength = 2)]
    [DataType(DataType.Text)]
    [Display(Name = "Project Name")]
    [UniqueProjectName]
    public string ProjectName { get; set; } = "";

    public bool Accepted { get; set; }

    public required string AcceptButtonText { get; set; }
}