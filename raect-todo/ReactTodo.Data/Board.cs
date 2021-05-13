using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactTodo.Data
{
    public class Board
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<TodoItem> TodoItems { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Board);
        }
        public bool Equals(Board other)
        {
            return other != null &&
                Id == other.Id &&
                Name == other.Name &&
                Enumerable.SequenceEqual(TodoItems, other.TodoItems);

        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, TodoItems.Select(s => s.GetHashCode()));
        }
    }
}
