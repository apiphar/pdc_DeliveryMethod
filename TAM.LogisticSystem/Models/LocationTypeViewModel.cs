using System.ComponentModel.DataAnnotations;

namespace TAM.LogisticSystem.Models
{
    public class LocationTypeViewModel
    {
        [Required]
        [MaxLength(8)]
        [RegularExpression("^[a-zA-Z0-9]*$")]
        public string LocationTypeCode { get; set; }
        [Required]
        [StringLength(255)]
        [RegularExpression("^[a-zA-Z0-9\\s\\-.&,\'/]*$")]
        public string Name { set; get; }
        [Required]
        public bool HasResponsibility { get; set; }
        [Required]
        public bool NeedSjkbTarikan { get; set; }
    }
}
