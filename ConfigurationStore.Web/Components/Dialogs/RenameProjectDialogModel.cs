using System.ComponentModel.DataAnnotations;

using ConfigurationStore.Validation.Projects;

namespace ConfigurationStore.Web.Components.Dialogs;

public class RenameProjectDialogModel
{
    [Required]
    [StringLength(128, ErrorMessage = "Project name must be at least 6, and at most 128 characters long", MinimumLength = 2)]
    [DataType(DataType.Text)]
    [Display(Name = "Project Name")]
    [UniqueProjectName]
    public string ProjectName { get; set; } = "";

    public bool Accepted { get; set; }
}