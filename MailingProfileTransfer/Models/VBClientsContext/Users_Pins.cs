namespace MailingProfileTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    /// <summary>
    /// Пины (ВН) компаний
    /// </summary>
    public partial class Users_Pins
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_User { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string PIN { get; set; }
        [ForeignKey("id_User")]
        public virtual Users User { get; set; }
    }
}
