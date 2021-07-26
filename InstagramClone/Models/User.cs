using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramClone.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TokenFirebase { get; set; }
        public string PhoneNumber { get; set; }
    }
}
