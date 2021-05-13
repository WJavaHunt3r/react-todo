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


        
    }
    public record TodoItemService(TodoContext DbContext) : ITodoItemService
    {
        

        public async Task<IReadOnlyCollection<TodoItemDto>> GetTodoItemsAsync() =>
            await DbContext.TodoItems.Select(t => new TodoItemDto(t.Id,t.Title, t.Description, t.DeadLine,  t.Priority, t.BoardId))                                       
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

        private void PriorityUp(long id, long priority, long oldPriority)
        {
            var todos = DbContext.TodoItems.Where(t => t.BoardId == id && t.Priority >= priority && t.Priority < oldPriority).ToList();
            todos.ForEach(t => t.Priority += 1);
        }

        private void PriorityDown(long id, long priority, long oldPriority)
        {
            var todos = DbContext.TodoItems.Where(t => t.BoardId == id && t.Priority <= priority && t.Priority > oldPriority).ToList();
            todos.ForEach(t => t.Priority -= 1);
        }

        public  void UpdateBoardAfterDeleteAsync(long id, int priority)
        {
            var todos =  DbContext.TodoItems.Where(t => t.BoardId == id && t.Priority > priority).ToList();
            todos.ForEach(t => t.Priority -= 1);
               
        }

        public void UpdateBoardAfterMove(long id, int priority)
        {
            var todos = DbContext.TodoItems.Where(t => t.BoardId == id && t.Priority >= priority).ToList();
            todos.ForEach(t => t.Priority += 1);

        }

        private bool TodoItemExists(long id) =>
            DbContext.TodoItems.Any(e => e.Id == id);
    }
}
