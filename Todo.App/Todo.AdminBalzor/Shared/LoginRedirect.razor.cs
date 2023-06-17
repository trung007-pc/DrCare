using Microsoft.AspNetCore.Components;

namespace Todo.AdminBlazor.Shared;

public partial class LoginRedirect
{
    [Parameter]
    public string NavigateTo { get; set; }  
        
    protected override void OnAfterRender(bool firstRender)
    {
        _navigationManager.NavigateTo($"login?ToURl={NavigateTo}");
    }

}