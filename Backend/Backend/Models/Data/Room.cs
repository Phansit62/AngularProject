﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Backend.Models.Data
{
    public partial class Room
    {
        public Room()
        {
            Orders = new HashSet<Orders>();
        }

        public string RoomId { get; set; }
        public int? CateRoomId { get; set; }
        public bool? StatusRoom { get; set; }
        public int? FloorId { get; set; }

        public virtual CategoryRoom CateRoom { get; set; }
        public virtual Floor Floor { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}