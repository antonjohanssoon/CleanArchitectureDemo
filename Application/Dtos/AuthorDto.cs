using System.ComponentModel.DataAnnotations;
namespace Application.Dtos
{
    public class AuthorDto
    {
        [Required(ErrorMessage = "Author name is required.")]
        [StringLength(100, ErrorMessage = "Author name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Book category is required.")]
        [StringLength(50, ErrorMessage = "Book category cannot exceed 50 characters.")]
        public string BookCategory { get; set; } = string.Empty;
    }

}
