using System.ComponentModel.DataAnnotations;
using System;

namespace Task5.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage ="Имя не указано")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Не указан E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Не указан пороль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }

        public DateTime LastLoginDate { get; set; } = DateTime.Now;
    }
}
