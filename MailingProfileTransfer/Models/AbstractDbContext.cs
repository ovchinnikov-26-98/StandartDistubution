using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace MailingProfileTransfer.Models
{
    /// <summary>
    /// Абстрактный класс подключения к БД
    /// </summary>
    public abstract class AbstractDbContext  : DbContext, IChek
    {
        public AbstractDbContext(string connectionString) : base(connectionString)
        {

        }

        public string DatabaseName => base.Database.Connection.Database;

        public string ServerName => base.Database.Connection.DataSource;

        public bool Exists => base.Database.Exists();
              

        public abstract ICompany AddCompany(int pin, string compName);
     

       // public abstract void EditProfile(int id, EmailCollection emails, TinsCollection tins);
    }
}
