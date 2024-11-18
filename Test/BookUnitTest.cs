using Application;
using Domain;
using Infrastructure.Database;

namespace Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]

        public void When_Method_AddNewBookIsCalledThenBookIsAddedToDB()
        {
            //Arrange
            var fakeDatabase = new FakeDatabase();
            var bookMethod = new BookMethods(fakeDatabase);

            string simulatedInput = "5\nNew Test Book\nDescription of the test book\n";
            Console.SetIn(new StringReader(simulatedInput));

            //Act
            Book addedBook = bookMethod.AddNewBook();

            //Assert
            Assert.Contains(addedBook, fakeDatabase.ShowAllBooksInDB());
            Assert.AreEqual(5, addedBook.Id);
            Assert.AreEqual("New Test Book", addedBook.Title);
            Assert.AreEqual("Description of the test book", addedBook.Description);
        }



        [Test]
        public void When_Method_GetBooksIsCalledAllBooksInDBIsShown()
        {
            //Arrange
            var fakeDatabase = new FakeDatabase();
            var bookMethod = new BookMethods(fakeDatabase);

            //Act
            var books = bookMethod.GetBooks();

            //Assert
            Assert.AreEqual(fakeDatabase.ShowAllBooksInDB().Count, books.Count);
            Assert.True(books.SequenceEqual(fakeDatabase.ShowAllBooksInDB()));
            Assert.IsNotNull(books);
        }

        [Test]
        public void When_Method_UpdateBookIsCalledThenBookIsUpdatedWithNewTitleAndDescription()
        {
            //Arrange
            var fakeDatabase = new FakeDatabase();
            var bookMethod = new BookMethods(fakeDatabase);
            var book = new Book(1, "Old Title", "Old description");
            fakeDatabase.AddNewBookToDB(book);

            string simulatedInput = "New Title\nNew Description\n";
            Console.SetIn(new StringReader(simulatedInput));

            //Act
            Book updatedBook = bookMethod.UpdateBook(1);

            //Assert
            Assert.IsNotNull(updatedBook);
            Assert.AreEqual("New Title", updatedBook.Title);
            Assert.AreEqual("New Description", updatedBook.Description);

        }

        [Test]
        public void When_Method_DeleteBookIsCalledThenBookIsDeletedFromDB()
        {
            //Arrange
            var fakeDatabase = new FakeDatabase();
            var bookMethod = new BookMethods(fakeDatabase);
            var book1 = new Book(1, "Old Book", "Old description");
            fakeDatabase.AddNewBookToDB(book1);

            //Act
            Book bookToDelete = bookMethod.DeleteBook(1);

            //Assert
            Assert.IsFalse(fakeDatabase.Books.Contains(book1));
            Assert.AreEqual(book1, bookToDelete);
            Assert.AreEqual(3, fakeDatabase.Books.Count);

        }
    }
}