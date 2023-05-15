﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Data;
using Data.Auth.Credentials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Core.Domains.Auth.AuthActivity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Agent")
                        .HasColumnType("text")
                        .HasColumnName("agent");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date_utc");

                    b.Property<string>("IpAddress")
                        .HasColumnType("text")
                        .HasColumnName("ip_address");

                    b.HasKey("Id");

                    b.HasIndex("Agent");

                    b.ToTable("auth_activities", (string)null);
                });

            modelBuilder.Entity("Core.Domains.Auth.Credentials.Credential", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date_utc");

                    b.Property<DateTime?>("ExpiresAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expires_at_utc");

                    b.Property<Guid?>("FirstActivityId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("LastActivityId")
                        .HasColumnType("uuid");

                    b.Property<bool>("OneTime")
                        .HasColumnType("boolean")
                        .HasColumnName("one_time");

                    b.Property<PasswordType>("Password")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int?>("SessionExpireTimeInMinutes")
                        .HasColumnType("integer")
                        .HasColumnName("session_expire_time_in_minutes");

                    b.Property<bool>("SingleSession")
                        .HasColumnType("boolean")
                        .HasColumnName("single_session");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.Property<Guid?>("first_activity_id")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("last_activity_id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("user_id")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Username");

                    b.HasIndex("first_activity_id");

                    b.HasIndex("last_activity_id");

                    b.HasIndex("user_id");

                    b.ToTable("credentials", (string)null);
                });

            modelBuilder.Entity("Core.Domains.Auth.Sessions.Session", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<DateTime>("ExpirationDateUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expiration_date_utc");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("refresh_token");

                    b.Property<Guid>("activity_id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("credential_id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("scope_id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("user_id")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RefreshToken")
                        .IsUnique();

                    b.HasIndex("activity_id");

                    b.HasIndex("credential_id");

                    b.HasIndex("scope_id");

                    b.HasIndex("user_id");

                    b.ToTable("sessions", (string)null);
                });

            modelBuilder.Entity("Core.Domains.Groups.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<DateTime>("StatusChangeDateUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("status_change_date_utc");

                    b.Property<Guid?>("parent_id")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("Status");

                    b.HasIndex("parent_id")
                        .IsUnique();

                    b.ToTable("groups", (string)null);
                });

            modelBuilder.Entity("Core.Domains.Permissions.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Dictionary<string, string>>("Meta")
                        .HasColumnType("jsonb")
                        .HasColumnName("meta");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("permissions", (string)null);
                });

            modelBuilder.Entity("Core.Domains.Roles.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Dictionary<string, string>>("Meta")
                        .HasColumnType("jsonb")
                        .HasColumnName("meta");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("roles", (string)null);
                });

            modelBuilder.Entity("Core.Domains.Scopes.Scope", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Secret")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("secret");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("Secret")
                        .IsUnique();

                    b.HasIndex("Status");

                    b.ToTable("scopes", (string)null);
                });

            modelBuilder.Entity("Core.Domains.Services.Service", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Secret")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("secret");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("Secret")
                        .IsUnique();

                    b.HasIndex("Status");

                    b.ToTable("services", (string)null);
                });

            modelBuilder.Entity("Core.Domains.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<DateTime>("StatusChangeDateUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("status_change_date_utc");

                    b.Property<string>("StatusChangeReason")
                        .HasColumnType("text")
                        .HasColumnName("status_change_reason");

                    b.Property<List<string>>("Usernames")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.HasIndex("Status");

                    b.HasIndex("Usernames");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("groups_children", b =>
                {
                    b.Property<Guid>("child_id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("group_id")
                        .HasColumnType("uuid");

                    b.HasKey("child_id", "group_id");

                    b.HasIndex("group_id");

                    b.ToTable("groups_children");
                });

            modelBuilder.Entity("groups_roles", b =>
                {
                    b.Property<Guid>("group_id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("role_id")
                        .HasColumnType("uuid");

                    b.HasKey("group_id", "role_id");

                    b.HasIndex("role_id");

                    b.ToTable("groups_roles");
                });

            modelBuilder.Entity("roles_permission", b =>
                {
                    b.Property<Guid>("permission_id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("role_id")
                        .HasColumnType("uuid");

                    b.HasKey("permission_id", "role_id");

                    b.HasIndex("role_id");

                    b.ToTable("roles_permission");
                });

            modelBuilder.Entity("scopes_roles", b =>
                {
                    b.Property<Guid>("role_id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("scope_id")
                        .HasColumnType("uuid");

                    b.HasKey("role_id", "scope_id");

                    b.HasIndex("scope_id");

                    b.ToTable("scopes_roles");
                });

            modelBuilder.Entity("scopes_services", b =>
                {
                    b.Property<Guid>("scope_id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("service_id")
                        .HasColumnType("uuid");

                    b.HasKey("scope_id", "service_id");

                    b.HasIndex("service_id");

                    b.ToTable("scopes_services");
                });

            modelBuilder.Entity("users_groups", b =>
                {
                    b.Property<Guid>("group_id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("user_id")
                        .HasColumnType("uuid");

                    b.HasKey("group_id", "user_id");

                    b.HasIndex("user_id");

                    b.ToTable("users_groups");
                });

            modelBuilder.Entity("users_roles", b =>
                {
                    b.Property<Guid>("role_id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("user_id")
                        .HasColumnType("uuid");

                    b.HasKey("role_id", "user_id");

                    b.HasIndex("user_id");

                    b.ToTable("users_roles");
                });

            modelBuilder.Entity("Core.Domains.Auth.Credentials.Credential", b =>
                {
                    b.HasOne("Core.Domains.Auth.AuthActivity", "FirstActivity")
                        .WithMany()
                        .HasForeignKey("first_activity_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Core.Domains.Auth.AuthActivity", "LastActivity")
                        .WithMany()
                        .HasForeignKey("last_activity_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Core.Domains.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("FirstActivity");

                    b.Navigation("LastActivity");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Core.Domains.Auth.Sessions.Session", b =>
                {
                    b.HasOne("Core.Domains.Auth.AuthActivity", "Activity")
                        .WithMany()
                        .HasForeignKey("activity_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domains.Auth.Credentials.Credential", "Credential")
                        .WithMany()
                        .HasForeignKey("credential_id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Core.Domains.Scopes.Scope", "Scope")
                        .WithMany()
                        .HasForeignKey("scope_id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Core.Domains.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Activity");

                    b.Navigation("Credential");

                    b.Navigation("Scope");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Core.Domains.Groups.Group", b =>
                {
                    b.HasOne("Core.Domains.Groups.Group", "Parent")
                        .WithOne()
                        .HasForeignKey("Core.Domains.Groups.Group", "parent_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("groups_children", b =>
                {
                    b.HasOne("Core.Domains.Groups.Group", null)
                        .WithMany()
                        .HasForeignKey("child_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domains.Groups.Group", null)
                        .WithMany()
                        .HasForeignKey("group_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("groups_roles", b =>
                {
                    b.HasOne("Core.Domains.Groups.Group", null)
                        .WithMany()
                        .HasForeignKey("group_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domains.Roles.Role", null)
                        .WithMany()
                        .HasForeignKey("role_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("roles_permission", b =>
                {
                    b.HasOne("Core.Domains.Permissions.Permission", null)
                        .WithMany()
                        .HasForeignKey("permission_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domains.Roles.Role", null)
                        .WithMany()
                        .HasForeignKey("role_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("scopes_roles", b =>
                {
                    b.HasOne("Core.Domains.Roles.Role", null)
                        .WithMany()
                        .HasForeignKey("role_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domains.Scopes.Scope", null)
                        .WithMany()
                        .HasForeignKey("scope_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("scopes_services", b =>
                {
                    b.HasOne("Core.Domains.Scopes.Scope", null)
                        .WithMany()
                        .HasForeignKey("scope_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domains.Services.Service", null)
                        .WithMany()
                        .HasForeignKey("service_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("users_groups", b =>
                {
                    b.HasOne("Core.Domains.Groups.Group", null)
                        .WithMany()
                        .HasForeignKey("group_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domains.Users.User", null)
                        .WithMany()
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("users_roles", b =>
                {
                    b.HasOne("Core.Domains.Roles.Role", null)
                        .WithMany()
                        .HasForeignKey("role_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domains.Users.User", null)
                        .WithMany()
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}