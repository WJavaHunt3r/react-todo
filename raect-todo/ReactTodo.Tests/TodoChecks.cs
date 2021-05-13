using Microsoft.EntityFrameworkCore;

using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NUnit.Framework;
using ReactTodo.Api.Controllers;
using ReactTodo.Bll;
using ReactTodo.Bll.Models;
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
    public class TodoChecks
    {
        private static readonly TodoItem[] testTodos = new[]
        {
             new TodoItem { Id = 1, Title="Todo1", Description="The first todo", DeadLine=new DateTime(2021, 05, 24), Priority=0, BoardId= 1 },
             new TodoItem { Id = 2, Title="Todo2", Description="The first todo", DeadLine=new DateTime(2021, 05, 27), Priority=1, BoardId= 1 },
             new TodoItem { Id = 3, Title="Todo3", Description="The first todo", DeadLine=new DateTime(2021, 05, 26), Priority=2, BoardId= 3 }
        };

        private static readonly Board[] boards =  new[]
        {
                new Board { Id = 1, Name = "TODO"  },
                new Board { Id = 2, Name = "ACTIVE" },
                new Board { Id = 3, Name = "BLOCKED" },
                new Board { Id = 4, Name = "COMPLETED" }
        };
        
       [TestMethod]
        public async Task GetAllTodosTest()
        {
            using (var testScope = TestWebFactory.Create())
            {
                testScope.AddSeedEntities(boards);
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
                testScope.AddSeedEntities(boards);
                testScope.AddSeedEntities(testTodos);


                var client = testScope.CreateClient();
                foreach(var expected in testScope.GetDbTableContent<TodoItem>())
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
                testScope.AddSeedEntities(boards);
                var toInsert = new TodoItem {Id=4, Title = "Todo4", Description = "The fourth todo", DeadLine = new DateTime(2021, 05, 26), Priority = 2, BoardId = 1 };

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
                testScope.AddSeedEntities(boards);
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
                testScope.AddSeedEntities(boards);
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
