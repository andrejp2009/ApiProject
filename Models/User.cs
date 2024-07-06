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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;

        [JsonIgnore]
        public ICollection<Order> Orders { get; set; }
    }
}
