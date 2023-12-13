using System.Text.Json;
using Core.Auth.Credentials;
using Core.Auth.Sessions;
using Core.Groups;
using Core.Permissions;
using Core.Roles;
using Core.Scopes;
using Core.Services;
using Core.Users;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data;

public sealed class AppDbContext : DbContext
{
    private readonly ICapPublisher _eventBus;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        IgnoreReadOnlyFields = true
    };

    public AppDbContext(DbContextOptions<AppDbContext> options, ICapPublisher eventBus) : base(options)
    {
        _eventBus = eventBus;
    }

    public DbSet<Credential> Credentials { get; set; }

    public DbSet<Session> Sessions { get; set; }

    public DbSet<Group> Groups { get; set; }

    public DbSet<Permission> Permissions { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Scope> Scopes { get; set; }

    public DbSet<Service> Services { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<AuthenticationActivity> AuthenticationActivities { get; set; }

    public DbSet<AuthorizationActivity> AuthorizationActivities { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new AuthenticationActivityEntityTypeConfiguration());
        builder.ApplyConfiguration(new AuthorizationActivityEntityTypeConfiguration());
        builder.ApplyConfiguration(new CredentialEntityTypeConfiguration());
        builder.ApplyConfiguration(new SessionEntityTypeConfiguration());
        builder.ApplyConfiguration(new UserEntityTypeConfiguration());
        builder.ApplyConfiguration(new GroupEntityTypeConfiguration());
        builder.ApplyConfiguration(new RoleEntityTypeConfiguration());
        builder.ApplyConfiguration(new PermissionEntityTypeConfiguration());
        builder.ApplyConfiguration(new ScopeEntityTypeConfiguration());
        builder.ApplyConfiguration(new ServiceEntityTypeConfiguration());
        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _eventBus.DispatchDomainEventsAsync(this);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private class CredentialEntityTypeConfiguration : IEntityTypeConfiguration<Credential>
    {
        public void Configure(EntityTypeBuilder<Credential> builder)
        {
            builder.ToTable("credentials")
                .HasKey(model => model.Id);

            builder.Property(model => model.Id)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("id");

            builder.Property(model => model.Username)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("username");

            builder.HasIndex(model => model.Username).IsUnique(false);

            builder.HasOne(model => model.User)
                .WithMany()
                .HasForeignKey("user_id")
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(b => b.Password)
                .HasConversion(password => JsonSerializer.Serialize(password, JsonSerializerOptions),
                    s => JsonSerializer.Deserialize<Password>(s, JsonSerializerOptions)!)
                .HasColumnName("Password");

            builder.Property(model => model.SingleSession)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("single_session");

            builder.Property(model => model.SessionTokenExpireTimeInMinutes)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("session_token_expire_time_in_minutes")
                .IsRequired(false);

            builder.Property(model => model.SessionRefreshTokenExpireTimeInMinutes)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("session_refresh_token_expire_time_in_minutes")
                .IsRequired(false);

            builder.Property(model => model.OneTime)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("one_time");

            builder.Property(model => model.ExpiresAtUtc)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("expires_at_utc")
                .IsRequired(false);

            builder.Property(model => model.CreatedDateUtc)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("created_date_utc");
        }
    }

    private class SessionEntityTypeConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("sessions")
                .HasKey(model => model.Id);

            builder.Property(model => model.Id)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("id");

            builder.Property(model => model.RefreshToken)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("refresh_token");

            builder.HasIndex(model => model.RefreshToken).IsUnique();

            builder.HasOne(model => model.Credential)
                .WithMany()
                .HasForeignKey("credential_id")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasOne(model => model.Scope)
                .WithMany()
                .HasForeignKey("scope_id")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(model => model.User)
                .WithMany()
                .HasForeignKey("user_id")
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(model => model.TokenExpirationDateUtc)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("token_expiration_date_utc");

            builder.Property(model => model.RefreshTokenExpirationDateUtc)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("refresh_token_expiration_date_utc");

            builder.Property(model => model.CreatedDateUtc)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("created_date_utc");
        }
    }

    private class GroupEntityTypeConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable("groups")
                .HasKey(model => model.Id);

            builder.Property(model => model.Id)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("id");

            builder.Property(model => model.Name)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("name");

            builder.HasIndex(model => model.Name).IsUnique();

            builder.Property(model => model.Description)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("description")
                .IsRequired(false);

            builder.HasOne(model => model.Parent)
                .WithMany()
                .HasForeignKey("parent_id")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(model => model.Children)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "groups_children",
                    x => x.HasOne<Group>().WithMany().HasForeignKey("group_id"),
                    x => x.HasOne<Group>().WithMany().HasForeignKey("child_id"));

            builder.HasMany(model => model.Roles)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "groups_roles",
                    x => x.HasOne<Role>().WithMany().HasForeignKey("role_id"),
                    x => x.HasOne<Group>().WithMany().HasForeignKey("group_id"));

            builder.Property(e => e.Status)
                .HasConversion<int>()
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("status");

            builder.Property(model => model.StatusChangeDateUtc)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("status_change_date_utc");
        }
    }


    private class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users")
                .HasKey(model => model.Id);

            builder.Property(model => model.Id)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("id");

            builder.HasIndex(model => model.Usernames);

            builder.HasMany(model => model.Groups)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "users_groups",
                    x => x.HasOne<Group>().WithMany().HasForeignKey("group_id"),
                    x => x.HasOne<User>().WithMany().HasForeignKey("user_id"));

            builder.HasMany(model => model.Roles)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "users_roles",
                    x => x.HasOne<Role>().WithMany().HasForeignKey("role_id"),
                    x => x.HasOne<User>().WithMany().HasForeignKey("user_id"));

            builder.Property(e => e.Status)
                .HasConversion<int>()
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("status");

            builder.Property(model => model.StatusChangeReason)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("status_change_reason")
                .IsRequired(false);

            builder.Property(model => model.StatusChangeDateUtc)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("status_change_date_utc");
        }
    }


    private class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles")
                .HasKey(model => model.Id);

            builder.Property(model => model.Id)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("id");

            builder.Property(model => model.Name)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("name");

            builder.HasIndex(model => model.Name).IsUnique();

            builder.HasMany(model => model.Permissions)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "roles_permission",
                    x => x.HasOne<Permission>().WithMany().HasForeignKey("permission_id"),
                    x => x.HasOne<Role>().WithMany().HasForeignKey("role_id"));

            builder.Property(e => e.Meta)
                .HasConversion(password => JsonSerializer.Serialize(password, JsonSerializerOptions),
                    s => JsonSerializer.Deserialize<Dictionary<string, string>>(s, JsonSerializerOptions)!)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("meta")
                .IsRequired(false);
        }
    }


    private class PermissionEntityTypeConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("permissions")
                .HasKey(model => model.Id);

            builder.Property(model => model.Id)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("id");

            builder.Property(model => model.Name)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("name");

            builder.Property(e => e.Meta)
                .HasConversion(password => JsonSerializer.Serialize(password, JsonSerializerOptions),
                    s => JsonSerializer.Deserialize<Dictionary<string, string>>(s, JsonSerializerOptions)!)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("meta")
                .IsRequired(false);
        }
    }

    private class ScopeEntityTypeConfiguration : IEntityTypeConfiguration<Scope>
    {
        public void Configure(EntityTypeBuilder<Scope> builder)
        {
            builder.ToTable("scopes")
                .HasKey(model => model.Id);

            builder.Property(model => model.Id)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("id");

            builder.Property(model => model.Name)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("name");

            builder.Property(model => model.Secret)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("secret");

            builder.HasIndex(model => model.Secret).IsUnique();

            builder.Property(model => model.SessionTokenExpireTimeInMinutes)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("session_token_expire_time_in_minutes");

            builder.Property(model => model.SessionRefreshTokenExpireTimeInMinutes)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("session_refresh_token_expire_time_in_minutes");

            builder.Property(model => model.Restricted)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("restricted");

            builder.HasMany(model => model.Services)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "scopes_services",
                    x => x.HasOne<Service>().WithMany().HasForeignKey("service_id"),
                    x => x.HasOne<Scope>().WithMany().HasForeignKey("scope_id"));

            builder.HasMany(model => model.Roles)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "scopes_roles",
                    x => x.HasOne<Role>().WithMany().HasForeignKey("role_id"),
                    x => x.HasOne<Scope>().WithMany().HasForeignKey("scope_id"));

            builder.Property(e => e.Status)
                .HasConversion<int>()
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("status");
        }
    }

    private class ServiceEntityTypeConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.ToTable("services")
                .HasKey(model => model.Id);

            builder.Property(model => model.Id)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("id");

            builder.Property(model => model.Name)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("name");

            builder.Property(model => model.Secret)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("secret");

            builder.HasIndex(model => model.Secret).IsUnique();

            builder.Property(e => e.Status)
                .HasConversion<int>()
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("status");

            builder.HasMany(model => model.Permissions)
                .WithOne(permission => permission.Service)
                .HasForeignKey("service_id")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    private class AuthenticationActivityEntityTypeConfiguration : IEntityTypeConfiguration<AuthenticationActivity>
    {
        public void Configure(EntityTypeBuilder<AuthenticationActivity> builder)
        {
            builder.ToTable("authentication_activities")
                .HasKey(model => model.Id);

            builder.Property(model => model.Id)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("id");

            builder.Property(model => model.Credential)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("credential_id");

            builder.HasIndex(activity => activity.Credential);

            builder.Property(model => model.UserId)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("user_id");

            builder.HasIndex(activity => activity.UserId);

            builder.Property(model => model.ScopeId)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("scope_id");

            builder.Property(model => model.Username)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("username");

            builder.Property(model => model.IpAddress)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("ip_address")
                .IsRequired(false);

            builder.Property(model => model.Agent)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("agent")
                .IsRequired(false);

            builder.Property(model => model.CreatedDateUtc)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("created_date_utc");

            builder.HasIndex(activity => activity.CreatedDateUtc);
        }
    }

    private class AuthorizationActivityEntityTypeConfiguration : IEntityTypeConfiguration<AuthorizationActivity>
    {
        public void Configure(EntityTypeBuilder<AuthorizationActivity> builder)
        {
            builder.ToTable("authorization_activities")
                .HasKey(model => model.Id);

            builder.Property(model => model.Id)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("id");

            builder.Property(model => model.Session)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("session_id");

            builder.HasIndex(activity => activity.Session);

            builder.Property(model => model.UserId)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("user_id");

            builder.HasIndex(activity => activity.Session);

            builder.Property(model => model.IpAddress)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("ip_address")
                .IsRequired(false);

            builder.Property(model => model.Agent)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("agent")
                .IsRequired(false);

            builder.Property(model => model.CreatedDateUtc)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("created_date_utc");

            builder.HasIndex(activity => activity.CreatedDateUtc);
        }
    }
}