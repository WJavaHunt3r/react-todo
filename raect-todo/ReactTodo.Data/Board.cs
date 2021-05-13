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
    }
}
