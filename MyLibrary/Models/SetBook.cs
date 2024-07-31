using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibrary.Models
{
    [NotMapped]
    public class SetBook
    {
        public int Id { get; set; }

        public string name { get; set; }

        public List<Book> books { get; set; }

    }
}
