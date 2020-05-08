using System.ComponentModel.DataAnnotations;

namespace CoreAppSample.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(8, MinimumLength =4, ErrorMessage ="you must provide password between 4 and 8")]
        public string Password { get; set; }
    }
}