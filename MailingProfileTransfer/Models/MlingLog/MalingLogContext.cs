using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace MailingProfileTransfer.Models
{
    public partial class MalingLogContext : DbContext, IChek
    {
        public MalingLogContext()
            : base($"name={ChoiseConnectionString.ConnectionStringMailingLog}")
        {
        }

        
        public virtual DbSet<MailingLog> MailingLog { get; set; }

        public string DatabaseName => base.Database.Connection.Database;

        public string ServerName => base.Database.Connection.DataSource;

        public bool Exists => base.Database.Exists();

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MailingLog>()
                .Property(e => e.ToAddress)
                .IsUnicode(false);

            modelBuilder.Entity<MailingLog>()
                .Property(e => e.Header)
                .IsUnicode(false);

            modelBuilder.Entity<MailingLog>()
                .Property(e => e.Body)
                .IsUnicode(false);
        }

        
    }
}
