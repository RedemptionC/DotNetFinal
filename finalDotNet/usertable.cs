namespace finalDotNet
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("usertable")]
    public partial class usertable
    {
        //static string Email = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        //static string Qq= @"[1-9][0-9]{4,}";
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public usertable()
        {
            noticetable = new HashSet<noticetable>();
        }

        [Key]
        [Required(ErrorMessage ="必填")]
        [StringLength(maximumLength:30,MinimumLength =2,ErrorMessage ="用户名位数为2到30")]
        public string username { get; set; }

        [Required(ErrorMessage = "必填")]
//        [StringLength(maximumLength:256,MinimumLength =7, ErrorMessage = "密码位数为7到30")]
        public string password { get; set; }

        [Required(ErrorMessage = "必填")]
        //[RegularExpression(@"[1-9][0-9]{4,}", ErrorMessage = "要求5~15位，不能以0开头，只能是数字")]
        public string qq { get; set; }

        [Required(ErrorMessage = "必填")]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "只允许英文字母、数字、下划线、英文句号、以及中划线组成")]
        public string email { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<noticetable> noticetable { get; set; }
    }
}
