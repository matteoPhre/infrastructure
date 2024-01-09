﻿using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Outbox.Stores.EfCore
{
    public class EfCoreOutboxContext : DbContext
    {
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<OutboxMessage>(b =>
            {
                b.Property(p => p.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                b.Property(p => p.CreatedUtc)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                b.Property(p => p.AssemblyType)
                    .IsRequired();

                b.Property(p => p.Data)
                    .IsRequired();

                b.HasKey(k => k.Id);
            });
        }
    }
}
