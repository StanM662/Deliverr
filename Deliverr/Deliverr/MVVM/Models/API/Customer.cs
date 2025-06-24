using System.ComponentModel.DataAnnotations;

namespace Deliverr.Models
{
    public partial class Customer
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [Required]
        public string Address { get; set; } = "";

        public bool Active { get; set; }
    }
}
