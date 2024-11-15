using Domain;
using Infrastructure.Database;

namespace Application
{
    public class BookMethods
    {
        private readonly FakeDatabase fakeDatabase;

        public BookMethods(FakeDatabase fakeDatabase)
        {
            this.fakeDatabase = fakeDatabase;
        }

        public Book AddNewBook()
        {
            Console.WriteLine("Give your book an ID:");
            int idInput = int.Parse(Console.ReadLine());

            Console.WriteLine("What’s the title of the book?");
            string titleInput = Console.ReadLine();

            Console.WriteLine("Add a description about your book:");
            string descriptionInput = Console.ReadLine();

            Book newBookToAdd = new Book(idInput, titleInput, descriptionInput);
            return fakeDatabase.AddNewBookToDB(newBookToAdd);
        }

        public List<Book> GetBooks()
        {
            return fakeDatabase.ShowAllBooksInDB();
        }

        public void PrintBooks()
        {
            var books = GetBooks();
            foreach (var book in books)
            {
                Console.WriteLine($"Id: {book.Id} Title: {book.Title}");
            }
        }

        public Book UpdateBook(int bookId)
        {
            try
            {
                Book bookToUpdate = fakeDatabase.Books.FirstOrDefault(b => b.Id == bookId);

                if (bookToUpdate == null)
                {
                    Console.WriteLine("Book not found!");
                    return null;
                }

                Console.WriteLine("Update book title (current: " + bookToUpdate.Title + "): ");
                string newTitle = Console.ReadLine();
                if (!string.IsNullOrEmpty(newTitle))
                {
                    bookToUpdate.Title = newTitle;
                }

                Console.WriteLine("Update book description (current: " + bookToUpdate.Description + "): ");
                string newDescription = Console.ReadLine();
                if (!string.IsNullOrEmpty(newDescription))
                {
                    bookToUpdate.Description = newDescription;
                }

                fakeDatabase.UpdateBookInDB(bookToUpdate);
                return bookToUpdate;
            }
            catch (Exception)
            {
                Console.WriteLine("An error occurred when updating the book.");
                return null;
            }
        }

    }
}
