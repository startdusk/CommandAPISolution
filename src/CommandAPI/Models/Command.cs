using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommandAPI.Models
{
    [Table("command")]
    public class Command
    {
        [Key] // primary key
        [Required]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        [Column("how_to")]
        public string HowTo { get; set; }

        [Required]
        [Column("platform")]
        public string Platform { get; set; }

        [Required]
        [Column("command_line")]
        public string CommandLine { get; set; }
    }
}