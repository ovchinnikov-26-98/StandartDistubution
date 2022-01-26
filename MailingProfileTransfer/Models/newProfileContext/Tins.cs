namespace MailingProfileTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    /// <summary>
    /// Табличка со списком ИНН
    /// </summary>
    public partial class Tins
    {
        [Key]
        public int TinID { get; set; }

        public int ProfileID { get; set; }

        [Required]
        public string Tin { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual MailingProfiles MailingProfiles { get; set; }
    }
}
