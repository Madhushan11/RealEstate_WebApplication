using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstateExplore.Models
{
    public class Price
    {
        [Key]
        public int PurchaseID { get; set; }
        [Required]
        [ForeignKey("Property")]
        public int PropertyID { get; set; }
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        //[NotMapped]
       // public DateOnly StartDate {  get; set; }
       // [NotMapped]
       // public DateOnly EndDate { get; set;}
    }
}
