namespace MailingProfileTransfer.Models
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    /// <summary>
    /// Компании.
    /// </summary>
    public partial class Users : ICompany
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            Profiles = new HashSet<Profiles>();
            Recipients_Users = new HashSet<Recipients_Users>();
            Users_Pins = new HashSet<Users_Pins>();
            Logins = new HashSet<Login>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string ContractNumber { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Profiles> Profiles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recipients_Users> Recipients_Users { get; set; }

        public virtual C_VBUsers C_VBUsers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Users_Pins> Users_Pins { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Login> Logins { get; set; }
        /// <summary>
        /// Добавить рассылку
        /// </summary>
        /// <param name="prName"></param>
        /// <param name="emails"></param>
        /// <param name="tins"></param>
        /// <param name="events"></param>
        public void AddProfile(string prName, EmailCollection emails, TinsCollection tins, List<Events> events)
        {


            Profiles pr = new Profiles()
            {
                id_User = id,
                id_AddressType = 1,
                SendAllTinsExcludingList = true,
                Name = prName + " сканы документов",
                IsActive = true,
                IsDeleted = false,
                LoginModified = 433,
                MailingType = 3,
                SendAllCustomsExcludingList = true
            };
            Profiles.Add(pr);

            pr.FillProfile(emails, tins);

        }
        /// <summary>
        /// Выводит строку из всех привязанных пинов
        /// </summary>
        public string PIN { get 
            {
                string result = "";
                foreach (var pin in Users_Pins)
                    result += pin.PIN;
                return result;
            } 
        }
        /// <summary>
        /// Описание в консоль
        /// </summary>
        public void Description()
        {
            Console.WriteLine("Общая информация о компании:");
            Console.WriteLine($"Наименование компании: {Name}");
            
            
        }
        /// <summary>
        /// Описание рассылок в консоль
        /// </summary>
        public void ProfilesDescription()
        {
            
            foreach (Profiles pr in Profiles.Where(x => x.MailingType == 3))
            {
                pr.Description();
            }
        }

    }
}
