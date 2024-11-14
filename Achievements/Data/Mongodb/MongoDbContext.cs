using Achievements.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Achievements.Data.Mongodb
{
    public class MongoDbContext : DbContext
    {
        public MongoDbContext(DbContextOptions<MongoDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserChatStats>().ToCollection("user_chat_stats");

            modelBuilder.Entity<Models.Achievement>().ToCollection("chat_achievements");
        }


        public DbSet<UserChatStats> UserChatStats { get; set; }

        public DbSet<Models.Achievement> Achievements { get; set; }
    }
}