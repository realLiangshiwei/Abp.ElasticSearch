using System.ComponentModel.DataAnnotations;

namespace BookStore.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}