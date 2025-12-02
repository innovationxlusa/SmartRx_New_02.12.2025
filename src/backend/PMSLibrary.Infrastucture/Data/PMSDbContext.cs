using Microsoft.EntityFrameworkCore;
using PMSBackend.Domain.Entities;
using System.Data;


namespace PMSBackend.Infrastucture.Data
{
    public class PMSDbContext : DbContext
    {      
        DbConnector connector;
        public PMSDbContext(DbContextOptions<PMSDbContext> options) : base(options)
        {

        }  

        public DbSet<SmartRxUserEntity> PMSUsers { get; set; }
        public DbSet<SmartRxRoleEntity> Roles { get; set; }
        public DbSet<UserRoleEntity> UserRoles { get; set; }

        public DbSet<PrescriptionUploadEntity> Prescription_UploadedPrescription { get; set; }
        public DbSet<PrescriptionSequenceEntity> Prescription_UploadedFileSequence { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {          

            // Many-to-Many User <-> Role (through UserRole)
            modelBuilder.Entity<UserRoleEntity>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRoleEntity>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRoleEntity>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<UserRoleEntity>().ToTable("UserRoles");

            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<SmartRxRole>().HasData(
            //    new SmartRxRole { Id = 1, Name = "Super Admin", IsSelfService = false, Description = "Super Admin is the only one user in the system and all super access has in it and to recover the system only this user can be usable " },
            //    new SmartRxRole { Id = 2, Name = "Admin", IsSelfService = false, Description = "Admin is to manipulated all general access and manage system" },
            //    new SmartRxRole { Id = 3, Name = "Entry User", IsSelfService = true, Description="An admin user who can enter all data into the system" },
            //    new SmartRxRole { Id = 4, Name = "Approver", IsSelfService = true },
            //    new SmartRxRole { Id = 5, Name = "Recommender", IsSelfService = true },
            //    new SmartRxRole { Id = 6, Name = "External User", IsSelfService = false, Description = "Only outside user are in this list" }
            //);

        }
    }
}
