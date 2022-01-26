namespace MailingProfileTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    /// <summary>
    /// Табличка, в которой перечислены какого типа рассылка
    /// </summary>
    [Table("_MailingTypes")]
    public partial class C_MailingTypes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_Profile { get; set; }

        public int MailingType { get; set; }
    }
}
