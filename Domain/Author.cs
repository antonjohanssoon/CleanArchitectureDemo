namespace Domain
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string BookCategory { get; set; }

        public Author(string name, string bookCategory)
        {
            Id = Guid.NewGuid();
            Name = name;
            BookCategory = bookCategory;
        }

        public Author()
        {

        }
    }
}
