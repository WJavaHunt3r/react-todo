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
    /// Controller for TodoItems
    /// </summary>
    [Route("todoitems")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private ITodoItemService TodoService { get; }

        /// <summary>
        /// Constructor of class
        /// </summary>
        /// <param name="todoService">ITodoItemService object.</param>
        public TodoItemsController(ITodoItemService todoService)
        {
            TodoService = todoService;
        }

        /// <summary>
        /// Method to get all TodoItems
        /// </summary>
        /// <returns>OK Http response with the todos</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<TodoItemDto>>> GetTodoItemsAsync()
        {
            var todo = await TodoService.GetTodoItemsAsync();
            return Ok(todo);
        }

        /// <summary>
        /// Adds new given TodoItem to the database
        /// </summary>
        /// <param name="todoItemDto"></param>
        /// <returns>BadRequest if response was null, else the new todoItemDto</returns>
        [HttpPost]
        public async Task<ActionResult<TodoItemDto>> PostTodoItemAsync(TodoItemDto todoItemDto)
        {
            var todo =  await TodoService.PostTodoItemAsync(todoItemDto);
            return todo == null ? BadRequest() : Ok(todoItemDto);
        }

        /// <summary>
        /// Get a Todo by the given Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>NotFound if response was null, else the todoItem</returns>
        [HttpGet("{id:long}")]
        public async Task<ActionResult<TodoItemDto>> GetTodoItemAsync(long id)
        {
            var todo= await TodoService.GetTodoItemAsync(id);
            return todo == null ? NotFound() : todo;
        }

        /// <summary>
        /// Delete a todo by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Not Found if null was returned, else OK</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodoItemAsync(long id) => await TodoService.DeleteTodoItemAsync(id) == 0 ? NotFound() : Ok();

        /// <summary>
        /// Update a Todo by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="todoItemDto"></param>
        /// <returns>BadRequest if null was returned, else OK, with the updated Dto</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<TodoItemDto>> UpdateTodoItemAsync(long id, TodoItemDto todoItemDto) => await TodoService.UpdateTodoItemAsync(id, todoItemDto) == null ? BadRequest():Ok(todoItemDto);
    }
}
