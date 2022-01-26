namespace MailingProfileTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    /// <summary>
    /// ������ �������� �� ����� ����
    /// </summary>
    public partial class Companies : ICompany
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Companies()
        {
            MailingProfiles = new HashSet<MailingProfiles>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Pin { get; set; }

        public string Name { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MailingProfiles> MailingProfiles { get; set; }

        /// <summary>
        /// ���������� 1 � 2, � 3 ��������
        /// </summary>
        /// <param name="prName"></param>
        /// <param name="emails"></param>
        /// <param name="tins"></param>
        /// <param name="events"></param>
        public void AddProfile(string prName, EmailCollection emails, TinsCollection tins, List<Events> events)
        {
           
                var wh = CreateCleanProfileWH(prName);
                wh.FillProfile(emails, tins);

                
                var ts = CreateCleanProfileTS(prName, events);
                ts.FillProfile(emails, tins);

                var cmr = CreateCleanProfileCmr(prName);
                cmr.FillProfile(emails, tins);


        }
        
        /// <summary>
        /// ����� ���������� 1 ��������
        /// </summary>
        /// <param name="prName"></param>
        /// <param name="emails"></param>
        /// <param name="tins"></param>
        public void AddProfileWH(string prName, EmailCollection emails, TinsCollection tins)
        {
            var wh = CreateCleanProfileWH(prName);
            wh.FillProfile(emails, tins);
        }

        /// <summary>
        /// ����� ���������� 3 ��������
        /// </summary>
        /// <param name="prName"></param>
        /// <param name="emails"></param>
        /// <param name="tins"></param>
        public void AddProfileCmr(string prName, EmailCollection emails, TinsCollection tins)
        {
            var cmr = CreateCleanProfileCmr(prName);
            cmr.FillProfile(emails, tins);
        }

        /// <summary>
        /// ����� ���������� 2 ��������
        /// </summary>
        /// <param name="prName"></param>
        /// <param name="emails"></param>
        /// <param name="tins"></param>
        /// <param name="events"></param>
        public void AddProfileTS(string prName, EmailCollection emails, TinsCollection tins, List<Events> events)
        {
            var ts = CreateCleanProfileTS(prName, events);
            ts.FillProfile(emails, tins);
        }

        /// <summary>
        /// �������� �������� "���������� � ��"
        /// </summary>
        /// <param name="prName"></param>
        /// <param name="events"></param>
        /// <returns></returns>
        private IProfile CreateCleanProfileTS(string prName, List<Events> events)
        {
            Models.MailingProfiles newPrTS = new Models.MailingProfiles
            {
                ChangedBy = $"{Decisions.Employee}",
                ContactType = "email",
                TypeID = 2,
                CustomsUsage = true,
                TinsUsage = false,
                Pin = Pin,
                ProfileName = prName + " ���������� � ��",
                TemplateID = 2,                
                IsActive = true,
                LastChangeTime = DateTime.Now,
                Events = events
            };

            MailingProfiles.Add(newPrTS);

            return newPrTS;

        }

        /// <summary>
        /// �������� �������� "���������� � �����������"
        /// </summary>
        /// <param name="prName"></param>
        /// <returns></returns>
        private IProfile CreateCleanProfileWH(string prName)
        {

            Models.MailingProfiles newPrWH = new Models.MailingProfiles
            {
                ChangedBy = $"{Decisions.Employee}",
                ContactType = "email",
                TypeID = 1,
                CustomsUsage = false,
                TinsUsage = false,
                Pin = Pin,
                ProfileName = prName + " ��� �� ��",
                TemplateID = 1,
                IsActive = true,
                LastChangeTime = DateTime.Now,
            };
            MailingProfiles.Add(newPrWH);
            return newPrWH;
        }

        /// <summary>
        /// �������� �������� "���� ���������"
        /// </summary>
        /// <param name="prName"></param>
        /// <returns></returns>
        private IProfile CreateCleanProfileCmr(string prName)
        {

            Models.MailingProfiles newPrCmr = new Models.MailingProfiles
            {
                ChangedBy = $"{Decisions.Employee}",
                ContactType = "email",
                TypeID = 3,
                CustomsUsage = true,
                TinsUsage = false,
                Pin = Pin,
                ProfileName = prName + " �����",
                TemplateID = 4,
                IsActive = true,
                LastChangeTime = DateTime.Now,
            };
            MailingProfiles.Add(newPrCmr);
            return newPrCmr;
        }


        /// <summary>
        /// ����� �������� �������� � �������
        /// </summary>
        public void Description()
        {
            Console.WriteLine("����� ���������� � ��������:");
            Console.WriteLine($"������������ ��������: {Name}");
            
            
        }

        /// <summary>
        /// ����� �������� �������� �������� � �������
        /// </summary>
        public void ProfilesDescription()
        {
            
            foreach (var pr in MailingProfiles)
            {
                pr.Description();
            }
        }
    }
}
