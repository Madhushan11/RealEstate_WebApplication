using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstateExplore.Models
{
	public class Property
	{
		public int PropertyID { get; set; }

		[NotMapped]
		public IFormFile? PropertyImage { get; set; }

		[StringLength(500)]
		public string? ImageUrl { get; set; }

		[Required]
		public string? PropertyName { get; set; }

		[Required]
		public string? Owner { get; set; }
		public string? Description { get; set; }

		[Required]
		public decimal Price { get; set; }

		[Required]
		public string? PopertyLocation { get; set; }
	}
}
