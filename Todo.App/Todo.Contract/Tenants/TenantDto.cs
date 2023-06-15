namespace Todo.Contract.Tenants;

public class TenantDto
{
    public Guid Id { get; set;}
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDayDate { get; set; }
}