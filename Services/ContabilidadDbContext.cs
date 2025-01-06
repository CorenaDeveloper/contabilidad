using Microsoft.EntityFrameworkCore;


public class ContabilidadDbContext : DbContext
{
    public ContabilidadDbContext(DbContextOptions<ContabilidadDbContext> options)
       : base(options)
    {
    }
}