﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using bageri.api.Data;

#nullable disable

namespace bageri.api.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20241229145112_Added SupplierProducts")]
    partial class AddedSupplierProducts
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("bageri.api.Entities.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ItemNumber")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PricePerKg")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductName")
                        .HasColumnType("TEXT");

                    b.HasKey("ProductId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("bageri.api.Entities.Supplier", b =>
                {
                    b.Property<int>("SupplierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("SupplierId");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("bageri.api.Entities.SupplierProduct", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SupplierId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ItemNumber")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PricePerKg")
                        .HasColumnType("TEXT");

                    b.HasKey("ProductId", "SupplierId");

                    b.HasIndex("SupplierId");

                    b.ToTable("SupplierProducts");
                });

            modelBuilder.Entity("bageri.api.Entities.SupplierProduct", b =>
                {
                    b.HasOne("bageri.api.Entities.Product", "Product")
                        .WithMany("SupplierProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("bageri.api.Entities.Supplier", "Supplier")
                        .WithMany("SupplierProducts")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("bageri.api.Entities.Product", b =>
                {
                    b.Navigation("SupplierProducts");
                });

            modelBuilder.Entity("bageri.api.Entities.Supplier", b =>
                {
                    b.Navigation("SupplierProducts");
                });
#pragma warning restore 612, 618
        }
    }
}
