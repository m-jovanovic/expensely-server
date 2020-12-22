﻿// <auto-generated />
using System;
using Expensely.Persistence.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Expensely.Persistence.Application.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20201222152540_AddCreatedOnUtcToMessageConsumer")]
    partial class AddCreatedOnUtcToMessageConsumer
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Expensely.Domain.Core.Budget", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("date");

                    b.Property<bool>("Expired")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModifiedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("date");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Budget");
                });

            modelBuilder.Entity("Expensely.Domain.Core.RefreshToken", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ExpiresOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("UserId");

                    b.HasIndex("Token");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("Expensely.Domain.Core.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("OccurredOn")
                        .HasColumnType("date");

                    b.Property<int>("TransactionType")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Transaction");

                    b.HasDiscriminator<int>("TransactionType");
                });

            modelBuilder.Entity("Expensely.Domain.Core.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasMaxLength(201)
                        .HasColumnType("nvarchar(201)")
                        .HasComputedColumnSql("[FirstName] + ' ' + [LastName]", true);

                    b.Property<DateTime?>("ModifiedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("_passwordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("PasswordHash");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Expensely.Messaging.Abstractions.Entities.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("Processed")
                        .HasColumnType("bit");

                    b.Property<int>("Retries")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("Expensely.Messaging.Abstractions.Entities.MessageConsumer", b =>
                {
                    b.Property<Guid>("MessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConsumerName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("MessageId", "ConsumerName");

                    b.ToTable("MessageConsumer");
                });

            modelBuilder.Entity("Expensely.Domain.Core.Expense", b =>
                {
                    b.HasBaseType("Expensely.Domain.Core.Transaction");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("Expensely.Domain.Core.Income", b =>
                {
                    b.HasBaseType("Expensely.Domain.Core.Transaction");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("Expensely.Domain.Core.Budget", b =>
                {
                    b.HasOne("Expensely.Domain.Core.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Expensely.Domain.Core.Money", "Money", b1 =>
                        {
                            b1.Property<Guid>("BudgetId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasPrecision(12, 4)
                                .HasColumnType("decimal(12,4)")
                                .HasColumnName("Amount");

                            b1.HasKey("BudgetId");

                            b1.ToTable("Budget");

                            b1.WithOwner()
                                .HasForeignKey("BudgetId");

                            b1.OwnsOne("Expensely.Domain.Core.Currency", "Currency", b2 =>
                                {
                                    b2.Property<Guid>("MoneyBudgetId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<int>("Value")
                                        .HasColumnType("int")
                                        .HasColumnName("Currency");

                                    b2.HasKey("MoneyBudgetId");

                                    b2.ToTable("Budget");

                                    b2.WithOwner()
                                        .HasForeignKey("MoneyBudgetId");
                                });

                            b1.Navigation("Currency")
                                .IsRequired();
                        });

                    b.OwnsOne("Expensely.Domain.Core.Name", "Name", b1 =>
                        {
                            b1.Property<Guid>("BudgetId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("Name");

                            b1.HasKey("BudgetId");

                            b1.ToTable("Budget");

                            b1.WithOwner()
                                .HasForeignKey("BudgetId");
                        });

                    b.Navigation("Money")
                        .IsRequired();

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("Expensely.Domain.Core.RefreshToken", b =>
                {
                    b.HasOne("Expensely.Domain.Core.User", null)
                        .WithOne()
                        .HasForeignKey("Expensely.Domain.Core.RefreshToken", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Expensely.Domain.Core.Transaction", b =>
                {
                    b.HasOne("Expensely.Domain.Core.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Expensely.Domain.Core.Description", "Description", b1 =>
                        {
                            b1.Property<Guid>("TransactionId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(500)
                                .HasColumnType("nvarchar(500)")
                                .HasColumnName("Description");

                            b1.HasKey("TransactionId");

                            b1.ToTable("Transaction");

                            b1.WithOwner()
                                .HasForeignKey("TransactionId");
                        });

                    b.OwnsOne("Expensely.Domain.Core.Money", "Money", b1 =>
                        {
                            b1.Property<Guid>("TransactionId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasPrecision(12, 4)
                                .HasColumnType("decimal(12,4)")
                                .HasColumnName("Amount");

                            b1.HasKey("TransactionId");

                            b1.ToTable("Transaction");

                            b1.WithOwner()
                                .HasForeignKey("TransactionId");

                            b1.OwnsOne("Expensely.Domain.Core.Currency", "Currency", b2 =>
                                {
                                    b2.Property<Guid>("MoneyTransactionId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<int>("Value")
                                        .HasColumnType("int")
                                        .HasColumnName("Currency");

                                    b2.HasKey("MoneyTransactionId");

                                    b2.ToTable("Transaction");

                                    b2.WithOwner()
                                        .HasForeignKey("MoneyTransactionId");
                                });

                            b1.Navigation("Currency")
                                .IsRequired();
                        });

                    b.OwnsOne("Expensely.Domain.Core.Name", "Name", b1 =>
                        {
                            b1.Property<Guid>("TransactionId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("Name");

                            b1.HasKey("TransactionId");

                            b1.ToTable("Transaction");

                            b1.WithOwner()
                                .HasForeignKey("TransactionId");
                        });

                    b.Navigation("Description")
                        .IsRequired();

                    b.Navigation("Money")
                        .IsRequired();

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("Expensely.Domain.Core.User", b =>
                {
                    b.OwnsMany("Expensely.Domain.Core.Currency", "Currencies", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Value")
                                .HasColumnType("int")
                                .HasColumnName("Currency");

                            b1.HasKey("UserId", "Value");

                            b1.ToTable("UserCurrency");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("Expensely.Domain.Core.Currency", "_primaryCurrency", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Value")
                                .HasColumnType("int")
                                .HasColumnName("PrimaryCurrency");

                            b1.HasKey("UserId");

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("Expensely.Domain.Core.Email", "Email", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(256)
                                .HasColumnType("nvarchar(256)")
                                .HasColumnName("Email");

                            b1.HasKey("UserId");

                            b1.HasIndex("Value")
                                .IsUnique()
                                .HasFilter("[Email] IS NOT NULL");

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("Expensely.Domain.Core.FirstName", "FirstName", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("FirstName");

                            b1.HasKey("UserId");

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("Expensely.Domain.Core.LastName", "LastName", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("LastName");

                            b1.HasKey("UserId");

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("_primaryCurrency");

                    b.Navigation("Currencies");

                    b.Navigation("Email")
                        .IsRequired();

                    b.Navigation("FirstName")
                        .IsRequired();

                    b.Navigation("LastName")
                        .IsRequired();
                });

            modelBuilder.Entity("Expensely.Messaging.Abstractions.Entities.MessageConsumer", b =>
                {
                    b.HasOne("Expensely.Messaging.Abstractions.Entities.Message", null)
                        .WithMany()
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
