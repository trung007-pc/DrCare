using Microsoft.AspNetCore.Components;
using Radzen;
using Todo.Contract.Users;
using Todo.Core.Enums;

namespace Todo.AdminBlazor.Pages;

public partial class Login 
{
    public bool IsLoading = false;
        
    [Parameter]
    [SupplyParameterFromQuery(Name = "toURl")]
    public string ToURl { get; set; }
    public Login()
    {
            
    }

    protected async override Task OnInitializedAsync()
    {
        if (await IsAuthenticatedAsync())
        {
            _navigationManager.NavigateTo("/");
        }
    }
        
    async Task  OnLogin(LoginArgs args, string name)
    {

        var req = new LoginRequestDto() {UserName = args.Username, Password = args.Password};
        IsLoading = true;
        await InvokeAsync(async () =>
        {
            await  _userService.LoginAsync(req);
            _navigationManager.NavigateTo(ToURl,true);
        }, ActionTypes.SignIn, true);
        IsLoading = false;

    }
}