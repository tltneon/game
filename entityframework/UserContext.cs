using System.Data.Entity;

namespace entityframework
{
    class UserContext : DbContext
    {
        public UserContext():base("DbConnection") { }

        public DbSet<User> Users { get; set; }
    }
}