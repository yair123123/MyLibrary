namespace MyLibrary.Models
{
    public class Shelf
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public int LibraryId { get; set; }
        public Library Library { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}
