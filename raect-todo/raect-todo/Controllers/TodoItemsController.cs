using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactTodo.Bll;
using ReactTodo.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactTodo.Api.Controllers
{

    /// <summary>
    /// Controller for TodoITem Services
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private ITodoItemService TodoService { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="todoService"></param>
        public TodoItemsController(ITodoItemService todoService)
        {
            TodoService = todoService;
        }

        /// <summary>
        /// Get all todo Items
        /// </summary>
        /// <returns>The todoItems in the database</returns>
        [HttpGet]
        public async Task<IReadOnlyCollection<TodoItemDto>> GetTodoItemsAsync() => await TodoService.GetTodoItemsAsync();

        /// <summary>
        /// Add TodoItem to the database
        /// </summary>
        /// <param name="todoItemDto">The todoItem to be added</param>
        /// <returns>Null if failed, The todoItem otherwise</returns>
        [HttpPost]
        public async Task<TodoItemDto> PostTodoItemAsync(TodoItemDto todoItemDto) => await TodoService.PostTodoItemAsync(todoItemDto);

        /// <summary>
        /// Get a todoitem by its id
        /// </summary>
        /// <param name="id">The id of the TodoItem to be returned</param>
        /// <returns>null if not found, the todoItem otherwise</returns>
        [HttpGet("{id}")]
        public async Task<TodoItemDto> GetTodoItemAsync(long id) => await TodoService.GetTodoItemAsync(id);

        /// <summary>
        /// Delete a todoItem ffrom the database
        /// </summary>
        /// <param name="id">Th id of the TodoItem to be deleted</param>
        /// <returns>0 if failed, the id of the removed TodoItem otherwise</returns>
        [HttpDelete("{id}")]
        public async Task<Boolean> DeleteTodoItemAsync(long id) => await TodoService.DeleteTodoItemAsync(id);

        /// <summary>
        /// Put, update a todoItem
        /// </summary>
        /// <param name="id">Thge id of the TodoItem to be updated</param>
        /// <param name="todoItemDto">The new TodoItem</param>
        /// <returns>Null if failed or not found, the new TodoItem otherwise</returns>
        [HttpPut("{id}")]
        public async Task<TodoItemDto> UpdateTodoItemAsync(long id, TodoItemDto todoItemDto) => await TodoService.UpdateTodoItemAsync(id, todoItemDto);
    }
}
