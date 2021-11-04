using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CommandAPI.Dtos
{
    public class CommandUpdateDto
    {
        [Required]
        [MaxLength(250)]
        [JsonProperty("how_to")]
        public string HowTo { get; set; }

        [Required]
        [JsonProperty("platform")]
        public string Platform { get; set; }

        [Required]
        [JsonProperty("command_line")]
        public string CommandLine { get; set; }
    }
}