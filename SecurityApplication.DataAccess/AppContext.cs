namespace SecurityApplication.DataAccess
{
    using SecurityApplication.Models;
    using System.Data.Entity;

    public class AppContext : DbContext
    {
        public AppContext()
            : base("name=AppContext")
        {
            Database.SetInitializer(new DataInitializer());
        }

        public DbSet<User> Users { get; set; }
    }
}