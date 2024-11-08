using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LanchesBrasil.Pages.Account.CadastrarUsuario;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    [BindProperty]
    public required string Usuario { get; set; }

    [BindProperty]
    public required string Senha { get; set; }

    [BindProperty]
    public required string Email { get; set; }

    [BindProperty]
    public required string Endereco { get; set; }

    [BindProperty]
    public required string Cep { get; set; }

    [BindProperty]
    public required DateOnly DataNascimento { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnPost()
    {
        Console.WriteLine("Teste");
    }
}
