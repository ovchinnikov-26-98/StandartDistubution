namespace MailingProfileTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    /// <summary>
    /// Пины профиля
    /// </summary>
    public partial class Profiles_Pins
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_Profile { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string Pin { get; set; }
        [ForeignKey("id_Profile")]
        public virtual Profiles Profile { get; set; }
    }
}
