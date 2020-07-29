using System.Security.Cryptography.X509Certificates;

namespace Model
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }


        public override string ToString()
        {
            return $"{FirstName}\t{LastName}";
        }
    }
}