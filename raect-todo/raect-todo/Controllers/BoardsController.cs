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
        private IBoardService BoardService { get; }
        public BoardsController(IBoardService boardService)
        {
            BoardService = boardService;
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<BoardDto>> GetBoardsAsync() => await BoardService.GetBoardsAsync();

        [HttpGet("{id}")]
        public async Task<BoardDto> GetBoardAsync(long id) => await BoardService.GetBoardAsync(id);
    }

}
