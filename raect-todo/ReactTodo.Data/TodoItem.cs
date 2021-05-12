using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactTodo.Data
{
    public class TodoItem
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        public int Priority { get; set; }

        public long BoardId { get; set; }
        public DateTime DeadLine { get; set; }

        public Board Board { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as TodoItem);
        }

        public bool Equals(TodoItem other)
        {
            return other != null &&
                Id == other.Id &&
                Title == other.Title &&
                Description == other.Description &&
                Priority == other.Priority &&
                BoardId == other.BoardId &&
                DeadLine == other.DeadLine;

        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title,Description, DeadLine, Priority, BoardId, Board);
        }
    }
}
