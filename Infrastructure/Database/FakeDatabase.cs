using Domain;

namespace Infrastructure.Database
{
    public class FakeDatabase
    {
        public List<Book> Books { get { return allBooksInDB; } set { allBooksInDB = value; } }
        public List<Author> Authors { get { return allAuthorsInDB; } set { allAuthorsInDB = value; } }

        public List<User> Users { get { return allUsersInDB; } set { allUsersInDB = value; } }


        private List<Book> allBooksInDB = new List<Book>
        {
            new Book(2, "Lord of the Rings", "A ring to destroy"),
            new Book(3, "The Bible", "Religious book"),
            new Book(4, "The Hobbit", "Adventure to the lonely mountain")
        };

        private List<Author> allAuthorsInDB = new List<Author>
        {
            new Author(1, "J.R.R. Tolkien", "Adventure"),
            new Author(2, "J.K. Rowling", "Magic"),
            new Author(3, "Camilla Läckberg", "Detective")
        };

        private List<User> allUsersInDB = new List<User>
        {
            new User(new Guid(), "AntonJohansson", "Hej123"),
            new User(new Guid(), "RandomUser", "Hola123"),
            new User(new Guid(), "NewUser00", "Hallå123")

        };

    }
}
