using System.ComponentModel.DataAnnotations;

namespace TAM.LogisticSystem.Models
{
    public class DealerCreateOrUpdateRequest
    {
        [Required]
        [StringLength(256)]
        public string Name { set; get; }
    }
}
