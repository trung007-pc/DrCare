using System.Security.Claims;
using AutoMapper;
using FluentDateTimeOffset;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Todo.Contract.Extensions;
using Todo.Core.Consts.Permissions;
using Todo.Core.Enums;
using Todo.Localziration;
using WebClient.Exceptions;
using DateRange = BlazorDateRangePicker.DateRange;

namespace Todo.AdminBlazor.Shared;

public class BaseBlazorPage : ComponentBase
{
      [Inject] private NavigationManager _navigationManager { get; set;}
      [Inject] public ISnackbar Snackbar { get; set; }  

      public   IEnumerable<int> PageSizeOptions = new int[] { 5,10, 20, 30 };

        [CascadingParameter]
        public Task<AuthenticationState> AuthState { get; set; }
        
        [Inject] public Localizer  L { get; set; } 
        protected IMapper ObjectMapper { get;}
        public bool IsDisable  { get; set; }
        
        public bool CanEdit;
        public bool CanCreate;
        public bool CanDelete;
    

        public BaseBlazorPage()
        {
            
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            ObjectMapper = config.CreateMapper();
            
        }
        

        public async Task<bool> InvokeAsync(Func<Task> action, ActionTypes type, bool showNotification = false)
        {

            try
            {
                IsDisable = true;
                await action();

                if (showNotification)
                {
                    switch (type)
                    {
                        case ActionTypes.Create:
                        {
                            NotifyMessage(L["Notification.Create"],Color.Success,Severity.Success);
                            break;
                        }
                        case ActionTypes.Update:
                        {
                            NotifyMessage(L["Notification.Update"],Color.Success,Severity.Success);
                            break;
                        }
                        case ActionTypes.Get:
                        {
                            NotifyMessage(L["Notification.Get"],Color.Success,Severity.Success);

                            break;
                        }
                        case ActionTypes.Send:
                        {
                            NotifyMessage(L["Notification.Send"],Color.Success,Severity.Success);

                            break;
                        }
                        case ActionTypes.GetList:
                        {
                            NotifyMessage(L["Notification.GetList"],Color.Success,Severity.Success);

                            break;
                        }
                        case ActionTypes.Delete:
                        {
                            NotifyMessage(L["Notification.Delete"],Color.Success,Severity.Success);

                            break;
                        }
                        case ActionTypes.SignIn:
                        {
                            NotifyMessage(L["Notification.SignIn"],Color.Success,Severity.Success);

                            break;
                        }
                        case ActionTypes.SignOut:
                        {
                            NotifyMessage(L["Notification.SignOut"],Color.Success,Severity.Success);

                            break;
                        }
                        case ActionTypes.SignUp:
                        {
                            NotifyMessage(L["Notification.Create"],Color.Success,Severity.Success);

                            break;
                        }

                        case ActionTypes.Review:
                        {
                            NotifyMessage(L["Notification.Review"],Color.Success,Severity.Success);

                            break;
                        }

                        case ActionTypes.UploadFile:
                        {
                            NotifyMessage(L["Notification.UploadFile"],Color.Success,Severity.Success);

                            break;
                        }

                        case ActionTypes.Reset:
                        {
                            NotifyMessage(L["Notification.Create"],Color.Success,Severity.Success);
                            break;
                        }
                        
                        case ActionTypes.LoadData:
                        {
                            break;
                        }

                    }
                }

               StateHasChanged();
                return true;
            }
            catch (Exception e)
            {
                var exceptionType = e.GetType();
                
                if (exceptionType == typeof(NotFoundException))
                {
                    NotifyMessage("Không tìm thấy địa chỉ cuộc gọi",Color.Secondary,Severity.Error);
                }
                
                else if (exceptionType == typeof(BadRequestException))
                {
                    NotifyMessage(e.Message,Color.Secondary,Severity.Error);
                }

                else if (exceptionType == typeof(UnauthorizedOperationException))
                {
                    NotifyMessage(e.Message,Color.Secondary,Severity.Error);
                }
                
                else if (exceptionType == typeof(UnauthorizedException))
                {
                    NotifyMessage(e.Message,Color.Secondary,Severity.Error);

                    Thread.Sleep(4000);
                    
                    _navigationManager.NavigateTo($"login?ToURl={_navigationManager.Uri}", true);
                }

                else if (exceptionType == typeof(ServerErrorException))
                {
                    _navigationManager.NavigateTo("server-error", true);
                }

                else if (exceptionType == typeof(DbConnectionException))
                {
                    _navigationManager.NavigateTo("connection-error", true);
                }

                else if (exceptionType == typeof(ConflictException))
                {
                    NotifyMessage(e.Message,Color.Secondary,Severity.Error);

                }

                else if (exceptionType == typeof(TooManyRequests))
                {
                    // too many request
                }
                
                else if (exceptionType == typeof(FailedOperation))
                {
                    NotifyMessage(e.Message,Color.Secondary,Severity.Error);


                }

                else if (exceptionType == typeof(HttpRequestException))
                {
                    NotifyMessage(e.Message,Color.Secondary,Severity.Error);

                }
                else
                {
                    NotifyMessage("Có lỗi ở phía client vui lòng báo lại cho IT",Color.Secondary,Severity.Error);
                }


            }finally
            {
                IsDisable = false;
            }

            return false;

        }

