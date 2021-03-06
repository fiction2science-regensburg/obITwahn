﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ObITwahn.Trinity.Services.Web.Data;

namespace ObITwahn.Trinity.Services.Web.Migrations
{
    [DbContext(typeof(TrinityContext))]
    [Migration("20180928151716_EmployeeMeetings2")]
    partial class EmployeeMeetings2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.2-rtm-30932")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ObITwahn.Services.Meeting.Model.Employee", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Department");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Phone");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Emloyees");
                });

            modelBuilder.Entity("ObITwahn.Services.Meeting.Model.EmployeeMeeting", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("EmployeeId");

                    b.Property<Guid?>("MeetingId");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("MeetingId");

                    b.ToTable("EmployeeMeeting");
                });

            modelBuilder.Entity("ObITwahn.Services.Meeting.Model.Meeting", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<DateTime?>("End")
                        .IsRequired();

                    b.Property<string>("Minutes");

                    b.Property<DateTime?>("Start")
                        .IsRequired();

                    b.Property<string>("Subject")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Meetings");
                });

            modelBuilder.Entity("ObITwahn.Services.Meeting.Model.EmployeeMeeting", b =>
                {
                    b.HasOne("ObITwahn.Services.Meeting.Model.Employee", "Employee")
                        .WithMany("Meetings")
                        .HasForeignKey("EmployeeId");

                    b.HasOne("ObITwahn.Services.Meeting.Model.Meeting", "Meeting")
                        .WithMany("Participants")
                        .HasForeignKey("MeetingId");
                });
#pragma warning restore 612, 618
        }
    }
}
