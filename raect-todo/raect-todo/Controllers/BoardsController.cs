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
    public class BoardsController : ControllerBase
    {
        private ITodoItemService TodoService { get; }
        public BoardsController(ITodoItemService todoService)
        {
            TodoService = todoService;
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<BoardDto>> GetBoardsAsync() => await TodoService.GetBoardsAsync();

        [HttpGet("{id}")]
        public async Task<BoardDto> GetBoardAsync(long id) => await TodoService.GetBoardAsync(id);
        
        [HttpPut("{id}")]
        public async Task<BoardDto> UpdateBoardAsync(long id, BoardDto boardDto) => await TodoService.UpdateBoardAsync(id, boardDto);
    }

}