        public void NotifyMessage(string message,Color color,Severity severity )
        {
            Snackbar.Add(message, severity, config =>
            {
                config.Icon = Icons.Custom.Brands.GitHub;
                config.IconSize = Size.Large;
            });
        }


        protected async Task<Dictionary<string, DateRange>> GetDateRangePickers()
        {
            
            var now = new DateTimeOffset(DateTime.Now.Date);
            
            var ranges =  new Dictionary<string, DateRange>()
          {
              {
                  DateRangeTypes.Today.GetDescriptionOrName(),
                  new DateRange()
                  {
                      Start = now,
                      End = now.Add(new TimeSpan(23,59,59))
                  }
              },
              {
                  DateRangeTypes.Yesterday.GetDescriptionOrName(),
                  new DateRange()
                  {
                      Start = now.AddDays(-1),
                      End = now.AddDays(-1).Add(new TimeSpan(23,59,59))
                  }
              },
              {
                  DateRangeTypes.Last7Days.GetDescriptionOrName(),
                  new DateRange()
                  {
                      Start = now.AddDays(-7).Add(new TimeSpan(23,59,59)),
                      End = now
                  }
              },
              {
                  DateRangeTypes.Last30Days.GetDescriptionOrName(),
                  new DateRange()
                  {
                      Start = now.AddDays(-30).Add(new TimeSpan(23,59,59)),
                      End = now
                  }
              },
              {
                  DateRangeTypes.ThisMonth.GetDescriptionOrName(),
                  new DateRange()
                  {
                      Start = now.FirstDayOfMonth(),
                      End = now.LastDayOfMonth()
                  }
              },
              {
                  DateRangeTypes.LastMonth.GetDescriptionOrName(),
                  new DateRange()
                  {
                      Start = now.AddMonths(-1).FirstDayOfMonth(),
                      End = now.AddMonths(-1).LastDayOfMonth()
                  }
              },
              {
                  DateRangeTypes._3MonthsAgo.GetDescriptionOrName(),
                  new DateRange()
                  {
                      Start = now.AddMonths(-3).FirstDayOfMonth(),
                      End = now.Add(new TimeSpan(23,59,59))
                  }
              }
              
          
          };

          
          

          return ranges;

        }

        protected async Task<(Dictionary<string, DateRange>DateRanges,DateTimeOffset StartDay,DateTimeOffset EndDay)>
            GetDateRangePickersWithDefault()
        {
           var dateRanges = await GetDateRangePickers();
            
            var dateRange = dateRanges.
                FirstOrDefault(x => x.Key ==  DateRangeTypes._3MonthsAgo.GetDescriptionOrName()).Value;

            return (dateRanges, dateRange.Start, dateRange.End);
        }

        protected (DateTime?,DateTime?) GetDateTimeFromOffSet(DateTimeOffset? fromDateOffset, DateTimeOffset? toDateTimeOffset)
        {
            if (!fromDateOffset.HasValue || !toDateTimeOffset.HasValue) return (null, null);
            return (fromDateOffset.Value.DateTime, toDateTimeOffset.Value.DateTime);
        }
        
        

        public async Task<string> GetUserNameAsync()
        {
            var authState = await AuthState;
            var user = authState.User;

            return user.Identity.Name;
        }
        
        public async Task<Guid> GetUserIdAsync()
        {
            var authState = await AuthState;
            var user = authState.User;
            return Guid.Parse(user.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value);
        }
        


        public async Task<List<string>> GetClaims(string type)
        {
            var authState = await AuthState;
            var user = authState.User;
            var roleClaims = user.Claims
                .Where(x => x.Type == type);
            return roleClaims.Select(x => x.Value).ToList();
        }
  

        public async Task<bool> IsAuthenticatedAsync()
        {
            var authState = await AuthState;
            var user = authState.User;
            if (user.Identity.IsAuthenticated)
            {
                return true;
            }

            return false;
        }
   
        
}

public class  FormActions
{
    public const string Create = "Create";
    public const string Edit = "Edit";
    public const string Claim = "Permission";
    public const string Delete = "Delete";
}

