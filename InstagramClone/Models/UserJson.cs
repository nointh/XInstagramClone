using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramClone.Models
{
    class UserJson
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public UserJson GetValue()
        {
            return new UserJson
            {
                Email = this.Email,
                Fullname = this.Fullname,
                Username = this.Username
            };
        }    
    }
}
