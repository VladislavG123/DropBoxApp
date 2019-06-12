using SecurityApplication.Services;
using System.Data.Entity;

namespace SecurityApplication.DataAccess
{
    class DataInitializer : CreateDatabaseIfNotExists<AppContext>
    {
        protected override void Seed(AppContext context)
        {
            context.Users.Add(new Models.User
            {
                Login = "admin",
                Password = SecurityHasher.HashPassword("123")
            });
        }
    }
}