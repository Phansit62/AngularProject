﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Backend.Models.Data
{
    public partial class CateImage
    {
        public int CateimgId { get; set; }
        public string Path { get; set; }
        public int? CateRoomId { get; set; }

        public virtual CategoryRoom CateRoom { get; set; }
    }
}