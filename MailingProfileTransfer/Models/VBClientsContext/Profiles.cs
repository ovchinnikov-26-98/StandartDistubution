namespace MailingProfileTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    /// <summary>
    /// Класс рассылок
    /// </summary>
    public partial class Profiles : IProfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Profiles()
        {
            Addresses = new HashSet<Addresses>();
            ConfirmationEmailAddresses = new HashSet<ConfirmationEmailAddresses>();
            MailingOptions = new HashSet<MailingOptions>();
            Profiles_Customs = new HashSet<Profiles_Customs>();
            Profiles_Pins = new HashSet<Profiles_Pins>();
            Profiles_Tins = new HashSet<Profiles_Tins>();
        }

        public int id { get; set; }

        public int id_User { get; set; }

        public byte id_AddressType { get; set; }

        public bool? SendAllTinsExcludingList { get; set; }

        public bool? SendAllCustomsExcludingList { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public DateTime? ModifiedTime { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public int LoginModified { get; set; }

        public bool? SendAllPinsExcludingList { get; set; }

        public int? MailingType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Addresses> Addresses { get; set; }

        public virtual AddressType AddressType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConfirmationEmailAddresses> ConfirmationEmailAddresses { get; set; }
        [ForeignKey("LoginModified")]
        public virtual Login Login { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MailingOptions> MailingOptions { get; set; }
        [ForeignKey("id_User")]
        public virtual Users User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Profiles_Customs> Profiles_Customs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Profiles_Pins> Profiles_Pins { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Profiles_Tins> Profiles_Tins { get; set; }

        /// <summary>
        /// Метод выводит описание рассылки в консоль.
        /// </summary>
        /// <param name="color"></param>
        public void Description(ConsoleColor color = ConsoleColor.DarkGray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine("Детализация по профилю.");
            Console.WriteLine($"\tРасположение:    VBSiteClientSettings");
            Console.WriteLine($"\tНомер профиля:    {id}");
            Console.WriteLine($"\tНазвание профиль: {Name}");
            Console.WriteLine($"\tВН компании:      {User.PIN}");
            Console.WriteLine($"\tНазвание компании:{User.Name}");
            Console.WriteLine($"\tБыл изменён:      ");
            Console.WriteLine($"\tДата последнего изменения: {ModifiedTime}");
            Console.WriteLine("\tСписок контактов, на которые уходит рассылка профиля:");
            foreach (var contact in Addresses)
            {
                Console.WriteLine($"\t\t{contact.Address}");
            }
            Console.WriteLine("\tСписок ИНН, по которым фильтруется рассылка профиля:");
            foreach (var tin in Profiles_Tins)
            {
                Console.WriteLine($"\t\t{tin.Tin}");
            }
            Console.WriteLine();
            Console.ResetColor();
        }
        /// <summary>
        /// редактирование рассылки
        /// </summary>
        /// <param name="email"></param>
        /// <param name="tins"></param>
        public void Edit(EmailCollection _email, TinsCollection tins)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            EmailCollection email = _email.Clone();
            Console.WriteLine("Проверьте планируемые изменения.");
            /* Console.WriteLine("Список планируемых ИНН для фильтрации:");

             foreach (var item in Profiles_Tins.Select(x => x.Tin).ToList())
             {
                 if (tins.delItems.Contains(item) || tins.DeleteAll)
                 {
                     Console.ForegroundColor = ConsoleColor.Red;
                 }
                 else
                 {
                     Console.ForegroundColor = ConsoleColor.DarkGray;

                 }
                 Console.WriteLine($"\t{item}");
             }
             Console.ForegroundColor = ConsoleColor.Green;
             foreach (var item in tins.newItems)
             {
                 Console.WriteLine($"\t{item}");
             }*/
            email.delItems = email.delItems.Where(x => Addresses.Select(y => y.Address.ToLower()).Contains(x)).ToList();
            email.newItems = email.newItems.Except(Addresses.Select(x => x.Address.ToLower())).ToList();
            if (email.delItems.Count == 0 && email.newItems.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"В рассылке номер {id} нет новых изменений.");
                return;
            }
            Console.WriteLine("Список планируемых адресов для рассылки:");

            

            foreach (var item in Addresses.Select(x => x.Address.ToLower()).ToList())
            {
                if (email.delItems.Contains(item.ToLower()) || email.DeleteAll)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;

                }
                Console.WriteLine($"\t{item.ToLower()}");
            }
            Console.ForegroundColor = ConsoleColor.Green;


            
            foreach (var item in email.newItems)
            {
                
                Console.WriteLine($"\t{item.ToLower()}");
            }
            


            Console.ResetColor();
            if (!Helper.Accept())
                return;

          //  EditTins(tins);
            EditEmail(email);
        }


        /// <summary>
        /// Редактирование списка ИНН
        /// </summary>
        /// <param name="tins"></param>
        public void EditTins(TinsCollection tins)
        {
            


            foreach (var item in tins.delItems)
            {
                Profiles_Tins contactToDel = Profiles_Tins.FirstOrDefault(
                            x => x.Tin == item);
                if (contactToDel != null)
                {
                    Profiles_Tins.Remove(contactToDel);
                }
            }
            if (tins.DeleteAll)
            {
                Profiles_Tins.Clear();
            }
            foreach (var addTin in tins.newItems)
            {
                Profiles_Tins.Add(new Profiles_Tins { Tin = addTin });

            }

            ModifiedTime = DateTime.Now;
            Description(ConsoleColor.DarkGreen);
        }
        /// <summary>
        /// редактирование списка емаилов
        /// </summary>
        /// <param name="email"></param>
        public void EditEmail(EmailCollection email)
        {
            
            foreach (var item in email.delItems)
            {
                Addresses contactToDel = Addresses.FirstOrDefault(
                            x => x.Address == item);
                if (contactToDel != null)
                {
                    Addresses.Remove(contactToDel);
                }
            }
            if (email.DeleteAll)
            {
                Addresses.Clear();
            }
            foreach (var addEmail in email.newItems)
            {
                Addresses.Add(new Addresses { Address = addEmail });

            }

            ModifiedTime = DateTime.Now;
            Description(ConsoleColor.DarkGreen);
        }
        /// <summary>
        /// заполнение профиля
        /// </summary>
        /// <param name="emails"></param>
        /// <param name="tins"></param>
        public void FillProfile(EmailCollection emails, TinsCollection tins)
        {
            List<Addresses> e = new List<Addresses>();
            foreach (string email in emails.newItems)
            {
                e.Add(new Addresses() { Address = email });
            }

            List<Profiles_Tins> t = new List<Profiles_Tins>();
            foreach (string tin in tins.newItems)
            {
                t.Add(new Profiles_Tins() { Tin = tin });
            }

            List<MailingOptions> m = new List<MailingOptions>();
            m.Add(new MailingOptions() { OptionNumber = 1, Value = 1 });
            m.Add(new MailingOptions() { OptionNumber = 3, Value = 1 });
            m.Add(new MailingOptions() { OptionNumber = 3, Value = 2 });
            m.Add(new MailingOptions() { OptionNumber = 3, Value = 3 });
            m.Add(new MailingOptions() { OptionNumber = 3, Value = 5 });
            m.Add(new MailingOptions() { OptionNumber = 3, Value = 6 });


            MailingOptions = m;
            Addresses = e;
            Profiles_Tins = t;
        }
    }
}
