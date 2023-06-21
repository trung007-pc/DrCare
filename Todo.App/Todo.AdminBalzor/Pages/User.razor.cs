
using Radzen;
using Radzen.Blazor;
using Todo.AdminBlazor.Components;
using Todo.AdminBlazor.Helper;
using Todo.AdminBlazor.Shared;
using Todo.Contract.Claims;
using Todo.Contract.Roles;
using Todo.Contract.Users;
using Todo.Core.Consts.Permissions;
using Todo.Core.Enums;


namespace Todo.AdminBlazor.Pages;

public partial class User
{
        public List<UserWithNavigationPropertiesDto> Users { get; set; } = new List<UserWithNavigationPropertiesDto>();
        public CreateUserDto NewUser { get; set; } = new CreateUserDto();
        public UpdateUserDto EditingUser { get; set; } = new UpdateUserDto();
        public List<RoleDto> Roles = new List<RoleDto>();
        public IEnumerable<Guid> SelectedRoleIds = new List<Guid>();
        public Dictionary<string,List<ClaimItem>> Claims { get; set; }

        public Guid EditingUserId { get; set; }

        public TDModel NewModal = new TDModel();
        public TDModel EditingModal = new TDModel();
        public TDModel DeletingModal = new TDModel();
        public TDModel ClaimModal;

        
        public string HeaderTitle = "User";
        public bool IsLoading { get; set; } = true;
        public UserClaimsDto UserClaimsDto { get; set; }
        public bool CanAuthorize { get; set; }

        
        

        public User()
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
                    await SetAuthorizedAction();
                    await GetUsers();
                    await GetRoles();
                    IsLoading = false;
                    StateHasChanged();
                }, ActionTypes.LoadData, false);


            }
        }

        public async Task SetAuthorizedAction()
        {
            var claims = await GetClaims(ExtendClaimTypes.Permission);
            CanCreate = claims.Any(x => x == AccessClaims.Users.Create);
            CanEdit = claims.Any(x => x == AccessClaims.Users.Edit);
            CanDelete = claims.Any(x => x == AccessClaims.Users.Delete);
            CanAuthorize = claims.Any(x => x == AccessClaims.Roles.Authorize);
            

        }
        public async Task GetUsers()
        {
            Users = await _userService.GetListWithNavigationAsync();
        }

        public async Task GetRoles()
        {
            Roles = await _roleService.GetListAsync();
        }

        public async Task CreateUser()
        {
            await InvokeAsync(async () =>
            {
                NewUser.UserName = NewUser.PhoneNumber;
                await _userService.CreateWithNavigationPropertiesAsync(NewUser);
                HideNewModal();
                await GetUsers();
            }, ActionTypes.Create, true);
        }

        public async Task UpdateUser()
        {
            await InvokeAsync(async () =>
            {
                EditingUser.RoleIds = SelectedRoleIds.ToList();
                await _userService.UpdateWithNavigationPropertiesAsync(EditingUser, EditingUserId);
                await GetUsers();
                HideEditingModal();
                
            }, ActionTypes.Update, true);
        }

        
        async  void OnClick(RadzenSplitButtonItem item,UserWithNavigationPropertiesDto userWithNavigationProperties)
        {
            if (item != null)
            {
                switch (item.Value)
                {
                    
                    case FormActions.Delete:
                    {
                        ShowDeletingModal(userWithNavigationProperties.User.Id);
                        break;
                    }
                    case FormActions.Edit:
                    {
                          ShowEditingModal(userWithNavigationProperties);
                        break;
                    }
                    case FormActions.Claim:
                    {
                        await ShowClaim(userWithNavigationProperties.User.Id);
                        break;
                    }
                }
            }
        }
        
        public  async Task ShowClaim(Guid id)
        {
             Claims =  SecurityHelper.GetClaims();
            UserClaimsDto = await _userService.GetClaimsAndRoleClaims(id);
            MarkClaimsSelection(UserClaimsDto.RoleClaims,UserClaimsDto.UserClaims);
            EditingUserId = id;
            await ClaimModal.ShowModel();
        }
        
        
        public void MarkClaimsSelection(List<ClaimDto> userRoleClaims,List<ClaimDto> userClaims)
        {
            var items = Claims
                .SelectMany(x => x.Value);

            foreach (var item in items)
            {
                if (userRoleClaims.Any(x => x.Value == item.Value))
                {
                    item.IsSelected = true;
                    item.IsDisabled = true;
                }

                if (userClaims.Any(x => x.Value == item.Value))
                {
                    item.IsSelected = true;
                    item.IsDisabled = false;
                }
            } 
         
        }
        
        
        public async Task HideClaim()
        {
            await ClaimModal.ShowModel();
        }

        public async Task UpdateClaimsToUser()
        {
            var claims = new List<CreateUpdateClaimDto>();
            
            var selectedClaims = Claims.SelectMany(x => x.Value)
                .Where(x => x.IsSelected && !x.IsDisabled).ToList();
            
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
               await _userService.UpdateUserClaims(EditingUserId, claims);
            },ActionTypes.Update,true);
        }

        public async Task ShowDeletingModal(Guid id)
        {
            if (await DeletingModal.ShowConfirmModal() == true)
            {
                await DeleteUser(id);
            }
        }
        
        public async Task DeleteUser(Guid id)
        {
            await InvokeAsync(async () =>
            {
                await _userService.DeleteAsync(id);
                await GetUsers();
            },ActionTypes.Delete,true );
        }
        
        
        public async void ShowNewModal()
        {
            NewUser = new CreateUserDto();
            SelectedRoleIds = new List<Guid>();
          
            await NewModal.ShowModel();
        }
        
        
        
        
        public void HideNewModal()
        {
            NewModal.HideModel();
        }
        
        
        public void HideEditingModal()
        {
             EditingModal.HideModel();
        }
   
        public async void ShowEditingModal(UserWithNavigationPropertiesDto userWithNavigationPropertiesDto)
        {
            
            EditingUser = new UpdateUserDto();
            SelectedRoleIds = new List<Guid>();
            
            SelectedRoleIds = userWithNavigationPropertiesDto.Roles.Select(x=>x.Id);
            EditingUserId = userWithNavigationPropertiesDto.User.Id;
            EditingUser = ObjectMapper.Map<UserDto, UpdateUserDto>(userWithNavigationPropertiesDto.User);
            EditingUser.RoleIds = userWithNavigationPropertiesDto.Roles.Select(x=>x.Id).ToList();
            await EditingModal.ShowModel();
        }


        public BadgeStyle ChooseColorByNumber(int i)
        {
            
            switch (i)
            {
                case 1:
                {
                    return BadgeStyle.Info;
                }
                case 2:
                {
                    return BadgeStyle.Primary;
                }
                case 3:
                {
                    return BadgeStyle.Success;
                }
                case 4:
                {
                    return BadgeStyle.Light;
                }
                default:
                {
                    return BadgeStyle.Danger;
                }
            }
        }

        public void OnSelectedRoleChange()
        {
            
        }
      
    



}