using System.ComponentModel.DataAnnotations;

namespace Presentation.View_Model.Params {
    public class LoginParams {
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Invalid email. Please try again!")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        [MinLength(2, ErrorMessage = "Password must be at least 2 characters long!")]
        public string? Password { get; set; }
    }

    public class SearchAccountParams {
        public string? Keyword { get; set; }
        public string? Role { get; set; }
    }

    public class CreateUpdateAccountParams {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Invalid email. Please try again!")]
        public string? Email { get; set; }

        //[Required(ErrorMessage = "Password is required!")]
        //[DataType(DataType.Password)]
        //[MinLength(2, ErrorMessage = "Password must be at least 2 characters long!")]
        public string? Password { get; set; }

        public string? Role { get; set; }
    }
}