using Microsoft.EntityFrameworkCore;
class Tododb:DbContext{
    public Tododb(DbContextOptions<Tododb> options):base(options){}
    public DbSet<Todo> Todos => Set<Todo>();

}