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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Board>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(20);
                entity.HasData(
                    new Board { Id = 1, Name = "Todo" },
                    new Board { Id = 2, Name = "Active" },
                    new Board { Id = 3, Name = "Blocked" },
                    new Board { Id = 4, Name = "Completed" });
            });

            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(e => e.Board)
                    .WithMany(t => t.TodoItems)
                    .HasForeignKey(e => e.BoardId);

                entity.HasData(
                    new TodoItem { Id=1, BoardId = 1, Title = "Todo #1", Description= "My fist todo", DeadLine = new DateTime(2021, 04, 24), Priority = 1},
                    new TodoItem { Id = 2, BoardId = 1, Title = "Todo #2", Description = "My second todo", DeadLine = new DateTime(2021, 04, 25), Priority = 2 },
                    new TodoItem { Id = 3, BoardId = 1, Title = "Todo #3", Description = "My third todo", DeadLine = new DateTime(2021, 04, 26), Priority = 3 },
                    new TodoItem { Id = 4, BoardId = 1, Title = "Todo #4", Description = "My fourth todo", DeadLine = new DateTime(2021, 04, 27), Priority = 4 });


        });
        }

    }
}
