using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models//114
{
    public class SignInput//kulanıcından alınac kverı
    {
        [Required]
        [Display(Name = "Email Adresiniz")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Şifreniz")]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool IsRemember { get; set; }
    }
}
