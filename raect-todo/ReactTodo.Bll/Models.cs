

using System;
using System.Collections.Generic;

/// <summary>
/// DTo models for TodoItem and Board
/// </summary>
namespace ReactTodo.Bll.Models
{
    public sealed record TodoItemDto(long Id, string Title, string Description, DateTime DeadLine, int Priority, long BoardId);
    public sealed record BoardDto(long Id, string Name, ICollection<TodoItemDto> TodoItems);
}
