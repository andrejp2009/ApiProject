using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiProject.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now; 

        [Required]
        public int UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }
    }
}
