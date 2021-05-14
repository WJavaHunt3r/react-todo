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
        public async Task<ActionResult<IReadOnlyCollection<TodoItemDto>>> GetTodoItemsAsync()
        {
            var todo = await TodoService.GetTodoItemsAsync();
            return Ok(todo);
        }

        /// <summary>
        /// Add TodoItem to the database
        /// </summary>
        /// <param name="todoItemDto">The todoItem to be added</param>
        /// <returns>Null if failed, The todoItem otherwise</returns>
        [HttpPost]
        public async Task<ActionResult<TodoItemDto>> PostTodoItemAsync(TodoItemDto todoItemDto)
        {
           if (todoItemDto is null )
                return BadRequest();
            var newTodo = await TodoService.PostTodoItemAsync(todoItemDto);
            return CreatedAtAction(nameof(GetTodoItem), new { id = newTodo.Id }, newTodo);
        }

        /// <summary>
        /// Get a todoitem by its id
        /// </summary>
        /// <param name="id">The id of the TodoItem to be returned</param>
        /// <returns>null if not found, the todoItem otherwise</returns>
        [HttpGet("{id:long}")]
        public async Task<ActionResult<TodoItemDto>> GetTodoItem(long id)
        {
            if (id == null)
            {
                return BadRequest();
            }
           var todo = await TodoService.GetTodoItemAsync(id);
           return todo == null ? NotFound() : Ok(todo);
        }

        /// <summary>
        /// Delete a todoItem ffrom the database
        /// </summary>
        /// <param name="id">Th id of the TodoItem to be deleted</param>
        /// <returns>0 if failed, the id of the removed TodoItem otherwise</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodoItemAsync(long id)
        {
            var response = await TodoService.DeleteTodoItemAsync(id);
            return response ? NoContent() : NotFound();
        }

        /// <summary>
        /// Put, update a todoItem
        /// </summary>
        /// <param name="id">Thge id of the TodoItem to be updated</param>
        /// <param name="todoItemDto">The new TodoItem</param>
        /// <returns>Null if failed or not found, the new TodoItem otherwise</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<TodoItemDto>> UpdateTodoItemAsync(long id, TodoItemDto todoItemDto)
        {
            if(id == null || todoItemDto == null || id !=  todoItemDto.Id)
            {
                return BadRequest();
            }
            var todo = await TodoService.UpdateTodoItemAsync(id, todoItemDto);
            return todo == null ? NotFound() : NotFound();
        }
    }
}
