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

    }
}
