using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3_Assignment.Domain
{
    internal class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public BookCondition Condition { get; set; }

        // To initialize the object of book class from outside.
        public Book(int id, string title, string author, BookCondition bookCondition)
        {
            Id = id;
            Title = title;
            Author = author;
            Condition = bookCondition;
        }
    }
}
