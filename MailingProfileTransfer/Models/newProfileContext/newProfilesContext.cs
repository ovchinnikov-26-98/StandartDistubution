namespace MailingProfileTransfer.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    /// <summary>
    /// Контекст данных новой базы
    /// </summary>
    public partial class newProfilesContext : AbstractDbContext
    {
        public newProfilesContext()
            : base($"name={ChoiseConnectionString.ConnectionStringName}")
        {
        }

        public virtual DbSet<Companies> Companies { get; set; }
        public virtual DbSet<Contacts> Contacts { get; set; }
        public virtual DbSet<ContactTypes> ContactTypes { get; set; }
        public virtual DbSet<Customs> Customs { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<MailingProfiles> MailingProfiles { get; set; }
        public virtual DbSet<MailingTypes> MailingTypes { get; set; }
        public virtual DbSet<Templates> Templates { get; set; }
        public virtual DbSet<Tins> Tins { get; set; }
        /// <summary>
        /// Метод добавления компании
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="compName"></param>
        /// <returns></returns>
        public override ICompany AddCompany(int pin, string compName)
        {
                if (Companies.FirstOrDefault(c => c.Pin == pin) == null)
                {
                    Companies comp = new Companies()
                    {
                        Name = compName,
                        Pin = pin
                    };
                    Companies.Add(comp);
                SaveChanges();
                }
                return Companies.FirstOrDefault(c => c.Pin == pin);


        }


        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Companies>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<Companies>()
                .HasMany(e => e.MailingProfiles)
                .WithRequired(e => e.Companies)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Contacts>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<Contacts>()
                .HasMany(e => e.MailingProfiles)
                .WithMany(e => e.Contacts)
                .Map(m => m.ToTable("ContactsMailingProfiles"));

            modelBuilder.Entity<ContactTypes>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<Customs>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<Events>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<Events>()
                .HasMany(e => e.MailingProfiles)
                .WithMany(e => e.Events)
                .Map(m => m.ToTable("EventsMailingProfiles"));

            modelBuilder.Entity<MailingProfiles>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<MailingProfiles>()
                .HasMany(e => e.Customs)
                .WithRequired(e => e.MailingProfiles)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MailingProfiles>()
                .HasMany(e => e.Tins)
                .WithRequired(e => e.MailingProfiles)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MailingTypes>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<MailingTypes>()
                .HasMany(e => e.Events)
                .WithRequired(e => e.MailingTypes)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MailingTypes>()
                .HasMany(e => e.MailingProfiles)
                .WithRequired(e => e.MailingTypes)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MailingTypes>()
                .HasMany(e => e.Templates)
                .WithRequired(e => e.MailingTypes)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Templates>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<Templates>()
                .HasMany(e => e.MailingProfiles)
                .WithRequired(e => e.Templates)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tins>()
                .Property(e => e.RowVersion)
                .IsFixedLength();
        }

       

    }
}
