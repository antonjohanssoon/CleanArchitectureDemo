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
            new Book("Lord of the Rings", "A ring to destroy"),
            new Book("The Bible", "Religious book"),
            new Book("The Hobbit", "Adventure to the lonely mountain")
        };

        private List<Author> allAuthorsInDB = new List<Author>
        {
            new Author("J.R.R. Tolkien", "Adventure"),
            new Author("J.K. Rowling", "Magic"),
            new Author("Camilla Läckberg", "Detective")
        };

        private List<User> allUsersInDB = new List<User>
        {
            new User("AntonJohansson", "Hej123"),
            new User("RandomUser", "Hola123"),
            new User("NewUser00", "Hallå123")

        };

    }
}
