using System.ComponentModel.DataAnnotations;

namespace MyLibrary.Models
{
    public class Shelf
    {
        public int Id { get; set; }
        [Display(Name = "קטגוריה")]
        public string Category { get; set; }
        public int LibraryId { get; set; }
        public Library Library { get; set; }
        [Display(Name = "גובה")]
        public int Height { get; set; }
        [Display(Name = "רוחב")]
        public int Width { get; set; }
        [Display(Name = "מקום שנשאר")]
        public int rest { get; set; }
        [Display( Name = "מס' ספרים במדף")]
        public int CountBooks {  get; set; }
    }
}
