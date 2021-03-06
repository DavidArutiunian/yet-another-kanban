﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using server;

namespace server.Migrations
{
    [DbContext(typeof(KanbanContext))]
    partial class KanbanContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("server.Models.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Context")
                        .IsRequired()
                        .HasColumnName("content")
                        .HasColumnType("character varying(1024)")
                        .HasMaxLength(1024);

                    b.Property<int>("IdColumn")
                        .HasColumnName("id_column")
                        .HasColumnType("integer");

                    b.Property<int>("Order")
                        .HasColumnName("order")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("IdColumn");

                    b.ToTable("card");
                });

            modelBuilder.Entity("server.Models.Desk", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("description")
                        .HasColumnType("character varying(1024)")
                        .HasMaxLength(1024);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("desk");
                });

            modelBuilder.Entity("server.Models.DeskColumn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("IdDesk")
                        .HasColumnName("id_desk")
                        .HasColumnType("integer");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnName("label")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<int>("Order")
                        .HasColumnName("order")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("IdDesk");

                    b.ToTable("desk_column");
                });

            modelBuilder.Entity("server.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnName("login")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("password")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("user_account");
                });

            modelBuilder.Entity("server.Models.UserHasDesk", b =>
                {
                    b.Property<int>("IdDesk")
                        .HasColumnName("id_desk")
                        .HasColumnType("integer");

                    b.Property<int>("IdUser")
                        .HasColumnName("id_user")
                        .HasColumnType("integer");

                    b.HasKey("IdDesk", "IdUser");

                    b.HasIndex("IdUser");

                    b.ToTable("user_x_desk");
                });

            modelBuilder.Entity("server.Models.Card", b =>
                {
                    b.HasOne("server.Models.DeskColumn", "DeskColumn")
                        .WithMany("Cards")
                        .HasForeignKey("IdColumn")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("server.Models.DeskColumn", b =>
                {
                    b.HasOne("server.Models.Desk", "Desk")
                        .WithMany("DeskColumns")
                        .HasForeignKey("IdDesk")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("server.Models.UserHasDesk", b =>
                {
                    b.HasOne("server.Models.Desk", "Desk")
                        .WithMany()
                        .HasForeignKey("IdDesk")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("server.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
