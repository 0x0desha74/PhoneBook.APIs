﻿using System.ComponentModel.DataAnnotations;

namespace PhoneBook.APIs.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$",
      ErrorMessage = "Password must have 1 Uppercase , 1 Lowercase , 1 Number , 1 non-alphanumeric and at least 6 characters")]
        public string Password { get; set; }
    }
}
