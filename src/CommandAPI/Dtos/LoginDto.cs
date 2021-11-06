using System.ComponentModel.DataAnnotations;

namespace CommandAPI.Dtos
{
    public class LoginDto
    {
        [Required]
        [MaxLength(254)] // 邮件长度限制：https://routinepanic.com/questions/what-is-the-maximum-length-of-a-valid-email-address
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string Password { get; set; }
    }
}