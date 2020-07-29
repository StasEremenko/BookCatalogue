using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Book
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public virtual Author Author { get; set; }

    }   
}
