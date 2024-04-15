namespace EntityFrameworkCore.Domain;

public class Coach : BaseDomainModel
{
    public int Id { get; set; } // 기본키
    public string Name { get; set; }
}
