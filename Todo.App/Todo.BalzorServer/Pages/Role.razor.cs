using Microsoft.AspNetCore.Components;
using MudBlazor;
using Radzen;
using Radzen.Blazor;
using Todo.BalzorServer.Components;
using Todo.BalzorServer.Helper;
using Todo.BalzorServer.Shared;
using Todo.Contract.Claims;
using Todo.Contract.Roles;
using Todo.Core.Consts.Permissions;
using Todo.Core.Enums;

namespace Todo.BalzorServer.Pages;

public partial class Role 
{
    public List<RoleDto> Roles = new List<RoleDto>();
    public CreateUpdateRoleDto NewRole = new CreateUpdateRoleDto();
    public CreateUpdateRoleDto EditingRole = new CreateUpdateRoleDto();
    public Guid EditingRoleId { get; set; }
    public string SearchString;
    public string HeaderTitle = "Role";
    public TDModel NewModal;
    public TDModel EditingModal;
    public TDModel ClaimModal;
    public Dictionary<string,List<ClaimItem>> Claims { get; set; }
    public bool CanAuthorize { get; set; }
    


     public Role()
     {
          
        
     }
        
     protected override async Task OnInitializedAsync()
     {
       
     }

        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                await InvokeAsync(async () =>
                {
                    HeaderTitle = L["Role"];
                    await SetAuthorizedAction();
                    await GetRoles();
                    StateHasChanged();
                }, ActionTypes.GetList);
             
            }
        }

    
        
        public async Task SetAuthorizedAction()
        {
            var claims = await GetClaims(ExtendClaimTypes.Permission);
            CanCreate = claims.Any(x => x == AccessClaims.Roles.Create);
            CanEdit = claims.Any(x => x == AccessClaims.Roles.Edit);
            CanCreate = claims.Any(x => x == AccessClaims.Roles.Create);
            CanAuthorize = claims.Any(x => x == AccessClaims.Roles.Authorize);
        }
        public async Task GetRoles()
        {
            Roles = await _roleService.GetListAsync();
        }

        
        async void OnClick(RadzenSplitButtonItem item,RoleDto role)
        {
            if (item != null)
            {
                switch (item.Value)
                {
                    
                    case FormActions.Delete:
                    {
                        break;
                    }
                    case FormActions.Edit:
                    {
                       await  ShowEditingModal(role);
                        break;
                    }
                    case FormActions.Claim:
                    {
                        await ShowClaim(role.Id);
                        break;
                    }
                }
            }
        }
        
        
        
        public async Task CreateRole()
        {
            await InvokeAsync(async () =>
            {
                await _roleService.CreateAsync(input: NewRole);
                HideNewModal();
                await GetRoles();
            }, ActionTypes.Create, true);
        }
        
        public async Task UpdateRole()
        {
            await InvokeAsync(async () =>
            {
                await _roleService.UpdateAsync(EditingRole, EditingRoleId);
                HideEditingModal();
                await GetRoles();
            },ActionTypes.Update,true);
            
        }
        
        public async Task DeleteRole(Guid id)
        {
            await InvokeAsync(async () =>
            {
                await _roleService.DeleteAsync(id);
                HideEditingModal();
                await GetRoles();
            },ActionTypes.Delete,true);
            
        }

 

        public async void ShowNewModal()
        {
            NewRole = new CreateUpdateRoleDto();
            await NewModal.ShowModel();
        }
        
        public void HideNewModal()
        {
            NewModal.HideModel();
        }
        
        
        public Task ShowEditingModal(RoleDto roleDto)
        {
            EditingRole = new CreateUpdateRoleDto();
            EditingRole = ObjectMapper.Map<RoleDto, CreateUpdateRoleDto>(roleDto);
            EditingRoleId = roleDto.Id;
            return EditingModal.ShowModel();
        }
        
        public void HideEditingModal()
        {
            EditingModal.HideModel();
        }

        public  async Task ShowClaim(Guid id)
        {
           Claims =  SecurityHelper.GetClaims();
           var roleClaims = await _roleService.GetClaims(id);
           MarkClaimsSelection(roleClaims);
           EditingRoleId = id;
           await ClaimModal.ShowModel();
        }

        public void MarkClaimsSelection(List<ClaimDto> roleClaims)
        {
          var items = Claims
                .SelectMany(x => x.Value);

          foreach (var item in items)
          {
              if (roleClaims.Any(x => x.Value == item.Value))
              {
                  item.IsSelected = true;
              }
          } 
         
        }
        
        
        public async Task HideClaim()
        {
            await ClaimModal.ShowModel();
        }

        public async Task UpdateClaimsToRole()
        {
            var claims = new List<CreateUpdateClaimDto>();
            var selectedClaims = Claims.SelectMany(x => x.Value)
                .Where(x => x.IsSelected).ToList();
            foreach (var item in selectedClaims)
            {
                claims.Add(new CreateUpdateClaimDto()
                {
                    ClaimType = ExtendClaimTypes.Permission,
                    ClaimValue = item.Value
                });
            }
            await InvokeAsync(async () =>
            {
                await _roleService.UpdateClaimsToRole(EditingRoleId, claims);
                HideClaim();
            },ActionTypes.Update,true);

        }
        
     
}

