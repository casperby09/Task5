using System.ComponentModel.DataAnnotations;
using System;

namespace Task5.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public DateTime? LastLoginDate { get; set; } = DateTime.Now;
    }
}
