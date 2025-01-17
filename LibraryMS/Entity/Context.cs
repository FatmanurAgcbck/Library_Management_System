using Microsoft.EntityFrameworkCore;

namespace LibraryMS.Entity
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context>options) : base(options){}
        public DbSet<Book> Books => Set<Book>();

    }   

}