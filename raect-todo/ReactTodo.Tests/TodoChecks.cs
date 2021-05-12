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
                new Board { Id = 1, Name = "TODO" 
                },
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

    }
}
