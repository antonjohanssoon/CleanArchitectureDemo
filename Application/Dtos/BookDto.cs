using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class BookDto
    {
        [Required(ErrorMessage = "Book title is required.")]
        [StringLength(150, ErrorMessage = "Book title cannot exceed 150 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Book description is required.")]
        [StringLength(500, ErrorMessage = "Book description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;
    }
}

