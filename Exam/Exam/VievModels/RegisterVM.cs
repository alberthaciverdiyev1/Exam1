using System.ComponentModel.DataAnnotations;

namespace Exam.VievModels
{
    public class RegisterVM
    {
        [Required]
        [MaxLength(25,ErrorMessage ="Ad 25 herfden uzun ola bilmez")]
        [MinLength(3, ErrorMessage = "Ad 3 herfden qisa ola bilmez")]
        public string Name { get; set; }
        [Required]
        [MaxLength(25, ErrorMessage = "soyad 25 herfden uzun ola bilmez")]
        [MinLength(3, ErrorMessage = "Soyad 3 herfden qisa ola bilmez")]

        public string Surname { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
