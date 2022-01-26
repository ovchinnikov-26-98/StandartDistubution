namespace MailingProfileTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    /// <summary>
    /// Табличка с ИНН. Непонятно как используется
    /// </summary>
    public partial class AvailableTins
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(6)]
        public string Pin { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string Tin { get; set; }
    }
}
