using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BookContext : DbContext
    {     
        
        public BookContext()
            : base("BookData")
        {            
            //Database.Log = logInfo => Console.WriteLine(logInfo);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().Property(book => book.Title).IsRequired();
            modelBuilder.Entity<Book>().HasIndex(book => book.Title);
            modelBuilder.Entity<Book>().Property(book => book.Title).HasMaxLength(200);
            modelBuilder.Entity<Book>().HasRequired(book => book.Author);


            modelBuilder.Entity<Author>().Property(a => a.FirstName).IsRequired();
            modelBuilder.Entity<Author>().Property(a => a.LastName).IsRequired();
            modelBuilder.Entity<Author>().HasIndex(book => book.FirstName);
            modelBuilder.Entity<Author>().HasIndex(book => book.LastName);
            modelBuilder.Entity<Author>().Property(book => book.FirstName).HasMaxLength(100);
            modelBuilder.Entity<Author>().Property(book => book.MiddleName).HasMaxLength(100);
            modelBuilder.Entity<Author>().Property(book => book.LastName).HasMaxLength(100);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        public IEnumerable<Book> FindByAuthor(Author author)
            => Books.Where(b => b.Author == author).ToArray();
        public IEnumerable<Book> FindByTitle(string title)
            => Books.Where(b => b.Title == title).ToArray();
        public IEnumerable<Book> FindByTitleBook(string title)
            => Books.Where(b => b.Title.Contains( title));
        public IEnumerable<Book> FindBooksByAuthorName(string name) => Books.Where(b => b.Author.FirstName.Contains(name) || b.Author.LastName.Contains(name));
        
        public IEnumerable<Author> FindAuthorByName(string firstName, string middleName, string lstName)
            => Authors.Where(a => a.FirstName == firstName && a.MiddleName == middleName && a.LastName == lstName);
        public void Add(Book book) => Books.Add(book);
        public Book RemoveById(Guid id)
        {
            var book = Books.FirstOrDefault(a => a.BookId == id);
            Books.Remove(book);
            return (book);
        }
        public void Remove(Book book) => Books.Remove(book);
    }
}