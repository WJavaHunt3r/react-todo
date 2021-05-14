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
    /// Controller for Board services
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private IBoardService BoardService { get; }

        /// <summary>
        /// Constructor for Controller
        /// </summary>
        /// <param name="boardService"></param>
        public BoardsController(IBoardService boardService)
        {
            BoardService = boardService;
        }

        /// <summary>
        /// Get all boards
        /// </summary>
        /// <returns>The boards in the database</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<BoardDto>>> GetBoardsAsync()
        {
            var boards = await BoardService.GetBoardsAsync();
            return Ok(boards);
        }

        /// <summary>
        /// Get a board by its id
        /// </summary>
        /// <param name="id">The id of the bopard to be returned</param>
        /// <returns>null if not found, else the Dto of the Board</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BoardDto>> GetBoardAsync(long id)
        {
            var board = await BoardService.GetBoardAsync(id);
            return board == null ? NotFound() : Ok(board);
        }
    }

}
