using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oiski.School.Web.DapperDoo.Domain
{
    [Table("Tasks")]
    public class TaskItem
    {
        [Key]
        public Guid ID { get; set; }    //  Primary key
        [MaxLength(25)]
        [Required] public string Title { get; set; }
        [MaxLength(150)]
        public string Description { get; set; }
    }
}
