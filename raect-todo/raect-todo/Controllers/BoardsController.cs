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
    /// Controller for Board
    /// Calls methods from TodoService
    /// </summary>
    [Route("boards")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private ITodoItemService TodoService { get; }
        /// <summary>
        /// Constructor of class
        /// </summary>
        /// <param name="todoService">ITodoItemService object.</param>
        public BoardsController(ITodoItemService todoService)
        {
            TodoService = todoService;
        }

        /// <summary>
        /// Get method
        /// </summary>
        /// <returns>OK Http response</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<BoardDto>>> GetBoardsAsync() => Ok(await TodoService.GetBoardsAsync());

        /// <summary>
        /// Get by id method
        /// </summary>
        /// <param name="id"> A long number</param>
        /// <returns>NotFound if board not Found, else the BoardDto</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BoardDto>> GetBoardAsync(long id)
        {
            var board= await TodoService.GetBoardAsync(id);
            return board == null ? NotFound() : board;
        }
    }

}
