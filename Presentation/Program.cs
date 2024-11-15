using Application;
using Infrastructure.Database;

namespace Presentation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool quit = false;

            FakeDatabase fakeDatabase = new FakeDatabase();
            BookMethods bookMethods = new BookMethods(fakeDatabase);

            while (quit != true)
            {
                Console.WriteLine("What do you want to do? Answer by number");
                Console.WriteLine("1. Add a new book to my collection");
                Console.WriteLine("2. See all your books");
                Console.WriteLine("3. Update a book");
                Console.WriteLine("4. Quit");
                Console.WriteLine();

                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        bookMethods.AddNewBook();
                        Console.WriteLine("The new book is added");
                        break;

                    case "2":
                        Console.WriteLine("Here´s all of your books:");
                        bookMethods.PrintBooks();
                        break;

                    case "3":
                        Console.WriteLine("Choose a book to update by it´s ID:");
                        bookMethods.PrintBooks();
                        int bookIdToUpdate = int.Parse(Console.ReadLine());
                        bookMethods.UpdateBook(bookIdToUpdate);
                        break;

                    case "4":
                        quit = true;
                        break;

                    default:
                        Console.WriteLine("Nu förstod jag inte riktigt?");
                        break;
                }
            }

        }
    }
}
