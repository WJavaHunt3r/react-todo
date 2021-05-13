using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactTodo.Data
{
    /// <summary>
    /// represents a Board in the database
    /// </summary>
    public class Board
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<TodoItem> TodoItems { get; set; }

        /// <summary>
        /// Used for tests
        /// overrides the equals method, than calls it with a Board instance
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Board);
        }

        /// <summary>
        /// Checks if the this and the given board are the same
        /// </summary>
        /// <param name="other">The Board to compare with</param>
        /// <returns>true if they are the same instance, false otherwise</returns>
        public bool Equals(Board other)
        {
            return other != null &&
                Id == other.Id &&
                Name == other.Name &&
                Enumerable.SequenceEqual(TodoItems, other.TodoItems);

        }

        /// <summary>
        /// Used for tests
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, TodoItems.Select(s => s.GetHashCode()));
        }
    }
}
