using Microsoft.AspNetCore.Components;

using Radzen;

namespace ConfigurationStore.Web.Components.Dialogs;

public partial class RenameProjectDialog : ComponentBase
{
    private readonly DialogService _dialogService;

    [Parameter]
    public RenameProjectDialogModel Model { get; set; } = null!;

    public RenameProjectDialog(DialogService dialogService)
    {
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
    }

    public void CloseDialog()
    {
        Model.Accepted = false;
        _dialogService.Close();
    }

    public void RenameProject()
    {
        Model.Accepted = true;
        _dialogService.Close(Model);
    }

    private void DoNotRenameProject()
    {
        Console.WriteLine("Here");
        StateHasChanged();
    }
}