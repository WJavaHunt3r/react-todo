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
    /// Interface for TodoItemService
    /// </summary>
    public interface ITodoItemService 
    {
        /// <summary>
        /// Gets all TodoItems
        /// 
        /// </summary>
        /// <returns>All TodoItems in the database</returns>
        Task<IReadOnlyCollection<TodoItemDto>> GetTodoItemsAsync();

        /// <summary>
        /// Adds the given todoItem to the database
        /// </summary>
        /// <param name="todoItemDto">The TodoItem to be added</param>
        /// <returns>null if failed, The TodoItemDto otherwise</returns>
        Task<TodoItemDto> PostTodoItemAsync(TodoItemDto todoItemDto);

        /// <summary>
        /// Gets the TodoItem which has the given id
        /// </summary>
        /// <param name="id">The id of the todoItem to be returned</param>
        /// <returns>null if not found, the TodoItem otherwise</returns>
        Task<TodoItemDto> GetTodoItemAsync(long id);

        /// <summary>
        /// Deletes the TodoItem with the given id from the Database
        /// </summary>
        /// <param name="id">The Id of the TodoItem to be deleted</param>
        /// <returns>0 if failed, the id of the deleted TodoItem otherwise</returns>
        Task<long> DeleteTodoItemAsync(long id);

        /// <summary>
        /// Updates a todoItem to the given todoItemDto
        /// </summary>
        /// <param name="id">the id of the TodoItem to be updated</param>
        /// <param name="todoItemDto">The TodoItem to be updated to</param>
        /// <returns></returns>
        Task<TodoItemDto> UpdateTodoItemAsync(long id, TodoItemDto todoItemDto);


        
    }

    ///<inheritdoc/>
    public record TodoItemService(TodoContext DbContext) : ITodoItemService
    {
        ///<inheritdoc/>
        public async Task<IReadOnlyCollection<TodoItemDto>> GetTodoItemsAsync() =>
            await DbContext.TodoItems.Select(t => new TodoItemDto(t.Id,t.Title, t.Description, t.DeadLine,  t.Priority, t.BoardId))                                       
                                            .ToListAsync();
        
        ///<inheritdoc/>
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
        
        ///<inheritdoc/>
        public async Task<TodoItemDto> GetTodoItemAsync(long id)
        {
            var todoItem = await DbContext.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return null;
            }

            return ItemToDTO(todoItem);
        }

        ///<inheritdoc/>
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

        /// <summary>
        /// Convert the given todoItem to a todoItemDto
        /// </summary>
        /// <param name="todoItem">The todoItem to be converted</param>
        /// <returns>The new TodoItemDto</returns>
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

        ///<inheritdoc/>
        public async Task<TodoItemDto> UpdateTodoItemAsync(long id, TodoItemDto todoItemDto)
        {
            if (id != todoItemDto.Id || string.IsNullOrEmpty(todoItemDto.Title) || string.IsNullOrEmpty(todoItemDto.Description) || DateTime.Today > todoItemDto.DeadLine)
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
        /// Must be called when a TodoItems priority was moved up in a board
        /// (Not between boards!!!)
        /// 
        /// Adds 1 to all TodoItems priority which are more than or equal <paramref name="priority"/> and less than <paramref name="oldPriority"/>
        /// </summary>
        /// <param name="boardId">The id of board where the todoItem is</param>
        /// <param name="priority">The new priority of the TodoItem</param>
        /// <param name="oldPriority">The old priority of the TodoItem</param>
        private void PriorityUp(long boardId, long priority, long oldPriority)
        {
            var todos = DbContext.TodoItems.Where(t => t.BoardId == boardId && t.Priority >= priority && t.Priority < oldPriority).ToList();
            todos.ForEach(t => t.Priority += 1);
        }

        /// <summary>
        /// Must be called when a TodoItems priority was moved down in a board
        /// (Not between boards!!!)
        /// 
        /// Substracts 1 from all TodoItems priority which are less than or equal <paramref name="priority"/> and bigger than <paramref name="oldPriority"/>
        /// </summary>
        /// <param name="boardId">The id of board where the todoItem i</param>
        /// <param name="priority">The new priority of the TodoItem</param>
        /// <param name="oldPriority">The old priority of the TodoItem</param>
        private void PriorityDown(long boardId, long priority, long oldPriority)
        {
            var todos = DbContext.TodoItems.Where(t => t.BoardId == boardId && t.Priority <= priority && t.Priority > oldPriority).ToList();
            todos.ForEach(t => t.Priority -= 1);
        }

        /// <summary>
        /// Updates a board when a  todoitem was deleted from the board
        /// Substracts 1 from all ToodItems that are Bigger than <paramref name="priority"/>
        /// </summary>
        /// <param name="id">The id of the board where the TodoItem was deleted from</param>
        /// <param name="priority">The priority of the deleted TodoItem</param>
        public  void UpdateBoardAfterDeleteAsync(long id, int priority)
        {
            var todos =  DbContext.TodoItems.Where(t => t.BoardId == id && t.Priority > priority).ToList();
            todos.ForEach(t => t.Priority -= 1);
               
        }

        /// <summary>
        /// Updates a board when a  todoitem was inserted from an other board
        /// Addds 1 to all ToodItems that are less than <paramref name="priority"/>
        /// </summary>
        /// <param name="id">The id of the board where the TodoItem was inserted to</param>
        /// <param name="priority">The priority of the inserted TodoItem</param>
        public void UpdateBoardAfterMove(long id, int priority)
        {
            var todos = DbContext.TodoItems.Where(t => t.BoardId == id && t.Priority >= priority).ToList();
            todos.ForEach(t => t.Priority += 1);

        }

        /// <summary>
        /// Checks if the given id belongs to an existing TodoItem
        /// </summary>
        /// <param name="id">The id of the TodoItem to be checked</param>
        /// <returns>true if the TodoItem exists, false otherwise</returns>
        private bool TodoItemExists(long id) =>
            DbContext.TodoItems.Any(e => e.Id == id);
    }
}
