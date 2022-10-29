using System;
namespace CyberForce.Models
{
    public class AuthUsers
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        //public string password { get; set; }
        public string emailAddress { get; set; }
        public string sAMAccountName { get; set; }
        public string userRole { get; set; }
        public string domain { get; set; }
        public DateTime validTo { get; set; }
        public Exception exception { get; set; }
    }
}

