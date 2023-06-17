using Radzen.Blazor;
using Todo.AdminBlazor.Components;
using Todo.AdminBlazor.Helper;
using Todo.AdminBlazor.Shared;
using Todo.Contract.Claims;
using Todo.Contract.Tenants;
using Todo.Core.Consts.Permissions;
using Todo.Core.Enums;

namespace Todo.AdminBlazor.Pages;

public partial class Tenant
{
    public List<TenantDto> Tenants = new List<TenantDto>();
    public CreateUpdateTenantDto NewTenant = new CreateUpdateTenantDto();
    public CreateUpdateTenantDto EditingTenant = new CreateUpdateTenantDto();
    public Guid EditingTenantId { get; set; }
    public string SearchString;
    public string HeaderTitle = "Tenant";
    public TDModel NewModal;
    public TDModel EditingModal;
    public TDModel ClaimModal;
    public Dictionary<string,List<ClaimItem>> Claims { get; set; }
    public bool CanAuthorize { get; set; }
    


     public Tenant()
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
                    HeaderTitle = L["Tenant"];
                    await SetAuthorizedAction();
                    await GetTenants();
                    StateHasChanged();
                }, ActionTypes.GetList);
             
            }
        }

    
        
        public async Task SetAuthorizedAction()
        {
            var claims = await GetClaims(ExtendClaimTypes.Permission);
            CanCreate = claims.Any(x => x == AccessClaims.Tenants.Create);
            CanEdit = claims.Any(x => x == AccessClaims.Tenants.Edit);
            CanCreate = claims.Any(x => x == AccessClaims.Tenants.Create);
        }
        public async Task GetTenants()
        {
            Tenants = await _tenantService.GetListAsync();
        }

        
        async void OnClick(RadzenSplitButtonItem item,TenantDto tenant)
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
                       await  ShowEditingModal(tenant);
                        break;
                    }
                    case FormActions.Claim:
                    {
                        await ShowClaim(tenant.Id);
                        break;
                    }
                }
            }
        }
        
        
        
        public async Task CreateTenant()
        {
            await InvokeAsync(async () =>
            {
                await _tenantService.CreateAsync(input: NewTenant);
                HideNewModal();
                await GetTenants();
            }, ActionTypes.Create, true);
        }
        
        public async Task UpdateTenant()
        {
            await InvokeAsync(async () =>
            {
                await _tenantService.UpdateAsync(EditingTenant, EditingTenantId);
                HideEditingModal();
                await GetTenants();
            },ActionTypes.Update,true);
            
        }
        
        public async Task DeleteTenant(Guid id)
        {
            await InvokeAsync(async () =>
            {
                await _tenantService.DeleteAsync(id);
                HideEditingModal();
                await GetTenants();
            },ActionTypes.Delete,true);
            
        }

 

        public async void ShowNewModal()
        {
            NewTenant = new CreateUpdateTenantDto();
            await NewModal.ShowModel();
        }
        
        public void HideNewModal()
        {
            NewModal.HideModel();
        }
        
        
        public Task ShowEditingModal(TenantDto tenantDto)
        {
            EditingTenant = new CreateUpdateTenantDto();
            EditingTenant = ObjectMapper.Map<TenantDto, CreateUpdateTenantDto>(tenantDto);
            EditingTenantId = tenantDto.Id;
            return EditingModal.ShowModel();
        }
        
        public void HideEditingModal()
        {
            EditingModal.HideModel();
        }

        public  async Task ShowClaim(Guid id)
        {
           Claims =  SecurityHelper.GetClaims();
           // var tenantClaims = await _tenantService.GetClaims(id);
           // MarkClaimsSelection(tenantClaims);
           EditingTenantId = id;
           await ClaimModal.ShowModel();
        }

        public void MarkClaimsSelection(List<ClaimDto> tenantClaims)
        {
          var items = Claims
                .SelectMany(x => x.Value);

          foreach (var item in items)
          {
              if (tenantClaims.Any(x => x.Value == item.Value))
              {
                  item.IsSelected = true;
              }
          } 
         
        }
        
        
        public async Task HideClaim()
        {
            await ClaimModal.ShowModel();
        }

        // public async Task UpdateClaimsToTenant()
        // {
        //     var claims = new List<CreateUpdateClaimDto>();
        //     var selectedClaims = Claims.SelectMany(x => x.Value)
        //         .Where(x => x.IsSelected).ToList();
        //     foreach (var item in selectedClaims)
        //     {
        //         claims.Add(new CreateUpdateClaimDto()
        //         {
        //             ClaimType = ExtendClaimTypes.Permission,
        //             ClaimValue = item.Value
        //         });
        //     }
        //     await InvokeAsync(async () =>
        //     {
        //         await _tenantService.UpdateAsync(claims,EditingTenantId);
        //         HideClaim();
        //     },ActionTypes.Update,true);
        //
        // }
}