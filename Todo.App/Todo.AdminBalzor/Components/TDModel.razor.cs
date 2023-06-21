using Microsoft.AspNetCore.Components;

namespace Todo.AdminBlazor.Components;

public partial class TDModel : ComponentBase{
    [Parameter] public RenderFragment ChildContent { get; set; }

    [Parameter] public string Width { get; set; }
    [Parameter] public string Height { get; set; }
    [Parameter] public string Style { get; set; }
    [Parameter] public string Title { get; set; }
    [Parameter] public bool ShowTitle { get; set; }
    
    [Parameter] public string ConfirmMessage { get; set; }

    

    public TDModel()
    {
          
    }

    protected override async Task OnParametersSetAsync()
    {
        DialogService.Refresh();
    }

    public void Refresh()
    {
        DialogService.Refresh();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        StateHasChanged();
    }
}