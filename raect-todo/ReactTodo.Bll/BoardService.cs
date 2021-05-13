using Microsoft.EntityFrameworkCore;
using ReactTodo.Bll.Models;
using ReactTodo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactTodo.Bll
{
    /// <summary>
    /// Interface for BoardService
    /// </summary>
    public interface IBoardService
    {
        /// <summary>
        /// Gets all the boards from the database
        /// </summary>
        /// <returns>The boards Dtos</returns>
        Task<IReadOnlyCollection<BoardDto>> GetBoardsAsync();

        /// <summary>
        /// Gets a board by its id
        /// </summary>
        /// <param name="id">The id of the board to be returned</param>
        /// <returns>The Board with the given id, or null if not found</returns>
        Task<BoardDto> GetBoardAsync(long id);
    }

    ///<inheritdoc/>
    public sealed record BoardService(TodoContext DbContext) : IBoardService
    {
        ///<inheritdoc/>
        public async Task<IReadOnlyCollection<BoardDto>> GetBoardsAsync()
        {
            var boards = await DbContext.Boards.Select(t => new BoardDto(t.Id, t.Name, ItemsToDTO(t.TodoItems))).ToListAsync();
            return boards;
        }

        /// <summary>
        /// Converts a collection of todoItems to Dtos
        /// </summary>
        /// <param name="todoItems">The todoItems to be converted</param>
        /// <returns>The Dtos of the TodoItems</returns>
        private static ICollection<TodoItemDto> ItemsToDTO(ICollection<TodoItem> todoItems) => todoItems.Select(t =>
           new TodoItemDto
           (
               t.Id,
               t.Title,
               t.Description,
               t.DeadLine,
               t.Priority,
               t.BoardId
           )).OrderBy(t => t.Priority).ToList();


        ///<inheritdoc/>
        public async Task<BoardDto> GetBoardAsync(long id)
        {
            var boards = await GetBoardsAsync();
            return boards.Where(b => b.Id == id).First();

        }
    }
}
