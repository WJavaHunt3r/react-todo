using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactTodo.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<Board> Boards { get; set; }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                throw new System.Exception("DbContext not configured");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Board>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(20);
                
                entity.HasData(
                    new Board { Id = 1, Name = "Todo" },
                    new Board { Id = 2, Name = "Active" },
                    new Board { Id = 3, Name = "Blocked" },
                    new Board { Id = 4, Name = "Completed" });
                entity.HasMany<TodoItem>(b => b.TodoItems)
                            .WithOne(i => i.Board)
                            .HasForeignKey(i => i.BoardId)
                            .HasPrincipalKey(b => b.Id);
            });
       

            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.HasKey(ti => ti.Id);
                entity.Property(ti => ti.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Title).HasMaxLength(50);
                


            });
        

            base.OnModelCreating(modelBuilder);
        }

    }
}
