// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Backend.Models.Data
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Orders>();
        }

        public int CusId { get; set; }
        public int? TitleId { get; set; }
        public string Fristname { get; set; }
        public string Lastname { get; set; }
        public string Telephone { get; set; }
        public string LineId { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual Title Title { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}