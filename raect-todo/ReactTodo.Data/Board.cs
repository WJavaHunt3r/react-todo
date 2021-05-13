using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactTodo.Data
{
    /// <summary>
    /// Represent a Board in the database
    /// </summary>
    public class Board
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<TodoItem> TodoItems { get; set; }

        /// <summary>
        /// Used for test
        /// overrides the Equal method
        /// </summary>
        /// <param name="obj">The object to compare with</param>
        /// <returns>True if the object is the same as this instance of Board</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Board);
        }




        /// <summary>
        /// Used for test
        /// Checks is the <paramref name="other"/> is the same as this Board 
        /// </summary>
        /// <param name="other">The object to compare with</param>
        /// <returns>True if <paramref name="other"/> is the same as this instance of Board</returns>
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
