namespace Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public User(Guid id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }
    }
}
