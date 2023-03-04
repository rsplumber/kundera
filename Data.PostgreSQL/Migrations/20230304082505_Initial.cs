using System;
using System.Collections.Generic;
using Data.Auth.Credentials;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "auth_activities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ip_address = table.Column<string>(type: "text", nullable: true),
                    agent = table.Column<string>(type: "text", nullable: true),
                    created_date_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auth_activities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "groups",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    status_change_date_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groups", x => x.id);
                    table.ForeignKey(
                        name: "FK_groups_groups_ParentId",
                        column: x => x.ParentId,
                        principalTable: "groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    meta = table.Column<Dictionary<string, string>>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    meta = table.Column<Dictionary<string, string>>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "scopes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    secret = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scopes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    secret = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_services", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    Usernames = table.Column<List<string>>(type: "text[]", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    status_change_reason = table.Column<string>(type: "text", nullable: true),
                    status_change_date_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "groups_children",
                columns: table => new
                {
                    ChildrenId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groups_children", x => new { x.ChildrenId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_groups_children_groups_ChildrenId",
                        column: x => x.ChildrenId,
                        principalTable: "groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_groups_children_groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "groups_roles",
                columns: table => new
                {
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    RolesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groups_roles", x => new { x.GroupId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_groups_roles_groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_groups_roles_roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "roles_permission",
                columns: table => new
                {
                    PermissionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles_permission", x => new { x.PermissionsId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_roles_permission_permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_roles_permission_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "scopes_roles",
                columns: table => new
                {
                    RolesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScopeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scopes_roles", x => new { x.RolesId, x.ScopeId });
                    table.ForeignKey(
                        name: "FK_scopes_roles_roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_scopes_roles_scopes_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "scopes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "scopes_services",
                columns: table => new
                {
                    ScopeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServicesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scopes_services", x => new { x.ScopeId, x.ServicesId });
                    table.ForeignKey(
                        name: "FK_scopes_services_scopes_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "scopes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_scopes_services_services_ServicesId",
                        column: x => x.ServicesId,
                        principalTable: "services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "credentials",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Password = table.Column<PasswordType>(type: "jsonb", nullable: false),
                    FirstActivityId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastActivityId = table.Column<Guid>(type: "uuid", nullable: true),
                    expires_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    session_expire_time_in_minutes = table.Column<int>(type: "integer", nullable: true),
                    one_time = table.Column<bool>(type: "boolean", nullable: false),
                    single_session = table.Column<bool>(type: "boolean", nullable: false),
                    created_date_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credentials", x => x.id);
                    table.ForeignKey(
                        name: "FK_credentials_auth_activities_FirstActivityId",
                        column: x => x.FirstActivityId,
                        principalTable: "auth_activities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_credentials_auth_activities_LastActivityId",
                        column: x => x.LastActivityId,
                        principalTable: "auth_activities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_credentials_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "users_groups",
                columns: table => new
                {
                    GroupsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_groups", x => new { x.GroupsId, x.UserId });
                    table.ForeignKey(
                        name: "FK_users_groups_groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_groups_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users_roles",
                columns: table => new
                {
                    RolesId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_roles", x => new { x.RolesId, x.UserId });
                    table.ForeignKey(
                        name: "FK_users_roles_roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_roles_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    refresh_token = table.Column<string>(type: "text", nullable: false),
                    CredentialId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScopeId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    expiration_date_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActivityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessions", x => x.id);
                    table.ForeignKey(
                        name: "FK_sessions_auth_activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "auth_activities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sessions_credentials_CredentialId",
                        column: x => x.CredentialId,
                        principalTable: "credentials",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_sessions_scopes_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "scopes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_sessions_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_auth_activities_agent",
                table: "auth_activities",
                column: "agent");

            migrationBuilder.CreateIndex(
                name: "IX_credentials_FirstActivityId",
                table: "credentials",
                column: "FirstActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_credentials_LastActivityId",
                table: "credentials",
                column: "LastActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_credentials_UserId",
                table: "credentials",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_credentials_username",
                table: "credentials",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "IX_groups_name",
                table: "groups",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_groups_ParentId",
                table: "groups",
                column: "ParentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_groups_status",
                table: "groups",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_groups_children_GroupId",
                table: "groups_children",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_groups_roles_RolesId",
                table: "groups_roles",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_permissions_name",
                table: "permissions",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_roles_name",
                table: "roles",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_roles_permission_RoleId",
                table: "roles_permission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_scopes_name",
                table: "scopes",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_scopes_secret",
                table: "scopes",
                column: "secret");

            migrationBuilder.CreateIndex(
                name: "IX_scopes_status",
                table: "scopes",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_scopes_roles_ScopeId",
                table: "scopes_roles",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_scopes_services_ServicesId",
                table: "scopes_services",
                column: "ServicesId");

            migrationBuilder.CreateIndex(
                name: "IX_services_name",
                table: "services",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_services_secret",
                table: "services",
                column: "secret");

            migrationBuilder.CreateIndex(
                name: "IX_services_status",
                table: "services",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_ActivityId",
                table: "sessions",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_CredentialId",
                table: "sessions",
                column: "CredentialId");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_refresh_token",
                table: "sessions",
                column: "refresh_token");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_ScopeId",
                table: "sessions",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_UserId",
                table: "sessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_status",
                table: "users",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_users_Usernames",
                table: "users",
                column: "Usernames");

            migrationBuilder.CreateIndex(
                name: "IX_users_groups_UserId",
                table: "users_groups",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_roles_UserId",
                table: "users_roles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "groups_children");

            migrationBuilder.DropTable(
                name: "groups_roles");

            migrationBuilder.DropTable(
                name: "roles_permission");

            migrationBuilder.DropTable(
                name: "scopes_roles");

            migrationBuilder.DropTable(
                name: "scopes_services");

            migrationBuilder.DropTable(
                name: "sessions");

            migrationBuilder.DropTable(
                name: "users_groups");

            migrationBuilder.DropTable(
                name: "users_roles");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "services");

            migrationBuilder.DropTable(
                name: "credentials");

            migrationBuilder.DropTable(
                name: "scopes");

            migrationBuilder.DropTable(
                name: "groups");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "auth_activities");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
