namespace finalDotNet
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class finalModel : DbContext
    {
        public finalModel()
            : base("name=finalModel3")
        {
        }

        public virtual DbSet<noticetable> noticetable { get; set; }
        public virtual DbSet<usertable> usertable { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<usertable>()
                .HasMany(e => e.noticetable)
                .WithRequired(e => e.usertable)
                .HasForeignKey(e => e.author);
        }
    }
}
