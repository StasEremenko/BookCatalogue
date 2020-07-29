using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Model.BookContext;

namespace BookCatalogue
{
    interface IPrintable
    {
        void Print();
    }
    interface Iprocessable
    {
        void Process();
    }
    class Menu :IPrintable, Iprocessable
    {
        private List<MenuItems> _items = new List<MenuItems>();
        public IEnumerable<MenuItems> Items
        {
            get => _items;
        }

        public Menu(params MenuItems[] items)
        {
            _items.AddRange(items);
        }
        public void Process()
        {
            while (true)
            {
                Print();
                var input = Console.ReadLine();
                if (int.TryParse(input, out int num) && Items.Any(i => i.Num == num))
                {
                    foreach(var item in Items.Where(i => i.Num == num))
                    {
                        item.Process();
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect input! Press enter to continue.");
                    Console.ReadLine();
                }
                Console.Clear();
            }
        }

        public void Print()
        {
            foreach (var item in _items)
                item.Print();
        }
    }
    class MenuItems : IPrintable, Iprocessable
    {
        private Action _processHandler;
        public string Title { get; }
        public int Num { get; }
        public MenuItems(int num, string title, Action processHanlder)
        {
            Title = title;
            Num = num;
            _processHandler = processHanlder;
        }      

        public void Print()
        {
            Console.WriteLine($"{Num}. {Title}");
        }
        public void Process()
        {
            _processHandler?.Invoke();
        }
    }
    class Program
    { 
        static void FindByAuthor()
        {
            using(var context = new BookContext())
            {
                Console.WriteLine("Enter author name, please: ");
                var name = Console.ReadLine(); 
                var books= context.FindBooksByAuthorName(name).ToList();
                if (books.Count > 0)
                {
                    foreach(var book in books)
                    {
                        Console.WriteLine($"{book.BookId}\t{book.Title}\t{book.Author}");
                        Console.ReadLine();
                    }
                }
                else
                    Console.WriteLine("Sorry, nothing found.");
                Console.ReadLine();
            }
        }
        static void FindByTitle()
        {
            using(var context = new BookContext())
            {
                Console.WriteLine("Enter books title, please: ");
                var title = Console.ReadLine();
                var titleBooks = context.FindByTitleBook(title).ToList();
                if (titleBooks.Count > 0)
                {
                    foreach(var t in titleBooks)
                    {
                        Console.WriteLine($"{t.BookId}\t{t.Title}");
                    }
                }
                else
                    Console.WriteLine("Sorry, nothing found.");
                Console.ReadLine();
            }            
        }
        static void AddBook()
        {
            using (var context = new BookContext())
            {
                Console.WriteLine("Enter book title, please: ");
                var title = Console.ReadLine();

                Console.WriteLine("Enter author name: ");
                var authorNames = Console.ReadLine();
                string firstName, midName = string.Empty, lastName;
                var names = authorNames.Split(' ');                
                if (names.Length > 2)
                {
                    firstName = names[0];
                    midName = names[1];
                    lastName = names[2];
                }
                else
                {
                    firstName = names[0];          
                    lastName = names[2];
                }

                var existingAuthor = context.FindAuthorByName(firstName, midName, lastName).FirstOrDefault();
                
                context.Add(new Book()
                {
                    BookId = Guid.NewGuid(),
                    Title = title,
                    Author = existingAuthor ?? new Author()
                    {
                        FirstName = firstName,
                        MiddleName = midName,
                        LastName = lastName
                    }
                });
            }
        }
        static void RemoveBook()
        {
            using (var context = new BookContext())
            {
                Console.WriteLine("Enter Id of books for remove, please: ");
                var input = Console.ReadLine();
                if (Guid.TryParse(input, out Guid bookId))
                {
                   if (context.RemoveById(bookId) != null)
                    {
                        Console.WriteLine("Book removed success!");
                    }
                    else
                    {
                        Console.WriteLine("Can't find any book with this id!");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input!");
                }
                Console.ReadLine();
            }
        }

        static void Main(string[] args)
        {
            var menuItems = new MenuItems[]
            {
                new MenuItems(1, "Find book by author",FindByAuthor),
                new MenuItems(2, "Find book by title", FindByTitle),
                new MenuItems(3, "Add book",AddBook),
                new MenuItems(4, "Remove book",RemoveBook)
            };
            var menu = new Menu(menuItems);
            menu.Process();
            
        }
    }
}