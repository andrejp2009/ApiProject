using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiProject.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Phone]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        public bool IsActive { get; set; } = true;

        [JsonIgnore]
        public ICollection<Order> Orders { get; set; }
    }
}
