namespace MailingProfileTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    /// <summary>
    /// В таблице добавлен только адрес severtrans@ntbroker.ru. Непонятно зачем.
    /// </summary>
    public partial class ConfirmationEmailAddresses
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProfileId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string EmailAddress { get; set; }

        public virtual Profiles Profile { get; set; }
    }
}
