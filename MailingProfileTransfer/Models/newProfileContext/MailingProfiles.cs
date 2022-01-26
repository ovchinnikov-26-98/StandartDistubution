namespace MailingProfileTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;


    /// <summary>
    /// Центральная таблица модели данных рассылок.
    /// Описывает профиль рассылки.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public partial class MailingProfiles : IProfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MailingProfiles()
        {
            Customs = new HashSet<Customs>();
            Tins = new HashSet<Tins>();
            Contacts = new HashSet<Contacts>();
            Events = new HashSet<Events>();
        }

        [Key]
        public int ProfileID { get; set; }

        [Required]
        public string ProfileName { get; set; }

        public int Pin { get; set; }

        /// <summary>
        /// Тип отправки сообщений.
        /// На 17.07.21 существует 2 типа отправляемых через систему сообщений:
        /// - Регистрация ТСД на ВХ;
        /// - Информация о ТС.
        /// Типы сообщений хранятся в таблице MailingTypes.
        /// </summary>
        public int TypeID { get; set; }

        /// <summary>
        /// Способ пересылки информации.
        /// По состоянию на 17.07.21 отрабатывается только способ доставки через email.
        /// Ссылка на таблицу ContactTypes.
        /// </summary>
        [StringLength(128)]
        public string ContactType { get; set; }

        public int TemplateID { get; set; }

        public bool TinsUsage { get; set; }

        public bool CustomsUsage { get; set; }

        [Required]
        public string ChangedBy { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastChangeTime { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Companies Companies { get; set; }

        public virtual ContactTypes ContactTypes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", 
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customs> Customs { get; set; }

        public virtual MailingTypes MailingTypes { get; set; }

        public virtual Templates Templates { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", 
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tins> Tins { get; set; }

        /// <summary>
        /// Список контактов, на которые идёт рассылка с профиля.
        /// В нашем случае - это список email-адресов.
        /// С контактами связь через промежуточную таблицу - ContactsMailingProfiles.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", 
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contacts> Contacts { get; set; }

        /// <summary>
        /// Список отрабатываемых событий привязанных к профилю.
        /// События и профили связаны через промежуточную таблицу EventsMailingProfiles.
        /// На 17.07.21 существует 6 типов событий:
        /// Въезд;
        /// Машина направлена на Вашутинский ТП;
        /// Завершение транзита;
        /// Регистрация прибытия;
        /// Выезд;
        /// Груз помещён на ВХ.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", 
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Events> Events { get; set; }

        /// <summary>
        /// Вывод полно информации о компании
        /// </summary>
        /// <param name="color"></param>
        public void Description(ConsoleColor color = ConsoleColor.DarkGray)
        {

            Console.ForegroundColor = color;
            Console.WriteLine("Детализация по профилю.");
            Console.WriteLine($"\tРасположение:    MessageDistributionSettings");
            Console.WriteLine($"\tНомер профиля:    {ProfileID}");
            Console.WriteLine($"\tАктивен:    {IsActive}");
            Console.WriteLine($"\tНомер рассылки:    {TypeID}");
            Console.WriteLine($"\tНазвание профиль: {ProfileName}");
            Console.WriteLine($"\tВН компании:      {Companies.Pin}");
            Console.WriteLine($"\tНазвание компании:{Companies.Name}");
            Console.WriteLine($"\tБыл изменён:      {ChangedBy}");
            Console.WriteLine($"\tДата последнего изменения: {LastChangeTime}");
            Console.WriteLine("\tСписок контактов, на которые уходит рассылка профиля:");
            foreach (var contact in Contacts.ToList())
            {
                Console.WriteLine($"\t\t{contact.Contact}");
            }
            if (TinsUsage)
            {
                Console.WriteLine("\tСписок ИНН, по которым фильтруется рассылка профиля:");
                foreach (var tin in Tins.ToList())
                {
                    Console.WriteLine($"\t\t{tin.Tin}");
                }
            }
            Console.WriteLine();
            Console.ResetColor();

        }

        /// <summary>
        /// Редактировать 
        /// </summary>
        /// <param name="_email"></param>
        /// <param name="tins"></param>
        public void Edit(EmailCollection _email, TinsCollection tins)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            EmailCollection email = _email.Clone();
            
          
            email.newItems = email.newItems.Except(Contacts.Select(x => x.Contact.ToLower()).ToList()).ToList();
            email.delItems = email.delItems.Where(x => Contacts.Select(y => y.Contact.ToLower()).Contains(x)).ToList();
            if (email.delItems.Count == 0 && email.newItems.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine($"В рассылке номер {ProfileID} нет новых изменений.");
                return;
            }
            Console.WriteLine("Проверьте планируемые изменения.");
            Console.WriteLine("Список планируемых адресов для рассылки:");

            

            foreach (var item in Contacts.Select(x => x.Contact.ToLower()).ToList())
            {
                if (email.delItems.Contains(item) || email.DeleteAll)
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
            
            foreach (var item in email.newItems)
            {               
                
                Console.WriteLine($"\t{item.ToLower()}");
            }
            

            Console.ResetColor();
            if (!Helper.Accept())
                return;

            EditEmail(email);
          //  EditTins(tins);
        }

        /// <summary>
        /// Метод редактирования списка ИНН
        /// </summary>
        /// <param name="tins"></param>
        public void EditTins(TinsCollection tins)
        {
            foreach (var item in tins.delItems)
            {
                Tins tinToDel = Tins.FirstOrDefault(x=> x.Tin == item.ToLower());
                if (tinToDel != null)
                {
                    Tins.Remove(tinToDel);
                }
            }
            if (tins.DeleteAll)
            {
                Tins.Clear();
            }
            foreach (var addTin in tins.newItems)
            {
                Tins.Add(new Tins {Tin = addTin.ToLower() });

            }
            ChangedBy = Decisions.Employee;
            LastChangeTime = DateTime.Now;
            Description(ConsoleColor.DarkGreen);
        }

        /// <summary>
        /// Метод редактирования списка емаилов
        /// </summary>
        /// <param name="email"></param>
        public void EditEmail(EmailCollection email)
        { 
            foreach (var item in email.delItems)
            {
                Contacts contactToDel = Contacts.FirstOrDefault(
                            x => x.Contact.ToLower() == item);
                if (contactToDel != null)
                {
                    Contacts.Remove(contactToDel);
                }
            }
            if (email.DeleteAll)
            {
                Contacts.Clear();
            }
            foreach (var addEmail in email.newItems)
            {
                Contacts.Add(new Contacts { Contact = addEmail.ToLower() });

            }
            ChangedBy = Decisions.Employee;
            LastChangeTime = DateTime.Now;
            Description(ConsoleColor.DarkGreen);
        }

        /// <summary>
        /// Метод заполнения информации профиля
        /// </summary>
        /// <param name="emails"></param>
        /// <param name="tins"></param>
        public void FillProfile(EmailCollection emails, TinsCollection tins)
        {
            List<Contacts> contacts = new List<Contacts>();
            foreach (string email in emails.newItems)
            {
                var contact = Contacts.FirstOrDefault(c => c.Contact.ToLower() == email.ToLower());
                if (contact == null)
                    contact = new Contacts { Contact = email.ToLower() };
                contacts.Add(contact);
            }
            Contacts = contacts;

            if (tins.newItems.Any())
            {

                List<Tins> TinsToAdd = new List<Tins>();
                foreach (string tin in tins.newItems)
                {
                    TinsToAdd.Add(new Tins { Tin = tin });
                }
                Tins = TinsToAdd;
                TinsUsage = true;

            }
        }
    }
}
