using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Models
{
    public class Portfolio:BaseEntity
    {
        [Required]
        [MaxLength(25,ErrorMessage ="Ad 25 herfden uzun ola bilmez !!!")]
        [MinLength(3,ErrorMessage ="Add 3 herfden qisa Ola bilmez !!!")]
        public string Name { get; set; }

        [Required]

        public string ImageUrl { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }


    }
}
