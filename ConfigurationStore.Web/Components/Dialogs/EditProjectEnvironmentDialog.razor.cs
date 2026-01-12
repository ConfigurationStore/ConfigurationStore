using System.ComponentModel.DataAnnotations;

using ConfigurationStore.Web.Models;

using Microsoft.AspNetCore.Components;

using Radzen;

namespace ConfigurationStore.Web.Components.Dialogs;

public partial class EditProjectEnvironmentDialog
{
    private readonly DialogService _dialogService;

    [Parameter]
    public EditProjectEnvironmentDialogModel Model { get; set; } = null!;

    public EditProjectEnvironmentDialog(DialogService dialogService)
    {
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
    }

    public void CloseDialog()
    {
        Model.Accepted = false;
        _dialogService.Close();
    }

    public void SaveEnvironment()
    {
        Model.Accepted = true;
        _dialogService.Close(Model);
    }

    private void InvalidSubmit()
    {
        Console.WriteLine("Here");
        StateHasChanged();
    }
}