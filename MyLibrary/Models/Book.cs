namespace MyLibrary.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ShelfId { get; set; }
        public Shelf shelf { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string Category { get; set; }
        public int? setId { get; set; }
        public SetBook? SetBook { get; set; }
    }
}
