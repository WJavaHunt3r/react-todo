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
        /// Used for test
        /// overrides the Equal method
        /// </summary>
        /// <param name="obj">The object to compare with</param>
        /// <returns>True if the object is the same as this instance of TodoItem</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as TodoItem);
        }

        /// <summary>
        /// Used for test
        /// Checks is the <paramref name="other"/> is the same as this TodoItem 
        /// </summary>
        /// <param name="other">The object to compare with</param>
        /// <returns>True if <paramref name="other"/> is the same as this instance of TodoItem</returns>
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
            return HashCode.Combine(Id, Title, Description, DeadLine, Priority, BoardId, Board);
        }

    }
}
