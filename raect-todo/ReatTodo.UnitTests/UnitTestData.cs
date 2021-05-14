using Microsoft.EntityFrameworkCore;
using ReactTodo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReatTodo.UnitTests
{
    public class UnitTestData : IDisposable
    {
        public TodoContext dbContext { get; private set; }
        


        public UnitTestData()
        {
            DbContextOptions < TodoContext > todoDbContextOptions = new DbContextOptionsBuilder<TodoContext>()
               .UseInMemoryDatabase(databaseName: "TodoDb")
           .Options;
            dbContext = new TodoContext(todoDbContextOptions);
            dbContext.AddRange(testTodos);
            dbContext.SaveChanges();

        }
        public void Dispose()
        {
            
            dbContext.Dispose();
        }


        private static readonly TodoItem[] testTodos = new[]
        {
                   new TodoItem { Id = 1, BoardId = 1, Title = "Todo #1", Description = "My fist todo", DeadLine = DateTime.Today, Priority = 0 },
                   new TodoItem { Id = 2, BoardId = 1, Title = "Todo #2", Description = "My second todo", DeadLine = DateTime.Today, Priority = 1 },
                   new TodoItem { Id = 3, BoardId = 1, Title = "Todo #3", Description = "My third todo", DeadLine = DateTime.Today, Priority = 2 },
                   new TodoItem { Id = 4, BoardId = 1, Title = "Todo #4", Description = "My fourth todo", DeadLine = DateTime.Today, Priority = 3 }
        };

    }
}
