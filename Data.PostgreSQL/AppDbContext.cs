using System.Text.Json;
using Core.Domains;
using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Sessions;
using Core.Domains.Groups;
using Core.Domains.Permissions;
using Core.Domains.Roles;
using Core.Domains.Scopes;
using Core.Domains.Services;
using Core.Domains.Users;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data;

public sealed class AppDbContext : DbContext
{
    private readonly ICapPublisher _capPublisher;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new();
    
    public AppDbContext(DbContextOptions<AppDbContext> options, ICapPublisher capPublisher) : base(options)
    {
        _capPublisher = capPublisher;
    }

    public DbSet<Credential> Credentials { get; set; }
    
    public DbSet<CredentialActivity> CredentialActivities { get; set; }

    public DbSet<Session> Sessions { get; set; }

    public DbSet<SessionActivity> SessionActivities { get; set; }
    
    public DbSet<Group> Groups { get; set; }

    public DbSet<Permission> Permissions { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Scope> Scopes { get; set; }
    
    public DbSet<Service> Services { get; set; }
    
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CredentialEntityTypeConfiguration());
        builder.ApplyConfiguration(new CredentialActivityEntityTypeConfiguration());
        builder.ApplyConfiguration(new SessionEntityTypeConfiguration());
        builder.ApplyConfiguration(new SessionActivityEntityTypeConfiguration());
        builder.ApplyConfiguration(new UserEntityTypeConfiguration());
        builder.ApplyConfiguration(new GroupEntityTypeConfiguration());
        builder.ApplyConfiguration(new RoleEntityTypeConfiguration());
        builder.ApplyConfiguration(new PermissionEntityTypeConfiguration());
        builder.ApplyConfiguration(new ScopeEntityTypeConfiguration());
        builder.ApplyConfiguration(new ServiceEntityTypeConfiguration());
        base.OnModelCreating(builder);
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
            builder.HasIndex(model => model.Username);

            builder.HasOne(model => model.User);

            builder.Property(b => b.Password)
                .HasColumnType("jsonb");

            builder.Ignore(c => c.FirstActivity);
            
            builder.Ignore(c => c.LastActivity);
            //
            // builder.HasOne(c => c.FirstActivity)
            //     .WithOne(c => c.Credential)
            //     .OnDelete(DeleteBehavior.Cascade);
            //     
            // builder.HasOne(c => c.LastActivity)
            //     .WithOne(c => c.Credential)
            //     .OnDelete(DeleteBehavior.Cascade);
                
            builder.Property(model => model.ExpiresAtUtc)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("expires_at_utc");
            
            builder.Property(model => model.SingleSession)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("single_session");
            
            builder.Property(model => model.SessionExpireTimeInMinutes)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("session_expire_time_in_minutes");
            
            builder.Property(model => model.OneTime)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("one_time");
            
            builder.Property(model => model.CreatedDateUtc)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("expires_at_utc");
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
            builder.HasIndex(model => model.RefreshToken);

            builder.HasOne(model => model.Credential);

            builder.HasOne(model => model.Scope);
            
            builder.HasOne(model => model.User);
            
            builder.HasOne(model => model.Activity)
                .WithOne(model  => model.Session);
            
            builder.Property(model => model.ExpirationDateUtc)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("expiration_date_utc");
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
            builder.HasIndex(model => model.Name);
            
            builder.Property(model => model.Description)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("description");

            builder.HasOne(model => model.Parent);
            
            builder.HasMany(model => model.Children);
            
            builder.HasMany(model => model.Roles);

            builder
                .Property(e => e.Status)
                .HasConversion(
                    v => v.Name,
                    v =>  Enumeration.GetAll<GroupStatus>().First(status => status.Name == v));
            
            builder.Property(model => model.Status)
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
            
            builder.HasMany(model => model.Groups);
            
            builder.HasMany(model => model.Roles);

            builder
                .Property(e => e.Status)
                .HasConversion(
                    v => v.Name,
                    v =>  Enumeration.GetAll<UserStatus>().First(status => status.Name == v));
            
            builder.Property(model => model.Status)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("status");
            
            builder.Property(model => model.StatusChangeReason)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("status_change_reason");
            
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
            
            builder.HasMany(model => model.Permissions);

            builder
                .Property(e => e.Meta)
                .HasConversion(
                    v => JsonSerializer.Serialize(v,JsonSerializerOptions),
                    v =>  JsonSerializer.Deserialize<Dictionary<string,string>>(v,JsonSerializerOptions)!);
            
            builder.Property(model => model.Meta)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("meta");
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
            
            builder
                .Property(e => e.Meta)
                .HasConversion(
                    v => JsonSerializer.Serialize(v,JsonSerializerOptions),
                    v =>  JsonSerializer.Deserialize<Dictionary<string,string>>(v, JsonSerializerOptions)!);
            
            builder.Property(model => model.Meta)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("meta");
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
            
            builder.HasMany(model => model.Services);
            builder.HasMany(model => model.Roles);
            
            builder
                .Property(e => e.Status)
                .HasConversion(
                    v => v.Name,
                    v =>  Enumeration.GetAll<ScopeStatus>().First(status => status.Name == v));
            
            builder.Property(model => model.Status)
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
            
            builder
                .Property(e => e.Status)
                .HasConversion(
                    v => v.Name,
                    v =>  Enumeration.GetAll<ServiceStatus>().First(status => status.Name == v));
            
            builder.Property(model => model.Status)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("status");
        }
    }
    
    private class CredentialActivityEntityTypeConfiguration : IEntityTypeConfiguration<CredentialActivity>
    {
        public void Configure(EntityTypeBuilder<CredentialActivity> builder)
        {
            builder.ToTable("credential_activities")
                .HasKey(model => model.Id);

            builder.Property(model => model.Id)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("id");
            
            builder.Property(model => model.IpAddress)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("ip_address");
            
            builder.Property(model => model.Agent)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("agent");

            builder.HasIndex(activity => activity.Agent);
            
            builder.Property(model => model.CreatedDateUtc)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("created_date_utc");
            
        }
    }
    
    private class SessionActivityEntityTypeConfiguration : IEntityTypeConfiguration<SessionActivity>
    {
        public void Configure(EntityTypeBuilder<SessionActivity> builder)
        {
            builder.ToTable("sessions_activities")
                .HasKey(model => model.Id);

            builder.Property(model => model.Id)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("id");
            
            builder.Property(model => model.IpAddress)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("ip_address");
            
            builder.Property(model => model.Agent)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("agent");

            builder.HasIndex(activity => activity.Agent);
            
            builder.Property(model => model.CreatedDateUtc)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("created_date_utc");
            
        }
    }
    
}