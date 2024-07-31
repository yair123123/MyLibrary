using System.ComponentModel.DataAnnotations;

namespace MyLibrary.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Display (Name= "שם הספר")]
        public string Name { get; set; }
        public int ShelfId { get; set; }
        public Shelf shelf { get; set; }
        [Display(Name = "גובה")]
        public int Height { get; set; }
        [Display(Name = "רוחב")]
        public int Width { get; set; }

        [Display(Name = "קטגוריה")]
        public string Category { get; set; }
        public int? setId { get; set; }
        public SetBook? SetBook { get; set; }
    }
}
