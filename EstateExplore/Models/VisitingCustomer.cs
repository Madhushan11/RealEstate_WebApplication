// VisitingCustomer.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace EstateExplore.Models
{
    public class VisitingCustomer
    {
        public int VisitingCustomerId { get; set; }

        public int PropertyId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [Required]
        public DateTime VisitDate { get; set; }

        // Add other relevant properties here
    }
}
