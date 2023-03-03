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
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.PostgreSQL;

public sealed class AppDbContext : DbContext
{
    private readonly ICapPublisher _capPublisher;
    
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
        builder.ApplyConfiguration(new AppSettingEntityTypeConfiguration());
        base.OnModelCreating(builder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    private class AppSettingEntityTypeConfiguration : IEntityTypeConfiguration<Credential>
    {
        public void Configure(EntityTypeBuilder<Credential> builder)
        {
            builder.ToTable("credentials")
                .HasKey(credential => credential.Id);

            builder.Property(credential => credential.Id)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("id");

            // builder.Property(credential => credential.MinVersion)
            //     .UsePropertyAccessMode(PropertyAccessMode.Property)
            //     .HasColumnName("min_version");
            //
            // builder.Property(appSetting => appSetting.LastVersion)
            //     .UsePropertyAccessMode(PropertyAccessMode.Property)
            //     .HasColumnName("last_version");
            //
            // builder.Property(appSetting => appSetting.Message)
            //     .UsePropertyAccessMode(PropertyAccessMode.Property)
            //     .HasColumnName("message");
            //
            // builder.Property(appSetting => appSetting.DownloadLink)
            //     .UsePropertyAccessMode(PropertyAccessMode.Property)
            //     .HasColumnName("link");
            //
            // builder.Property(appSetting => appSetting.CreatedOnUtc)
            //     .UsePropertyAccessMode(PropertyAccessMode.Property)
            //     .HasColumnName("created_on_utc");
        }
    }
}