namespace MailingProfileTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    /// <summary>
    /// ИНН профиля
    /// </summary>
    public partial class Profiles_Tins
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_Profile { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string Tin { get; set; }
       
        public virtual Profiles Profile { get; set; }
    }
}
