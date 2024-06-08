﻿// <auto-generated />
using System;
using Autoglass.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Autoglass.Infra.Data.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Autoglass.Domain.Entities.Produto", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("CnpjFornecedor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CodigoFornecedor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DataFabricacao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataValidade")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DescricaoFornecedor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Situcao")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Produto", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
