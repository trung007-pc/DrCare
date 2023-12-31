﻿namespace Todo.Core.Consts.ErrorCodes;

public class BaseErrorCode
{
    public const string NotFound = "Api:0001";
    public const string ItemAlreadyExist = "Api:0002";
    public const string EmailAlreadyExist = "Api:0003";
    public const string InvalidRequirement = "Api:0004";
    public const string TryMore = "Api:0005";
    public const string InvalidTimeRange = "Api:0006";


}

public class UserErrorCode
{
    public const string WrongPassword = "User:0001";
    public const string UserNameAlreadyExist = "User:0002";

}

public class UploadErrorCode
{
    public const string InvalidExtension = "Upload:0001";
}

public class RoleErrorCode 
{
    public const string AlreadyExist = "Role:0001";
}

public class TenantErrorCode{
    
    
    public const string PhoneNumberAlreadyExist = "Tenant:0001";
    public const string NameAlreadyExist = "Tenant:0002";

}
