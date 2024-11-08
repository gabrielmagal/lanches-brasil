using LanchesBrasil.Commons.Model;
using LanchesBrasil.Commons.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanchesBrasil.Backend.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly static List<User> users = [];
        private readonly IKeycloakService _keycloakService;

        public AccountController(ILogger<AccountController> logger, IKeycloakService keycloakService)
        {
            _logger = logger;
            _keycloakService = keycloakService;
        }

        [Authorize]
        [HttpGet("users")]
        public ResultResponse GetUsers()
        {
            return _keycloakService.GetUsersAsync();
        }
    }
}
