namespace finalDotNet
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("noticetable")]
    public partial class noticetable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public string author { get; set; }

        [Required(ErrorMessage = "必填")]
        public string content { get; set; }

        public DateTime submitTime { get; set; }

        [Required(ErrorMessage = "必填")]
        [StringLength(maximumLength:30,MinimumLength =3,ErrorMessage ="标题字数长度为3到30")]
        public string title { get; set; }

        public int isApproved { get; set; }


        public string kind { get; set; }

        public virtual usertable usertable { get; set; }
    }
}
