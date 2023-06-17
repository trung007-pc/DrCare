using System.Reflection;
using Todo.Core.Consts.Permissions;

namespace Todo.AdminBlazor.Helper;

public class SecurityHelper
{
    public static Dictionary<string, List<ClaimItem>> GetClaims()
    {
        var permissionValues = new Dictionary<string, List<ClaimItem>>();

        // Lấy kiểu của lớp Permissions
        Type permissionsType = typeof(AccessClaims);

         // Lấy danh sách tất cả các lớp con của Permissions và thêm vào Hashtable
        foreach (Type nestedType in permissionsType.GetNestedTypes())
        {
            var permissionList = new List<ClaimItem>();

            // Lấy danh sách các thành viên tĩnh của lớp con và thêm vào ArrayList
            foreach (var field in nestedType.GetFields())
            {
                permissionList.Add(new ClaimItem()
                {
                    Name = field.Name,
                    Value = (string)field.GetValue(null)
                });
                
            }

            // Thêm ArrayList vào Hashtable với key là tên lớp con
            permissionValues.Add(nestedType.Name, permissionList);
        }

        return permissionValues;
    }
}

public class ClaimItem
{
    public string Name { get; set; }
    public string Value { get; set; }
    public bool IsSelected { get; set; } 
    public bool IsDisabled { get; set; }
}