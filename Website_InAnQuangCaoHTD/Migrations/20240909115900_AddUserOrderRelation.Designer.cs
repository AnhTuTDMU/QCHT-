﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Website_InAnQuangCaoHTD.Data;

#nullable disable

namespace Website_InAnQuangCaoHTD.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240909115900_AddUserOrderRelation")]
    partial class AddUserOrderRelation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryID"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("CategoryID");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryID = 1,
                            CategoryName = "Ấn Phẩm Văn Phòng"
                        });
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.FlashSale", b =>
                {
                    b.Property<int>("FlashSaleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FlashSaleId"));

                    b.Property<DateTime>("FlashSaleEndTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("FlashSalePrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("FlashSaleStartTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("FlashSaleId");

                    b.HasIndex("ProductId");

                    b.ToTable("FlashSales");

                    b.HasData(
                        new
                        {
                            FlashSaleId = 1,
                            FlashSaleEndTime = new DateTime(2024, 9, 10, 18, 58, 58, 604, DateTimeKind.Local).AddTicks(8750),
                            FlashSalePrice = 800m,
                            FlashSaleStartTime = new DateTime(2024, 9, 8, 18, 58, 58, 604, DateTimeKind.Local).AddTicks(8732),
                            IsActive = true,
                            ProductId = 1
                        },
                        new
                        {
                            FlashSaleId = 2,
                            FlashSaleEndTime = new DateTime(2024, 9, 10, 18, 58, 58, 604, DateTimeKind.Local).AddTicks(8752),
                            FlashSalePrice = 1500m,
                            FlashSaleStartTime = new DateTime(2024, 9, 8, 18, 58, 58, 604, DateTimeKind.Local).AddTicks(8751),
                            IsActive = true,
                            ProductId = 2
                        });
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderDetailId"));

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("OrderDetailId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductID"));

                    b.Property<string>("CategoryProduct")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("ImageFileName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<decimal?>("Price")
                        .IsRequired()
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Size")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("SubCategoryID")
                        .HasColumnType("int");

                    b.HasKey("ProductID");

                    b.HasIndex("SubCategoryID");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ProductID = 1,
                            CategoryProduct = "Hộp",
                            Description = "Mô tả sản phẩm 2",
                            ImageFileName = "DanhThiep_1.jpg",
                            Price = 100000m,
                            ProductName = "Danh Thiếp 1",
                            Size = "80x100cm",
                            SubCategoryID = 1
                        },
                        new
                        {
                            ProductID = 2,
                            CategoryProduct = "Hộp",
                            Description = "Mô tả sản phẩm 3",
                            ImageFileName = "DanhThiep_1.jpg",
                            Price = 100000m,
                            ProductName = "Danh Thiếp 2",
                            Size = "80x100cm",
                            SubCategoryID = 1
                        },
                        new
                        {
                            ProductID = 3,
                            CategoryProduct = "Hộp",
                            Description = "Mô tả sản phẩm 2",
                            ImageFileName = "bao.jpg",
                            Price = 100000m,
                            ProductName = "Bao Thư 1",
                            Size = "80x100cm",
                            SubCategoryID = 2
                        },
                        new
                        {
                            ProductID = 4,
                            CategoryProduct = "Hộp",
                            Description = "Mô tả sản phẩm 3",
                            ImageFileName = "bao.jpg",
                            Price = 100000m,
                            ProductName = "Bao Thư 2",
                            Size = "80x100cm",
                            SubCategoryID = 2
                        },
                        new
                        {
                            ProductID = 5,
                            CategoryProduct = "Hộp",
                            Description = "Mô tả sản phẩm 2",
                            ImageFileName = "TuiGiay_1.jpg",
                            Price = 100000m,
                            ProductName = "Bìa Đựng Hồ Sơ 1",
                            Size = "80x100cm",
                            SubCategoryID = 3
                        },
                        new
                        {
                            ProductID = 6,
                            CategoryProduct = "Hộp",
                            Description = "Mô tả sản phẩm 3",
                            ImageFileName = "TuiGiay_1.jpg",
                            Price = 100000m,
                            ProductName = "Bìa Đựng Hồ Sơ 2",
                            Size = "80x100cm",
                            SubCategoryID = 3
                        });
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.RolesModel", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            RoleName = "Quản lý"
                        },
                        new
                        {
                            RoleId = 2,
                            RoleName = "Khách hàng"
                        });
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.ShoppingCart", b =>
                {
                    b.Property<int>("ShoppingCartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShoppingCartId"));

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ShoppingCartId");

                    b.HasIndex("UserId");

                    b.ToTable("ShoppingCarts");
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.ShoppingCartItem", b =>
                {
                    b.Property<int>("ShoppingCartItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShoppingCartItemId"));

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("ShoppingCartId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ShoppingCartItemId");

                    b.HasIndex("ProductId");

                    b.HasIndex("ShoppingCartId");

                    b.HasIndex("UserId");

                    b.ToTable("ShoppingCartItems");
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.Slider", b =>
                {
                    b.Property<int>("SliderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SliderId"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SliderImg")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SliderName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("SliderStatus")
                        .HasColumnType("bit");

                    b.Property<string>("SliderTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SliderId");

                    b.ToTable("Sliders");
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.SubCategory", b =>
                {
                    b.Property<int>("SubCategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubCategoryID"));

                    b.Property<int>("CategoryID")
                        .HasColumnType("int");

                    b.Property<string>("SubCategoryImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubCategoryName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("SubCategoryID");

                    b.HasIndex("CategoryID");

                    b.ToTable("SubCategories");

                    b.HasData(
                        new
                        {
                            SubCategoryID = 1,
                            CategoryID = 1,
                            SubCategoryName = "Danh Thiếp"
                        },
                        new
                        {
                            SubCategoryID = 2,
                            CategoryID = 1,
                            SubCategoryName = "Bao Thư"
                        },
                        new
                        {
                            SubCategoryID = 3,
                            CategoryID = 1,
                            SubCategoryName = "Bìa Đựng Hồ Sơ"
                        });
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.UsersModel", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicture")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            Address = "Bình Dương",
                            Password = "IP/yw120FvWx13bNsVOPMGYHxop2ubZh2Fpzz6gVgm4=",
                            PhoneNumber = "0332613703",
                            ProfilePicture = "",
                            RoleId = 1,
                            UserEmail = "AnhTu080302@gmail.com",
                            UserName = "Nguyễn Anh Tú"
                        });
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.FlashSale", b =>
                {
                    b.HasOne("Website_InAnQuangCaoHTD.Models.Product", "Product")
                        .WithMany("FlashSales")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.Order", b =>
                {
                    b.HasOne("Website_InAnQuangCaoHTD.Models.UsersModel", "Users")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.OrderDetail", b =>
                {
                    b.HasOne("Website_InAnQuangCaoHTD.Models.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.Product", b =>
                {
                    b.HasOne("Website_InAnQuangCaoHTD.Models.SubCategory", "SubCategory")
                        .WithMany("Products")
                        .HasForeignKey("SubCategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SubCategory");
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.ShoppingCart", b =>
                {
                    b.HasOne("Website_InAnQuangCaoHTD.Models.UsersModel", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.ShoppingCartItem", b =>
                {
                    b.HasOne("Website_InAnQuangCaoHTD.Models.Product", "Product")
                        .WithMany("ShoppingCartItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Website_InAnQuangCaoHTD.Models.ShoppingCart", "ShoppingCart")
                        .WithMany("ShoppingCartItems")
                        .HasForeignKey("ShoppingCartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Website_InAnQuangCaoHTD.Models.UsersModel", "User")
                        .WithMany("ShoppingCartItems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("ShoppingCart");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.SubCategory", b =>
                {
                    b.HasOne("Website_InAnQuangCaoHTD.Models.Category", "Category")
                        .WithMany("SubCategories")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.UsersModel", b =>
                {
                    b.HasOne("Website_InAnQuangCaoHTD.Models.RolesModel", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.Category", b =>
                {
                    b.Navigation("SubCategories");
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.Product", b =>
                {
                    b.Navigation("FlashSales");

                    b.Navigation("ShoppingCartItems");
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.RolesModel", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.ShoppingCart", b =>
                {
                    b.Navigation("ShoppingCartItems");
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.SubCategory", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Website_InAnQuangCaoHTD.Models.UsersModel", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("ShoppingCartItems");
                });
#pragma warning restore 612, 618
        }
    }
}
