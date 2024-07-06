using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiProject.Models
{
    public class Order
    {
        [Key]
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }
    }
}
