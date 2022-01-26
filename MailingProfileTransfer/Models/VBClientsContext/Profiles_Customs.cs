namespace MailingProfileTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    /// <summary>
    /// непонятно зачем
    /// </summary>
    public partial class Profiles_Customs
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_Profile { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string Customs { get; set; }
        [ForeignKey("id_Profile")]
        public virtual Profiles Profile { get; set; }
    }
}
