namespace Todo.Contract.Tenants;

public class TenantDto
{
    public Guid Id { get; set;}
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; }
}