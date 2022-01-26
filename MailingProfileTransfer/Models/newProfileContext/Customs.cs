namespace MailingProfileTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    /// <summary>
    /// Непонятно что, не используется. Таблица пуста.
    /// </summary>
    public partial class Customs
    {
        public int CustomsID { get; set; }

        public int ProfileID { get; set; }

        [Required]
        public string Custom { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual MailingProfiles MailingProfiles { get; set; }
    }
}
