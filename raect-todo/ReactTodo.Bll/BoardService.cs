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
    public interface IBoardService
    {
        Task<IReadOnlyCollection<BoardDto>> GetBoardsAsync();
        Task<BoardDto> GetBoardAsync(long id);
    }

    public sealed record BoardService(TodoContext DbContext) : IBoardService
    {
        public async Task<IReadOnlyCollection<BoardDto>> GetBoardsAsync()
        {
            var boards = await DbContext.Boards.Select(t => new BoardDto(t.Id, t.Name, ItemsToDTO(t.TodoItems))).ToListAsync();
            return boards;
        }

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



        public async Task<BoardDto> GetBoardAsync(long id)
        {
            var boards = await GetBoardsAsync();
            return boards.Where(b => b.Id == id).First();

        }
    }
}
