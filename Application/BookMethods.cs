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
            try
            {
                Console.WriteLine("Give your book an ID:");
                if (!int.TryParse(Console.ReadLine(), out int idInput))
                {
                    throw new ArgumentException("Invalid ID format. Please enter a valid number.");
                }

                Console.WriteLine("What’s the title of the book?");
                string titleInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(titleInput))
                {
                    throw new ArgumentException("Title cannot be empty.");
                }

                Console.WriteLine("Add a description about your book:");
                string descriptionInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(descriptionInput))
                {
                    throw new ArgumentException("Description cannot be empty.");
                }

                Book newBookToAdd = new Book(idInput, titleInput, descriptionInput);
                return fakeDatabase.AddNewBookToDB(newBookToAdd);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Input error: " + ex.Message);
                return null;
            }
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

        public Book DeleteBook(int bookId)
        {
            try
            {
                Book bookToDelete = fakeDatabase.Books.FirstOrDefault(b => b.Id == bookId);

                if (bookToDelete == null)
                {
                    Console.WriteLine("Book not found");
                }

                return fakeDatabase.DeleteBookInDB(bookToDelete);
            }

            catch (Exception)
            {
                Console.WriteLine("An error occurred when deleting the book");
                return null;
            }
        }

    }
}
