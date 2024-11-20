namespace Domain
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BookCategory { get; set; }

        public Author(int id, string name, string bookCategory)
        {
            Id = id;
            Name = name;
            BookCategory = bookCategory;
        }
    }
}
