using System.ComponentModel.DataAnnotations;

namespace MyLibrary.Models
{
    public class SetBook
    {
        public int Id { get; set; }
        [Display(Name = "שם הסט")]
        public string Name { get; set; }
        public int ShelfId { get; set; }
        public Shelf shelf { get; set; }
        [Display(Name = "גובה")]
        public int Height { get; set; }
        [Display(Name = "רוחב")]
        public int Width { get; set; }

        [Display(Name = "קטגוריה")]
        public string Category { get; set; }
    }
}
