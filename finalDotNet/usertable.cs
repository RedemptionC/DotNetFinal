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
        [Required(ErrorMessage ="����")]
        [StringLength(maximumLength:30,MinimumLength =2,ErrorMessage ="�û���λ��Ϊ2��30")]
        public string username { get; set; }

        [Required(ErrorMessage = "����")]
//        [StringLength(maximumLength:256,MinimumLength =7, ErrorMessage = "����λ��Ϊ7��30")]
        public string password { get; set; }

        [Required(ErrorMessage = "����")]
        //[RegularExpression(@"[1-9][0-9]{4,}", ErrorMessage = "Ҫ��5~15λ��������0��ͷ��ֻ��������")]
        public string qq { get; set; }

        [Required(ErrorMessage = "����")]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "ֻ����Ӣ����ĸ�����֡��»��ߡ�Ӣ�ľ�š��Լ��л������")]
        public string email { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<noticetable> noticetable { get; set; }
    }
}
