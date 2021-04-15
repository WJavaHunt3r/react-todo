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
    public interface ITodoItemService 
    {
        Task<IReadOnlyCollection<TodoItemDto>> GetTodoItemsAsync();
        Task<TodoItemDto> PostTodoItemAsync(TodoItemDto todoItemDto);

        Task<TodoItemDto> GetTodoItemAsync(long id);
        Task<long> DeleteTodoItemAsync(long id);
        Task<TodoItemDto> UpdateTodoItemAsync(long id, TodoItemDto todoItemDto);


        Task<IReadOnlyCollection<BoardDto>> GetBoardsAsync();
        Task<BoardDto> GetBoardAsync(long id);
        Task<BoardDto> UpdateBoardAsync(long id, BoardDto todoItemDto);
    }
    public record TodoItemService(TodoContext DbContext) : ITodoItemService
    {
        public async Task<IReadOnlyCollection<BoardDto>> GetBoardsAsync() =>
            await DbContext.Boards.Select(t => new BoardDto(t.Id, t.Name,ItemsToDTO( t.TodoItems))).ToListAsync();

        public async Task<IReadOnlyCollection<TodoItemDto>> GetTodoItemsAsync() =>
            await DbContext.TodoItems.Select(t => new TodoItemDto(t.Id,t.Title, t.Description, t.DeadLine,  t.Priority, t.BoardId)).ToListAsync();

        public async Task<TodoItemDto> PostTodoItemAsync(TodoItemDto todoItemDto)
        {
            var boards = await GetBoardsAsync();
            var todoItem = new TodoItem
            {
                Title = todoItemDto.Title,
                DeadLine = todoItemDto.DeadLine,
                Description = todoItemDto.Description,
                BoardId = 1,
                Priority = boards.Where(t=>t.Id==1).First().TodoItems.Count+1


        };


            DbContext.TodoItems.Add(todoItem);
            await DbContext.SaveChangesAsync();

            return ItemToDTO(todoItem);
        }

        public async Task<TodoItemDto> GetTodoItemAsync(long id)
        {
            var todoItem = await DbContext.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return null;
            }

            return ItemToDTO(todoItem);
        }

        public async Task<BoardDto> GetBoardAsync(long id)
        {
            var allBoards = await GetBoardsAsync();
            return allBoards.Where(t => t.Id == id).FirstOrDefault();

        }

        public async Task<long> DeleteTodoItemAsync(long id)
        {
            var todoItem = await DbContext.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return 0;
            }

            DbContext.TodoItems.Remove(todoItem);
            //UpdateBoardAfterDeleteAsync(todoItem.BoardId, todoItem.Priority);
            var todos = await DbContext.TodoItems.Where(t => t.BoardId == todoItem.BoardId && t.Priority >= todoItem.Priority).ToListAsync();
            todos.Select(async t => await UpdateTodoItemAsync(t.Id, new TodoItemDto(t.Id, t.Title, t.Description, t.DeadLine, t.Priority - 1, t.BoardId)));
            await DbContext.SaveChangesAsync();

            return 1;
        }

        private static BoardDto BoardToDTO(Board board) =>
            new BoardDto(board.Id, board.Name, ItemsToDTO( board.TodoItems));



        private static TodoItemDto ItemToDTO(TodoItem todoItem) =>
            new TodoItemDto
            (
                todoItem.Id,
                todoItem.Title,
                todoItem.Description,
                todoItem.DeadLine,             
                todoItem.Priority,
                todoItem.BoardId
            );
        private static ICollection<TodoItemDto> ItemsToDTO(ICollection<TodoItem> todoItems) => todoItems.Select(t =>
            new TodoItemDto
            (
                t.Id,
                t.Title,
                t.Description,
                t.DeadLine,
                t.Priority,
                t.BoardId
            )).ToList();

        public async Task<TodoItemDto> UpdateTodoItemAsync(long id, TodoItemDto todoItemDto)
        {
            if (id != todoItemDto.Id)
            {
                return null;
            }

            var todoItem = await DbContext.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return null;
            }

            todoItem.Title = todoItemDto.Title;
            todoItem.DeadLine = todoItemDto.DeadLine;
            todoItem.Description = todoItemDto.Description;
            todoItem.Priority = todoItemDto.Priority;
            todoItem.BoardId = todoItemDto.BoardId;



            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return null;
            }

            return ItemToDTO(todoItem);
        }

        private static int PriorityUp(long id, long boardId)
        {
            return 0;
        }

        private static int PriorityDown(long id, long boardId)
        {
            return 0;
        }
        public async Task<BoardDto> UpdateBoardAsync(long id, BoardDto boardDto)
        {
            throw new NotImplementedException();
        }

        public async void UpdateBoardAfterDeleteAsync(long id, int priority)
        {
            
        }

        private bool TodoItemExists(long id) =>
            DbContext.TodoItems.Any(e => e.Id == id);
    }
}
