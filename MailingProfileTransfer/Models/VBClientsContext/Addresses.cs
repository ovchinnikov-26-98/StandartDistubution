namespace MailingProfileTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    /// <summary>
    /// Табличка с емаилами
    /// </summary>
    public partial class Addresses
    {        
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_Profile { get; set; }

        [Key]
        [Column("Address", Order = 1)]
        [StringLength(100)]
        public string Address { get; set; }
        [ForeignKey("id_Profile")]
        public virtual Profiles Profile { get; set; }
    }
}
