﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProvidenceBotDAL;

namespace ProvidenceBotDAL.Migrations.Migrations
{
  [DbContext(typeof(SuggestContext))]
  partial class SuggestContextModelSnapshot : ModelSnapshot
  {
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
      modelBuilder
          .HasAnnotation("Relational:MaxIdentifierLength", 128)
          .HasAnnotation("ProductVersion", "5.0.6")
          .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

      modelBuilder.Entity("ProvidenceBotDAL.Models.Item.Suggestion", b =>
          {
            b.Property<decimal>("ID")
                      .ValueGeneratedOnAdd()
                      .HasColumnType("decimal(20,0)")
                      .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);

            b.Property<decimal>("Author")
                      .HasColumnType("decimal(20,0)");

            b.Property<DateTime>("Date")
                      .HasColumnType("datetime2");

            b.Property<string>("Description")
                      .HasColumnType("nvarchar(max)");

            b.Property<string>("Keywords")
                      .HasColumnType("nvarchar(max)");

            b.Property<int>("Number")
                      .HasColumnType("int");

            b.Property<string>("Title")
                      .HasColumnType("nvarchar(max)");

            b.HasKey("ID");

            b.ToTable("Suggestion");
          });
#pragma warning restore 612, 618
    }
  }
}