﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Backend.Models.Data
{
    public partial class ProjectroomContext : DbContext
    {
        public ProjectroomContext()
        {
        }

        public ProjectroomContext(DbContextOptions<ProjectroomContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<CateImage> CateImage { get; set; }
        public virtual DbSet<CategoryRoom> CategoryRoom { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Floor> Floor { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<PaymentBill> PaymentBill { get; set; }
        public virtual DbSet<ReportProblem> ReportProblem { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<RoomBill> RoomBill { get; set; }
        public virtual DbSet<Title> Title { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Thai_100_CI_AI");

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.Username);

                entity.Property(e => e.Username)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.Property(e => e.Password)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("password");
            });

            modelBuilder.Entity<CateImage>(entity =>
            {
                entity.HasKey(e => e.CateimgId);

                entity.Property(e => e.CateimgId).HasColumnName("cateimg_id");

                entity.Property(e => e.CateRoomId).HasColumnName("CateRoom_id");

                entity.Property(e => e.Path)
                    .IsUnicode(false)
                    .HasColumnName("path");

                entity.HasOne(d => d.CateRoom)
                    .WithMany(p => p.CateImage)
                    .HasForeignKey(d => d.CateRoomId)
                    .HasConstraintName("FK_CateImage_CategoryRoom1");
            });

            modelBuilder.Entity<CategoryRoom>(entity =>
            {
                entity.HasKey(e => e.CateRoomId)
                    .HasName("PK_CategoryPrice");

                entity.Property(e => e.CateRoomId).HasColumnName("CateRoom_id");

                entity.Property(e => e.CateRoomName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CateRoom_name");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("description");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CusId);

                entity.Property(e => e.CusId).HasColumnName("Cus_id");

                entity.Property(e => e.Fristname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("fristname");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lastname");

                entity.Property(e => e.LineId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LineID");

                entity.Property(e => e.Password)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Telephone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("telephone");

                entity.Property(e => e.TitleId).HasColumnName("Title_id");

                entity.Property(e => e.Token).IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Title)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.TitleId)
                    .HasConstraintName("FK_Customer_Title");
            });

            modelBuilder.Entity<Floor>(entity =>
            {
                entity.Property(e => e.FloorId).HasColumnName("floor_id");

                entity.Property(e => e.FloorNum)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Floor_num");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.OrderId).HasColumnName("Order_Id");

                entity.Property(e => e.CusId).HasColumnName("Cus_id");

                entity.Property(e => e.DateIn)
                    .HasColumnType("date")
                    .HasColumnName("Date_In");

                entity.Property(e => e.RoomId)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("Room_Id");

                entity.Property(e => e.StatusOrder).HasColumnName("Status_order");

                entity.HasOne(d => d.Cus)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CusId)
                    .HasConstraintName("FK_Orders_Customer");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_Orders_Room");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.PaymentId).HasColumnName("Payment_Id");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("date")
                    .HasColumnName("Payment_Date");

                entity.Property(e => e.PaymentOrderId).HasColumnName("PaymentOrder_Id");

                entity.Property(e => e.PaymentPic)
                    .IsUnicode(false)
                    .HasColumnName("Payment_Pic");

                entity.Property(e => e.StatusPay).HasColumnName("Status_pay");

                entity.HasOne(d => d.PaymentOrder)
                    .WithMany(p => p.Payment)
                    .HasForeignKey(d => d.PaymentOrderId)
                    .HasConstraintName("FK_Payment_Orders");
            });

            modelBuilder.Entity<PaymentBill>(entity =>
            {
                entity.Property(e => e.PaymentBillId).HasColumnName("PaymentBill_Id");

                entity.Property(e => e.PaymentBillDate)
                    .HasColumnType("date")
                    .HasColumnName("PaymentBill_date");

                entity.Property(e => e.PaymentPic).IsUnicode(false);

                entity.Property(e => e.RoomBillId).HasColumnName("RoomBill_id");

                entity.Property(e => e.StatusPay).HasColumnName("Status_pay");

                entity.HasOne(d => d.RoomBill)
                    .WithMany(p => p.PaymentBill)
                    .HasForeignKey(d => d.RoomBillId)
                    .HasConstraintName("FK_PaymentBill_RoomBill");
            });

            modelBuilder.Entity<ReportProblem>(entity =>
            {
                entity.HasKey(e => e.ProblemId);

                entity.Property(e => e.ProblemId).HasColumnName("problem_id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.ProblemText)
                    .IsUnicode(false)
                    .HasColumnName("problem_text");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ReportProblem)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_ReportProblem_Orders");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.RoomId)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("Room_Id");

                entity.Property(e => e.CateRoomId).HasColumnName("CateRoom_id");

                entity.Property(e => e.FloorId).HasColumnName("floor_id");

                entity.Property(e => e.StatusRoom).HasColumnName("Status_room");

                entity.HasOne(d => d.CateRoom)
                    .WithMany(p => p.Room)
                    .HasForeignKey(d => d.CateRoomId)
                    .HasConstraintName("FK_Room_CategoryRoom");

                entity.HasOne(d => d.Floor)
                    .WithMany(p => p.Room)
                    .HasForeignKey(d => d.FloorId)
                    .HasConstraintName("FK_Room_Floor");
            });

            modelBuilder.Entity<RoomBill>(entity =>
            {
                entity.Property(e => e.RoomBillId).HasColumnName("RoomBill_Id");

                entity.Property(e => e.BillDate)
                    .HasColumnType("date")
                    .HasColumnName("Bill_Date");

                entity.Property(e => e.Electricity).HasColumnName("electricity");

                entity.Property(e => e.OrderId).HasColumnName("Order_Id");

                entity.Property(e => e.StatusBill).HasColumnName("Status_bill");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.RoomBill)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_RoomBill_Orders");
            });

            modelBuilder.Entity<Title>(entity =>
            {
                entity.Property(e => e.TitleId).HasColumnName("Title_id");

                entity.Property(e => e.TitleName)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Title_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}