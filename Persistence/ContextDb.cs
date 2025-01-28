using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class ContextDb : DbContext
{
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<ConfirmCode> ConfirmCodes => Set<ConfirmCode>();
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Video> Videos => Set<Video>();
    public DbSet<Nft> Nfts => Set<Nft>();
    public DbSet<PublishOptions> PublishOptionsEnumerable => Set<PublishOptions>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Like> Likes => Set<Like>();
    public DbSet<Subscriber> Subscribers => Set<Subscriber>();

    public ContextDb(DbContextOptions<ContextDb> opt) : base(opt) 
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Role>().HasData(new Role
        {
            Id = 1,
            Name = UserRoles.User,
            NormalizedName = UserRoles.User.ToUpper(),
        }, new Role
        {
            Id = 2,
            Name = UserRoles.Admin,
            NormalizedName = UserRoles.Admin.ToUpper()
        });

        builder.Entity<Comment>()
            .HasOne(x => x.Nft)
            .WithMany(x => x.Comments)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Comment>()
            .HasOne(x => x.Reply)
            .WithMany(x => x.Replies)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Like>()
            .HasOne(x => x.Nft)
            .WithMany(x => x.Likes)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Like>()
            .HasOne(x => x.Comment)
            .WithMany(x => x.Likes)
            .OnDelete(DeleteBehavior.Cascade);
        
        base.OnModelCreating(builder);
    }
}
