﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PAWS_NDV_PetLovers.Data;

#nullable disable

namespace PAWS_NDV_PetLovers.Migrations
{
    [DbContext(typeof(PAWS_NDV_PetLoversContext))]
    partial class PAWS_NDV_PetLoversContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Appointments.Appointment", b =>
                {
                    b.Property<int>("AppointId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AppointId"));

                    b.Property<string>("contact")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.Property<string>("fname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("mname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ownerId")
                        .HasColumnType("int");

                    b.Property<string>("remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("time")
                        .HasColumnType("datetime2");

                    b.HasKey("AppointId");

                    b.HasIndex("ownerId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Appointments.AppointmentDetails", b =>
                {
                    b.Property<int>("AppDetailsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AppDetailsId"));

                    b.Property<int>("AppointId")
                        .HasColumnType("int");

                    b.Property<int?>("serviceID")
                        .HasColumnType("int");

                    b.HasKey("AppDetailsId");

                    b.HasIndex("AppointId");

                    b.HasIndex("serviceID");

                    b.ToTable("AppointmentDetails");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Appointments.PetFollowUps", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.Property<int>("diagnosticsId")
                        .HasColumnType("int");

                    b.Property<int>("serviceId")
                        .HasColumnType("int");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("diagnosticsId");

                    b.HasIndex("serviceId");

                    b.ToTable("PetFollowUps");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Records.Category", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("categoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("lastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("registeredDate")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Records.Owner", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("contact")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("lastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("lname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("mname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("registeredDate")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.ToTable("Owners");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Records.Pet", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int?>("age")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<DateTime?>("bdate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("breed")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("gender")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<DateTime?>("lastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ownerId")
                        .HasColumnType("int");

                    b.Property<string>("petName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("registeredDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("species")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("ownerId");

                    b.ToTable("Pets");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Records.Product", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("lastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("productName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("quantity")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<DateTime?>("registeredDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<double?>("sellingPrice")
                        .IsRequired()
                        .HasColumnType("float");

                    b.Property<double?>("supplierPrice")
                        .IsRequired()
                        .HasColumnType("float");

                    b.HasKey("id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Records.Services", b =>
                {
                    b.Property<int>("serviceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("serviceId"));

                    b.Property<bool>("followUp")
                        .HasColumnType("bit");

                    b.Property<double>("serviceCharge")
                        .HasColumnType("float");

                    b.Property<string>("serviceName")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("serviceType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("serviceId");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Records.StockAdjustment", b =>
                {
                    b.Property<int>("stockAdj_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("stockAdj_Id"));

                    b.Property<int?>("billingNavbillingId")
                        .HasColumnType("int");

                    b.Property<int?>("billing_Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("productId")
                        .HasColumnType("int");

                    b.Property<string>("source")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("stock")
                        .HasColumnType("int");

                    b.HasKey("stockAdj_Id");

                    b.HasIndex("billingNavbillingId");

                    b.HasIndex("productId");

                    b.ToTable("StockAdjustments");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Records.UserAccount", b =>
                {
                    b.Property<int>("acc_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("acc_Id"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPasswordChanged")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("bdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("contact")
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<DateTime?>("dateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("mname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("passWord")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("userName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("userType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("acc_Id");

                    b.ToTable("UserAccounts");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Transactions.Billing", b =>
                {
                    b.Property<int>("billingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("billingId"));

                    b.Property<int?>("DiagnosticsId")
                        .HasColumnType("int");

                    b.Property<int?>("PurchaseId")
                        .HasColumnType("int");

                    b.Property<double?>("cashReceive")
                        .HasColumnType("float");

                    b.Property<double?>("changeAmount")
                        .HasColumnType("float");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.Property<double?>("grandTotal")
                        .HasColumnType("float");

                    b.HasKey("billingId");

                    b.HasIndex("DiagnosticsId");

                    b.HasIndex("PurchaseId");

                    b.ToTable("Billings");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Transactions.DiagnosticDetails", b =>
                {
                    b.Property<int>("diagnosticDet_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("diagnosticDet_Id"));

                    b.Property<string>("details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("diagnosticsId")
                        .HasColumnType("int");

                    b.Property<int>("serviceId")
                        .HasColumnType("int");

                    b.Property<double>("totalServicePayment")
                        .HasColumnType("float");

                    b.HasKey("diagnosticDet_Id");

                    b.HasIndex("diagnosticsId");

                    b.HasIndex("serviceId");

                    b.ToTable("DiagnosticDetails");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Transactions.Diagnostics", b =>
                {
                    b.Property<int>("diagnostic_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("diagnostic_Id"));

                    b.Property<int?>("PurchaseNavpurchaseId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("date")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<int>("petId")
                        .HasColumnType("int");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("weight")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.HasKey("diagnostic_Id");

                    b.HasIndex("PurchaseNavpurchaseId");

                    b.HasIndex("petId");

                    b.ToTable("Diagnostics");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Transactions.Purchase", b =>
                {
                    b.Property<int>("purchaseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("purchaseId"));

                    b.Property<string>("customerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("diagnosisId_holder")
                        .HasColumnType("int");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("purchaseId");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Transactions.PurchaseDetails", b =>
                {
                    b.Property<int>("purchaseDet_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("purchaseDet_Id"));

                    b.Property<int>("productId")
                        .HasColumnType("int");

                    b.Property<int>("purchaseId")
                        .HasColumnType("int");

                    b.Property<int>("quantity")
                        .HasColumnType("int");

                    b.Property<double>("sellingPrice")
                        .HasColumnType("float");

                    b.HasKey("purchaseDet_Id");

                    b.HasIndex("productId");

                    b.HasIndex("purchaseId");

                    b.ToTable("PurchaseDetails");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Appointments.Appointment", b =>
                {
                    b.HasOne("PAWS_NDV_PetLovers.Models.Records.Owner", "OwnerNav")
                        .WithMany()
                        .HasForeignKey("ownerId");

                    b.Navigation("OwnerNav");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Appointments.AppointmentDetails", b =>
                {
                    b.HasOne("PAWS_NDV_PetLovers.Models.Appointments.Appointment", "Appointment")
                        .WithMany("IAppDetails")
                        .HasForeignKey("AppointId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PAWS_NDV_PetLovers.Models.Records.Services", "Services")
                        .WithMany()
                        .HasForeignKey("serviceID");

                    b.Navigation("Appointment");

                    b.Navigation("Services");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Appointments.PetFollowUps", b =>
                {
                    b.HasOne("PAWS_NDV_PetLovers.Models.Transactions.Diagnostics", "Diagnostics")
                        .WithMany("IPetFollowUps")
                        .HasForeignKey("diagnosticsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PAWS_NDV_PetLovers.Models.Records.Services", "Services")
                        .WithMany()
                        .HasForeignKey("serviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Diagnostics");

                    b.Navigation("Services");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Records.Pet", b =>
                {
                    b.HasOne("PAWS_NDV_PetLovers.Models.Records.Owner", "owner")
                        .WithMany("Pets")
                        .HasForeignKey("ownerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("owner");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Records.Product", b =>
                {
                    b.HasOne("PAWS_NDV_PetLovers.Models.Records.Category", "category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("category");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Records.StockAdjustment", b =>
                {
                    b.HasOne("PAWS_NDV_PetLovers.Models.Transactions.Billing", "billingNav")
                        .WithMany("stockAdjustments")
                        .HasForeignKey("billingNavbillingId");

                    b.HasOne("PAWS_NDV_PetLovers.Models.Records.Product", "productsNav")
                        .WithMany("stockAdjustmentNav")
                        .HasForeignKey("productId");

                    b.Navigation("billingNav");

                    b.Navigation("productsNav");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Transactions.Billing", b =>
                {
                    b.HasOne("PAWS_NDV_PetLovers.Models.Transactions.Diagnostics", "diagnostics")
                        .WithMany()
                        .HasForeignKey("DiagnosticsId");

                    b.HasOne("PAWS_NDV_PetLovers.Models.Transactions.Purchase", "purchase")
                        .WithMany()
                        .HasForeignKey("PurchaseId");

                    b.Navigation("diagnostics");

                    b.Navigation("purchase");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Transactions.DiagnosticDetails", b =>
                {
                    b.HasOne("PAWS_NDV_PetLovers.Models.Transactions.Diagnostics", null)
                        .WithMany("IdiagnosticDetails")
                        .HasForeignKey("diagnosticsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PAWS_NDV_PetLovers.Models.Records.Services", "Services")
                        .WithMany()
                        .HasForeignKey("serviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Services");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Transactions.Diagnostics", b =>
                {
                    b.HasOne("PAWS_NDV_PetLovers.Models.Transactions.Purchase", "PurchaseNav")
                        .WithMany()
                        .HasForeignKey("PurchaseNavpurchaseId");

                    b.HasOne("PAWS_NDV_PetLovers.Models.Records.Pet", "pet")
                        .WithMany()
                        .HasForeignKey("petId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PurchaseNav");

                    b.Navigation("pet");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Transactions.PurchaseDetails", b =>
                {
                    b.HasOne("PAWS_NDV_PetLovers.Models.Records.Product", "product")
                        .WithMany()
                        .HasForeignKey("productId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PAWS_NDV_PetLovers.Models.Transactions.Purchase", "Purchase")
                        .WithMany("purchaseDetails")
                        .HasForeignKey("purchaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Purchase");

                    b.Navigation("product");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Appointments.Appointment", b =>
                {
                    b.Navigation("IAppDetails");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Records.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Records.Owner", b =>
                {
                    b.Navigation("Pets");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Records.Product", b =>
                {
                    b.Navigation("stockAdjustmentNav");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Transactions.Billing", b =>
                {
                    b.Navigation("stockAdjustments");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Transactions.Diagnostics", b =>
                {
                    b.Navigation("IPetFollowUps");

                    b.Navigation("IdiagnosticDetails");
                });

            modelBuilder.Entity("PAWS_NDV_PetLovers.Models.Transactions.Purchase", b =>
                {
                    b.Navigation("purchaseDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
