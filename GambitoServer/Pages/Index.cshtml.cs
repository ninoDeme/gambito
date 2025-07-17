using GambitoServer.Db;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GambitoServer.Pages;

public class IndexModel : PageModel
{
  private readonly ILogger<IndexModel> _logger;

  private readonly SignInManager<User> _signInManager;

  public IndexModel(ILogger<IndexModel> logger, SignInManager<User> signInManager)
  {
    _logger = logger;
    _signInManager = signInManager;
  }

  public void OnGet() {
    Console.WriteLine("LOOOOOOOOOOOOOOOOOOOOOOOOOOO");
    Console.WriteLine(_signInManager.IsSignedIn(User));
  }
}
