namespace Todo.Contract.Tenants;

public class CreateUpdateTenantDto
{
    public Guid Id { get; set;}
    public string Name { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; } = DateTime.Now;
    public string PhoneNumber { get; set; }
    
    public bool IsActive { get; set; }

}