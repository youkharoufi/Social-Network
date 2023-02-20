using Facebook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Facebook.Data.SeedConfigurations;
using Microsoft.AspNetCore.Identity;

namespace Facebook.Data
{
    public class DataContext : IdentityDbContext<User>
    {

        public DataContext(DbContextOptions options) : base(options)
        {
         
        }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }





        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            //photos
            builder.Entity<Photo>()
                .HasOne(u => u.User)
                .WithMany(m => m.Photos)
                .OnDelete(DeleteBehavior.Restrict);

            //friends
            builder.Entity<FriendRequest>()
                .HasOne(u => u.TargetUser)
                .WithMany(m => m.FriendRequestsReceived)
                .OnDelete(DeleteBehavior.Cascade);

            //messages
            builder.Entity<Message>()
                .HasOne(u => u.Target)
                .WithMany(m => m.MessagesRecieved)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);


            //Seed Data :

            //builder.ApplyConfiguration(new UserSeeding());
            //builder.ApplyConfiguration(new RoleSeeding());



        }

    }
}

