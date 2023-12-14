using Eaf.Configuration;
using Eaf.Middleware.Authorization.Roles;
using Eaf.Middleware.Authorization.Users;
using Eaf.Middleware.Chat;
using Eaf.Middleware.EntityFrameworkCore;
using Eaf.Middleware.Friendships;
using Eaf.Middleware.MultiTenancy;
using Eaf.Middleware.Storage;
using Eaf.Str.Airplanes;
using Eaf.Str.Airports;
using Eaf.Str.AWBs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Eaf.Str.EntityFrameworkCore
{
    public class StrDbContext : EafMiddlewareDbContext<Tenant, Role, User, StrDbContext>
    {
        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<Airplane> Airplanes { get; set; }
        public virtual DbSet<Airport> Airports { get; set; }
        public virtual DbSet<AwbAddress> AwbAddress { get; set; }
        public virtual DbSet<AwbItem> AwbItens { get; set; }
        public virtual DbSet<Awb> Awb { get; set; }
        public virtual DbSet<Tracking> Trackings { get; set; }

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public StrDbContext(DbContextOptions<StrDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!IsOracle)
                optionsBuilder.ConfigureWarnings(w => w.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.Name });
                b.HasIndex(e => new { e.CreationTime });
            });

            if (IsOracle)
            {
                //For Oracle Bytes -> BLOB not RAW(16)
                modelBuilder.Entity<Setting>().Property(o => o.Value).HasColumnType("CLOB");
                modelBuilder.Entity<BinaryObject>().Property(o => o.Bytes).HasColumnType("BLOB");
                modelBuilder.Entity<BinaryObject>().Property(o => o.Bytes).HasMaxLength(24000000); //24mb
            }
            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });
        }
    }
}