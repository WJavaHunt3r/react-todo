using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactTodo.Data
{
    /// <summary>
    /// Represents a TodoItem in the database
    /// </summary>
    public class TodoItem
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        public int Priority { get; set; }

        public long BoardId { get; set; }
        public DateTime DeadLine { get; set; }

        public Board Board { get; set; }

        /// <summary>
        /// Used for tests
        /// overrides the equals method, than calls it with a TodoItem instance
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as TodoItem);
        }

        /// <summary>
        /// Checks if the this and the given TodoItem are the same
        /// </summary>
        /// <param name="other">The TodoItem to compare with</param>
        /// <returns>true if they are the same instance, false otherwise</returns>
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
