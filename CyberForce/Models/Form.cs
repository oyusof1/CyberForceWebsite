using System;
namespace CyberForce.Models
{
    public class Form
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Password { get; set; } = "";
        public IFormFile? File { get; set; }
    }
}

