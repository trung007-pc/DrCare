namespace Todo.MongoDb;

public class DatabaseSettings
{
    
    public string UsersCollectionName { get; set; }
    public string RolesCollectionName { get; set; }
    public string UserRolesCollectionName { get; set; }
    public string RoleClaimsCollectionName { get; set; }
    
    
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}