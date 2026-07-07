using OnlineLibrary.Domain.Enums;


namespace OnlineLibrary.Domain.Entitites
{
    public class Author: BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Surname { get; set; }
        public Gender Gender { get; set; }

        public List<Book> Books { get; set; } = new();
    }
}
