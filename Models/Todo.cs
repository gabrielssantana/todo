using System;

namespace MyTodoBaltaIo.Models
{
    public class Todo
    {
        public int id { get; set; }
        public string Title { get; set; }
        public bool Done { get; set; }
        public DateTime Date { get; set; }
    }
}