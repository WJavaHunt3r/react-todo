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
    }
    public record TodoItemService(TodoContext DbContext) : ITodoItemService
    {
        public async Task<IReadOnlyCollection<BoardDto>> GetBoardsAsync() =>
            await DbContext.Boards.Select(t => new BoardDto(t.Id, t.Name,ItemsToDTO( t.TodoItems))).ToListAsync();

        public async Task<IReadOnlyCollection<TodoItemDto>> GetTodoItemsAsync() =>
            await DbContext.TodoItems.OrderBy(t => t.Priority)
                                        .Select(t => new TodoItemDto(t.Id,t.Title, t.Description, t.DeadLine,  t.Priority, t.BoardId))                      
                                            .ToListAsync();

        public async Task<TodoItemDto> PostTodoItemAsync(TodoItemDto todoItemDto)
        {
            //var tasks = await GetTodoItemsAsync();
            var todoItem = new TodoItem
            {
                Title = todoItemDto.Title,
                DeadLine = todoItemDto.DeadLine,
                Description = todoItemDto.Description,
                BoardId = 1,
                Priority = DbContext.TodoItems.Where(t=>t.BoardId==1).Count() 
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
            var boards = await GetBoardsAsync();
            return boards.Where(b=>b.Id == id).First();

        }

        public async Task<long> DeleteTodoItemAsync(long id)
        {
            var todoItem = await DbContext.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return 0;
            }

            DbContext.TodoItems.Remove(todoItem);
            UpdateBoardAfterDeleteAsync(todoItem.BoardId, todoItem.Priority);
            await DbContext.SaveChangesAsync();
            
            return 1;
        }

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
            )).OrderBy(t=>t.Priority).ToList();

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
            var oldPriority = todoItem.Priority;
            var oldBoardId = todoItem.BoardId;
            if (oldBoardId != todoItemDto.BoardId)
            {
                UpdateBoardAfterMove(todoItemDto.BoardId, todoItemDto.Priority);
                UpdateBoardAfterDeleteAsync(oldBoardId, oldPriority);
            }
            else if (oldPriority > todoItemDto.Priority)
            {
                PriorityUp(todoItemDto.BoardId, todoItemDto.Priority, oldPriority);
            }
            else if (oldPriority < todoItemDto.Priority)
            {
                PriorityDown(todoItemDto.BoardId, todoItemDto.Priority, oldPriority);
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

        private async void PriorityUp(long id, long priority, long oldPriority)
        {
            var todos = await DbContext.TodoItems.Where(t => t.BoardId == id && t.Priority >= priority && t.Priority < oldPriority).ToListAsync();
            todos.ForEach(t => t.Priority += 1);
        }

        private async void PriorityDown(long id, long priority, long oldPriority)
        {
            var todos = await DbContext.TodoItems.Where(t => t.BoardId == id && t.Priority <= priority && t.Priority > oldPriority).ToListAsync();
            todos.ForEach(t => t.Priority -= 1);
        }

        public async void UpdateBoardAfterDeleteAsync(long id, int priority)
        {
            var todos = await DbContext.TodoItems.Where(t => t.BoardId == id && t.Priority > priority).ToListAsync();
            todos.ForEach(t => t.Priority -= 1);
               
        }

        public async void UpdateBoardAfterMove(long id, int priority)
        {
            var todos = await DbContext.TodoItems.Where(t => t.BoardId == id && t.Priority >= priority).ToListAsync();
            todos.ForEach(t => t.Priority += 1);

        }

        private bool TodoItemExists(long id) =>
            DbContext.TodoItems.Any(e => e.Id == id);
    }
}
