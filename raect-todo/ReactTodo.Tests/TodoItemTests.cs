using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactTodo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ReactTodo.Tests
{
    [TestClass]
    public class TodoItemTests
    {
        private static readonly TodoItem[] testTodos = new[]
        {
                   new TodoItem { Id = 1, BoardId = 1, Title = "Todo #1", Description = "My fist todo", DeadLine = DateTime.Today, Priority = 1 },
                   new TodoItem { Id = 2, BoardId = 1, Title = "Todo #2", Description = "My second todo", DeadLine = DateTime.Today, Priority = 2 },
                   new TodoItem { Id = 3, BoardId = 1, Title = "Todo #3", Description = "My third todo", DeadLine = DateTime.Today, Priority = 3 },
                   new TodoItem { Id = 4, BoardId = 1, Title = "Todo #4", Description = "My fourth todo", DeadLine = DateTime.Today, Priority = 4 }
        };

        [TestMethod]
        public async Task GetAllTodosTest()
        {
            using (var testScope = TestWebFactory.Create())
            {
                testScope.AddSeedEntities(testTodos);
                var client = testScope.CreateClient();
                var response = await client.GetAsync("api/todoitems");

                response.EnsureSuccessStatusCode();
                var actual = await response.Content.ReadFromJsonAsync<TodoItem[]>();
                Assert.IsNotNull(actual);
                CollectionAssert.AreEquivalent(testTodos, actual);

            }
        }

        [TestMethod]
        public async Task GetTodoByIdTest()
        {
            using (var testScope = TestWebFactory.Create())
            {

                testScope.AddSeedEntities(testTodos);
                var client = testScope.CreateClient();
                foreach (var expected in testScope.GetDbTableContent<TodoItem>())
                {
                    var response = await client.GetAsync($"api/todoitems/{expected.Id}");
                    response.EnsureSuccessStatusCode();
                    var actual = await response.Content.ReadFromJsonAsync<TodoItem>();
                    Assert.IsNotNull(actual);
                    Assert.AreEqual(expected, actual);

                }



            }
        }

        [TestMethod]
        public async Task PostNewWithSuccessTest()
        {

            using (var testScope = TestWebFactory.Create())
            {
                testScope.AddSeedEntities(testTodos);
                var toInsert = new TodoItem { Id = 4, Title = "Todo4", Description = "The fourth todo", DeadLine = new DateTime(2021, 05, 26), Priority = 2, BoardId = 1 };

                var client = testScope.CreateClient();
                var response = await client.PostAsJsonAsync("api/todoitems", toInsert);

                response.EnsureSuccessStatusCode();
                var postResponse = await response.Content.ReadFromJsonAsync<TodoItem>();

                Assert.IsNotNull(postResponse);
                var insertedRecord = testScope.GetDbTableContent<TodoItem>().SingleOrDefault(x => x.Id == postResponse.Id);
                Assert.AreEqual(insertedRecord, postResponse);
            }
        }

        [TestMethod]
        public async Task DeleteTodoByIdTest()
        {
            using (var testScope = TestWebFactory.Create())
            {
                testScope.AddSeedEntities(testTodos);
                var client = testScope.CreateClient();

                var response = await client.DeleteAsync($"api/todoitems/{3}");

                response.EnsureSuccessStatusCode();
            }
        }

        [TestMethod]
        public async Task PutWithSuccessTest()
        {
            using (var testScope = TestWebFactory.Create())
            {
                testScope.AddSeedEntities(testTodos);

                var client = testScope.CreateClient();
                var recordToUpdate = testScope.GetDbTableContent<TodoItem>().Last();
                recordToUpdate.Title = "A new title";


                var response = await client.PutAsJsonAsync($"api/todoitems/{recordToUpdate.Id}", recordToUpdate);

                response.EnsureSuccessStatusCode();
                var putResponse = await response.Content.ReadFromJsonAsync<TodoItem>();

                Assert.IsNotNull(putResponse);
                Assert.AreEqual(recordToUpdate.Id, putResponse.Id);
                Assert.AreEqual(recordToUpdate.Title, putResponse.Title);

                var updatedRecord = testScope.GetDbTableContent<TodoItem>().SingleOrDefault(x => x.Id == recordToUpdate.Id);
                Assert.IsNotNull(updatedRecord);
                Assert.AreEqual(recordToUpdate.Id, updatedRecord.Id);
                Assert.AreEqual(recordToUpdate.Title, updatedRecord.Title);
            }
        }

    }
}
