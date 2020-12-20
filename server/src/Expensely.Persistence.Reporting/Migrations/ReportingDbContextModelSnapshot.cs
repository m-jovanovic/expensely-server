﻿// <auto-generated />
using System;
using Expensely.Persistence.Reporting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Expensely.Persistence.Reporting.Migrations
{
    [DbContext(typeof(ReportingDbContext))]
    partial class ReportingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Expensely.Domain.Reporting.Transactions.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasPrecision(12, 4)
                        .HasColumnType("decimal(12,4)");

                    b.Property<int>("Currency")
                        .HasColumnType("int");

                    b.Property<DateTime>("OccurredOn")
                        .HasColumnType("date");

                    b.Property<int>("TransactionType")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Transaction", t => t.ExcludeFromMigrations());
                });

            modelBuilder.Entity("Expensely.Domain.Reporting.Transactions.TransactionSummary", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasPrecision(12, 4)
                        .HasColumnType("decimal(12,4)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("Currency")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<int>("TransactionType")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .IsClustered(false);

                    b.HasIndex("UserId")
                        .IsClustered();

                    b.ToTable("TransactionSummary");
                });

            modelBuilder.Entity("Expensely.Domain.Reporting.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("User", t => t.ExcludeFromMigrations());
                });

            modelBuilder.Entity("Expensely.Domain.Reporting.Transactions.Transaction", b =>
                {
                    b.HasOne("Expensely.Domain.Reporting.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Expensely.Domain.Reporting.Transactions.TransactionSummary", b =>
                {
                    b.HasOne("Expensely.Domain.Reporting.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
