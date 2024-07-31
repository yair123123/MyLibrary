using System.ComponentModel.DataAnnotations;

namespace MyLibrary.Models
{
    public class Library
    {
        public int Id { get; set; }
        [Display(Name = "קטגוריה")]
        public string Category { get; set; }
        [Display(Name = "רוחב")]
        public int width { get; set; }
        [Display(Name = "מס' מדפים")]
        public int CountShelves { get; set; }
    }
}
