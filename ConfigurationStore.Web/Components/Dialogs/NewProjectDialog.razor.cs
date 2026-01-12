using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Components;

using Radzen;

namespace ConfigurationStore.Web.Components.Dialogs;

public partial class NewProjectDialog
{
    private readonly DialogService _dialogService;

    [Parameter]
    public NewProjectDialogModel Model { get; set; } = null!;

    public NewProjectDialog(DialogService dialogService)
    {
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
    }

    public void CloseDialog()
    {
        Model.Accepted = false;
        _dialogService.Close();
    }

    public void CreateProject()
    {
        Model.Accepted = true;
        _dialogService.Close(Model);
    }

    private void DoNotCreateProject()
    {
        Console.WriteLine("Here");
        StateHasChanged();
    }
}