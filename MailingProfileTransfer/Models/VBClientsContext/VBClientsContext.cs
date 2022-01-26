using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace MailingProfileTransfer.Models
{
    /// <summary>
    /// Контекст старой БД
    /// </summary>
    public partial class VBClientsContext : AbstractDbContext
    {
        public VBClientsContext()
            : base($"name={ChoiseConnectionString.ConnectionStringVBClientsName}")
        {
        }

        public virtual DbSet<C_MailingTypes> C_MailingTypes { get; set; }
        public virtual DbSet<C_Roles> C_Roles { get; set; }
        public virtual DbSet<C_Subscribes> C_Subscribes { get; set; }
        public virtual DbSet<Addresses> Addresses { get; set; }
        public virtual DbSet<AddressType> AddressTypes { get; set; }
        public virtual DbSet<AvailableTins> AvailableTins { get; set; }
        public virtual DbSet<ConfirmationEmailAddresses> ConfirmationEmailAddresses { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<Profiles> Profiles { get; set; }
        public virtual DbSet<Profiles_Customs> Profiles_Customs { get; set; }
        public virtual DbSet<Profiles_Tins> Profiles_Tins { get; set; }
        public virtual DbSet<Recipients_Users> Recipients_Users { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<C_VBUsers> C_VBUsers { get; set; }
        public virtual DbSet<MailingNames> MailingNames { get; set; }
        public virtual DbSet<MailingOptions> MailingOptions { get; set; }
        public virtual DbSet<Profiles_Pins> Profiles_Pins { get; set; }
        public virtual DbSet<Users_Pins> Users_Pins { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Addresses>().HasKey(x => new { x.Address, x.id_Profile });
            modelBuilder.Entity<Users_Pins>().HasKey(x => x.PIN);
            modelBuilder.Entity<MailingOptions>().HasKey(x => new { x.id_Profile, x.OptionNumber, x.Value });
            modelBuilder.Entity<C_MailingTypes>().HasKey(x => x.id_Profile);
            modelBuilder.Entity<ConfirmationEmailAddresses>().HasKey(x => x.EmailAddress);
           // modelBuilder.Entity<Recipients_Users>().HasKey(x => x.id_User);
            modelBuilder.Entity<Profiles_Pins>().HasKey(x => x.Pin);
            modelBuilder.Entity<Profiles_Tins>().HasKey(x => new { x.Tin, x.id_Profile });
            modelBuilder.Entity<AvailableTins>().HasKey(x => x.Tin);
            modelBuilder.Entity<Profiles_Customs>().HasKey(x => x.Customs);


            modelBuilder.Entity<MailingNames>().HasKey(x => x.ValueNumber);




            modelBuilder.Entity<C_Roles>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<C_Roles>()
                .HasMany(e => e.C_Subscribes)
                .WithMany(e => e.C_Roles)
                .Map(m => m.ToTable("_Subscribes_Roles").MapLeftKey("id_Role").MapRightKey("id_Subscribe"));

            modelBuilder.Entity<AddressType>()
                .HasMany(e => e.Profiles)
                .WithRequired(e => e.AddressType)
                .HasForeignKey(e => e.id_AddressType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AvailableTins>()
                .Property(e => e.Pin)
                .IsUnicode(false);

            modelBuilder.Entity<AvailableTins>()
                .Property(e => e.Tin)
                .IsUnicode(false);

            modelBuilder.Entity<ConfirmationEmailAddresses>()
                .Property(e => e.EmailAddress)
                .IsUnicode(false);

            modelBuilder.Entity<Login>()
                .HasMany(e => e.Profiles)
                .WithRequired(e => e.Login)
                .HasForeignKey(e => e.LoginModified)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Login>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Logins)
                .Map(m => m.ToTable("Users_Logins").MapLeftKey("id_Login").MapRightKey("id_User"));

          modelBuilder.Entity<Profiles>()
               .HasMany(e => e.Addresses)
                .WithRequired(e => e.Profile)
                .HasForeignKey(e => e.id_Profile);

            modelBuilder.Entity<Profiles>()
                .HasMany(e => e.MailingOptions)
                .WithRequired(e => e.Profile)
                .HasForeignKey(e => e.id_Profile);

            modelBuilder.Entity<Profiles>()
                .HasMany(e => e.Profiles_Customs)
                .WithRequired(e => e.Profile)
                .HasForeignKey(e => e.id_Profile);

            modelBuilder.Entity<Profiles>()
                .HasMany(e => e.Profiles_Pins)
                .WithRequired(e => e.Profile)
                .HasForeignKey(e => e.id_Profile);

            modelBuilder.Entity<Profiles>()
                .HasMany(e => e.Profiles_Tins)
                .WithRequired(e => e.Profile)
                .HasForeignKey(e => e.id_Profile);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Profiles)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.id_User);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Recipients_Users)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.id_User);

            modelBuilder.Entity<Users>()
                .HasOptional(e => e.C_VBUsers)
                .WithRequired(e => e.User);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Users_Pins)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.id_User);

            modelBuilder.Entity<Profiles_Pins>()
                .Property(e => e.Pin)
                .IsUnicode(false);

            modelBuilder.Entity<Users_Pins>()
                .Property(e => e.PIN)
                .IsUnicode(false);
        }
        /// <summary>
        /// Добавить компанию непосредственно в БД. 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="loginName"></param>
        /// <param name="pin"></param>
        /// <returns></returns>
        public virtual int AddUser(string userName, string loginName, string pin)
        {
            var userNameParameter = userName != null ?
                new SqlParameter("userName", userName) :
                new SqlParameter("userName", typeof(string));

            var loginNameParameter = loginName != null ?
                new SqlParameter("loginName", loginName) :
                new SqlParameter("loginName", typeof(string));

            var pinParameter = pin != null ?
                new SqlParameter("pin", pin) :
                new SqlParameter("pin", typeof(string));

            var ccc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec AddUser @userName,@loginName,@pin", userNameParameter, loginNameParameter, pinParameter);
            return 0;
            // var xxx =  ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<int>("AddUser", userNameParameter, loginNameParameter, pinParameter);

        }

        public override ICompany AddCompany(int pin, string compName)
        {
            if (Users.Where(x => x.Name.ToLower() == compName.ToLower()).Count() == 0 && Users.Where(x => x.Users_Pins.Select(y => y.PIN).Contains(pin.ToString())).Count() == 0)
            {
                AddUser(compName, compName, pin.ToString());
                SaveChanges();

            }
            return Users.Where(x => x.Users_Pins.Select(y => y.PIN).Contains(pin.ToString())).FirstOrDefault();
        }

       
    }
}
