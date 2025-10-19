using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.Requests {
    public class LoginRequest {
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Email is invalid!")]
        public string? AccountEmail { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [MinLength(2, ErrorMessage = "Password must be at least 2 characters!")]
        public string? AccountPassword { get; set; }
    }

    public class CreateAccountRequest {
        [Required(ErrorMessage = "Name is required!")]
        public string? AccountName { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Email is invalid!")]
        public string? AccountEmail { get; set; }

        [Required(ErrorMessage = "Role is required!")]
        public int? AccountRole { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [MinLength(2, ErrorMessage = "Password must be at least 2 characters!")]
        public string? AccountPassword { get; set; }
    }

    public class UpdateAccountRequest {
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        public string? AccountName { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Email is invalid!")]
        public string? AccountEmail { get; set; }

        [Required(ErrorMessage = "Role is required!")]
        public int? AccountRole { get; set; }
    }
}
