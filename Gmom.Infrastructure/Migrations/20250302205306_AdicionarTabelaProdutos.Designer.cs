﻿// <auto-generated />
using Gmom.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Gmom.Infrastructure.Migrations
{
    [DbContext(typeof(PostgresContext))]
    [Migration("20250302205306_AdicionarTabelaProdutos")]
    partial class AdicionarTabelaProdutos
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseSerialColumns(modelBuilder);

            modelBuilder.Entity("Gmom.Domain.Entities.ProductEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<int>("Id"));

                    b.Property<string>("BarCode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("codigo_barras");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("nome");

                    b.Property<double>("Price")
                        .HasColumnType("double precision")
                        .HasColumnName("preco");

                    b.Property<double>("Stock")
                        .HasColumnType("double precision")
                        .HasColumnName("estoque");

                    b.HasKey("Id");

                    b.HasIndex("BarCode")
                        .IsUnique();

                    b.ToTable("produtos");
                });

            modelBuilder.Entity("Gmom.Domain.Entities.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean")
                        .HasColumnName("administrador");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("nome");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("senha");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("usuarios");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsAdmin = true,
                            Name = "ADMIN",
                            Password = "21232f297a57a5a743894a0e4a801fc3"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
