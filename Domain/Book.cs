namespace Domain
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public Book(string title, string description)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
        }

        public Book()
        {

        }
    }
}
