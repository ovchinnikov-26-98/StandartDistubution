namespace MailingProfileTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;


    /// <summary>
    /// ����������� ������� ������ ������ ��������.
    /// ��������� ������� ��������.
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
        /// ��� �������� ���������.
        /// �� 17.07.21 ���������� 2 ���� ������������ ����� ������� ���������:
        /// - ����������� ��� �� ��;
        /// - ���������� � ��.
        /// ���� ��������� �������� � ������� MailingTypes.
        /// </summary>
        public int TypeID { get; set; }

        /// <summary>
        /// ������ ��������� ����������.
        /// �� ��������� �� 17.07.21 �������������� ������ ������ �������� ����� email.
        /// ������ �� ������� ContactTypes.
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
        /// ������ ���������, �� ������� ��� �������� � �������.
        /// � ����� ������ - ��� ������ email-�������.
        /// � ���������� ����� ����� ������������� ������� - ContactsMailingProfiles.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", 
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contacts> Contacts { get; set; }

        /// <summary>
        /// ������ �������������� ������� ����������� � �������.
        /// ������� � ������� ������� ����� ������������� ������� EventsMailingProfiles.
        /// �� 17.07.21 ���������� 6 ����� �������:
        /// �����;
        /// ������ ���������� �� ����������� ��;
        /// ���������� ��������;
        /// ����������� ��������;
        /// �����;
        /// ���� ������� �� ��.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", 
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Events> Events { get; set; }

        /// <summary>
        /// ����� ����� ���������� � ��������
        /// </summary>
        /// <param name="color"></param>
        public void Description(ConsoleColor color = ConsoleColor.DarkGray)
        {

            Console.ForegroundColor = color;
            Console.WriteLine("����������� �� �������.");
            Console.WriteLine($"\t������������:    MessageDistributionSettings");
            Console.WriteLine($"\t����� �������:    {ProfileID}");
            Console.WriteLine($"\t�������:    {IsActive}");
            Console.WriteLine($"\t����� ��������:    {TypeID}");
            Console.WriteLine($"\t�������� �������: {ProfileName}");
            Console.WriteLine($"\t�� ��������:      {Companies.Pin}");
            Console.WriteLine($"\t�������� ��������:{Companies.Name}");
            Console.WriteLine($"\t��� ������:      {ChangedBy}");
            Console.WriteLine($"\t���� ���������� ���������: {LastChangeTime}");
            Console.WriteLine("\t������ ���������, �� ������� ������ �������� �������:");
            foreach (var contact in Contacts.ToList())
            {
                Console.WriteLine($"\t\t{contact.Contact}");
            }
            if (TinsUsage)
            {
                Console.WriteLine("\t������ ���, �� ������� ����������� �������� �������:");
                foreach (var tin in Tins.ToList())
                {
                    Console.WriteLine($"\t\t{tin.Tin}");
                }
            }
            Console.WriteLine();
            Console.ResetColor();

        }

        /// <summary>
        /// ������������� 
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

                Console.WriteLine($"� �������� ����� {ProfileID} ��� ����� ���������.");
                return;
            }
            Console.WriteLine("��������� ����������� ���������.");
            Console.WriteLine("������ ����������� ������� ��� ��������:");

            

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
        /// ����� �������������� ������ ���
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
        /// ����� �������������� ������ �������
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
        /// ����� ���������� ���������� �������
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
