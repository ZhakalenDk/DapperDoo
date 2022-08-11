using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oiski.School.Web.DapperDoo.Domain
{
    [Table("SubTasks")]
    public class SubTasks
    {
        [Key]
        public Guid ID { get; set; }    //  Primary Key
        [MaxLength(25)]
        [Required] public string Title { get; set; }

        [ForeignKey(nameof(TaskID))]
        public Guid TaskID { get; set; }    //  Relational ID for parent Task
    }
}
