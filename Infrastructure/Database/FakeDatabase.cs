﻿using Domain;

namespace Infrastructure.Database
{
    public class FakeDatabase
    {
        public List<Book> Books { get { return allBooksInDB; } set { allBooksInDB = value; } }

        private List<Book> allBooksInDB = new List<Book>
        {
            new Book(2, "Lord of the Rings", "A ring to destroy"),
            new Book(3, "The Bible", "Religious book"),
            new Book(4, "The Hobbit", "Adventure to the lonely mountain")
        };

        public Book AddNewBookToDB(Book book)
        {
            allBooksInDB.Add(book);
            return book;
        }

        public List<Book> ShowAllBooksInDB()
        {
            return Books.ToList();
        }

        public void UpdateBookInDB(Book updatedBook)
        {
            var bookToUpdate = Books.FirstOrDefault(b => b.Id == updatedBook.Id);
            if (bookToUpdate != null)
            {
                bookToUpdate.Title = updatedBook.Title;
                bookToUpdate.Description = updatedBook.Description;
            }
        }

        public Book DeleteBookInDB(Book bookToDelete)
        {
            allBooksInDB.Remove(bookToDelete);
            return bookToDelete;
        }
    }
}
