﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Backend.Models.Data
{
    public partial class Floor
    {
        public Floor()
        {
            Room = new HashSet<Room>();
        }

        public int FloorId { get; set; }
        public string FloorNum { get; set; }

        public virtual ICollection<Room> Room { get; set; }
    }
}