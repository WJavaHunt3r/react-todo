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
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private ITodoItemService TodoService { get; }
        public TodoItemsController(ITodoItemService todoService)
        {
            TodoService = todoService;
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<TodoItemDto>> GetTodoItemsAsync() => await TodoService.GetTodoItemsAsync();

        [HttpPost]
        public async Task<TodoItemDto> PostTodoItemAsync(TodoItemDto todoItemDto) => await TodoService.PostTodoItemAsync(todoItemDto);

        [HttpGet("{id}")]
        public async Task<TodoItemDto> GetTodoItemAsync(long id) => await TodoService.GetTodoItemAsync(id);

        [HttpDelete("{id}")]
        public async Task<long> DeleteTodoItemAsync(long id) => await TodoService.DeleteTodoItemAsync(id);

        [HttpPut("{id}")]
        public async Task<TodoItemDto> UpdateTodoItemAsync(long id, TodoItemDto todoItemDto) => await TodoService.UpdateTodoItemAsync(id, todoItemDto);
    }
}
