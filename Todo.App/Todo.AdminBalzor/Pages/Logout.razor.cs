using Todo.Core.Enums;

namespace Todo.AdminBlazor.Pages;

public partial class Logout
{
    protected override async Task OnInitializedAsync()
    {
        await InvokeAsync( async () =>
        {
            _userService.Logout();
            _navigationManager.NavigateTo("/login",true);
        }, ActionTypes.SignOut, true);
  
    }
}