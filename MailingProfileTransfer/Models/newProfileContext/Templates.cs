namespace MailingProfileTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    /// <summary>
    /// Шаблоны писем. Пока непонятно как используетсяы
    /// </summary>
    public partial class Templates
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Templates()
        {
            MailingProfiles = new HashSet<MailingProfiles>();
        }

        [Key]
        public int TemplateID { get; set; }

        public int TypeID { get; set; }

        [Required]
        public string Description { get; set; }

        public string MessageSample { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MailingProfiles> MailingProfiles { get; set; }

        public virtual MailingTypes MailingTypes { get; set; }
    }
}
