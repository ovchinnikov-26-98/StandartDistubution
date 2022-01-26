namespace MailingProfileTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    /// <summary>
    /// Какой то список пользователей. Непонятно зачем
    /// </summary>

    [Table("_VBUsers")]
    public partial class C_VBUsers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_User { get; set; }

        public virtual Users User { get; set; }
    }
}
