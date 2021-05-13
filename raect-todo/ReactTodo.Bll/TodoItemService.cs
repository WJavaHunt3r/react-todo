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
    /// Interface for TodoItemServices
    /// </summary>
    public interface ITodoItemService 
    {
        /// <summary>
        /// Get all todoItems
        /// </summary>
        /// <returns>all todoitems in priority order</returns>
        Task<IReadOnlyCollection<TodoItemDto>> GetTodoItemsAsync();

        /// <summary>
        /// Adds the given todoItem to the database
        /// </summary>
        /// <param name="todoItemDto"></param>
        /// <returns>null if failed, else the todoItem</returns>
        Task<TodoItemDto> PostTodoItemAsync(TodoItemDto todoItemDto);

        /// <summary>
        /// Get a todoItem by the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>null if not found else the todoitem</returns>
        Task<TodoItemDto> GetTodoItemAsync(long id);

        /// <summary>
        /// Deletes a todoitem with the given id
        /// </summary>
        /// <param name="id">id of the todoitem we want to delete</param>
        /// <returns>0 if failed, otherwise the id</returns>
        Task<long> DeleteTodoItemAsync(long id);

        /// <summary>
        /// Updates a todoItem with the given id to the given todoitem
        /// </summary>
        /// <param name="id">the id of the todoitem to update</param>
        /// <param name="todoItemDto">the new todoitem</param>
        /// <returns>null if ids not matching or a required property is missing</returns>
        Task<TodoItemDto> UpdateTodoItemAsync(long id, TodoItemDto todoItemDto);

        /// <summary>
        /// Get all boards drom the database
        /// </summary>
        /// <returns>all boards</returns>
        Task<IReadOnlyCollection<BoardDto>> GetBoardsAsync();

        /// <summary>
        /// Gets a board with the provided id
        /// </summary>
        /// <param name="id">the id o the board to get</param>
        /// <returns>null if not found, else the boardDto</returns>
        Task<BoardDto> GetBoardAsync(long id);
    }

    /// <inheritdoc/>
    public record TodoItemService(TodoContext DbContext) : ITodoItemService
    {
        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<BoardDto>> GetBoardsAsync() =>
            await DbContext.Boards.Select(t => new BoardDto(t.Id, t.Name,ItemsToDTO( t.TodoItems))).ToListAsync();

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<TodoItemDto>> GetTodoItemsAsync() =>
            await DbContext.TodoItems.OrderBy(t => t.Priority)
                                        .Select(t => ItemToDTO(t))                      
                                            .ToListAsync();

        /// <inheritdoc/>
        public async Task<TodoItemDto> PostTodoItemAsync(TodoItemDto todoItemDto)
        {
            if (todoItemDto == null || string.IsNullOrEmpty(todoItemDto.Title) || string.IsNullOrEmpty(todoItemDto.Description) || todoItemDto.DeadLine < DateTime.Today)
            {
                return null;
            }
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

        /// <inheritdoc/>
        public async Task<TodoItemDto> GetTodoItemAsync(long id)
        {
            var todoItem = await DbContext.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return null;
            }

            return ItemToDTO(todoItem);
        }

        /// <inheritdoc/>
        public async Task<BoardDto> GetBoardAsync(long id)
        {
            var boards = await GetBoardsAsync();
            var board =  boards.Where(b=>b.Id == id).First();
            return board ?? null;

        }

        /// <inheritdoc/>
        
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
            
            return id;
        }

        /// <summary>
        /// Converts the given todoItem to a TodoItemDto
        /// </summary>
        /// <param name="todoItem">the todoItem to cconvert</param>
        /// <returns>the converted todoitemDto</returns>
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

        /// <summary>
        /// Converts list of todiItems to Dtos
        /// </summary>
        /// <param name="todoItems">the Collection of todoitems to convert</param>
        /// <returns>the new TodoItemDto collection</returns>
        private static ICollection<TodoItemDto> ItemsToDTO(ICollection<TodoItem> todoItems) => todoItems.Select(t => ItemToDTO(t))
                                                                                                                        .OrderBy(t=>t.Priority)
                                                                                                                            .ToList();
        /// <inheritdoc/>
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

        /// <summary>
        /// Changes the priority of all todoitems to +1 with a priority betweeen the <paramref name="Priority"/> and <paramref name="oldPriority"/>
        /// in the board with id of <paramref name="id"/>
        /// </summary>
        /// <param name="id">the id of the board where the priorities must change</param>
        /// <param name="priority">The new priotity of the todoitem that was moved</param>
        /// <param name="oldPriority">The old priotity of the todoitem that was moved</param>
        private async void PriorityUp(long id, long priority, long oldPriority)
        {
            var todos = await DbContext.TodoItems.Where(t => t.BoardId == id && t.Priority >= priority && t.Priority < oldPriority).ToListAsync();
            todos.ForEach(t => t.Priority += 1);
        }

        /// <summary>
        /// Changes the priority of all todoitems to -1 with a priority betweeen the <paramref name="priority"/> and <paramref name="oldPriority"/>
        /// in the board with the id of <paramref name="id"/>
        /// </summary>
        /// <param name="id">the id of the board where the priorities must change</param>
        /// <param name="priority">The new priotity of the todoitem that was moved</param>
        /// <param name="oldPriority">The old priotity of the todoitem that was moved</param>
        private async void PriorityDown(long id, long priority, long oldPriority)
        {
            var todos = await DbContext.TodoItems.Where(t => t.BoardId == id && t.Priority <= priority && t.Priority > oldPriority).ToListAsync();
            todos.ForEach(t => t.Priority -= 1);
        }

        /// <summary>
        /// Changes the priority of all todoitems with a priority bigger than <paramref name="priority"/>
        /// int board with id of <paramref name="id"/> after deleteing one todoitem
        /// </summary>
        /// <param name="id">the id of the board where the priorities must change</param>
        /// <param name="priority">The new priotity of the todoitem that was moved</param>
        public async void UpdateBoardAfterDeleteAsync(long id, int priority)
        {
            var todos = await DbContext.TodoItems.Where(t => t.BoardId == id && t.Priority > priority).ToListAsync();
            todos.ForEach(t => t.Priority -= 1);
               
        }

        /// <summary>
        /// Changes the priority of all todoitems to +with a priority bigger than or equal <paramref name="priority"/>
        /// int board with id of <paramref name="id"/> after a todoitem was inserted
        /// </summary>
        /// <param name="id">the id of the board where the priorities must change</param>
        /// <param name="priority">The new priotity of the todoitem that was moved</param>
        public async void UpdateBoardAfterMove(long id, int priority)
        {
            var todos = await DbContext.TodoItems.Where(t => t.BoardId == id && t.Priority >= priority).ToListAsync();
            todos.ForEach(t => t.Priority += 1);

        }

        /// <summary>
        /// checks if a todoItem with the id <paramref name="id"/> exists
        /// </summary>
        /// <param name="id">The id of the todoitem to check</param>
        /// <returns>true if exists, false otherwise</returns>
        private bool TodoItemExists(long id) =>
            DbContext.TodoItems.Any(e => e.Id == id);
    }
}
