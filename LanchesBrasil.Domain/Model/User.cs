using System.ComponentModel.DataAnnotations;

namespace LanchesBrasil.Commons.Model
{
    public class User
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = "O campo Usuário é obrigatório.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "O campo Password é obrigatório.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo Email deve ser um endereço de e-mail válido.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O campo Address é obrigatório.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "O campo PostalCode é obrigatório.")]
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "O campo DateOfBirth é obrigatório.")]
        public DateOnly DateOfBirth { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}
