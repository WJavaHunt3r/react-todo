using Microsoft.EntityFrameworkCore;

using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NUnit.Framework;
using ReactTodo.Api.Controllers;
using ReactTodo.Bll;
using ReactTodo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactTodo.Tests
{
    [TestClass]
    class TodoChecks
    {
        private DbContextOptions<TodoContext> dbContextOptions = new DbContextOptionsBuilder<TodoContext>().UseInMemoryDatabase(databaseName: "PrimeDb")
        .Options;
        private TodoItemsController controller;

        //[OneTimeSetUp]
        public void Setup()
        {
            SeedDb();

            controller = new TodoItemsController(new TodoItemService(new TodoContext(dbContextOptions)));
        }
        private void SeedDb()
        {
            using var context = new TodoContext(dbContextOptions);
            var boards = new List<Board>
            {
                new Board { Id = 1, Name = "TODO"  },
                new Board { Id = 2, Name = "ACTIVE" },
                new Board { Id = 3, Name = "BLOCKED" },
                new Board { Id = 4, Name = "COMPLETED" }
            };

            var todos = new List<TodoItem>
            {
                new TodoItem { Id = 1, Title="Todo1", Description="The first todo", DeadLine=new DateTime(2021, 05, 24), Priority=0 },
                new TodoItem { Id = 2, Title="Todo2", Description="The first todo", DeadLine=new DateTime(2021, 05, 27), Priority=1 },
                new TodoItem { Id = 3, Title="Todo3", Description="The first todo", DeadLine=new DateTime(2021, 05, 26), Priority=0 }
            };
            boards[0].TodoItems.Add(todos[0]);
            boards[0].TodoItems.Add(todos[1]);
            boards[3].TodoItems.Add(todos[2]);
            context.AddRange(boards);
            context.AddRange(todos);

           

            context.SaveChanges();
        }
        [TestMethod]
        public async Task TestMethod1()
        {
            using var context = new TodoContext(dbContextOptions);
            var response = (await controller.GetTodoItemsAsync()).ToList();
            Assert.Equals(response.Count, context.Boards.Select(b => b.TodoItems).Count());
        }

    }
}
