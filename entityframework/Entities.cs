using System.Data.Entity;

namespace entityframework
{
    class Entities : DbContext
    {
        public Entities():base("DbConnection") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Base> Bases { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Squad> Squads { get; set; }

        /*protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<Entities>(null);
            base.OnModelCreating(modelBuilder);
        }*/
    }
}