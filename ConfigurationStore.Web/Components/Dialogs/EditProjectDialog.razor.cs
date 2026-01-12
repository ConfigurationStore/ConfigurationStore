using System.ComponentModel.DataAnnotations;

using ConfigurationStore.Web.Models;

using Microsoft.AspNetCore.Components;

using Radzen;

namespace ConfigurationStore.Web.Components.Dialogs;

public partial class EditProjectDialog
{
    private readonly DialogService _dialogService;

    [Parameter]
    public EditProjectDialogModel Model { get; set; } = null!;

    public EditProjectDialog(DialogService dialogService)
    {
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
    }

    public void CloseDialog()
    {
        Model.Accepted = false;
        _dialogService.Close();
    }

    public void SaveProject()
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