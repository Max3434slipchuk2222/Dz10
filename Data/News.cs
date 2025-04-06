using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAliona.Data
{
    [Table("tblNews")]
    public class News
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; } = String.Empty;

        [Required, StringLength(200)]
        public string Slug { get; set; } = String.Empty;
        [StringLength(300)]
        public string? Summary { get; set; }

        [Required, StringLength(200)]
        public string Image { get; set; } = String.Empty;

        [Required, StringLength(10000)]
        public string Content { get; set; } 
    }
}

