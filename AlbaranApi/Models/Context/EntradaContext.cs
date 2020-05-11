using AlbaranApi.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AlbaranApi.Models.Context
{
    public sealed class EntradaContext : DbContext, IEntradaContext
    {
        public EntradaContext(DbContextOptions<EntradaContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Entrada> Entradas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entrada>(u => u.HasKey(k => k.EntradaId))
                .Entity<Entrada>(u => u.Property(p => p.CreationDate).IsRequired())
                .Entity<Entrada>(u => u.Property(p => p.ProductAmount).IsRequired())
                .Entity<Entrada>(u => u.Property(p => p.ProductIdentity).IsRequired())
                .Entity<Entrada>(u => u.Property(p => p.QrCodeData).IsRequired())
                .Entity<Entrada>(u => u.Property(p => p.ProviderId).IsRequired());
        }
    }
}