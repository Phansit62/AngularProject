// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Backend.Models.Data
{
    public partial class PaymentBill
    {
        public int PaymentBillId { get; set; }
        public int? RoomBillId { get; set; }
        public DateTime? PaymentBillDate { get; set; }
        public string PaymentPic { get; set; }
        public bool? StatusPay { get; set; }

        public virtual RoomBill RoomBill { get; set; }
    }
}