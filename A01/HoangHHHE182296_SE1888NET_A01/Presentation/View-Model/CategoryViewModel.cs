using System.ComponentModel.DataAnnotations;

namespace Presentation.View_Model {
    public class CategoryViewModel {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category description is required")]
        public string? Description { get; set; }

        public int? ParentId { get; set; }

        public string? ParentName { get; set; }

        public bool? IsActive { get; set; }

        public int? QuantityArticles { get; set; }
    }
}
