using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using APIdemo.Models;

public class ConfirmEmailModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ConfirmEmailModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [BindProperty]
    public string Message { get; set; }

    public async Task<IActionResult> OnGetAsync(string email, string code)
    {
        if (email == null || code == null)
        {
            Message = "Invalid email confirmation request.";
            return Page();
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            Message = "Unable to load user.";
            return Page();
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);
        Message = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
        return Page();
    }
}
