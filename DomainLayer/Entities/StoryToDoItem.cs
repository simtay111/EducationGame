using System;
using DomainLayer.Constants;
using DomainLayer.Entities.Stories;

namespace DomainLayer.Entities
{
    public class StoryToDoItem
    {
        public int Id { get; set; }
        public ToDoType Type { get; set; } 
        public int ToDoId { get; set; }
        public Member Member { get; set; }
        public Story Story { get; set; }
    }
}