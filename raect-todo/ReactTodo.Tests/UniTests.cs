using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using ReactTodo.Bll;
using ReactTodo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactTodo.Bll.Models;
using ReatTodo.Tests;

namespace ReactTodo.Tests
{
    [TestClass]
    public class TodoItemUnitTests 
    {
        private DbContextOptions<TodoContext> todoDbContextOptions = new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase(databaseName: "TodoDb")
        .Options;
        private TodoItemService todoService;
       /* public TodoItemUnitTests()
        {
            if (todoService == null)
            {
                TodoContext dbContext = new TodoContext(todoDbContextOptions);
                todoService = new TodoItemService(dbContext);
                dbContext.AddRange(testTodos);
                dbContext.SaveChanges();

            }
        }*/

        [TestMethod]
        public void TestGetAllTodos()
        {
            using (var unitTestData = new UnitTestData())
            {
                todoService = new TodoItemService(unitTestData.dbContext);
                var actual = todoService.GetTodoItemsAsync().Result.ToList();
                CollectionAssert.AreEquivalent(unitTestData.TestTodos,convertListDtos(actual));
            }
        }

        [TestMethod]
        public void TestGetTodoById()
        {
            using (var unitTestData = new UnitTestData())
            {
                todoService = new TodoItemService(unitTestData.dbContext);
                var expected = unitTestData.TestTodos[0];
                var actual = todoService.GetTodoItemAsync(unitTestData.TestTodos[0].Id).Result;
                Assert.AreEqual(expected, convertDtos(actual));
            }
        }

        [TestMethod]
        public void TestPostTodo()
        {
            using (var unitTestData = new UnitTestData())
            {
                todoService = new TodoItemService(unitTestData.dbContext);
                var newTodo = new TodoItem { Id = 5, BoardId = 1, Title = "Todo #1", Description = "My fist todo", DeadLine = DateTime.Today, Priority = 4 };

                var actual = todoService.PostTodoItemAsync(convertToDto(newTodo)).Result;
                Assert.AreEqual(newTodo, convertDtos(actual));
            }
        }

        [TestMethod]
        public void TestDeleteTodoById()
        {
            using (var unitTestData = new UnitTestData())
            {
                todoService = new TodoItemService(unitTestData.dbContext);
                var expected = true;
                var actual = todoService.DeleteTodoItemAsync(unitTestData.TestTodos[0].Id).Result;
                Assert.AreEqual(expected, actual);
            }
        }


        private List<TodoItem> convertListDtos(ICollection<TodoItemDto> todos)
        {
            return todos.Select(t => new TodoItem{Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DeadLine = t.DeadLine,
                Priority = t.Priority,
                BoardId = t.BoardId }).ToList();
        }
        private TodoItem convertDtos(TodoItemDto t)
        {
            return  new TodoItem
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DeadLine = t.DeadLine,
                Priority = t.Priority,
                BoardId = t.BoardId
            };
        }

        private TodoItemDto convertToDto(TodoItem todoItem) =>
        
            new TodoItemDto
            (
                todoItem.Id,
                todoItem.Title,
                todoItem.Description,
                todoItem.DeadLine,
                todoItem.Priority,
                todoItem.BoardId
            );
        

        private static readonly TodoItem[] testTodos = new[]
        {
                   new TodoItem { Id = 1, BoardId = 1, Title = "Todo #1", Description = "My fist todo", DeadLine = DateTime.Today, Priority = 0 },
                   new TodoItem { Id = 2, BoardId = 1, Title = "Todo #2", Description = "My second todo", DeadLine = DateTime.Today, Priority = 1 },
                   new TodoItem { Id = 3, BoardId = 1, Title = "Todo #3", Description = "My third todo", DeadLine = DateTime.Today, Priority = 2 },
                   new TodoItem { Id = 4, BoardId = 1, Title = "Todo #4", Description = "My fourth todo", DeadLine = DateTime.Today, Priority = 3 }
        };

    }
}
